using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.items;

namespace ApplicationSoftwareProjectTeam2.entities
{    
    public class LivingEntity : Entity
    {
        public int currentHealth, attackDamage, deathTime = 0, maxDeathTime = 30, moveSpeed, entityState = 0;
        public Direction direction = Direction.Right;
        public bool hadTarget, isMoving, isActuallyMoving;
        public LivingEntity? target;
        public List<Item> EquippedItems = new List<Item>(3);
        public LivingEntity(GamePanel level) : base(level) 
        {
            hadTarget = false;
        }

        public override void tick()
        {
            base.tick();
            if (isAlive())
            {
                tickAlive();
            }
            else
            {
                tickDeath();
            }
        }
        public virtual void tickAlive() 
        {
            checkCollisionsLiving();
        }
        public virtual void tickDeath()
        {
            if (++deathTime == maxDeathTime)
            {
                shouldRemove = true;
            }
        }

        public override bool doHurtTarget(LivingEntity entity)
        {
            Vector3 direction = Vector3.Normalize(new Vector3(entity.x - x, entity.y - y, entity.z - z));
            entity.push(direction.X * 30, direction.Y * 30, direction.Z * 30);
            push(direction.X * -10, direction.Y * -10, direction.Z * -10);
            return entity.hurt(this, attackDamage);
        }
        public virtual LivingEntity? getTarget()
        {
            if (target != null && !target.isAlive()) target = null;
            return target;
        }


        public override bool hurt(LivingEntity? attacker, int damage)
        {
            currentHealth -= damage;
            return true;
        }

        public override void applyCollisionLiving(LivingEntity entity)
        {
            base.applyCollisionLiving(entity);
        }
        public virtual bool isAlive()
        {
            return currentHealth > 0;
        }
        //맨해튼 거리 기반으로 주변에 있는 타겟을 찾는 메서드
        public virtual LivingEntity detectTargetManhattan(int range)
        {
            float dist = float.MaxValue, currentDist;
            LivingEntity found = null;
            foreach (var item in level.getAllLivingEntities<LivingEntity?>())
            {
                currentDist = Math.Abs(item.x - x) + Math.Abs(item.z - z);
                if (!item.Equals(this) 
                    && item.isAlive()
                    && !item.team.Equals(this.team) 
                    && range > currentDist
                    && currentDist < dist)
                {
                    dist = currentDist;
                    found = item;
                }
            };
            return found;
        }
    }
}
