using System;
using System.Windows;
using System.Windows.Controls;
using Dyno.Forms;
using Prorubim.DynoStudio.ViewModels;
using ColorWindow = Prorubim.DynoStudio.Props.ColorWindow;

namespace Prorubim.DynoStudio
{
    /// <summary>
    /// Interaction logic for App.xaml and app init
    /// </summary>
    [Serializable]
    public partial class App
    {
        public static App Instance;
        public SettingsWindow Sw; 
   
        public static DynoManager Manager;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Instance = this;

            Manager = new DynoManager();
        }

        private void ColorButtonSelectorClick(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            if (b != null && ColorWindow.Instance != null)
                ColorWindow.Instance.Color = b.Background.ToString();

            if (ColorWindow.Instance != null)
                ColorWindow.Instance.Close();

            ColorWindow.Instance = null;
        }
    }
}
