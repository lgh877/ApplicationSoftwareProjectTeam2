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
            deltaMovement = deltaMovement * 0.95f;
            if (target == null)
            {
                float a, b;
                a = level.getRandomInteger(5) - 2;
                b = level.getRandomInteger(5) - 2;
                push(a, 0, b);
            }
            else
            {
                Vector3 direction = Vector3.Normalize(new Vector3(target.x - x, 0, target.z - z));
                push(direction.X * 2, 0, direction.Z * 2);
            }
            if (tickCount % 50 == 0 && target == null)
            {
                LivingEntity foundTarget = detectTargetManhattan(50);
                if (foundTarget != null)
                {
                    Image = Properties.Resources._4;
                    target = foundTarget;
                }
            }
            
        }

        //맨해튼 거리 기반으로 주변에 있는 타겟을 찾는 메서드
        public LivingEntity detectTargetManhattan(int range)
        {
            foreach (var item in level.getAllLivingEntities<LivingEntity>())
            {
                if (!item.Equals(this) && !item.team.Equals(this.team) && range > Math.Abs(item.x - x) + Math.Abs(item.z - z))
                {
                    return item;
                }
            };
            return null;
        }
    }
}
