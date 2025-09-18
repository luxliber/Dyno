namespace Dyno.Models.Parameters
{
    internal class WarningParameter : WorkspaceParameter
    {
        public WarningParameter(string name, string value)
            : base(name)
        {
            Value = value;
        }
    }
}
