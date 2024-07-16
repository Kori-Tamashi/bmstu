using code;
using code.Managers;
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
        SceneManager sceneManager;

        public Facade()
        { 
            drawManager = new DrawManager();
            sceneManager = new SceneManager();
        }

        public void _execute(DrawCommand command)
        {
            drawManager._execute(command);
        }

        public void _execute(SceneCommand command)
        {
            sceneManager._execute(command);
        }
    }
}
