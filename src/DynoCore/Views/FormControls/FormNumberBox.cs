using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dyno.Views.FormControls;
using ProtoBuf;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Dyno.FormControls
{
    [ProtoContract]
    public class FormNumberBox : FormBaseText
    {
        private Button _upButton;
        private Button _downButton;

        static FormNumberBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormNumberBox), new FrameworkPropertyMetadata(typeof(FormNumberBox)));
        }

        public FormNumberBox()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _upButton = (Button)Template.FindName("UpButton", this);
            _downButton = (Button)Template.FindName("DownButton", this);

            _upButton.Click += UpButton_Click;
            _downButton.Click += DownButton_Click;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (InternalElement is TextBox textBox)
                if (double.TryParse(textBox.Text, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double x))
                {
                    if (double.TryParse(Values["EditorStep"].ToString(), out double y))
                        x -= y;
                    else
                        x -= 1;

                    if (double.TryParse(Values["EditorMin"].ToString(), out double min) && x <= min)
                        x = min;

                    Values["EditorTextBinding"] = x;
                    OnValuesChanged();
                }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (InternalElement is TextBox textBox)
                if (double.TryParse(textBox.Text, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double x))
                {
                    if (double.TryParse(Values["EditorStep"].ToString(), out double y))
                        x += y;
                    else
                        x += 1;

                    if (double.TryParse(Values["EditorMax"].ToString(), out double max) && x >= max)
                        x = max;

                    Values["EditorTextBinding"] = x;
                    OnValuesChanged();
                }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                var textBox = InternalElement as TextBox;
                if (textBox != null)
                {
                    if (double.TryParse(textBox.Text, out double x))
                        Values["EditorTextBinding"] = x;
                    else
                        textBox.Text = Values["EditorTextBinding"].ToString();

                    if (double.TryParse(Values["EditorMax"].ToString(), out double max) && x >= max)
                    {
                        Values["EditorTextBinding"] = max;
                        textBox.Text = max.ToString(CultureInfo.InvariantCulture);
                    }

                    if (double.TryParse(Values["EditorMin"].ToString(), out double min) && x <= min)
                    {
                        Values["EditorTextBinding"] = min;
                        textBox.Text = min.ToString(CultureInfo.InvariantCulture);
                    }

                    
                    OnValuesChanged();
                }

            }
            base.OnKeyDown(e);
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            var textBox = InternalElement as TextBox;
            if (textBox != null)
            {
                if (double.TryParse(textBox.Text, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double x))
                    Values["EditorTextBinding"] = x;
                else
                    textBox.Text = Values["EditorTextBinding"].ToString();

                if (double.TryParse(Values["EditorMax"].ToString(), NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double max) && x >= max)
                {
                    Values["EditorTextBinding"] = max;
                    textBox.Text = max.ToString(CultureInfo.InvariantCulture);
                }

                if (double.TryParse(Values["EditorMin"].ToString(), NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double min) && x <= min)
                {
                    Values["EditorTextBinding"] = min;
                    textBox.Text = min.ToString();
                }
                
                OnValuesChanged();
            }

            base.OnLostKeyboardFocus(e);
        }

        public static readonly DependencyProperty EditorTextBindingProperty = DependencyProperty.Register("EditorTextBinding", typeof(string), typeof(FormNumberBox), new FrameworkPropertyMetadata(OnChanged));
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

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("EditorTextAlignment", typeof(TextAlignment), typeof(FormNumberBox), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName(@"Text Alignment")]
        [ProtoMember(2)]
        public TextAlignment EditorTextAlignment
        {
            get { return GetValue(TextAlignmentProperty) is TextAlignment ? (TextAlignment)GetValue(TextAlignmentProperty) : TextAlignment.Left; }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public static readonly DependencyProperty EditorStepProperty = DependencyProperty.Register("EditorStep", typeof(string), typeof(FormNumberBox), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.Content)]
        [DisplayName("Step")]
        [Description("Counter Step Value")]
        [Editor("Expression", "")]
        [Expression]
        [ProtoMember(3)]
        public string EditorStep
        {
            get => (string)GetValue(EditorStepProperty);
            set => SetValue(EditorStepProperty, value);
        }

        public static readonly DependencyProperty EditorMaxProperty = DependencyProperty.Register("EditorMax", typeof(string), typeof(FormNumberBox), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.Content)]
        [DisplayName("Max Limit")]
        [Description("Counter Max Value")]
        [Editor("Expression", "")]
        [Expression]
        [ProtoMember(4)]
        public string EditorMax
        {
            get => (string)GetValue(EditorMaxProperty);
            set => SetValue(EditorMaxProperty, value);
        }

        public static readonly DependencyProperty EditorMinProperty = DependencyProperty.Register("EditorMin", typeof(string), typeof(FormNumberBox), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.Content)]
        [DisplayName("Min Limit")]
        [Description("Counter Min Value")]
        [Editor("Expression", "")]
        [Expression]
        [ProtoMember(5)]
        public string EditorMin
        {
            get => (string)GetValue(EditorMinProperty);
            set => SetValue(EditorMinProperty, value);
        }
    }
}
