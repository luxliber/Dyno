using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Dyno.ViewModels;
using Dyno.Views.FormControls;
using ProtoBuf;

namespace Dyno.FormControls
{
    [ProtoContract]
    [ProtoInclude(1111, typeof(FormListBox))]
    [ProtoInclude(1112, typeof(FormComboBox))]
    [ProtoInclude(1113, typeof(FormNumberBox))]
    public class FormBaseBox : FormBaseText
    {
        public override void Update()
        {
            base.Update();
            OnPropertyChanged(nameof(ItemsList));
            OnPropertyChanged(nameof(SelectedItem));
        }

        public static readonly DependencyProperty EditorItemsBindingProperty = DependencyProperty.Register("EditorItemsBinding", typeof(string), typeof(FormBaseBox), new FrameworkPropertyMetadata(OnChanged));


        [Category(FormControlHelper.PropCategory.Binding)]
        [DisplayName("List Items")]
        [Editor("Binding", "")]
        [Binding]
        [ProtoMember(1)]
        public string EditorItemsBinding
        {
            get { return GetValue(EditorItemsBindingProperty) as string; }
            set { SetValue(EditorItemsBindingProperty, value); }
        }

        public static readonly DependencyProperty EditorTextAlignmentProperty = DependencyProperty.Register("EditorTextAlignment", typeof(HorizontalAlignment), typeof(FormBaseBox), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName("Text Alignment")]
        [ProtoMember(2)]
        public HorizontalAlignment EditorTextAlignment
        {
            get { return GetValue(EditorTextAlignmentProperty) is HorizontalAlignment ? (HorizontalAlignment)GetValue(EditorTextAlignmentProperty) : HorizontalAlignment.Left; }
            set { SetValue(EditorTextAlignmentProperty, value); }
        }

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(FormBaseBox), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.Binding)]
        [DisplayName("Multiple Selection")]
        [ProtoMember(3)]
        public bool EditorMultipleSelection
        {
            get { return GetValue(SelectionModeProperty) is SelectionMode && (SelectionMode)GetValue(SelectionModeProperty) == SelectionMode.Extended; }
            set { SetValue(SelectionModeProperty, value ? SelectionMode.Extended : SelectionMode.Single); }
        }

        public IList ItemsList
        {
            get
            {

                if (string.IsNullOrEmpty(EditorItemsBinding)) return new ArrayList();

                if (DynoManagerBase.SelectedWorkspacePreset != null)
                {
                    var par = DynoManagerBase.SelectedWorkspacePreset != null ? DynoManagerBase.SelectedWorkspacePreset.GetParameterByName(EditorItemsBinding) : null;
                    if (par?.Values != null)
                        return par.Values;
                }

                var portParBinding = Form.PortPars.Keys.FirstOrDefault(x => x == EditorItemsBinding);

                if (portParBinding != null)
                    return Form.PortPars[portParBinding].Values;

                var testPar = Form.UserPars.FirstOrDefault(x => x.Name == EditorItemsBinding);
                if (testPar != null)
                    return new ArrayList { testPar.Value };

                return new ArrayList { "None" };
            }
        }

        public object SelectedItem
        {
            get
            {
                return Values.ContainsKey("EditorItemsBinding") ? Values["EditorItemsBinding"] : "";

            }

            set
            {
                Values["EditorItemsBinding"] = value;
                OnValuesChanged();
            }
        }

        public IList SelectedItems { get; set; }
    }
}