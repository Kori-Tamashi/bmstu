using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

using IntBuffer = code.Matrix<int>;
using ColorBuffer = System.Drawing.Color[][];

namespace code
{
    class ZBufferShadows : SolidShading
    {
        IntBuffer zBufferShadows;
        ColorBuffer colorBufferShadows;

    }
}
