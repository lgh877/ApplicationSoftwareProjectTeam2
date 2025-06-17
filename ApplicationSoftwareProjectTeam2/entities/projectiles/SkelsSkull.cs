using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.entities.miscellaneous;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.projectiles
{
    public class SkelsSkull : ProjectileEntity
    {
        bool isRight = true;
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.skel_skull,
            Properties.Resources.skel_torso,
            ImageUtils.FlipImageHorizontally(Properties.Resources.skel_skull),
            ImageUtils.FlipImageHorizontally(Properties.Resources.skel_torso),
        };
        public SkelsSkull(GamePanel level) : base(level)
        {
            elasticForce = -0.5f;
            this.maxLifeTime = 100;
            this.canDamage = true;
            this.weight = 1;
            this.visualSize = 2f; width = 30; height = 26;
            this.hasGravity = true;
        }
        public override void landed()
        {
            base.landed();
            maxLifeTime -= 20;
            canDamage = false;
        }
        public override void tick()
        {
            base.tick();
        }
        public override void collisionOccurred(LivingEntity victim)
        {
            victim.hurt(Owner != null ? Owner : null, attackDamage);
            canDamage = false;
            if (victim.x == x && victim.y == y && victim.z == z)
            {
                return; // 충돌이 발생하지 않도록 동일한 위치에 있는 경우 무시
            }
            Vector3 direction = Vector3.Normalize(new Vector3(victim.x - x, victim.y - y, victim.z - z));
            float powerFactor = pushPower / victim.weight;
            victim.push(direction.X * 2 * powerFactor,
                direction.Y * 2 * powerFactor,
                direction.Z * 2 * powerFactor);
            deltaMovement *= -0.3f;
        }
    }
}
