using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class FloatingNumberEntity : NumberEntity
    {
        private int initialY;
        public FloatingNumberEntity(GamePanel level, int x, int y, int z, int value) : base(level, x, y, z, value)
        {
            initialY = y;
        }
        public override void tick()
        {
            tickCount++;
            y = level.Lerp(y, initialY + 10, 0.3f);
            if (tickCount > 40) discard();
        }
    }
}
