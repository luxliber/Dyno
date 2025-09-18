using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Dyno.Annotations;
using Dyno.FormControls;
using Dyno.Models.Workspaces;
using Dyno.ViewModels;
using Dyno.Views.FormControls;
using ProtoBuf;

namespace Dyno.Models.Forms
{
    [ProtoContract]
    public class WorkspaceForm : INotifyPropertyChanged
    {
        private double _zoom = 1.0;

        public Dictionary<string, PortParameter> PortPars { get; set; } = new Dictionary<string, PortParameter>();

        public bool IsNoError
        {
            get
            {
                foreach (var tabItem in ShownTabItems)
                    if (tabItem.Items.Any(x => x.IsError))
                        return false;

                return true;
            }
        }


        [ProtoMember(102)]
        public ObservableCollection<UserParameter> UserPars { get; set; }

        [ProtoMember(1)]
        public ObservableCollection<FormTab> TabItems { get; set; }
        public ObservableCollection<FormTab> ShownTabItems
        {
            get
            {
                if (IsVisibleAllTabs)
                    return TabItems;

                if (DynoManagerBase.SelectedWorkspacePreset != null && PortTabNames == null)
                    return new ObservableCollection<FormTab>(TabItems.Where(x => DynoManagerBase.SelectedWorkspacePreset.FormTabs.Contains(x.Header)).ToList());

                if (PortTabNames != null)
                    return new ObservableCollection<FormTab>(TabItems.Where(x => PortTabNames.Contains(x.Header)).ToList());

                return new ObservableCollection<FormTab>();
            }
        }

        [ProtoMember(2)]
        public int SelectedIndex { get; set; }


        public double Width
        {
            get { return SelectedIndex != -1 ? TabItems[SelectedIndex].Width : TabItems[0].Width; }
            set
            {
                TabItems[SelectedIndex].EditorWidth = value;
                TabItems[SelectedIndex].OnPropertyChanged(nameof(FormTab.EditorWidth));
            }
        }

        public double Height
        {
            get { return SelectedIndex != -1 ? TabItems[SelectedIndex].Height : TabItems[0].Height; }
            set { TabItems[SelectedIndex].Height = value; }
        }

        [ProtoMember(9)]
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName(@"""Run"" Button")]
        public bool EditorIsRunVisible { get; set; }

        [ProtoMember(10)]
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName(@"""Run and Close"" Button")]
        public bool EditorIsRunAndCloseVisible { get; set; }

        [ProtoMember(11)]
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName(@"Resizable")]
        public bool EditorIsResizable { get; set; }

