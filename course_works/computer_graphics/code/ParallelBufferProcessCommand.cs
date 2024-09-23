
namespace code
{
    internal class ParallelBufferProcessCommand : FormCommand
    {
        private Canvas canvas;
        private PictureBox picture;

        public ParallelBufferProcessCommand(ref Canvas canvas, ref PictureBox picture)
        {
            this.canvas = canvas;
            this.picture = picture;
        }
    }
}