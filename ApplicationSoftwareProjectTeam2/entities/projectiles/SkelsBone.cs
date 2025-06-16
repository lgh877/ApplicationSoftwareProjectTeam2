using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.projectiles
{
    public class SkelsBone : ProjectileEntity
    {
        int imageOffset = 0;
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.skel_projectile,
            ImageUtils.RotateImage(Properties.Resources.skel_projectile, 45),
            ImageUtils.RotateImage(Properties.Resources.skel_projectile, 90),
            ImageUtils.RotateImage(Properties.Resources.skel_projectile, 135),
        };
        public SkelsBone(GamePanel level) : base(level)
        {
            elasticForce = -0.5f;
            this.maxLifeTime = 100;
            this.canDamage = true;
            this.weight = 1;
            this.visualSize = 2f; width = 22; height = 22;
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
            if(canDamage)
                Image = images[(imageOffset + tickCount) % 4];
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
            float powerFactor = pushPower * 10 / victim.weight;
            victim.push(direction.X * 2 * powerFactor,
                direction.Y * 2 * powerFactor,
                direction.Z * 2 * powerFactor);
            deltaMovement *= -0.5f;
            //push(direction.X * -2 * powerFactor, direction.Y * -2 * powerFactor, direction.Z * -2 * powerFactor);
        }
    }
}
