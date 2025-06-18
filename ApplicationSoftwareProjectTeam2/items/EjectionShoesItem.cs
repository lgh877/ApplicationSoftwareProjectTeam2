using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.entities.eventArgs;
using ApplicationSoftwareProjectTeam2.entities;
using ApplicationSoftwareProjectTeam2.resources.sounds;
using WMPLib;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class EjectionShoesItem : Item
    {
        public static List<WindowsMediaPlayer> sounds = new List<WindowsMediaPlayer>()
        {
            SoundCache.cosmicJump1, SoundCache.cosmicJump2, SoundCache.cosmicJump3
        };
        public EjectionShoesItem() : base("긴급탈출용부츠", 10, ItemType.Special, "피격 시 일정 확률로 피해량을 50% 줄여서 받고 아무 방향으로 튕겨 오른다. 또한 탄성력도 굉장히 증가한다.")
        {
            Id = 3;
        }
        public override void ApplyTo(LivingEntity unit)
        {
            unit.elasticForce *= 4;
            unit.hurtEvent += hurtOccur;
            unit.landedEvent += (sender, e) =>
            {
                GamePanel level = ((LivingEntity)sender).level;
                level.playSound(sounds[level.getRandomInteger(3)]);
            };
        }
        private void hurtOccur(object attacker, AttackEventArgs attackEventArgs)
        {
            GamePanel level = attackEventArgs.Target.level;
            if (attackEventArgs.Target.isOnGround() && level.getRandomInteger(4) == 0) // 25% 확률로 발동
            {
                attackEventArgs.damage *= 0.5f; // 피해량을 90% 감소
                level.playSound(sounds[level.getRandomInteger(3)]);
                // 필드 내 무작위 위치로 순간이동
                attackEventArgs.Target.push(level.getRandomInteger(81) - 40, 35, level.getRandomInteger(81) - 40);
            }
        }
    }
}
