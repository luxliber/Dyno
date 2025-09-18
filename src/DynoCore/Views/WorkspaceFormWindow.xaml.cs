using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Dyno.Models.Forms;
using Dyno.Models.Workspaces;

namespace Dyno.Views
{
    public partial class WorkspaceFormWindow
    {
        public WorkspaceFormWindow(WorkspacePreset preset)
        {
            _form = WorkspaceForm.Load(preset.Workspace);

            _form.Workspace = preset.Workspace;
            _form.SelectedIndex = 0;
            _form.IsProduction = true;
            _form.Refresh();

            InitializeComponent();
            DataContext = preset.Workspace;

            FormTabControl.DataContext = _form;
            RunAndCloseButton.DataContext = _form;
            RunButton.DataContext = _form;

            Closed += WorkspaceFormWindow_Closed;
            StateChanged += OnStateChanged;

        }

        public WorkspaceFormWindow(WorkspacePreset preset, MemoryStream memoryStream, bool isAllTabs = false)
        {
            _form = WorkspaceForm.LoadFromStream(memoryStream);

            _form.Workspace = preset.Workspace;
            _form.SelectedIndex = 0;
            _form.IsVisibleAllTabs = isAllTabs;
            _form.IsProduction = true;
            _form.Refresh();

            Width = _form.TabItems[_form.SelectedIndex].Width + 16;
            Height = _form.TabItems[_form.SelectedIndex].Height + 122;

            InitializeComponent();
            DataContext = preset.Workspace;

            FormTabControl.DataContext = _form;
            RunAndCloseButton.DataContext = _form;
            RunButton.DataContext = _form;

            Closed += WorkspaceFormWindow_Closed;
            StateChanged +=OnStateChanged;
        }

        private void OnStateChanged(object sender, EventArgs eventArgs)
        {
            if (WindowState == WindowState.Maximized && !_form.EditorIsResizable)
            {
                WindowState = WindowState.Normal;
            }
        }

        public ObservableCollection<FormTab> TabItems => _form.ShownTabItems;

        public WorkspaceFormWindow(string formPath)
        {
            _form = WorkspaceForm.LoadFromPath(formPath);

            _form.Workspace = null;
            _form.SelectedIndex = 0;
            _form.IsProduction = true;
            _form.Refresh();

            InitializeComponent();

            FormTabControl.DataContext = _form;
            RunAndCloseButton.DataContext = _form;
            RunButton.DataContext = _form;

            Closed += WorkspaceFormWindow_Closed;
        }

        private void WorkspaceFormWindow_Closed(object sender, EventArgs e)
        {
            _form.IsProduction = false;
           
        }

        private readonly WorkspaceForm _form;
        internal bool IsSizeChanged;

        public WorkspaceFormWindow()
        {
            InitializeComponent();
        }

        private void FormTabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsSizeChanged) return;

            var tab = _form.TabItems.FirstOrDefault(x => x.Header == TabItems[_form.SelectedIndex].Header);

            Width = tab?.Width + 16 ?? 200;
            Height = tab?.Height + 122 ?? 200;
        }

       
    }
}
