<<<<<<< HEAD
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
=======
상점 시스템 설계
1. 개요 (Overview)
본 상점 시스템은 실시간 오토배틀러 게임의 전투 전 단계에서 유닛의 성능을 전략적으로 강화할 수 있도록 설계되었다. 플레이어는 각 라운드 시작 전 골드를 활용하여 아이템을 구매하고 유닛에게 장착함으로써, 전투 성능에 영향을 줄 수 있다. 기존 롤토체스 시스템에 존재하지 않는 직접 골드로 아이템을 구매하는 상점이라는 차별화된 구조로, 전략성과 게임 반복성, 재미 요소를 증대시킨다.
2. 시스템 설계 (Architecture)
2.1 핵심 요소
- 아이템(Item): 유닛의 특정 능력치를 강화하거나 효과를 부여
- 골드(Gold): 게임 내 화폐, 전투 성과 또는 시간 경과에 따라 지급
- 장착 시스템: 유닛당 장착 가능한 아이템 수는 n개로 제한
- 효과 적용: 실시간 전투 중 지속/조건 기반으로 자동 발동
2.2 주요 클래스 구조

docx파일에 있음.

3. WinForms 기반 UI 구성
3.1 기본 구성 요소
- 아이템 목록 패널: PictureBox + Label + Tooltip 구조로 시각적 표현
- 아이템 상세 정보창: 클릭 또는 마우스오버 시 효과 설명 표시
- 장착 버튼: 아이템 선택 후 유닛을 클릭하면 장착
- 골드 표시창: 현재 소지 골드 시각화
- 잠금/회색 처리: 구매 불가 아이템은 비활성화 표시
3.2 예시 화면 배치
[ 골드: 15G ]

[아이템1] [아이템2] [아이템3]
[아이템4] [아이템5] [아이템6]
[아이템7] [아이템8] [아이템9]

[설명창: 효과 상세 내용]
4. 아이템 목록 (예시 13종)
이름	사용 대상	가격	효과 요약
철갑 피부	탱커	5G	방어력 +n, 받는 피해 -n%
전방 방패	탱커	6G	전방 피해 감소
역반사 장치	탱커	6G	피해 반사
마력 방해장	탱커	6G	주변 적 스킬속도 감소
강화 심장	공용	5G	최대 체력 +n, 재생속도 증가
관통 화살	원딜	5G	방어력 무시 +n%
속사 장치	원딜	6G	공격속도 +n%
명중 향상 모듈	원딜	5G	명중률 및 치명타 확률 증가
연소 탄환	원딜	6G	화염 지속 데미지
혼란 광선기	특수	6G	적 명중률/스킬확률 감소
에너지 확산기	특수	5G	범위 증가, 쿨타임 감소
사이오닉 점멸기	특수	7G	체력 낮을 때 자동 회피
다중 연계장치	특수	6G	스킬 효과 인접 아군 공유

5. 아이템 디자인
아이템은 카드 형태로 구성되며, 한 눈에 정보를 파악할 수 있도록 시각적 구성을 단순화한다. 각 아이템은 다음과 같은 요소로 구성된다:
• 아이콘 (PictureBox): 아이템 유형을 상징하는 32x32 또는 64x64 크기의 이미지
(예시: 🛡️ 강화 심장)
• 이름 (Label): 아이템의 이름, 강조(Bold) 처리
• 가격 (Label): 골드 수치, 금색 또는 주황색 글씨로 표시
• 유형 (Label): 탱커 / 원딜 / 특수 / 공용, 색상으로 구분
• 효과 설명 (Label): 아이템 효과 요약
• 구매 버튼 (Button): 해당 아이템을 구매하여 유닛에 장착

6. 확장 가능성
확실하진 않지만 필요에 따라서 다음 항목들을 추가할 수 있다.
- 아이템 합성 시스템: 두 아이템 조합 → 고급 아이템 생성
- 유닛별 추천 아이템 표시: 초보자 UX 개선
- 판매 기능: 구매한 아이템 일부 환급 가능

>>>>>>> 5862acffa1a5bd0a8da6360d73e7a68e086c31f6
