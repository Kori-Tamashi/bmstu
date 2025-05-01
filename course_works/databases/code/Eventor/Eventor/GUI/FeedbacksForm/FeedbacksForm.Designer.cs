namespace Eventor.GUI
{
    partial class FeedbacksForm
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
            eventFeedbacks_label = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            feedbacks_dataGridView = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            _timer = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)feedbacks_dataGridView).BeginInit();
            SuspendLayout();
            // 
            // eventFeedbacks_label
            // 
            eventFeedbacks_label.Anchor = AnchorStyles.None;
            eventFeedbacks_label.AutoSize = true;
            eventFeedbacks_label.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            eventFeedbacks_label.Location = new Point(139, 9);
            eventFeedbacks_label.Name = "eventFeedbacks_label";
            eventFeedbacks_label.Size = new Size(228, 28);
            eventFeedbacks_label.TabIndex = 0;
            eventFeedbacks_label.Text = "Отзывы мероприятия";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(eventFeedbacks_label, 0, 0);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(507, 47);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // feedbacks_dataGridView
            // 
            feedbacks_dataGridView.AllowUserToAddRows = false;
            feedbacks_dataGridView.AllowUserToResizeRows = false;
            feedbacks_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            feedbacks_dataGridView.BackgroundColor = SystemColors.Menu;
            feedbacks_dataGridView.BorderStyle = BorderStyle.None;
            feedbacks_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            feedbacks_dataGridView.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6 });
            feedbacks_dataGridView.Location = new Point(12, 65);
            feedbacks_dataGridView.Name = "feedbacks_dataGridView";
            feedbacks_dataGridView.RowHeadersWidth = 51;
            feedbacks_dataGridView.Size = new Size(507, 373);
            feedbacks_dataGridView.TabIndex = 2;
            feedbacks_dataGridView.CellClick += feedbacks_dataGridView_CellClick;
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
            Column2.DataPropertyName = "EventId";
            Column2.HeaderText = "EventId";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Visible = false;
            // 
            // Column3
            // 
            Column3.DataPropertyName = "PersonId";
            Column3.HeaderText = "PersonId";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Visible = false;
            // 
            // Column4
            // 
            Column4.DataPropertyName = "Name";
            Column4.HeaderText = "Участник";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            // 
            // Column5
            // 
            Column5.DataPropertyName = "Comment";
            Column5.HeaderText = "Комментарий";
            Column5.MinimumWidth = 6;
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            // 
            // Column6
            // 
            Column6.DataPropertyName = "Rating";
            Column6.HeaderText = "Рейтинг";
            Column6.MinimumWidth = 6;
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            // 
            // _timer
            // 
            _timer.Interval = 15000;
            // 
            // FeedbacksForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(531, 450);
            Controls.Add(feedbacks_dataGridView);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FeedbacksForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Отзывы мероприятия";
            Load += FeedbacksForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)feedbacks_dataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label eventFeedbacks_label;
        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView feedbacks_dataGridView;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.Timer _timer;
    }
}