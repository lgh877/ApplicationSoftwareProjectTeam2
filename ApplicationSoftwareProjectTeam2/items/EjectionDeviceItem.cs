using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.entities;
using ApplicationSoftwareProjectTeam2.entities.eventArgs;
using ApplicationSoftwareProjectTeam2.resources.sounds;
using WMPLib;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class EjectionDeviceItem : Item
    {
        public static List<WindowsMediaPlayer> sounds = new List<WindowsMediaPlayer>()
        {
            SoundCache.teleport1, SoundCache.teleport2, SoundCache.teleport3
        };
        public EjectionDeviceItem() : base("긴급탈출장치", 10, ItemType.Special, "피격 시 일정 확률로 피해량을 90% 줄여서 받고 필드 내 무작위 위치로 순간이동 한다.")
        {
            Id = 1;
        }
        public override void ApplyTo(LivingEntity unit)
        {
            base.ApplyTo(unit);
            unit.hurtEvent += hurtOccur;
        }
        private void hurtOccur(object attacker, AttackEventArgs attackEventArgs)
        {
            GamePanel level = attackEventArgs.Target.level;
            if(level.getRandomInteger(10) == 0) // 10% 확률로 발동
            {
                attackEventArgs.damage *= 0.1f; // 피해량을 90% 감소
                level.playSound(sounds[level.getRandomInteger(3)]);
                // 필드 내 무작위 위치로 순간이동
                int newX = level.getRandomInteger(1000) - 500;
                float newZ = level.getRandomInteger(500) + 200;
                attackEventArgs.Target.setPosition(newX, attackEventArgs.Target.y, newZ);
            }   
        }
    }
}
