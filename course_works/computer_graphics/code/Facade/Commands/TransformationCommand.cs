using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace code
{
    class TransformationCommand : Command
    {
        protected Canvas canvas;

        public TransformationCommand(ref Canvas canvas)
        {
            this.canvas = canvas;
        }
    }

    class MoveCommand : TransformationCommand
    {
        Move move;

        public MoveCommand(ref Canvas canvas, float dx, float dy, float dz) : base(ref canvas)
        {
            move = new Move(dx, dy, dz);
        }

        public override void _execute()
        {
            canvas.Move(move);
        }
    }

    class MoveModelCommand : TransformationCommand
    {
        Move move;
        int index;

        public MoveModelCommand(ref Canvas canvas, float dx, float dy, float dz, int index) : base(ref canvas)
        {
            move = new Move(dx, dy, dz);
            this.index = index;
        }

        public MoveModelCommand(ref Canvas canvas, ref Move move, int index) : base(ref canvas)
        {
            this.move = move;
            this.index = index;
        }

        public override void _execute()
        {
            canvas.Move(move, index);
        }
    }

    class RotateCommand : TransformationCommand
    {
        Rotate rotate;

        public RotateCommand(ref Canvas canvas, float angleX, float angleY, float angleZ) : base(ref canvas)
        {
            rotate = new Rotate(angleX, angleY, angleZ);
        }

        public override void _execute()
        {
            canvas.Rotate(rotate);
        }
    }

    class RotateModelCommand : TransformationCommand
    {
        Rotate rotate;
        int index;

        public RotateModelCommand(ref Canvas canvas, float angleX, float angleY, float angleZ, int index) : base(ref canvas)
        {
            rotate = new Rotate(angleX, angleY, angleZ);
            this.index = index;
        }

        public RotateModelCommand(ref Canvas canvas, ref Rotate rotate, int index) : base(ref canvas)
        {
            this.rotate = rotate;
            this.index = index;
        }

        public override void _execute()
        {
            canvas.Rotate(rotate, index);
        }
    }

    class ScaleCommand : TransformationCommand
    {
        Scale scale;

        public ScaleCommand(ref Canvas canvas, float kx, float ky, float kz) : base(ref canvas)
        {
            scale = new Scale(kx, ky, kz);
        }

        public override void _execute()
        {
            canvas.Scale(scale);
        }
    }

    class ScaleModelCommand : TransformationCommand
    {
        Scale scale;
        int index;

        public ScaleModelCommand(ref Canvas canvas, float kx, float ky, float kz, int index) : base(ref canvas)
        {
            scale = new Scale(kx, ky, kz);
            this.index = index;
        }

        public ScaleModelCommand(ref Canvas canvas, ref Scale scale, int index) : base(ref canvas)
        {
            this.scale = scale;
            this.index = index;
        }

        public override void _execute()
        {
            canvas.Scale(scale, index);
        }
    }

    class CenteringCommand : TransformationCommand 
    {
        Centering centering;

        public CenteringCommand(ref Canvas canvas, Model model, Point3D dstCenter, Size dstSize) : base(ref canvas)
        {
            centering = new Centering(model, dstCenter, dstSize);
        }

        public override void _execute()
        {
            canvas.Centering(centering);
        }
    }
}
