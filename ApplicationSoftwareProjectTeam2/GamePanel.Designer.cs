namespace ApplicationSoftwareProjectTeam2
{
    partial class GamePanel
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
            components = new System.ComponentModel.Container();
            logicTick = new System.Windows.Forms.Timer(components);
            panelPlayScreen = new Panel();
            label1 = new Label();
            btnGameStart = new Button();
            button1 = new Button();
            panelPlayScreen.SuspendLayout();
            SuspendLayout();
            // 
            // logicTick
            // 
            logicTick.Enabled = true;
            logicTick.Interval = 42;
            logicTick.Tick += logicTick_Tick;
            // 
            // panelPlayScreen
            // 
            panelPlayScreen.BackColor = SystemColors.ButtonShadow;
            panelPlayScreen.Controls.Add(label1);
            panelPlayScreen.Controls.Add(btnGameStart);
            panelPlayScreen.Controls.Add(button1);
            panelPlayScreen.Location = new Point(6, 6);
            panelPlayScreen.Margin = new Padding(2, 1, 2, 1);
            panelPlayScreen.Name = "panelPlayScreen";
            panelPlayScreen.Size = new Size(388, 200);
            panelPlayScreen.TabIndex = 0;
            panelPlayScreen.MouseClick += panelPlayScreen_MouseClick;
            panelPlayScreen.MouseMove += panelPlayScreen_MouseMove;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(0, 0);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // btnGameStart
            // 
            btnGameStart.Location = new Point(233, 1);
            btnGameStart.Margin = new Padding(2, 1, 2, 1);
            btnGameStart.Name = "btnGameStart";
            btnGameStart.Size = new Size(75, 22);
            btnGameStart.TabIndex = 1;
            btnGameStart.Text = "gameStart";
            btnGameStart.UseVisualStyleBackColor = true;
            btnGameStart.Click += btnGameStart_Click;
            // 
            // button1
            // 
            button1.Location = new Point(312, 1);
            button1.Margin = new Padding(2, 1, 2, 1);
            button1.Name = "button1";
            button1.Size = new Size(75, 22);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // GamePanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(405, 226);
            Controls.Add(panelPlayScreen);
            Margin = new Padding(2, 1, 2, 1);
            MinimumSize = new Size(416, 248);
            Name = "GamePanel";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AutoCombatant";
            FormClosed += GamePanel_FormClosed;
            Load += Form1_Load;
            Resize += GamePanel_Resize;
            panelPlayScreen.ResumeLayout(false);
            panelPlayScreen.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer logicTick;
        private Panel panelPlayScreen;
        private Button button1;
        public Label label1;
        private Button btnGameStart;
    }
}
