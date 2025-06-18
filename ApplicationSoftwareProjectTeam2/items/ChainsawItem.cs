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
    public class ChainsawItem : Item
    {
        public static List<WindowsMediaPlayer> sounds = new List<WindowsMediaPlayer>()
        {
            SoundCache.chainsaw1, SoundCache.chainsaw2, SoundCache.chainsaw3
        };
        public ChainsawItem() : base("체인톱", 10, ItemType.Melee, "캐릭터에게 광기를 불어넣어 이동속도를 빠르게 하고 데미지를 20% 증가시켜준다. 또한 공격 시 전기톱 효과음이 난다.")
        {
            Id = 0;
            this.SpeedBonus = 3;
        }
        public override void ApplyTo(LivingEntity unit)
        {
            unit.moveSpeed += SpeedBonus;
            unit.finalAttackDamage *= 1.2f; // 공격력 20% 증가
            unit.attackEvent += attackOccur;
        }
        private void attackOccur(object attacker, AttackEventArgs attackEventArgs)
        {
            GamePanel level = ((LivingEntity)attacker).level;
            level.playSound(sounds[level.getRandomInteger(3)]);
        }
    }
}
