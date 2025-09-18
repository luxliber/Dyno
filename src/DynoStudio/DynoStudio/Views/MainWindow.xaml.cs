using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dyno;
using Dyno.FormControls;
using Dyno.Forms;
using Dyno.Forms.History;
using Dyno.Models.Forms;
using Dyno.Models.Workspaces;
using Dyno.ViewModels;
using Dyno.Views.FormControls;
using Prorubim.DynoStudio.Editor;
using Prorubim.DynoStudio.ViewModels;
using WPG;
using Button = System.Windows.Controls.Button;
using DragDropEffects = System.Windows.DragDropEffects;

namespace Prorubim.DynoStudio.Main
{
    public partial class MainWindow
    {
        private double _oldLeftSplitterWidth = 300;
        private double _oldRightSplitterWidth = 300;
        private double _oldCenterSplitterWidth = 0.2;

        private Point _startPoint;
        private bool _isDown;
        private bool _isDragging;
        private bool _isCtrl;
        private readonly EditorManager _editorVm;

        public MainWindow()
        {
            InitializeComponent();

            StorageTreeView.Loaded += (o, args) => ExpandTree();

            PropertyGrid.Filter = "Editor";

            AddHandler(KeyDownEvent, new KeyEventHandler(MainWindow_KeyDown), true);
            AddHandler(KeyUpEvent, new KeyEventHandler(MainWindow_KeyUp), true);

            PropertyGrid.PropertyChanged += PropertyGridOnPropertyChanged;

            _editorVm = EditorCanvas.DataContext as EditorManager;

            if (!(DataContext is ViewModels.MainViewModel mVm)) return;
            mVm.Mw = this;

            var presetArg = Environment.GetCommandLineArgs().FirstOrDefault(x => x.StartsWith("preset="));

            //check startup arguments and select workspace if needded
            if (string.IsNullOrEmpty(presetArg)) return;
            presetArg = presetArg.Replace("preset=", "");
            var w = DynoManager.Instance.Root.GetWorkspaceByInnerName(presetArg);
            if (w != null)
                mVm.SelectWorkspaceCommand.Execute(w);
        }

