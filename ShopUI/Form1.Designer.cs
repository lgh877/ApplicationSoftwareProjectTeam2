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
            components = new System.ComponentModel.Container();
            flowItems = new FlowLayoutPanel();
            lblGold = new Label();
            flowUnits = new FlowLayoutPanel();
            label1 = new Label();
            lblSlotStatus = new Label();
            lstEquipped = new ListBox();
            toolTip1 = new ToolTip(components);
            flowItems.SuspendLayout();
            SuspendLayout();
            // 
            // flowItems
            // 
            flowItems.Controls.Add(lblGold);
            flowItems.Controls.Add(flowUnits);
            flowItems.Controls.Add(label1);
            flowItems.Controls.Add(lblSlotStatus);
            flowItems.Controls.Add(lstEquipped);
            flowItems.Dock = DockStyle.Fill;
            flowItems.Location = new Point(0, 0);
            flowItems.Margin = new Padding(6, 8, 6, 8);
            flowItems.Name = "flowItems";
            flowItems.Size = new Size(1324, 1200);
            flowItems.TabIndex = 0;
            // 
            // lblGold
            // 
            lblGold.AutoSize = true;
            lblGold.Location = new Point(6, 0);
            lblGold.Margin = new Padding(6, 0, 6, 0);
            lblGold.Name = "lblGold";
            lblGold.Size = new Size(118, 32);
            lblGold.TabIndex = 0;
            lblGold.Text = "골드: 15G";
            // 
            // flowUnits
            // 
            flowUnits.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowUnits.Location = new Point(136, 8);
            flowUnits.Margin = new Padding(6, 8, 6, 8);
            flowUnits.Name = "flowUnits";
            flowUnits.Size = new Size(400, 228);
            flowUnits.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(548, 0);
            label1.Margin = new Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new Size(198, 32);
            label1.TabIndex = 3;
            label1.Text = "장착 아이템 목록";
            // 
            // lblSlotStatus
            // 
            lblSlotStatus.AutoSize = true;
            lblSlotStatus.Location = new Point(758, 0);
            lblSlotStatus.Margin = new Padding(6, 0, 6, 0);
            lblSlotStatus.Name = "lblSlotStatus";
            lblSlotStatus.Size = new Size(127, 32);
            lblSlotStatus.TabIndex = 4;
            lblSlotStatus.Text = "슬롯: 0 / 3";
            // 
            // lstEquipped
            // 
            lstEquipped.Anchor = AnchorStyles.Right;
            lstEquipped.FormattingEnabled = true;
            lstEquipped.Location = new Point(897, 8);
            lstEquipped.Margin = new Padding(6, 8, 6, 8);
            lstEquipped.Name = "lstEquipped";
            lstEquipped.Size = new Size(236, 228);
            lstEquipped.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1324, 1200);
            Controls.Add(flowItems);
            Margin = new Padding(6, 8, 6, 8);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            flowItems.ResumeLayout(false);
            flowItems.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowItems;
        private System.Windows.Forms.Label lblGold;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.FlowLayoutPanel flowUnits;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstEquipped;
        private System.Windows.Forms.Label lblSlotStatus;
    }
}
