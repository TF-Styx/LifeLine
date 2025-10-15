using System.Windows;
using System.Windows.Controls;

namespace Shared.WPF.Resources.Attachables
{
    public static class TextBoxHelper
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached
            (
                "Icon",
                typeof(ControlTemplate),
                typeof(TextBoxHelper),
                new PropertyMetadata(null)
            );

        public static void SetIcon(DependencyObject element, ControlTemplate value) => element.SetValue(IconProperty, value);

        public static ControlTemplate GetIcon(DependencyObject element) => (ControlTemplate)element.GetValue(IconProperty);
    }

}
