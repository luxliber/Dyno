using System;
using System.Collections.Generic;
using Dyno.ViewModels;
using LitJson;

namespace Dyno.Models.Parameters
{
    public class SelectReferenceParameter : WorkspaceParameter
    {
        public readonly string Type;


        public IList<object> SelectedReferences = new List<object>();

        public SelectReferenceParameter(string name, JsonData data) : base(name, data)
        {
            Type = (string)data["type"];
        }

        public SelectReferenceParameter(string name, string type) : base(name, null)
        {
            Type = type;
        }

        public void UpdateValue()
        {
            if (SelectedReferences.Count == 0 || Workspace.Workspace.ManagerCollector == null)
            {
                Value = "None";
                return;
            }

            if (Type == "face" || Type == "edge" || Type == "pointOnFace")
                Value = "Host Id: " + Workspace.Workspace.ManagerCollector.GetElementId(SelectedReferences[0]);

            var ids = "Host Ids: ";
            var en = SelectedReferences.GetEnumerator();
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