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
        public int tickCount, sharedFlags, visualSize = 40, width = 40, height = 40;
        public float x, xold, y, yold, z, zold;
        public Vector3 deltaMovement;
        public GamePanel level;
        public string team;
        public Image Image;
        public bool shouldRemove = false;


        public Entity(GamePanel level, float x, float y, float z, Vector3 vec3)
        {
            Image = Properties.Resources._2;
            tickCount = 0;
            this.level = level;
            this.x = this.xold = x;
            this.y = this.yold = y;
            this.z = this.zold = z;
            deltaMovement = vec3;
        }

        public Entity(GamePanel level) 
        {
            Image = Properties.Resources._2;
            tickCount = 0;
            this.level = level;
            this.x = this.xold = 0;
            this.y = this.yold = 0;
            this.z = this.zold = 0;
            deltaMovement = Vector3.Zero;
        }

        public virtual void setPosition(float x, float y, float z)
        {
            this.x = this.xold = x;
            this.y = this.yold = y;
            this.z = this.zold = z;
        }
        public virtual void setPosition(float x, float z)
        {
            this.x = this.xold = x;
            this.z = this.zold = z;
        }
        public virtual void setPosition(float y)
        {
            this.y = this.yold = y;
        }

        public virtual void moveTo(float x, float y, float z)
        {
            xold = this.x; yold = this.y; zold = this.z;
            if (x < 500 && x > -500)
            {
                this.x = x;
            }
            else
            {
                this.x = x >= 500 ? 500 : -500;
                deltaMovement.X *= -1;
            }
            this.y = y;
            if (z < 700 && z > 200)
                this.z = z;
            else
            {
                this.z = z <= 200 ? 200 : 700;
                deltaMovement.Z *= -1;
            }
        }
        public virtual void moveTo(float x, float z)
        {
            xold = this.x; yold = this.y; zold = this.z;
            if (x < 500 && x > -500)
            {
                this.x = x;
            }
            else
            {
                this.x = x >= 500 ? 500 : -500;
                deltaMovement.X *= -1;
            }
            if (z < 700 && z > 200)
                this.z = z;
            else
            {
                this.z = z <= 200 ? 200 : 700;
                deltaMovement.Z *= -1;
            }
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
            Vector3 direction = Vector3.Normalize(new Vector3(entity.x - x, entity.y - y, entity.z - z));
            entity.push(direction.X * 2, direction.Y * 2, direction.Z * 2);
            push(direction.X * -2, direction.Y * -2, direction.Z * -2);
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

            if (deltaMovement != Vector3.Zero)
                if (deltaMovement.Y == 0)
                    this.moveTo(x + deltaMovement.X, z + deltaMovement.Z);
                else
                    this.moveTo(x + deltaMovement.X, y + deltaMovement.Y, z + deltaMovement.Z);
        }
    }
}
