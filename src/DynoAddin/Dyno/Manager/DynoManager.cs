using System.IO;
using System.Linq;
using Autodesk.Revit.UI;
using Dyno.Models;
using Dyno.Models.Workspaces;
using Prorubim.DynoRevitCore;
using Prorubim.DynoRevitCore.ViewModels;

namespace Dyno.Manager
{
    public partial class DynoManager : DynoRevitManagerBase
    {
        internal static string OpenedWorkspaceInDynamo = "";

        private static ExternalEvent _openInDynamoExternalEvent;
        public static ExternalEvent DropDownExternalEvent;
        public static DropDownExternalIvent DropDownExternalHandler;

        public DynoSettings Settings;

        public override DynoSettingsBase GetSettingsBase() => Settings;

        public DynoManager()
        {
            Settings = DynoSettings.ReadSettings();
            Packages.Add(new WorkspaceGroupPackage {Name = "Dyno", Root = Root});
            ScanFileStorage(true);
        }

        internal void ScanFileStorage(bool createButtons = false)
        {
            SelectedWorkspacePreset = null;
            Root.WorkspaceGroups.Clear();
            Root.Workspaces.Clear();
            Root.WorkspacePlaylists.Clear();

            foreach (var storageFolder in Settings.StorageFoldersZip)
                if (storageFolder.Status)
                    ScanFileStorage(storageFolder.Path);

            Root.WorkspaceGroups.Sort();


            if (DynoAppBase.Doc != null && DynoAppBase.Doc.PathName != "" && Directory.Exists(DynoAppBase.Doc.PathName))
            {
                var path = Path.GetDirectoryName(DynoAppBase.Doc.PathName);
                var prName = DynoAppBase.Doc.Title;
                ScanProjectScripts(path, prName);
            }

            WorkspaceCommandsManager.CreateRibbonWorkspaceButtons(Root, "Dyno", createButtons,
                Settings.StorageFoldersZip.Select(x => x.Path).ToList());

            Root.OnPropertyChanged("Childs");
        }

        public override void InitExternalEvents()
        {
            base.InitExternalEvents();

            var openInDynamoExternalIvent = new OpenInDynamoExternalIvent();
            _openInDynamoExternalEvent = ExternalEvent.Create(openInDynamoExternalIvent);

            DropDownExternalHandler = new DropDownExternalIvent();
            DropDownExternalEvent = ExternalEvent.Create(DropDownExternalHandler);
        }

        public static void OpenInDynamo(string path)
        {
            OpenedWorkspaceInDynamo = path;
            _openInDynamoExternalEvent.Raise();
        }
    }
}