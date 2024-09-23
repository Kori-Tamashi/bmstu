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
        protected CameraSystem cameraSystem;

        public CameraCommand(ref Canvas canvas, ref CameraSystem cameraSystem)
        {
            this.canvas = canvas;
            this.cameraSystem = cameraSystem;
        }
    }

    class CameraMoveCommand : CameraCommand
    {
        public CameraMoveCommand(ref Canvas canvas, ref CameraSystem cameraSystem) : base(ref canvas, ref cameraSystem) { }
    }

    class CameraRightMoveCommand : CameraMoveCommand
    {
        public CameraRightMoveCommand(ref Canvas canvas, ref CameraSystem cameraSystem) : base(ref canvas, ref cameraSystem) { }

        public override void _execute()
        {
            Move move = new Move(cameraSystem.MoveStep, 0, 0);
            cameraSystem.MoveCamera(move);
            move.Turn();
            canvas.Move(move);
        }
    }

    class CameraLeftMoveCommand : CameraMoveCommand
    {
        public CameraLeftMoveCommand(ref Canvas canvas, ref CameraSystem cameraSystem) : base(ref canvas, ref cameraSystem) { }

        public override void _execute()
        {
            Move move = new Move(-cameraSystem.MoveStep, 0, 0);
            cameraSystem.MoveCamera(move);
            move.Turn();
            canvas.Move(move);
        }
    }

    class CameraUpMoveCommand : CameraMoveCommand
    {
        public CameraUpMoveCommand(ref Canvas canvas, ref CameraSystem cameraSystem) : base(ref canvas, ref cameraSystem) { }

        public override void _execute()
        {
            Move move = new Move(0, -cameraSystem.MoveStep, 0);
            cameraSystem.MoveCamera(move);
            move.Turn();
            canvas.Move(move);
        }
    }

    class CameraDownMoveCommand : CameraMoveCommand
    {
        public CameraDownMoveCommand(ref Canvas canvas, ref CameraSystem cameraSystem) : base(ref canvas, ref cameraSystem) { }

        public override void _execute()
        {
            Move move = new Move(0, cameraSystem.MoveStep, 0);
            cameraSystem.MoveCamera(move);
            move.Turn();
            canvas.Move(move);

        }
    }

    class CameraDownRightMoveCommand : CameraMoveCommand
    {
        public CameraDownRightMoveCommand(ref Canvas canvas, ref CameraSystem cameraSystem) : base(ref canvas, ref cameraSystem) { }

        public override void _execute()
        {
            new CameraRightMoveCommand(ref canvas, ref cameraSystem)._execute();
            new CameraDownMoveCommand(ref canvas, ref cameraSystem)._execute();
        }
    }

    class CameraDownLeftMoveCommand : CameraMoveCommand
    {
        public CameraDownLeftMoveCommand(ref Canvas canvas, ref CameraSystem cameraSystem) : base(ref canvas, ref cameraSystem) { }

        public override void _execute()
        {
            new CameraLeftMoveCommand(ref canvas, ref cameraSystem)._execute();
            new CameraDownMoveCommand(ref canvas, ref cameraSystem)._execute();
        }
    }

    class CameraUpRightMoveCommand : CameraMoveCommand
    {
        public CameraUpRightMoveCommand(ref Canvas canvas, ref CameraSystem cameraSystem) : base(ref canvas, ref cameraSystem) { }

        public override void _execute()
        {
            new CameraRightMoveCommand(ref canvas, ref cameraSystem)._execute();
            new CameraUpMoveCommand(ref canvas, ref cameraSystem)._execute();
        }
    }

    class CameraUpLeftMoveCommand : CameraMoveCommand
    {
        public CameraUpLeftMoveCommand(ref Canvas canvas, ref CameraSystem cameraSystem) : base(ref canvas, ref cameraSystem) { }

        public override void _execute()
        {
            new CameraLeftMoveCommand(ref canvas, ref cameraSystem)._execute();
            new CameraUpMoveCommand(ref canvas, ref cameraSystem)._execute();
        }
    }
}
