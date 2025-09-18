namespace Prorubim.DynoStudio.Props
{
    /// <summary>
    /// Логика взаимодействия для ColorWindow.xaml
    /// </summary>
    public partial class ColorWindow
    {
        public static ColorWindow Instance;

        public ColorWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        public string Color { get; set; }
    }
}
