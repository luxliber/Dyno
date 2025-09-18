using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Dyno.External;
using Dyno.Manager;
using Dyno.Models;
using Dyno.Models.Workspaces;
using Dyno.ViewModels;
using DynoUI;
using LitJson;
using Prorubim.DynoRevitCore;
using static System.Windows.PresentationSource;
using Application = System.Windows.Application;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.MessageBox;
using Visibility = System.Windows.Visibility;

namespace Dyno
{
    [Transaction(TransactionMode.Manual),
     Regeneration(RegenerationOption.Manual)]
    public class DynoApp : DynoAppBase
    {
        private static readonly string DynoAssemblyName = Assembly.GetExecutingAssembly().Location;

        internal static PushButton DynoPaneButton;

        internal static WorkspaceWindow MainWindow;
        internal static SettingsWindow SettingsWindow;

        private static DockablePaneId _dynoPaneId;
        private static DockablePane _dynoPane;
        internal DynoManager DynoManager;

        public override DynoSettingsBase GetSettings() => DynoManager.Settings;
        public override DynoManagerBase GetManager() => DynoManager;
        internal static DynoApp GetApp() => (DynoApp) Instance;


        private readonly object _ribbonName = "Dyno";

        public override Result OnStartup(UIControlledApplication application)
        {
            base.OnStartup(application);
            DynoManager = new DynoManager();

            DynoManager.InitExternalEvents();
            Instance = this;

            UiControlledApp.DockableFrameVisibilityChanged += Application_DockableFrameVisibilityChanged;

            CreateRibbonButton();

            try
            {
                SettingsWindow = new SettingsWindow {DataContext = DynoManager.Settings};
                SettingsWindow.FolderRemove += (o, args) =>
                {
                    if (!(o is FrameworkElement sender) ||
                        !DynoManager.Settings.Contains(sender.Tag.ToString())) return;
                    DynoManager.Settings.StorageFoldersZip.Remove(
                        DynoManager.Settings.GetItemByPath(sender.Tag.ToString()));
                    DynoManager.Settings.OnPropertyChanged(nameof(DynoSettings.StorageFoldersZip));
                    SettingsWindow.UpdateLayout();
                };

                SettingsWindow.FolderIsChanged += (o, args) =>
                {
                    if (!(((FrameworkElement) o).Tag is StorageFolderItem p) ||
                        !DynoManager.Settings.Contains(p.Path)) return;

                    var ind = DynoManager.Settings.StorageFoldersZip.IndexOf(
                        DynoManager.Settings.GetItemByPath(p.Path));
                    DynoManager.Settings.StorageFoldersZip[ind].Path = ((FolderChangedEventArgs) args).NewFolder;
                    DynoManager.Settings.StorageFoldersZip[ind].OnPropertyChanged(nameof(StorageFolderItem.Path));
                };

                SettingsWindow.AddButton.Click += (sender, args) =>
                {
                    var dialog = new FolderBrowserDialog();

                    var result = dialog.ShowDialog();
                    if (result != DialogResult.OK) return;
                    if (DynoManager.Settings.Contains(dialog.SelectedPath)) return;

                    DynoManager.Settings.StorageFoldersZip.Add(new StorageFolderItem
                    {
                        Path = dialog.SelectedPath,
                        Status = true
                    });
                    GetSettings().OnPropertyChanged(nameof(DynoSettings.StorageFoldersZip));
                };
                SettingsWindow.ApplyButton.Click += (sender, args) =>
                {
                    DynoManager.ScanFileStorage();
                    Expand();
                    SettingsWindow.Hide();
                };

                //Creating panel
                MainWindow = new WorkspaceWindow();
                CreateMainWindow(MainWindow);

                _dynoPaneId = new DockablePaneId(new Guid("{5bb22324-3d0f-4aae-98de-30845e882f85}"));
                UiControlledApp.RegisterDockablePane(_dynoPaneId, "Dyno Browser", MainWindow);

                application.ViewActivated += Application_ViewActivated;
                application.ControlledApplication.DocumentOpened += ControlledApplication_DocumentOpened;
                application.ControlledApplication.DocumentClosing += ControlledApplication_DocumentClosing;
                application.ControlledApplication.DocumentCreated += ControlledApplication_DocumentCreated;

                DynoManager.WriteWorkspaces();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return Result.Failed;
            }
        }

