using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class WeirdGuy : LivingEntity
    {
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.weirdGuy_idle1,
            Properties.Resources.weirdGuy_idle2,
            Properties.Resources.weirdGuy_walk1,
            Properties.Resources.weirdGuy_walk2,
            Properties.Resources.weirdGuy_died
        };
        public WeirdGuy(GamePanel level) : base(level)
        {
            currentHealth = 100;
            attackDamage = 50;
        }
    }
}
