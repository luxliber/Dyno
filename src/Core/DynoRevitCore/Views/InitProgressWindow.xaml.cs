using System;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;


namespace Prorubim.DynoRevitCore.Views
{
    /// <summary>
    /// Логика взаимодействия для InitProgressWindow.xaml
    /// </summary>
    public partial class InitProgressWindow
    {
        public InitProgressWindow()
        {
            InitializeComponent();

            var random = new Random();
            var number = random.Next(20) + 1;
            var uri = new Uri($"pack://application:,,,/DynoRevitCore;component/Images/progress{number}.gif");

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = uri;
            image.EndInit();

            ImageBehavior.SetAnimatedSource(ProgressImage, image);
        }
        
      
    }
}
