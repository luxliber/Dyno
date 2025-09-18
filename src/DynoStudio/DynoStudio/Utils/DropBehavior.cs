using System.Windows;
using System.Windows.Input;

namespace Prorubim.DynoStudio.Utils
{
    public static class DropBehavior
    {
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.RegisterAttached
            (
                "DropCommand",
                typeof(ICommand),
                typeof(DropBehavior),
                new PropertyMetadata(DropCommandPropertyChangedCallBack)
            );

        public static void DropCommandPropertyChangedCallBack(
            DependencyObject inDependencyObject, DependencyPropertyChangedEventArgs inEventArgs)
        {
            var uiElement = inDependencyObject as UIElement;
            if (null == uiElement) return;

            uiElement.Drop += (sender, args) =>
            {
                GetDropCommand(uiElement).Execute(new [] { sender, args });
                args.Handled = true;
            };
        }

        public static ICommand GetDropCommand(UIElement inUiElement) => (ICommand)inUiElement.GetValue(DropCommandProperty);
        public static void SetDropCommand(this UIElement inUiElement, ICommand inCommand) => inUiElement.SetValue(DropCommandProperty, inCommand);
    }
}