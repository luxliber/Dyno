using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Color = System.Drawing.Color;

namespace Dyno.Models.Workspaces
{
    public class WorkspaceGroup : WorkspaceNode, IComparable<WorkspaceGroup>
    {
        public string Tag => "group";

        public string Name { get; set; }
        public List<Workspace> Workspaces { get; set; }
        public List<WorkspacePlaylist> WorkspacePlaylists { get; set; }
        public List<WorkspaceGroup> WorkspaceGroups { get; set; }
        public WorkspaceGroupPackage WorkspaceGroupPackage { get; set; }

        readonly ObservableCollection<WorkspaceNode> _childs = new ObservableCollection<WorkspaceNode>();
        public string GroupPath;

        public string ImagePath
        {
            get
            {
                var pngPath = Path.Combine(GroupPath, $"{Path.GetFileName(GroupPath)}.png");
                var jpgPath = Path.Combine(GroupPath, $"{Path.GetFileName(GroupPath)}.jpg");

                return File.Exists(pngPath) ? pngPath : (File.Exists(jpgPath) ? jpgPath : null);
            }
        }

        public ObservableCollection<WorkspaceNode> Childs
        {
            get
            {
                _childs.Clear();
                foreach (var wg in WorkspaceGroups)
                    _childs.Add(wg);

                foreach (var workspace in Workspaces)
                    foreach (var pr in workspace.WorkspacePresets)
                        _childs.Add(pr);

                foreach (var playlist in WorkspacePlaylists)
                    _childs.Add(playlist);

                return _childs;
            }
        }

        public ObservableCollection<WorkspaceNode> ChildsMinimal
        {
            get
            {
                _childs.Clear();
                foreach (var wg in WorkspaceGroups)
                    _childs.Add(wg);
                foreach (var workspace in Workspaces)
                    _childs.Add(workspace);
                return _childs;
            }
        }

        public Workspace GetWorkspaceByInnerName(string wName)
        {
            foreach (var workspace in Workspaces)

                if (workspace.Name == wName)
                    return workspace;

            foreach (var workspaceGroup in WorkspaceGroups)
            {
                var res = workspaceGroup.GetWorkspaceByInnerName(wName);
                if (res != null) return res;
            }

            return null;
        }

        public WorkspacePlaylist GetPlaylistByName(string name)
        {
            foreach (var playlist in WorkspacePlaylists)
                if (playlist.Name == name)
                    return playlist;

            foreach (var workspaceGroup in WorkspaceGroups)
            {
                var res = workspaceGroup.GetPlaylistByName(name);
                if (res != null) return res;
            }

            return null;
        }

        public WorkspacePreset GetWorkspacePresetByInnerName(string wpName)
        {
            foreach (var workspace in Workspaces)
                foreach (var workspacePreset in workspace.WorkspacePresets)
                    if (workspacePreset.InnerName == wpName)
                        return workspacePreset;

            foreach (var workspaceGroup in WorkspaceGroups)
            {
                var res = workspaceGroup.GetWorkspacePresetByInnerName(wpName);
                if (res != null) return res;
            }

            return null;
        }

        public WorkspacePreset GetWorkspacePresetByInnerNameAndWorkspaceInnerName(string wpName, string prName)
        {
            foreach (var workspace in Workspaces)
                foreach (var workspacePreset in workspace.WorkspacePresets)
                    if (workspace.Name == wpName && workspacePreset.InnerName == prName)
                        return workspacePreset;

            foreach (var workspaceGroup in WorkspaceGroups)
            {
                var res = workspaceGroup.GetWorkspacePresetByInnerNameAndWorkspaceInnerName(wpName, prName);
                if (res != null) return res;
            }

            return null;
        }

        public WorkspaceGroup GetWorkspaceGroupByName(string wgName)
        {
            foreach (var workspaceGroup in WorkspaceGroups)
            {
                if (workspaceGroup.Name == wgName)
                    return workspaceGroup;
                var res = workspaceGroup.GetWorkspaceGroupByName(wgName);
                if (res != null) return res;
            }

            return null;
        }

        public WorkspaceGroup GetWorkspaceGroupByXPath(string xpath)
        {
            if (XPath == xpath)
                return this;

            foreach (var workspaceGroup in WorkspaceGroups)
            {
                var res = workspaceGroup.GetWorkspaceGroupByXPath(xpath);
                if (res != null) return res;
            }

            return null;
        }

        public ObservableCollection<WorkspacePreset> WorkspacePresets
        {
            get
            {
                var presets = new ObservableCollection<WorkspacePreset>();

                foreach (var workspace in Workspaces)
                    foreach (var pr in workspace.WorkspacePresets)
                        presets.Add(pr);

                return presets;
            }
        }

        public bool IsExpanded { get; set; }
        public string XPath { get; set; }
        public WorkspaceGroup ParentGroup { get; set; }
        public Color RibbonButtonColor { get; set; }

        public Brush RibbonButtonColorBrush => new SolidColorBrush(
            System.Windows.Media.Color.FromArgb(RibbonButtonColor.A, RibbonButtonColor.R, RibbonButtonColor.G,
                RibbonButtonColor.B));

        public bool IsProject { get; set; }


        public WorkspaceGroup()
        {
            Workspaces = new List<Workspace>();
            WorkspacePlaylists = new List<WorkspacePlaylist>();
            WorkspaceGroups = new List<WorkspaceGroup>();
        }

        public int CompareTo(WorkspaceGroup other)
        {
            if (other == null)
                return 1;

            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }
}