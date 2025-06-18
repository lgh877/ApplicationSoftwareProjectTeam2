using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class Human1 : LivingEntity
    {
        public static List<Image> images = new List<Image>()
        {
            // 0~3: idle
            Properties.Resources.sprite_Human1_idle1,
            Properties.Resources.sprite_Human1_idle2,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_idle1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_idle2),

            // 4~7: walk
            Properties.Resources.sprite_Human1_walk1,
            Properties.Resources.sprite_Human1_walk2,
            Properties.Resources.sprite_Human1_walk3,
            Properties.Resources.sprite_Human1_walk4,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_walk1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_walk2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_walk3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_walk4),

            // 12~14: attack
            Properties.Resources.sprite_Human1_attack1,
            Properties.Resources.sprite_Human1_attack2,
            Properties.Resources.sprite_Human1_attack3,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_attack1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_attack2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Human1_attack3)
        };

        public Human1(GamePanel level) : base(level)
        {
            cost = 2;
            visualSize = 1.2f;
            width = 45;
            height = 70;
            weight = 12;
            pushPower = 35;
            Image = images[0];
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 140;
            currentHealth = 140;
            finalMaxHealth = maxHealth;
            attackDamage = 30;
            finalAttackDamage = attackDamage;
            moveSpeed = 3;
        }

        public override EntityTypes getEntityType() => EntityTypes.Avengers;
        public override byte getLivingEntityId() => 8;

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

                    // walk or idle animation
                    if (deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z > 4)
                    {
                        walkTicks++;
                        int frame = (walkTicks / 5) % 4;
                        Image = direction == Direction.Right ? images[4 + frame] : images[8 + frame];
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

                    if (atkFrame >= 3)
                    {
                        walkTicks = 0;
                        entityState = 0;
                    }
                    else
                    {
                        Image = direction == Direction.Right ? images[12 + atkFrame] : images[15 + atkFrame];

                        if (atkFrame == 1 && target != null &&
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
            Image = Properties.Resources.sprite_Human1_basic;
        }
    }
}
