using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ApplicationSoftwareProjectTeam2.network;

namespace ApplicationSoftwareProjectTeam2.network //TCP 수신 + 저장
{
    public class Client
    {
        private string playerId;
        private string host;
        private int port;
        public bool isReady;

        public Client(string playerId, string host = "127.0.0.1", int port = 5555)
        {
            this.playerId = playerId;
            this.host = host;
            this.port = port;
        }

        private void Send(object payload)
        {
            using TcpClient client = new TcpClient(host, port);
            using NetworkStream stream = client.GetStream();
            string json = JsonSerializer.Serialize(payload);
            byte[] data = Encoding.UTF8.GetBytes(json);
            stream.Write(data, 0, data.Length);
        }
        public void SendLeave()
        {
            var leave = new { type = "PlayerLeave", playerId };
            Send(leave);
        }

        public void SendPing()
        {
            var ping = new { type = "Ping", playerId };
            Send(ping);
        }

        public void SendEmote(string emote)
        {
            var emo = new { type = "Emote", playerId, emote };
            Send(emo);
        }

        public void SendEntities(List<SerializedEntity> entities)
        {
            var pack = new
            {
                type = "EntitiesUpdate",
                playerId,
                entities
            };
            Send(pack);
        }

        public async Task<(List<SerializedEntity> entities, ulong seed)> RequestOpponentEntities() // 수정된 반환 형식
        {
            using var client = new TcpClient(host, port);
            using var stream = client.GetStream();

            // 요청
            var reqJson = JsonSerializer.Serialize(new
            {
                type = "RequestEntities",
                playerId = this.playerId
            });
            var reqBuf = Encoding.UTF8.GetBytes(reqJson);
            await stream.WriteAsync(reqBuf, 0, reqBuf.Length);

            // 응답
            var buf = new byte[4096];
            int n = await stream.ReadAsync(buf, 0, buf.Length);
            if (n == 0)
                return (new List<SerializedEntity>(), 0UL);

            var resp = Encoding.UTF8.GetString(buf, 0, n);
            using var doc = JsonDocument.Parse(resp);

            var seed = doc.RootElement.GetProperty("seed").GetUInt64();
            var ents = JsonSerializer.Deserialize<List<SerializedEntity>>(
                doc.RootElement.GetProperty("entities").GetRawText()
            );

            return (ents ?? new(), seed);
        }
    }
}
