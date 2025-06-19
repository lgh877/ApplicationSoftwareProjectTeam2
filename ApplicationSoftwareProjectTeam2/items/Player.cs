using ApplicationSoftwareProjectTeam2.entities;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class Player
    {
        public List<LivingEntity?> entitiesofplayer = new List<LivingEntity?>();
        public string playerName, actualPlayerName;
        public int lifeLeft = 4, Gold;
    }
}