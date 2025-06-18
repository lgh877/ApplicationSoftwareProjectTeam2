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
        public ChainsawItem() : base("체인톱", 10, ItemType.Melee, "근접 공격력 +n, 적을 베어 넘어뜨림")
        {
            Id = 0;
            this.SpeedBonus = 3;
        }
        public override void ApplyTo(LivingEntity unit)
        {
            unit.moveSpeed += SpeedBonus;
            unit.attackEvent += attackOccur;

        }
        public void attackOccur(object attacker, AttackEventArgs attackEventArgs)
        {
            attackEventArgs.damage *= 1.2f;
            GamePanel level = ((LivingEntity)attacker).level;
            level.playSound(sounds[level.getRandomInteger(3)]);
        }
    }
}
