using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Dyno.Models.Parameters;
using Dyno.Models.Workspaces;
using DynoUI;
using Prorubim.DynoRevitCore.ViewModels;
using Button = System.Windows.Controls.Button;
using Clipboard = System.Windows.Clipboard;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;

namespace Dyno.Resources
{
    partial class TreeViewResource
    {
        public void TreeViewItemOnItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (!((TreeViewItem)sender).IsSelected)
                return;

            var treeItem = (TreeViewItem)sender;

            var item = treeItem.Header;

            if (item.GetType() == typeof(WorkspaceGroup))
            {
                var wg = item as WorkspaceGroup;
                DynoApp.MainWindow.Search.Text += wg.Name;
                DynoApp.MainWindow.Search.CaretIndex = int.MaxValue;
                DynoApp.MainWindow.Search.Focus();
            }
            else if (item.GetType() == typeof(WorkspacePreset))
            {
                DynoRevitManagerBase.Evaluate((WorkspacePreset)item);
                e.Handled = true;
            }
            else if (item.GetType() == typeof(WorkspacePlaylist))
            {
                DynoRevitManagerBase.Evaluate((WorkspacePlaylist)item);
                e.Handled = true;
            }
            else if (item.GetType().BaseType == typeof(WorkspaceParameter))
            {
                var header = (ContentPresenter)treeItem.Template.FindName("PART_Header", treeItem);

                var content = header.ContentTemplate.FindName("parameter", header) as FrameworkElement;
                var point = content.PointToScreen(new Point());

                var source = PresentationSource.FromVisual(content);

                var dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
                var dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;

                point.X = point.X * 96.0 / dpiX;
                point.Y = point.Y * 96.0 / dpiY;

                if (item.GetType() == typeof(DropDownParameter))
                {
                    var par = item as DropDownParameter;

                    Manager.DynoManager.DropDownExternalHandler.Par = par;
                    Manager.DynoManager.DropDownExternalHandler.Point = point;
                    Manager.DynoManager.DropDownExternalEvent.Raise();
                }
                else if (item.GetType() == typeof(StringParameter))
                {
                    var par = item as StringParameter;
                    var window = new StringWindow
                    {
                        Values = par.Values,
                        ValueText = par.Value.ToString(),
                        Left = point.X,
                        Top = point.Y,
                        Desc = par.Desc,
                        Title = par.Name
                    };
                    if (window.ShowDialog() == true)
                    {
                        par.Workspace.IsChanged = true;
                        par.Value = window.ValueText;
                        par.Workspace.OnPropertyChanged("IsChanged");
                    }

                    par.OnPropertyChanged("Value");
                }
                else if (item.GetType() == typeof(NumberParameter))
                {
                    var par = item as NumberParameter;
                    double.TryParse(par.Value.ToString(), out var dval);

                    var window = new StringWindow
                    {
                        Values = par.Values,
                        ValueText = dval.ToString(CultureInfo.InvariantCulture),
                        Left = point.X,
                        Top = point.Y,
                        Desc = par.Desc,
                        Title = par.Name
                    };
                    if (window.ShowDialog() == true)
                    {
                        par.Workspace.IsChanged = true;
                        par.Value = window.ValueText;
                        par.Workspace.OnPropertyChanged("IsChanged");
                    }

                    par.OnPropertyChanged("Value");
                }
                else if (item.GetType() == typeof(IntSliderParameter))
                {
                    var par = item as IntSliderParameter;
                    var window = new IntSliderWindow
                    {
                        DataContext = par,
                        IntValue = int.Parse(par.Value.ToString()),
                        Left = point.X,
                        Top = point.Y,
                        Title = par.Name,
                        Min = par.Min,
                        Max = par.Max
                    };
                    if (window.ShowDialog() == true)
                    {
                        par.Workspace.IsChanged = true;
                        par.Value = window.IntValue.ToString();
                        par.Workspace.OnPropertyChanged("IsChanged");
                    }

                    par.OnPropertyChanged("Value");
                }
                else if (item.GetType() == typeof(SelectElementParameter))
                {
                    DynoApp.LockUi();
                    var par = item as SelectElementParameter;

                    DynoApp.GetApp().DynoManager.SelectElements(par);
                    DynoApp.UnlockUi();
                }
                else if (item.GetType() == typeof(SelectReferenceParameter))
                {
                    DynoApp.LockUi();
                    var par = item as SelectReferenceParameter;
                    DynoApp.GetApp().DynoManager.SelectReference(par);
                    DynoApp.UnlockUi();
                }
                else if (item.GetType() == typeof(PathParameter))
                {
                    var par = item as PathParameter;
                    DynoApp.GetApp().DynoManager.SelectFile(par);
                }
                else if (item.GetType() == typeof(BooleanParameter))
                {
                    var par = item as BooleanParameter;

                    if (par.FastMode)
                    {
                        par.Workspace.IsChanged = true;
                        par.Value = (!par.AsBoolean()).ToString();
                        par.Workspace.OnPropertyChanged("IsChanged");
                    }
                    else
                    {
                        var window = new BooleanWindow(par.AsBoolean(), par.FalseText, par.TrueText)
                        {
                            Left = point.X,
                            Top = point.Y,
                            Desc = par.Desc,
                            Title = par.Name
                        };
                        if (window.ShowDialog() == true)
                        {
                            par.Workspace.IsChanged = true;
                            par.Value = window.TrueButton.IsChecked.ToString();
                            par.Workspace.OnPropertyChanged("IsChanged");
                        }
                    }

                    par.OnPropertyChanged("Value");
                }
                else if (item.GetType() == typeof(OutputParameter))
                {
                    var par = item as OutputParameter;
                    Clipboard.SetText(par.Value.ToString());
                }
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void OnRescanStorageClick(object sender, RoutedEventArgs e)
        {
            DynoApp.GetApp().DynoManager.ScanFileStorage();
            DynoApp.Expand();
        }

        private void OnOpenWorkspaceClick(object sender, RoutedEventArgs e)
        {
            var ws = DynoApp.MainWindow.Tv.SelectedItem as WorkspacePreset;

            Manager.DynoManager.OpenInDynamo(ws.Workspace.WorkspacePath);
        }

        private void TreeViewItemOnPreviewMouseRightButtonDown(object sender, MouseEventArgs e)
        {
            var item = VisualUpwardSearch(e.OriginalSource as DependencyObject);
            if (item != null)
            {
                item.Focus();
                e.Handled = true;
            }
        }

        public static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }

