using code.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class Facade
    {
        DrawManager drawManager;

        public Facade()
        { 
            drawManager = new DrawManager();
        }

        public void _execute(DrawCommand command)
        {
            drawManager._execute(command);
        }
    }
}
