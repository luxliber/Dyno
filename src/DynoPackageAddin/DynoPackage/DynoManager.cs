using System.Collections.Generic;
using Dyno;
using Dyno.Models;
using Dyno.Models.Workspaces;
using Prorubim.DynoRevitCore;
using Prorubim.DynoRevitCore.ViewModels;

namespace Prorubim.DynoPackage
{
    public class DynoManager : DynoRevitManagerBase
    {
        //public static DynoSettings Settings;

        public DynoManager()
        {
            //Settings = DynoSettings.ReadSettings();
            ScanFileStorage(true);
        }

        internal void ScanFileStorage(bool createButtons = false)
        {
            SelectedWorkspacePreset = null;
            Root.WorkspaceGroups.Clear();
            Root.Workspaces.Clear();
            Root.WorkspacePlaylists.Clear();

            ////////////////
            ScanFileStorage(DynoPackageApp.StorageFolder);
            ////////////////

            Packages.Add(new WorkspaceGroupPackage { Name = "Package1", Root = Root });

            Root.WorkspaceGroups.Sort();

            WorkspaceCommandsManager.CreateRibbonWorkspaceButtons(Root,"Package1",createButtons,
                new List<string> {DynoPackageApp.StorageFolder});


            Root.OnPropertyChanged("Childs");
        }
    }
}