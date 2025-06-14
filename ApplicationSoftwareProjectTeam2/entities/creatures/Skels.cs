using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
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
        #region 캐릭터 아이디 기록
        public byte getLivingEntityId()
        {
            return 10;
        }
        #endregion
        public override void tickAlive()
        {
            base.tickAlive();
            switch (entityState)
            {
                case 0:
                    if (hasAi)
                    {
                        #region 평상시에 아무렇게 걸어다니기 + 타겟 탐색
                        if (level.getRandomInteger(10) == 0)
                        {
                            isMoving = !isMoving || getTarget() != null;
                        }
                        #endregion
                        if (isMoving)
                        {
                            if (tickCount % 4 == 0)
                            {
                                //mana++;
                                if (getTarget() == null)
                                {
                                    int dir = (int)direction;
                                    dir = dir + (level.getRandomInteger(2) == 0 ? 1 : -1);
                                    dir = dir > 9 ? 0 : dir < 0 ? 9 : dir;
                                    direction = (Direction)dir;
                                }
                            }
                            if (isOnGround()) move(moveSpeed);
                        }
                    }
                    #region 캐릭터가 실제로 움직이고 있는 지 결정하는 부분으로, 애니매이션 재생에 쓰임
                    if (deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z > 4)
                    {
                        walkTicks++;
                        isActuallyMoving = true;

                        //걷는 애니메이션
                        switch (walkTicks)
                        {
                            case 2:
                                Image = (int)direction < 5 ? images[4] : images[6];
                                break;
                            case 4:
                                Image = (int)direction < 5 ? images[5] : images[7];
                                break;
                            case 6:
                                Image = (int)direction < 5 ? images[4] : images[6];
                                break;
                            case 8:
                                Image = (int)direction < 5 ? images[0] : images[2];
                                walkTicks = 0;
                                break;
                        }
                    }
                    #endregion
                    #region 평상시 애니매이션 재생 부분
                    else
                    {
                        if (isActuallyMoving)
                        {
                            isActuallyMoving = false;
                            Image = (int)direction < 5 ? images[0] : images[2];
                        }
                        walkTicks = 0;
                        switch (tickCount % 16)
                        {
                            case 0:
                                Image = (int)direction < 5 ? images[0] : images[2];
                                break;
                            case 8:
                                Image = (int)direction < 5 ? images[1] : images[3];
                                break;
                        }
                    }
                    #endregion
                    
                    break;
            }
        }
    }
}
