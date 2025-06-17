using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class PartEntity : Entity
    {
        public float offsetX, offsetY, offsetZ;
        public LivingEntity Owner;
        public PartEntity(GamePanel level) : base(level) { renderType = 0; visualSize = 2.0f; }
        public PartEntity(GamePanel level, float x, float y, float z) : base(level) { renderType = 0; visualSize = 2.0f; setPosition(x, y, z); }
        public override void tick()
        {
            tickCount++;
            this.setPosition(Owner.x + offsetX,
                            Owner.y + offsetY,
                            Owner.z + offsetZ);
            if(Owner == null)
            {
                shouldRemove = true;
            }
        }
        public override void push(float x, float y, float z)
        {
        }
        public override void push(Vector3 vec)
        {
        }
        public override bool hurt(LivingEntity attacker, float damage)
        {
            return Owner.hurt(attacker, damage);
        }
    }
}
