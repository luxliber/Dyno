using System.Windows;
using Dyno.FormControls;
using ProtoBuf;

namespace Dyno.FormControls
{
    [ProtoContract]
    public class FormCheckBox : FormBaseState
    {
        static FormCheckBox()
        {
           DefaultStyleKeyProperty.OverrideMetadata(typeof(FormCheckBox), new FrameworkPropertyMetadata(typeof(FormCheckBox)));
        }

        public FormCheckBox()
        {
            EditorTextBinding = @"""Checkbox Text""";
            Width = 150;
            Height = 25;
        }
    }
}