        internal void PropertyGridOnPropertyChanged(object sender, CurPropertyChangedEventHandlerArgs e)
        {
            var propertyName = e.SelectedProperty.OriginalName;

            if (_editorVm.SelectedElement != null && _editorVm.SelectedElements.Count > 1)
            {
                var prs = TypeDescriptor.GetProperties(_editorVm.SelectedElement,
                    new Attribute[] { new PropertyFilterAttribute(PropertyFilterOptions.All) });

                if (propertyName == "Left" || propertyName == "Top")
                    _editorVm.SyncValueForSelectedElements("Canvas.", prs, propertyName);
                else
                    _editorVm.SyncValueForSelectedElements("", prs, propertyName);
            }

            DynoManager.SelectedHistory.AddAction(new HistoryPropChanging(e.SelectedProperty.Instance, e.SelectedProperty.Prop, e.SelectedProperty.OldValue, e.SelectedProperty.Value));
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl) _isCtrl = false;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl) _isCtrl = true;
        }

        public void ExpandTree()
        {
            foreach (var item in StorageTreeView.Items)
            {
                var itm = (TreeViewItem)StorageTreeView.ItemContainerGenerator.ContainerFromItem(item);
                if (itm == null) continue;
                itm.IsExpanded = true;
            }
        }

        private void ToolButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Button)) return;

            var draggedItem = (Button)sender;
            DragDrop.DoDragDrop(draggedItem, draggedItem, DragDropEffects.Move);
        }

        private void PresetComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PresetCombobox.SelectedIndex == -1)
                return;

            var pr = PresetCombobox.SelectedItem as WorkspacePreset;
            if (pr == null || DynoManagerBase.SelectedWorkspacePreset == pr) return;

            DynoManager.SelectWorkspacePreset(pr);
        }

        private void LeftSplitter_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainGrid.ColumnDefinitions[0].Width.Value > 1)
            {
                _oldLeftSplitterWidth = MainGrid.ColumnDefinitions[0].Width.Value;
                MainGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
            }
            else
            {
                MainGrid.ColumnDefinitions[0].Width = new GridLength(_oldLeftSplitterWidth, GridUnitType.Pixel);
                _oldLeftSplitterWidth = 0;
            }
        }

        private void MainTabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (MainTabControl.SelectedIndex == 0 && DynoManagerBase.SelectedWorkspacePreset != null && DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm != null)
            {
                FormEditorTools.Visibility = Visibility.Visible;
                BackScrollViewer.Visibility = Visibility.Visible;

                CreateFormPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                FormEditorTools.Visibility = Visibility.Collapsed;
                BackScrollViewer.Visibility = Visibility.Collapsed;

                CreateFormPanel.Visibility = Visibility.Visible;
            }*/

            
        }

        private void FormTabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm == null) return;

            DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.OnPropertyChanged(nameof(WorkspaceForm.Width));
            DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.OnPropertyChanged(nameof(WorkspaceForm.Height));

            var currentTab = ((TabControl)sender).SelectedItem as FormTab;

            _editorVm.UnselectAll(false);
            _editorVm.PropertyGridItem = currentTab;
            var control = Utils.Utils.FindVisualChild((TabControl)sender, "FormCanvas");
            _editorVm.Select(control);

        }

        private void RightSplitter_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainGrid.ColumnDefinitions[6].Width.Value > 0)
            {
                _oldRightSplitterWidth = MainGrid.ColumnDefinitions[6].Width.Value;
                MainGrid.ColumnDefinitions[6].Width = new GridLength(0, GridUnitType.Pixel);
            }
            else
            {
                MainGrid.ColumnDefinitions[6].Width = new GridLength(_oldRightSplitterWidth, GridUnitType.Pixel);
                _oldRightSplitterWidth = 0;
            }
        }

        private void CenterSplitter_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainGrid.ColumnDefinitions[4].Width.Value > 0)
            {
                _oldCenterSplitterWidth = MainGrid.ColumnDefinitions[4].Width.Value;
                MainGrid.ColumnDefinitions[4].Width = new GridLength(0, GridUnitType.Star);
            }
            else
            {
                MainGrid.ColumnDefinitions[4].Width = new GridLength(_oldCenterSplitterWidth, GridUnitType.Star);
                _oldCenterSplitterWidth = 0;
            }
        }

        private void MainWindow_OnMouseLeave(object sender, MouseEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDown) return;

            if (_isDragging == false &&
                (Math.Abs(e.GetPosition(EditorCanvas).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(e.GetPosition(EditorCanvas).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance))
                _isDragging = true;

            if (!_isDragging) return;

            var position = Mouse.GetPosition(EditorCanvas);

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

            _editorVm.TranslateSelectedElements(tx, ty, false);
        }

        private void MainWindow_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var el = e.Source as FrameworkElement;
            if (el == null) return;

            switch (el.Name)
            {
                case "BackScrollViewer":
                    //             if (!_isCtrl) _editorVm.UnselectAll();
                    break;

                case "HeaderBorder":
                    _editorVm.UnselectAll(false);
                    _editorVm.PropertyGridItem = DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm;
                    break;

                case "FormTabControl":
                    FrameworkElement control = null;

                    var currentTab = ((TabControl)el).SelectedItem as FormTab;
                    if (currentTab != null)
                        foreach (var item in currentTab.Items)
                            if (item.IsMouseOver)
                            {
                                control = item;
                                break;
                            }

                    if (control != null)
                    {
                        if (!_editorVm.SelectedElements.Contains((FormControl)control))
                        {
                            if (!_isCtrl)
                                _editorVm.UnselectAll(false);
                            _editorVm.Select(control);
                            _editorVm.PropertyGridItem = control;
                        }

                        foreach (var els in _editorVm.SelectedElements)
                            els.SaveOldBounds();

                        var window = GetWindow(EditorCanvas);
                        if (window != null)
                        {
                            var back = window.FindName("BackScrollViewer") as UIElement;
                            back?.Focus();
                        }
                        _isDown = true;

                        e.Handled = true;
                    }
                    else
                    {
                        _editorVm.UnselectAll(false);
                        _editorVm.PropertyGridItem = currentTab;
                        control = Utils.Utils.FindVisualChild(el, "FormCanvas");
                        _editorVm.Select(control);
                    }
                    break;
            }

            _startPoint = Mouse.GetPosition(EditorCanvas);
        }

        private void MainWindow_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is PropertyGrid) return;
            StopDragging();
        }

        private void StopDragging()
        {
            if (!_isDown) return;
            _isDown = false;

            if (_isDragging)
                _editorVm.AddMoveActionForAllSelectedElements();

            _isDragging = false;
            _editorVm.OnPropertyChanged(nameof(EditorManager.PropertyGridItem));
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e) => ViewModels.MainViewModel.Settings.WriteSettings(this);

        private void BackScrollViewer_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
            var form = _editorVm.WorkspaceForm;


            form.Zoom += e.Delta * 0.001;

            if (form.Zoom < 0.3)
                form.Zoom = 0.3;

            if (form.Zoom > 3)
                form.Zoom = 3;
        }

        private void BindingsDataGrid_OnCurrentCellChanged(object sender, EventArgs e)
        {
            _editorVm.OnPropertyChanged(nameof(EditorManager.PropertyGridItem));
        }
    }
}
