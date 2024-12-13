using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    enum Modeltype
    {
        Model,
        Cube,
        DirectPrism,
        InclinedPrism,
        Pyramid,
        TruncatedPyramid,
        Icosahedron,
        Composite
    };

    abstract class ModelType
    {
        public static int ModelImageIndex(Modeltype modelType)
        {
            switch (modelType)
            {
                case Modeltype.Cube:
                    return 0;
                case Modeltype.DirectPrism:
                    return 1;
                case Modeltype.InclinedPrism:
                    return 2;
                case Modeltype.Pyramid:
                    return 3;
                case Modeltype.Model:
                    return 4;
                case Modeltype.TruncatedPyramid:
                    return 4;
                case Modeltype.Icosahedron:
                    return 5;
                default:
                    return 5;
            }
        }
    }
}
