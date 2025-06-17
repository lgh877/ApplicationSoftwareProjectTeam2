using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities.miscellaneous
{
    public class GroundExplosion : Explosion
    {
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.groundExplosion1,
            Properties.Resources.groundExplosion2,
            Properties.Resources.groundExplosion3,
            Properties.Resources.groundExplosion4,
            Properties.Resources.groundExplosion5,
            Properties.Resources.groundExplosion6,
            Properties.Resources.groundExplosion7,
            Properties.Resources.groundExplosion8,
            Properties.Resources.groundExplosion9,
        };
        public LivingEntity Owner;
        public float attackDamage;
        public GroundExplosion(GamePanel level) : base(level)
        {
            expImages = images;
            this.width = 256;
            Image = images[0];
        }
    }
}
