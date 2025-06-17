using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities.miscellaneous
{
    public class GroundSoulExplosion : Explosion
    {
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.groundSoulExplosion1,
            Properties.Resources.groundSoulExplosion2,
            Properties.Resources.groundSoulExplosion3,
            Properties.Resources.groundSoulExplosion4,
            Properties.Resources.groundSoulExplosion5,
            Properties.Resources.groundSoulExplosion6,
            Properties.Resources.groundSoulExplosion7,
            Properties.Resources.groundSoulExplosion8,
            Properties.Resources.groundSoulExplosion9,
        };
        public LivingEntity Owner;
        public float attackDamage;
        public GroundSoulExplosion(GamePanel level) : base(level)
        {
            expImages = images;
            visualSize = 1.5f;
            this.width = 192;
            this.height = 96;
            Image = images[0];
        }
    }
}
