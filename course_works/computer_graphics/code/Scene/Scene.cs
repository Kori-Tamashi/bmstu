using code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class Scene
    {
        List<Model> models;
        Size size;
        
        public Scene(Size size) 
        { 
            models = new List<Model>();
            this.size = size;
        }

        public void Draw(ref Graphics graphics, Pen pen)
        {
            foreach (Model model in models)
            {
                model.Draw(ref graphics, pen);
            }
        }

        public void AddModel(Model model)
        {
            models.Add(model);
        }

        public void RemoveModel(Model model)
        {
            models.Remove(model);
        }

        public void RemoveModel(int index)
        {
            models.RemoveAt(index);
        }

        public void Clear()
        {
            models.Clear();
        }
    }
}
