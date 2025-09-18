using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Dyno.Annotations;
using Dyno.FormControls;
using Dyno.Models.Forms;
using Dyno.ViewModels;
using Dyno.Views.FormControls;
using ProtoBuf;
using Expression = CalcEngine.Expression;

namespace Dyno.FormControls
{
    [ProtoContract]
    [ProtoInclude(11, typeof(FormBaseText))]
    [ProtoInclude(12, typeof(FormImage))]
    public class FormControl : UserControl, IFormControl, INotifyPropertyChanged
    {
        internal FrameworkElement InternalElement;
        public bool IsActivated;

        public Thickness OldRect;
        public Size OldSize;

        public WorkspaceForm Form { get; set; }

        public FormControl SetPosition(double left, double top, double right = 0, double bottom = 0)
        {
            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
            Canvas.SetRight(this, right);
            Canvas.SetBottom(this, bottom);

            return this;
        }

        public FormControl()
        {
            EditorHorizontalAlignment = HorizontalAlignment.Left;
            EditorVerticalAlignment = VerticalAlignment.Top;

            EditorWidth = 100;
            EditorHeight = 30;
        }

        public void UpdatePosition()
        {
            HorizontalAlignment = EditorHorizontalAlignment;
            VerticalAlignment = EditorVerticalAlignment;

            var left = 0d;
            var right = 0d;
            var top = 0d;
            var bottom = 0d;

            if (HorizontalAlignment == HorizontalAlignment.Left || HorizontalAlignment == HorizontalAlignment.Stretch || HorizontalAlignment == HorizontalAlignment.Center)
                left = EditorLeft;
            if (HorizontalAlignment == HorizontalAlignment.Right || HorizontalAlignment == HorizontalAlignment.Stretch || HorizontalAlignment == HorizontalAlignment.Center)
                right = EditorRight;
            if (VerticalAlignment == VerticalAlignment.Top || VerticalAlignment == VerticalAlignment.Stretch || VerticalAlignment == VerticalAlignment.Center)
                top = EditorTop;
            if (VerticalAlignment == VerticalAlignment.Bottom || VerticalAlignment == VerticalAlignment.Stretch || VerticalAlignment == VerticalAlignment.Center)
                bottom = EditorBottom;

            //temporaly disable center margins
            if (HorizontalAlignment == HorizontalAlignment.Center)
                left = right = 0;
            if (VerticalAlignment == VerticalAlignment.Center)
                top = bottom = 0;

            Margin = new Thickness(left, top, right, bottom);

            Width = EditorHorizontalAlignment == HorizontalAlignment.Stretch ? double.NaN : EditorWidth;
            Height = EditorVerticalAlignment == VerticalAlignment.Stretch ? double.NaN : EditorHeight;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DataContext = this;
            InternalElement = Template.FindName("internal", this) as FrameworkElement;
        }

        public virtual void Update()
        {
            if (!IsActivated) return;

            FormControlHelper.UpdateCommonVisualProperties(this);


            OnPropertyChanged(nameof(Values));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, object> Values { get; set; } = new Dictionary<string, object>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, Expression> Expressions { get; set; } = new Dictionary<string, Expression>();

        public bool IsError { get; set; }

        internal static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FormControl c) || !c.IsActivated) return;

            c.UpdatePosition();

            if (c.Form.PortPars.Count != 0) return;

