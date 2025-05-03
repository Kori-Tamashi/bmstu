namespace Eventor.GUI
{
    partial class ParticipantsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            title_label = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            participants_dataGridView = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            _timer = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)participants_dataGridView).BeginInit();
            SuspendLayout();
            // 
            // title_label
            // 
            title_label.Anchor = AnchorStyles.None;
            title_label.AutoSize = true;
            title_label.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            title_label.Location = new Point(211, 23);
            title_label.Name = "title_label";
            title_label.Size = new Size(253, 28);
            title_label.TabIndex = 0;
            title_label.Text = "Участники мероприятия";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(title_label, 0, 0);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(676, 74);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // participants_dataGridView
            // 
            participants_dataGridView.AllowUserToAddRows = false;
            participants_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            participants_dataGridView.BackgroundColor = SystemColors.Menu;
            participants_dataGridView.BorderStyle = BorderStyle.None;
            participants_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            participants_dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column4 });
            participants_dataGridView.Location = new Point(12, 92);
            participants_dataGridView.MultiSelect = false;
            participants_dataGridView.Name = "participants_dataGridView";
            participants_dataGridView.RowHeadersWidth = 51;
            participants_dataGridView.Size = new Size(676, 346);
            participants_dataGridView.TabIndex = 2;
            // 
            // Column1
            // 
            Column1.DataPropertyName = "Id";
            Column1.HeaderText = "Id";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Visible = false;
            // 
            // Column2
            // 
            Column2.DataPropertyName = "Name";
            Column2.HeaderText = "Имя";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column4
            // 
            Column4.DataPropertyName = "Paid";
            Column4.HeaderText = "Оплата";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            // 
            // _timer
            // 
            _timer.Interval = 15000;
            // 
            // ParticipantsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 450);
            Controls.Add(participants_dataGridView);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ParticipantsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Участники мероприятия";
            Load += Participants_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)participants_dataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label title_label;
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView participants_dataGridView;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Timer _timer;
    }
}