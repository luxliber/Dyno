using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    /// <summary>
    /// Function definition class (keeps function name, parameter counts, and delegate).
    /// </summary>
    public class FunctionDefinition
    {
        // ** fields
        public int ParmMin, ParmMax;
        public CalcEngineFunction Function;
        public string Category;

        // ** ctor
        public FunctionDefinition(int parmMin, int parmMax, CalcEngineFunction function, string cat)
        {
            ParmMin = parmMin;
            ParmMax = parmMax;
            Function = function;
            Category = cat;
        }
    }
}
