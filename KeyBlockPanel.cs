using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShimizuToolkit.HotkeyWinUI.Controls
{
    public sealed partial class KeyBlockPanel : Control
    {
        private WrapPanel? _wrapPanel;

        public IEnumerable<uint> VirtualKeys
        {
            get => field;
            set
            {
                field = value;
                UpdateUI();
            }
        } = [];
        public KeyBlockPanel()
        {
            DefaultStyleKey = typeof(KeyBlockPanel);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _wrapPanel = GetTemplateChild("PART_KeyBlockWrapPanel") as WrapPanel;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if(_wrapPanel == null)return;
            _wrapPanel.Children.Clear();
            foreach (uint keyCode in VirtualKeys)
            {
                KeyBlock keyBlock = new() { KeyCode = keyCode };
                _wrapPanel.Children.Add(keyBlock);
            }
        }
    }
}
