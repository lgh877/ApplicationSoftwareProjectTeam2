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
        public int maxLifeTime;
        public float attackDamage;
        public bool canDamage;
        public ProjectileEntity(GamePanel level) : base(level)
        {
            canDamage = true;
        }
        public override void tick()
        {
            base.tick();
            checkCollisionsLiving();
            if (tickCount > maxLifeTime) shouldRemove = true;
        }
        public override void checkCollisionsLiving()
        {
            if (!canDamage) return;
            base.checkCollisionsLiving();
        }
        public override void applyCollisionLiving(LivingEntity entity)
        {
            if (entity.team.Equals(team)) return;
            //base.applyCollisionLiving(entity);
            collisionOccurred(entity);
        }
        public virtual void collisionOccurred(LivingEntity victim)
        {
            victim.hurt(Owner != null ? Owner : null, attackDamage);
        }
    }
}
