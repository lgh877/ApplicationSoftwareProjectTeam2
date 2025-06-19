using System;
using System.Collections.Generic;
using System.Numerics;
using ApplicationSoftwareProjectTeam2.entities.miscellaneous;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.projectiles
{
    public class MushroomSpore : ProjectileEntity
    {

        public MushroomSpore(GamePanel level) : base(level)
        {
            elasticForce = -1f;
            gravity = 0.1f;
            airFraction = 0.01f;
            groundFraction = 0.3f;
            this.maxLifeTime = 50;
            this.weight = 1;
            this.visualSize = 1.5f;
            width = 14;
            height = 14;
            Image = Properties.Resources.mushroom_projectile;
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
            discard();
        }
    }
}
