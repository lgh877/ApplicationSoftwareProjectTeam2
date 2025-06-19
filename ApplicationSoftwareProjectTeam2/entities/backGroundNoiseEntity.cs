using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class backGroundNoiseEntity : Entity
    {
        public int lifeTime = 30;
        public backGroundNoiseEntity(GamePanel level, int lifetime) : base(level) 
        {
            visualSize = 20f;
            Image = Properties.Resources.backGroundNoise;
            renderType = 0;
            setPosition(0, -10, 1);
            lifeTime = lifetime;
        }
        public override void tick()
        {
            tickCount++;
            if (tickCount == lifeTime) discard();
        }
    }
}
