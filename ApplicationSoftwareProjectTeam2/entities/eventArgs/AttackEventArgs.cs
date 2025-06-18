using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities.eventArgs
{
    public class AttackEventArgs : EventArgs
    {
        public LivingEntity Target;
        public float damage;

        public AttackEventArgs(LivingEntity target, float damage)
        {
            Target = target;
            this.damage = damage;
        }
    }
}
