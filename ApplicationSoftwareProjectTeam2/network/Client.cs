using System;
using System.Collections.Generic;
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
    }
}
