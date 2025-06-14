using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities.projectiles
{
    public class ProjectileEntity : Entity
    {
        public LivingEntity Owner;
        public float damage;
        public bool canDamage;
        public ProjectileEntity(GamePanel level) : base(level)
        {
            //Image = Properties.Resources._2;
            canDamage = true;
            visualSize = 0.05f;
            width = 10; height = 10; weight = 30;
        }
        public override void landed()
        {
            base.landed();
            canDamage = false;
        }
        public override void tick()
        {
            base.tick();
            checkCollisionsLiving();
            if (tickCount > 100) shouldRemove = true;
        }
        public override void checkCollisionsLiving()
        {
            if (!canDamage) return;
            base.checkCollisionsLiving();
        }
        public override void applyCollisionLiving(LivingEntity entity)
        {
            if (entity.team.Equals(team)) return;
            base.applyCollisionLiving(entity);
            entity.hurt(Owner != null ? Owner : null, damage);
            //canDamage = false;
        }
    }
}
