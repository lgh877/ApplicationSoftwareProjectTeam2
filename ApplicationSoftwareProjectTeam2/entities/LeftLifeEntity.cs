using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class LeftLifeEntity : Entity
    {
        public LeftLifeEntity(GamePanel level, float x) : base(level)
        {
            visualSize = 2f;
            Image = Properties.Resources.heart;
            hasGravity = false;
            renderType = 0;
            setPosition(x, 550, 2);
        }
    }
}
