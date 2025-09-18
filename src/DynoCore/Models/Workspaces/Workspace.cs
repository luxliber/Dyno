using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Dyno.Models.Forms;
using Dyno.Utils;
using Dyno.ViewModels;
using LitJson;

namespace Dyno.Models.Workspaces
{
    public class Workspace : WorkspaceNode
    {
        internal IDynoManagerCollector ManagerCollector;
        public string DynamoVersion { get; set; }

        public string ImagePath
        {
            get
            {
                var pngPath = Path.ChangeExtension(WorkspacePath, "png");
                var jpgPath = Path.ChangeExtension(WorkspacePath, "jpg");

                return File.Exists(pngPath) ? pngPath : (File.Exists(jpgPath) ? jpgPath : null);
            }
        }

        public Brush HeaderBrush
        {
            get
            {
                switch (DynoManagerBase.SelectedWorkspacePreset.Status)
                {
                    case WorkspaceBase.WorkspaceStatus.Ok:
                        return new SolidColorBrush(Color.FromRgb(80, 185, 80));
                    case WorkspaceBase.WorkspaceStatus.Working:
                        return new SolidColorBrush(Color.FromRgb(80, 160, 185));
                    case WorkspaceBase.WorkspaceStatus.Error:
                        return new SolidColorBrush(Color.FromRgb(185, 130, 30));
                }
                return /*new SolidColorBrush(Color.FromRgb(60, 60, 60));*/ SystemParameters.WindowGlassBrush;
            }
        }

        public string Tag => "workspace";

        public List<WorkspacePreset> WorkspacePresets { get; set; }

        public IEnumerable<WorkspacePreset> WorkspacePresetsForForm => WorkspacePresets;

        public bool IsExpanded { get; set; }
        
        public bool IsNoPresets => WorkspacePresets.Count == 0 || WorkspacePresets.First().Name == "" || (WorkspacePresets.First().Name == Name && WorkspacePresets.Count == 1);

        public string WorkspacePath { get; set; }

      

        public string PresetTitle => DynoManagerBase.SelectedWorkspacePreset.Name == Name ? "" : DynoManagerBase.SelectedWorkspacePreset.Name;
        public string WorkspaceTitle => Name;

        public bool IsChanged
        {
            get
            {
                return WorkspacePresets.Any(preset => preset.IsChanged);
            }
            set
            {
                foreach (var preset in WorkspacePresets)
                    preset.IsChanged = value;
                OnPropertyChanged(nameof(IsChanged));
            }
        }

        public Workspace()
        {
            WorkspacePresets = new List<WorkspacePreset>();
        }

        public string Name => Path.GetFileNameWithoutExtension(WorkspacePath);

        public void ScanDynoAssets()
        {
            ScanPresetsFile();
            ScanFormFile();
        }

        private void ScanFormFile()
        {
            var formPath = new FileInfo(WorkspacePath).DirectoryName;
            if (formPath == null) return;

            var formFile = Path.Combine(formPath, Name + ".dfm");

            if (File.Exists(formFile))
                Load();
        }

        private void ScanPresetsFile()
        {
            var presetPath = new FileInfo(WorkspacePath).DirectoryName;
            if (presetPath == null) return;

            var presetFile = Path.Combine(presetPath, Name + ".dpr");

            if (File.Exists(presetFile))
            {
                var r = new StreamReader(presetFile);
                var json = r.ReadToEnd();
                r.Close();

                try
                {
                    var jreader = new JsonReader(json) { AllowComments = true };
                    var jmdata = JsonMapper.ToObject(jreader);

                    foreach (var it in jmdata.Keys)
                        if (it == "presets")
                            foreach (KeyValuePair<string, JsonData> pr in jmdata[it])
                            {
                                var key = pr.Key.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                var workspacePreset = new WorkspacePreset { Name = key, Workspace = this, XPath = Path.Combine(XPath, $"{Name}_{key}") };

                                workspacePreset.ScanWorkspaceFile();

                                workspacePreset.ScanParameters(pr.Value);

                                WorkspacePresets.Add(workspacePreset);
                            }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Can`t parse workspace preset file: {presetFile}\n\n Error:\n{e.Message}");
                }
            }
            else
            {
                var workspacePreset = new WorkspacePreset { Name = "", Workspace = this, XPath = Path.Combine(XPath, Name) };
                workspacePreset.ScanWorkspaceFile();
                WorkspacePresets.Add(workspacePreset);
            }
        }

        public WorkspaceGroup WorkspaceGroup { get; set; }
        public WorkspaceForm WorkspaceForm { get; set; }
        public string XPath { get; set; }

        public void CreateForm()
        {
            if (WorkspaceForm == null)
                WorkspaceForm = new WorkspaceForm(true, this);

            Refresh();
        }

        public void Save(bool forceFileSaving = true)
        {
            if (WorkspaceForm != null)
                WorkspaceForm.Save(WorkspaceForm, forceFileSaving);

            if (forceFileSaving)
                IsChanged = false;
            //WorkspacePresets[0].Save();
        }

        public void Load()
        {
            WorkspaceForm = WorkspaceForm.Load(this);
            if (WorkspaceForm != null)
            {
                WorkspaceForm.Workspace = this;
                WorkspaceForm.SelectedIndex = 0;
                //         Refresh();
            }
            IsChanged = false;
        }

        public void Refresh()
        {
            if (WorkspaceForm == null) return;

            WorkspaceForm.Refresh();
            OnPropertyChanged(nameof(WorkspaceForm));
        }
    }
}