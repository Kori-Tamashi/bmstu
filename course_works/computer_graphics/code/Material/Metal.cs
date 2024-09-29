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
            k_a = 0.15f;
            k_d = 0.15f;
            k_s = 0.8f;
            K = 1;
            n = 5;
            d = 0;
            type = MaterialType.Metal;
        }
    }
}
