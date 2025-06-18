using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.entities.miscellaneous;
using ApplicationSoftwareProjectTeam2.entities.projectiles;
using ApplicationSoftwareProjectTeam2.resources.sounds;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class Skulls : LivingEntity
    {
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.sprite_Skel3_idle0,
            Properties.Resources.sprite_Skel3_idle1,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel3_idle0),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel3_idle1),
            //0 ~ 3번 인덱스
            Properties.Resources.sprite_Skel3_move0,
            Properties.Resources.sprite_Skel3_move1,
            Properties.Resources.sprite_Skel3_move2,
            Properties.Resources.sprite_Skel3_move3,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel3_move0),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel3_move1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel3_move2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel3_move3),
            //4 ~ 11번 인덱스
            Properties.Resources.sprite_Skel3_attack0,
            Properties.Resources.sprite_Skel3_attack1,
            Properties.Resources.sprite_Skel3_attack2,
            Properties.Resources.sprite_Skel3_attack3,
            Properties.Resources.sprite_Skel3_attack4,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel3_attack0),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel3_attack1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel3_attack2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel3_attack3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel3_attack4),
            //12 ~ 21번 인덱스
            Properties.Resources.Skel3_skull_png,
            Properties.Resources.Skel3_torso_png,
            Properties.Resources.Skel3_chin_png,
            ImageUtils.FlipImageHorizontally(Properties.Resources.Skel3_skull_png),
            ImageUtils.FlipImageHorizontally(Properties.Resources.Skel3_torso_png),
            ImageUtils.FlipImageHorizontally(Properties.Resources.Skel3_chin_png),
            //22 ~ 27번 인덱스
        };
        public Skulls(GamePanel level) : base(level)
        {
            cost = 4;
            visualSize = 2.5f; width = 57; height = 80; weight = 55; pushPower = 50;
            Image = images[0];
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 250; currentHealth = 250; finalMaxHealth = maxHealth;
            attackDamage = 15; finalAttackDamage = attackDamage;
            moveSpeed = 2;
        }
        public override void scaleEntity(float scale)
        {
            base.scaleEntity(scale);
            maxHealth *= scale * 1.4f; currentHealth = maxHealth * 1.4f;
            attackDamage *= scale * 1.4f; pushPower = (int)(pushPower * scale); moveSpeed = (int)(moveSpeed * Math.Sqrt(scale));
        }
        public override EntityTypes getEntityType()
        {
            return EntityTypes.Undeads;
        }
        #region 캐릭터 아이디 기록
        public override byte getLivingEntityId()
        {
            return 12;
        }
        #endregion
        public override void tickAlive()
        {
            base.tickAlive();
            switch (entityState)
            {
                case 0:
                    if (canStartTask())
                    {
                        effectWeather(); // 날씨 효과 적용
                        #region 평상시에 아무렇게 걸어다니기 + 타겟 탐색
                        if (level.getRandomInteger(10) == 0)
                        {
                            if (getTarget() == null)
                            {
                                if (hadTarget)
                                {
                                    hadTarget = false;
                                }
                                LivingEntity foundTarget = detectTargetManhattan(1000);
                                if (foundTarget != null)
                                {
                                    target = foundTarget;
                                    hadTarget = true;
                                }
                            }
                            isMoving = !isMoving || getTarget() != null;
                        }
                        #endregion
                        if (isMoving)
                        {
                            if (tickCount % 4 == 0)
                            {
                                mana++;
                                if (getTarget() == null)
                                {
                                    int dir = (int)direction;
                                    dir = dir + (level.getRandomInteger(2) == 0 ? 1 : -1);
                                    dir = dir > 9 ? 0 : dir < 0 ? 9 : dir;
                                    direction = (Direction)dir;
                                }
                                else
                                {
                                    float ydiff = target.y - y;

                                    if ((target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.25 * (width * width + (target.width + width) * (target.width + width))
                                    && ydiff < height
                                    && ydiff > -target.height)
                                    {
                                        entityState = 1;
                                        walkTicks = 0;
                                    }

                                    //공격 시도가 실패한 경우 상대 방향으로 이동
                                    if (entityState == 0)
                                    {
                                        int dir = (int)direction;
                                        Vector3 targetVec = Vector3.Normalize(new Vector3(target.x - x, 0, target.z - z));

                                        if (targetVec.X > 0)
                                        {
                                            if (targetVec.Z > 0.9659) dir = 0;
                                            else if (targetVec.Z > 0.7071) dir = 1;
                                            else if (targetVec.Z > -0.7071) dir = 2;
                                            else if (targetVec.Z > -0.9659) dir = 3;
                                            else dir = 4;
                                        }
                                        else
                                        {
                                            if (targetVec.Z > 0.9659) dir = 9;
                                            else if (targetVec.Z > 0.7071) dir = 8;
                                            else if (targetVec.Z > -0.7071) dir = 7;
                                            else if (targetVec.Z > -0.9659) dir = 6;
                                            else dir = 5;
                                        }
                                        direction = (Direction)dir;
                                    }
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
                        if (walkTicks == 3) Image = (int)direction < 5 ? images[4] : images[8];
                        else if (walkTicks == 6) Image = (int)direction < 5 ? images[5] : images[9];
                        else if (walkTicks == 9) Image = (int)direction < 5 ? images[6] : images[10];
                        else if (walkTicks > 11) { Image = (int)direction < 5 ? images[7] : images[11]; walkTicks = 0; }
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
                        walkTicks = tickCount % 20;
                        if (walkTicks == 0) Image = (int)direction < 5 ? images[0] : images[2];
                        else if (walkTicks == 10) Image = (int)direction < 5 ? images[1] : images[3];
                    }
                    #endregion
                    break;
                case 1:
                    direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    walkTicks++;
                    #region 평타 공격 작동 코드
                    if (walkTicks == 1) Image = (int)direction < 5 ? images[12] : images[17];
                    else if (walkTicks == 4) Image = (int)direction < 5 ? images[13] : images[18];
                    else if (walkTicks == 7) Image = (int)direction < 5 ? images[14] : images[19];
                    else if (walkTicks == 10)
                    {
                        Image = (int)direction < 5 ? images[15] : images[20];
                        level.playSound(SoundCache.skulls_bite);
                        VerticalImpactVisualEffect eff = new VerticalImpactVisualEffect(level, (int)direction < 5, x + ((int)direction < 5 ? 20 : -20) * visualSize, y - 28 + height * 0.1875f, z - 2);
                        level.addFreshEntity(eff);
                        if (target != null && (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.25 * (width * width + (target.width + width) * (target.width + width))
                                && target.y - y < height && target.y - y > -target.height)
                        {
                            doHurtTarget(target);
                        }
                    }
                    else if (walkTicks == 11) Image = (int)direction < 5 ? images[16] : images[21];
                    else if (walkTicks == 13) Image = (int)direction < 5 ? images[15] : images[20];
                    else if (walkTicks == 15) Image = (int)direction < 5 ? images[12] : images[17];
                    else if (walkTicks == 17)
                    {
                        walkTicks = 0;
                        entityState = 0;
                    }
                    #endregion
                    break;
            }
        }
        public override void setDeath(object? sender, EventArgs e)
        {
            level.playSound(Skels.sounds[level.getRandomInteger(2)]);
            for (int i = 0; i < 4; i++)
            {
                SkelsBone bone = new SkelsBone(level);
                bone.imageOffset = level.getRandomInteger(4);
                bone.Owner = this;
                bone.attackDamage = finalAttackDamage / 2; bone.pushPower = pushPower;
                bone.x = x; bone.y = y + height; bone.z = z;
                bone.deltaMovement = new Vector3(deltaMovement.X + level.getRandomInteger(11) - 5
                    , deltaMovement.Y + level.getRandomInteger(7)
                    , deltaMovement.Z + level.getRandomInteger(11) - 5);
                bone.team = team;
                for (int j = 0; j < entityLevel; j++) bone.scaleEntity(1.2f);
                level.addFreshEntity(bone);
            }
            SkullsSkull skull = new SkullsSkull(level);
            skull.Owner = this; skull.Image = (int)direction < 5 ? images[22] : images[25];
            skull.visualSize = 3f; skull.width = 57; skull.height = 55;
            skull.attackDamage = finalAttackDamage; skull.pushPower = pushPower;
            skull.x = x; skull.y = y + height / 2; skull.z = z;
            skull.deltaMovement = -deltaMovement;
            skull.team = team;
            for (int j = 0; j < entityLevel; j++) skull.scaleEntity(1.2f);
            level.addFreshEntity(skull);
            SkelsSkull skull2 = new SkelsSkull(level);
            skull2.Owner = this; skull2.Image = (int)direction < 5 ? images[23] : images[26];
            skull2.visualSize = 3f; skull2.width = 17; skull2.height = 17;
            skull2.attackDamage = finalAttackDamage / 2; skull2.pushPower = pushPower;
            skull2.x = x; skull2.y = y + height / 3; skull2.z = z;
            skull2.deltaMovement = deltaMovement * 0.5f;
            skull2.team = team;
            for (int j = 0; j < entityLevel; j++) skull2.scaleEntity(1.2f);
            level.addFreshEntity(skull2);
            SkelsSkull skull3 = new SkelsSkull(level);
            skull3.Owner = this; skull3.Image = (int)direction < 5 ? images[24] : images[27];
            skull3.visualSize = 3f; skull3.width = 32; skull3.height = 15;
            skull3.attackDamage = finalAttackDamage / 2; skull3.pushPower = pushPower;
            skull3.x = x; skull3.y = y + height / 3; skull3.z = z;
            skull3.deltaMovement = deltaMovement * 0.7f;
            skull3.team = team;
            for (int j = 0; j < entityLevel; j++) skull3.scaleEntity(1.2f);
            level.addFreshEntity(skull3);
            discard();
        }
    }
}
