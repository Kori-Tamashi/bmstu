﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}