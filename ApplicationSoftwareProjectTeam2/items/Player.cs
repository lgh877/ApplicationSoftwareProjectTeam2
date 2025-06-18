using ApplicationSoftwareProjectTeam2.entities;
<<<<<<< HEAD
using System.Net.Sockets;
=======
>>>>>>> 5862acffa1a5bd0a8da6360d73e7a68e086c31f6

namespace ApplicationSoftwareProjectTeam2.items
{
    public class Player
    {
<<<<<<< HEAD
        public string Name;
        public TcpClient Client;
        public NetworkStream Stream;
        public int Score = 0; // 추가
        public readonly object StreamLock = new object(); // 추가

=======
>>>>>>> 5862acffa1a5bd0a8da6360d73e7a68e086c31f6
        public LinkedList<LivingEntity> entitiesofplayer = new LinkedList<LivingEntity>();
        public string playerName { get; set; } = "Player1";
        public int Gold = 250;

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