using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Dyno.Annotations;
using WPG.Data;

namespace Prorubim.DynoStudio.Props
{
    public class ColorButton : Button, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(string), typeof(ColorButton));
     
        protected override void OnClick()
        {

            var bindingExpression = GetBindingExpression(ColorProperty);
            if (bindingExpression == null) return;

            var prop = bindingExpression.ResolvedSource as Property;

            var colorWindow = new ColorWindow {Color = Color};

            colorWindow.ShowDialog();
        
            if (prop != null)
                prop.Value = colorWindow.Color;
        }

        public string Color
        {
            get
            {
                return (string)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
                OnPropertyChanged(nameof(Color));
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