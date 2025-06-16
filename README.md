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

namespace WindowsFormsApp23
{
    public partial class RpsClientForm : Form
    {
        private delegate void UpdateRankingCallback(string[] lines);

        TcpClient client;
        NetworkStream stream;

        public RpsClientForm()
        {
            InitializeComponent();
        }
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            { 
            client = new TcpClient();
            await client.ConnectAsync("127.0.0.1", 5000);
            stream = client.GetStream();

            Log("서버에 연결되었습니다.");

            string name = txtName.Text.Trim();
            byte[] nameBytes = Encoding.UTF8.GetBytes(name);
            await stream.WriteAsync(nameBytes, 0, nameBytes.Length);

            Log("서버에 연결됨.");

            _ = Task.Run(() => ReceiveLoop());
        } 
            catch (Exception ex)
    {
        Log($"서버에 연결 실패: {ex.Message}");
    }
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

        private void EnableInput()
        {
            btnSend.Enabled = true;
            txtMove.Enabled = true;
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string move = txtMove.Text.Trim();
            byte[] moveBytes = Encoding.UTF8.GetBytes(move);
            await stream.WriteAsync(moveBytes, 0, moveBytes.Length);

            btnSend.Enabled = false;
            txtMove.Enabled = false;
        }
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
    }
}
