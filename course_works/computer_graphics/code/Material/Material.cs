using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class Material
    {
        MaterialType type;

        public Material()
        {
            type = MaterialType.None;
        }

        public MaterialType Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
