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
        public event EventHandler deathEvent;
        public byte entityLevel = 0;
        public int deathTime = 0, maxDeathTime = 30, moveSpeed, entityState = 0, deckIndex;
        public float attackDamage, currentHealth, maxHealth;
        public Direction direction = Direction.Right;
        public bool hadTarget, isMoving, isActuallyMoving, hasLife;
        public LivingEntity? target;
        public List<Item> EquippedItems = new List<Item>(3);
        public (int, int) deckPosition = (0, 0); // (x, z) 좌표로 표현되는 덱 위치
        public LivingEntity(GamePanel level) : base(level) 
        {
            grabbedEvent += resetDeck;
            deathEvent += setDeath;
            deckIndex = -1;
            hadTarget = false;
            hasLife = true;
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
            if (!hasAi && isOnGround() && deckIndex != -1)
            {
                if (Math.Abs(x - deckPosition.Item1) + Math.Abs(z - deckPosition.Item2) > 3)
                {
                    int dir = (int)direction;
                    Vector3 targetVec = Vector3.Normalize(new Vector3(deckPosition.Item1 - x, 0, deckPosition.Item2 - z));

                    if (targetVec.X > 0)
                    {
                        if (targetVec.Z > 0.9659) dir = 0;
                        else if (targetVec.Z > 0.7071) dir = 1;
                        else if (targetVec.Z > -0.7071) dir = 2;
                        else if (targetVec.Z > -0.9659) dir = 3;
                        else dir = 4;
                    }
                    else
                    {
                        if (targetVec.Z > 0.9659) dir = 9;
                        else if (targetVec.Z > 0.7071) dir = 8;
                        else if (targetVec.Z > -0.7071) dir = 7;
                        else if (targetVec.Z > -0.9659) dir = 6;
                        else dir = 5;
                    }
                    direction = (Direction)dir;
                    move(moveSpeed);
                }
                else
                {
                    deltaMovement = Vector3.Zero;
                    direction = Direction.Right;
                }
            }
            checkCollisionsLiving();
        }
        public byte getLivingEntityId()
        {
            return 0;
        }
        public virtual void detectLivingEntityAndMerge(Object? sender, EventArgs e)
        {
            foreach (var item in level.getAllEntities<LivingEntity>())
            {
                if (!item.Equals(this) && getLivingEntityId() == item.getLivingEntityId() && entityLevel == item.entityLevel
                    && 40 > Math.Abs(item.x - x) + Math.Abs(item.z - z))
                {
                    grabOccurred();
                    item.entityLevel++;
                    item.scaleEntity(1.2f);
                    level.entities.Remove(this);
                    break;
                }
            };
        }
        public void resetDeck(Object? sender, EventArgs e)
        {
            if (deckIndex != -1)
            {
                // 덱 위치를 해제
                level.valueTupleList[deckIndex] = level.valueTupleList[deckIndex] with { Item3 = false };
                deckIndex = -1; // 인덱스 초기화
            }
        }
        public virtual void setDeath(Object? sender, EventArgs e)
        {
        }
        public virtual void tickDeath()
        {
            if (hasLife)
            {
                hasLife = false;
                deathEvent?.Invoke(this, EventArgs.Empty);
            }
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
        public virtual bool findClosestDeckPosition()
        {
            float minDistance = int.MaxValue;
            int foundIndex = -1;
            for (int i = 0; i < level.valueTupleList.Count; i++)
            {
                var (pointx, pointz, isOccupied) = level.valueTupleList[i];
                if (isOccupied) continue; // false인 위치만 고려

                // 거리 계산 (제곱 연산을 생략하고 단순 절대값 차이 비교)
                float distance = Math.Abs(x - pointx) + Math.Abs(z - pointz);

                // 최소 거리 업데이트
                if (distance < minDistance)
                {
                    minDistance = distance;
                    foundIndex = i;
                }
            }
            if (foundIndex != -1)
            {
                deckPosition = (level.valueTupleList[foundIndex].Item1, level.valueTupleList[foundIndex].Item2);
                level.valueTupleList[foundIndex] = level.valueTupleList[foundIndex] with { Item3 = true };
                deckIndex = foundIndex; // 인덱스 저장
                return true; // 성공적으로 위치를 찾음
            }
            else
            {
                return false; // 빈 위치를 찾지 못함    
            }
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
            direction = Direction.Right;
            //마우스에서 놓았을 때 z값이 200보다 낮다면 해당 객체를 level의 livingentities에서 entities 리스트로 옮기고 hasAi를 fasle로 해주세요
            if (z < 200)
            {
                if (!findClosestDeckPosition()) return;
                if (hasAi)
                {
                    landedEvent += detectLivingEntityAndMerge;
                    level.addFreshEntity(this);
                    level.livingentities.Remove(this);
                    hasAi = false;
                }
            }
            else if(!hasAi && z >= 200)
            {
                landedEvent -= detectLivingEntityAndMerge;
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