        public static void Expand()
        {
            ExpandSubContainers(MainWindow.Tv);
            FilterTree(MainWindow.Tv.Items, MainWindow.Tv.ItemContainerGenerator);
        }

        private static void ExpandSubContainers(ItemsControl parentContainer)
        {
            foreach (var item in parentContainer.Items)
            {
                if (!(parentContainer.ItemContainerGenerator
                        .ContainerFromItem(item) is TreeViewItem currentContainer) ||
                    currentContainer.Items.Count <= 0) continue;

                if (item is WorkspaceGroup &&
                    GetApp().DynoManager.Settings.ExpandedKnots.Contains((item as WorkspaceGroup).XPath))
                    currentContainer.IsExpanded = true;

                if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    currentContainer.ItemContainerGenerator.StatusChanged += delegate
                    {
                        ExpandSubContainers(currentContainer);
                    };
                else
                    ExpandSubContainers(currentContainer);
            }
        }

        public static void SaveExpanded()
        {
            GetApp().DynoManager.Settings.ExpandedKnots.Clear();
            SaveExpandedLevel(MainWindow.Tv.Items, MainWindow.Tv.ItemContainerGenerator);
        }

        public static void SaveExpandedLevel(IList items, ItemContainerGenerator itemContainerGenerator)
        {
            foreach (var item in items)
            {
                var itm = (TreeViewItem) itemContainerGenerator.ContainerFromItem(item);
                if (itm == null) continue;

                if (item is WorkspaceGroup)
                {
                    var wg = item as WorkspaceGroup;
                    if (itm.IsExpanded && !GetApp().DynoManager.Settings.ExpandedKnots.Contains(wg.XPath))
                        GetApp().DynoManager.Settings.ExpandedKnots.Add(wg.XPath);
                }

                SaveExpandedLevel(itm.Items, itm.ItemContainerGenerator);
            }
        }

        private void Application_DockableFrameVisibilityChanged(object sender,
            DockableFrameVisibilityChangedEventArgs e)
        {
            if (e.PaneId == _dynoPaneId && e.DockableFrameShown == false)
                DynoManager.Settings.WindowShowing = false;
        }

        internal void CreateMainWindow(WorkspaceWindow mw)
        {
            mw.Search.TextChanged += SearchOnTextChanged;

            mw.inner.SizeChanged += Inner_SizeChanged;
            mw.inner.Loaded += Inner_Loaded;
            var resourceLocater = new Uri("/Dyno;component/Resources/MainWindowRes.xaml", UriKind.Relative);
            var dic = (ResourceDictionary) Application.LoadComponent(resourceLocater);

            mw.Tv.ItemContainerStyle = (Style) dic["ItemMouseDoubleClick"];

            mw.Tv.Style = (Style) dic["TreeViewMenu"];

            mw.Download.Style = (Style) dic["Download"];
            mw.Download.Click += Download_Click;


            mw.DataContext = DynoManager;
            mw.Search.Text = DynoManager.Settings.Filter;
        }

        private static void Download_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://content.prorubim.com/materials/dyno"));
            e.Handled = true;
        }

        private static void Inner_Loaded(object sender, RoutedEventArgs e) => CheckPaneScaledSize();
        private static void Inner_SizeChanged(object sender, SizeChangedEventArgs e) => CheckPaneScaledSize();

        private static void CheckPaneScaledSize()
        {
            var ver = WinMajorVersion;

            if (ver != 6) return;

            var m = FromVisual(MainWindow.inner).CompositionTarget.TransformToDevice;
            var dx = m.M11;
            var dy = m.M22;
            if (dx == 1.0f && dy == 1.0f) return;

            MainWindow.Master.Width = MainWindow.inner.Width / dx;
            MainWindow.Master.Height = MainWindow.inner.Height / dy;
            MainWindow.Master.HorizontalAlignment = HorizontalAlignment.Left;
            MainWindow.Master.VerticalAlignment = VerticalAlignment.Top;
        }