            FormControlHelper.UpdateControlValuesFromBindings(d as Control);
            FormControlHelper.UpdateControlValuesFromExpressions(d as Control);
        }

        internal void OnValuesChanged(bool forceFillAllControls = true)
        {
            if (Form.PortPars.Count > 0)
                WorkspaceForm.UpdateNodeBinding(Form.PortPars, this);
            else if (DynoManagerBase.SelectedWorkspacePreset != null)
                WorkspaceForm.UpdatePresetParameter(DynoManagerBase.SelectedWorkspacePreset, this);

            if (forceFillAllControls)
                Form.FillAllControls(this);

            Form.OnPropertyChanged(nameof(WorkspaceForm.IsNoError));
        }

        public static readonly DependencyProperty EditorIsEnabedBindingProperty = DependencyProperty.Register("EditorIsEnabedBinding", typeof(string), typeof(FormControl), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName("Is Enabled")]
        [Editor("Expression", "")]
        [Expression]
        [ProtoMember(1)]
        public string EditorIsEnabedBinding
        {
            get { return GetValue(EditorIsEnabedBindingProperty) as string; }
            set { SetValue(EditorIsEnabedBindingProperty, value); }
        }

        public static readonly DependencyProperty EditorIsVisibleBindingProperty = DependencyProperty.Register("EditorIsVisibleBinding", typeof(string), typeof(FormControl), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.VisualStyle)]
        [DisplayName("Is Visible")]
        [Editor("Expression", "")]
        [Expression]
        [ProtoMember(2)]
        public string EditorIsVisibleBinding
        {
            get { return GetValue(EditorIsVisibleBindingProperty) as string; }
            set { SetValue(EditorIsVisibleBindingProperty, value); }
        }

        public static readonly DependencyProperty EditorWidthProperty = DependencyProperty.Register("EditorWidth", typeof(double), typeof(FormControl), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.Size)]
        [DisplayName("Width")]
        [ProtoMember(3)]
        public double EditorWidth
        {
            get { return (double)GetValue(EditorWidthProperty); }
            set
            {
                SetValue(EditorWidthProperty, value);
            }
        }

        [Category(FormControlHelper.PropCategory.Size)]
        [DisplayName("Height")]
        [ProtoMember(4)]
        public double EditorHeight
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        [Category(FormControlHelper.PropCategory.Position)]
        [DisplayName("Top")]
        [ProtoMember(5)]
        public double EditorTop
        {
            get
            {
                var res = (double)GetValue(Canvas.TopProperty);
                return !double.IsNaN(res) ? res : 0;
            }
            set
            {
                SetValue(Canvas.TopProperty, value);
                UpdatePosition();
            }
        }

        [Category(FormControlHelper.PropCategory.Position)]
        [DisplayName("Left")]
        [ProtoMember(6)]
        public double EditorLeft
        {
            get
            {
                var res = (double)GetValue(Canvas.LeftProperty);
                return !double.IsNaN(res) ? res : 0;
            }
            set
            {
                SetValue(Canvas.LeftProperty, value);
                UpdatePosition();
            }
        }

        [Category(FormControlHelper.PropCategory.Position)]
        [DisplayName("Bottom")]
        [ProtoMember(51)]
        public double EditorBottom
        {
            get
            {
                var res = (double)GetValue(Canvas.BottomProperty);
                return !double.IsNaN(res) ? res : 0;
            }
            set
            {
                SetValue(Canvas.BottomProperty, value);
                UpdatePosition();
            }
        }

        [Category(FormControlHelper.PropCategory.Position)]
        [DisplayName("Right")]
        [ProtoMember(61)]
        public double EditorRight
        {
            get
            {
                var res = (double)GetValue(Canvas.RightProperty);
                return !double.IsNaN(res) ? res : 0;
            }
            set
            {
                SetValue(Canvas.RightProperty, value);
                UpdatePosition();
            }
        }

        public static readonly DependencyProperty EditorHorizontalAlignmentProperty = DependencyProperty.Register("EditorHorizontalAlignment", typeof(HorizontalAlignment), typeof(FormControl), new FrameworkPropertyMetadata(OnChanged));
        [Category(FormControlHelper.PropCategory.Position)]
        [DisplayName("Horizontal Alignment")]
        [ProtoMember(72)]
        public HorizontalAlignment EditorHorizontalAlignment
        {
            get
            {

                return (HorizontalAlignment)GetValue(EditorHorizontalAlignmentProperty);
            }
            set
            {
                SetValue(EditorHorizontalAlignmentProperty, value);
                UpdatePosition();
            }
        }

        public static readonly DependencyProperty EditorVerticalAlignmentProperty = DependencyProperty.Register("EditorVerticalAlignment", typeof(VerticalAlignment), typeof(FormControl), new FrameworkPropertyMetadata(OnChanged));
        [ProtoMember(73)]
        [DisplayName("Vertical Alignment")]
        [Category(FormControlHelper.PropCategory.Position)]
        public VerticalAlignment EditorVerticalAlignment
        {
            get
            {
                return (VerticalAlignment)GetValue(EditorVerticalAlignmentProperty);
            }
            set
            {
                SetValue(EditorVerticalAlignmentProperty, value);
                UpdatePosition();
            }
        }

        [ProtoMember(74)]
        public int ZPos
        {
            get
            {
                return Panel.GetZIndex(this);
            }
            set
            {
                Panel.SetZIndex(this, value);
                UpdatePosition();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MemoryStream Serialize()
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, this);
            return ms;
        }

        public static FormControl Deserialize(KeyValuePair<Type, byte[]> bi)
        {
            FormControl item = null;
            try
            {
                using (var ms = new MemoryStream(bi.Value))
                {
                    if (bi.Key == typeof(FormLabel))
                        item = Serializer.Deserialize<FormLabel>(ms);
                    if (bi.Key == typeof(FormTextBox))
                        item = Serializer.Deserialize<FormTextBox>(ms);
                    if (bi.Key == typeof(FormNumberBox))
                        item = Serializer.Deserialize<FormNumberBox>(ms);
                    if (bi.Key == typeof(FormCheckBox))
                        item = Serializer.Deserialize<FormCheckBox>(ms);
                    if (bi.Key == typeof(FormListBox))
                        item = Serializer.Deserialize<FormListBox>(ms);
                    if (bi.Key == typeof(FormComboBox))
                        item = Serializer.Deserialize<FormComboBox>(ms);
                    if (bi.Key == typeof(FormImage))
                        item = Serializer.Deserialize<FormImage>(ms);
                    if (bi.Key == typeof(FormRadioButton))
                        item = Serializer.Deserialize<FormRadioButton>(ms);
                    if (bi.Key == typeof(FormSelectButton))
                        item = Serializer.Deserialize<FormSelectButton>(ms);
                }

                if (item != null)
                {
                    item.IsActivated = true;
                    item.UpdatePosition();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                     "Some of controls in saved form are not supported or corrupted and will not be loaded",
                     "Dyno Studio");
            }

            return item;
        }


        public void SaveOldBounds()
        {
            OldRect = Margin;
            OldSize = new Size(Width, Height);
        }
    }
}
