using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code.Commands
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

    class CameraMove : CameraCommand
    {
        Direction direction;

        public CameraMove(ref Canvas canvas, ref CameraSystem cameraSystem, Direction direction) : base(ref canvas, ref cameraSystem) { }

        public override void _execute()
        {
            cameraSystem.MoveCamera(direction);


        }
    }
}
