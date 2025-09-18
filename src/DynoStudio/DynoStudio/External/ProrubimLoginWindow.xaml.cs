using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Prorubim.DynoStudio.External;

namespace Prorubim.External
{
    /// <summary>
    /// Interaction logic for ProrubimLoginWindow.xaml
    /// </summary>
    internal partial class ProrubimLoginWindow : Window
    {
        public ProrubimLoginWindow()
        {
            InitializeComponent();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProrubimExternal.Request(LoginBox.Text, PasswordBox.Password);
                

                DialogResult = true;
            }
            catch (Exception ex)
            {
                StatusBox.Text = ex.Message;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
