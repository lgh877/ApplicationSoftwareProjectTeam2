using ApplicationSoftwareProjectTeam2.items;

namespace ShopUI
{
    public partial class ShopUI : Form
    {
        private ItemStore store;
        private Player player;

        private List<Unit> units = new List<Unit>();
        private Unit selectedUnit = null;

        //Ư�� ���� ������ ���� ����Ʈ �Լ�
        private void UpdateEquippedList()
        {
            lstEquipped.Items.Clear();

            if (selectedUnit != null)
            {
                foreach (var item in selectedUnit.EquippedItems)
                {
                    lstEquipped.Items.Add(item.Name);
                }

                lblSlotStatus.Text = $"����: {selectedUnit.EquippedItems.Count} / {selectedUnit.MaxItemSlots}";
            }
            else
            {
                lblSlotStatus.Text = "���� �̼���";
            }
        }

        public ShopUI()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //������ ��� FlowLayoutPanel ���� ���� + ���� ��ư Ŭ���� ��� ���� �� �޽��� ���

            store = new ItemStore();
            player = new Player { Gold = 100 };

            lblGold.Text = $"���: {player.Gold}G";

            // ������ ī�� ����
            foreach (var item in store.AvailableItems)
            {
                Panel card = new Panel();
                card.Width = 300;
                card.Height = 200;
                card.Margin = new Padding(5);
                card.BorderStyle = BorderStyle.FixedSingle;

                Label lblName = new Label();
                lblName.Height = 40;
                lblName.Text = item.Name;
                lblName.Dock = DockStyle.Top;
                lblName.TextAlign = ContentAlignment.MiddleCenter;

                Label lblPrice = new Label();
                lblPrice.Height = 40;
                lblPrice.Text = $"{item.Price}G";
                lblPrice.Dock = DockStyle.Bottom;
                lblPrice.TextAlign = ContentAlignment.MiddleCenter;

                Button btnEquip = new Button();
                btnEquip.Text = "����";
                btnEquip.Dock = DockStyle.Fill;
                btnEquip.Tag = item;
                btnEquip.Click += BtnEquip_Click;

                // ���콺 �ø��� ����� �⺻ tooltip ����
                toolTip1.SetToolTip(lblName, item.Description);
                toolTip1.SetToolTip(btnEquip, item.Description);

                card.Controls.Add(lblName);
                card.Controls.Add(btnEquip);
                card.Controls.Add(lblPrice);

                flowItems.Controls.Add(card);

            }

            //���� ���� ��� �߰�
            units.Add(new Unit { Name = "��Ŀ A" });
            units.Add(new Unit { Name = "���� B" });
            units.Add(new Unit { Name = "Ư�� C" });

            foreach (var unit in units)
            {
                Button btnUnit = new Button();
                btnUnit.Text = unit.Name;
                btnUnit.Width = 100;
                btnUnit.Height = 40;
                btnUnit.Margin = new Padding(5);
                btnUnit.Tag = unit;
                btnUnit.Click += BtnUnit_Click;

                flowUnits.Controls.Add(btnUnit);
            }

        }


        // ���� ���� ��ư
        private void BtnUnit_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            selectedUnit = btn.Tag as Unit;

            UpdateEquippedList();
        }


        // ���� ��ư
        private void BtnEquip_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var item = button.Tag as Item;

            if (selectedUnit == null)
            {
                MessageBox.Show("������ ���� �����ϼ���!");
                return;
            }

            if (player.PurchaseItem(item, selectedUnit))
            {
                lblGold.Text = $"���: {player.Gold}G";
                UpdateEquippedList();
            }
            else
            {
                MessageBox.Show("���� ���� (��� ���� �Ǵ� ���� �ʰ�)");
            }
        }
    }
}
