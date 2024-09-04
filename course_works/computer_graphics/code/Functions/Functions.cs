using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class Functions<T>
    {
        const float eps = (float)1e-12;

        static public void Swap(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }
    }

    class Functions
    {
        const float eps = (float)1e-5;

        static public bool Equal(float a, float b)
        {
            return Math.Abs(a - b) < eps;
        }

        static public bool LowerEqual(float a, float b)
        {
            return a < b || Equal(a, b);
        }

        static public bool GreaterEqual(float a, float b)
        {
            return a > b || Equal(a, b);
        }
    }
}
