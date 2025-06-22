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
        public float offsetX, offsetY, offsetZ, baseX, baseY, baseZ;
        public LivingEntity Owner;
        public PartEntity(GamePanel level) : base(level) { renderType = 0; visualSize = 1.0f; }
        public PartEntity(GamePanel level, LivingEntity owner, float x, float y, float z) : base(level) 
        {
            Owner = owner; baseX = x; baseY = y; baseZ = z;
            renderType = 0; visualSize = 1.0f; setPosition(owner.x + x, owner.y + y, owner.z + z); 
        }
        public override void tick()
        {
            tickCount++;
            float sizeFactor = visualSize;
            float bx = baseX;
            if ((int)Owner.direction < 5)
            {
                bx *= -1; // 반전
            }
            this.setPosition(Owner.x + (bx + offsetX) * sizeFactor,
                            Owner.y + (baseY + offsetY) * sizeFactor,
                            Owner.z + (baseZ + offsetZ) * sizeFactor);
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
