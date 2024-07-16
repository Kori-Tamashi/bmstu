using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class DrawCommand : Command 
    {
        protected Canvas canvas;

        public DrawCommand(ref Canvas canvas) 
        { 
            this.canvas = canvas;
        }
    }

    class DrawCmd : DrawCommand
    {
        public DrawCmd(ref Canvas canvas) : base(ref canvas) { }

        public override void _execute()
        {
            canvas.Draw();
        }
    }

    class ClearCmd : DrawCommand
    {
        public ClearCmd(ref Canvas canvas) : base(ref canvas) { }

        public override void _execute() 
        {
            canvas.Clear();
        }
    }

    class RefreshCmd : DrawCommand
    {
        public RefreshCmd(ref Canvas canvas) : base(ref canvas) { }

        public override void _execute()
        {
            canvas.Refresh();
        }
    }
}
