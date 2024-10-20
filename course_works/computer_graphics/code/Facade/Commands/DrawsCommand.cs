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

    class ImageUpdateCommand : DrawsCommand
    {
        protected PictureBox pictureBox;

        public ImageUpdateCommand(ref Canvas canvas, ref PictureBox pb) : base(ref canvas) 
        {
            pictureBox = pb;
        }

        public override void _execute()
        {
            canvas.Render();
            canvas.UpdateImage(ref pictureBox);
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
            });

            bar.Stop();
        }
    }

    class SolidShadingProcessCommand : CanvasProcessCommand
    {
        public SolidShadingProcessCommand(ref Canvas canvas, ref PictureBox picture) : base(ref canvas, ref picture) { }

        public async override void _execute()
        {
            LoadingBar bar = new LoadingBar();
            bar.Start();

            List<Light> lights = new List<Light> { new Light() };
            Vector3D supervisor = new Vector3D(0, 0, -1);

            await Task.Run(() =>
            {
                SolidShading solidShading = new SolidShading(canvas.Size, canvas.Models, lights, supervisor);
                Action updateImage = () => picture.Image = solidShading.Image;
                picture.Invoke(updateImage);
            });

            bar.Stop();
        }
    }

    class ParallelSolidShadingProcessCommand : CanvasProcessCommand
    {
        public ParallelSolidShadingProcessCommand(ref Canvas canvas, ref PictureBox picture) : base(ref canvas, ref picture) { }

        public async override void _execute()
        {
            LoadingBar bar = new LoadingBar();
            bar.Start();

            List<Light> lights = new List<Light> { new Light() };
            Vector3D supervisor = new Vector3D(0, 0, -1);

            await Task.Run(() =>
            {
                ParallelSolidShading solidShading = new ParallelSolidShading(canvas.Size, canvas.Models, lights, supervisor);
                Action updateImage = () => picture.Image = solidShading.Image;
                picture.Invoke(updateImage);
            });

            bar.Stop();
        }
    }
}
