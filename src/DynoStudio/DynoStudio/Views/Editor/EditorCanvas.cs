using System;
using System.Windows;
using System.Windows.Controls;

namespace Prorubim.DynoStudio.Editor
{
    class EditorCanvas : Canvas
    {
        protected override Size MeasureOverride(Size constraint)
        {
            double bottomMost = 0d;
            double rightMost = 0d;

            foreach (var obj in Children)
            {
                var child = obj as FrameworkElement;

                if (child != null)
                {
                    child.Measure(constraint);

                    bottomMost = Math.Max(bottomMost, GetTop(child) + child.DesiredSize.Height);
                    rightMost = Math.Max(rightMost, GetLeft(child) + child.DesiredSize.Width);
                }
            }
            return new Size(rightMost, bottomMost);
        }
    }
}
