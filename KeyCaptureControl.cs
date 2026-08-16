using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using ShimizuToolkit.HotkeyWinUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System;
using Windows.UI.Core;
using ShimizuToolkit.HotkeyWinUI.Controls.Win32;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

#pragma warning disable IDE0305

namespace ShimizuToolkit.HotkeyWinUI.Controls
{
    public sealed partial class KeyCaptureControl : Control
    {
        private KeyBlockPanel? _keyBlockPanel;

        public static readonly DependencyProperty CapturedKeysProperty =
            DependencyProperty.Register(
                nameof(CapturedKeys),
                typeof(ObservableCollection<VirtualKey>),
                typeof(KeyCaptureControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsCapturingProperty =
            DependencyProperty.Register(
                nameof(IsCapturing),
                typeof(bool),
                typeof(KeyCaptureControl),
                new PropertyMetadata(false, OnIsCapturingChanged));

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(
                nameof(PlaceholderText),
                typeof(string),
                typeof(KeyCaptureControl),
                new PropertyMetadata("Click to set hotkey"));

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(
                nameof(IsReadOnly),
                typeof(bool),
                typeof(KeyCaptureControl),
                new PropertyMetadata(false));

        public event EventHandler<KeysChangedEventArgs>? KeysChanged;
        public event EventHandler<KeysCapturedEventArgs>? KeysCaptured;

        private List<VirtualKey> _currentKeys = [];
        private readonly HashSet<Windows.System.VirtualKey> _pressedKeys = [];
        private bool _isProcessingKey = false;

        public bool IgnoreModifierLR
        {
            get => (bool)GetValue(IgnoreModifierLRProperty);
            set => SetValue(IgnoreModifierLRProperty, value);
        }

        public static readonly DependencyProperty IgnoreModifierLRProperty =
            DependencyProperty.Register(nameof(IgnoreModifierLR), typeof(bool), typeof(KeyCaptureControl), new PropertyMetadata(false));

        private KeyboardHook? _hook;
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_hook == null)
                {
                    _hook = new();
                }
                else
                {
                    return;
                }
                _hook.KeyDown += OnHookKeyDown;
                _hook.KeyUp += OnHookKeyUp;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Keyboard hook failed: {ex.Message}");
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
            {
                DisposeResources();
            }
        }

        private void DisposeResources()
        {
            if (_hook != null)
            {
                _hook.KeyDown -= OnHookKeyDown;
                _hook.KeyUp -= OnHookKeyUp;
                _hook.Dispose();
                _hook = null;
            }
        }

        public void ApplyExternalHook(KeyboardHook keyboardHook)
        {
            if (_hook == keyboardHook)
            {
                return;
            }
            if (_hook != null)
            {
                _hook.KeyDown -= OnHookKeyDown;
                _hook.KeyUp -= OnHookKeyUp;
                _hook.Dispose();
                _hook = null;
            }
            _hook = keyboardHook;
            _hook.KeyDown += OnHookKeyDown;
            _hook.KeyUp += OnHookKeyUp;
        }

        private void OnHookKeyDown(object? sender, KeyboardHookEventArgs e)
        {
            if (!IsCapturing || IsReadOnly || _isProcessingKey)
                return;

            _isProcessingKey = true;
            try
            {
                if (_pressedKeys.Count == 0)
                {
                    _currentKeys.Clear();
                }
                var key = e.VirtualKey;

                // If ignore modifier LR, set left modifier as default.
                if (IgnoreModifierLR)
                {
                    key = NormalizeModifierKey(key);
                }

                _pressedKeys.Add(e.VirtualKey); // Use physical keys to judge release

                // Check if already exists
                if (!_currentKeys.Contains(key))
                {
                    // Only retain one action key
                    if (!IsModifierKey(key))
                    {
                        _currentKeys.RemoveAll(k => !IsModifierKey(k));
                    }
                    _currentKeys.Add(key);
                    UpdateUI();
                }
            }
            finally
            {
                _isProcessingKey = false;
            }
        }
        private VirtualKey NormalizeModifierKey(VirtualKey key)
        {
            return key switch
            {
                VirtualKey.LeftShift or VirtualKey.RightShift => VirtualKey.LeftShift,
                VirtualKey.LeftControl or VirtualKey.RightControl => VirtualKey.LeftControl,
                VirtualKey.LeftMenu or VirtualKey.RightMenu => VirtualKey.LeftMenu,
                VirtualKey.LeftWindows or VirtualKey.RightWindows => VirtualKey.LeftWindows,
                _ => key
            };
        }

        private void OnHookKeyUp(object? sender, KeyboardHookEventArgs e)
        {
            if (!IsCapturing || IsReadOnly)
                return;

            var key = e.VirtualKey;
            _pressedKeys.Remove(key); // Remove physical key

            // Finish capture when all physical keys released
            if (AutoStopCapturing && _pressedKeys.Count == 0 && _currentKeys.Count > 0)
            {
                CompleteCapture();
            }
        }

