using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using ApplicationSoftwareProjectTeam2.network;

namespace ApplicationSoftwareProjectTeam2.network //이벤트 전송 전용 클라이언트
{
    class MatchInfo
    {
        public string PlayerA, PlayerB;
        public ulong Seed;
    }

    public class Server
    {
        private TcpListener listener;
        private bool running = false;

        // 저장소
        private Dictionary<string, List<SerializedEntity>> entitiesByPlayer
            = new();
        private Queue<string> waitingQueue = new Queue<string>();
        private Dictionary<string, MatchInfo> matchesByPlayer
            = new Dictionary<string, MatchInfo>();

        private Random rnd = new Random();

        public void Start(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            running = true;
            Console.WriteLine($"[서버] 포트 {port} 대기 중...");

            new Thread(async () =>
            {
                while (running)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    _ = HandleClient(client);
                }
            })
            { IsBackground = true }
            .Start();
        }

        private async Task HandleClient(TcpClient client)
        {
            using var stream = client.GetStream();
            var buf = new byte[4096];

            while (client.Connected)
            {
                int read = await stream.ReadAsync(buf, 0, buf.Length);
                if (read == 0) break;

                var json = Encoding.UTF8.GetString(buf, 0, read);
                using var doc = JsonDocument.Parse(json);
                var type = doc.RootElement.GetProperty("type").GetString();
                var playerId = doc.RootElement.GetProperty("playerId").GetString();

                switch (type)
                {
                    case "EntitiesUpdate":
                        {
                            // 1) 덱 저장
                            var list = JsonSerializer
                                .Deserialize<List<SerializedEntity>>(
                                  doc.RootElement.GetProperty("entities")
                                     .GetRawText()
                                );
                            entitiesByPlayer[playerId] = list;

                            // 2) 대기열 중복 방지 후 enqueue
                            if (!waitingQueue.Contains(playerId)
                                && !matchesByPlayer.ContainsKey(playerId))
                            {
                                waitingQueue.Enqueue(playerId);
                            }

                            // 3) 큐에 2명 이상 모였으면 매칭
                            if (waitingQueue.Count >= 2)
                            {
                                var a = waitingQueue.Dequeue();
                                var b = waitingQueue.Dequeue();
                                // 한 쌍에 한 번만 seed 생성
                                var seed = NextUInt64(rnd);
                                var mi = new MatchInfo
                                {
                                    PlayerA = a,
                                    PlayerB = b,
                                    Seed = seed
                                };
                                matchesByPlayer[a] = mi;
                                matchesByPlayer[b] = mi;
                                Console.WriteLine(
                                  $"[매칭] {a} ↔ {b}, seed={seed}"
                                );
                            }
                        }
                        break;

                    case "RequestEntities":
                        {
                            var requesterId = doc.RootElement.GetProperty("playerId").GetString();

                            // (기존) 매칭정보 꺼내기
                            if (matchesByPlayer.TryGetValue(requesterId, out var match))
                            {
                                var opp = match.PlayerA == requesterId
                                              ? match.PlayerB
                                              : match.PlayerA;

                                // (기존) 상대 엔티티와 seed 내려주기
                                entitiesByPlayer.TryGetValue(opp, out var oppList);
                                await SendToClientAsync(stream, new
                                {
                                    type = "EntitiesUpdate",
                                    playerId = opp,
                                    entities = oppList ?? new List<SerializedEntity>(),
                                    seed = match.Seed
                                });

                                // ★ 매칭 기록 제거 ★
                                // → 다음 라운드에 다시 대기열에 올라가서 새로운 상대와 매칭
                                matchesByPlayer.Clear();
                            }
                            else
                            {
                                // 매칭 전 요청에 대해선 빈 응답
                                await SendToClientAsync(stream, new
                                {
                                    type = "EntitiesUpdate",
                                    playerId = (string)null,
                                    entities = new List<SerializedEntity>(),
                                    seed = 0UL
                                });
                            }
                        }
                        break;
                    case "PlayerLeave":
                        {
                            var pid = doc.RootElement.GetProperty("playerId").GetString();

                            // 1) 엔티티 캐시에서 제거
                            entitiesByPlayer.Remove(pid);

                            // 2) 대기열에서 제거
                            waitingQueue = new Queue<string>(
                                waitingQueue.Where(id => id != pid)
                            );

                            // 3) 매칭 정보에서 제거
                            if (matchesByPlayer.TryGetValue(pid, out var mi))
                            {
                                var other = mi.PlayerA == pid ? mi.PlayerB : mi.PlayerA;
                                matchesByPlayer.Remove(pid);
                                matchesByPlayer.Remove(other);
                            }

                            Console.WriteLine($"[Leave] {pid}님이 게임을 나갔습니다.");
                        }
                        break;
                }
            }
        }

        // ulong 난수 생성 헬퍼
        private static ulong NextUInt64(Random rng)
        {
            // 32비트 난수 2개로 64비트 합성
            var hi = (ulong)rng.Next(int.MinValue, int.MaxValue);
            var lo = (ulong)rng.Next(int.MinValue, int.MaxValue);
            return (hi << 32) | lo;
        }

        private async Task SendToClientAsync(
            NetworkStream stream, object payload)
        {
            var json = JsonSerializer.Serialize(payload);
            var bytes = Encoding.UTF8.GetBytes(json);
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }

}
