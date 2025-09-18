using LitJson;

namespace Dyno.Models.Parameters
{
    public class NumberParameter : WorkspaceParameter
    {
        public NumberParameter(string name, double value)
            : base(name)
        {
            Value = value;

            CreateValuesFromValue();
        }

        public NumberParameter(string name, JsonData data)
            : base(name, data)
        {

        }
    }
}
