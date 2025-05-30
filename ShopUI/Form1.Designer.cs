namespace ShopUI
{
    partial class ShopUI
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
            btnSell = new Button();
            toolTip1 = new ToolTip(components);
            lstInventory = new ListBox();
            lblInventory = new Label();
            btnUnequip = new Button();
            btnEquipFromInventory = new Button();
            SuspendLayout();
            // 
            // flowItems
            // 
            flowItems.Location = new Point(0, 180);
            flowItems.Margin = new Padding(3, 4, 3, 4);
            flowItems.Name = "flowItems";
            flowItems.Size = new Size(662, 662);
            flowItems.TabIndex = 0;
            // 
            // lblGold
            // 
            lblGold.AutoSize = true;
            lblGold.Location = new Point(12, 9);
            lblGold.Name = "lblGold";
            lblGold.Size = new Size(60, 15);
            lblGold.TabIndex = 0;
            lblGold.Text = "골드: 15G";
            // 
            // flowUnits
            // 
            flowUnits.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowUnits.Location = new Point(12, 28);
            flowUnits.Margin = new Padding(3, 4, 3, 4);
            flowUnits.Name = "flowUnits";
            flowUnits.Size = new Size(200, 139);
            flowUnits.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(229, 9);
            label1.Name = "label1";
            label1.Size = new Size(99, 15);
            label1.TabIndex = 3;
            label1.Text = "장착 아이템 목록";
            // 
            // lblSlotStatus
            // 
            lblSlotStatus.AutoSize = true;
            lblSlotStatus.Location = new Point(344, 9);
            lblSlotStatus.Name = "lblSlotStatus";
            lblSlotStatus.Size = new Size(65, 15);
            lblSlotStatus.TabIndex = 4;
            lblSlotStatus.Text = "슬롯: 0 / 3";
            // 
            // lstEquipped
            // 
            lstEquipped.Anchor = AnchorStyles.Right;
            lstEquipped.FormattingEnabled = true;
            lstEquipped.ItemHeight = 15;
            lstEquipped.Location = new Point(218, 28);
            lstEquipped.Margin = new Padding(3, 4, 3, 4);
            lstEquipped.Name = "lstEquipped";
            lstEquipped.Size = new Size(120, 139);
            lstEquipped.TabIndex = 2;
            // 
            // btnSell
            // 
            btnSell.Location = new Point(344, 28);
            btnSell.Name = "btnSell";
            btnSell.Size = new Size(75, 23);
            btnSell.TabIndex = 5;
            btnSell.Text = "판매";
            btnSell.UseVisualStyleBackColor = true;
            btnSell.Click += btnSell_Click;
            // 
            // lstInventory
            // 
            lstInventory.FormattingEnabled = true;
            lstInventory.ItemHeight = 15;
            lstInventory.Location = new Point(425, 28);
            lstInventory.Name = "lstInventory";
            lstInventory.Size = new Size(200, 139);
            lstInventory.TabIndex = 6;
            // 
            // lblInventory
            // 
            lblInventory.AutoSize = true;
            lblInventory.Location = new Point(491, 10);
            lblInventory.Name = "lblInventory";
            lblInventory.Size = new Size(55, 15);
            lblInventory.TabIndex = 7;
            lblInventory.Text = "인벤토리";
            // 
            // btnUnequip
            // 
            btnUnequip.Location = new Point(344, 66);
            btnUnequip.Name = "btnUnequip";
            btnUnequip.Size = new Size(75, 23);
            btnUnequip.TabIndex = 8;
            btnUnequip.Text = "장착 해제";
            btnUnequip.UseVisualStyleBackColor = true;
            btnUnequip.Click += btnUnequip_Click;
            // 
            // btnEquipFromInventory
            // 
            btnEquipFromInventory.Location = new Point(344, 108);
            btnEquipFromInventory.Name = "btnEquipFromInventory";
            btnEquipFromInventory.Size = new Size(75, 23);
            btnEquipFromInventory.TabIndex = 9;
            btnEquipFromInventory.Text = "장착";
            btnEquipFromInventory.UseVisualStyleBackColor = true;
            btnEquipFromInventory.Click += btnEquipFromInventory_Click;
            // 
            // ShopUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(662, 542);
            Controls.Add(btnEquipFromInventory);
            Controls.Add(lblInventory);
            Controls.Add(btnUnequip);
            Controls.Add(btnSell);
            Controls.Add(lblSlotStatus);
            Controls.Add(label1);
            Controls.Add(lstEquipped);
            Controls.Add(flowUnits);
            Controls.Add(lblGold);
            Controls.Add(lstInventory);
            Controls.Add(flowItems);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ShopUI";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowItems;
        private System.Windows.Forms.Label lblGold;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.FlowLayoutPanel flowUnits;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstEquipped;
        private System.Windows.Forms.Label lblSlotStatus;
        private Button btnSell;
        private ListBox lstInventory;
        private Button btnUnequip;
        private Label lblInventory;
        private Button btnEquipFromInventory;
    }
}
