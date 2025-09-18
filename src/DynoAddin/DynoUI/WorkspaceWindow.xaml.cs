using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Autodesk.Revit.UI;

namespace DynoUI
{
    /// <summary>
    /// Interaction logic for WorkspaceWindow.xaml
    /// </summary>
    public partial class WorkspaceWindow : IDockablePaneProvider
    {
        public WorkspaceWindow()
        {
            InitializeComponent();
            Update.Visibility = Visibility.Hidden;
           
          //  inner.SizeChanged += Inner_SizeChanged;
        }

       /*  private void Inner_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           var presentationSource = PresentationSource.FromVisual(inner);

            if (presentationSource?.CompositionTarget == null) return;

            var m = presentationSource.CompositionTarget.TransformToDevice;
            var dx = m.M11;
            var dy = m.M22;
           
            if (dx == 1.0f && dy == 1.0f) return;

            //master.Width = inner.Width * dx;
            //master.Height = inner.Height * dy;
            //master.HorizontalAlignment = HorizontalAlignment.Left;
            //master.VerticalAlignment = VerticalAlignment.Top;
        }*/

     
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Update.Visibility = Visibility.Collapsed;
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = inner;

            data.InitialState = new DockablePaneState();

            data.InitialState.DockPosition
              = DockPosition.Tabbed;

            data.InitialState.TabBehind = DockablePanes
              .BuiltInDockablePanes.ProjectBrowser;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Search.Text = "";
        }


    }
}