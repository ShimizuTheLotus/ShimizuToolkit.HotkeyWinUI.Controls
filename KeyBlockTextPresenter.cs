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
    public sealed partial class KeyBlockTextPresenter : Control
    {
        private TextBlock? _textBlock;

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(KeyBlockTextPresenter),
                new PropertyMetadata(string.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as KeyBlockTextPresenter;
            var newValue = e.NewValue as string;
            c?._textBlock?.Text = newValue;
        }

        public string? Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public KeyBlockTextPresenter()
        {
            DefaultStyleKey = typeof(KeyBlockTextPresenter);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _textBlock = GetTemplateChild("PART_TextBlock") as TextBlock;
            _textBlock?.Text = Text;
        }
    }
}
