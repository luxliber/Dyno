using LitJson;

namespace Dyno.Models.Parameters
{
    public class BooleanParameter : WorkspaceParameter
    {        
        public string FalseText;
        public string TrueText;
        public bool FastMode=true;

        public BooleanParameter(string name, bool boolValue)
            : base(name)
        {
            Value = boolValue.ToString();
            FalseText = "False";
            TrueText = "True";

            CreateValuesFromValue();

        }

        public BooleanParameter(string name, JsonData data) : base(name,data)
        {
           
            FalseText = (string) (data.Keys.Contains("falseText") ? data["falseText"] : "False");
            TrueText = (string)(data.Keys.Contains("TrueText") ? data["trueText"] : "True");

            FastMode = (bool)(data.Keys.Contains("fastMode") ? data["fastMode"] : true);
        }
    }
}
