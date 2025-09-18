using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dyno.Annotations;
using Dyno.ViewModels;

namespace Dyno.Models.Forms
{
    public class PortParameter : INotifyPropertyChanged
    {
        private object _value;
        private string _name;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                DynoManagerBase.UpdateFormIfExists();
            }
        }

        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                DynoManagerBase.UpdateFormIfExists();
            }
        }

        public List<object> Values = new List<object>();

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}