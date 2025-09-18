using System.ComponentModel;
using System.Windows;
using Dyno.Views.FormControls;
using ProtoBuf;

namespace Dyno.FormControls
{
    [ProtoContract]
    public class FormRadioButton : FormBaseState
    {
        static FormRadioButton()
        {
           DefaultStyleKeyProperty.OverrideMetadata(typeof(FormRadioButton), new FrameworkPropertyMetadata(typeof(FormRadioButton)));
        }

       

        public FormRadioButton()
        {
            EditorTextBinding = @"""Radiobutton Text""";
            Width = 150;
            Height = 25;
            EditorGroupName = "";
        }

        public static readonly DependencyProperty EditorGroupNameProperty = DependencyProperty.Register("EditorGroupName", typeof(string), typeof(FormRadioButton), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName(@"GroupName")]
        [ProtoMember(1)]
        public string EditorGroupName
        {
            get { return (string) GetValue(EditorGroupNameProperty); }
            set { SetValue(EditorGroupNameProperty, value); }
        }
    }
}
