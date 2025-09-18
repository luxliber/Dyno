using System.Windows;
using System.Windows.Controls;
using Dyno;
using Dyno.FormControls;
using Dyno.Views.FormControls;
using Prorubim.DynoStudio.ViewModels;

namespace Prorubim.DynoStudio.History
{
    public class HistoryElementBounds : IHistoryAction
    {
        private readonly FormControl _element;
        private readonly Thickness _oldRect;
        private readonly Size _oldSize;
        private readonly Thickness _newRect;
        private readonly Size _newSize;


        public HistoryElementBounds(FormControl element)
        {
            _element = element;
            _oldRect = element.OldRect;
            _oldSize = element.OldSize;
            _newRect = new Thickness(Canvas.GetLeft(element), Canvas.GetTop(element), Canvas.GetRight(element), Canvas.GetBottom(element));
            _newSize = new Size(element.Width, element.Height);
        }

        public void RedoAction()
        {
            _element.Height = _newSize.Height;
            _element.Width = _newSize.Width;

            Canvas.SetLeft(_element, _newRect.Left);
            Canvas.SetTop(_element, _newRect.Top);
            Canvas.SetRight(_element, _newRect.Right);
            Canvas.SetBottom(_element, _newRect.Bottom);

            _element.UpdatePosition();
        }

        public void UndoAction()
        {
            _element.EditorHeight = _oldSize.Height;
            _element.EditorWidth = _oldSize.Width;

            Canvas.SetLeft(_element, _oldRect.Left);
            Canvas.SetTop(_element, _oldRect.Top);
            Canvas.SetRight(_element, _oldRect.Right);
            Canvas.SetBottom(_element, _oldRect.Bottom);

            _element.UpdatePosition();
        }
    }
}