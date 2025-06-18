using System;
using System.Collections.Generic;
using System.Numerics;
using ApplicationSoftwareProjectTeam2.entities.projectiles;
using ApplicationSoftwareProjectTeam2.utils;
using ApplicationSoftwareProjectTeam2.resources.sounds;
using WMPLib;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class Mushroom1 : LivingEntity
    {

        public static List<Image> images = new List<Image>()
        {
            // idle (0~3)
            Properties.Resources.sprite_Mushroom1_idle1,
            Properties.Resources.sprite_Mushroom1_idle2,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_idle1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_idle2),

            // walk (4~11)
            Properties.Resources.sprite_Mushroom1_walk1,
            Properties.Resources.sprite_Mushroom1_walk2,
            Properties.Resources.sprite_Mushroom1_walk3,
            Properties.Resources.sprite_Mushroom1_walk4,
            Properties.Resources.sprite_Mushroom1_walk5,
            Properties.Resources.sprite_Mushroom1_walk6,
            Properties.Resources.sprite_Mushroom1_walk7,
            Properties.Resources.sprite_Mushroom1_walk8,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_walk1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_walk2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_walk3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_walk4),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_walk5),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_walk6),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_walk7),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_walk8),

            // attack (12~19)
            Properties.Resources.sprite_Mushroom1_attack1,
            Properties.Resources.sprite_Mushroom1_attack2,
            Properties.Resources.sprite_Mushroom1_attack3,
            Properties.Resources.sprite_Mushroom1_attack4,
            Properties.Resources.sprite_Mushroom1_attack5,
            Properties.Resources.sprite_Mushroom1_attack6,
            Properties.Resources.sprite_Mushroom1_attack7,
            Properties.Resources.sprite_Mushroom1_attack8,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_attack1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_attack2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_attack3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_attack4),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_attack5),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_attack6),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_attack7),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_attack8)
        };

        public Mushroom1(GamePanel level) : base(level)
        {
            cost = 2;
            visualSize = 2f;
            width = 40;
            height = 52;
            weight = 7;
            pushPower = 12;
            Image = images[0];
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 120;
            currentHealth = 120;
            finalMaxHealth = maxHealth;
            attackDamage = 40;
            finalAttackDamage = attackDamage;
            moveSpeed = 3;
        }

        public override byte getLivingEntityId() => 11;
        public override EntityTypes getEntityType() => EntityTypes.Avengers;

        public override void tickAlive()
        {
            base.tickAlive();

            switch (entityState)
            {
                case 0:
                    if (canStartTask())
                    {
                        effectWeather();
                        if (level.getRandomInteger(10) == 0)
                        {
                            if (getTarget() == null)
                            {
                                if (hadTarget) hadTarget = false;
                                LivingEntity foundTarget = detectTargetManhattan(1000);
                                if (foundTarget != null)
                                {
                                    target = foundTarget;
                                    hadTarget = true;
                                }
                            }
                            isMoving = !isMoving || getTarget() != null;
                        }

                        if (isMoving)
                        {
                            if (tickCount % 4 == 0)
                            {
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

                    if (deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z > 4)
                    {
                        walkTicks++;
                        isActuallyMoving = true;
                        if (walkTicks == 2) Image = (int)direction < 5 ? images[4] : images[12];
                        else if (walkTicks == 4) Image = (int)direction < 5 ? images[5] : images[13];
                        else if (walkTicks == 6) Image = (int)direction < 5 ? images[6] : images[14];
                        else if (walkTicks == 8) Image = (int)direction < 5 ? images[7] : images[15];
                        else if (walkTicks > 8) { Image = (int)direction < 5 ? images[0] : images[2]; walkTicks = 0; }
                    }
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
                    break;

                case 1:
                    direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    walkTicks++;
                    switch (walkTicks)
                    {
                        case 1:
                        case 2:
                            Image = (int)direction < 5 ? images[12] : images[20];
                            break;
                        case 3:
                        case 4:
                            Image = (int)direction < 5 ? images[13] : images[21];
                            break;
                        case 5:
                        case 6:
                            Image = (int)direction < 5 ? images[14] : images[22];
                            break;
                        case 7:
                            Image = (int)direction < 5 ? images[15] : images[23];
                            Vector3 targetVec = Vector3.Normalize(new Vector3(target.x - x, (target.y - y) * 1.8f, target.z - z));
                            float distance = (float)Math.Cbrt((target.x - x) * (target.x - x) + (target.y - y) * (target.y - y) + (target.z - z) * (target.z - z)) * 1.4f;

                            MushroomSpore spore = new MushroomSpore(level);
                            spore.Owner = this;
                            spore.attackDamage = finalAttackDamage;
                            spore.pushPower = pushPower;
                            spore.x = x;
                            spore.y = y + height;
                            spore.z = z;
                            spore.deltaMovement = targetVec * distance;
                            spore.team = team;
                            for (int j = 0; j < entityLevel; j++) spore.scaleEntity(1.2f);
                            level.addFreshEntity(spore);
                            break;
                        case 13:
                            Image = (int)direction < 5 ? images[18] : images[26];
                            break;
                        case 17:
                            Image = (int)direction < 5 ? images[19] : images[27];
                            break;
                        case 20:
                            walkTicks = 0;
                            entityState = 0;
                            break;
                    }
                    break;

                case 2: // 죽는 중
                    walkTicks++;
                    if (walkTicks == 3) Image = Properties.Resources.sprite_Mushroom1_death1;
                    else if (walkTicks == 6) Image = Properties.Resources.sprite_Mushroom1_death2;
                    else if (walkTicks == 9) Image = Properties.Resources.sprite_Mushroom1_death3;
                    else if (walkTicks == 12) Image = Properties.Resources.sprite_Mushroom1_death4;
                    else if (walkTicks > 15) discard();
                    break;
            }
        }

        public override void setDeath(object? sender, EventArgs e)
        {
            entityState = 2; // 죽음 애니메이션 전용 상태
            walkTicks = 0;
        }
    }
}