        private void OnDownloadClick(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            if (b.Name == "download")
            {
                Process.Start("http://dyno.parseapp.com/download");
            }
        }


        private void OnOpenStorageClick(object sender, RoutedEventArgs e)
        {
            foreach (var storageFolder in DynoApp.GetApp().DynoManager.Settings.StorageFoldersZip)
                if (storageFolder.Status)
                    Process.Start(storageFolder.Path);
        }

        private void OnOpenWorkspaceInEditorClick(object sender, RoutedEventArgs e)
        {
            var ws = DynoApp.MainWindow.Tv.SelectedItem as WorkspacePreset;

            var fileName = Path.GetExtension(ws.Workspace.WorkspacePath).ToLower();

            if (fileName == ".dyn")
            {
                fileName = Path.ChangeExtension(ws.Workspace.WorkspacePath, "dpr");

                if (fileName == null) return;
                if (File.Exists(fileName))
                    try
                    {
                        Process.Start(fileName);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Presets file has been created but Dyno can`t open it.\nProbably you need to make association for DPR file extension\n with any text editor in your Windows system.");
                    }
                else
                {
                    using (var fs = File.CreateText(fileName))
                    {
                        var str = @"{
    ""presets"" : {
        ""<name>"":{
            ""forceReopen"":false
        }
    }
}".Replace("<name>", Path.GetFileNameWithoutExtension(fileName));

                        fs.Write(str);
                    }

                    try
                    {
                        Process.Start(fileName);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Presets file has been created but Dyno can`t open it.\nProbably you need to make association for DPR file extension\n with any text editor in your Windows system.");
                    }
                }
            }
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var treeItem = (TreeViewItem)sender;
            var item = treeItem.Header;

            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.LeftCtrl) ||
                Keyboard.IsKeyDown(Key.LeftShift))
            {
                if (item.GetType() == typeof(WorkspaceGroup))
                {
                    var wg = item as WorkspaceGroup;
                    DynoApp.MainWindow.Search.Text += wg.Name;
                    DynoApp.MainWindow.Search.CaretIndex = int.MaxValue;
                    DynoApp.MainWindow.Search.Focus();
                }
                else if (item.GetType() == typeof(WorkspacePreset))
                {
                    var wp = item as WorkspacePreset;
                    DynoApp.MainWindow.Search.Text += wp.InnerName;
                    DynoApp.MainWindow.Search.CaretIndex = int.MaxValue;
                    DynoApp.MainWindow.Search.Focus();
                }
            }
            else
            {
                if (e.OriginalSource.GetType() != typeof(TextBlock)) return;
                var b = (TextBlock)e.OriginalSource;

                if (b.Name == "val")
                    TreeViewItemOnItemMouseDoubleClick(sender, e);
            }

            e.Handled = true;
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            DynoApp.SettingsWindow.ShowDialog();
            DynoApp.GetApp().UpdateRibbonButtonsVisibility();
        }

        private void OnCheckUpdates(object sender, RoutedEventArgs e) => DynoApp.CheckUpdates();

        private void OnEditWorkspaceFormClick(object sender, RoutedEventArgs e)
        {
            //            var path = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var path1 = Path.Combine(path, "DynoStudio");
            var path2 = Path.Combine(path1, "DynoStudio.exe");

            if (File.Exists(path2))
            {
                var startInfo = new ProcessStartInfo { FileName = path2 };
                var ws = DynoApp.MainWindow.Tv.SelectedItem as WorkspacePreset;
                var arg = $@"preset=""{ws.Workspace.Name}""";
                startInfo.Arguments = arg;
                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show("Dyno Studio is not installed", "Dyno");
            }
        }

        private void OnOpenButtonsFileClick(object sender, RoutedEventArgs e)
        {
            foreach (var storageFolder in DynoApp.GetApp().DynoManager.Settings.StorageFoldersZip)
                if (storageFolder.Status)
                {
                    var str = Path.Combine(storageFolder.Path, "buttons.txt");

                    if (File.Exists(str))
                        Process.Start(str);
                    else
                    {
                        using (var fs = File.CreateText(str))
                        {
                        }

                        Process.Start(str);
                    }
                }
        }

        private void EventSetter_OnHandler(object sender, RoutedEventArgs e)
        {
            DynoApp.Expand();
        }


        private void TreeViewItemOnExpanded(object sender, RoutedEventArgs e)
        {
            var treeItem = (TreeViewItem)sender;
            var item = treeItem.Header;

            if (item.GetType() != typeof(WorkspaceGroup)) return;

            var wg = (WorkspaceGroup)item;
            if (!DynoApp.GetApp().DynoManager.Settings.ExpandedKnots.Contains(wg.XPath))
                DynoApp.GetApp().DynoManager.Settings.ExpandedKnots.Add(wg.XPath);
        }

        private void TreeViewItemOnCollapsed(object sender, RoutedEventArgs e)
        {
            var treeItem = (TreeViewItem)sender;
            var item = treeItem.Header;

            if (item.GetType() != typeof(WorkspaceGroup)) return;

            var wg = (WorkspaceGroup)item;
            if (DynoApp.GetApp().DynoManager.Settings.ExpandedKnots.Contains(wg.XPath))
                DynoApp.GetApp().DynoManager.Settings.ExpandedKnots.Remove(wg.XPath);
        }

        private void OnRunInSilentMode(object sender, RoutedEventArgs e)
        {
            if (!(DynoApp.MainWindow.Tv.SelectedItem is WorkspacePreset ws)) return;

            DynoRevitManagerBase.IsSilentMode = true;
            DynoRevitManagerBase.Evaluate(ws);
        }
    }
}