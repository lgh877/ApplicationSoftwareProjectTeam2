using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.entities;
using ApplicationSoftwareProjectTeam2.entities.eventArgs;
using ApplicationSoftwareProjectTeam2.entities.miscellaneous;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class ExplosiveGasVesselItem : Item
    {
        public ExplosiveGasVesselItem() : base("폭발성 가스 용기", 10, ItemType.Special, "받는 피해량이 소폭 증가하지만 사망 시 최대체력의 50% 만큼의 데미지를 주는 폭발을 발생시킨다.")
        {
            Id = 2;
        }
        public override void ApplyTo(LivingEntity unit)
        {
            unit.deathEvent += deathOccur;
            unit.hurtEvent += hurtOccur;
            // Implement the logic for applying this item to a living entity
        }
        private void deathOccur(object sender, EventArgs e)
        {
            LivingEntity entity = (LivingEntity)sender;
            GamePanel level = entity.level;
            float damage = entity.maxHealth * 0.5f; // 최대 체력의 50% 만큼의 데미지
            level.playSound(GroundExplosion.sounds[level.getRandomInteger(4)]);
            GroundExplosion exp = new GroundExplosion(level);
            exp.Owner = entity; exp.attackDamage = entity.maxHealth / 2;
            exp.x = entity.x; exp.y = entity.y; exp.z = entity.z - 32;
            exp.team = entity.team;
            level.addFreshEntity(exp);
            exp.checkCollisionsLiving();
        }
        private void hurtOccur(object attacker, AttackEventArgs attackEventArgs)
        {
            attackEventArgs.damage *= 1.1f; // 받는 피해량이 소폭 증가
        }
    }
}
