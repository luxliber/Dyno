using System.Windows;

namespace DynoUI
{
    /// <summary>
    /// Interaction logic for IntSliderWindow.xaml
    /// </summary>
    public partial class IntSliderWindow
    {
        public int Min { get; set; }
        public int Max { get; set; }


        public IntSliderWindow()
        {
            InitializeComponent();
            TitleControl.Cancel.Click += OnCancel;
        }

        public IntSliderWindow(int min, int max, int value) : this()
        {
         
            IntValue = value;

            Min = min;
            Max = max;
        }

        public int IntValue { get; set; }

        public double DoubleValue
        {
            get { return IntValue/(double) (Max - Min); }
            set { IntValue = (int) (value * (Max - Min)); }
        }


        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}