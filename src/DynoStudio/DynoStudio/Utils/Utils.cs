using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Prorubim.DynoStudio.Utils
{
    internal class Utils
    {
        public static IEnumerable<FrameworkElement> FindVisualChildren(FrameworkElement depObj, string name)
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i);
                    var children = child as FrameworkElement;
                    if (children != null && children.Name == name)
                        yield return children;

                    foreach (var childOfChild in FindVisualChildren(children, name))
                        yield return childOfChild;
                }
            }
        }

        public static FrameworkElement FindVisualChild(FrameworkElement obj, string name) => FindVisualChildren(obj, name).FirstOrDefault();


        public static T FindElementByName<T>(FrameworkElement element, string sChildName) where T : FrameworkElement
        {
            T childElement = null;
            var nChildCount = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < nChildCount; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;

                if (child == null)
                    continue;

                if (child is T && child.Name.Equals(sChildName))
                {
                    childElement = (T)child;
                    break;
                }

                childElement = FindElementByName<T>(child, sChildName);

                if (childElement != null)
                    break;
            }
            return childElement;
        }


    }
}
