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
        public float I_a = 0.5f; // интенсивность рассеянного источника
        public float k_a = 0.2f; // коэффииент диффузного отражения рассеянного источника
        public float k_d = 0.15f; // коэффициент диффузного отражения
        public float k_s = 0.2f; // аналог функции w_i
        public float K = 1;      // произвольная постоянная
        public int n = 3;        // степень аппрокисмации зеркально отраженного света
        public int d = 1;        // приоритет освещения

        // Phong Shading Coeffs
        public float a = 20;  // коэффициент блеска
        public float k_m = 0.3f; // коэффициент зеркального отражения

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
