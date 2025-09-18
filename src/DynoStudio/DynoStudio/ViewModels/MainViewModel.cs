using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dyno.Forms.Annotations;
using Dyno.Models.Workspaces;
using Dyno.ViewModels;
using Prorubim.DynoStudio.Main;
using Prorubim.DynoStudio.Models;

namespace Prorubim.DynoStudio.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        public static WorkspaceGroup Root => DynoManager.Instance.Root;
        public static DynoSettings Settings => DynoManager.Instance.Settings;
        public static HistoryManager History => DynoManager.SelectedHistory;

        public static EditorManager EditorManager { get; set; }

        public bool IsTreeItemSelected => Mw?.StorageTreeView.SelectedItem != null;

        public bool IsGroupSelected => Mw?.StorageTreeView.SelectedItem is WorkspaceGroup;
        public bool IsPackageSelected => false;//IsGroupSelected && DynoManager.SelectedPackage != null;

        public bool IsWorkspaceSelected => Mw?.StorageTreeView.SelectedItem is Workspace || DynoManagerBase.SelectedWorkspacePreset!=null;
        public bool IsFormSelected => IsWorkspaceSelected && DynoManagerBase.SelectedWorkspacePreset != null && DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm != null;
        
        public string Title => DynoManagerBase.SelectedWorkspacePreset != null ? $"Dyno Studio - {DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspacePath}" : "Dyno Studio";

        public MainWindow Mw { get; set; }

        public MainViewModel()
        {
            EditorManager = new EditorManager();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}