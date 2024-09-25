using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class Metal : Material
    {
        public Metal()
        {
            I_a = 1;
            k_a = (float)0.15;
            k_d = (float)0.15;
            k_s = (float)0.8;
            K = 1;
            n = 5;
            type = MaterialType.Metal;
        }
    }
}
