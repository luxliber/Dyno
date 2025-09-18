using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DynoUI.Annotations;

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
    ///     <MyNamespace:ParameterWindow/>
    ///
    /// </summary>
    public class ParameterWindow : Window , INotifyPropertyChanged
    {
        public string Desc {
            get
            {
                if (IsError)
                    return errorMessage;
                
                return desc;
            }
            set { desc = value; }
        }

        public bool IsError { get; set; }

        public string errorMessage;
        private string desc;

        static ParameterWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ParameterWindow), new FrameworkPropertyMetadata(typeof(ParameterWindow)));
        }

        public ParameterWindow()
        {
            Width = 260;
            MinWidth = 180;
            SizeToContent = SizeToContent.Height;
           

        }

        protected void SetError(string s)
        {
            IsError = true;
            errorMessage = "Error: " + s;
        }

        protected void UnsetError()
        {
            IsError = false;
            OnPropertyChanged("IsError");
            OnPropertyChanged("Desc");
        }

        public override void OnApplyTemplate()
        {
            var okButton = Template.FindName("Ok", this) as Button;
            if(okButton!=null)
                okButton.Click += delegate
                {
                    CheckData();
                    if(!IsError)
                    {
                        
                        DialogResult = true; 
                        Close();
                    }
                    else
                    {
                        OnPropertyChanged("IsError");
                        OnPropertyChanged("Desc");
                    }
                };

            
        }

        public virtual void CheckData()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected internal virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        public static T FindChild<T>(DependencyObject parent, string childName)
   where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }


}
