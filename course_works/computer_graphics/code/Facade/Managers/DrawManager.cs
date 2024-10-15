using code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{ 
    class DrawManager : Manager
    {
        public DrawManager() { }

        public void _execute(DrawsCommand command)
        {
            command._execute();
        }
    }
}
