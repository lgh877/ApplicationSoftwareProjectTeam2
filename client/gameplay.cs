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

namespace client
{
    public partial class gameplay : Form
    {
        private delegate void UpdateRankingCallback(string[] lines);

        private TcpClient client;
        private NetworkStream stream;

        public gameplay(TcpClient client, NetworkStream stream)
        {
            InitializeComponent();
            this.client = client;
            this.stream = stream;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _ = Task.Run(() => ReceiveLoop());
        }

        private async Task ReceiveLoop()
        {
            byte[] buffer = new byte[256];
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                int len = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (len == 0) break;
                sb.Append(Encoding.UTF8.GetString(buffer, 0, len));

                string content = sb.ToString();
                string[] lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                // FINAL_RANK 처리
                if (lines.Length > 0 && lines[0].StartsWith("FINAL_RANK"))
                {
                    // 순위 메시지 조립
                    string rankMsg = string.Join(Environment.NewLine, lines.Skip(1));
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, rankMsg, "최종 순위", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }));
                    break; // 루프 종료
                }

                int rankStart = Array.FindIndex(lines, l => l.StartsWith("RANK_UPDATE"));
                if (rankStart != -1)
                {
                    // RANK_UPDATE 블록 추출
                    var rankLines = new List<string>();
                    for (int i = rankStart + 1; i < lines.Length; i++)
                    {
                        var trimmed = lines[i].Trim();
                        // "등:"으로 시작하는 줄만 순위로 간주
                        if (trimmed.StartsWith("1등:") || trimmed.StartsWith("2등:") || trimmed.StartsWith("3등:") || trimmed.StartsWith("4등:"))
                            rankLines.Add(lines[i]);
                        // 또는 정규식 사용 가능: if (Regex.IsMatch(trimmed, @"^\d+등:"))
                    }
                    Invoke(new UpdateRankingCallback(UpdateRanking), new object[] { rankLines.ToArray() });

                    // RANK_UPDATE 이후의 메시지 처리
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i == rankStart || (i > rankStart && i <= rankStart + rankLines.Count)) continue;
                        if (!string.IsNullOrWhiteSpace(lines[i]))
                        {
                            Log(lines[i]);
                            if (lines[i].Contains("입력"))
                                Invoke(new Action(() => EnableInput()));
                        }
                    }
                    sb.Clear();
                }
                else
                {
                    // RANK_UPDATE가 없으면 일반 메시지로 처리
                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            Log(line);
                            if (line.Contains("입력"))
                                Invoke(new Action(() => EnableInput()));
                        }
                    }
                    sb.Clear();
                }
            }
        }

        // EnableInput, btnSend_Click, UpdateRanking 등도 이곳에 구현
        private void UpdateRanking(string[] lines)
        {
            listViewRank.Items.Clear();

            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2 && parts[0].Trim().EndsWith("등"))
                {
                    string rank = parts[0].Trim();         // "1등"
                    string playerInfo = parts[1].Trim();   // "q1 (5점)"

                    ListViewItem item = new ListViewItem(rank);
                    item.SubItems.Add(playerInfo);
                    listViewRank.Items.Add(item);
                }
            }
        }

        private void Log(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => listBoxLog.Items.Add(msg)));
            }
            else
            {
                listBoxLog.Items.Add(msg);
            }
        }
        // 필드 추가
        private int timeLeft = 30;
        //   private System.Windows.Forms.Timer imer1 = new System.Windows.Forms.Timer();

        private void EnableInput()
        {
            btnSend.Enabled = true;
            txtMove.Enabled = true;
            timeLeft = 30;
            lblTimer.Text = $"남은 시간: {timeLeft}초";
            imer1.Interval = 1000;
            imer1.Tick -= Timer1_Tick; // 중복 방지
            imer1.Tick += Timer1_Tick;
            imer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTimer.Text = $"남은 시간: {timeLeft}초";
            if (timeLeft <= 0)
            {
                imer1.Stop();
                if (string.IsNullOrWhiteSpace(txtMove.Text))
                    txtMove.Text = "가위";
                btnSend_Click(btnSend, EventArgs.Empty); // 직접 호출
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            imer1.Stop(); // 타이머 중지
            string move = txtMove.Text.Trim();
            byte[] moveBytes = Encoding.UTF8.GetBytes(move);
            await stream.WriteAsync(moveBytes, 0, moveBytes.Length);
            btnSend.Enabled = false;
            txtMove.Enabled = false;
            lblTimer.Text = ""; // 타이머 표시 지움
        }
    }

}
