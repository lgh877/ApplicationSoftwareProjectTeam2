using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class NumberEntity : Entity
    {
        public static List<Image> images = new List<Image>
        {
            Properties.Resources.zero,
            Properties.Resources.one,
            Properties.Resources.two,
            Properties.Resources.three,
            Properties.Resources.four,
            Properties.Resources.five,
            Properties.Resources.six,
            Properties.Resources.seven,
            Properties.Resources.eight,
            Properties.Resources.nine,
            Properties.Resources.gold,
        };
        public NumberEntity(GamePanel level, int x, int y, int z, int value) : base(level)
        {
            visualSize = 2f;
            setPosition(x, y, z);
            Image = images[value];
            renderType = 0;
        }
        public override void tick()
        {
        }
    }
}
