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
    public enum EntityTypes
    {
        Nothing = 0,
        Weirdos,
        Skeletons,
        Avengers
    }
    public class LivingEntity : Entity
    {
        public int deathTime = 0, maxDeathTime = 30, moveSpeed, entityState = 0;
        public float attackDamage, currentHealth;
        public Direction direction = Direction.Right;
        public bool hadTarget, isMoving, isActuallyMoving;
        public LivingEntity? target;
        public List<Item> EquippedItems = new List<Item>(3);
        public LivingEntity(GamePanel level) : base(level) 
        {
            hadTarget = false;
        }
        public virtual EntityTypes getEntityType()
        {
            return EntityTypes.Nothing;
        }
        
        public void move(int speed)
        {
            switch (direction)
            {
                case Direction.UpperRight:
                    push(speed * 0.2588f, 0, speed * 0.9659f);
                    break;
                case Direction.UpRight:
                    push(speed * 0.7071f, 0, speed * 0.7071f);
                    break;
                case Direction.Right:
                    push(speed, 0, 0);
                    break;
                case Direction.DownRight:
                    push(speed * 0.7071f, 0, -speed * 0.7071f);
                    break;
                case Direction.LowerRight:
                    push(speed * 0.2588f, 0, -speed * 0.9659f);
                    break;
                case Direction.UpperLeft:
                    push(-speed * 0.2588f, 0, speed * 0.9659f);
                    break;
                case Direction.UpLeft:
                    push(-speed * 0.7071f, 0, speed * 0.7071f);
                    break;
                case Direction.Left:
                    push(-speed, 0, 0);
                    break;
                case Direction.DownLeft:
                    push(-speed * 0.7071f, 0, -speed * 0.7071f);
                    break;
                case Direction.LowerLeft:
                    push(-speed * 0.2588f, 0, -speed * 0.9659f);
                    break;
            }
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
            float powerFactor = Math.Max(0, pushPower - entity.weight);
            entity.push(direction.X * powerFactor, direction.Y * powerFactor, direction.Z * powerFactor);
            return entity.hurt(this, attackDamage);
        }
        public override bool doHurtTarget(LivingEntity entity, float damage)
        {
            Vector3 direction = Vector3.Normalize(new Vector3(entity.x - x, entity.y - y, entity.z - z));
            float powerFactor = Math.Max(0, pushPower - entity.weight);
            entity.push(direction.X * powerFactor, direction.Y * powerFactor, direction.Z * powerFactor);
            return entity.hurt(this, damage);
        }
        public virtual LivingEntity? getTarget()
        {
            if (target != null && (!target.isAlive() || !target.hasAi)) target = null;
            return target;
        }


        public override bool hurt(LivingEntity? attacker, float damage)
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
        public override void releaseFromMouse()
        {
            //마우스에서 놓았을 때 z값이 200보다 낮다면 해당 객체를 level의 livingentities에서 entities 리스트로 옮기고 hasAi를 fasle로 해주세요
            if (hasAi && z < 200)
            {
                level.addFreshEntity(this);
                level.livingentities.Remove(this);
                hasAi = false;
            }
            else if(!hasAi && z >= 200)
            {
                //z값이 200보다 높다면 해당 객체를 level의 livingentities에서 entities 리스트로 옮기고 hasAi를 true로 해주세요
                level.addFreshLivingEntity(this);
                level.entities.Remove(this);
                hasAi = true;
            }
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
