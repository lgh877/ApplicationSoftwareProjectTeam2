using ApplicationSoftwareProjectTeam2;

namespace GameMenu
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            GamePanel panel = new GamePanel();
            panel.Owner = this;
            panel.Show();
            this.Hide();
        }
    }
}
