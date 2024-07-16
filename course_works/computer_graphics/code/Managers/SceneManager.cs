using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code.Managers
{
    class SceneManager : Manager
    {
        public SceneManager() { }

        public void _execute(SceneCommand command)
        {
            command._execute();
        }
    }
}
