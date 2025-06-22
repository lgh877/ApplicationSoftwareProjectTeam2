using System;
using System.Collections.Generic;
using System.Numerics;
using ApplicationSoftwareProjectTeam2.entities.projectiles;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class FlyingEye1 : LivingEntity
    {
        public static List<Image> images = new List<Image>()
        {
            // idle/walk 공유 (0~15)
            Properties.Resources.sprite_FlyingEye1_idlewalk1,
            Properties.Resources.sprite_FlyingEye1_idlewalk2,
            Properties.Resources.sprite_FlyingEye1_idlewalk3,
            Properties.Resources.sprite_FlyingEye1_idlewalk4,
            Properties.Resources.sprite_FlyingEye1_idlewalk5,
            Properties.Resources.sprite_FlyingEye1_idlewalk6,
            Properties.Resources.sprite_FlyingEye1_idlewalk7,
            Properties.Resources.sprite_FlyingEye1_idlewalk8,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_idlewalk1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_idlewalk2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_idlewalk3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_idlewalk4),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_idlewalk5),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_idlewalk6),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_idlewalk7),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_idlewalk8),

            // attack (16 ~ 29)
            Properties.Resources.sprite_FlyingEye1_attack1,
            Properties.Resources.sprite_FlyingEye1_attack2,
            Properties.Resources.sprite_FlyingEye1_attack3,
            Properties.Resources.sprite_FlyingEye1_attack4,
            Properties.Resources.sprite_FlyingEye1_attack5,
            Properties.Resources.sprite_FlyingEye1_attack6,
            Properties.Resources.sprite_FlyingEye1_attack7,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_attack1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_attack2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_attack3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_attack4),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_attack5),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_attack6),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_FlyingEye1_attack7),
        };

        public FlyingEye1(GamePanel level) : base(level)
        {
            cost = 4;
            visualSize = 1.4f;
            width = 40;
            height = 40;
            weight = 4;
            pushPower = 20;
            Image = images[0];
            direction = level.usualRandom.Next(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 60;
            currentHealth = 60;
            finalMaxHealth = maxHealth;
            attackDamage = 20;
            finalAttackDamage = attackDamage;
            moveSpeed = 4;
        }

        public override byte getLivingEntityId() => (byte) 21;
        public override EntityTypes getEntityType() => EntityTypes.Avengers;

        public override void tickAlive()
        {
            base.tickAlive();

            switch (entityState)
            {
                case 0:
                    if (canStartTask())
                    {
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
                                    if (level.getRandomInteger(3) == 0 && (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 250000)
                                    {
                                        entityState = 1;
                                        walkTicks = 0;
                                    }
                                    if(entityState == 0)
                                    {
                                        int dir = (int)direction;
                                            Vector3 targetVec = Vector3.Normalize(new Vector3(target.x - x, 0, target.z - z));
                                        if (targetVec.X > 0 ^ (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 250000)
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

                    walkTicks++;
                    Image = (int)direction < 5 ? images[walkTicks / 4 % 8] : images[8 + walkTicks / 4 % 8]; // idle/walk 애니 공유
                    break;

                case 1:
                    direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    walkTicks++;
                    int atkFrame = walkTicks / 2;
                    if (atkFrame < 7)
                        Image = (int)direction < 5 ? images[16 + atkFrame] : images[23 + atkFrame]; // attack1~7
                    else
                    {
                        walkTicks = 0;
                        entityState = 0;
                    }
                    if (walkTicks == 10)
                    {
                        Vector3 targetVec = Vector3.Normalize(new Vector3(target.x - x, target.y - y - height, target.z - z));
                        FlyingEyeShot shot = new FlyingEyeShot(level);
                        shot.Owner = this;
                        shot.attackDamage = finalAttackDamage * 0.06f;
                        shot.pushPower = pushPower;
                        shot.x = x;
                        shot.y = y + height;
                        shot.z = z;
                        shot.deltaMovement = targetVec * 10;
                        shot.team = team;
                        shot.scaleEntity((float) Math.Pow(1.2f, entityLevel + 1));
                        level.addFreshEntity(shot);
                    }
                    break;
            }
        }
        public override void tickDeath()
        {
            base.tickDeath();
            if (deathTime > 9) return;
            if (deathTime == 0)
            {
                Image = Properties.Resources.sprite_FlyingEye1_death1;
            }
            else if (deathTime == 3)
            {
                Image = Properties.Resources.sprite_FlyingEye1_death2;
            }
            else if (deathTime == 6)
            {
                Image = Properties.Resources.sprite_FlyingEye1_death3;
            }
            else if (deathTime == 9)
            {
                Image = Properties.Resources.sprite_FlyingEye1_death4;
            }
        }
    }
}
