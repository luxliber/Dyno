using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Dyno.Annotations;
using Dyno.Models;

namespace Dyno
{
    public class StorageFolderItem : INotifyPropertyChanged
    {
        public string Path { get; set; }
        public bool Status { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected internal virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [Serializable]
    public class DynoSettings : DynoSettingsBase
    {
        public bool WindowShowing = false;
        public string Filter = "";
        public List<string> ExpandedKnots = new List<string>();
        public bool IsCheckUpdates { get; set; } = true;

        
        public bool IsShowDynoAddinButton { get; set; } = false;
        public bool IsShowDynoManageButton { get; set; } = true;
        public bool IsHidePlayerButton { get; set; } = false;

        public ObservableCollection<string> StorageFolders { get; set; }

        public ObservableCollection<StorageFolderItem> StorageFoldersZip { get; set; } = new ObservableCollection<StorageFolderItem>();

        public void WriteSettings(Window mainWindow)
        {
            WriteSettings("Dyno\\Dyno.cfg", this);
        }

        public bool Contains(string path) => StorageFoldersZip.Any(x => x.Path == path);

        public StorageFolderItem GetItemByPath(string path) => StorageFoldersZip.FirstOrDefault(x => x.Path == path);

        public static DynoSettings ReadSettings()
        {
            var settings = ReadSettings<DynoSettings>("Dyno\\Dyno.cfg");
            return settings;
        }
    }
}