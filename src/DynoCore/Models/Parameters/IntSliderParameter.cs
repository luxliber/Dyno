using System.Globalization;

namespace Dyno.Models.Parameters
{
    public class IntSliderParameter : WorkspaceParameter
    {
        public int Max;
        public int Min;



        public IntSliderParameter(string name, int min, int max, int value) : base(name)
        {
            Max = max;
            Min = min;
            Value = value.ToString(CultureInfo.InvariantCulture);

            CreateValuesFromValue();
        }
    }
}
