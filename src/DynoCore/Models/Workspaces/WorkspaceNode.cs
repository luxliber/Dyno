using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dyno.Annotations;

namespace Dyno.Models.Workspaces
{
    public class WorkspaceNode : INotifyPropertyChanged
    {
       
        public event CollectionChangeEventHandler CollectionChanged;
        public bool IsSelected { get; set; }


        protected void OnCollectionChanged(WorkspaceNode feed)
        {
            var handler = CollectionChanged;
            handler?.Invoke(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, feed));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
