using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicationSoftwareProjectTeam2.items;
namespace ShopUI
{
    public partial class Form1 : Form
    {
        private ItemStore store;
        private Player player;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //아이템 목록 FlowLayoutPanel 동적 생성 + 장착 버튼 클릭시 골드 차감 및 메시지 출력

            store = new ItemStore();
            player = new Player { Gold = 15 };

            lblGold.Text = $"골드: {player.Gold}G";

            foreach (var item in store.AvailableItems)
            {
                Panel card = new Panel();
                card.Width = 150;
                card.Height = 100;
                card.Margin = new Padding(5);
                card.BorderStyle = BorderStyle.FixedSingle;

                Label lblName = new Label();
                lblName.Text = item.Name;
                lblName.Dock = DockStyle.Top;
                lblName.TextAlign = ContentAlignment.MiddleCenter;

                Label lblPrice = new Label();
                lblPrice.Text = $"{item.Price}G";
                lblPrice.Dock = DockStyle.Bottom;
                lblPrice.TextAlign = ContentAlignment.MiddleCenter;

                Button btnEquip = new Button();
                btnEquip.Text = "장착";
                btnEquip.Dock = DockStyle.Fill;
                btnEquip.Tag = item;
                btnEquip.Click += BtnEquip_Click;

                //tooltip연결
                toolTip1.SetToolTip(lblName, item.Description);
                toolTip1.SetToolTip(btnEquip, item.Description);

                card.Controls.Add(lblName);
                card.Controls.Add(btnEquip);
                card.Controls.Add(lblPrice);

                flowItems.Controls.Add(card);

            }


        }

        private void BtnEquip_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var item = button.Tag as Item;

            var unit = new Unit { Name = "기본 유닛" };

            if (player.PurchaseItem(item, unit))
            {
                MessageBox.Show($"{item.Name} 장착 성공!");
                lblGold.Text = $"골드: {player.Gold}G";
            }
            else
            {
                MessageBox.Show("장착 실패 (골드 부족 또는 슬롯 초과)");
            }
        }


    }
}
