using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class Human2 : LivingEntity
    {
        public static List<Image> images = new List<Image>()
        {
            // 0~3: idle
            Properties.Resources.sprite_Knight1_idle1,
            Properties.Resources.sprite_Knight1_idle2,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_idle1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_idle2),

            // 4~13: walk (10장)
            Properties.Resources.sprite_Knight1_walk1,
            Properties.Resources.sprite_Knight1_walk2,
            Properties.Resources.sprite_Knight1_walk3,
            Properties.Resources.sprite_Knight1_walk4,
            Properties.Resources.sprite_Knight1_walk5,
            Properties.Resources.sprite_Knight1_walk6,
            Properties.Resources.sprite_Knight1_walk7,
            Properties.Resources.sprite_Knight1_walk8,
            Properties.Resources.sprite_Knight1_walk9,
            Properties.Resources.sprite_Knight1_walk10,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_walk1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_walk2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_walk3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_walk4),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_walk5),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_walk6),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_walk7),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_walk8),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_walk9),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_walk10),

            // 24~27: attack
            Properties.Resources.sprite_Knight1_attack1,
            Properties.Resources.sprite_Knight1_attack2,
            Properties.Resources.sprite_Knight1_attack3,
            Properties.Resources.sprite_Knight1_attack4,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_attack1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_attack2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_attack3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Knight1_attack4)
        };

        public Human2(GamePanel level) : base(level)
        {
            cost = 3;
            visualSize = 2f;
            width = 50;
            height = 75;
            weight = 15;
            pushPower = 40;
            Image = images[0];
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 200;
            currentHealth = 200;
            finalMaxHealth = maxHealth;
            attackDamage = 35;
            finalAttackDamage = attackDamage;
            moveSpeed = 3;
        }

        public override EntityTypes getEntityType() => EntityTypes.Avengers;
        public override byte getLivingEntityId() => 9;

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

                                LivingEntity found = detectTargetManhattan(1000);
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
                                    if (tickCount % 8 == 0)
                                    {
                                        LivingEntity found = detectTargetManhattan(1000);
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

                                    if (distance < 2500)
                                    {
                                        entityState = 1;
                                        walkTicks = 0;
                                    }
                                    else
                                    {
                                        Vector3 moveVec = Vector3.Normalize(new Vector3(dx, 0, dz));
                                        push(moveVec.X * moveSpeed, 0, moveVec.Z * moveSpeed);
                                    }
                                }
                            }

                            if (isOnGround()) move(moveSpeed);
                        }
                    }

                    // 걷기 or idle 애니메이션
                    if (deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z > 4)
                    {
                        walkTicks++;
                        int frame = (walkTicks / 4) % 10;
                        Image = direction == Direction.Right ? images[4 + frame] : images[14 + frame];
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
                    int atkFrame = walkTicks / 4;

                    if (atkFrame >= 4)
                    {
                        walkTicks = 0;
                        entityState = 0;
                    }
                    else
                    {
                        Image = direction == Direction.Right ? images[24 + atkFrame] : images[28 + atkFrame];

                        if (atkFrame == 2 && target != null &&
                            (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 2500 &&
                            target.y - y < height && target.y - y > -target.height)
                        {
                            doHurtTarget(target);
                        }
                    }
                    break;
            }
        }

        public override void setDeath(object? sender, EventArgs e)
        {
            base.setDeath(sender, e);
            Image = Properties.Resources.sprite_Knight1_idle1;
        }
    }
}
