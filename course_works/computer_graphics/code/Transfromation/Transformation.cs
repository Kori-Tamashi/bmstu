using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class Transformation
    {
        public TransformationMatrix matrix;

        public Transformation()
        {
            matrix = new TransformationMatrix();
        }

        public Transformation(TransformationMatrix matrix)
        {
            this.matrix = matrix;
        }


    }
}
