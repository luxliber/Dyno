using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Dyno.Annotations;
using Dyno.Models;
using Dyno.ViewModels;
using Microsoft.Win32;

namespace Prorubim.DynoRevitCore
{
    [Transaction(TransactionMode.Manual),
     Regeneration(RegenerationOption.Manual)]
    public class DynoAppBase : IExternalApplication, INotifyPropertyChanged
    {
        private const string RegKey64 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";

        public static readonly string DynoPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal static Application App;
        
        public static ControlledApplication ControlledApp;
        public static UIApplication UiApp;
        protected internal static UIControlledApplication UiControlledApp;
        public static Document Doc;
        public static string DynamoRevitLoadPath;

        public static DynoAppBase Instance;

        public virtual DynoSettingsBase GetSettings() => null;
        public virtual DynoManagerBase GetManager() => null;

        private static RegistryKey OpenKey(string key)
        {
            var regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            return regKey.OpenSubKey(key);
        }

        public virtual Result OnStartup(UIControlledApplication application)
        {
            Instance = this;
            ControlledApp = application.ControlledApplication;
            UiControlledApp = application;
            var versionNumber = ControlledApp.VersionNumber;
            DynamoProductsManager.ScanProducts(versionNumber);

            CheckProrubimNodesPath();
            application.Idling += ApplicationOnIdling;
            application.ViewActivated += Application_ViewActivated;
            return Result.Succeeded;
        }

        

        private static void ApplicationOnIdling(object sender, IdlingEventArgs idlingEventArgs)
        {
            UiControlledApp.Idling -= ApplicationOnIdling;
            UiApp = sender as UIApplication;
        }

        static string GetDynamoInstallVersion(RegistryKey key)
        {
            if (key != null)
                return key.GetValue("Version") as string;

            return string.Empty;
        }

        private static void CheckProrubimNodesPath()
        {
            if(DynamoProductsManager.SelectedProduct==null) return;

            var version = DynamoProductsManager.SelectedProduct.VersionInfo;
            var prorubimNodes = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "Prorubim Nodes");

            if (!Directory.Exists(prorubimNodes)) return;

            var dynamoCfg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                $"Dynamo/Dynamo Revit\\{version.Major}.{version.Minor}\\DynamoSettings.xml");

            if (!File.Exists(dynamoCfg)) return;

            var doc = new XmlDocument();
            try
            {
                doc.Load(dynamoCfg);

                XmlNode root = doc.DocumentElement;

                var pkgListNode = root.SelectSingleNode("CustomPackageFolders");
                var foundNode = pkgListNode.ChildNodes.Cast<XmlNode>()
                    .FirstOrDefault(x => x.InnerText == prorubimNodes);

                if (foundNode != null) return;

                var elem = doc.CreateElement("string");
                elem.InnerText = prorubimNodes;
                pkgListNode.AppendChild(elem);
                doc.Save(dynamoCfg);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static bool TryGetRegistryKey(string path, string key, out dynamic value)
        {
            value = null;
            try
            {
                var rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return false;
                value = rk.GetValue(key);
                return value != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Returns the Windows major version number for this computer.
        /// </summary>
        public static int WinMajorVersion
        {
            get
            {
                // The 'CurrentMajorVersionNumber' string value in the CurrentVersion key is new for Windows 10, 
                // and will most likely (hopefully) be there for some time before MS decides to change this - again...
                if (TryGetRegistryKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentMajorVersionNumber",
                    out var major))
                {
                    return (int) major;
                }

                // When the 'CurrentMajorVersionNumber' value is not present we fallback to reading the previous key used for this: 'CurrentVersion'
                if (!TryGetRegistryKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion",
                    out var version))
                    return 0;

                var versionParts = ((string) version).Split('.');
                if (versionParts.Length != 2) return 0;
                return int.TryParse(versionParts[0], out var majorAsInt) ? majorAsInt : 0;
            }
        }

        internal void Application_ViewActivated(object sender, ViewActivatedEventArgs e)
        {
            App = e.Document.Application;
            Doc = e.Document;

            if (DynamoProductsManager.SelectedProduct != null)
                GetManager().OnDocumentChanged();
        }

        public virtual Result OnShutdown(UIControlledApplication application) => Result.Succeeded;

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected internal virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}