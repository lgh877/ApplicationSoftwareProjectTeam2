using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.entities.eventArgs;
using ApplicationSoftwareProjectTeam2.entities;
using ApplicationSoftwareProjectTeam2.entities.miscellaneous;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class FlamingGloveItem : Item
    {
        public FlamingGloveItem() : base("불꽃권투장갑", 10, ItemType.Melee, "근접 공격 시 25% 확률로 폭발이 발생한다.")
        {
            Id = 4;
        }
        public override void ApplyTo(LivingEntity unit)
        {
            unit.attackEvent += attackOccur;
        }
        private void attackOccur(object attacker, AttackEventArgs attackEventArgs)
        {
            LivingEntity entity = (LivingEntity)attacker;
            GamePanel level = entity.level;
            if(level.getRandomInteger(4) != 0) return;
            Explosion exp = new Explosion(level);
            exp.scaleEntity((entity.entityLevel + 1) * 0.33f);
            exp.Owner = entity; exp.attackDamage = entity.finalAttackDamage;
            exp.x = attackEventArgs.Target.x; exp.y = attackEventArgs.Target.y; exp.z = attackEventArgs.Target.z - (float) Math.Cbrt(exp.width * 0.25f);
            exp.team = entity.team;
            level.addFreshEntity(exp);
            exp.checkCollisionsLiving();
        }
    }
}
