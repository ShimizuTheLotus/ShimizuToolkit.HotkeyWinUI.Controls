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
using static System.Net.Mime.MediaTypeNames;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ShimizuToolkit.HotkeyWinUI.Controls
{
    public sealed partial class KeyBlockFontIconPresenter : Control
    {
        private FontIcon? _fontIcon;

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
        nameof(Glyph),
        typeof(string),
        typeof(KeyBlockFontIconPresenter),
        new PropertyMetadata(string.Empty, OnGlyphChanged));

        private static void OnGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as KeyBlockFontIconPresenter;
            var newValue = e.NewValue as string;
            c?._fontIcon?.Glyph = newValue;
        }
        public string Glyph
        {
            get => (string)GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }
        public KeyBlockFontIconPresenter()
        {
            DefaultStyleKey = typeof(KeyBlockFontIconPresenter);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _fontIcon = GetTemplateChild("PART_FontIcon") as FontIcon;
            _fontIcon?.Glyph = Glyph;
        }
    }
}
