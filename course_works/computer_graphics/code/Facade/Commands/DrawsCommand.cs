using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    class RenderCommand : DrawsCommand
    {
        RenderMode renderMode;
        PictureBox pictureBox;
        ToolStripStatusLabel timeLabel;
        ToolStripStatusLabel statusLabel;

        public RenderCommand(ref Canvas canvas, ref PictureBox pictureBox, RenderMode renderMode) : base(ref canvas) 
        {
            this.renderMode = renderMode;
            this.pictureBox = pictureBox;
        }

        public RenderCommand(ref Canvas canvas, ref PictureBox pictureBox, RenderMode renderMode, 
            ref ToolStripStatusLabel timeLabel, ref ToolStripStatusLabel statusLabel) : base(ref canvas)
        {
            this.renderMode = renderMode;
            this.pictureBox = pictureBox;
            this.timeLabel = timeLabel;
            this.statusLabel = statusLabel;
        }

        public override void _execute()
        {
            if (statusLabel != null)
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.ToolTipText = "В процессе";
            }
                
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (renderMode != RenderMode.CarcassDisplay)
            {
                canvas.Render(renderMode);
                canvas.UpdateImage(ref pictureBox);
                pictureBox.Refresh();
            }
            else
            {
                canvas.GraphicsClear();
                canvas.Render(renderMode);
            }

            stopwatch.Stop();
            canvas.LastRender = stopwatch.Elapsed.TotalSeconds;

            if (timeLabel != null)
                timeLabel.Text = canvas.LastRender.ToString("F2") + " секунд";

            if (statusLabel != null)
            {
                statusLabel.ForeColor = Color.Green;
                statusLabel.ToolTipText = "Завершен";
            }
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
