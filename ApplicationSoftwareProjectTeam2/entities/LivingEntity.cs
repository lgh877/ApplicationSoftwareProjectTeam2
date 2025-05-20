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
            if (tickCount % 10 == 0)
            {
                if(target != null)
                {
                    Vector3 direction = Vector3.Normalize(new Vector3(target.x - x, target.y - y, target.z - z));
                    push(direction.X, direction.Y, direction.Z);
                }else
                {
                    LivingEntity foundTarget = detectTarget();
                    if (foundTarget != null && 50 > Math.Abs(foundTarget.x - x) + Math.Abs(foundTarget.z - z))
                    {
                        target = foundTarget;
                    }
                    else
                    {
                        float a, b;
                        level.random.setSeed(level.randomSeed++);
                        a = level.random.Next(5) - 2;
                        level.random.setSeed(level.randomSeed++);
                        b = level.random.Next(5) - 2;
                        push(a, 0, b);
                    }
                }
                
            }
            
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
