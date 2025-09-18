using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dyno.Annotations;

namespace Dyno.Models.Workspaces
{
    public class WorkspaceGroupPackage : INotifyPropertyChanged
    {
        public string License { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string SiteUrl { get; set; }
        public string RepositoryUrl { get; set; }
        public WorkspaceGroup Root { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}