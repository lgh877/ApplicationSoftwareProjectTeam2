using System;
using System.Collections.Generic;
using System.Numerics;
using ApplicationSoftwareProjectTeam2.entities.miscellaneous;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.projectiles
{
    public class FlyingEyeShot : ProjectileEntity
    {
        public int imageOffset = 0;

        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.flyingeye_projectile,
            ImageUtils.RotateImage(Properties.Resources.flyingeye_projectile, 45),
            ImageUtils.RotateImage(Properties.Resources.flyingeye_projectile, 90),
            ImageUtils.RotateImage(Properties.Resources.flyingeye_projectile, 135),
        };

        public FlyingEyeShot(GamePanel level) : base(level)
        {
            elasticForce = -1f;
            groundFraction = 1f; airFraction = 1f;
            this.maxLifeTime = 50;
            this.canDamage = true;
            this.weight = 1;
            this.visualSize = 1f;
            width = 16;
            height = 16;
            this.hasGravity = false;
        }

        public override void tick()
        {
            if (canDamage)
                Image = images[(imageOffset + tickCount) % 4];
            if (tickCount % 4 == 0)
                if (tickCount < 40) scaleEntity(1.15f);
                else scaleEntity(0.7f);
            base.tick();
        }

        public override void collisionOccurred(LivingEntity victim)
        {
            victim.hurt(Owner != null ? Owner : null, attackDamage);
            if (victim.x == x && victim.y == y && victim.z == z)
                return;

            Vector3 direction = Vector3.Normalize(new Vector3(victim.x - x, victim.y - y, victim.z - z));
            float powerFactor = pushPower * 4 / victim.weight;

            victim.push(direction.X * 2 * powerFactor,
                        direction.Y * 2 * powerFactor,
                        direction.Z * 2 * powerFactor);
        }
    }
}
