using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using ShimizuToolkit.HotkeyWinUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShimizuToolkit.HotkeyWinUI.Controls
{
    public sealed partial class KeyBlock : Control, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        private Grid? _keyNamePanel;
        private TextBlock? _specialSourceTextBlock;
        private FontIcon? _leftArrowFontIcon;
        private FontIcon? _upArrowFontIcon;
        private FontIcon? _rightArrowFontIcon;
        private FontIcon? _downArrowFontIcon;

        public VirtualKeyInfo? VirtualKeyInfo
        {
            get => _virtualKeyInfo;
            set
            {
                if (_virtualKeyInfo != value)
                {
                    _virtualKeyInfo = value;
                    OnPropertyChanged();
                    UpdateUI();
                }
            }
        }
        private VirtualKeyInfo? _virtualKeyInfo;

        public uint KeyCode
        {
            get => _keyCode;
            set
            {
                if (_keyCode != value)
                {
                    _keyCode = value;
                    OnPropertyChanged();
                    UpdateUI();
                }
            }
        }
        private uint _keyCode;


        public KeyBlock()
        {
            DefaultStyleKey = typeof(KeyBlock);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _keyNamePanel = GetTemplateChild("PART_KeyNamePanel") as Grid;
            _specialSourceTextBlock = GetTemplateChild("PART_SpecialSourceTextBlock") as TextBlock;
            _leftArrowFontIcon = GetTemplateChild("PART_LeftArrow") as FontIcon;
            _upArrowFontIcon = GetTemplateChild("PART_UpArrow") as FontIcon;
            _rightArrowFontIcon = GetTemplateChild("PART_RightArrow") as FontIcon;
            _downArrowFontIcon = GetTemplateChild("PART_DownArrow") as FontIcon;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_keyNamePanel == null || _specialSourceTextBlock == null)
                return;
            _keyNamePanel.Children.Clear();
            _specialSourceTextBlock.Text = string.Empty;

            VirtualKeyInfo? info = ShimizuToolkit.HotkeyWinUI.VirtualKeyNameDict.Current.GetVirtualKeyInfo(_keyCode);
            _leftArrowFontIcon?.Visibility = Visibility.Collapsed;
            _upArrowFontIcon?.Visibility = Visibility.Collapsed;
            _rightArrowFontIcon?.Visibility = Visibility.Collapsed;
            _downArrowFontIcon?.Visibility = Visibility.Collapsed;
            if (_keyCode == (uint)Windows.System.VirtualKey.Shift)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uE752"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.LeftShift)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uE752"
                };
                _keyNamePanel.Children.Add(p);
                _leftArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.RightShift)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uE752"
                };
                _keyNamePanel.Children.Add(p);
                _rightArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.Enter)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uE751"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.Down)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uE70D"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.Up)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uE70E"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.Left)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uE76B"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.Right)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uE76C"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadA)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF093"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadB)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF094"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadY)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF095"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadX)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF096"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadDPadLeft)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF10E"
                };
                _keyNamePanel.Children.Add(p);
                _leftArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadDPadUp)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF10E"
                };
                _keyNamePanel.Children.Add(p);
                _upArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadDPadRight)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF10E"
                };
                _keyNamePanel.Children.Add(p);
                _rightArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadDPadDown)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF10E"
                };
                _keyNamePanel.Children.Add(p);
                _downArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadLeftTrigger)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF10A"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadRightTrigger)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF10B"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadLeftShoulder)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF10C"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadRightShoulder)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF10D"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadMenu)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uEDE3"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadView)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uEECA"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadLeftThumbstickButton)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF108"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadLeftThumbstickLeft)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF108"
                };
                _keyNamePanel.Children.Add(p);
                _leftArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadLeftThumbstickUp)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF108"
                };
                _keyNamePanel.Children.Add(p);
                _upArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadLeftThumbstickRight)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF108"
                };
                _keyNamePanel.Children.Add(p);
                _rightArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadLeftThumbstickDown)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF108"
                };
                _keyNamePanel.Children.Add(p);
                _downArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadRightThumbstickButton)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF109"
                };
                _keyNamePanel.Children.Add(p);
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadRightThumbstickLeft)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF109"
                };
                _keyNamePanel.Children.Add(p);
                _leftArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadRightThumbstickUp)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF109"
                };
                _keyNamePanel.Children.Add(p);
                _upArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadRightThumbstickRight)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF109"
                };
                _keyNamePanel.Children.Add(p);
                _rightArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else if (_keyCode == (uint)Windows.System.VirtualKey.GamepadRightThumbstickDown)
            {
                KeyBlockFontIconPresenter p = new()
                {
                    Glyph = "\uF109"
                };
                _keyNamePanel.Children.Add(p);
                _downArrowFontIcon?.Visibility = Visibility.Visible;
            }
            else
            {
                KeyBlockTextPresenter p = new()
                {
                    Text = info?.MainName
                };

                _keyNamePanel.Children.Add(p);

                if (_keyCode == (uint)Windows.System.VirtualKey.LeftControl
                    || _keyCode == (uint)Windows.System.VirtualKey.LeftMenu
                    || _keyCode == (uint)Windows.System.VirtualKey.LeftWindows)
                {
                    _leftArrowFontIcon?.Visibility = Visibility.Visible;
                }
                if (_keyCode == (uint)Windows.System.VirtualKey.RightControl
                    || _keyCode == (uint)Windows.System.VirtualKey.RightMenu
                    || _keyCode == (uint)Windows.System.VirtualKey.RightWindows)
                {
                    _rightArrowFontIcon?.Visibility = Visibility.Visible;
                }
            }

            if (info == null)
                return;
            if (info.IsOEMKey)
            {
                _specialSourceTextBlock.Text = "OEM";
            }
            else if (info.IsNumpadKey)
            {
                _specialSourceTextBlock.Text = "NUMPAD";
            }
            else if (info.IsNumKey)
            {
                _specialSourceTextBlock.Text = "NUM";
            }
            else if (info.IsModifierKey)
            {
                _specialSourceTextBlock.Text = "MOD";
            }
            else if (info.InputDeviceType == InputDeviceType.Gamepad)
            {
                _specialSourceTextBlock.Text = "GAMEPAD";
            }
        }
    }
}
