using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Dyno.Annotations;
using Dyno.Models;
using Prorubim.DynoStudio.Main;

namespace Prorubim.DynoStudio.Models
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
        public List<string> ExpandedKnots = new List<string>();

        public double WindowTop = 30;
        public double WindowLeft = 30;

        public double TreeSplitterWidth { get; set; } = 300;
        public double PropSplitterWidth { get; set; } = 300;
        public string UserPropSplitterWidth { get; set; } = "0.2*";
        public bool Contains(string path) => StorageFoldersZip.Any(x => x.Path == path);
        public bool IsCheckUpdates { get; set; } = false;

        public StorageFolderItem GetItemByPath(string path) => StorageFoldersZip.FirstOrDefault(x => x.Path == path);

        public ObservableCollection<StorageFolderItem> StorageFoldersZip { get; set; } = new ObservableCollection<StorageFolderItem>();

        public void WriteSettings(MainWindow mainWindow)
        {
            if (mainWindow != null)
            {
                if (!double.IsNaN(mainWindow.Top))
                    WindowTop = mainWindow.Top;
                if (!double.IsNaN(mainWindow.Left))
                    WindowLeft = mainWindow.Left;


                TreeSplitterWidth = mainWindow.MainGrid.ColumnDefinitions[0].Width.Value;

                UserPropSplitterWidth = (mainWindow.MainGrid.ColumnDefinitions[4].Width.Value/ mainWindow.MainGrid.ColumnDefinitions[2].Width.Value).ToString(CultureInfo.InvariantCulture); 
                UserPropSplitterWidth = $"{UserPropSplitterWidth}*";
                PropSplitterWidth = mainWindow.MainGrid.ColumnDefinitions[6].Width.Value;
            }

            WriteSettings("DynoStudio\\DynoStudio.cfg", this);
        }

        public static DynoSettings ReadSettings()
        {
            var settings = ReadSettings<DynoSettings>("DynoStudio\\DynoStudio.cfg");
            return settings;
        }
    }
}