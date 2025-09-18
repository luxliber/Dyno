using System.Globalization;

namespace Dyno.Models.Parameters
{
    public class NumberSliderParameter : WorkspaceParameter
    {
        internal float Max;
        internal float Min;

        public NumberSliderParameter(string name, float min, float max, double value)
            : base(name)
        {
            Max = max;
            Min = min;
            Value = value.ToString(CultureInfo.InvariantCulture);

            CreateValuesFromValue();
        }
    }
}
