using System.Collections.Generic;
using LitJson;

namespace Dyno.Models.Parameters
{
    public class SelectElementParameter : WorkspaceParameter
    {
        public readonly string Mode;

        public IList<object> SelectedElements = new List<object>();

        public SelectElementParameter(string name, JsonData data) : base(name, data)
        {
            Mode = (string)data["mode"];
        }

        public SelectElementParameter(string name, string mode) : base(name, null)
        {
            Mode = mode;
        }

        public void UpdateValue()
        {

            if (SelectedElements.Count == 0 || Workspace.Workspace.ManagerCollector == null)
            {
                Value = "None";
                return;
            }

            if (Mode == "one")
                Value = "Id: " + Workspace.Workspace.ManagerCollector.GetElementId(SelectedElements[0]);

            var ids = "Ids: ";
            var en = SelectedElements.GetEnumerator();
            en.MoveNext();
            while (en.Current != null)
            {
                ids += Workspace.Workspace.ManagerCollector.GetElementId(en.Current);
                en.MoveNext();
                if (en.Current != null)
                    ids += ", ";
            }

            Value = ids;
        }
    }
}