using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace code
{
    partial class Form2 : Form
    {

        Canvas mainCanvas;
        Canvas selfCanvas;

        public Form2()
        {
            InitializeComponent();
            UpdateListModels(); 
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void UpdateListModels()
        {
            List<Model> models = new List<Model>
            {
                new Cube(),
                new Pyramid(),
                new DirectPrism()
            };

            // Clear list
            listView_models.Clear();

            // Initialize group
            ListViewGroup listViewGroup = new ListViewGroup();
            listViewGroup.Header = "Модели";

            // Initialize items
            foreach (Model model in models)
            {
                ListViewItem item = new ListViewItem(model.name, listViewGroup);
                item.ImageIndex = ModelImageIndex(model.type);
                listView_models.Items.Add(item);
            }

            // Add items
            listView_models.Groups.Add(listViewGroup);
            listView_models.Refresh();
        }

        private int ModelImageIndex(String modelType)
        {
            switch (modelType)
            {
                case "Cube":
                    return 0;
                case "Direct prism":
                    return 1;
                case "Inclined prism":
                    return 2;
                case "Pyramid":
                    return 3;
                default:
                    return 5;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void numericUpDown_angle_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown_width_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
