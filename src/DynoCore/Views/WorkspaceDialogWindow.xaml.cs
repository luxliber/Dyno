using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Autodesk.Windows;
using Dyno.Models.Forms;

namespace Dyno.Views
{
    public partial class WorkspaceDialogWindow
    {
        internal bool IsSizeChanged=false;

        public WorkspaceDialogWindow(string title, WorkspaceForm form, Dictionary<string, PortParameter> formBindings, IList tabNames)
        {
            _form = form;
            _form.PortTabNames = tabNames;
            _form.SetPortData(formBindings);
            _form.IsProduction = true;
            
            _form.Refresh();

            InitializeComponent();

            Title = title;
            DataContext = _form;
            
            FormTabControl.SelectedIndex = 0;

            CancelButton.Click += CancelButton_Click;
          
            Closed += WorkspaceFormWindow_Closed;
            StateChanged += OnStateChanged;

            // ReSharper disable once UseObjectOrCollectionInitializer
            var windowInteropHelper = new WindowInteropHelper(this);
            windowInteropHelper.Owner = ComponentManager.ApplicationWindow;
        }

        private void OnStateChanged(object sender, EventArgs eventArgs)
        {
            if (WindowState == WindowState.Maximized && !_form.EditorIsResizable)
            {
                WindowState = WindowState.Normal;
            }
        }

       

       

        private void WorkspaceFormWindow_Closed(object sender, EventArgs e)
        {
            _form.IsProduction = false;
        }

        void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _form.IsProduction = false;
            Close();

        }

        private readonly WorkspaceForm _form;

        public WorkspaceDialogWindow()
        {
            InitializeComponent();
        }

        private void FormTabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsSizeChanged) return;

            var tab = _form.ShownTabItems.FirstOrDefault(x => x.Header == _form.ShownTabItems[FormTabControl.SelectedIndex].Header);

            Width = tab?.Width + 16 ?? 200;
            Height = tab?.Height + 137 ?? 200;
        }


    }
}
