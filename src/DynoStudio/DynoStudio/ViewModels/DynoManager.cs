using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Dyno.Models;
using Dyno.Models.Forms;
using Dyno.Models.Parameters;
using Dyno.Models.Workspaces;
using Dyno.ViewModels;
using Prorubim.DynoStudio.History;
using Prorubim.DynoStudio.Models;
using WorkspaceFormWindow = Dyno.Views.WorkspaceFormWindow;

namespace Prorubim.DynoStudio.ViewModels
{
    public class DynoManager : DynoManagerBase, INotifyPropertyChanged
    {
        public DynoSettings Settings { get; set; }


        private static readonly Dictionary<WorkspaceForm, HistoryManager> History =
            new Dictionary<WorkspaceForm, HistoryManager>();

        internal static DynoManager Instance;

        public static WorkspaceForm IsWorkspaceForm =>
            (SelectedWorkspacePreset != null && SelectedWorkspacePreset.Workspace.WorkspaceForm != null)
                ? SelectedWorkspacePreset.Workspace.WorkspaceForm
                : null;

        public static WorkspaceGroupPackage SelectedPackage { get; set; } = null;

        public static HistoryManager SelectedHistory
        {
            get
            {
                if (SelectedWorkspacePreset?.Workspace.WorkspaceForm == null)
                    return null;

                if (History.ContainsKey(SelectedWorkspacePreset.Workspace.WorkspaceForm))
                    return History[SelectedWorkspacePreset.Workspace.WorkspaceForm];

                History.Add(SelectedWorkspacePreset.Workspace.WorkspaceForm, new HistoryManager());

                return History[SelectedWorkspacePreset.Workspace.WorkspaceForm];
            }
        }

        public DynoManager()
        {
            Settings = DynoSettings.ReadSettings();
            Instance = this;
            ScanFileStorage();
        }

        internal void ScanFileStorage()
        {
            Root.WorkspaceGroups.Clear();
            Root.Workspaces.Clear();

            foreach (var storageFolder in Settings.StorageFoldersZip.Where(x => x.Status))
                ScanFileStorage(storageFolder.Path);

            Root.WorkspaceGroups.Sort();
            Root.OnPropertyChanged(nameof(WorkspaceGroup.ChildsMinimal));
        }

        public static void SelectWorkspacePreset(WorkspacePreset pr)
        {
            if (pr == null)
                return;

            SelectedWorkspacePreset = pr;
            pr.Workspace.Refresh();
        }

        public static void SelectWorkspacePackage(WorkspaceGroupPackage pkg)
        {
            if (pkg == null)
                return;

            SelectedPackage = pkg;
            //pkg.Refresh();
        }

        public static void TestForm()
        {
            var ms = WorkspaceForm.SaveToStream(SelectedWorkspacePreset.Workspace.WorkspaceForm);
            var w = new WorkspaceFormWindow(SelectedWorkspacePreset, ms, true);
            w.ShowDialog();

            UpdateFormIfExists();
        }


        public static void DeleteSelectedHistory()
        {
            if (SelectedWorkspacePreset.Workspace.WorkspaceForm != null &&
                History.ContainsKey(SelectedWorkspacePreset.Workspace.WorkspaceForm))
            {
                SelectedHistory.Clear();

                History.Remove(SelectedWorkspacePreset.Workspace.WorkspaceForm);
            }
        }

        public override DynoSettingsBase GetSettingsBase() => Settings;

        public override void SelectElements(SelectElementParameter spar)
        {
            throw new System.NotImplementedException();
        }

        public override void SelectReference(SelectReferenceParameter spar)
        {
            throw new System.NotImplementedException();
        }

        public override void SelectFile(PathParameter spar)
        {
            throw new System.NotImplementedException();
        }

        public override void OnDocumentChanged()
        {
            throw new System.NotImplementedException();
        }

        public override List<object> SelectObjectsInOrder()
        {
            throw new System.NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}