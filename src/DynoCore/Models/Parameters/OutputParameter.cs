using System.Collections.ObjectModel;

namespace Dyno.Models.Parameters
{
    public class OutputParameter : WorkspaceParameter
    {
        public ObservableCollection<string> Childs { get; set; }

        public OutputParameter(string name, string value)
            : base(name)
        {
            Value = value;
            Childs = new ObservableCollection<string>();
        }
    }
}
