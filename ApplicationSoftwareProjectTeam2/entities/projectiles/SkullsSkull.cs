using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.entities.miscellaneous;

namespace ApplicationSoftwareProjectTeam2.entities.projectiles
{
    public class SkullsSkull : SkelsSkull
    {
        public SkullsSkull(GamePanel level) : base(level) { elasticForce = -0.1f; }
        public override void landed()
        {
            level.playSound(GroundExplosion.sounds[level.getRandomInteger(4)]);
            GroundSoulExplosion exp = new GroundSoulExplosion(level);
            exp.Owner = this.Owner; exp.attackDamage = attackDamage;
            exp.x = x; exp.y = 2; exp.z = z - 16;
            exp.team = team;
            level.addFreshEntity(exp);
            exp.checkCollisionsLiving();
            base.landed();
        }
    }
}
