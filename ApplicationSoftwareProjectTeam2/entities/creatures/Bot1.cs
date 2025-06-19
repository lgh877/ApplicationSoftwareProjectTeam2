using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class Bot1 : LivingEntity
    {
        public static List<Image> images = new List<Image>()
        {
            // 0~3: idle
            Properties.Resources.sprite_Bot1_idle1,
            Properties.Resources.sprite_Bot1_idle2,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Bot1_idle1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Bot1_idle2),

            // 4~7: walk
            Properties.Resources.sprite_Bot1_walk1,
            Properties.Resources.sprite_Bot1_walk2,
            Properties.Resources.sprite_Bot1_walk3,
            Properties.Resources.sprite_Bot1_walk4,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Bot1_walk1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Bot1_walk2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Bot1_walk3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Bot1_walk4),

            // 8~13: attack
            Properties.Resources.sprite_Bot1_attack1,
            Properties.Resources.sprite_Bot1_attack2,
            Properties.Resources.sprite_Bot1_attack3,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Bot1_attack1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Bot1_attack2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Bot1_attack3)
        };

        public Bot1(GamePanel level) : base(level)
        {
            cost = 4;
            visualSize = 1.5f;
            width = 40;
            height = 70;
            weight = 40;
            pushPower = 25;
            Image = images[0];
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 200;
            currentHealth = 200;
            finalMaxHealth = maxHealth;
            attackDamage = 30;
            finalAttackDamage = attackDamage;
            moveSpeed = 2;
        }

        public override EntityTypes getEntityType() => EntityTypes.Avengers;

        public override byte getLivingEntityId() => 6;

        public override void tickAlive()
        {
            base.tickAlive();

            switch (entityState)
            {
                case 0: // 기본 상태
                    if (canStartTask())
                    {
                        effectWeather();

                        // 타겟 초기화 및 탐색
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

                        // 타겟 추적 또는 랜덤 이동
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

                    // 애니메이션 처리
                    if (deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z > 4)
                    {
                        walkTicks++;
                        isActuallyMoving = true;

                        int frame = (walkTicks / 5) % 4;
                        Image = direction == Direction.Right ? images[4 + frame] : images[8 + frame];
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

                case 1: // 공격 상태
                    direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    walkTicks++;
                    switch (walkTicks)
                    {
                        case 1:
                            Image = (int)direction < 5 ? images[12] : images[15];
                            break;
                        case 3:
                            Image = (int)direction < 5 ? images[13] : images[16];
                            break;
                        case 5:
                            Image = (int)direction < 5 ? images[14] : images[17];
                            if (target != null &&
                                (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 2500 &&
                                target.y - y < height && target.y - y > -target.height)
                            {
                                doHurtTarget(target);
                            }
                            break;
                        case 7:
                            walkTicks = 0;
                            entityState = 0;
                            break;
                    }
                    break;
            }
        }

        public override void setDeath(object? sender, EventArgs e)
        {
            base.setDeath(sender, e);
            Image = Properties.Resources.sprite_Bot1_basic;
        }
    }
}
