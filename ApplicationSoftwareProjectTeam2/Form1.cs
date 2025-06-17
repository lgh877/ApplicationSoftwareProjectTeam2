using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationSoftwareProjectTeam2
{

    public partial class Form1 : Form
    {


        List<items.Player> players = new List<items.Player>();
        TcpListener listener;

        public Form1()
        {
            InitializeComponent();
        }
        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();
            Log("서버 시작됨. 클라이언트 접속 대기 중...");

            players.Clear();

            //초기 아이디 입력으로 서버에 클라이언트들 연결후 player클래스로 정보 가져옴
            while (players.Count < 4)
            {
                var client = await listener.AcceptTcpClientAsync();
                var stream = client.GetStream();

                byte[] buffer = new byte[256];
                int len = await stream.ReadAsync(buffer, 0, buffer.Length);
                string name = Encoding.UTF8.GetString(buffer, 0, len);

                players.Add(new items.Player { Name = name, Client = client, Stream = stream });
                Log($"{name} 접속 완료 ({players.Count}/4)");
            }
            foreach (var player in players)
            {
                byte[] startMsg = Encoding.UTF8.GetBytes("GAME_START");
                player.Stream.Write(startMsg, 0, startMsg.Length);
                player.Stream.Flush();
            }
            Log("4명 접속 완료. 게임 시작.");

            //5회 경기 시작 handlematch메서드를 gamelpanel과 연계시키면 됨.
            for (int i = 0; i < 2; i++)
            {
                if (i % 3 == 0)
                {
                    Task t1 = HandleMatch(players[0], players[1]);
                    Task t2 = HandleMatch(players[2], players[3]);
                    await Task.WhenAll(t1, t2);
                    SendScoresToAll();
                }
                else if (i % 3 == 1)
                {
                    Task t1 = HandleMatch(players[0], players[2]);
                    Task t2 = HandleMatch(players[1], players[3]);
                    await Task.WhenAll(t1, t2);
                    SendScoresToAll();
                }
                else if (i % 3 == 2)
                {
                    Task t1 = HandleMatch(players[0], players[3]);
                    Task t2 = HandleMatch(players[1], players[2]);
                    await Task.WhenAll(t1, t2);
                    SendScoresToAll();
                }
            }
            Log("모든 게임 완료.");
            await Task.Delay(500); // 0.5초 대기 (네트워크 안정화)
            SendFinalRankToAll();
        }

        private async Task HandleMatch(items.Player p1, items.Player p2)
        {
            Send(p1, $"당신은 {p2.Name}과 가위바위보를 합니다. (가위/바위/보 입력): ");
            Send(p2, $"당신은 {p1.Name}과 가위바위보를 합니다. (가위/바위/보 입력): ");

            // 비동기로 동시에 입력 받기, 플레이어의 정보를 입렵받는 부분.
            var t1 = ReceiveAsync(p1);
            var t2 = ReceiveAsync(p2);
            await Task.WhenAll(t1, t2);
            string m1 = t1.Result;
            string m2 = t2.Result;
            //judge 메서드로 승패를 판단하고 결과를 클라이언트에게 전달, gamepanel과 연계시켜서 결과를 보여주면 됨.
            string r1 = Judge(m1, m2);
            string r2 = Invert(r1);

            if (r1 == "승리") p1.Score += 1;
            else if (r1 == "패배") p2.Score += 1;

            Send(p1, $"결과: {m1} vs {m2} → 당신은 {r1}");
            Send(p2, $"결과: {m2} vs {m1} → 당신은 {r2}");
        }

        private async Task<string> ReceiveAsync(items.Player p)
        {
            byte[] buffer = new byte[256];
            int len;
            // NetworkStream.ReadAsync는 lock이 필요 없습니다.
            len = await p.Stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, len).Trim();
        }
        private void Send(items.Player p, string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            lock (p.StreamLock)
            {
                p.Stream.Write(data, 0, data.Length);
                p.Stream.Flush(); // 추가 (대부분 필요 없지만, 혹시 모를 경우)
            }

        }

        private string Receive(items.Player p)
        {
            byte[] buffer = new byte[256];
            int len;
            lock (p.StreamLock)
            {
                len = p.Stream.Read(buffer, 0, buffer.Length);
            }
            return Encoding.UTF8.GetString(buffer, 0, len).Trim();
        }

        private string Judge(string m1, string m2)
        {
            if (m1 == m2) return "무승부";
            if ((m1 == "가위" && m2 == "보") ||
                (m1 == "바위" && m2 == "가위") ||
                (m1 == "보" && m2 == "바위")) return "승리";
            return "패배";
        }

        private string Invert(string r) => r == "승리" ? "패배" : r == "패배" ? "승리" : "무승부";

        private void Log(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() =>
                    txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}")
                ));
            }
            else
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            }
        }
        private void SendScoresToAll()
        {
            foreach (var k in players)
            {
                var ranked = players.OrderByDescending(p => p.Score).ToList();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("RANK_UPDATE");
                for (int i = 0; i < ranked.Count; i++)
                {
                    sb.AppendLine($"{i + 1}등: {ranked[i].Name} - {ranked[i].Score}점");
                }

                string rankMsg = sb.ToString();

                Send(k, rankMsg);
            }
        }
        private void SendFinalRankToAll()
        {
            var ranked = players.OrderByDescending(p => p.Score).ToList();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FINAL_RANK");
            for (int i = 0; i < ranked.Count; i++)
            {
                sb.AppendLine($"{i + 1}등: {ranked[i].Name} - {ranked[i].Score}점");
            }
            string finalRankMsg = sb.ToString();

            foreach (var k in players)
            {
                Send(k, finalRankMsg);
            }
        }
    }
}
