namespace ShopUI
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.flowItems = new System.Windows.Forms.FlowLayoutPanel();
            this.lblGold = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.flowUnits = new System.Windows.Forms.FlowLayoutPanel();
            this.flowItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowItems
            // 
            this.flowItems.Controls.Add(this.lblGold);
            this.flowItems.Controls.Add(this.flowUnits);
            this.flowItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowItems.Location = new System.Drawing.Point(0, 0);
            this.flowItems.Name = "flowItems";
            this.flowItems.Size = new System.Drawing.Size(800, 450);
            this.flowItems.TabIndex = 0;
            // 
            // lblGold
            // 
            this.lblGold.AutoSize = true;
            this.lblGold.Location = new System.Drawing.Point(3, 0);
            this.lblGold.Name = "lblGold";
            this.lblGold.Size = new System.Drawing.Size(58, 12);
            this.lblGold.TabIndex = 0;
            this.lblGold.Text = "골드: 15G";
            // 
            // flowUnits
            // 
            this.flowUnits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowUnits.Location = new System.Drawing.Point(67, 3);
            this.flowUnits.Name = "flowUnits";
            this.flowUnits.Size = new System.Drawing.Size(200, 6);
            this.flowUnits.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flowItems);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.flowItems.ResumeLayout(false);
            this.flowItems.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowItems;
        private System.Windows.Forms.Label lblGold;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.FlowLayoutPanel flowUnits;
    }
}

