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

    class CanvasProcessCommand : DrawsCommand
    {
        protected PictureBox picture;

        public CanvasProcessCommand(ref Canvas canvas, ref PictureBox picture) : base(ref canvas)
        {
            this.picture = picture;
        }
    }

    class ZBufferProcessCommand : CanvasProcessCommand
    {
        public ZBufferProcessCommand(ref Canvas canvas, ref PictureBox picture) : base(ref canvas, ref picture) { }

        public async override void _execute()
        {
            LoadingBar bar = new LoadingBar();
            bar.Start();

            await Task.Run(() =>
            {
                ZBuffer zBuffer = new ZBuffer(canvas.Size, canvas.Models);
                Action updateImage = () => picture.Image = zBuffer.Image;
                picture.Invoke(updateImage);
                Thread.Sleep(10);
            });

            bar.Stop();
        }
    }

    class ParallelZBufferProcessCommand : CanvasProcessCommand
    {
        public ParallelZBufferProcessCommand(ref Canvas canvas, ref PictureBox picture) : base(ref canvas, ref picture) { }

        public async override void _execute()
        {
            LoadingBar bar = new LoadingBar();
            bar.Start();

            await Task.Run(() =>
            {
                ParallelZBuffer zBuffer = new ParallelZBuffer(canvas.Size, canvas.Models);
                Action updateImage = () => picture.Image = zBuffer.Image;
                picture.Invoke(updateImage);
                Thread.Sleep(10);
            });

            bar.Stop();
        }
    }
}
