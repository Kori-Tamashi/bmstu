using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class Material
    {
        protected MaterialType type;

        // Solid Shading Coeffs
        public float I_a; // интенсивность рассеянного источника
        public float k_a; // коэффииент диффузного отражения рассеянного источника
        public float k_d; // коэффициент диффузного отражения
        public float k_s; // аналог функции w_i
        public float K; // произвольная постоянная
        public int n; // степень аппрокисмации зеркально отраженного света
        public int d; // приоритет освещения

        // Phong Shading Coeffs
        public float a = 56; // коэффициент блеска
        public float k_m = 4; // коэффициент зеркального отражения

        public Material()
        {
            I_a = 1;
            k_a = 1;
            k_d = 1;
            k_s = 1;
            K = 1;
            n = 1;
            d = 1;
            type = MaterialType.None;
        }

        public MaterialType Type
        {
            get { return type; }
            set { type = value; }
        }

    }
}
