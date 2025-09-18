using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Dyno.Annotations;
using Dyno.Forms;
using WPG.Data;

namespace Prorubim.DynoStudio.Props
{
    internal class ExpressionButton : Button, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ExpressionButton));

        protected override void OnClick()
        {
            var bindingExpression = GetBindingExpression(TextProperty);
            if (bindingExpression == null) return;

            var prop = bindingExpression.ResolvedSource as Property;

            var ew = new ExpressionWindow { Text = { Text = Text } };
           
            ew.ShowDialog();

            if (ew.DialogResult != true) return;

            if (prop != null)
                prop.Value = ew.Text.Text;
        }



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                OnPropertyChanged(nameof(Text));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}