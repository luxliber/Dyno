using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Dyno.Models;
using Dyno.Models.Forms;
using Dyno.Models.Parameters;
using Dyno.Models.Workspaces;
using MessageBox = System.Windows.MessageBox;

namespace Dyno.ViewModels
{
    public abstract class DynoManagerBase : IDynoManagerCollector
    {
        public WorkspaceGroup Root { get; set; } = new WorkspaceGroup {Name = "Dyno"};
        public static CalcEngine.CalcEngine CEngine = new CalcEngine.CalcEngine();

        public static WorkspacePreset SelectedWorkspacePreset
        {
            get => _selectedWorkspacePreset;
            set
            {
                if (_selectedWorkspacePreset != value)
                {
                    _selectedWorkspacePreset = value;
                    //         ResetCalcEngineFromWorkspace();
                }
            }
        }

        public static WorkspacePlaylist SelectedWorkspaceList;
        public static int SelectedWorkspaceListIndex;

        internal static void ResetCalcEngineFromWorkspace()
        {
            if (_selectedWorkspacePreset == null) return;

            CEngine.Variables.Clear();
            foreach (var par in _selectedWorkspacePreset.Parameters)
                CEngine.Variables[par.Name] = par.Value;

            if (_selectedWorkspacePreset.Workspace.WorkspaceForm != null)
                foreach (var par in _selectedWorkspacePreset.Workspace.WorkspaceForm.UserPars)
                    if (par.Value != null && par.Name != null)
                        CEngine.Variables[par.Name] = par.Value;
        }

        public void WriteWorkspaces()
        {
            //         foreach (var item in Items)
            //           foreach (var wp in item.Workspaces)
            //         {
            //    if (wp.Parameters.Count > 0)
            //  {
            //    wp.Save();
            //}
            //       }
        }

        public static void UpdateFormIfExists()
        {
            SelectedWorkspacePreset?.Workspace.WorkspaceForm?.FillAllControls();
        }

        private static string GetRelativePath(string filespec, string folder)
        {
            var pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                folder += Path.DirectorySeparatorChar;

            var folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString()
                .Replace('/', Path.DirectorySeparatorChar));
        }

        public void ScanWorkspaceGroups(string storagePath, string path,
            WorkspaceGroup group, bool isFirstLevel = false)
        {
            var dirs = Directory.GetDirectories(path).Where(x => Path.GetFileName(x).ToLower() != "backup")
                .OrderBy(x => x);


            var dirInfo = new DirectoryInfo(path);
            var wg = Root;
            var exWg = Root.GetWorkspaceGroupByXPath(GetRelativePath(path, storagePath));
            if (!isFirstLevel)
                if (exWg != null)
                    wg = exWg;
                else
                    wg = new WorkspaceGroup
                    {
                        Name = dirInfo.Name,
                        XPath = GetRelativePath(path, storagePath),
                        ParentGroup = group,
                        GroupPath = path
                    };

            foreach (var dirPath in dirs)
                ScanWorkspaceGroups(storagePath, dirPath, wg);

            if (!isFirstLevel && exWg == null)
                group.WorkspaceGroups.Add(wg);

            var workspaceFiles = Directory.GetFiles(path, "*.dyn").OrderBy(x => x).ToList();
            if (!workspaceFiles.Any()) return;

            foreach (var filePath in workspaceFiles)
            {
                var workspace = new Workspace
                {
                    ManagerCollector = this,
                    WorkspacePath = filePath,
                    WorkspaceGroup = wg,
                    XPath = GetRelativePath(path, storagePath)
                };
                workspace.ScanDynoAssets();
                if (workspace.WorkspacePresets.Count > 0)
                    wg.Workspaces.Add(workspace);
            }

            var playlistFiles = Directory.GetFiles(path, "*.dpl").OrderBy(x => x).ToList();
            if (!playlistFiles.Any()) return;

            foreach (var filePath in playlistFiles)
            {
                var playlist = new WorkspacePlaylist
                {
                    FilePath = filePath,
                    WorkspaceGroup = wg,
                    XPath = GetRelativePath(path, storagePath)
                };
                playlist.ScanPlaylistFile(Root);
                if (playlist.Presets.Count > 0)
                    wg.WorkspacePlaylists.Add(playlist);
            }
        }

        public void ScanProjectScripts(string path, string prName)
        {
            var workspaceFiles = Directory.GetFiles(path, "*.dyn");
            if (workspaceFiles.Length > 0)
            {
                var wg = new WorkspaceGroup {Name = prName, XPath = prName, IsProject = true};

                foreach (var filePath in workspaceFiles)
                {
                    var workspace = new Workspace
                    {
                        ManagerCollector = this,
                        WorkspacePath = filePath,
                        WorkspaceGroup = wg,
                        XPath = Path.Combine(prName, Path.GetFileNameWithoutExtension(filePath))
                    };
                    workspace.ScanDynoAssets();
                    wg.Workspaces.Add(workspace);
                }

                Root.WorkspaceGroups.Insert(0, wg);
            }

            Root.OnPropertyChanged("Childs");
        }

        public void ScanFileStorage(string storagePath)
        {
            if (!Directory.Exists(storagePath))
            {
                MessageBox.Show($"Dynamo workspaces storage folder: {storagePath} not found!");
                return;
            }

            Root.GroupPath = storagePath;
            ScanWorkspaceGroups(storagePath, storagePath, Root, true);
        }

        public abstract void OnDocumentChanged();

        //    public static IDynoManagerCollector Instance = null;
        private static WorkspacePreset _selectedWorkspacePreset;

        public virtual object SelectObject() => null;

        public virtual List<object> SelectObjectsInOrder() => null;

        public virtual object SelectFace() => null;
        public virtual List<object> SelectFaces() => null;

        public virtual object SelectEdge() => null;
        public virtual List<object> SelectEdges() => null;

        public object SelectPointOnFace() => null;

        public virtual List<object> SelectObjectsByRectangle() => null;

        public virtual List<object> SelectObjects() => null;

        public virtual DynoSettingsBase GetSettingsBase() => null;

        public virtual string GetElementId(object element) => null;
        public abstract void SelectElements(SelectElementParameter spar);
        public abstract void SelectReference(SelectReferenceParameter spar);
        public abstract void SelectFile(PathParameter spar);

        public WorkspacePreset GetWorkspacePresetByXpath(string presetName)
        {
            return SearchPresetInGroupsTreeByXPath(Root, presetName);
        }

        private WorkspacePreset SearchPresetInGroupsTreeByXPath(WorkspaceGroup workspaceGroup, string presetName)
        {
            foreach (var wg in workspaceGroup.WorkspaceGroups)
            {
                var res = SearchPresetInGroupsTreeByXPath(wg, presetName);
                if (res != null)
                    return res;
            }

            foreach (var w in workspaceGroup.Workspaces)
            {
                var res = SearthPresetInWorkspacePresetsByXPath(w, presetName);
                if (res != null)
                    return res;
            }

            return null;
        }

        private WorkspacePreset SearthPresetInWorkspacePresetsByXPath(Workspace workspace, string presetName)
        {
            return workspace.WorkspacePresets.FirstOrDefault(p => p.XPath == presetName);
        }

        public static void ResetCalcEngineFromPortData(Dictionary<string, PortParameter> portData)
        {
            CEngine.Variables.Clear();
            foreach (var par in portData)
                if (par.Value.Value != null)
                    CEngine.Variables[par.Key] = par.Value.Value.ToString();
        }

        
    }
}