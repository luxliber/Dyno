using System.Windows;
using System.Windows.Controls;
using Dyno.Views.FormControls;
using ProtoBuf;

namespace Dyno.FormControls
{
    [ProtoContract]
    public class FormListBox : FormBaseBox
    {
        private ListBox _listBox;

        static FormListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormListBox), new FrameworkPropertyMetadata(typeof(FormListBox)));
        }

        public FormListBox()
        {
            Width = 150;
            Height = 100;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _listBox = InternalElement as ListBox;

            if (_listBox != null)
                _listBox.SelectionChanged += ListBox_SelectionChanged;

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Form.PortPars.Count == 0)
            {
                FormControlHelper.UpdateControlValuesFromBindings(this);
                FormControlHelper.UpdateControlValuesFromExpressions(this);
            }

            SelectedItems = _listBox.SelectedItems;
            OnValuesChanged();
        }
    }
}
