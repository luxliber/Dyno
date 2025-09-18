using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;
using Prorubim.DynoStudio;
using Prorubim.DynoStudio.Main;
using Prorubim.DynoStudio.Models;
using Prorubim.DynoStudio.ViewModels;

namespace Dyno.Forms
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        public string Version
        {
            get
            {
                var v = Assembly.GetExecutingAssembly().GetName().Version;
                return $"v {v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
        }

        private void DeleteButtonOnClick(object o, RoutedEventArgs e)
        {
            if (o is FrameworkElement sender && DynoManager.Instance.Settings.StorageFoldersZip.Contains(sender.Tag))
            {
                DynoManager.Instance.Settings.StorageFoldersZip.Remove(
                    DynoManager.Instance.Settings.GetItemByPath(((StorageFolderItem) sender.Tag).Path));
                DynoManager.Instance.Settings.OnPropertyChanged(nameof(DynoSettings.StorageFoldersZip));
                UpdateLayout();
            }
        }

        private void Browse_OnClick(object o, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (o is FrameworkElement fe)
                if (fe.Tag is StorageFolderItem item)
                {
                    dialog.SelectedPath = item.Path;

                    var result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        if (!DynoManager.Instance.Settings.Contains(item.Path)) return;

                        var ind = DynoManager.Instance.Settings.StorageFoldersZip.IndexOf(
                            DynoManager.Instance.Settings.GetItemByPath(item.Path));
                        DynoManager.Instance.Settings.StorageFoldersZip[ind].Path = dialog.SelectedPath;
                        DynoManager.Instance.Settings.StorageFoldersZip[ind]
                            .OnPropertyChanged(nameof(StorageFolderItem.Path));
                    }
                }
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;
            if (DynoManager.Instance.Settings.Contains(dialog.SelectedPath)) return;

            DynoManager.Instance.Settings.StorageFoldersZip.Add(new StorageFolderItem
            {
                Path = dialog.SelectedPath,
                Status = true
            });
            DynoManager.Instance.OnPropertyChanged(nameof(DynoSettings.StorageFoldersZip));
        }

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            //        DynoManager.Settings.WriteSettings(App.Instance.Mw);


            // DynoManager.Instance.RescanStorage();

            //Expand();
            Hide();
        }
    }
}