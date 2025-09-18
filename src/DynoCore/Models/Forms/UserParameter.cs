using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dyno.Annotations;
using Dyno.ViewModels;
using ProtoBuf;

namespace Dyno.Models.Forms
{
    [ProtoContract]
    public class UserParameter : INotifyPropertyChanged
    {
        private string _value;
        private string _name;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [ProtoMember(1)]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                DynoManagerBase.UpdateFormIfExists();
            }
        }

        [ProtoMember(2)]
        public string Value
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