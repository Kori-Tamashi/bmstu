using code.Commands;
using code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace code
{
   

    class Canvas
    {
        Pen pen;
        Size size;
        public Scene scene;
        Bitmap bitmap;
        Graphics graphics;
        
        public Canvas(Size size, Graphics g)
        {
            this.size = size;
            pen = new Pen(Color.Black);
            scene = new Scene(size);
            bitmap = new Bitmap(size.Width, size.Height);
            graphics = g;
        }

        public void AddModel(Model model)
        {
            scene.AddModel(model);
        }

        public void RemoveModel(Model model)
        {
            scene.RemoveModel(model);
        }

        public void RemoveModel(int index)
        {
            scene.RemoveModel(index);
        }

        public void DeleteModels()
        {
            scene.Clear();
        }

        public void Draw()
        {
            scene.Draw(graphics, pen);
        }

        public void Clear()
        {
            graphics.Clear(Color.White);
        }

        public void Refresh()
        {
            Clear();
            Draw();
        }

    }
}
