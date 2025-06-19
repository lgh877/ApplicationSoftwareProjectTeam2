using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using ApplicationSoftwareProjectTeam2.resources.sounds;
using ApplicationSoftwareProjectTeam2.utils;
using WMPLib;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class Boxer : LivingEntity
    {
        public static List<WindowsMediaPlayer> sounds = new List<WindowsMediaPlayer>()
        {
            SoundCache.boxingPunch1, SoundCache.boxingPunch2, SoundCache.boxingPunch3, SoundCache.boxingPunch4,
        };
        public static List<Image> images = new List<Image>()
        {
            // 0~3: idle
            Properties.Resources.sprite_Human1_idle1,
            Properties.Resources.sprite_Human1_idle2,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_idle1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_idle2),

            // 4~11: walk
            Properties.Resources.sprite_Human1_walk1,
            Properties.Resources.sprite_Human1_walk2,
            Properties.Resources.sprite_Human1_walk3,
            Properties.Resources.sprite_Human1_walk4,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_walk1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_walk2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_walk3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_walk4),

            // 12~19: attack
            Properties.Resources.sprite_Human1_attack1,
            Properties.Resources.sprite_Human1_attack2,
            Properties.Resources.sprite_Human1_attack4,
            Properties.Resources.sprite_Human1_basic,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_attack1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_attack2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_attack4),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_basic),

            Properties.Resources.sprite_Human1_death,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_death)
        };

        public Boxer(GamePanel level) : base(level)
        {
            cost = 2;
            visualSize = 1.2f;
            width = 45;
            height = 70;
            weight = 12;
            pushPower = 35;
            Image = images[0];
            direction = level.usualRandom.Next(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 90;
            currentHealth = 90;
            finalMaxHealth = maxHealth;
            attackDamage = 25;
            finalAttackDamage = attackDamage;
            moveSpeed = 3;
        }

        public override EntityTypes getEntityType() => EntityTypes.Avengers;
        public override byte getLivingEntityId() => (byte) 20;

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

                                LivingEntity found = detectTargetManhattan(2000);
                                if (found != null)
                                {
                                    target = found;
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
                                    direction = (Direction)level.getRandomInteger(10);
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
                                    float dx = target.x - x;
                                    float dz = target.z - z;
                                    float distance = dx * dx + dz * dz;

                                    direction = dx >= 0 ? Direction.Right : Direction.Left;

                                    if (distance < 0.4 * (width * width + (target.width + width) * (target.width + width)))
                                    {
                                        entityState = 1;
                                        walkTicks = 0;
                                    }
                                    else
                                    {
                                        int dir = (int)direction;
                                        Vector3 targetVec = Vector3.Normalize(new Vector3(dx, 0, dz));

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

                    // walk or idle animation
                    if (deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z > 4)
                    {
                        walkTicks++;
                        int frame = (walkTicks / 3) % 4;
                        Image = (int)direction < 5 ? images[4 + frame] : images[8 + frame];
                    }
                    else
                    {
                        walkTicks = tickCount % 16;
                        if (walkTicks == 0) Image = (int)direction < 5 ? images[0] : images[2];
                        else if (walkTicks == 8) Image = (int)direction < 5 ? images[1] : images[3];
                    }
                    break;

                case 1:
                    direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    walkTicks++;
                    int atkFrame = walkTicks / 3;
                    if (atkFrame == 4)
                    {
                        walkTicks = 0;
                        entityState = 0;
                    }
                    else
                    {
                        Image = direction == Direction.Right ? images[12 + atkFrame] : images[16 + atkFrame];
                        if (atkFrame == 3)
                        {
                            level.playSound(sounds[level.getRandomInteger(4)]);
                            if (target != null &&
                            (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.4 * (width * width + (target.width + width) * (target.width + width)) &&
                            target.y - y < height && target.y - y > -target.height)
                            {
                                doHurtTarget(target);
                            }
                        }
                    }
                    break;
            }
        }

        public override void setDeath(object? sender, EventArgs e)
        {
            base.setDeath(sender, e);
            width = (int)(width * 1.5); height /= 3; Image = (int)direction < 5 ? images[20] : images[21];
        }
    }
}
