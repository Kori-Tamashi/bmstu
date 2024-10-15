using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class CameraCommand : Command
    {
        protected Canvas canvas;

        public CameraCommand(ref Canvas canvas)
        {
            this.canvas = canvas;
        }
    }

    class CameraMoveCommand : CameraCommand
    {
        protected float d = 0;
        protected Move move;

        public CameraMoveCommand(ref Canvas canvas, ref Move move) : base(ref canvas) 
        {
            this.move = move;
        }

        public CameraMoveCommand(ref Canvas canvas, int dX, int dY, int dZ) : base(ref canvas)
        {
            this.move = new Move(dX, dY, dZ);
        }

        public CameraMoveCommand(ref Canvas canvas, float d) : base(ref canvas)
        {
            this.d = d;
        }

        public override void _execute()
        {
            canvas.MoveCamera(move);
        }
    }

    class CameraRightMoveCommand : CameraMoveCommand
    {
        public CameraRightMoveCommand(ref Canvas canvas, float d) : base(ref canvas, d) { }

        public override void _execute()
        {
            canvas.MoveCameraRight(d);
        }
    }

    class CameraLeftMoveCommand : CameraMoveCommand
    {
        public CameraLeftMoveCommand(ref Canvas canvas, float d) : base(ref canvas, d) { }

        public override void _execute()
        {
            canvas.MoveCameraLeft(d);
        }
    }

    class CameraUpMoveCommand : CameraMoveCommand
    {
        public CameraUpMoveCommand(ref Canvas canvas, float d) : base(ref canvas, d) { }

        public override void _execute()
        {
            canvas.MoveCameraUp(d);
        }
    }

    class CameraDownMoveCommand : CameraMoveCommand
    {
        public CameraDownMoveCommand(ref Canvas canvas, float d) : base(ref canvas, d) { }

        public override void _execute()
        {
            canvas.MoveCameraDown(d);
        }
    }

    class CameraDownRightMoveCommand : CameraMoveCommand
    {
        public CameraDownRightMoveCommand(ref Canvas canvas, float d) : base(ref canvas, d) { }

        public override void _execute()
        {
            canvas.MoveDownRight(d);
        }
    }

    class CameraDownLeftMoveCommand : CameraMoveCommand
    {
        public CameraDownLeftMoveCommand(ref Canvas canvas, float d) : base(ref canvas, d) { }

        public override void _execute()
        {
            canvas.MoveDownLeft(d);
        }
    }

    class CameraUpRightMoveCommand : CameraMoveCommand
    {
        public CameraUpRightMoveCommand(ref Canvas canvas, float d) : base(ref canvas, d) { }

        public override void _execute()
        {
            canvas.MoveUpRight(d);
        }
    }

    class CameraUpLeftMoveCommand : CameraMoveCommand
    {
        public CameraUpLeftMoveCommand(ref Canvas canvas, float d) : base(ref canvas, d) { }

        public override void _execute()
        {
            canvas.MoveUpLeft(d);
        }
    }
}
