using code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * Модель освещения Фонга
 * Загрузка для теней
 * Изменение количества вершин в основании
 * Изменение радиусов оснований
 * Устанока источника света на сцену
 * Разобраться с блеском в свете
 */

namespace code
{
    public partial class Form1 : Form
    {
        Canvas canvas;
        Facade facade;

        public Form1()
        {
            InitializeComponent();
            InitializeCanvas();
            InitializeFacade();
            InitializeYawPitch();
            InitializeLight();
        }

        #region Initialize

        private void InitializeCanvas()
        {
            canvas = new Canvas(new Size(picture.Width, picture.Height), picture.CreateGraphics());
        }

        private void InitializeFacade()
        {
            facade = new Facade();
        }

        private void InitializeYawPitch()
        {
            label_yawMin.Text = trackBar_yaw.Minimum.ToString();
            label_yawMax.Text = trackBar_yaw.Maximum.ToString();
            label_pitchMin.Text = trackBar_pitch.Minimum.ToString();
            label_pitchMax.Text = trackBar_pitch.Maximum.ToString();

            trackBar_yaw.Value = (int)canvas.Yaw;
            trackBar_pitch.Value = (int)canvas.Pitch;
        }

        private void InitializeLight()
        {
            numericUpDown_lightIntensity.Value = (decimal)canvas.LightIntensity;

            numericUpDown_lightPositionX.Value = (decimal)canvas.LightPosition.X;
            numericUpDown_lightPositionY.Value = (decimal)canvas.LightPosition.Y;
            numericUpDown_lightPositionZ.Value = (decimal)canvas.LightPosition.Z;

            numericUpDown_lightDirectionX.Value = (decimal)canvas.LightDirection.X;
            numericUpDown_lightDirectionY.Value = (decimal)canvas.LightDirection.Y;
            numericUpDown_lightDirectionZ.Value = (decimal)canvas.LightDirection.Z;
        }

        #endregion

        #region ModelsButtons