        private static void SearchOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            GetApp().DynoManager.Settings.Filter = MainWindow.Search.Text;
            FilterTree(MainWindow.Tv.Items, MainWindow.Tv.ItemContainerGenerator);
        }

        internal static bool FilterTree(ItemCollection items, ItemContainerGenerator gen, bool forceVisible = false)
        {
            var isVisible = false;

            foreach (var item in items)
            {
                var itm = (TreeViewItem) gen.ContainerFromItem(item);
                if (itm == null) continue;

                if (item is WorkspaceGroup)
                {
                    var wg = item as WorkspaceGroup;

                    var forceChildsVisible = !IsFilterApply(wg.Name);

                    var isChildsVisible = FilterTree(itm.Items, itm.ItemContainerGenerator, forceChildsVisible);

                    if (isChildsVisible)
                    {
                        isVisible = true;
                        itm.Visibility = Visibility.Visible;
                    }
                    else
                        itm.Visibility = Visibility.Collapsed;
                }

                if (item is WorkspacePreset)
                {
                    var workspacePreset = item as WorkspacePreset;

                    if (IsFilterApply(workspacePreset.InnerName)
                        && IsFilterApply(workspacePreset.Workspace.Name)
                        && !forceVisible)
                        itm.Visibility = Visibility.Collapsed;
                    else
                    {
                        itm.Visibility = Visibility.Visible;
                        isVisible = true;
                    }
                }
            }

            return forceVisible || isVisible;
        }

        private static bool IsFilterApply(string name)
        {
            if (MainWindow.Search.Text == string.Empty)
                return false;

            var splitted = MainWindow.Search.Text.ToLower().Split(',');
            var res = true;
            foreach (var s in splitted)
                if (name.ToLower().Contains(s.Trim()))
                    res = false;

            return res;
        }

        private void CreateRibbonButton()
        {
            var addinRibbonPanel = UiControlledApp.CreateRibbonPanel("Dyno");

            DynoPaneButton =
                (PushButton)
                addinRibbonPanel.AddItem(
                    new PushButtonData(
                        "Dyno Browser",
                        "Dyno Browser",
                        DynoAssemblyName,
                        "Dyno.DynoStart"));

            var dynoIcon = Properties.Resources.logo_button;

            var bitmapSource =
                Imaging.CreateBitmapSourceFromHBitmap(
                    dynoIcon.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            var dynoIconLarge = Properties.Resources.logo_button_large;

            var bitmapSourceLarge =
                Imaging.CreateBitmapSourceFromHBitmap(
                    dynoIconLarge.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            DynoPaneButton.LargeImage = bitmapSourceLarge;
            DynoPaneButton.Image = bitmapSource;

            //copy button to visual programming panel
            var ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            var panel = ribbon.FindTab("Manage").Panels.FirstOrDefault(x => x.Source.Id == "visualprogramming_shr");
            if (panel == null) return;

            var panel1 = ribbon.FindTab("Add-Ins").Panels.FirstOrDefault(x => x.Source.AutomationName == "Dyno");
            if (panel1 == null) return;

            panel.Source.Items.Add(panel1.Source.Items[0].Clone());

            UpdateRibbonButtonsVisibility();
        }

        internal void UpdateRibbonButtonsVisibility()
        {
            var ribbon = Autodesk.Windows.ComponentManager.Ribbon;
            var panel = ribbon.FindTab("Manage").Panels.FirstOrDefault(x => x.Source.Id == "visualprogramming_shr");
            if (panel == null) return;

            var panel1 = ribbon.FindTab("Add-Ins").Panels.FirstOrDefault(x => x.Source.AutomationName == "Dyno");
            if (panel1 == null) return;

            if (!DynoManager.Settings.IsShowDynoAddinButton && !DynoManager.Settings.IsShowDynoManageButton)
                DynoManager.Settings.IsShowDynoManageButton = true;

            if (!DynoManager.Settings.IsShowDynoAddinButton)
                panel1.IsVisible = false;
            else
                panel1.IsVisible = true;

            if (!DynoManager.Settings.IsShowDynoManageButton)
                panel.Source.Items.Last().IsVisible = false;
            else
                panel.Source.Items.Last().IsVisible = true;

            var playerItem = panel.Source.Items.FirstOrDefault(x => x.Id == "ID_PLAYLIST_DYNAMO");
            if (playerItem != null) playerItem.IsVisible = !DynoManager.Settings.IsHidePlayerButton;
        }

        public static void UnlockUi() => MainWindow.Tv.IsEnabled = true;
        public static void LockUi() => MainWindow.Tv.IsEnabled = false;

        private void ControlledApplication_DocumentCreated(object sender,
            Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
            _dynoPane = UiControlledApp.GetDockablePane(_dynoPaneId);

            if (DynoManager.Settings.IsCheckUpdates)
                CheckUpdates(false);
        }

        internal static async void CheckUpdates(bool forceNoUpdatesShowing = true)
        {
            try
            {
                await XmlRpcClient.CheckInternetConnection();
                using (var client = new XmlRpcClient(new SiteConfig {BaseUrl = "https://content.prorubim.com"}))
                {
                    string res;
                    try
                    {
                        res = client.XmlRpcService.GetItemInfo("dyno");
                    }
                    catch (WebException e)
                    {
                        throw new Exception($"Server link error: {e.Message}");
                    }
                    catch (Exception e1)
                    {
                        throw new Exception($"Login or password is incorrect: {e1.Message}");
                    }

                    var jreader = new JsonReader(res) {AllowComments = true};
                    var jdata = JsonMapper.ToObject(jreader);

                    if (!jdata.Keys.Contains("version")) throw new Exception($"Server answer error");

                    var siteVer = Version.Parse((string) jdata["version"]);
                    var actualVer = Assembly.GetExecutingAssembly().GetName().Version;

                    if (siteVer > actualVer)
                    {
                        MainWindow.UpdateTextBox.Content = $"A new version {siteVer.ToString(3)} is avaliable";
                        MainWindow.Update.Visibility = Visibility.Visible;
                        MainWindow.Download.Visibility = Visibility.Visible;
                    }
                    else if (forceNoUpdatesShowing)
                    {
                        MainWindow.UpdateTextBox.Content = "Updates have been not found";
                        MainWindow.Update.Visibility = Visibility.Visible;
                        MainWindow.Download.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private void ControlledApplication_DocumentClosing(object sender,
            Autodesk.Revit.DB.Events.DocumentClosingEventArgs e)
        {
            SaveExpanded();

            if (DynoManager.Root.WorkspaceGroups.Select(x => x.Name).Contains(e.Document.Title))
                DynoManager.Root.WorkspaceGroups.Remove(
                    DynoManager.Root.WorkspaceGroups.First(x => x.Name == e.Document.Title));
        }

        private void ControlledApplication_DocumentOpened(object sender,
            Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
            _dynoPane = UiControlledApp.GetDockablePane(_dynoPaneId);

            if (DynoManager.Settings.IsCheckUpdates)
                CheckUpdates(false);

            var path = Path.GetDirectoryName(e.Document.PathName);
            var prName = e.Document.Title;
            DynoManager.ScanProjectScripts(path, prName);
            Expand();
        }

        internal void Application_ViewActivated(object sender, ViewActivatedEventArgs e)
        {
            if (!DynoStart.IsFirstStart) return;
            try
            {
                var id = RevitCommandId.LookupCommandId("CustomCtrl_%CustomCtrl_%Add-Ins%" + _ribbonName +
                                                        "%Dyno Browser");
                UiApp.PostCommand(id);
            }
            catch
            {
                // ignored
            }
        }

        public override Result OnShutdown(UIControlledApplication application)
        {
            DynoManager.Settings.WriteSettings(MainWindow);
            return base.OnShutdown(application);
        }

        public void WindowHide()
        {
            _dynoPane.Hide();
            DynoManager.Settings.WindowShowing = false;
        }

        public void WindowShow()
        {
            _dynoPane.Show();
            DynoManager.Settings.WindowShowing = true;
        }
    }
}