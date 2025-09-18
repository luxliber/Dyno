using System.Collections.Generic;

namespace CalcEngine
{
    static class Logical
    {
        public static void Register(CalcEngine ce)
        {
            ce.RegisterFunction("AND", 1, int.MaxValue, And, "Logical");
            ce.RegisterFunction("OR", 1, int.MaxValue, Or, "Logical");
            ce.RegisterFunction("NOT", 1, Not, "Logical");
            ce.RegisterFunction("IF", 3, If, "Logical");
            ce.RegisterFunction("TRUE", 0, True, "Logical");
            ce.RegisterFunction("FALSE", 0, False, "Logical");
        }

        
        
        static object And(List<Expression> p)
        {
            var b = true;
            foreach (var v in p)
            {
                b = b && (bool)v;
            }
            return b;
        }
        static object Or(List<Expression> p)
        {
            var b = false;
            foreach (var v in p)
            {
                b = b || (bool)v;
            }
            return b;
        }
        static object Not(List<Expression> p)
        {
            var v = p[0].Evaluate().ToString();

            return !bool.Parse(v);
       
        }
        static object If(List<Expression> p)
        {
            return (bool)p[0] 
                ? p[1].Evaluate() 
                : p[2].Evaluate();
        }
        static object True(List<Expression> p)
        {
            return true;
        }
        static object False(List<Expression> p)
        {
            return false;
        }
    }
}
