namespace GameMenu
{
    partial class MenuForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStart = new Button();
            txtInsertName = new TextBox();
            lblInsertName = new Label();
            lblTitle = new Label();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Font = new Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnStart.Location = new Point(196, 307);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(404, 131);
            btnStart.TabIndex = 0;
            btnStart.Text = "Game Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // txtInsertName
            // 
            txtInsertName.Location = new Point(357, 262);
            txtInsertName.Name = "txtInsertName";
            txtInsertName.Size = new Size(243, 39);
            txtInsertName.TabIndex = 1;
            // 
            // lblInsertName
            // 
            lblInsertName.AutoSize = true;
            lblInsertName.Location = new Point(196, 269);
            lblInsertName.Name = "lblInsertName";
            lblInsertName.Size = new Size(155, 32);
            lblInsertName.TabIndex = 2;
            lblInsertName.Text = "닉네임 입력 :";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblTitle.Location = new Point(82, 35);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(674, 86);
            lblTitle.TabIndex = 3;
            lblTitle.Text = "|| Auto Combatant ||";
            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            ClientSize = new Size(800, 450);
            Controls.Add(lblTitle);
            Controls.Add(lblInsertName);
            Controls.Add(txtInsertName);
            Controls.Add(btnStart);
            MaximumSize = new Size(826, 521);
            MinimumSize = new Size(826, 521);
            Name = "MenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Menu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private TextBox txtInsertName;
        private Label lblInsertName;
        private Label lblTitle;
    }
}
