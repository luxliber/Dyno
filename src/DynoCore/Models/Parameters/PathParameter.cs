using System.IO;
using System.Windows.Forms;
using LitJson;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Dyno.Models.Parameters
{
    public class PathParameter : WorkspaceParameter
    {
        public readonly string Mode;

        public readonly string Filter;
        public readonly int Filterindex;

        public PathParameter(string name, JsonData data)
            : base(name, data)
        {
            Mode = (string)data["mode"];

            Filter = (string)(data.Keys.Contains("filter") ? data["filter"] : "All files (*.*) | *.*");
            Filterindex = (int)(data.Keys.Contains("filterindex") ? data["filterindex"] : 1);
        }

        public PathParameter(string name, string mode, string value)
            : base(name, null)
        {
            Mode = mode;
            Value = value;
            Filter = "All files (*.*) | *.*";
            Filterindex = 1;
        }

        public static string GetAbsolutePath(string relativePath, string basePath)
        {
            if (relativePath == null)
                return null;
            if (basePath == null)
                basePath = Path.GetFullPath("."); // quick way of getting current working directory
            else
                basePath = GetAbsolutePath(basePath, null); // to be REALLY sure ;)
            string path;
            // specific for windows paths starting on \ - they need the drive added to them.
            // I constructed this piece like this for possible Mono support.
            if (!Path.IsPathRooted(relativePath) || "\\".Equals(Path.GetPathRoot(relativePath)))
            {
                if (relativePath.StartsWith(Path.DirectorySeparatorChar.ToString()))
                    path = Path.Combine(Path.GetPathRoot(basePath), relativePath.TrimStart(Path.DirectorySeparatorChar));
                else
                    path = Path.Combine(basePath, relativePath);
            }
            else
                path = relativePath;
            // resolves any internal "..\" to get the true full path.
            return Path.GetFullPath(path);
        }
    }
}