using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Dyno.Annotations;
using Dyno.Models.Workspaces;
using LitJson;

namespace Dyno.Models.Parameters
{
    public class WorkspaceParameter : INotifyPropertyChanged
    {
        public WorkspaceParameter(string name)
        {
            Name = name;

        }

        protected WorkspaceParameter(string name, JsonData data) : this(name)
        {
            if (data == null)
                return;
            Values = new ObservableCollection<object>();

            if (data.IsArray)
            {
                foreach (JsonData value in data)
                    if (value.IsDouble)
                        Values.Add((double)value);
                    else if (value.IsInt)
                        Values.Add((int)value);
                    else if (value.IsString)
                        Values.Add(value.ToString());

                Value = (string)(Values.Count > 0 ? Values[0] : null);
            }
            else
            {
                if (data.Keys.Contains("value"))
                    if (data["value"].IsDouble)
                        Value = (double)data["value"];
                    else if (data["value"].IsInt)
                        Value = (int)data["value"];
                    else if (data["value"].IsString)
                        Value = data["value"].ToString();



                if (data.Keys.Contains("values") && data["values"].IsArray)
                {
                    foreach (JsonData value in data["values"])
                        if (value.IsDouble)
                            Values.Add(((double)value).ToString(CultureInfo.InvariantCulture));
                        else if (value.IsInt)
                            Values.Add(((int)value).ToString(CultureInfo.InvariantCulture));
                        else if (value.IsString)
                            Values.Add(value.ToString());
                }
                else
                    CreateValuesFromValue();

                Desc = (data.Keys.Contains("desc") ? (string)data["desc"] : "");
                IsHidden = data.Keys.Contains("hidden") && (bool)data["hidden"];
            }
        }

        public bool IsHidden { get; set; }

        public string Desc { get; set; }

        public string Tag => IsHidden ? "hidden" : "parameter";

        public WorkspacePreset Workspace { get; set; }

        public string Name { get; set; }
        public string Guid { get; set; }

        public ObservableCollection<object> Values { set; get; }

        public object Value { get; set; }


        public string Format
        {
            get
            {
                if (GetType() == typeof(OutputParameter))
                    return "output";
                if (GetType() == typeof(SelectElementParameter))
                    return "select";
                if (GetType() == typeof(WarningParameter))
                    return "warning";
                return "input";
            }
        }

        public int AsInt()
        {
            var res = 0;
            try
            {
                res = Convert.ToInt32(Value);
            }
            catch (Exception)
            {
                // ignored
            }

            return res;
        }

        public double AsDouble()
        {
            double res = 0;

            try
            {
                res = Convert.ToDouble(Value, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                // ignored
            }

            return res;
        }

        public bool AsBoolean()
        {
            var res = false;

            try
            {
                res = Convert.ToBoolean(Value, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                // ignored
            }

            return res;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void writeJson(JsonWriter writer)
        {
            writer.Write("none");
        }

        protected void CreateValuesFromValue()
        {
            if (Values == null)
                Values = new ObservableCollection<object>();

            if (Value != null)
                Values.Add(Value);
        }
    }
}
