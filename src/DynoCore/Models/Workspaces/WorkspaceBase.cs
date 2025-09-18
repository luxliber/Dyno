using System;
using System.Text;
using System.Windows.Media;

namespace Dyno.Models.Workspaces
{
    public abstract class WorkspaceBase : WorkspaceNode
    {
        public static string WrapText(string text, double maxLineLen)
        {
            var originalLines = text.Split(new[] { " " },
                StringSplitOptions.None);

            var wrappedLine = "";

            var actualLine = new StringBuilder();
            var actualLineLen = 0;

            if (originalLines.Length > 1)
                foreach (var item in originalLines)
                {
                    if (item != originalLines[0])
                        actualLine.Append(" ");

                    actualLine.Append(item);
                    actualLineLen += item.Length;

                    if (!(actualLineLen > maxLineLen)) continue;

                    wrappedLine += actualLine + "\n";
                    actualLine.Clear();
                    actualLineLen = 0;
                }
            else
                actualLine.Append(originalLines[0]);

            if (actualLine.Length > 0)
                wrappedLine += actualLine.ToString();

            return wrappedLine;
        }

        public string Name { get; set; }
        public string DisplayName => Name;
        public string ButtonName => WrapText(InnerName, 6);

        public enum WorkspaceStatus
        {
            Working,
            Error,
            Ok,
            Nothing,
        }

        public string InnerName => Name;

        public string XPath { get; set; }

        public System.Drawing.Color RibbonButtonColor;

        public Brush RibbonButtonColorBrush => new SolidColorBrush(Color.FromArgb(RibbonButtonColor.A,
            RibbonButtonColor.R, RibbonButtonColor.G, RibbonButtonColor.B));

        public string Desc { get; set; }

        public abstract string Save();
    }
}
