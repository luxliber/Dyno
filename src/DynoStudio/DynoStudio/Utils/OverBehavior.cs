using System.Windows;
using System.Windows.Input;

namespace Prorubim.DynoStudio.Utils
{
    public static class OverBehavior
    {
        private static readonly DependencyProperty OverCommandProperty =
            DependencyProperty.RegisterAttached
            (
                "OverCommand",
                typeof(ICommand),
                typeof(OverBehavior),
                new PropertyMetadata(OverCommandPropertyChangedCallBack)
            );

        public static void SetOverCommand(this UIElement inUiElement, ICommand inCommand) => inUiElement.SetValue(OverCommandProperty, inCommand);


        private static ICommand GetOverCommand(UIElement inUiElement) => (ICommand)inUiElement.GetValue(OverCommandProperty);

        private static void OverCommandPropertyChangedCallBack(
            DependencyObject inDependencyObject, DependencyPropertyChangedEventArgs inEventArgs)
        {
            var uiElement = inDependencyObject as UIElement;
            if (null == uiElement) return;

            uiElement.PreviewMouseMove += (sender, args) =>
            {
                GetOverCommand(uiElement).Execute(args);
                
            };
        }

    }
}
