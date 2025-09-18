using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using Dyno.FormControls;
using Dyno.Models.Forms;
using Dyno.ViewModels;

namespace Dyno.Views.FormControls
{
    public interface IFormControl
    {
        void Update();
        Dictionary<string, object> Values { get; set; }
        Dictionary<string, CalcEngine.Expression> Expressions { get; set; }
        bool IsError { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class BindingAttribute : Attribute
    {
        public string Value { set; get; }
    }

    public class ExpressionAttribute : Attribute
    {
        public string Value { set; get; }
    }

    public static class FormControlHelper
    {
        public struct PropCategory
        {
            public const string Binding = "Binding";
            public const string Content = "Content";
            public const string VisualStyle = "Visual Style";
            public const string Size = "Size";
            public const string Position = "Position";

        }

        public static bool FromStringToBool(object value, bool forceTrueOnEmpty = false)
        {
            var val = value?.ToString();

            if (String.IsNullOrEmpty(val))
                return forceTrueOnEmpty;

            if (val.ToLower() == "false")
                return false;

            if (val.ToLower() == "true")
                return true;

            var isNumeric = double.TryParse(val, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out double n);

            if (isNumeric)
                if (n > 0)
                    return true;
                else
                    return false;

            return true;
        }

        public static void UpdateControlValuesFromExpressions(FrameworkElement control)
        {
            var preset = DynoManagerBase.SelectedWorkspacePreset;
            if (preset == null)
                return;

            if (!(control is IFormControl fControl))
                return;

            var values = control.GetType().GetProperty("Values").GetValue(control) as Dictionary<string, object>;

            var expressionsProp = control.GetType().GetProperty("Expressions");
            Dictionary<string, CalcEngine.Expression> expressions = null;

            if (expressionsProp != null)
                expressions = expressionsProp.GetValue(control) as Dictionary<string, CalcEngine.Expression>;

            foreach (var prop in GetExpressionProperties(control))
            {
                var text = prop.GetValue(control) as string;

                if (!String.IsNullOrEmpty(text))
                    try
                    {
                        if (expressions != null && expressions.ContainsKey(prop.Name))
                            if (expressions[prop.Name] != null)
                            {
                                var value = expressions[prop.Name].Evaluate();
                                values[prop.Name] = value.ToString();
                            }
                            else
                                values[prop.Name] = "Error";
                        else
                        {
                            var x = DynoManagerBase.CEngine.Parse(text);
                            var value = x.Evaluate();
                            values[prop.Name] = value;
                        }
                    }
                    catch (Exception e)
                    {
                        values[prop.Name] = $"Error: {e.Message}";
                    }
                else
                    values[prop.Name] = "";
            }

            fControl.Update();
        }

        public static void UpdateControlValuesFromBindings(FrameworkElement control)
        {
            var preset = DynoManagerBase.SelectedWorkspacePreset;
            if (preset == null)
                return;

            if (!(control is IFormControl fControl))
                return;

            var values = control.GetType().GetProperty("Values").GetValue(control) as Dictionary<string, object>;
            
            foreach (var prop in GetBindingProperties(control))
            {
                var text = prop.GetValue(control) as string;

                if (!String.IsNullOrEmpty(text))
                    try
                    {
                        var t = text.Trim();
                        if (t.First() == '"' && t.Last() == '"')
                            values[prop.Name] = t.Replace(@"""", "");
                        else
                        {
                            var par = preset.GetParameterByName(t);
                            var testPar = preset.Workspace.WorkspaceForm.UserPars.FirstOrDefault(x => x.Name == t);

                            Regex validationRegEx = null;
                            bool res = true;

                            if (par != null)
                            {
                                values[prop.Name] = par.Value;
                            }
                            else if (testPar != null)
                                values[prop.Name] = testPar.Value;
                            else
                            {
                                values[prop.Name] = "Error: parameter not found";
                                fControl.IsError = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        values[prop.Name] = "Error";
                        fControl.IsError = true;
                    }
                else
                    values[prop.Name] = "";
            }

        }

        public static bool ValidateControlFromBindings(FrameworkElement control)
        {
            var preset = DynoManagerBase.SelectedWorkspacePreset;
            if (preset == null)
                return true;

            if (!(control is FormControl fControl))
                return true;

            foreach (var prop in GetBindingProperties(control))
            {
                var text = prop.GetValue(control) as string;
                if (!String.IsNullOrEmpty(text))
                    try
                    {
                        if (control is FormTextBox && prop.Name == "EditorTextBinding" && ((FormTextBox)control).ValidationRegEx != null)
                        {
                            text = text.Trim();

                            var par = preset.GetParameterByName(text);
                            var testPar = preset.Workspace.WorkspaceForm.UserPars.FirstOrDefault(x => x.Name == text);

                            var validationRegEx = ((FormTextBox)control).ValidationRegEx;
                            var res = true;

                            if (par != null)
                                res = validationRegEx.IsMatch(par.Value.ToString());
                            else if (testPar != null)
                                res = validationRegEx.IsMatch(testPar.Value);

                            if (!res)
                            {
                                fControl.IsError = true;
                                fControl.OnPropertyChanged("IsError");

                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        fControl.IsError = false;
                        fControl.OnPropertyChanged("IsError");

                    }
            }

            fControl.IsError = false;
            fControl.OnPropertyChanged("IsError");

            return true;
        }

        public static bool ValidateControlFromPortData(FrameworkElement control, Dictionary<string, PortParameter> portData)
        {
            if (!(control is FormControl fControl))
                return true;

            var values = control.GetType().GetProperty("Values").GetValue(control) as Dictionary<string, object>;

            foreach (var prop in GetBindingProperties(control))
            {
                var text = prop.GetValue(control) as string;
                if (!String.IsNullOrEmpty(text))
                    try
                    {
                        if (control is FormTextBox && prop.Name == "EditorTextBinding" && ((FormTextBox)control).ValidationRegEx != null)
                        {
                            text = text.Trim();

                            var validationRegEx = ((FormTextBox)control).ValidationRegEx;
                            var res = true;

                            if (portData.ContainsKey(text))

                                res = validationRegEx.IsMatch(portData[text].Value.ToString());
                            else
                            {
                                values[prop.Name] = "";
                                fControl.IsError = true;
                            }

                            if (!res)
                            {
                                fControl.IsError = true;
                                fControl.OnPropertyChanged("IsError");

                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        fControl.IsError = false;
                        fControl.OnPropertyChanged("IsError");

                    }
            }

            fControl.IsError = false;
            fControl.OnPropertyChanged("IsError");

            return true;
        }

        public static void FillControlValuesFromPortData(FrameworkElement control, Dictionary<string, PortParameter> portData)
        {
            var fControl = control as IFormControl;

            var values = control.GetType().GetProperty("Values").GetValue(control) as Dictionary<string, object>;

            foreach (var prop in GetBindingProperties(control))
            {
                var text = prop.GetValue(control) as string;



                if (!String.IsNullOrEmpty(text))
                    try
                    {
                        var t = text.Trim();
                        if (t.First() == '"' && t.Last() == '"')
                            values[prop.Name] = t.Replace(@"""", "");
                        else if (portData.ContainsKey(t))
                            values[prop.Name] = portData[t].Value;
                        else
                        {
                            values[prop.Name] = "";
                            fControl.IsError = true;
                        }
                    }
                    catch (Exception e)
                    {
                        values[prop.Name] = $"Error: {e.Message}";
                        fControl.IsError = true;
                    }
                else
                    values[prop.Name] = "";
            }

            
        }


        public static IEnumerable<PropertyInfo> GetBindingProperties(FrameworkElement control)
        {
            return control.GetType().GetProperties().Where(x =>
                x.Name.StartsWith("Editor") &&
                x.GetCustomAttributes(typeof(BindingAttribute)).Any());
        }

        private static IEnumerable<PropertyInfo> GetExpressionProperties(FrameworkElement control)
        {
            return control.GetType().GetProperties().Where(x =>
                x.Name.StartsWith("Editor") &&
                x.GetCustomAttributes(typeof(ExpressionAttribute)).Any());
        }

        public static void UpdateCommonVisualProperties(IFormControl control)
        {
            var fc = control as FormControl;
            if (fc.Form.IsProduction)
            {
                fc.IsEnabled = FromStringToBool(control.Values["EditorIsEnabedBinding"], true);
                fc.Visibility = FromStringToBool(control.Values["EditorIsVisibleBinding"], true) ? Visibility.Visible : Visibility.Hidden;
            }
            else
            {
                fc.IsEnabled = true;
                fc.Visibility = Visibility.Visible;
            }
        }

        public static void UpdateExpressionForControl(DependencyObject d, string name, string newValue)
        {
            if (!(d is IFormControl fControl)) return;

            if (fControl.Expressions.ContainsKey(name) && fControl.Expressions[name] != null &&
                fControl.Expressions[name])
                return;
            try
            {
                fControl.Expressions[name] = DynoManagerBase.CEngine.Parse(newValue);
            }
            catch
            {
                fControl.Expressions[name] = null;
            }
        }
    }
}
