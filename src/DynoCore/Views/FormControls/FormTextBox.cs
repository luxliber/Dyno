using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dyno.Views.FormControls;
using ProtoBuf;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Dyno.FormControls
{
    [ProtoContract]
   
    public class FormTextBox : FormBaseText
    {
        static FormTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormTextBox), new FrameworkPropertyMetadata(typeof(FormTextBox)));
        }

        public FormTextBox()
        {
            EditorWidth = 150;
            EditorHeight = 30;
            EditorTextWrapping = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {


            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                var textBox = InternalElement as TextBox;
                if (textBox != null)
                {
                    if (double.TryParse(textBox.Text, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double x))
                        Values["EditorTextBinding"] = x;
                    else
                        Values["EditorTextBinding"] = textBox.Text;

                }
                OnValuesChanged();
            }
            base.OnKeyDown(e);
        }

        private static void OnRegexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as FormTextBox;
            if (c == null) return;

            if (!string.IsNullOrEmpty(c.EditorValidationRegEx))
                c.ValidationRegEx = new Regex($"^{c.EditorValidationRegEx}$", RegexOptions.ExplicitCapture);
            else
                c.ValidationRegEx = null;

            FormControlHelper.ValidateControlFromBindings(c);

        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            var textBox = InternalElement as TextBox;
            if (textBox != null)
                if (double.TryParse(textBox.Text, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double x))
                    Values["EditorTextBinding"] = x;
                else
                    Values["EditorTextBinding"] = textBox.Text;

            OnValuesChanged();
            base.OnLostKeyboardFocus(e);
        }

        public static readonly DependencyProperty EditorTextBindingProperty = DependencyProperty.Register("EditorTextBinding", typeof(string), typeof(FormTextBox), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.Binding)]
        [DisplayName(@"Text")]
        [Editor("Binding", "")]
        [Binding]
        [ProtoMember(1)]
        public string EditorTextBinding
        {
            get { return (string)GetValue(EditorTextBindingProperty); }
            set { SetValue(EditorTextBindingProperty, value); }
        }

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("EditorTextAlignment", typeof(TextAlignment), typeof(FormTextBox), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName(@"Text Alignment")]
        [ProtoMember(2)]
        public TextAlignment EditorTextAlignment
        {
            get { return GetValue(TextAlignmentProperty) is TextAlignment ? (TextAlignment)GetValue(TextAlignmentProperty) : TextAlignment.Left; }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(FormTextBox), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.Content)]
        [DisplayName("Text Wrapping")]
        [Description("Mult-iline or single-line text mode")]
        [ProtoMember(3)]
        public bool EditorTextWrapping
        {
            get { return GetValue(TextWrappingProperty) is TextWrapping && (TextWrapping)GetValue(TextWrappingProperty) == TextWrapping.Wrap; }
            set { SetValue(TextWrappingProperty, value ? TextWrapping.Wrap : TextWrapping.NoWrap); }
        }

        public static readonly DependencyProperty EditorValidationRegExProperty = DependencyProperty.Register("EditorValidationRegEx", typeof(string), typeof(FormTextBox), new FrameworkPropertyMetadata(OnRegexChanged));
        [Category(FormControlHelper.PropCategory.Content)]
        [DisplayName("Validation RegEx")]
        [Description("Regular Expression for validation text inside textbox")]
        [ProtoMember(4)]
        public string EditorValidationRegEx
        {
            get { return (string)GetValue(EditorValidationRegExProperty); }
            set { SetValue(EditorValidationRegExProperty, value); }
        }

        internal Regex ValidationRegEx;

        public static readonly DependencyProperty EditorErrorTooltipProperty = DependencyProperty.Register("EditorErrorTooltip", typeof(string), typeof(FormTextBox), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.Content)]
        [DisplayName("Error Tooltip")]
        [Description("Tooltip help text on Validation error")]
        [Editor("Expression", "")]
        [Expression]
        [ProtoMember(5)]
        public string EditorErrorTooltip
        {
            get { return (string)GetValue(EditorErrorTooltipProperty); }
            set { SetValue(EditorErrorTooltipProperty, value); }
        }
    }
}
