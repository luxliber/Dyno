namespace DynoUI
{
    /// <summary>
    /// Interaction logic for IntSliderWindow.xaml
    /// </summary>
    public partial class BooleanWindow
    {
        public BooleanWindow()
        {
            InitializeComponent();
        }

        public BooleanWindow(bool val, string falsetext, string truetext)
            : this()
        {
            FalseButton.IsChecked = !val;
            FalseButton.Content = falsetext;
            TrueButton.IsChecked = val;
            TrueButton.Content = truetext;
        }

    }
}