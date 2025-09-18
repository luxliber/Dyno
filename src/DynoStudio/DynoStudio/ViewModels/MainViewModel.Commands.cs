using System;
using System.Linq;
using System.Windows;
using Dyno.Forms;
using Dyno.Models.Workspaces;
using Dyno.ViewModels;
using Prorubim.DynoStudio.External;
using Prorubim.DynoStudio.Utils;

namespace Prorubim.DynoStudio.ViewModels
{
    public partial class MainViewModel
    {
        public RelayCommand RefreshCommand => new RelayCommand(o =>
        {
            EditorManager.UnselectAll();

            string oldWpPath = null;
            if (DynoManagerBase.SelectedWorkspacePreset != null)
                oldWpPath = DynoManagerBase.SelectedWorkspacePreset.XPath;

            App.Manager.ScanFileStorage();
            Mw.ExpandTree();

            if (oldWpPath == null) return;

            var wp = DynoManager.Instance.GetWorkspacePresetByXpath(oldWpPath);

            SelectWorkspaceCommand.Execute(wp?.Workspace);
        });

        public RelayCommand SelectWorkspaceCommand => new RelayCommand(o =>
        {
            var w = o as Workspace;

            EditorManager.UnselectAll();

            if (w != null)
            {
                DynoManager.SelectWorkspacePreset(w.WorkspacePresets.First());

                w.IsSelected = true;
                w.OnPropertyChanged(nameof(WorkspaceNode.IsSelected));
                DynoManager.SelectedPackage = null;
            }
            else
                DynoManagerBase.SelectedWorkspacePreset = null;

          
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(IsTreeItemSelected));
            OnPropertyChanged(nameof(IsWorkspaceSelected));
            OnPropertyChanged(nameof(IsGroupSelected));
            OnPropertyChanged(nameof(IsFormSelected));

            EditorManager.OnPropertyChanged(nameof(ViewModels.EditorManager.WorkspaceForm));
            
        });

        public RelayCommand SelectGroupCommand => new RelayCommand(o =>
        {
            var workspaceGroup = o as WorkspaceGroup;

            EditorManager.UnselectAll();

            if (workspaceGroup != null)
            {
                DynoManager.SelectWorkspacePackage(workspaceGroup.WorkspaceGroupPackage);
                workspaceGroup.IsSelected = true;
                workspaceGroup.OnPropertyChanged(nameof(WorkspaceNode.IsSelected));
                DynoManagerBase.SelectedWorkspacePreset = null;
            }
            else
                DynoManager.SelectedPackage = null;

            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(IsTreeItemSelected));
            OnPropertyChanged(nameof(IsGroupSelected));
            OnPropertyChanged(nameof(IsWorkspaceSelected));
            OnPropertyChanged(nameof(IsPackageSelected));
            OnPropertyChanged(nameof(IsFormSelected));
        });


        public RelayCommand SaveCommand => new RelayCommand(o =>
        {
            try
            {
                ProrubimExternal.ProcessLicenseChecking("dyno_studio");
                DynoManagerBase.SelectedWorkspacePreset.Workspace.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Preset is not saved. {ex.Message}", "Dyno Studio License Checking");
            }
        });

        public RelayCommand LoadCommand => new RelayCommand(o =>
        {
            if (DynoManagerBase.SelectedWorkspacePreset == null)
                return;
            EditorManager.UnselectAll();
            DynoManager.DeleteSelectedHistory();

            DynoManagerBase.SelectedWorkspacePreset.Workspace.Load();

            DynoManager.SelectWorkspacePreset(DynoManagerBase.SelectedWorkspacePreset);

            EditorManager.OnPropertyChanged("WorkspaceForm");
        });

        public RelayCommand SettingsCommand => new RelayCommand(o => new SettingsWindow { DataContext = Settings }.ShowDialog());
        public RelayCommand TestCommand => new RelayCommand(o => DynoManager.TestForm());

        public RelayCommand UndoCommand => new RelayCommand(o =>
        {
            DynoManager.SelectedHistory.Undo();
            EditorManager.OnPropertyChanged(nameof(ViewModels.EditorManager.PropertyGridItem));
        }, c => History?.IsUndo ?? false);

        public RelayCommand RedoCommand => new RelayCommand(o =>
        {
            DynoManager.SelectedHistory.Redo();
            EditorManager.OnPropertyChanged(nameof(ViewModels.EditorManager.PropertyGridItem));
        }, c => History?.IsRedo ?? false);

        public RelayCommand CreateCommand => new RelayCommand(o =>
        {
            DynoManagerBase.SelectedWorkspacePreset.Workspace.CreateForm();
            EditorManager.OnPropertyChanged("WorkspaceForm");
            EditorManager.OnPropertyChanged(nameof(IsFormSelected));
            OnPropertyChanged(nameof(IsFormSelected));
        });

        public RelayCommand CreatePackageCommand => new RelayCommand(o =>
        {
            //PackageManager.CreatePackage(DynoManagerBase.SelectedWorkspacePreset);

        });
    }
}