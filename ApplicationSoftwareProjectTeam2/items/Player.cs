using ApplicationSoftwareProjectTeam2.entities;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class Player
    {
        public List<LivingEntity> entitiesofplayer { get; set; }
        public string playerName { get; set; } = "Player1";
        public int Gold { get; set; } = 15;

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