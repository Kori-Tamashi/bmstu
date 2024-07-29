using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class DrawsCommand : Command 
    {
        protected Canvas canvas;

        public DrawsCommand(ref Canvas canvas) 
        { 
            this.canvas = canvas;
        }
    }

    class DrawCommand : DrawsCommand
    {
        public DrawCommand(ref Canvas canvas) : base(ref canvas) { }

        public override void _execute()
        {
            canvas.Draw();
        }
    }

    class ClearCommand: DrawsCommand
    {
        public ClearCommand(ref Canvas canvas) : base(ref canvas) { }

        public override void _execute() 
        {
            canvas.Clear();
        }
    }

    class RefreshCommand : DrawsCommand
    {
        public RefreshCommand(ref Canvas canvas) : base(ref canvas) { }

        public override void _execute()
        {
            canvas.Refresh();
        }
    }
}
