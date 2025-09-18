using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Prorubim.PackageBootstrapperUI.Annotations;

namespace Prorubim.PackageBootstrapperUI
{
    public class MainWindowModel : INotifyPropertyChanged
    {

        public BootstrapperApplication Bootstrapper { get; }
        public bool IsThinking { get; set; }

        private bool installEnabled;
        public bool InstallEnabled
        {
            get => installEnabled;
            set
            {
                installEnabled = value;
                OnPropertyChanged(nameof(InstallEnabled));
            }
        }
        private bool uninstallEnabled;
        public bool UninstallEnabled
        {
            get => uninstallEnabled;
            set
            {
                uninstallEnabled = value;
                OnPropertyChanged(nameof(UninstallEnabled));
            }
        }

        public MainWindowModel(BootstrapperApplication app)
        {
            Bootstrapper = app;
            IsThinking = false;

            Bootstrapper.ApplyComplete += OnApplyComplete;
            Bootstrapper.DetectPackageComplete += OnDetectPackageComplete;
            Bootstrapper.PlanComplete += OnPlanComplete;
        }

        private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
        {

        }

        private void OnDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
        {
            if (e.PackageId == "InstallationPackageId")
            {
                if (e.State == PackageState.Absent)
                    InstallEnabled = true;
                else if (e.State == PackageState.Present)
                {
                    UninstallEnabled = true;
                    if (args.Length != 0 && args == "/IsCacheFile")
                        return;

                    Launcher.CheckInstalledInstance();
                }
            }
        }

        private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
        {

        }



        public RelayCommand InstallCommand => new RelayCommand(o => { InstallExecute(); });
        public RelayCommand UninstallCommand => new RelayCommand(o => { UninstallExecute(); });
        public RelayCommand ExitCommand => new RelayCommand(o => { ExitExecute(); });

        private void InstallExecute()
        {
            IsThinking = true;
            Bootstrapper.Engine.Plan(LaunchAction.Install);
        }
        private void UninstallExecute()
        {
            IsThinking = true;
            Bootstrapper.Engine.Plan(LaunchAction.Uninstall);
        }
        private void ExitExecute() => App.BootstrapperDispatcher.InvokeShutdown();

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
