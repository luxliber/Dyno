using System.ComponentModel;
using System.Windows;
using Dyno.FormControls;
using Dyno.Views.FormControls;
using ProtoBuf;

namespace Dyno.FormControls
{
    [ProtoContract]
    [ProtoInclude(111, typeof(FormBaseBox))]
    [ProtoInclude(112, typeof(FormLabel))]
    [ProtoInclude(113, typeof(FormTextBox))]
    [ProtoInclude(114, typeof(FormBaseState))]
    [ProtoInclude(115, typeof(FormSelectButton))]
    [ProtoInclude(116, typeof(FormNumberBox))]
    
    public class FormBaseText : FormControl
    {

        public FormBaseText()
        {
            EditorFontWeight = 400;
           
           
           
        }

        public override void Update()
        {
            FontWeight = FontWeight.FromOpenTypeWeight(EditorFontWeight == 0 ? 400 : EditorFontWeight);
            base.Update();
        }


        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName("Font Size")]
        [Editor("FontSize", "")]
        [ProtoMember(1)]
        public double EditorFontSize
        {
            get { return GetValue(FontSizeProperty) is double ? (double)GetValue(FontSizeProperty) : 12; }
            set
            {
                SetValue(FontSizeProperty, value);
            }
        }

        public static readonly DependencyProperty EditorWeightProperty = DependencyProperty.Register("EditorFontWeight", typeof(int), typeof(FormBaseText), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName("Font Weight")]
        [Editor("FontWeight", "")]
        [ProtoMember(2)]
        public int EditorFontWeight
        {
            get { return GetValue(EditorWeightProperty) is int ? (int)GetValue(EditorWeightProperty) : 400; }
            set { SetValue(EditorWeightProperty, value); }
        }
    }
}