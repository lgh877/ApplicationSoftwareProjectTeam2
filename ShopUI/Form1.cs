using ApplicationSoftwareProjectTeam2.items;

namespace ShopUI
{
    public partial class ShopUI : Form
    {
        private ItemStore store;
        private Player player;

        private List<Unit> units = new List<Unit>();
        private Unit selectedUnit = null;

        //특정 유닛 아이템 장착 리스트 함수
        private void UpdateEquippedList()
        {
            lstEquipped.Items.Clear();

            if (selectedUnit != null)
            {
                foreach (var item in selectedUnit.EquippedItems)
                {
                    lstEquipped.Items.Add(item.Name);
                }

                lblSlotStatus.Text = $"슬롯: {selectedUnit.EquippedItems.Count} / {selectedUnit.MaxItemSlots}";
            }
            else
            {
                lblSlotStatus.Text = "유닛 미선택";
            }
        }

        public ShopUI()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //아이템 목록 FlowLayoutPanel 동적 생성 + 장착 버튼 클릭시 골드 차감 및 메시지 출력

            store = new ItemStore();
            player = new Player { Gold = 100 };

            lblGold.Text = $"골드: {player.Gold}G";

            // 아이템 카드 생성
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
                btnEquip.Text = "장착";
                btnEquip.Dock = DockStyle.Fill;
                btnEquip.Tag = item;
                btnEquip.Click += BtnEquip_Click;

                // 마우스 올리면 생기는 기본 tooltip 연결
                toolTip1.SetToolTip(lblName, item.Description);
                toolTip1.SetToolTip(btnEquip, item.Description);

                card.Controls.Add(lblName);
                card.Controls.Add(btnEquip);
                card.Controls.Add(lblPrice);

                flowItems.Controls.Add(card);

            }

            //유닛 선택 기능 추가
            units.Add(new Unit { Name = "탱커 A" });
            units.Add(new Unit { Name = "원딜 B" });
            units.Add(new Unit { Name = "특수 C" });

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


        // 유닛 선택 버튼
        private void BtnUnit_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            selectedUnit = btn.Tag as Unit;

            UpdateEquippedList();
        }


        // 장착 버튼
        private void BtnEquip_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var item = button.Tag as Item;

            if (selectedUnit == null)
            {
                MessageBox.Show("유닛을 먼저 선택하세요!");
                return;
            }

            if (player.PurchaseItem(item, selectedUnit))
            {
                lblGold.Text = $"골드: {player.Gold}G";
                UpdateEquippedList();
            }
            else
            {
                MessageBox.Show("장착 실패 (골드 부족 또는 슬롯 초과)");
            }
        }
    }
}
