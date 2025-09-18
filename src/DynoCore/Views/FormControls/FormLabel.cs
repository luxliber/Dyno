using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Dyno.Views.FormControls;
using ProtoBuf;

namespace Dyno.FormControls
{
    [ProtoContract]
    public class FormLabel : FormBaseText
    {
        static FormLabel()
        {
           DefaultStyleKeyProperty.OverrideMetadata(typeof(FormLabel), new FrameworkPropertyMetadata(typeof(FormLabel)));
        }

        public FormLabel()
        {
            EditorTextBinding = @"""Label Text""";
            Width = 80;
            Height = 25;
        }

        public static readonly DependencyProperty EditorTextBindingProperty = DependencyProperty.Register("EditorTextBinding", typeof(string), typeof(FormLabel), new FrameworkPropertyMetadata(OnChanged));
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
      
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName("Text Vertical Alignment")]
        [ProtoMember(2)]
        public VerticalAlignment EditorTextVerticalAlignment
        {
            get { return GetValue(VerticalContentAlignmentProperty) is VerticalAlignment ? (VerticalAlignment)GetValue(VerticalContentAlignmentProperty) : VerticalAlignment.Top; }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }

        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName("Text Color")]
        [Editor("Color", "")]


        [ProtoMember(3)]
        public string EditorTextColor
        {
            get
            {
                var b = GetValue(ForegroundProperty) as Brush;
                if (b != null)
                    return b.ToString();

                return "#000000";
            }
            set
            {
                var color = ColorConverter.ConvertFromString(value);
                if (color != null)
                {
                    Brush b = new SolidColorBrush((Color) color);
                    SetValue(ForegroundProperty, b);
                }
                
            }
        }

        public static readonly DependencyProperty EditorTextAlignmentProperty = DependencyProperty.Register("EditorTextAlignment", typeof(TextAlignment), typeof(FormLabel), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName("Text Alignment")]
        [ProtoMember(4)]
        public TextAlignment EditorTextAlignment
        {
            get { return GetValue(EditorTextAlignmentProperty) is TextAlignment ? (TextAlignment)GetValue(EditorTextAlignmentProperty) : TextAlignment.Left; }
            set { SetValue(EditorTextAlignmentProperty, value); }
        }
    }
}
