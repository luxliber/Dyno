using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Interop;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Dynamo.Applications;
using Prorubim.DynoRevitCore.ViewModels;

namespace Prorubim.DynoRevitCore
{
    public static class DynamoProductsManager
    {
        private const string UninstallRegKey64 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
        internal static List<DynamoProduct> Products;

        internal static EventWaitHandle ProgressWindowWaitHandle;

        private static Views.InitProgressWindow _initProgressWindow;

        public static DynamoProduct SelectedProduct;

        public class DynamoProduct
        {
            public string InstallLocation;
            public Version VersionInfo;
        }

        public static void ScanProducts(string versionNumber)
        {
            Products = new List<DynamoProduct>();

            try
            {
                var revitAPIPath = Assembly.GetAssembly(typeof(Document))?.Location;

                var revitDir = Path.GetDirectoryName(revitAPIPath);

                var dynamoRevitDir = Path.Combine(revitDir, "AddIns\\DynamoForRevit");
                var dynamoProducts = FindDynamoRevitInstallations(dynamoRevitDir);

                foreach (var p in dynamoProducts)
                {
                    var path = GetDynamoRevitDSPath(p);
                    if (path == null) continue;

                    Products.Add(p);
                }

                if (Products.Any())
                    SelectedProduct = Products.LastOrDefault();

            }
            catch (Exception)
            {
                // ignored
            }


        }

        internal static string GetDynamoRevitDSPath(DynamoProduct product)
        {
            var p1 = Path.Combine(product.InstallLocation, "DynamoRevitDS.dll");
            var p2 = Path.Combine(product.InstallLocation, "Revit", "DynamoRevitDS.dll");
            return File.Exists(p1) ? p1 : File.Exists(p2) ? p2 : null;
        }

        private static IEnumerable<DynamoProduct> FindDynamoRevitInstallations(string debugPath)
        {
            var assembly = Assembly.LoadFrom(Path.Combine(debugPath, "DynamoInstallDetective.dll"));
            var type = assembly.GetType("DynamoInstallDetective.Utilities");

            var installationsMethod = type.GetMethod(
                "LocateDynamoInstallations",
                BindingFlags.Public | BindingFlags.Static);

            if (installationsMethod == null)
            {
                throw new MissingMethodException(
                    "Method 'DynamoInstallDetective.Utilities.LocateDynamoInstallations' not found");
            }

            string FileLocator(string p) => Path.Combine(p, $"Revit", "DynamoRevitDS.dll");

            var methodParams = new object[] { debugPath, (Func<string, string>)FileLocator };
            var installs = installationsMethod.Invoke(null, methodParams) as IEnumerable;

            return installs?.Cast<KeyValuePair<string, Tuple<int, int, int, int>>>()
                .Select(
                    p => new DynamoProduct()
                    {
                        InstallLocation = p.Key,
                        VersionInfo = new Version(p.Value.Item1, p.Value.Item2, p.Value.Item3, p.Value.Item4)
                    });
        }

        private static void ShowInitProgressWindow()
        {
            //creates and shows the progress window
            _initProgressWindow = new Views.InitProgressWindow();

            // ReSharper disable once UseObjectOrCollectionInitializer
            var windowInteropHelper = new WindowInteropHelper(_initProgressWindow)
            {
                Owner = Autodesk.Windows.ComponentManager.ApplicationWindow
            };

            _initProgressWindow.Show();

            System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(
                new Func<bool>(ProgressWindowWaitHandle.Set));

            //Starts window dispatcher
            System.Windows.Threading.Dispatcher.Run();
        }

        public static void LaunchDynamoCommandForInit()
        {
            if (SelectedProduct == null) return;

            if (DynoRevitManagerBase.RevitDynamoModelInstance == null ||
                DynoRevitManagerBase.RevitDynamoModelInstance != null &&
                DynoRevitManagerBase.RevitDynamoModelInstance.Scheduler == null)
            {
                if (DynoAppBase.Instance.GetSettings().IsShowSplashScreen)
                    using (ProgressWindowWaitHandle = new AutoResetEvent(false))
                    {
                        //Starts the progress window thread
                        var newprogWindowThread = new Thread(ShowInitProgressWindow);
                        newprogWindowThread.SetApartmentState(ApartmentState.STA);
                        newprogWindowThread.IsBackground = true;
                        newprogWindowThread.Start();

                        //Wait for thread to notify that it has created the window
                        ProgressWindowWaitHandle.WaitOne();
                    }

                DynoRevitManagerBase.CommandData.JournalData.Clear();
                DynoRevitManagerBase.CommandData.JournalData.Add(JournalKeys.ShowUiKey, "false");

                var data = new DynamoRevitCommandData
                {
                    JournalData = DynoRevitManagerBase.CommandData.JournalData,
                    Application = new UIApplication(DynoAppBase.App)
                };

                var cmd = new DynamoRevit();
                cmd.ExecuteCommand(data);

                if (DynoAppBase.Instance.GetSettings().IsShowSplashScreen)
                    _initProgressWindow?.Dispatcher.Invoke(_initProgressWindow.Close);
            }

            if (DynoRevitManagerBase.RevitDynamoModelInstance == null)
                throw new Exception("Dynamo core init is failed.");
        }

        public static void LaunchDynamoCommandForOpen()
        {
            if (SelectedProduct == null) return;

            var data = new DynamoRevitCommandData
            {
                JournalData = DynoRevitManagerBase.CommandData.JournalData,
                Application = new UIApplication(DynoAppBase.App)
            };

            var cmd = new DynamoRevit();
            cmd.ExecuteCommand(data);
        }

        private static void ShowNoVersionsDialog()
        {
            var mainDialog =
                new TaskDialog("No Dynamo")
                {
                    MainInstruction = "Dyno can`t detect Dynamo on this Revit version",
                };

            mainDialog.MainContent += "\nProbably you need to reinstall Revit.\n";

            mainDialog.CommonButtons = TaskDialogCommonButtons.Close;
            mainDialog.Show();
        }

        public static string ActiveDynamoProduct => SelectedProduct == null
            ? "No active Dynamo version selected"
            : $"Dynamo {SelectedProduct.VersionInfo.ToString(3)}";

        public static bool CheckActiveDynamoProduct(bool showMultipleProductsMessage = true)
        {
            if (SelectedProduct != null) return true;

            var prod = CheckDsLibrary();
            if (prod == null)
            {
                if (showMultipleProductsMessage) ShowNoVersionsDialog();
                SelectedProduct = null;
                return false;
            }
            return true;
        }

        private static DynamoProduct CheckDsLibrary()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
                if (assembly.FullName.Contains("DynamoRevitDS"))
                {
                    var version = assembly.GetName().Version;
                    var prod = Products.FirstOrDefault(x => x.VersionInfo.ToString(3) == version.ToString(3));
                    if (prod != null) return prod;
                }

            return null;
        }
    }
}