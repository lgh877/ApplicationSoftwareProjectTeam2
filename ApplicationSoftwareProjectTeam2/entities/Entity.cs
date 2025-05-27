using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class Entity
    {
        public int tickCount, sharedFlags, width = 40, height = 40, pushPower = 2, weight = 1;
        public float x, y, z, visualSize;
        public Vector3 deltaMovement;
        public GamePanel level;
        public string team;
        public Image Image;
        public bool shouldRemove = false, hasGravity = true;


        public Entity(GamePanel level, float x, float y, float z, Vector3 vec3)
        {
            tickCount = 0;
            this.level = level;
            this.x = x;
            this.y = y;
            this.z = z;
            deltaMovement = vec3;
        }

        public Entity(GamePanel level) 
        {
            tickCount = 0;
            this.level = level;
            this.x = 0;
            this.y = 0;
            this.z = 0;
            deltaMovement = Vector3.Zero;
        }

        public virtual void setPosition(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public virtual void setPosition(float x, float z)
        {
            this.x = x;
            this.z = z;
        }
        public virtual void setPosition(float y)
        {
            this.y = y;
        }

        public virtual void moveTo(float x, float y, float z)
        {
            if (x < 500 && x > -500)
            {
                this.x = x;
            }
            else
            {
                this.x = x >= 500 ? 500 : -500;
                deltaMovement.X *= -1;
                collisionOccurred();
            }
            if (y>0)
            {
                this.y = y;
            }
            else
            {
                this.y = 0;
                deltaMovement.Y *= -0.5f;
                landed();
            }
            if (z < 700 && z > 200)
                this.z = z;
            else
            {
                this.z = z <= 200 ? 200 : 700;
                deltaMovement.Z *= -1;
                collisionOccurred();
            }
        }
        public virtual void moveTo(float x, float z)
        {
            if (x < 500 && x > -500)
            {
                this.x = x;
            }
            else
            {
                this.x = x >= 500 ? 500 : -500;
                deltaMovement.X *= -1;
                collisionOccurred();
            }
            if (z < 700 && z > 200)
                this.z = z;
            else
            {
                this.z = z <= 200 ? 200 : 700;
                deltaMovement.Z *= -1;
                collisionOccurred();
            }
        }
        public virtual void collisionOccurred()
        {
        }
        public virtual void landed()
        {
        }
        public virtual void setDeltaMovement(Vector3 vec)
        {
            this.deltaMovement = vec;
        }

        public virtual void setDeltaMovement(float x, float y, float z)
        {
            this.deltaMovement = new Vector3(x, y, z);
        }

        public virtual void push(Vector3 vec)
        {
            this.deltaMovement += vec;
        }

        public virtual void push(float x, float y, float z)
        {
            this.deltaMovement = new Vector3(this.deltaMovement.X + x
                , this.deltaMovement.Y + y
                , this.deltaMovement.Z + z);
        }

        public virtual void checkCollisionsLiving()
        {
            foreach (var item in level.getAllLivingEntities<LivingEntity>())
            {
                if (!item.Equals(this)
                    && (width + item.width) * 0.5 > Math.Abs(item.x - x) + Math.Abs(item.z - z)
                    && height > item.y - y
                    && item.height > y - item.y)
                {
                    applyCollisionLiving(item);
                }
            };
        }

        public virtual void applyCollisionLiving(LivingEntity entity)
        {
            if(entity.x == x && entity.y == y && entity.z == z)
            {
                return; // 충돌이 발생하지 않도록 동일한 위치에 있는 경우 무시
            }
            Vector3 direction = Vector3.Normalize(new Vector3(entity.x - x, entity.y - y, entity.z - z));
            float powerFactor = weight / entity.weight;
            entity.push(direction.X * pushPower * powerFactor,
                direction.Y * pushPower * powerFactor,
                direction.Z * pushPower * powerFactor);
            push(direction.X * -pushPower, direction.Y * -pushPower, direction.Z * -pushPower);
        }

        public virtual bool doHurtTarget(LivingEntity entity)
        {
            return false;
        }

        public virtual bool hurt(LivingEntity attacker, int damage)
        {
            return false;
        }

        public void setSharedFlag(int input, bool enable)
        {
            if (enable)
            {
                sharedFlags |= 1 << input; // 플래그 설정
            }
            else
            {
                sharedFlags &= ~1 << input; // 플래그 해제
            }
        }

        // 비트 플래그 확인 함수
        public bool getSharedFlag(int input)
        {
            return (sharedFlags & 1 << input) != 0;
        }
        
        public virtual void tick()
        {
            tickCount++;
            if(y < 2)
            {
                deltaMovement = deltaMovement * 0.7f;
            }
            else
            {
                push(-deltaMovement.X * 0.1f, 0, -deltaMovement.Z * 0.1f);
                push(0, -2f, 0);
            }
            if (deltaMovement != Vector3.Zero)
                if (deltaMovement.Y == 0)
                    this.moveTo(x + deltaMovement.X, z + deltaMovement.Z);
                else
                    this.moveTo(x + deltaMovement.X, y + deltaMovement.Y, z + deltaMovement.Z);
        }
    }
}
