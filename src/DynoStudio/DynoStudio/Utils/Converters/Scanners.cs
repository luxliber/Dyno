using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using Dyno.ViewModels;

namespace Prorubim.DynoStudio.Utils.Converters
{
    internal class FormImageSourceScanner : IValueConverter
    {
        internal static List<string> Items = new List<string>();

        static FormImageSourceScanner()
        {
           
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Items.Clear();
            var workspaceDirPath = new FileInfo(DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspacePath).DirectoryName;
            if (workspaceDirPath == null) return Items;

            var workspaceFiles = Directory.GetFiles(workspaceDirPath, "*.*", SearchOption.AllDirectories);
            foreach (var filePath in workspaceFiles)
                if (Regex.IsMatch(filePath, @".jpg|.png|.gif$"))
                    Items.Add(new FileInfo(filePath).Name);
            return Items;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class PresetBindingScanner : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nodesList = DynoManagerBase.SelectedWorkspacePreset.Parameters.Select(par => par.Name).ToList();
            var testParamsList = DynoManagerBase.SelectedWorkspacePreset.Workspace.WorkspaceForm.UserPars.Select(par => par.Name).ToList();

            nodesList.AddRange(testParamsList);

            return nodesList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { throw new NotImplementedException(); }
    }

    internal class FontWeightScanner : IValueConverter
    {
        protected static readonly List<int> List = new List<int>() {100,200,300,400,600,800};
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return List;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { throw new NotImplementedException(); }
    }

    internal class FontSizeScanner : IValueConverter
    {
        protected static readonly List<double> List = new List<double>(){ 11, 12, 13, 14, 16, 18, 20, 24, 28 };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return List;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { throw new NotImplementedException(); }
    }
}