using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dyno.Annotations;
using Dyno.FormControls;
using Dyno.Views.FormControls;
using ProtoBuf;

namespace Dyno.Models.Forms
{
    [ProtoContract]
    public class FormTab : INotifyPropertyChanged
    {
        [ProtoMember(3)] internal List<KeyValuePair<Type, byte[]>> ByteItems = new List<KeyValuePair<Type, byte[]>>();

        [ProtoMember(2)]
        public string Header { get; set; }

        [ProtoMember(4)]
        public double Width { get; set; }

        [ProtoMember(5)]
        public double Height { get; set; }

        [Category(FormControlHelper.PropCategory.Size)]
        [DisplayName("Window Width")]
        public double EditorWidth
        {
            get { return Width; }
            set
            {
                Width = value > 200 ? value : 200;
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(Width));
                // ReSharper disable once ExplicitCallerInfoArgument
                WorkspaceForm.OnPropertyChanged(nameof(WorkspaceForm.Width));
            }
        }

        [Category(FormControlHelper.PropCategory.Size)]
        [DisplayName("Window Height")]
        public double EditorHeight
        {
            get { return Height; }
            set
            {
                Height = value;
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(Height));
                // ReSharper disable once ExplicitCallerInfoArgument
                WorkspaceForm.OnPropertyChanged(nameof(WorkspaceForm.Height));
            }
        }

        [Category("Visual Style")]
        [DisplayName("Tab Title")]
        public string EditorHeader
        {
            get { return Header; }
            set
            {
                Header = value;
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(Header));
            }
        }

        public ObservableCollection<FormControl> Items { get; set; } = new ObservableCollection<FormControl>();

        public WorkspaceForm WorkspaceForm { get; set; }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void PrepareControlsByteList()
        {
            ByteItems.Clear();

            foreach (var control in Items)
            {
                var ms = control.Serialize();
                ByteItems.Add(new KeyValuePair<Type, byte[]>(control.GetType(), ms.ToArray()));
                ms.Close();
            }
        }

        public void RestoreControlsformByteList()
        {
            Items = new ObservableCollection<FormControl>();

            if (ByteItems.Count > 0)
                foreach (var bi in ByteItems)
                {
                    var item = FormControl.Deserialize(bi);

                    if (item == null) continue;

                    item.Form = WorkspaceForm;
                    Items.Add(item);
                }
        }
    }
}