        private void model_button_Click(object sender, EventArgs e)
        {
            Model model = new Model();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void Cube_button_Click(object sender, EventArgs e)
        {
            Model model = new Cube();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void directPrism_button_Click(object sender, EventArgs e)
        {
            Model model = new DirectPrism();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void triangularPyramid_button_Click(object sender, EventArgs e)
        {
            Model model = new Pyramid();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void Icosahedron_button_Click(object sender, EventArgs e)
        {
            Model model = new Icosahedron();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void inclinedPrism_button_Click(object sender, EventArgs e)
        {
            Model model = new InclinedPrism();

            facade._execute(
                new ListViewAddModelCommand(ref listView_modelsMain, ref model),
                new AddModelCommand(ref canvas, ref model),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        #endregion

        #region ActionsButtons

        private void button_dialogEdit_Click(object sender, EventArgs e)
        {
            facade._execute(new DialogEditShowCommand(ref canvas, ref picture, GetCurrentRenderMode()));
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            facade._execute(
                new ClearCommand(ref canvas),
                new ListViewClearCommand(ref listView_modelsMain)
            );
        }

        #endregion

        #region TransformationsButtons

        private void button_moveModel_Click(object sender, EventArgs e)
        {
            int currentIndex = GetCurrentModelIndex();

            if (currentIndex == -1)
                return;

            Move move = new Move(
                (float)numericUpDown_moveX.Value,
                (float)numericUpDown_moveY.Value,
                (float)numericUpDown_moveZ.Value
            );

            facade._execute(
                new MoveModelCommand(ref canvas, ref move, currentIndex),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_rotateModel_Click(object sender, EventArgs e)
        {
            int currentIndex = GetCurrentModelIndex();

            if (currentIndex == -1)
                return;

            Rotate rotate = new Rotate(
                (float)numericUpDown_angleOx.Value,
                (float)numericUpDown_angleOy.Value,
                (float)numericUpDown_angleOz.Value
            );

            facade._execute(
                new RotateModelCommand(ref canvas, ref rotate, currentIndex),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        #endregion

        #region Camera

        private void button_cameraForward_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraForwardMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraBack_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraBackMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraRight_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraRightMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_CmeraLeft_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraLeft_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraLeftMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraRightUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpRightMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraRightDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownRightMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraLeftUp_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraUpLeftMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraLeftDown_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraDownLeftMoveCommand(ref canvas, 15),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraRotateRight_Click(object sender, EventArgs e)
        {
            facade._execute(
                new CameraRightRotateCommand(ref canvas, 10),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraRotateLeft_Click(object sender, EventArgs e)
        {
            facade._execute(new CameraLeftRotateCommand(ref canvas, 10),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraRotateDown_Click(object sender, EventArgs e)
        {
            facade._execute(new CameraDownRotateCommand(ref canvas, 10),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void button_cameraRotateUp_Click(object sender, EventArgs e)
        {
            facade._execute(new CameraUpRotateCommand(ref canvas, 10),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void trackBar_yaw_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_yaw.Value = trackBar_yaw.Value;

            facade._execute(new CameraYawCommand(ref canvas, trackBar_yaw.Value),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void trackBar_pitch_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_pitch.Value = trackBar_pitch.Value;

            facade._execute(new CameraPitchCommand(ref canvas, trackBar_pitch.Value),
                new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                    ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel)
            );
        }

        private void numericUpDown_yaw_ValueChanged(object sender, EventArgs e)
        {
            trackBar_yaw.Value = (int)numericUpDown_yaw.Value;
        }

        private void numericUpDown_pitch_ValueChanged(object sender, EventArgs e)
        {
            trackBar_pitch.Value = (int)numericUpDown_pitch.Value;
        }

        #endregion

        #region Functions

        private int GetCurrentModelIndex()
        {
            int currentIndex = -1;

            try
            {
                currentIndex = listView_modelsMain.SelectedItems[0].Index;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                facade._execute(new ErrorMessageShowCommand("Модель не выбрана."));
            }

            return currentIndex;
        }

        private RenderMode GetCurrentRenderMode()
        {
            if (checkedListBox_renderMode.CheckedItems.Count == 0)
            {
                facade._execute(new ErrorMessageShowCommand("Режим вывода изображения не выбран. Установлен режим по умолчанию: \"Каркасное отображение\"."));
                checkedListBox_renderMode.SetItemChecked(3, true);
                return RenderMode.CarcassDisplay;
            }

            if (checkedListBox_renderMode.GetItemChecked(0))
                return RenderMode.Shadows;
            else if (checkedListBox_renderMode.GetItemChecked(1))
                return RenderMode.Shading;
            else if (checkedListBox_renderMode.GetItemChecked(2))
                return RenderMode.RealDisplay;
            else
                return RenderMode.CarcassDisplay;
        }

        #endregion

        #region Other

        private void checkedListBox_renderMode_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue != CheckState.Checked)
                return;

            for (int i = 0; i < checkedListBox_renderMode.Items.Count; i++)
            {
                if (i != e.Index)
                    checkedListBox_renderMode.SetItemChecked(i, false);
            }

            switch (e.Index)
            {
                case 0:
                    facade._execute(new RenderCommand(ref canvas, ref picture, RenderMode.Shadows,
                        ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
                    break;
                case 1:
                    facade._execute(new RenderCommand(ref canvas, ref picture, RenderMode.Shading,
                        ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
                    break;
                case 2:
                    facade._execute(new RenderCommand(ref canvas, ref picture, RenderMode.RealDisplay,
                        ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
                    break;
                default:
                    facade._execute(new RenderCommand(ref canvas, ref picture, RenderMode.CarcassDisplay,
                        ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
                    break;
            }
        }

        private void numericUpDown_lightIntensity_ValueChanged(object sender, EventArgs e)
        {
            canvas.LightIntensity = (float)numericUpDown_lightIntensity.Value;
            facade._execute(new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
        }

        private void numericUpDown_lightDirectionX_ValueChanged(object sender, EventArgs e)
        {
            Vector3D newDirection = new Vector3D(canvas.LightDirection);
            newDirection.X = (float)numericUpDown_lightDirectionX.Value;

            canvas.LightDirection = newDirection;
            facade._execute(new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
        }

        private void numericUpDown_lightDirectionY_ValueChanged(object sender, EventArgs e)
        {
            Vector3D newDirection = new Vector3D(canvas.LightDirection);
            newDirection.Y = (float)numericUpDown_lightDirectionY.Value;

            canvas.LightDirection = newDirection;
            facade._execute(new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
        }

        private void numericUpDown_lightDirectionZ_ValueChanged(object sender, EventArgs e)
        {
            Vector3D newDirection = new Vector3D(canvas.LightDirection);
            newDirection.Z = (float)numericUpDown_lightDirectionZ.Value;

            canvas.LightDirection = newDirection;
            facade._execute(new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
        }

        private void numericUpDown_lightPositionX_ValueChanged(object sender, EventArgs e)
        {
            canvas.LightPosition.X = (int)numericUpDown_lightPositionX.Value;
            facade._execute(new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
        }

        private void numericUpDown_lightPositionY_ValueChanged(object sender, EventArgs e)
        {
            canvas.LightPosition.Y = (int)numericUpDown_lightPositionY.Value;
            facade._execute(new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
        }

        private void numericUpDown_lightPositionZ_ValueChanged(object sender, EventArgs e)
        {
            canvas.LightPosition.Z = (int)numericUpDown_lightPositionZ.Value;
            facade._execute(new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
        }

        private void button_moveLight_Click(object sender, EventArgs e)
        {
            Move move = new Move(
                (float)numericUpDown_moveLightX.Value,
                (float)numericUpDown_moveLightY.Value,
                (float)numericUpDown_moveLightZ.Value
            );

            canvas.MoveLight(move);

            numericUpDown_lightPositionX.Value = (decimal)canvas.LightPosition.X;
            numericUpDown_lightPositionY.Value = (decimal)canvas.LightPosition.Y;
            numericUpDown_lightPositionZ.Value = (decimal)canvas.LightPosition.Z;

            facade._execute(new RenderCommand(ref canvas, ref picture, GetCurrentRenderMode(),
                ref toolStripStatusLabel_lastRender, ref toolStripStatusLabel_statusLabel));
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void toolTip1_Popup_1(object sender, PopupEventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkedBox_algorithm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        #endregion


        
    }
}
