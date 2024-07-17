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

    class AddModelCmd : SceneCommand
    {
        protected Model model;
        public AddModelCmd(ref Canvas canvas, ref Model model) : base(ref canvas)
        {
            this.model = model;
        }

        public override void _execute()
        {
            canvas.AddModel(model);
        }
    }

    class RemoveModelCmd : SceneCommand
    {
        protected Model model;

        public RemoveModelCmd(ref Canvas canvas, ref Model model) : base(ref canvas)
        {
            this.model = model;
        }

        public override void _execute()
        {
            canvas.RemoveModel(model);
        }
    }
}
