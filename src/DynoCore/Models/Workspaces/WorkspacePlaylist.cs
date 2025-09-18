using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using LitJson;

namespace Dyno.Models.Workspaces
{
    public class WorkspacePlaylist : WorkspaceBase
    {
        public string FilePath;
        public string Tag => "playlist";

        public string ImagePath
        {
            get
            {
                var pngPath = Path.ChangeExtension(FilePath, "png");
                var jpgPath = Path.ChangeExtension(FilePath, "jpg");

                return File.Exists(pngPath) ? pngPath : (File.Exists(jpgPath) ? jpgPath : null);
            }
        }

        public ObservableCollection<WorkspacePreset> Presets { get; set; }
        public ObservableCollection<WorkspacePreset> Childs => Presets;

        public WorkspaceGroup WorkspaceGroup { get; set; }

        public new string Name => Path.GetFileNameWithoutExtension(FilePath);

        public WorkspacePlaylist()
        {
            Presets = new ObservableCollection<WorkspacePreset>();
        }

        public void AddPreset(WorkspacePreset preset) => Presets.Add(preset);

        public override string Save()
        {
            var sb = new StringBuilder();
            var writer = new JsonWriter(sb) { PrettyPrint = true };

            return "";
        }

        public void ScanPlaylistFile(WorkspaceGroup root)
        {
            if (FilePath == null) return;
            if (!File.Exists(FilePath)) return;

            try
            {
                var r = new StreamReader(FilePath);
                var json = r.ReadToEnd();
                r.Close();

                var jReader = new JsonReader(json) { AllowComments = true };
                var jObject = JsonMapper.ToObject(jReader);
                var jPresets = jObject["presets"];

                foreach (var jPreset in jPresets)
                {
                    var name = jPreset.ToString();
                    var pr = root.GetWorkspacePresetByInnerName(name);
                    if (pr == null) continue;

                    AddPreset(pr);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Can`t parse playlist file: " + FilePath);
            }
        }
    }
}
