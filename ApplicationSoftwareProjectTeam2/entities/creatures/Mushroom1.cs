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
            // walk (4~20)
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
            // attack (20~35)
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
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_attack8),
            // death (36~43)
            Properties.Resources.sprite_Mushroom1_death1,
            Properties.Resources.sprite_Mushroom1_death2,
            Properties.Resources.sprite_Mushroom1_death3,
            Properties.Resources.sprite_Mushroom1_death4,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_death1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_death2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_death3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Mushroom1_death4)
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
            direction = level.usualRandom.Next(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 120;
            currentHealth = 120; finalMaxHealth = maxHealth;
            attackDamage = 8; finalAttackDamage = attackDamage;
            moveSpeed = 3;
            maxDeathTime = 20;
        }
        public override void scaleEntity(float scale)
        {
            base.scaleEntity(scale);
            maxHealth *= scale * 1.4f; currentHealth = maxHealth * 1.4f;
            attackDamage *= scale * 1.4f; pushPower = (int)(pushPower * scale); moveSpeed = (int)(moveSpeed * Math.Sqrt(scale));
        }
        public override byte getLivingEntityId() => 23;
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
                                LivingEntity foundTarget = detectTargetManhattan(2000);
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
                                    if (tickCount % 16 == 0)
                                    {
                                        LivingEntity found = detectTargetManhattan(2000);
                                        if (found != null)
                                        {
                                            target = found;
                                            hadTarget = true;
                                        }
                                    }
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
                        Image = (int)direction < 5 ? images[4 + walkTicks % 8] : images[12 + walkTicks % 8];
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
                    if (walkTicks == 1) Image = (int)direction < 5 ? images[20] : images[28];
                    else if (walkTicks == 3) Image = (int)direction < 5 ? images[21] : images[29];
                    else if (walkTicks == 5) Image = (int)direction < 5 ? images[22] : images[30];
                    else if (walkTicks == 7) Image = (int)direction < 5 ? images[23] : images[31];
                    else if (walkTicks == 9) Image = (int)direction < 5 ? images[24] : images[32];
                    else if (walkTicks == 11) Image = (int)direction < 5 ? images[25] : images[33];
                    else if (walkTicks == 13)
                    {
                        Image = (int)direction < 5 ? images[26] : images[34];
                        float distance = (float)Math.Cbrt((target.x - x) * (target.x - x) + (target.y - y) * (target.y - y) + (target.z - z) * (target.z - z)) * 0.3f;
                        for(int i = 0; i < 4; i++)
                        {
                            Vector3 targetVec = Vector3.Normalize(new Vector3(target.x - x + level.getRandomInteger(51) - 25, (target.y - y) * 1.8f + level.getRandomInteger(51) - 25, target.z - z + level.getRandomInteger(51) - 25));
                            MushroomSpore spore = new MushroomSpore(level);
                            spore.Owner = this;
                            spore.attackDamage = finalAttackDamage;
                            spore.pushPower = pushPower;
                            spore.x = x;
                            spore.y = y + height;
                            spore.z = z;
                            spore.deltaMovement = targetVec * distance;
                            spore.team = team;
                            spore.scaleEntity((float) Math.Pow(1.2f,entityLevel));
                            level.addFreshEntity(spore);
                        }
                    }
                    else if (walkTicks == 15) Image = (int)direction < 5 ? images[27] : images[35];
                    else if (walkTicks == 17)
                    {
                        walkTicks = 0;
                        entityState = 0;
                    }
                    break;
            }
        }
        public override void tickDeath()
        {
            base.tickDeath();
            if (deathTime > 12) return;
            if (deathTime == 3) Image = (int)direction < 5 ? images[36] : images[40];
            else if (deathTime == 6) Image = (int)direction < 5 ? images[37] : images[41];
            else if (deathTime == 9) Image = (int)direction < 5 ? images[38] : images[42];
            else if (deathTime == 12) Image = (int)direction < 5 ? images[39] : images[43];
        }
    }
}
