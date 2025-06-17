using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities.miscellaneous
{
    public class GroundExplosion : Entity
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
            this.visualSize = 2.0f;
            this.width = 256;
            this.height = 128;
            this.weight = 1000;
            this.pushPower = 100;
            this.hasGravity = false;
            Image = images[0];
            renderType = 0; // no shadow
        }
        public override void tick()
        {
            base.tick();
            if (tickCount < 9)
            {
                Image = images[tickCount - 1];
            }
            else
            {
                shouldRemove = true;
            }
        }
        public override void applyCollisionLiving(LivingEntity entity)
        {
            if (entity.x == x && entity.y == y && entity.z == z)
            {
                return; // 충돌이 발생하지 않도록 동일한 위치에 있는 경우 무시
            }
            Vector3 direction = Vector3.Normalize(new Vector3(entity.x - x, entity.y - y, entity.z - z));
            float powerFactor = pushPower / entity.weight;
            entity.push(direction.X * 2 * powerFactor,
                direction.Y * 2 * powerFactor,
                direction.Z * 2 * powerFactor);
            if (!entity.team.Equals(team))
            {
                entity.hurt(Owner != null ? Owner : null, attackDamage);
            }
        }
    }
}
