using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.Skels
{
    public class Skels : LivingEntity
    {
        private int walkTicks, mana;
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.sprite_Skel1_idle0,
            Properties.Resources.sprite_Skel1_idle1,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_idle0),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_idle1),
            //0 ~ 3번 인덱스
            Properties.Resources.sprite_Skel1_move0,
            Properties.Resources.sprite_Skel1_move1,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_move0),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_move1),
            //4 ~ 7번 인덱스
            Properties.Resources.weirdGuy_died,
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_died),
            //8 ~ 9번 인덱스
            Properties.Resources.sprite_Skel1_attack0,
            Properties.Resources.sprite_Skel1_attack1,
            Properties.Resources.sprite_Skel1_attack2,
            Properties.Resources.sprite_Skel1_attack3,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_attack0),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_attack1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_attack2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_attack3),
            //10 ~ 17번 인덱스
        };
        public Skels(GamePanel level) : base(level)
        {
            cost = 1;
            visualSize = 2f; width = 46; height = 54; weight = 8; pushPower = 30;
            Image = images[0];
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 75; currentHealth = 75;
            attackDamage = 15;
            moveSpeed = 3;
            mana = 0;
        }
        public override EntityTypes getEntityType()
        {
            return EntityTypes.Undeads;
        }
    }
}
