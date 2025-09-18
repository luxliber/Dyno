using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Dyno.FormControls;
using Dyno.Forms.History;
using Dyno.FormsAndWindows;
using WPG;
using Clipboard = System.Windows.Clipboard;
using Control = System.Windows.Controls.Control;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using KeyEventHandler = System.Windows.Input.KeyEventHandler;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using PropertyGrid = WPG.PropertyGrid;

namespace Dyno.Forms
{
    internal class BackCanvas : Canvas
    {
        AdornerLayer _aLayer;

        bool _isDown;
        bool _isDragging;
        bool _selected;
        public FrameworkElement SelectedElement { get; private set; }

        private readonly List<FormControl> _selectedElements = new List<FormControl>();

        private Point _startPoint;
        private bool _isCtrl;

        public override void EndInit()
        {
            base.EndInit();

            var parentWindow = Window.GetWindow(this);
            if (parentWindow == null) return;


            var scrollViewer = Parent as ScrollViewer;
            if (scrollViewer != null) scrollViewer.PreviewMouseWheel += ParentWindow_MouseWheel;

            parentWindow.MouseMove += BackCanvas_MouseMove;
            parentWindow.MouseLeave += BackCanvas_MouseLeave;

            parentWindow.MouseDoubleClick += ParentWindowOnMouseDoubleClick;


            parentWindow.PreviewMouseLeftButtonDown += BackCanvas_PreviewMouseLeftButtonDown;
            parentWindow.PreviewMouseLeftButtonUp += BackCanvas_PreviewMouseLeftButtonUp;

            parentWindow.AddHandler(KeyDownEvent, new KeyEventHandler(BackCanvas_KeyDown), true);
            parentWindow.AddHandler(KeyUpEvent, new KeyEventHandler(BackCanvas_KeyUp), true);
        }

        private void ParentWindowOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ParentWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
            var form = DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm;


            form.Zoom += e.Delta * 0.001;

            if (form.Zoom < 0.3)
                form.Zoom = 0.3;

            if (form.Zoom > 3)
                form.Zoom = 3;
        }

        internal void PropertyGridOnPropertyChanged(object sender, CurPropertyChangedEventHandlerArgs e)
        {
            var propertyName = e.SelectedProperty.OriginalName;

            if (SelectedElement != null && _selectedElements.Count > 1)
            {
                var prs = TypeDescriptor.GetProperties(SelectedElement,
                    new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) });

