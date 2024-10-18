using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class Stone : Material
    {
        public Stone()
        {
            I_a = 0.5f;
            k_a = 0.2f;
            k_d = 0.3f;
            k_s = 0.2f;
            K = 1;
            n = 3;
            d = 1;
            type = MaterialType.Stone;
        }
    }
}
