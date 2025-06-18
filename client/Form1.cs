using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient client = new TcpClient();
                //내 컴퓨터(서버 컴퓨터)의 ip주소
                await client.ConnectAsync("172.100.7.197", 5000);
                NetworkStream stream = client.GetStream();

                string name = txtName.Text.Trim();
                byte[] nameBytes = Encoding.UTF8.GetBytes(name);
                await stream.WriteAsync(nameBytes, 0, nameBytes.Length);

                // 서버에서 "GAME_START" 메시지 수신 대기
                byte[] buffer = new byte[256];
                int len = await stream.ReadAsync(buffer, 0, buffer.Length);
                string msg = Encoding.UTF8.GetString(buffer, 0, len).Trim();

                if (msg == "GAME_START")
                {
                    this.Hide();
                    using (var gameForm = new gameplay(client, stream))
                    {
                        gameForm.ShowDialog();
                    }
                    this.Show();
                }
                else
                {
                    MessageBox.Show("서버로부터 게임 시작 신호를 받지 못했습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버 연결 실패: " + ex.Message);
            }
        }
    }
}
