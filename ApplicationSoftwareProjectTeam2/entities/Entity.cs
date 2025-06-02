using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class Entity
    {
        public event EventHandler landedEvent, collisionEvent;
        public int tickCount, sharedFlags, width = 40, height = 40, weight = 1, pushPower = 2;
        public float x, y, z, visualSize, elasticForce = -0.1f, groundFraction = 0.7f, airFraction = 0.1f, gravity = 2.0f;
        public Vector3 deltaMovement;
        public GamePanel level;
        public string team;
        public Image Image;
        public bool shouldRemove = false, hasGravity = true, grabbedByMouse = false, hasAi = true, wasOnGround;


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
        public virtual void scaleEntity(float scale)
        {
            this.visualSize *= scale;
            this.width = (int)(width * scale);
            this.height = (int)(width * scale);
            this.weight = (int)(weight * scale);
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
            if (!hasAi || x < 500 && x > -500)
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
                if (deltaMovement.Y < -2)
                    landed();
                deltaMovement.Y *= elasticForce;
            }
            if (!hasAi || z < 700 && z > 200)
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
            if (!hasAi || x < 500 && x > -500)
            {
                this.x = x;
            }
            else
            {
                this.x = x >= 500 ? 500 : -500;
                deltaMovement.X *= -1;
                collisionOccurred();
            }
            if (!hasAi || z < 700 && z > 200)
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
            collisionEvent?.Invoke(this, EventArgs.Empty);
        }
        public virtual void landed()
        {
            landedEvent?.Invoke(this, EventArgs.Empty);
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
        public virtual void checkCollisions()
        {
            foreach (var item in level.getAllEntities<LivingEntity>())
            {
                if (!item.Equals(this)
                    && (width + item.width) * 0.5 > Math.Abs(item.x - x) + Math.Abs(item.z - z)
                    && height > item.y - y
                    && item.height > y - item.y)
                {
                    applyCollision(item);
                }
            };
        }
        public virtual void applyCollision(Entity? entity)
        {

        }
        public virtual void applyCollisionLiving(LivingEntity entity)
        {
            if(entity.x == x && entity.y == y && entity.z == z)
            {
                return; // 충돌이 발생하지 않도록 동일한 위치에 있는 경우 무시
            }
            Vector3 direction = Vector3.Normalize(new Vector3(entity.x - x, entity.y - y, entity.z - z));
            float powerFactor = weight / entity.weight;
            entity.push(direction.X * 2 * powerFactor,
                0,//direction.Y * 2 * powerFactor,
                direction.Z * 2 * powerFactor);
            //push(direction.X * -2, direction.Y * -2, direction.Z * -2);
        }

        public virtual bool doHurtTarget(LivingEntity entity)
        {
            return false;
        }
        public virtual bool doHurtTarget(LivingEntity entity, float damage)
        {
            return false;
        }
        public virtual bool doHurtTarget(LivingEntity entity, float damage, int pushPower)
        {
            return false;
        }
        public virtual bool hurt(LivingEntity attacker, float damage)
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
        public virtual bool isOnGround()
        {
            return y < 1;
        }
        public virtual void releaseFromMouse()
        {
            setPosition(x, 0, z);
        }
        public virtual void grabOccurred()
        {
        }
        public virtual void tick()
        {
            tickCount++;
            if (grabbedByMouse)
            {
                float scale = (float)level.currentWidth / GamePanel.worldWidth;
                double fixedScale2 = z > 200 ? 6.184 / Math.Cbrt(z + 250) : 0.823654768;

                // 화면 좌표 변환의 역변환을 적용합니다.
                x = (level.mouseX - level.currentWidth / 2f) / (scale * (float)fixedScale2);
                z = (level.currentHeight - level.mouseY) / (scale * (float)fixedScale2);
                deltaMovement = Vector3.Zero;
                setPosition(x, height / 2, Math.Max(z - height / 2, 0));
                if (!level.grabbed)
                {
                    this.releaseFromMouse();
                    grabbedByMouse = false;
                }
            }
            else
            {
                if (isOnGround())
                {
                    deltaMovement = deltaMovement * groundFraction;
                }
                else
                {
                    push(-deltaMovement.X * airFraction, 0, -deltaMovement.Z * airFraction);
                    push(0, -gravity, 0);
                }
                if (deltaMovement != Vector3.Zero)
                    if (deltaMovement.Y == 0)
                        this.moveTo(x + deltaMovement.X, z + deltaMovement.Z);
                    else
                        this.moveTo(x + deltaMovement.X, y + deltaMovement.Y, z + deltaMovement.Z);
            }
            wasOnGround = isOnGround();
        }
    }
}
