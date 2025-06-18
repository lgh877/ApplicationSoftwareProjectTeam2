using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.entities.projectiles;
using ApplicationSoftwareProjectTeam2.resources.sounds;
using ApplicationSoftwareProjectTeam2.utils;
using WMPLib;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class Skels : LivingEntity
    {
        public static List<WindowsMediaPlayer> sounds = new List<WindowsMediaPlayer>()
        {
            SoundCache.bone_crack1, SoundCache.bone_crack2, SoundCache.swosh
        };
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
            Properties.Resources.sprite_Skel1_attack0,
            Properties.Resources.sprite_Skel1_attack1,
            Properties.Resources.sprite_Skel1_attack2,
            Properties.Resources.sprite_Skel1_attack3,
            Properties.Resources.sprite_Skel1_attack4,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_attack0),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_attack1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_attack2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_attack3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Skel1_attack4),
            //8 ~ 17번 인덱스
        };
        public Skels(GamePanel level) : base(level)
        {
            cost = 2;
            visualSize = 2f; width = 46; height = 54; weight = 8; pushPower = 10;
            Image = images[0];
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 75; currentHealth = 75; finalMaxHealth = maxHealth;
            attackDamage = 13; finalAttackDamage = attackDamage;
            moveSpeed = 3;
        }
        public override void scaleEntity(float scale)
        {
            base.scaleEntity(scale);
            maxHealth *= scale; currentHealth = maxHealth;
            attackDamage *= scale; pushPower = (int)(pushPower * scale); moveSpeed = (int)(moveSpeed * Math.Sqrt(scale));
        }
        public override EntityTypes getEntityType()
        {
            return EntityTypes.Undeads;
        }
        #region 캐릭터 아이디 기록
        public override byte getLivingEntityId()
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
                                //mana++;
                                if (getTarget() == null)
                                {
                                    int dir = (int)direction;
                                    dir = dir + (level.getRandomInteger(2) == 0 ? 1 : -1);
                                    dir = dir > 9 ? 0 : dir < 0 ? 9 : dir;
                                    direction = (Direction)dir;
                                }
                                else
                                {
                                    if ((target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 90000)
                                    {
                                        entityState = 1;
                                        walkTicks = 0;
                                    }
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
                        if (walkTicks == 2) Image = (int)direction < 5 ? images[4] : images[6];
                        else if (walkTicks == 4) Image = (int)direction < 5 ? images[5] : images[7];
                        else if (walkTicks == 6) Image = (int)direction < 5 ? images[4] : images[6];
                        else if (walkTicks > 7) { Image = (int)direction < 5 ? images[0] : images[2]; walkTicks = 0; }
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
                        walkTicks = tickCount % 16;
                        if (walkTicks == 0) Image = (int)direction < 5 ? images[0] : images[2];
                        else if (walkTicks == 8) Image = (int)direction < 5 ? images[1] : images[3];
                    }
                    #endregion
                    break;
                case 1:
                    direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    walkTicks++;
                    switch (walkTicks)
                    {
                        case 1:
                            Image = (int)direction < 5 ? images[8] : images[13];
                            break;
                        case 3:
                            Image = (int)direction < 5 ? images[9] : images[14];
                            break;
                        case 5:
                            Image = (int)direction < 5 ? images[10] : images[15];
                            break;
                        case 7:
                            Image = (int)direction < 5 ? images[11] : images[16];
                            level.playSound(sounds[2]);
                            Vector3 targetVec = Vector3.Normalize(new Vector3(target.x - x,
                                                                                (target.y - y) * 2f,
                                                                                target.z - z));
                            float distance = (float)Math.Cbrt((target.x - x) * (target.x - x) + (target.y - y) * (target.y - y) + (target.z - z) * (target.z - z)) * 1.5f;
                            SkelsBone bone = new SkelsBone(level);
                            bone.Owner = this;
                            bone.attackDamage = finalAttackDamage; bone.pushPower = pushPower;
                            bone.x = x; bone.y = y + height; bone.z = z;
                            bone.deltaMovement = targetVec * distance;
                            bone.team = team;
                            for (int j = 0; j < entityLevel; j++) bone.scaleEntity(1.2f);
                            level.addFreshEntity(bone);
                            break;
                        case 13:
                            Image = (int)direction < 5 ? images[12] : images[17];
                            break;
                        case 17:
                            Image = (int)direction < 5 ? images[5] : images[7];
                            break;
                        case 19:
                            Image = (int)direction < 5 ? images[1] : images[3];
                            break;
                        case 20:
                            walkTicks = 0;
                            entityState = 0;
                            break;
                    }
                    break;
            }
        }
        public override void setDeath(object? sender, EventArgs e)
        {
            level.playSound(sounds[level.getRandomInteger(2)]);
            SkelsBone bone = new SkelsBone(level);
            bone.Owner = this;
            bone.attackDamage = finalAttackDamage; bone.pushPower = pushPower;
            bone.x = x; bone.y = y + height; bone.z = z;
            bone.deltaMovement = deltaMovement * 1.6f;
            bone.team = team;
            for (int j = 0; j < entityLevel; j++) bone.scaleEntity(1.2f);
            level.addFreshEntity(bone);
            SkelsSkull skull = new SkelsSkull(level);
            skull.Owner = this; skull.Image = (int)direction < 5 ? SkelsSkull.images[0] : SkelsSkull.images[2];
            skull.attackDamage = finalAttackDamage; skull.pushPower = pushPower;
            skull.x = x; skull.y = y + height; skull.z = z;
            skull.deltaMovement = deltaMovement;
            skull.team = team;
            for (int j = 0; j < entityLevel; j++) skull.scaleEntity(1.2f);
            level.addFreshEntity(skull);
            SkelsSkull skull2 = new SkelsSkull(level);
            skull2.Owner = this; skull2.Image = (int)direction < 5 ? SkelsSkull.images[1] : SkelsSkull.images[3];
            skull2.attackDamage = finalAttackDamage; skull2.pushPower = pushPower;
            skull2.x = x; skull2.y = y + height / 2; skull2.z = z;
            skull2.deltaMovement = deltaMovement * 1.3f;
            skull2.team = team;
            for (int j = 0; j < entityLevel; j++) skull2.scaleEntity(1.2f);
            level.addFreshEntity(skull2);
            discard();
        }
    }
}
