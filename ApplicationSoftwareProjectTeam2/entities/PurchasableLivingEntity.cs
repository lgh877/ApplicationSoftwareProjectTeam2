using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public abstract class PurchasableLivingEntity : LivingEntity
    {
        public bool isPurchased;
        protected PurchasableLivingEntity(GamePanel level) : base(level)
        {
        }
    }
}
