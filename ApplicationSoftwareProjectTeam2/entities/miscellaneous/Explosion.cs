using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities.miscellaneous
{
    public class Explosion : Entity
    {
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.explode1,
            Properties.Resources.explode2,
            Properties.Resources.explode3,
            Properties.Resources.explode4,
            Properties.Resources.explode5,
            Properties.Resources.explode6,
            Properties.Resources.explode7,
        };
        public List<Image> expImages;
        public LivingEntity Owner;
        public float attackDamage;
        public Explosion(GamePanel level) : base(level)
        {
            this.visualSize = 2.0f;
            expImages = images;
            this.width = 128;
            this.height = 128;
            this.weight = 1000;
            this.pushPower = 100;
            this.hasGravity = false;
            Image = expImages[0];
            renderType = 0; // no shadow
        }
        public override void tick()
        {
            base.tick();
            if (tickCount < expImages.Count)
            {
                Image = expImages[tickCount - 1];
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
