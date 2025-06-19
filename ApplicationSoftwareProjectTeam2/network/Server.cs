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
    public class Server 
    {
        private TcpListener listener;
        private bool running = false;

        private List<string> playerIDs = new List<string>();
        private Dictionary<string, List<SerializedEntity>> entitiesByPlayer = new();

        public void Start(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            running = true;

            Console.WriteLine($"[서버] 포트 {port}에서 대기 중...");

            Thread acceptThread = new Thread(() =>
            {
                while (running)
                {
                    var client = listener.AcceptTcpClient();
                    new Thread(() => HandleClient(client)).Start();
                }
            });
            acceptThread.Start();
        }

        private void HandleClient(TcpClient client)
        {
            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (client.Connected)
            {
                int byteCount = stream.Read(buffer, 0, buffer.Length);
                if (byteCount == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, byteCount);
                var doc = JsonDocument.Parse(message);
                string type = doc.RootElement.GetProperty("type").GetString();

                switch (type)
                {
                    case "Ping":
                        Console.WriteLine($"[Ping] From: {doc.RootElement.GetProperty("playerId")}");
                        break;

                    case "Emote":
                        Console.WriteLine($"[Emote] {doc.RootElement.GetProperty("playerId")} → {doc.RootElement.GetProperty("emote")}");
                        break;

                    case "EntitiesUpdate":
                        string playerId = doc.RootElement.GetProperty("playerId").GetString();
                        var entities = JsonSerializer.Deserialize<List<SerializedEntity>>(doc.RootElement.GetProperty("entities").ToString());

                        if (!entitiesByPlayer.ContainsKey(playerId))
                            entitiesByPlayer[playerId] = new List<SerializedEntity>();

                        entitiesByPlayer[playerId] = entities;
                        Console.WriteLine($"[Entity] {playerId}의 유닛 {entities.Count}개 저장됨");
                        break;
                }
            }
        }

        public List<string> GetPlayerIDs() => playerIDs;

        public void AddTestPlayers()
        {
            playerIDs.Add("Player1");
            playerIDs.Add("Player2");
        }

        public List<(string, string)> MakeMatches()
        {
            var matches = new List<(string, string)>();
            for (int i = 0; i + 1 < playerIDs.Count; i += 2)
                matches.Add((playerIDs[i], playerIDs[i + 1]));
            return matches;
        }
    }
}
