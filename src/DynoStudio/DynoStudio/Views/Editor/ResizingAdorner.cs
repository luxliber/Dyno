using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Dyno;
using Dyno.FormControls;
using Dyno.Views.FormControls;
using Prorubim.DynoStudio.History;
using Prorubim.DynoStudio.ViewModels;

namespace Prorubim.DynoStudio.Editor
{
    public class ResizingAdorner : Adorner
    {
        private readonly ViewModels.EditorManager _editorManager;

        // Resizing adorner uses Thumbs for visual elements.  
        // The Thumbs have built-in mouse input handling.
        readonly Thumb _topLeft, _topRight, _bottomLeft, _bottomRight;

        // To store and manage the adorner's visual children.
        readonly VisualCollection _visualChildren;

        // Initialize the ResizingAdorner.
        public ResizingAdorner(FrameworkElement adornedElement, ViewModels.EditorManager editorManager)
            : base(adornedElement)
        {
            _editorManager = editorManager;
            _visualChildren = new VisualCollection(this);

            // Call a helper method to initialize the Thumbs
            // with a customized cursors.
            BuildAdornerCorner(ref _topLeft, Cursors.SizeNWSE);
            BuildAdornerCorner(ref _topRight, Cursors.SizeNESW);
            BuildAdornerCorner(ref _bottomLeft, Cursors.SizeNESW);
            BuildAdornerCorner(ref _bottomRight, Cursors.SizeNWSE);

            // Add handlers for resizing.
            _bottomLeft.DragDelta += HandleBottomLeft;
            _bottomRight.DragDelta += HandleBottomRight;
            _topLeft.DragDelta += HandleTopLeft;
            _topRight.DragDelta += HandleTopRight;

            _bottomLeft.DragCompleted += DragCompleted;
            _bottomRight.DragCompleted += DragCompleted;
            _topLeft.DragCompleted += DragCompleted;
            _topRight.DragCompleted += DragCompleted;



        }

        private void DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var canvas = AdornedElement as Grid;
            if (canvas != null)
            {
               _editorManager.OnPropertyChanged(nameof(ViewModels.EditorManager.PropertyGridItem));
            }
            var control = AdornedElement as FormControl;


            if (control != null)
                DynoManager.SelectedHistory.AddAction(new HistoryElementBounds(control));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
           
            var adornedElementRect = new Rect(AdornedElement.RenderSize);

         
            var renderBrush = new SolidColorBrush(Colors.DodgerBlue) { Opacity = 0.5 };
            var renderPen = new Pen(new SolidColorBrush(Colors.DodgerBlue), 1.5);
            const double renderRadius = 5.0;

            // Draw a circle at each corner.
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
        }

        // Handler for resizing from the bottom-right.
        private void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {
            var hitThumb = sender as Thumb;
            if (hitThumb == null)
                return;

            var border = AdornedElement as Grid;
            if (border != null)
            {
                var adornedBorder = border;
                adornedBorder.Width = Math.Max(adornedBorder.Width + Math.Floor(args.HorizontalChange / 5) * 5, hitThumb.DesiredSize.Width);
                adornedBorder.Height = Math.Max(adornedBorder.Height + Math.Floor(args.VerticalChange / 5) * 5, hitThumb.DesiredSize.Height);
                return;
            }

            var adornedElement = AdornedElement as FormControl;
            if (adornedElement == null) return;

            EnforceSize(adornedElement);
            ModifyControlHorizontalRight(args, adornedElement, hitThumb);
            ModifyControlVerticalBottom(args, adornedElement, hitThumb);
            adornedElement.UpdatePosition();
        }

        private static void ModifyControlVerticalBottom(DragDeltaEventArgs args, FormControl control, Thumb hitThumb)
        {
            if (control.VerticalAlignment == VerticalAlignment.Top ||
                control.VerticalAlignment == VerticalAlignment.Center)
            {
                control.EditorHeight = Math.Max(control.Height + Math.Floor(args.VerticalChange / 5) * 5,
                    hitThumb.DesiredSize.Height);
            }
            else if (control.VerticalAlignment == VerticalAlignment.Stretch)
            {
                control.EditorBottom -= Math.Floor(args.VerticalChange / 5) * 5;
            }
            else
            {
                control.EditorHeight = Math.Max(control.Height + Math.Floor(args.VerticalChange / 5) * 5,
                    hitThumb.DesiredSize.Height);
                control.EditorBottom -= Math.Floor(args.VerticalChange / 5) * 5;
            }
        }

        private static void ModifyControlHorizontalRight(DragDeltaEventArgs args, FormControl control, Thumb hitThumb)
        {
            if (control.HorizontalAlignment == HorizontalAlignment.Left ||
                control.HorizontalAlignment == HorizontalAlignment.Center)
            {
                control.EditorWidth = Math.Max(control.Width + Math.Floor(args.HorizontalChange / 5) * 5,
                    hitThumb.DesiredSize.Width);
            }
            else if (control.HorizontalAlignment == HorizontalAlignment.Stretch)
            {
                control.EditorRight -= Math.Floor(args.HorizontalChange / 5) * 5;
            }
            else
            {
                control.EditorWidth = Math.Max(control.Width + Math.Floor(args.HorizontalChange / 5) * 5,
                    hitThumb.DesiredSize.Width);
                control.EditorRight -= Math.Floor(args.HorizontalChange / 5) * 5;
            }
        }

