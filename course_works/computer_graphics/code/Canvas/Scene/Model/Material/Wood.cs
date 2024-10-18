using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class Wood : Material
    {
        public Wood()
        {
            I_a = 0.4f;
            k_a = 0.1f;
            k_d = 0.1f;
            k_s = 0.1f;
            K = 1;
            n = 1;
            d = 2;
            type = MaterialType.Wood;
        }
    }
}
