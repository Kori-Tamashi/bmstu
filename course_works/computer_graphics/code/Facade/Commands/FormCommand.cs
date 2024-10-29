using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code
{
    class FormCommand : Command
    {
    }

    class PictureRefreshCommand : FormCommand
    {
        PictureBox pictureBox;

        public PictureRefreshCommand(ref PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
        }

        public override void _execute()
        {
            pictureBox.Refresh();
        }
    }

    class ListViewAddModelCommand : FormCommand
    {
        protected ListView listView;
        protected Model model;

        public ListViewAddModelCommand(ref ListView listView, ref Model model)
        {
            this.listView = listView;
            this.model = model;
        }

        public override void _execute()
        {
            ListViewGroup listViewGroup;

            if (listView.Groups.Count == 0)
            {
                listViewGroup = new ListViewGroup();
                listViewGroup.Header = "Модели";
                listView.Groups.Add(listViewGroup);
            }
            else
            {
                listViewGroup = listView.Groups[0];
            }
                
            ListViewItem item = new ListViewItem(model.Name, listViewGroup);
            item.ImageIndex = ModelType.ModelImageIndex(model.Type);

            listView.Items.Add(item);
            listView.Refresh();
        }
    }

    class ListViewClearCommand : FormCommand
    {
        protected ListView listView;

        public ListViewClearCommand(ref ListView listView) 
        {
            this.listView = listView;
        }

        public override void _execute()
        {
            listView.Clear();
            listView.Refresh();
        }
    }

    class DialogEditShowCommand : FormCommand 
    {
        protected PictureBox pictureBox;
        protected RenderMode renderMode;
        protected Canvas mainCanvas;

        public DialogEditShowCommand(ref Canvas mainCanvas, ref PictureBox pictureBox, RenderMode renderMode) 
        { 
            this.mainCanvas = mainCanvas;
            this.pictureBox = pictureBox;
            this.renderMode = renderMode;
        }

        public override void _execute()
        {
            DialogEdit dialogEdit = new DialogEdit(ref mainCanvas, ref pictureBox, renderMode);
            dialogEdit.ShowDialog();
        }
    }

    class ErrorMessageShowCommand : FormCommand
    {
        protected string text;

        public ErrorMessageShowCommand(string text)
        {
            this.text = text;
        }

        public override void _execute()
        {
            ErrorMessage errorMessage = new ErrorMessage();
            errorMessage.Show(text);
        }
    }
}
