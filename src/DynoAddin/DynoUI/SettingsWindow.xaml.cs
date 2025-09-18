using System;
using System.Windows;
using System.Windows.Forms;

namespace DynoUI
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        public event EventHandler FolderRemove;
        protected virtual void OnFolderRemove(EventArgs e, FrameworkElement sender) => FolderRemove?.Invoke(sender, e);

        public event EventHandler FolderIsChanged;
        protected virtual void OnFolderIsChanged(FolderChangedEventArgs e, FrameworkElement sender) => FolderIsChanged?.Invoke(sender, e);

        public SettingsWindow()
        {
            InitializeComponent();
            
            StorageFoldersListBox.Items.Clear();
        }
       
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void DeleteButtonOnClick(object sender, RoutedEventArgs e)
        {
            OnFolderRemove(new EventArgs(), (FrameworkElement) sender);
        }

        private void Browse_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = ((FrameworkElement)sender).Tag.ToString();

            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                OnFolderIsChanged(new FolderChangedEventArgs {NewFolder = dialog.SelectedPath}, (FrameworkElement)sender);
            }
        }
    }

    public class FolderChangedEventArgs : EventArgs
    {
        public string NewFolder;
    }
}
