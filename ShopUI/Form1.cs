using ApplicationSoftwareProjectTeam2.items;

namespace ShopUI
{
    public partial class ShopUI : Form
    {
        private ItemStore store;
        private Player player;

        private List<Unit> units = new List<Unit>();
        private Unit selectedUnit = null;

        // 아이템 장착 해제 및 장착 리스트
        private List<Item> inventory = new List<Item>();

        //특정 유닛 아이템 장착 리스트 함수
        private void UpdateEquippedList()
        {
            lstEquipped.Items.Clear();

            if (selectedUnit != null)
            {
                foreach (var item in selectedUnit.EquippedItems)
                {
                    lstEquipped.Items.Add(item);
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
                card.Width = 100;
                card.Height = 80;
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

        // 판매 버튼
        private void btnSell_Click(object sender, EventArgs e)
        {
            if (selectedUnit == null)
            {
                MessageBox.Show("유닛을 먼저 선택하세요.");
                return;
            }

            if (lstEquipped.SelectedItem == null)
            {
                MessageBox.Show("판매할 아이템을 선택하세요.");
                return;
            }

            Item targetItem = lstEquipped.SelectedItem as Item;

            if (targetItem != null)
            {
                selectedUnit.EquippedItems.Remove(targetItem);
                player.Gold += targetItem.Price / 2;

                lblGold.Text = $"골드: {player.Gold}G";
                UpdateEquippedList();
            }
        }

        private void btnUnequip_Click(object sender, EventArgs e)
        {
            if (selectedUnit == null || lstEquipped.SelectedItem == null) return;

            Item item = lstEquipped.SelectedItem as Item;

            if (item != null)
            {
                selectedUnit.EquippedItems.Remove(item);
                inventory.Add(item);

                UpdateEquippedList();
                UpdateInventoryList(); // 아직 없으면 다음 단계에서 추가
            }
        }

        private void btnEquipFromInventory_Click(object sender, EventArgs e)
        {
            if (selectedUnit == null || lstInventory.SelectedItem == null) return;

            Item item = lstInventory.SelectedItem as Item;

            if (item != null && selectedUnit.EquipItem(item))
            {
                inventory.Remove(item);

                UpdateEquippedList();
                UpdateInventoryList();
            }
            else
            {
                MessageBox.Show("장착 실패 (슬롯 초과)");
            }
        }

        private void UpdateInventoryList()
        {
            lstInventory.Items.Clear();

            foreach (var item in inventory)
            {
                lstInventory.Items.Add(item);
            }
        }
    }
}