        [ProtoMember(12)]
        [Category(FormControlHelper.PropCategory.Content)]
        [DisplayName(@"""Run"" Button Text")]
        public string EditorRunText { get; set; }
        public string RunText => String.IsNullOrEmpty(EditorRunText) ? "RUN" : EditorRunText.ToUpper();


        [ProtoMember(13)]
        [Category(FormControlHelper.PropCategory.Content)]
        [DisplayName(@"""Run and Close"" Button Text")]
        public string EditorRunAndCloseText { get; set; }
        public string RunAndCloseText => String.IsNullOrEmpty(EditorRunAndCloseText) ? "RUN AND CLOSE" : EditorRunAndCloseText.ToUpper();

        [ProtoMember(14)]
        [Category(FormControlHelper.PropCategory.Content)]
        [DisplayName(@"""Continue"" Button Text")]
        public string EditorContinueText { get; set; }
        public string ContinueText => String.IsNullOrEmpty(EditorContinueText) ? "CONTINUE" : EditorContinueText.ToUpper();

        public bool IsVisibleAllTabs = false;

        public IList PortTabNames;

        public Visibility CloseButtonVisibility => TabItems.Count < 2 ? Visibility.Hidden : Visibility.Visible;

        public Workspace Workspace { get; set; }
        public bool IsProduction { get; set; }

        public double Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                OnPropertyChanged(nameof(Zoom));
            }
        }

        public WorkspaceForm(bool createTab, Workspace workspace)
        {
            Workspace = workspace;

            UserPars = new ObservableCollection<UserParameter>();

            TabItems = new ObservableCollection<FormTab>();
            if (createTab)
                AddTab();

            EditorIsRunAndCloseVisible = true;
            EditorIsRunVisible = true;
            EditorIsResizable = true;
            IsProduction = false;
        }

        public WorkspaceForm()
        {
            TabItems = new ObservableCollection<FormTab>();
            UserPars = new ObservableCollection<UserParameter>();
        }

        public void FillAllControls(FrameworkElement ignoredElement = null)
        {
            if (Workspace != null && PortPars.Count == 0)
            {
                if (ignoredElement != null)
                    FormControlHelper.ValidateControlFromBindings(ignoredElement);

                foreach (var tabItem in TabItems)
                    foreach (var control in tabItem.Items)
                        if (!Equals(control, ignoredElement))
                        {
                            FormControlHelper.UpdateControlValuesFromBindings(control);
                            FormControlHelper.ValidateControlFromBindings(control);
                        }

                DynoManagerBase.ResetCalcEngineFromWorkspace();
            }
            else
            {
                if (ignoredElement != null)
                    FormControlHelper.ValidateControlFromPortData(ignoredElement, PortPars);

                foreach (var tabItem in TabItems)
                    foreach (var control in tabItem.Items)
                        if (!Equals(control, ignoredElement))
                        {
                            FormControlHelper.FillControlValuesFromPortData(control, PortPars);
                            FormControlHelper.ValidateControlFromPortData(control, PortPars);
                        }

                DynoManagerBase.ResetCalcEngineFromPortData(PortPars);
            }

            foreach (var tabItem in TabItems)
                foreach (var control in tabItem.Items)
                    FormControlHelper.UpdateControlValuesFromExpressions(control);

            OnPropertyChanged(nameof(IsNoError));
        }

        public void DeleteTab(FormTab formTab)
        {
            TabItems.Remove(formTab);

            Refresh();
        }



        public FormTab AddTab(FormTab ortab = null)
        {
            var w = 400.0d;
            var h = 300.0d;

            if (TabItems.Count > 0)
            {
                w = TabItems[0].Width;
                h = TabItems[0].Height;
            }

            var tab = new FormTab { Header = "New Tab", WorkspaceForm = this, Width = w, Height = h };

            if (ortab != null)
            {
                foreach (var byteItem in ortab.ByteItems)
                    tab.ByteItems.Add(byteItem);

                tab.RestoreControlsformByteList();
            }

            TabItems.Add(tab);
            SelectedIndex = TabItems.Count - 1;
            Refresh();
            return tab;
        }

        public void Refresh()
        {
            FillAllControls();
            OnPropertyChanged(nameof(SelectedIndex));
            OnPropertyChanged(nameof(CloseButtonVisibility));
        }

        public static void Save(WorkspaceForm wf, bool forceFileSaving = true)
        {
            foreach (var item in wf.TabItems) item.PrepareControlsByteList();

            if (!forceFileSaving) return;

            var formPath = new FileInfo(wf.Workspace.WorkspacePath).DirectoryName;
            if (formPath == null) return;

            var formFile = Path.Combine(formPath, wf.Workspace.Name + ".dfm");
            using (var file = File.Create(formFile))
                Serializer.Serialize(file, wf);
        }

        public static MemoryStream SaveToStream(WorkspaceForm wf)
        {
            foreach (var item in wf.TabItems) item.PrepareControlsByteList();

            var stream = new MemoryStream();

            Serializer.Serialize(stream, wf);
            stream.Flush();
            return stream;

        }

        public static WorkspaceForm Load(Workspace w)
        {
            var formPath = new FileInfo(w.WorkspacePath).DirectoryName;
            if (formPath == null) return null;

            var formFile = Path.Combine(formPath, w.Name + ".dfm");
            return LoadFromPath(formFile);
        }

        public static WorkspaceForm LoadFromPath(string path)
        {
            WorkspaceForm wf;
            var formFile = path;

            if (!File.Exists(formFile)) return null;

            using (var file = File.OpenRead(formFile))
                wf = Serializer.Deserialize<WorkspaceForm>(file);

            wf?.RestoreControlsformXmlList();

            return wf;
        }

        public static WorkspaceForm LoadFromStream(MemoryStream ms)
        {
            if (ms == null) return null;

            ms.Position = 0;
            var wf = Serializer.Deserialize<WorkspaceForm>(ms);
            ms.Close();

            wf?.RestoreControlsformXmlList();
            return wf;
        }

        private void RestoreControlsformXmlList()
        {
            foreach (var item in TabItems)
            {
                item.WorkspaceForm = this;
                item.RestoreControlsformByteList();
            }
        }

        public void UpdatePreset(WorkspacePreset workspacePreset)
        {

            foreach (var tab in TabItems)
                foreach (var item in tab.Items)
                {
                    UpdatePresetParameter(workspacePreset, item);
                }

        }

        public static void UpdateNodeBinding(Dictionary<string, PortParameter> bindings, FrameworkElement item)
        {
            var ctype = item.GetType();
            if (item is FormTextBox)
            {
                var tb = item as FormTextBox;
                if (!String.IsNullOrEmpty(tb.EditorTextBinding))
                    if (bindings.ContainsKey(tb.EditorTextBinding) && tb.Values.Count > 0)
                        bindings[tb.EditorTextBinding].Value = tb.Values["EditorTextBinding"];
            }
            else if (item is FormNumberBox)
            {
                var tb = item as FormNumberBox;
                if (!String.IsNullOrEmpty(tb.EditorTextBinding))
                    if (bindings.ContainsKey(tb.EditorTextBinding) && tb.Values.Count > 0)
                        bindings[tb.EditorTextBinding].Value = tb.Values["EditorTextBinding"];
            }
            else if (ctype == typeof(FormCheckBox))
            {
                var tb = item as FormCheckBox;
                if (!String.IsNullOrEmpty(tb.EditorCheckStatusBinding))
                    if (bindings.ContainsKey(tb.EditorCheckStatusBinding) && tb.Values.Count > 0)
                        bindings[tb.EditorCheckStatusBinding].Value = tb.Values["EditorCheckStatusBinding"];
            }
            else if (ctype == typeof(FormRadioButton))
            {
                var tb = item as FormRadioButton;
                if (!String.IsNullOrEmpty(tb.EditorCheckStatusBinding))
                    if (bindings.ContainsKey(tb.EditorCheckStatusBinding) && bindings[tb.EditorCheckStatusBinding].Value != tb.Values["EditorCheckStatusBinding"] && tb.Values.Count > 0)
                        bindings[tb.EditorCheckStatusBinding].Value = tb.Values["EditorCheckStatusBinding"];
            }
            else if (ctype == typeof(FormComboBox))
            {
                var tb = item as FormComboBox;
                if (!String.IsNullOrEmpty(tb.EditorItemsBinding))
                    if (bindings.ContainsKey(tb.EditorItemsBinding) && tb.Values.Count > 0)
                        bindings[tb.EditorItemsBinding].Value = tb.Values["EditorItemsBinding"];
            }
            else if (ctype == typeof(FormListBox))
            {
                var tb = item as FormListBox;
                if (!String.IsNullOrEmpty(tb.EditorItemsBinding))
                    if (tb.SelectedItems != null && tb.SelectedItems.Count > 1)
                        bindings[tb.EditorItemsBinding].Value = tb.SelectedItems;
                    else
                    if (bindings.ContainsKey(tb.EditorItemsBinding) && tb.Values.Count > 0)
                        bindings[tb.EditorItemsBinding].Value = tb.Values["EditorItemsBinding"];
            }
        }

        public static void UpdatePresetParameter(WorkspacePreset workspacePreset, FrameworkElement item)
        {

            if (item is FormTextBox)
            {
                var formTextBox = item as FormTextBox;
                if (!String.IsNullOrEmpty(formTextBox.EditorTextBinding))
                {
                    var prPar = workspacePreset.GetParameterByName(formTextBox.EditorTextBinding);
                    var testPar = workspacePreset.Workspace.WorkspaceForm.UserPars.FirstOrDefault(par => par.Name == formTextBox.EditorTextBinding);
                    if (prPar != null && formTextBox.Values.Count > 0)
                    {
                        prPar.Value = formTextBox.Values["EditorTextBinding"];
                        prPar.OnPropertyChanged("Value");
                    }
                    else if (testPar != null && formTextBox.Values.Count > 0)
                    {
                        testPar.Value = formTextBox.Values["EditorTextBinding"].ToString();
                        testPar.OnPropertyChanged("Value");
                    }
                }
            }
            else if (item is FormNumberBox)
            {
                var formTextBox = item as FormNumberBox;
                if (!String.IsNullOrEmpty(formTextBox.EditorTextBinding))
                {
                    var prPar = workspacePreset.GetParameterByName(formTextBox.EditorTextBinding);
                    var testPar = workspacePreset.Workspace.WorkspaceForm.UserPars.FirstOrDefault(par => par.Name == formTextBox.EditorTextBinding);
                    if (prPar != null && formTextBox.Values.Count > 0)
                    {
                        prPar.Value = formTextBox.Values["EditorTextBinding"];
                        prPar.OnPropertyChanged("Value");
                    }
                    else if (testPar != null && formTextBox.Values.Count > 0)
                    {
                        testPar.Value = ((double)formTextBox.Values["EditorTextBinding"]).ToString(new NumberFormatInfo { NumberDecimalSeparator = "." });
                        testPar.OnPropertyChanged("Value");
                    }
                }
            }
            else if (item is FormBaseState)
            {
                var tb = item as FormBaseState;
                if (!String.IsNullOrEmpty(tb.EditorCheckStatusBinding))
                {
                    var prPar = workspacePreset.GetParameterByName(tb.EditorCheckStatusBinding);
                    var testPar = workspacePreset.Workspace.WorkspaceForm.UserPars.FirstOrDefault(par => par.Name == tb.EditorCheckStatusBinding);
                    if (prPar != null && tb.Values.Count > 0)
                    {
                        prPar.Value = tb.IsChecked.ToString();
                        prPar.OnPropertyChanged("Value");
                    }
                    else if (testPar != null && tb.Values.Count > 0)
                    {
                        testPar.Value = tb.IsChecked.ToString();
                        testPar.OnPropertyChanged("Value");
                    }
                }
            }
            else
            {
                var tb = item as FormBaseBox;
                if (!String.IsNullOrEmpty(tb?.EditorItemsBinding))
                {
                    var prPar = workspacePreset.GetParameterByName(tb.EditorItemsBinding);
                    var testPar = workspacePreset.Workspace.WorkspaceForm.UserPars.FirstOrDefault(par => par.Name == tb.EditorItemsBinding);
                    if (prPar != null && tb.Values.Count > 0)
                    {
                        prPar.Value = tb.Values["EditorItemsBinding"];
                        prPar.OnPropertyChanged("Value");
                    }
                    else if (testPar != null && tb.Values.Count > 0)
                    {
                        testPar.Value = tb.Values["EditorItemsBinding"].ToString();
                        testPar.OnPropertyChanged("Value");
                    }
                }
            }
        }

        public FormTab CopyTab()
        {
            TabItems[SelectedIndex].PrepareControlsByteList();

            var tab = AddTab(TabItems[SelectedIndex]);
            return tab;
        }

        public void SetPortData(Dictionary<string, PortParameter> formBindings)
        {
            PortPars.Clear();
            PortPars = formBindings;

        }


        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}