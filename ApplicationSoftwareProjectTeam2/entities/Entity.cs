using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class Entity
    {
        int tickCount, sharedFlags;
        float x, xold, y, yold, z, zold;
        Vector3 deltaMovement;
        Form1 level;

        public Entity(Form1 level, float x, float y, float z, Vector3 vec3)
        {
            tickCount = 0;
            this.level = level;
            this.x = this.xold = x;
            this.y = this.yold = y;
            this.z = this.zold = z;
            deltaMovement = vec3;
        }

        public Entity(Form1 level) 
        {
            tickCount = 0;
            this.level = level;
            this.x = this.xold = 0;
            this.y = this.yold = 0;
            this.z = this.zold = 0;
            deltaMovement = Vector3.Zero;
        }

        public void setPosition(float x, float y, float z)
        {
            this.x = this.xold = x;
            this.y = this.yold = y;
            this.z = this.zold = z;
        }
        public void setPosition(float x, float z)
        {
            this.x = this.xold = x;
            this.z = this.zold = z;
        }
        public void setPosition(float y)
        {
            this.y = this.yold = y;
        }

        public void moveTo(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public void moveTo(float x, float z)
        {
            this.x = x;
            this.z = z;
        }

        public void setDeltaMovement(Vector3 vec)
        {
            this.deltaMovement = vec;
        }

        public void setDeltaMovement(float x, float y, float z)
        {
            this.deltaMovement = new Vector3(x, y, z);
        }

        public void push(Vector3 vec)
        {
            this.deltaMovement += vec;
        }

        public void push(float x, float y, float z)
        {
            this.deltaMovement = new Vector3(this.deltaMovement.X + x
                , this.deltaMovement.Y + y
                , this.deltaMovement.Z + z);
        }

        public void tick()
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
