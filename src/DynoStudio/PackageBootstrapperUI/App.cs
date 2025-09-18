using System.Diagnostics;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace Prorubim.PackageBootstrapperUI
{
    public class App : BootstrapperApplication
    {
        // global dispatcher
        public static Dispatcher BootstrapperDispatcher { get; private set; }
        internal static MainWindowModel ViewModel;
        internal static MainWindow View;

        public static string Args = string.Empty;
        // entry point for our custom UI
        protected override void Run()
        {
            //Debugger.Launch();

            Engine.Log(LogLevel.Verbose, "Launching custom App UX");

            BootstrapperDispatcher = Dispatcher.CurrentDispatcher;

            if (!Launcher.IsRunAsAdmin())
                Launcher.RunAsAdministrator();
            
            ViewModel = new MainWindowModel(this);
            ViewModel.Bootstrapper.Engine.Detect();

            var mainWindow = new MainWindow {DataContext = ViewModel};
            mainWindow.Closed += (sender, e) => BootstrapperDispatcher.InvokeShutdown();

            mainWindow.Show();

            Dispatcher.Run();
            Engine.Quit(0);
        }
    }
}
