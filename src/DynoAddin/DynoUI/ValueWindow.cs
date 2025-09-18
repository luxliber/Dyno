using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DynoUI
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DynoUI"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:DynoUI;assembly=DynoUI"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ValueWindow/>
    ///
    /// </summary>
    public class ValueWindow : ParameterWindow
    {
        static ValueWindow()
        {
            //      DefaultStyleKeyProperty.OverrideMetadata(typeof(ValueWindow), new FrameworkPropertyMetadata(typeof(ValueWindow)));
        }

        public string ValueText { get; set; }
        public ObservableCollection<object> Values { get; set; }

        protected override void OnActivated(EventArgs e)
        {
            OnPropertyChanged(nameof(ValueText));

            var cb = FindName("ValueComboBox") as ComboBox;

            if (!(cb.Template.FindName("PART_EditableTextBox", cb) is TextBox textBox)) return;
            base.OnActivated(e);

            textBox.SelectAll();
            cb.Focus();

            textBox.TextChanged += (sender, args) => UnsetError();
        }
    }
}
