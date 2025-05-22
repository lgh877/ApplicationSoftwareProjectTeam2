using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{    
    public class LivingEntity : Entity
    {
        public int currentHealth, attackDamage, deathTime = 0, maxDeathTime = 30, moveSpeed;
        public Direction direction = Direction.Right;
        public bool hadTarget, isMoving, isActuallyMoving;
        public LivingEntity? target;
        public LivingEntity(GamePanel level) : base(level) 
        {
            hadTarget = false;
        }

        public override void tick()
        {
            base.tick();
            deltaMovement = deltaMovement * 0.7f;
            if (isAlive())
            {
                checkCollisionsLiving();
                /*
                if (getTarget() == null)
                {
                    if (hadTarget)
                    {
                        hadTarget = false;
                    }
                    float a, b;
                    a = level.getRandomInteger(11) - 5;
                    b = level.getRandomInteger(11) - 5;
                    push(a, 0, b);
                    if (tickCount % 50 == 0)
                    {
                        LivingEntity foundTarget = detectTargetManhattan(200);
                        if (foundTarget != null)
                        {
                            Image = Properties.Resources._4;
                            target = foundTarget;
                            hadTarget = true;
                        }
                    }
                }
                else
                {
                    Vector3 direction = Vector3.Normalize(new Vector3(target.x - x, 0, target.z - z));
                    push(direction.X * 2, 0, direction.Z * 2);
                    if (tickCount % 5 == 0 && level.getRandomInteger(5) == 0
                        && (width + target.width) * 0.5 + 50 > Math.Abs(target.x - x) + Math.Abs(target.z - z)
                        && height > target.y - y
                        && target.height > y - target.y)
                    {
                        doHurtTarget(target);
                    }
                }
                */
            }
            else
            {
                tickDeath();
            }
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


        public override bool hurt(LivingEntity attacker, int damage)
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
            foreach (var item in level.getAllLivingEntities<LivingEntity?>())
            {
                if (!item.Equals(this) 
                    && item.isAlive()
                    && !item.team.Equals(this.team) 
                    && range > Math.Abs(item.x - x) + Math.Abs(item.z - z))
                {
                    return item;
                }
            };
            return null;
        }
    }
}