                if (propertyName == "Left" || propertyName == "Top")
                    SyncValueForSelectedElements("Canvas.", prs, propertyName);
                else
                    SyncValueForSelectedElements("", prs, propertyName);
            }


            DynoManager.SelectedHistory.AddAction(new HistoryPropChanging(e.SelectedProperty.Instance, e.SelectedProperty.Prop, e.SelectedProperty.OldValue, e.SelectedProperty.Value));
        }

        private void SyncValueForSelectedElements(string baseType, PropertyDescriptorCollection prs, string pName)
        {
            var dp = prs.Find($"{baseType}{pName}", false);
            if (dp == null) return;

            var val = dp.GetValue(SelectedElement);

            foreach (var element in _selectedElements)
                if (!Equals(element, SelectedElement))
                {
                    var eprs = TypeDescriptor.GetProperties(element,
                        new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) });
                    var edp = eprs.Find($"{baseType}{pName}", false);

                    if (edp == null) continue;

                    var oldVal = edp.GetValue(element);
                    edp.SetValue(element, val);

                    DynoManager.SelectedHistory.AddAction(new HistoryPropChanging(element, edp, oldVal, val));
                }
        }

        private void BackCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
                _isCtrl = false;
        }

        private void BackCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Source is PropertyGrid || e.Source is DataGrid && ((DataGrid)e.Source).Name == "BindingsDataGrid")
                return;


            if (e.Key == Key.LeftCtrl)
                _isCtrl = true;

            if (e.Key == Key.Delete && _selected)
                DeleteControl();
            else if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && _selected)
                CopyControl();
            else if (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && _selected)
                CutControl();
            else if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                PasteControl();
            else if (e.Key == Key.Right && _selected)
                TranslateSelectedElements((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift ? 20 : 5, 0);
            else if (e.Key == Key.Left && _selected)
                TranslateSelectedElements((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift ? -20 : -5, 0);
            else if (e.Key == Key.Up && _selected)
                TranslateSelectedElements(0, (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift ? -20 : -5);
            else if (e.Key == Key.Down && _selected)
                TranslateSelectedElements(0, (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift ? 20 : 5);
            else if (e.Key == Key.Z)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    DynoManager.SelectedHistory.Undo();
            }
            else if (e.Key == Key.Y)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    DynoManager.SelectedHistory.Redo();
            }
            else if ((e.Key == Key.T && ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)) ||
                     e.Key == Key.F5)
            {
                DynoManager.TestForm();
            }

            if (_selected)
                e.Handled = true;
        }

        internal void DeleteControl()
        {
            var tb =
                DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.TabItems[
                    DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.SelectedIndex];


            foreach (var el in _selectedElements)
            {
                DynoManager.SelectedHistory.AddAction(new HistoryElementDeleting(el, tb));
                tb.Items.Remove(el);
            }
            UnselectAll();
        }

        internal void CopyControl()
        {
            var myFormat = DataFormats.GetDataFormat("FormControl");
            var dataObject = new DataObject();

            var scs = new List<KeyValuePair<Type, byte[]>>();

            foreach (var el in _selectedElements)
            {
                if (el.GetType() == typeof(FormImage))
                    ((FormImage)el).Source = null;

                using (var ms = el.Serialize())
                    scs.Add(new KeyValuePair<Type, byte[]>(el.GetType(), ms.ToArray()));

                if (el.GetType() == typeof(FormImage))
                    ((FormImage)el).Update();
            }

            dataObject.SetData(myFormat.Name, scs);
            Clipboard.SetDataObject(dataObject);
        }

        internal void PasteControl()
        {
            if (!Clipboard.ContainsData("FormControl")) return;

            var myRetrievedObject = Clipboard.GetDataObject();
            var myDereferencedObjects = (List<KeyValuePair<Type, byte[]>>)myRetrievedObject?.GetData("FormControl");
            if (myDereferencedObjects == null) return;

            UnselectAll();

            foreach (var obj in myDereferencedObjects)
            {
                var c = FormControl.Deserialize(obj);
                if (c == null) continue;

                var tb =
                    DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.TabItems[
                        DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.SelectedIndex];
                tb.Items.Add(c);
                Select(c);
                c.Form = tb.WorkspaceForm;
                FormControlHelper.UpdateControlValuesFromPresetOrBindings(c);
                DynoManager.SelectedHistory.AddAction(new HistoryElementCreating(c, tb));
            }
        }

        internal void CutControl()
        {
            var myFormat = DataFormats.GetDataFormat("FormControl");
            var dataObject = new DataObject();

            var scs = new List<KeyValuePair<Type, byte[]>>();


            foreach (var el in _selectedElements)
            {
                if (el.GetType() == typeof(FormImage))
                    ((FormImage)el).Source = null;

                using (var ms = el.Serialize())
                    scs.Add(new KeyValuePair<Type, byte[]>(el.GetType(), ms.ToArray()));

                if (el.GetType() == typeof(FormImage))
                    ((FormImage)el).Update();
            }

            dataObject.SetData(myFormat.Name, scs);
            Clipboard.SetDataObject(dataObject);

            var tb =
                DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.TabItems[
                    DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.SelectedIndex];

            foreach (var el in _selectedElements)
            {
                DynoManager.SelectedHistory.AddAction(new HistoryElementDeleting(el, tb));
                tb.Items.Remove(el);
            }

            UnselectAll();
        }

        private void TranslateSelectedElements(double x, double y, bool forceAddingAction = true)
        {
            if (SelectedElement is Border)
            {
                SetLeft(SelectedElement, GetLeft(SelectedElement) + x);
                SetTop(SelectedElement, GetTop(SelectedElement) + y);
                return;
            }

            foreach (var el in _selectedElements)
            {
                var control = el;
                if (control != null)
                {
                    if (forceAddingAction)
                        control.SaveOldBounds();

                    if (control.HorizontalAlignment == HorizontalAlignment.Left)
                        control.EditorLeft += x;
                    else if (control.HorizontalAlignment == HorizontalAlignment.Right)
                        control.EditorRight -= x;
                    else
                    {
                        control.EditorLeft += x;
                        control.EditorRight -= x;
                    }

                    if (control.VerticalAlignment == VerticalAlignment.Top)
                        control.EditorTop += y;
                    else if (control.VerticalAlignment == VerticalAlignment.Bottom)
                        control.EditorBottom -= y;
                    else
                    {
                        control.EditorTop += y;
                        control.EditorBottom -= y;
                    }

                    control.UpdatePosition();

                    if (forceAddingAction)
                        DynoManager.SelectedHistory.AddAction(new HistoryElementBounds(control));
                }
            }
        }

        private void BackCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is PropertyGrid)
                return;

            StopDragging();
        }

        private void BackCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is PropertyGrid || e.Source is ScrollViewer || !CheckControlParents(e.Source))
                return;

            var el = e.Source as FrameworkElement;
            FrameworkElement control = null;

            if (el != null && el.Name == "FormTabControl")
            {
                object selItem = null;
                var cp = Utils.FindVisualChild<FormCanvas>(el);
                foreach (var item in ((FormTabControl)el).Items)
                {
                    var tc = ((FormTabControl)el).ItemContainerGenerator.ContainerFromItem(item) as FormTabItem;
                    if ((tc == null || !tc.IsMouseOver) && (cp == null || !cp.IsMouseOver)) continue;

                    selItem = item;
                    break;
                }

                var currentTab = ((FormTabControl)el).SelectedItem as FormTab;
                if (currentTab != null)
                    foreach (var item in currentTab.Items)
                        if (item.IsMouseOver)
                        {
                            control = item;

                            e.Handled = true;
                            break;
                        }

                if (selItem != null && control == null)
                {
                    UnselectAll();
                    App.Instance.Mw.PropertyGrid.Instance = selItem;
                    control = cp;
                }
            }
            else if (el != null && el.Name == "FormBorder")
            {
                UnselectAll();
                control = el;
            }
            else if (el != null && el.Name == "HeaderBorder")
            {
                UnselectAll();
                control = ((Control)sender).FindName("FormBorder") as FrameworkElement;
            }

            if (control != null)
            {
                if (!_selectedElements.Contains(control))
                {
                    if (!_isCtrl)
                        UnselectAll();
                    Select(control);
                }

                foreach (var els in _selectedElements)
                    els.SaveOldBounds();

                var window = Window.GetWindow(this);
                if (window != null)
                {
                    var back = window.FindName("BackgroundView") as UIElement;
                    back?.Focus();
                }
                _isDown = true;

            }
            else if (!_isCtrl)
                UnselectAll();

            _startPoint = Mouse.GetPosition(this);
        }

        private static bool CheckControlParents(object source)
        {
            var control = source as FrameworkElement;

            if (control != null && control.Name == "FormBorder")
                return true;

            while (control?.Parent != null)
            {
                var controlParent = control.Parent as FrameworkElement;
                if (controlParent != null && controlParent.Name == "FormBorder")
                    return true;
                control = controlParent;
            }

            return false;
        }

        public void Select(FrameworkElement control)
        {
            if (control is FormCanvas)
                SelectedElement = control;
            else if (control != null && control.Name == "FormBorder")
            {
                App.Instance.Mw.PropertyGrid.Instance = DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm;
                return;
            }
            else if (control is FormControl)
            {
                App.Instance.Mw.PropertyGrid.Instance = control;
                _selectedElements.Add((FormControl) control);
                SelectedElement = control;
            }

            if (SelectedElement == null) return;

            _aLayer = AdornerLayer.GetAdornerLayer(SelectedElement);
            _aLayer.Add(new ResizingAdorner(SelectedElement));

            _selected = true;
        }

        internal void UnselectAll()
        {
            if (!_selected) return;

            foreach (var el in _selectedElements)
            {
                var ad = _aLayer.GetAdorners(el);
                if (ad != null && ad.Any() && ad[0].IsMouseOver)
                    return;
            }

            _selected = false;
            if (SelectedElement != null)
            {
                // Remove the adorner from the selected element
                var adorners = _aLayer.GetAdorners(SelectedElement);
                if (adorners != null)
                    _aLayer.Remove(adorners[0]);

                SelectedElement = null;
                App.Instance.Mw.PropertyGrid.Instance = null;
            }

            foreach (var el in _selectedElements)
            {
                var ad = _aLayer.GetAdorners(el);
                if (ad != null && ad.Any())
                    _aLayer.Remove(ad[0]);
            }

            _selectedElements.Clear();
        }

        void BackCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        private void AddMoveActionForAllSelectedElements()
        {
            if (SelectedElement == null) return;

            foreach (var el in _selectedElements)
            {
                var control = el;
                if (control != null)
                    DynoManager.SelectedHistory.AddAction(new HistoryElementBounds(control));
            }
        }

        void BackCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(this).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(this).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                    _isDragging = true;

                if (_isDragging)
                {
                    var position = Mouse.GetPosition(this);

                    var dx = position.X - _startPoint.X;
                    var dy = position.Y - _startPoint.Y;

                    double tx = 0;
                    double ty = 0;

                    if (dx >= 5)
                    {
                        tx = Math.Floor(dx / 5) * 5;
                        _startPoint.X = _startPoint.X + tx;
                    }

                    if (dx <= -5)
                    {
                        tx = Math.Ceiling(dx / 5) * 5;
                        _startPoint.X = _startPoint.X + tx;
                    }

                    if (dy >= 5)
                    {
                        ty = Math.Floor(dy / 5) * 5;
                        _startPoint.Y = _startPoint.Y + ty;
                    }

                    if (dy <= -5)
                    {
                        ty = Math.Ceiling(dy / 5) * 5;
                        _startPoint.Y = _startPoint.Y + ty;
                    }

                    TranslateSelectedElements(tx, ty, false);

                    InvalidateMeasure();
                }
            }
        }

        private void StopDragging()
        {
            if (_isDown)
            {
                _isDown = false;

                if (_isDragging)
                    AddMoveActionForAllSelectedElements();

                _isDragging = false;
            }


            if (SelectedElement != null && (SelectedElement.Name == "HeaderBorder" || SelectedElement.Name == "FormBorder"))
                return;

            if (_selectedElements.Count > 0)
            {
                App.Instance.Mw.PropertyGrid.Instance = null;
                App.Instance.Mw.PropertyGrid.Instance = _selectedElements.Last();
            }

        }

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
