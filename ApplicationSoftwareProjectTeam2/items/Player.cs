using ApplicationSoftwareProjectTeam2.entities;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class Player
    {
        public LinkedList<LivingEntity> entitiesofplayer = new LinkedList<LivingEntity>();
        public string playerName, actualPlayerName;
        public int Gold = 250;
    }
}