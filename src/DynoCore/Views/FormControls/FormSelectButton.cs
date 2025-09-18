using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Dyno.Models.Parameters;
using Dyno.ViewModels;
using Dyno.Views.FormControls;
using ProtoBuf;

namespace Dyno.FormControls
{
    [ProtoContract]
    public class FormSelectButton : FormBaseText
    {
        static FormSelectButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormSelectButton), new FrameworkPropertyMetadata(typeof(FormSelectButton)));
        
        }

        public FormSelectButton()
        {
            EditorText = "Button";
            EditorTextAlignment = HorizontalAlignment.Center;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (InternalElement is Button el) el.Click += El_Click;
        }

        private void El_Click(object sender, RoutedEventArgs e)
        {
            var par = DynoManagerBase.SelectedWorkspacePreset.GetParameterByName(EditorSelectionBinding);
            if (par == null) return;

            if (par.GetType() == typeof(SelectElementParameter))
            {
                var spar = par as SelectElementParameter;
                DynoManagerBase.SelectedWorkspacePreset.Workspace.ManagerCollector.SelectElements(spar);
            }

            else if (par.GetType() == typeof(SelectReferenceParameter))
            {
                var spar = par as SelectReferenceParameter;
                DynoManagerBase.SelectedWorkspacePreset.Workspace.ManagerCollector.SelectReference(spar);
            }

            else if (par.GetType() == typeof(PathParameter))
            {
                var spar = par as PathParameter;
                DynoManagerBase.SelectedWorkspacePreset.Workspace.ManagerCollector.SelectFile(spar);
            }


            Form.FillAllControls(this);
        }

        [Category(FormControlHelper.PropCategory.Binding)]
        [DisplayName(@"Selection node")]
        [Editor("Binding", "")]
        [Binding]
        [ProtoMember(1)]
        public string EditorSelectionBinding
        {
            get { return GetValue(EditorSelectionBindingProperty) as string; }
            set { SetValue(EditorSelectionBindingProperty, value); }
        }
        public static readonly DependencyProperty EditorSelectionBindingProperty =
            DependencyProperty.Register("EditorSelectionBinding", typeof(string), typeof(FormSelectButton), new FrameworkPropertyMetadata(OnChanged));

        [Category(FormControlHelper.PropCategory.Content)]
        [DisplayName(@"Text")]
        [ProtoMember(2)]
        public string EditorText
        {
            get
            {
                return GetValue(EditorTextProperty) as string;
            }
            set
            {
                SetValue(EditorTextProperty, value);
                
            }
        }
        public static readonly DependencyProperty EditorTextProperty =
            DependencyProperty.Register("EditorText", typeof(string), typeof(FormSelectButton), new FrameworkPropertyMetadata(OnChanged));


        public static readonly DependencyProperty EditorTextAlignmentProperty = DependencyProperty.Register("EditorTextAlignment", typeof(HorizontalAlignment), typeof(FormBaseText), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName("Text Alignment")]
        [ProtoMember(3)]
        public HorizontalAlignment EditorTextAlignment
        {
            get { return GetValue(EditorTextAlignmentProperty) is HorizontalAlignment ? (HorizontalAlignment)GetValue(EditorTextAlignmentProperty) : HorizontalAlignment.Left; }
            set { SetValue(EditorTextAlignmentProperty, value); }
        }
    }
    

}
