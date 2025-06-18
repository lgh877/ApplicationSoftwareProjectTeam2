using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.items;

namespace ApplicationSoftwareProjectTeam2.entities.items
{
    public class EjectionDeviceItemEntity : ItemEntity
    {
        public EjectionDeviceItemEntity(GamePanel level) : base(level)
        {
            cost = 4;
            visualSize = 1.0f; width = 48; height = 64;
            Image = Properties.Resources.ejectionDevice;
            mainItem = new EjectionDeviceItem();
        }
    }
}
