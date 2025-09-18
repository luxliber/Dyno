using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Dyno.FormControls;
using Dyno.Forms.Annotations;
using Dyno.Forms.History;
using Dyno.Models.Forms;
using Dyno.Models.Workspaces;
using Dyno.ViewModels;
using Dyno.Views.FormControls;
using Prorubim.DynoStudio.Editor;
using Prorubim.DynoStudio.History;
using Clipboard = System.Windows.Clipboard;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace Prorubim.DynoStudio.ViewModels
{
    public partial class EditorManager : INotifyPropertyChanged
    {
        public WorkspacePreset WorkspacePreset => DynoManagerBase.SelectedWorkspacePreset;
        public Workspace Workspace => WorkspacePreset?.Workspace;
        public WorkspaceForm WorkspaceForm => Workspace?.WorkspaceForm;

       
        public bool IsFormSelected => DynoManagerBase.SelectedWorkspacePreset != null && DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm != null;

       

        private AdornerLayer _aLayer;

        internal bool IsElementSelected;
        public FrameworkElement SelectedElement { get; private set; }
        internal readonly List<FormControl> SelectedElements = new List<FormControl>();

        private object _propertyGridItem;

        public object PropertyGridItem
        {
            get { return _propertyGridItem; }
            set
            {
                _propertyGridItem = value;
                OnPropertyChanged(nameof(PropertyGridItem));
            }
        }

        internal void SyncValueForSelectedElements(string baseType, PropertyDescriptorCollection prs, string pName)
        {
            var dp = prs.Find($"{baseType}{pName}", false);
            if (dp == null) return;

            var val = dp.GetValue(SelectedElement);

            foreach (var element in SelectedElements)
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

        internal void DeleteControl()
        {
            var tb =
                DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.TabItems[
                    DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.SelectedIndex];


            foreach (var el in SelectedElements)
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

            foreach (var el in SelectedElements)
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
               
                c.Form = tb.WorkspaceForm;
                FormControlHelper.UpdateControlValuesFromBindings(c);
                FormControlHelper.UpdateControlValuesFromExpressions(c);

                DynoManager.SelectedHistory.AddAction(new HistoryElementCreating(c, tb));
                Select(c);
                PropertyGridItem = c;
            }
        }

        internal void CutControl()
        {
            var myFormat = DataFormats.GetDataFormat("FormControl");
            var dataObject = new DataObject();

            var scs = new List<KeyValuePair<Type, byte[]>>();


            foreach (var el in SelectedElements)
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

            foreach (var el in SelectedElements)
            {
                DynoManager.SelectedHistory.AddAction(new HistoryElementDeleting(el, tb));
                tb.Items.Remove(el);
            }

            UnselectAll();
        }

        internal void TranslateSelectedElements(double x, double y, bool forceAddingAction = true)
        {
            if (SelectedElement is Border)
            {
                Canvas.SetLeft(SelectedElement, Canvas.GetLeft(SelectedElement) + x);
                Canvas.SetTop(SelectedElement, Canvas.GetTop(SelectedElement) + y);
                return;
            }

            foreach (var el in SelectedElements)
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

        public void Select(FrameworkElement control)
        {
            if (control == null)
                return;

            if (control is FormControl)
                SelectedElements.Add((FormControl)control);

            SelectedElement = control;


            _aLayer = AdornerLayer.GetAdornerLayer(control);
            _aLayer.Add(new ResizingAdorner(control,this));

            IsElementSelected = true;
        }

        internal void UnselectAll(bool forceClearPropertyGrid = true)
        {
            if (PropertyGridItem != null && forceClearPropertyGrid)
            {
                PropertyGridItem = null;
                OnPropertyChanged(nameof(PropertyGridItem));
            }

            if (!IsElementSelected) return;

            foreach (var el in SelectedElements)
            {
                var ad = _aLayer.GetAdorners(el);
                if (ad != null && ad.Any() && ad[0].IsMouseOver)
                    return;
            }

            IsElementSelected = false;

            if (SelectedElement != null)
            {
                // Remove the adorner from the selected element
                var adorners = _aLayer.GetAdorners(SelectedElement);
                if (adorners != null)
                    _aLayer.Remove(adorners[0]);

                SelectedElement = null;
            }

            foreach (var el in SelectedElements)
            {
                var ad = _aLayer.GetAdorners(el);
                if (ad != null && ad.Any())
                    _aLayer.Remove(ad[0]);
            }

            SelectedElements.Clear();

        }


        internal void AddMoveActionForAllSelectedElements()
        {
            if (SelectedElement == null) return;

            foreach (var el in SelectedElements)
            {
                var control = el;
                if (control != null)
                    DynoManager.SelectedHistory.AddAction(new HistoryElementBounds(control));
            }
        }

        public void BringToFrontControl()
        {
            if (SelectedElement != null) Panel.SetZIndex(SelectedElement, 100000);
        }

        public void SendToBackControl()
        {
            if (SelectedElement != null) Panel.SetZIndex(SelectedElement, -1);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected internal virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
