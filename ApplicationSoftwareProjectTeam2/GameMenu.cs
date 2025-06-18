using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationSoftwareProjectTeam2
{
    public partial class GameMenu : Form
    {
        public GameMenu()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            GamePanel gamePanel = new GamePanel();
            gamePanel.Owner = this;
            gamePanel.clientPlayer = new items.Player();
            gamePanel.clientPlayer.actualPlayerName = txtInsertName.Text;
            gamePanel.clientPlayer.playerName = txtInsertName.Text + new Random().NextInt64();
            gamePanel.Show();
            this.Hide();
        }
    }
}
