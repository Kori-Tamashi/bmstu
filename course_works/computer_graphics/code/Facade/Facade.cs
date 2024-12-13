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
        Manager manager;
        FormManager formManager;
        DrawManager drawManager;
        SceneManager sceneManager;
        TransformationManager transformationManager;

        public Facade()
        { 
            manager = new Manager();
            formManager = new FormManager();
            drawManager = new DrawManager();
            sceneManager = new SceneManager();
            transformationManager = new TransformationManager();
        }

        public void _execute(FormCommand command)
        {
            formManager._execute(command);
        }

        public void _execute(DrawsCommand command)
        {
            drawManager._execute(command);
        }

        public void _execute(SceneCommand command)
        {
            sceneManager._execute(command);
        }

        public void _execute(TransformationCommand command) 
        { 
            transformationManager._execute(command);
        }

        public void _execute(params Command[] commands)
        {
            foreach (var command in commands)
            {
                manager._execute(command);
            }
        }
    }
}
