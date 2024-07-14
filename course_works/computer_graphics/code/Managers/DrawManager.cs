using code.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{ 
    class DrawManager : Manager
    {
        Canvas canvas; 
        public DrawManager() { }

        public void _execute(DrawCommand command)
        {
            command._execute();
        }
    }
}