        // Handler for resizing from the top-right.
        private void HandleTopRight(object sender, DragDeltaEventArgs args)
        {
            var hitThumb = sender as Thumb;
            if (hitThumb == null)
                return;

            var border = AdornedElement as Grid;
            if (border != null)
            {
                var adornedBorder = border;
                adornedBorder.Width = Math.Max(adornedBorder.Width + Math.Floor(args.HorizontalChange / 5) * 5, hitThumb.DesiredSize.Width);

                return;
            }

            var adornedElement = AdornedElement as FormControl;
            if (adornedElement == null) return;

            EnforceSize(adornedElement);
            ModifyControlHorizontalRight(args, adornedElement, hitThumb);
            ModifyControlVerticalTop(args, adornedElement, hitThumb);
            adornedElement.UpdatePosition();
        }

        private static void ModifyControlVerticalTop(DragDeltaEventArgs args, FormControl control, Thumb hitThumb)
        {
            if (control.VerticalAlignment == VerticalAlignment.Bottom ||
                control.VerticalAlignment == VerticalAlignment.Center)
            {
                control.EditorHeight = Math.Max(control.Height - Math.Floor(args.VerticalChange / 5) * 5,
                    hitThumb.DesiredSize.Height);
            }
            else if (control.VerticalAlignment == VerticalAlignment.Stretch)
            {
                control.EditorTop += Math.Floor(args.VerticalChange / 5) * 5;
            }
            else
            {
                control.EditorHeight = Math.Max(control.Height - Math.Floor(args.VerticalChange / 5) * 5,
                    hitThumb.DesiredSize.Height);
                control.EditorTop += Math.Floor(args.VerticalChange / 5) * 5;
            }
        }

        // Handler for resizing from the top-left.
        private void HandleTopLeft(object sender, DragDeltaEventArgs args)
        {
            var hitThumb = sender as Thumb;
            if (hitThumb == null)
                return;



            var adornedElement = AdornedElement as FormControl;

            if (adornedElement == null) return;

            EnforceSize(adornedElement);
            ModifyControlHorizontalLeft(args, adornedElement, hitThumb);
            ModifyControlVerticalTop(args, adornedElement, hitThumb);
            adornedElement.UpdatePosition();
        }

        // Handler for resizing from the bottom-left.
        public void HandleBottomLeft(object sender, DragDeltaEventArgs args)
        {
            var hitThumb = sender as Thumb;
            if (hitThumb == null)
                return;

            var border = AdornedElement as Grid;
            if (border != null)
            {
                var adornedBorder = border;
                adornedBorder.Height = Math.Max(adornedBorder.Height + Math.Floor(args.VerticalChange / 5) * 5, hitThumb.DesiredSize.Height);
                return;
            }

            var adornedElement = AdornedElement as FormControl;
            if (adornedElement == null) return;

            EnforceSize(adornedElement);
            ModifyControlHorizontalLeft(args, adornedElement, hitThumb);
            ModifyControlVerticalBottom(args, adornedElement, hitThumb);
            adornedElement.UpdatePosition();
        }

        private static void ModifyControlHorizontalLeft(DragDeltaEventArgs args, FormControl control, Thumb hitThumb)
        {
            if (control.HorizontalAlignment == HorizontalAlignment.Right ||
                control.HorizontalAlignment == HorizontalAlignment.Center)
            {
                control.EditorWidth = Math.Max(control.Width - Math.Floor(args.HorizontalChange / 5) * 5,
                    hitThumb.DesiredSize.Width);
            }
            else if (control.HorizontalAlignment == HorizontalAlignment.Stretch)
            {
                control.EditorLeft += Math.Floor(args.HorizontalChange / 5) * 5;
            }
            else
            {
                control.EditorWidth = Math.Max(control.Width - Math.Floor(args.HorizontalChange / 5) * 5,
                    hitThumb.DesiredSize.Width);
                control.EditorLeft += Math.Floor(args.HorizontalChange / 5) * 5;
            }
        }

        // Arrange the Adorners.
        protected override Size ArrangeOverride(Size finalSize)
        {
            // desiredWidth and desiredHeight are the width and height of the element that's being adorned.  
            // These will be used to place the ResizingAdorner at the corners of the adorned element.  
            var desiredWidth = AdornedElement.RenderSize.Width;
            var desiredHeight = AdornedElement.RenderSize.Height;
            // adornerWidth & adornerHeight are used for placement as well.
            var adornerWidth = RenderSize.Width;
            var adornerHeight = RenderSize.Height;

            _topLeft.Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            _topRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            _bottomLeft.Arrange(new Rect(-adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            _bottomRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));

            // Return the final size.
            return finalSize;
        }

        // Helper method to instantiate the corner Thumbs, set the Cursor property, 
        // set some appearance properties, and add the elements to the visual tree.
        private void BuildAdornerCorner(ref Thumb cornerThumb, Cursor customizedCursor)
        {
            if (cornerThumb != null) return;

            cornerThumb = new Thumb { Cursor = customizedCursor };

            // Set some arbitrary visual characteristics.
            cornerThumb.Height = cornerThumb.Width = 10;
            cornerThumb.Opacity = 0.0;
            cornerThumb.Background = new SolidColorBrush(Colors.Transparent);

            _visualChildren.Add(cornerThumb);
        }

        // This method ensures that the Widths and Heights are initialized.  Sizing to content produces
        // Width and Height Values of Double.NaN.  Because this Adorner explicitly resizes, the Width and Height
        // need to be set first.  It also sets the maximum size of the adorned element.
        private void EnforceSize(FrameworkElement adornedElement)
        {
            var parent = adornedElement.Parent as FrameworkElement;
            if (parent == null) return;
            adornedElement.MaxHeight = parent.ActualHeight;
            adornedElement.MaxWidth = parent.ActualWidth;
        }
        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount => _visualChildren.Count;
        protected override Visual GetVisualChild(int index) => _visualChildren[index];
    }
}
