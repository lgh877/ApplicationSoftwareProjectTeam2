using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class NumberEntity : Entity
    {
        static List<Image> images = new List<Image>
        {
            Properties.Resources.one,
            Properties.Resources.two,
            Properties.Resources.three,
            Properties.Resources.four,
            Properties.Resources.five,
            Properties.Resources.six,
            Properties.Resources.seven,
            Properties.Resources.eight,
            Properties.Resources.nine,
        };
        public NumberEntity(GamePanel level, int x, int y, int z, byte value) : base(level)
        {
            visualSize = 2f;
            setPosition(x, y, z);
            Image = images[value - 1];
        }
        public override void tick()
        {
        }
    }
}
