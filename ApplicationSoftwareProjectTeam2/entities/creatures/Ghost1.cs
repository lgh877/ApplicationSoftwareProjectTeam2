using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class Ghost1 : LivingEntity
    {
        public static List<Image> images = new List<Image>()
        {
            // 0~3: idle
            Properties.Resources.sprite_Ghost1_idle1,
            Properties.Resources.sprite_Ghost1_idle2,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_idle1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_idle2),

            // 4~11: walk
            Properties.Resources.sprite_Ghost1_walk1,
            Properties.Resources.sprite_Ghost1_walk2,
            Properties.Resources.sprite_Ghost1_walk3,
            Properties.Resources.sprite_Ghost1_walk4,
            Properties.Resources.sprite_Ghost1_walk5,
            Properties.Resources.sprite_Ghost1_walk6,
            Properties.Resources.sprite_Ghost1_walk7,
            Properties.Resources.sprite_Ghost1_walk8,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_walk1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_walk2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_walk3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_walk4),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_walk5),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_walk6),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_walk7),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_walk8),

            // 12~27: attack (오른쪽 12~19, 왼쪽 20~27)
            Properties.Resources.sprite_Ghost1_attack1,
            Properties.Resources.sprite_Ghost1_attack2,
            Properties.Resources.sprite_Ghost1_attack3,
            Properties.Resources.sprite_Ghost1_attack4,
            Properties.Resources.sprite_Ghost1_attack5,
            Properties.Resources.sprite_Ghost1_attack6,
            Properties.Resources.sprite_Ghost1_attack7,
            Properties.Resources.sprite_Ghost1_attack8,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_attack1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_attack2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_attack3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_attack4),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_attack5),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_attack6),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_attack7),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_attack8)
        };

        public Ghost1(GamePanel level) : base(level)
        {
            cost = 2;
            visualSize = 1.5f;
            width = 35;
            height = 65;
            weight = 8;
            pushPower = 20;
            Image = images[0];
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 100;
            currentHealth = 100;
            finalMaxHealth = maxHealth;
            attackDamage = 40;
            finalAttackDamage = attackDamage;
            moveSpeed = 4;
        }

        public override EntityTypes getEntityType() => EntityTypes.Avengers;
        public override byte getLivingEntityId() => 7;

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

                    // 애니메이션 처리
                    if (deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z > 4)
                    {
                        walkTicks++;
                        int frame = (walkTicks / 4) % 8;
                        Image = direction == Direction.Right ? images[4 + frame] : images[12 + frame];
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
                    int atkFrame = walkTicks / 2;

                    if (atkFrame >= 8)
                    {
                        walkTicks = 0;
                        entityState = 0;
                    }
                    else
                    {
                        Image = direction == Direction.Right ? images[12 + atkFrame] : images[20 + atkFrame];

                        if (atkFrame == 4 && target != null &&
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
            Image = Properties.Resources.sprite_Ghost1_idle1;
        }
    }
}
