namespace client
{
    partial class gameplay
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
            this.components = new System.ComponentModel.Container();
            this.txtMove = new System.Windows.Forms.TextBox();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblTimer = new System.Windows.Forms.Label();
            this.listViewRank = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // txtMove
            // 
            this.txtMove.Location = new System.Drawing.Point(59, 49);
            this.txtMove.Name = "txtMove";
            this.txtMove.Size = new System.Drawing.Size(100, 28);
            this.txtMove.TabIndex = 0;
            // 
            // listBoxLog
            // 
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.ItemHeight = 18;
            this.listBoxLog.Location = new System.Drawing.Point(59, 121);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(304, 292);
            this.listBoxLog.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(308, 49);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "button1";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Visible = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Location = new System.Drawing.Point(562, 53);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(54, 18);
            this.lblTimer.TabIndex = 3;
            this.lblTimer.Text = "label1";
            // 
            // listViewRank
            // 
            this.listViewRank.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewRank.GridLines = true;
            this.listViewRank.HideSelection = false;
            this.listViewRank.Location = new System.Drawing.Point(445, 121);
            this.listViewRank.Name = "listViewRank";
            this.listViewRank.Size = new System.Drawing.Size(237, 292);
            this.listViewRank.TabIndex = 4;
            this.listViewRank.UseCompatibleStateImageBehavior = false;
            this.listViewRank.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "순위";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "이름(점수)";
            this.columnHeader2.Width = 120;
            // 
            // imer1
            // 
            this.imer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // gameplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listViewRank);
            this.Controls.Add(this.lblTimer);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.listBoxLog);
            this.Controls.Add(this.txtMove);
            this.Name = "gameplay";
            this.Text = "gameplay";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMove;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.ListView listViewRank;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Timer imer1;
    }
}