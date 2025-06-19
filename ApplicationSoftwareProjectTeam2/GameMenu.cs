using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicationSoftwareProjectTeam2.entities.creatures;
using ApplicationSoftwareProjectTeam2.resources.sounds;
using WMPLib;

namespace ApplicationSoftwareProjectTeam2
{
    public partial class GameMenu : Form
    {
        public GameMenu()
        {
            InitializeComponent();
            //Controls.Add(txtInsertName);
            //Controls.Add(lblInsertName);
            //Controls.Add(btnStart);
            //Controls.Add(lblTitle);
            this.Shown += GameMenu_Shown;
        }
        private async void GameMenu_Shown(object sender, EventArgs e)
        {
            // 로딩 레이블 생성 및 추가
            txtInsertName.Hide();
            lblInsertName.Hide();
            btnStart.Hide();
            lblTitle.Hide();
            txbIpAddress.Hide();
            ckbClient.Hide();
            Label lblLoading = new Label();
            lblLoading.AutoSize = true;
            lblLoading.BackColor = SystemColors.ActiveCaption;
            lblLoading.Font = new Font("맑은 고딕", 36F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblLoading.Location = new Point(145, 177);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(493, 128);
            lblLoading.TabIndex = 4;
            lblLoading.Text = "Loading...";
            lblLoading.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(lblLoading);

            // UI가 레이블을 먼저 렌더링할 수 있도록 잠깐 대기
            await Task.Delay(50);

            // loadResources를 백그라운드 스레드에서 실행하여 UI 블록을 피함
            await Task.Run(() => loadResources());

            // 로딩이 완료되면 레이블 제거
            Controls.Remove(lblLoading);

            txtInsertName.Show();
            lblInsertName.Show();
            btnStart.Show();
            lblTitle.Show();
            txbIpAddress.Show();
            ckbClient.Show();
        }

        private void loadResources()
        {
            //음향효과 로딩
            SoundCache.bone_crack1 = new WindowsMediaPlayer() { URL = "sounds//bone_crack1.mp3" }; SoundCache.bone_crack1.controls.stop();
            SoundCache.bone_crack2 = new WindowsMediaPlayer() { URL = "sounds//bone_crack2.mp3" }; SoundCache.bone_crack2.controls.stop();

            SoundCache.swosh = new WindowsMediaPlayer() { URL = "sounds//swosh.mp3" }; SoundCache.swosh.controls.stop();

            SoundCache.skulls_bite = new WindowsMediaPlayer() { URL = "sounds//skulls_bite.mp3" }; SoundCache.skulls_bite.controls.stop();

            SoundCache.bonely_punch1 = new WindowsMediaPlayer() { URL = "sounds//bonely_punch1.mp3" }; SoundCache.bonely_punch1.controls.stop();
            SoundCache.bonely_punch2 = new WindowsMediaPlayer() { URL = "sounds//bonely_punch2.mp3" }; SoundCache.bonely_punch2.controls.stop();
            SoundCache.bonely_punch3 = new WindowsMediaPlayer() { URL = "sounds//bonely_punch3.mp3" }; SoundCache.bonely_punch3.controls.stop();

            SoundCache.slap1 = new WindowsMediaPlayer() { URL = "sounds//slap1.mp3" }; SoundCache.slap1.controls.stop();
            SoundCache.slap2 = new WindowsMediaPlayer() { URL = "sounds//slap2.mp3" }; SoundCache.slap2.controls.stop();
            SoundCache.slap3 = new WindowsMediaPlayer() { URL = "sounds//slap3.mp3" }; SoundCache.slap3.controls.stop();

            SoundCache.explosion1 = new WindowsMediaPlayer() { URL = "sounds//explosion1.mp3" }; SoundCache.explosion1.controls.stop();
            SoundCache.explosion2 = new WindowsMediaPlayer() { URL = "sounds//explosion2.mp3" }; SoundCache.explosion2.controls.stop();
            SoundCache.explosion3 = new WindowsMediaPlayer() { URL = "sounds//explosion3.mp3" }; SoundCache.explosion3.controls.stop();
            SoundCache.explosion4 = new WindowsMediaPlayer() { URL = "sounds//explosion4.mp3" }; SoundCache.explosion4.controls.stop();

            SoundCache.explosionSmall1 = new WindowsMediaPlayer() { URL = "sounds//explosionSmall1.mp3" }; SoundCache.explosionSmall1.controls.stop();
            SoundCache.explosionSmall2 = new WindowsMediaPlayer() { URL = "sounds//explosionSmall2.mp3" }; SoundCache.explosionSmall2.controls.stop();

            SoundCache.jump1 = new WindowsMediaPlayer() { URL = "sounds//jump1.mp3" }; SoundCache.jump1.controls.stop();
            SoundCache.jump2 = new WindowsMediaPlayer() { URL = "sounds//jump2.mp3" }; SoundCache.jump2.controls.stop();

            SoundCache.chainsaw1 = new WindowsMediaPlayer() { URL = "sounds//chainsaw1.mp3" }; SoundCache.chainsaw1.controls.stop();
            SoundCache.chainsaw2 = new WindowsMediaPlayer() { URL = "sounds//chainsaw2.mp3" }; SoundCache.chainsaw2.controls.stop();
            SoundCache.chainsaw3 = new WindowsMediaPlayer() { URL = "sounds//chainsaw3.mp3" }; SoundCache.chainsaw3.controls.stop();

            SoundCache.teleport1 = new WindowsMediaPlayer() { URL = "sounds//teleport1.mp3" }; SoundCache.teleport1.controls.stop();
            SoundCache.teleport2 = new WindowsMediaPlayer() { URL = "sounds//teleport2.mp3" }; SoundCache.teleport2.controls.stop();
            SoundCache.teleport3 = new WindowsMediaPlayer() { URL = "sounds//teleport3.mp3" }; SoundCache.teleport3.controls.stop();

            SoundCache.cosmicJump1 = new WindowsMediaPlayer() { URL = "sounds//cosmicJump1.mp3" }; SoundCache.cosmicJump1.controls.stop();
            SoundCache.cosmicJump2 = new WindowsMediaPlayer() { URL = "sounds//cosmicJump2.mp3" }; SoundCache.cosmicJump2.controls.stop();
            SoundCache.cosmicJump3 = new WindowsMediaPlayer() { URL = "sounds//cosmicJump3.mp3" }; SoundCache.cosmicJump3.controls.stop();

            SoundCache.boxingPunch1 = new WindowsMediaPlayer() { URL = "sounds//boxingPunch1.mp3" }; SoundCache.boxingPunch1.controls.stop();
            SoundCache.boxingPunch2 = new WindowsMediaPlayer() { URL = "sounds//boxingPunch2.mp3" }; SoundCache.boxingPunch2.controls.stop();
            SoundCache.boxingPunch3 = new WindowsMediaPlayer() { URL = "sounds//boxingPunch3.mp3" }; SoundCache.boxingPunch3.controls.stop();
            SoundCache.boxingPunch4 = new WindowsMediaPlayer() { URL = "sounds//boxingPunch4.mp3" }; SoundCache.boxingPunch4.controls.stop();

            SoundCache.ghostSwing1 = new WindowsMediaPlayer() { URL = "sounds//ghostSwing1.mp3" }; SoundCache.ghostSwing1.controls.stop();
            SoundCache.ghostSwing2 = new WindowsMediaPlayer() { URL = "sounds//ghostSwing2.mp3" }; SoundCache.ghostSwing2.controls.stop();

            SoundCache.ghostTeleport1 = new WindowsMediaPlayer() { URL = "sounds//ghostTeleport1.mp3" }; SoundCache.ghostTeleport1.controls.stop();
            SoundCache.ghostTeleport2 = new WindowsMediaPlayer() { URL = "sounds//ghostTeleport2.mp3" }; SoundCache.ghostTeleport2.controls.stop();

            SoundCache.sell = new WindowsMediaPlayer() { URL = "sounds//sell.mp3" }; SoundCache.sell.controls.stop();
            SoundCache.purchaseSound = new WindowsMediaPlayer() { URL = "sounds//purchaseSound.mp3" }; SoundCache.purchaseSound.controls.stop();
            SoundCache.reroll = new WindowsMediaPlayer() { URL = "sounds//reroll.mp3" }; SoundCache.reroll.controls.stop();

            SoundCache.gameVictory = new WindowsMediaPlayer() { URL = "sounds//gameVictory.mp3" }; SoundCache.gameVictory.controls.stop();
            SoundCache.gameLost = new WindowsMediaPlayer() { URL = "sounds//gameLost.mp3" }; SoundCache.gameLost.controls.stop();
            SoundCache.gameOver = new WindowsMediaPlayer() { URL = "sounds//gameOver.mp3" }; SoundCache.gameOver.controls.stop();


            //각 개체들의 사운드 리스트 정의
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

        private void ckbClient_CheckedChanged(object sender, EventArgs e)
        {
            txbIpAddress.Enabled = !ckbClient.Checked;
        }
    }
}
