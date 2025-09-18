using System;
using Dyno.Annotations;

namespace Dyno.Models.Parameters
{
    public class DropDownParameter : WorkspaceParameter
    {
        public int Index;

        public DropDownParameter(string name, [NotNull] string value)
            : base(name)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            Value = "None";

            var splits = value.Split(':');
            if (splits.Length > 1)
                Value = value.Substring(value.IndexOf(':') + 1);

            CreateValuesFromValue();
        }

        public DropDownParameter(string name, int index)
            : base(name)
        {
            Index = index;
            CreateValuesFromValue();
            Value = "Click To Update";
        }

    }
}