        public ObservableCollection<VirtualKey> CapturedKeys
        {
            get => (ObservableCollection<VirtualKey>)GetValue(CapturedKeysProperty);
            set => SetValue(CapturedKeysProperty, value);
        }

        public bool IsCapturing
        {
            get => (bool)GetValue(IsCapturingProperty);
            set => SetValue(IsCapturingProperty, value);
        }

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public bool AutoStopCapturing = false;

        public KeyCaptureControl()
        {
            this.DefaultStyleKey = typeof(KeyCaptureControl);
            this.CapturedKeys = [];
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _keyBlockPanel = GetTemplateChild("PART_KeyBlockPanel") as KeyBlockPanel;
            _keyBlockPanel?.VirtualKeys = _currentKeys.Select(k => (uint)k).ToList();
            _keyBlockPanel?.UpdateUI();
            UpdateUI();
        }

        private static void OnIsCapturingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (KeyCaptureControl)d;
            if ((bool)e.NewValue)
                control.StartCapture();
            else
                control.StopCapture();
        }

        private void StartCapture()
        {
            if (IsReadOnly)
                return;

            //_currentKeys.Clear();
            _pressedKeys.Clear();
            UpdateUI();

            this.Focus(FocusState.Programmatic);
        }

        private void StopCapture()
        {
            _isProcessingKey = false;
            _pressedKeys.Clear();
        }

        public void CompleteCapture()
        {
            _pressedKeys.Clear();

            if (_currentKeys.Count > 0)
            {
                // Copy captured keys to CapturedKeys
                CapturedKeys.Clear();
                foreach (var key in _currentKeys)
                    CapturedKeys.Add(key);

                OnKeysChanged(new KeysChangedEventArgs(CapturedKeys.ToList()));
                OnKeysCaptured(new KeysCapturedEventArgs(CapturedKeys.ToList()));
            }

            IsCapturing = false;
            UpdateUI();
        }

        private static bool IsModifierKey(VirtualKey key)
        {
            return key == VirtualKey.LeftShift || key == VirtualKey.RightShift || key == VirtualKey.Shift ||
                   key == VirtualKey.LeftControl || key == VirtualKey.RightControl || key == VirtualKey.Control ||
                   key == VirtualKey.LeftMenu || key == VirtualKey.RightMenu || key == VirtualKey.Menu ||
                   key == VirtualKey.LeftWindows || key == VirtualKey.RightWindows;
        }

        private void UpdateUI()
        {
            _currentKeys = SortKeys(_currentKeys);
            var uintKeys = _currentKeys.Select(k => (uint)k).ToList();
            _keyBlockPanel?.VirtualKeys = uintKeys;
        }

        private List<VirtualKey> SortKeys(IEnumerable<VirtualKey> keys)
        {
            List<VirtualKey> result = [];
            List<VirtualKey> order =
            [
                VirtualKey.LeftWindows,
                VirtualKey.RightWindows,
                VirtualKey.Control,
                VirtualKey.LeftControl,
                VirtualKey.RightControl,
                VirtualKey.Menu,
                VirtualKey.LeftMenu,
                VirtualKey.RightMenu,
                VirtualKey.Shift,
                VirtualKey.LeftShift,
                VirtualKey.RightShift,
            ];

            foreach (var modifier in order)
            {
                if (keys.Contains(modifier))
                {
                    result.Add(modifier);
                }
            }

            foreach (var key in keys)
            {
                if (!IsModifierKey(key))
                {
                    result.Add(key);
                }
            }

            return result;
        }

        private void OnKeysChanged(KeysChangedEventArgs e) => KeysChanged?.Invoke(this, e);
        private void OnKeysCaptured(KeysCapturedEventArgs e) => KeysCaptured?.Invoke(this, e);

        public void SetKeys(List<VirtualKey> keys)
        {
            if (keys == null)
                return;
            CapturedKeys.Clear();
            _currentKeys.Clear();
            _keyBlockPanel?.VirtualKeys = [];
            foreach (var key in keys)
            {
                CapturedKeys.Add(key);
                _currentKeys.Add(key);
                _keyBlockPanel?.VirtualKeys = (IEnumerable<uint>)_currentKeys;
            }
            _keyBlockPanel?.UpdateUI();
            UpdateUI();
            OnKeysChanged(new KeysChangedEventArgs(CapturedKeys.ToList()));
        }

        public void ClearKeys()
        {
            CapturedKeys.Clear();
            _currentKeys.Clear();
            UpdateUI();
            OnKeysChanged(new KeysChangedEventArgs([]));
        }

        public List<VirtualKey> GetCapturedKeys() => [.. CapturedKeys];
    }

    public class KeysChangedEventArgs(List<VirtualKey> keys) : EventArgs
    {
        public List<VirtualKey> Keys { get; } = keys;
    }

    public class KeysCapturedEventArgs(List<VirtualKey> keys) : EventArgs
    {
        public List<VirtualKey> Keys { get; } = keys;
    }
}
