namespace ApplicationSoftwareProjectTeam2
{
    partial class GameMenu
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
            lblTitle = new Label();
            btnStart = new Button();
            lblInsertName = new Label();
            txtInsertName = new TextBox();
            ckbClient = new CheckBox();
            txbIpAddress = new TextBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.None;
            lblTitle.AutoSize = true;
            lblTitle.BorderStyle = BorderStyle.Fixed3D;
            lblTitle.Font = new Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblTitle.Location = new Point(62, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(676, 88);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "|| Auto Combatant ||";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnStart
            // 
            btnStart.AutoSize = true;
            btnStart.Font = new Font("맑은 고딕", 19.875F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnStart.Location = new Point(213, 312);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(381, 126);
            btnStart.TabIndex = 1;
            btnStart.Text = "Game Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // lblInsertName
            // 
            lblInsertName.Location = new Point(213, 220);
            lblInsertName.Name = "lblInsertName";
            lblInsertName.Size = new Size(155, 32);
            lblInsertName.TabIndex = 2;
            lblInsertName.Text = "닉네임 입력 :";
            // 
            // txtInsertName
            // 
            txtInsertName.Location = new Point(374, 213);
            txtInsertName.Name = "txtInsertName";
            txtInsertName.Size = new Size(220, 39);
            txtInsertName.TabIndex = 3;
            // 
            // ckbClient
            // 
            ckbClient.AutoSize = true;
            ckbClient.Checked = true;
            ckbClient.CheckState = CheckState.Checked;
            ckbClient.Location = new Point(213, 267);
            ckbClient.Name = "ckbClient";
            ckbClient.Size = new Size(166, 36);
            ckbClient.TabIndex = 4;
            ckbClient.Text = "싱글플레이";
            ckbClient.UseVisualStyleBackColor = true;
            ckbClient.CheckedChanged += ckbClient_CheckedChanged;
            // 
            // txbIpAddress
            // 
            txbIpAddress.Enabled = false;
            txbIpAddress.Location = new Point(394, 267);
            txbIpAddress.Name = "txbIpAddress";
            txbIpAddress.Size = new Size(200, 39);
            txbIpAddress.TabIndex = 5;
            // 
            // GameMenu
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            ClientSize = new Size(800, 450);
            Controls.Add(txbIpAddress);
            Controls.Add(ckbClient);
            Controls.Add(txtInsertName);
            Controls.Add(lblInsertName);
            Controls.Add(btnStart);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "GameMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GameMenu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Button btnStart;
        private Label lblInsertName;
        private TextBox txtInsertName;
        private CheckBox ckbClient;
        private TextBox txbIpAddress;
    }
}