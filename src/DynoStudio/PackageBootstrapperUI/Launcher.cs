using System;
using System.Diagnostics;
using System.Security.Principal;

namespace Prorubim.PackageBootstrapperUI
{
    public static class Launcher
    {
        public static void RunAsAdministrator()
        {
            var directory = Environment.CurrentDirectory;
            var fileName = Process.GetCurrentProcess().MainModule.ModuleName;
            var path = directory + $@"\{fileName}";
            // Launch itself as administrator
            var proc = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = path,
                Verb = "runas"
            };

            try
            {
                Process.Start(proc);
            }
            catch (Exception)
            {
                // The user refused the elevation.
                Environment.Exit(0);
            }
            Environment.Exit(0); // Quit itself

        }

        public static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
