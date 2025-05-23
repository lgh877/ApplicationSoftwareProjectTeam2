using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class ProjectileEntity : Entity
    {
        public LivingEntity Owner;
        public int damage;
        public ProjectileEntity(GamePanel level) : base(level)
        {
            Image = Properties.Resources._2;
            damage = 100;
            visualSize = 10;
            width = 10; height = 10; pushPower = 60;
        }
        public override void tick()
        {
            base.tick();
            checkCollisionsLiving();
            if (tickCount > 100) shouldRemove = true;
        }
        public override void applyCollisionLiving(LivingEntity entity)
        {
            if (entity.team.Equals(team)) return;
            base.applyCollisionLiving(entity);
            entity.hurt(Owner != null ? Owner : null, damage);
            shouldRemove = true;
        }
    }
}
