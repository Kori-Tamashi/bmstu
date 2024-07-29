using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class SceneCommand : Command
    {
        protected Canvas canvas;

        public SceneCommand(ref Canvas canvas)
        {
            this.canvas = canvas;
        }
    }

    class AddModelCommand : SceneCommand
    {
        protected Model model;
        public AddModelCommand(ref Canvas canvas, ref Model model) : base(ref canvas)
        {
            this.model = model;
        }

        public override void _execute()
        {
            canvas.AddModel(model);
        }
    }

    class RemoveModelCommand : SceneCommand
    {
        protected Model model;

        public RemoveModelCommand(ref Canvas canvas, ref Model model) : base(ref canvas)
        {
            this.model = model;
        }

        public override void _execute()
        {
            canvas.RemoveModel(model);
        }
    }

    class DeleteModelsCommand : SceneCommand
    {
        public DeleteModelsCommand(ref Canvas canvas) : base(ref canvas) { }

        public override void _execute()
        {
            canvas.DeleteModels();
        }
    }
}
