using System;
using Dyno.Annotations;
using LitJson;

namespace Dyno.Models.Parameters
{
    public class StringParameter : WorkspaceParameter
    {
        public StringParameter(string name, [NotNull] string value)
            : base(name)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            CreateValuesFromValue();
        }

        public StringParameter(string name, JsonData data)
            : base(name, data) {}
    }
}
