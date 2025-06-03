using ApplicationSoftwareProjectTeam2.entities;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class Player
    {
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