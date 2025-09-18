using System.Windows;
using System.Windows.Media;

namespace Prorubim.DynoStudio.Main
{
    public class ToolButtonProps
    {
        public static ImageSource GetImage(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(ImageProperty);
        }

        public static void SetImage(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.RegisterAttached("Image", typeof(ImageSource), typeof(ToolButtonProps), new UIPropertyMetadata((ImageSource)null));
    }

   
}
