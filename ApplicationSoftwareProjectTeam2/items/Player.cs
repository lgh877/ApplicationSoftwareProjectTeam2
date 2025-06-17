using ApplicationSoftwareProjectTeam2.entities;
using System.Net.Sockets;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class Player
    {
        public string Name;
        public TcpClient Client;
        public NetworkStream Stream;
        public int Score = 0; // 추가
        public readonly object StreamLock = new object(); // 추가

        public LinkedList<LivingEntity> entitiesofplayer = new LinkedList<LivingEntity>();
        public string playerName { get; set; } = "Player1";
        public int Gold = 25;

        public bool PurchaseItem(Item item, Unit unit)
        {
            if (Gold < item.Price)
                return false;

            if (unit.EquipItem(item))
            {
                Gold -= item.Price;
                return true;
            }

            return false;
        }
    }
}