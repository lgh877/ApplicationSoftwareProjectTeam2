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
            SuspendLayout();
            // 
            // logicTick
            // 
            logicTick.Enabled = true;
            logicTick.Interval = 17;
            logicTick.Tick += logicTick_Tick;
            // 
            // panelPlayScreen
            // 
            panelPlayScreen.BackColor = SystemColors.ButtonShadow;
            panelPlayScreen.Location = new Point(12, 12);
            panelPlayScreen.Name = "panelPlayScreen";
            panelPlayScreen.Size = new Size(776, 426);
            panelPlayScreen.TabIndex = 0;
            // 
            // GamePanel
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panelPlayScreen);
            MinimumSize = new Size(826, 521);
            Name = "GamePanel";
            Text = "Form1";
            Load += Form1_Load;
            Resize += GamePanel_Resize;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer logicTick;
        private Panel panelPlayScreen;
    }
}
