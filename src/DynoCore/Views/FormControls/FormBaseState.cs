using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using Dyno.Views.FormControls;
using ProtoBuf;

namespace Dyno.FormControls
{
    [ProtoContract]
    [ProtoInclude(1141, typeof(FormCheckBox))]
    [ProtoInclude(1142, typeof(FormRadioButton))]
    public class FormBaseState : FormBaseText
    {
        public override void Update()
        {

            var newCheck = FormControlHelper.FromStringToBool(Values["EditorCheckStatusBinding"]);

            if (newCheck != IsChecked)
            {
                IsChecked = newCheck;
                if (!IsError)
                    OnPropertyChanged(nameof(IsChecked));

            }
            base.Update();

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var el = InternalElement as ToggleButton;

            if (el != null)
            {
                el.Checked += El_Checked;
                el.Unchecked += El_Unchecked;
            }
        }

        private void El_Unchecked(object sender, RoutedEventArgs e)
        {
            Values["EditorCheckStatusBinding"] = false;
            IsChecked = false;

            OnPropertyChanged(nameof(IsChecked));

            if (this is FormRadioButton)
                OnValuesChanged(false);
            else
                OnValuesChanged();


        }

        private void El_Checked(object sender, RoutedEventArgs e)
        {
            Values["EditorCheckStatusBinding"] = true;
            IsChecked = true;
            OnPropertyChanged(nameof(IsChecked));
            OnValuesChanged();

        }

        public bool IsChecked { get; set; }

        public static readonly DependencyProperty EditorTextBindingProperty = DependencyProperty.Register("EditorTextBinding", typeof(string), typeof(FormBaseState), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.Binding)]
        [DisplayName("Text")]
        [Editor("Expression", "")]
        [Expression]
        [ProtoMember(1)]
        public string EditorTextBinding
        {
            get { return GetValue(EditorTextBindingProperty) as string; }
            set { SetValue(EditorTextBindingProperty, value); }
        }

        public static readonly DependencyProperty EditorCheckStatusBindingProperty = DependencyProperty.Register("EditorCheckStatusBinding", typeof(string), typeof(FormBaseState), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.Binding)]
        [DisplayName("Check Status")]
        [Editor("Binding", "")]
        [Binding]
        [ProtoMember(2)]
        public string EditorCheckStatusBinding
        {
            get { return GetValue(EditorCheckStatusBindingProperty) as string; }
            set { SetValue(EditorCheckStatusBindingProperty, value); }
        }
    }
}