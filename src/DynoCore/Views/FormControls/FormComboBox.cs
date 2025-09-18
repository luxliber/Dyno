using System.Windows;
using System.Windows.Controls;
using Dyno.Views.FormControls;
using ProtoBuf;

namespace Dyno.FormControls
{
    [ProtoContract]
    public class FormComboBox : FormBaseBox
    {
        static FormComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormComboBox), new FrameworkPropertyMetadata(typeof(FormComboBox)));
        }

        public FormComboBox()
        {
            Width = 150;
            Height = 30;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var listBox = InternalElement as ComboBox;

            if (listBox != null)
                listBox.SelectionChanged += ListBox_SelectionChanged;

            OnPropertyChanged(nameof(SelectedItem));

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Form.PortPars.Count == 0)
            {
                FormControlHelper.UpdateControlValuesFromBindings(this);
                FormControlHelper.UpdateControlValuesFromExpressions(this);
            }

            OnValuesChanged();
        }
    }
}
