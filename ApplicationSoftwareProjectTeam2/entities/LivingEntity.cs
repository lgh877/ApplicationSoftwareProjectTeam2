using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class LivingEntity : Entity
    {
        LivingEntity target;
        public LivingEntity(GamePanel level) : base(level) { }

        public override void tick()
        {
            base.tick();
            float a, b;
            level.random.setSeed(level.randomSeed++);
            a = level.random.Next(10) - 5;
            level.random.setSeed(level.randomSeed++);
            b = level.random.Next(10) - 5;
            push(a, 0, b);
            /*
            if (tickCount % 10 == 0)
            {
                if(target != null)
                {
                    Vector3 direction = Vector3.Normalize(new Vector3(target.x - x, target.y - y, target.z - z));
                    push(direction.X, direction.Y, direction.Z);
                }else if(detectTarget() == null)
                {
                    level.random.setSeed(level.randomSeed++);
                    push((float)level.random.NextDouble() * 3, 0, (float)level.random.NextDouble() * 3);
                }
            }
            */
        }

        public LivingEntity detectTarget()
        {
            foreach (var item in level.getAllLivingEntities<LivingEntity>())
            {
                if (item.team.Equals(this.team))
                {
                    return item;
                }
            };
            return null;
        }
    }
}
