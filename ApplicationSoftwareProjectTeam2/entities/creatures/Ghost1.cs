using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using ApplicationSoftwareProjectTeam2.entities.eventArgs;
using ApplicationSoftwareProjectTeam2.resources.sounds;
using ApplicationSoftwareProjectTeam2.utils;
using WMPLib;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class Ghost1 : LivingEntity
    {
        private int atkFrame;
        public static List<WindowsMediaPlayer> sounds = new List<WindowsMediaPlayer>()
        {
            SoundCache.ghostSwing1, SoundCache.ghostSwing2, SoundCache.ghostTeleport1, SoundCache.ghostTeleport2,
        };
        public static List<Image> images = new List<Image>()
        {
            // 0~3: idle
            Properties.Resources.sprite_Ghost1_idle1,
            Properties.Resources.sprite_Ghost1_idle2,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_idle1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_idle2),

            // 4~19: walk
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
            
            // 20 ~ 35: attack
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
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_attack8),

            // 36 ~ 41: teleport
            Properties.Resources.sprite_Ghost1_teleport1,
            Properties.Resources.sprite_Ghost1_teleport2,
            Properties.Resources.sprite_Ghost1_teleport3,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_teleport1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_teleport2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_teleport3),

            // 42 ~ 51: death
            Properties.Resources.sprite_Ghost1_death1,
            Properties.Resources.sprite_Ghost1_death2,
            Properties.Resources.sprite_Ghost1_death3,
            Properties.Resources.sprite_Ghost1_death4,
            Properties.Resources.sprite_Ghost1_death5,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_death1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_death2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_death3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_death4),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_Ghost1_death5),
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
            direction = level.usualRandom.Next(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 60;
            currentHealth = 60;
            finalMaxHealth = maxHealth;
            attackDamage = 40;
            finalAttackDamage = attackDamage;
            moveSpeed = 4;
            maxDeathTime = 20;
        }

        public override EntityTypes getEntityType() => EntityTypes.Avengers;
        public override byte getLivingEntityId() => (byte) 22;

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
                                    mana += level.getRandomInteger(6);
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
                                    if (mana > 20)
                                    {
                                        mana = 0;
                                        level.playSound(sounds[level.getRandomInteger(2) + 2]);
                                        entityState = 2;
                                        walkTicks = 0;
                                        hurtEvent += disableDamage;
                                        renderType = 0;
                                    }
                                    else
                                    {
                                        if (distance < 0.5 * (width * width + (target.width + width) * (target.width + width)))
                                        {
                                            level.playSound(sounds[level.getRandomInteger(2)]);
                                            entityState = 1;
                                            walkTicks = 0;
                                        }
                                        else
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
                            }

                            if (isOnGround()) move(moveSpeed);
                        }
                    }

                    // 애니메이션 처리
                    if (deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z > 4)
                    {
                        walkTicks++;
                        int frame = (walkTicks / 2) % 8;
                        Image = (int)direction < 5 ? images[4 + frame] : images[12 + frame];
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
                    atkFrame = walkTicks / 2;

                    if (atkFrame == 8)
                    {
                        walkTicks = 0;
                        entityState = 0;
                    }
                    else
                    {
                        Image = direction == Direction.Right ? images[20 + atkFrame] : images[27 + atkFrame];

                        if (atkFrame == 4 && target != null &&
                            (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.6 * (width * width + (target.width + width) * (target.width + width)) &&
                            target.y - y < height && target.y - y > -target.height)
                        {
                            doHurtTarget(target);
                        }
                    }
                    break;
                case 2:
                    walkTicks++;
                    atkFrame = walkTicks / 3;
                    if(atkFrame < 3) {
                        Image = direction == Direction.Right ? images[36 + atkFrame] : images[39 + atkFrame];
                    }
                    else
                    {
                        if (walkTicks == 9) setPosition(level.getRandomInteger(1000) - 500, 2, level.getRandomInteger(500) + 200);
                        else if(atkFrame == 5)
                        {
                            entityState = 0;
                            walkTicks = 0;
                            hurtEvent -= disableDamage;
                            renderType = 1;
                        }
                        Image = direction == Direction.Right ? images[41 - atkFrame] : images[44 - atkFrame];
                    }
                    break;
            }
        }
        private void disableDamage(Object? sender, AttackEventArgs e)
        {
            e.damage = 0;
        }
        public override void tickDeath()
        {
            base.tickDeath();
            if (deathTime > 11) return;
            if (deathTime == 3) Image = (int)direction < 5 ? images[42] : images[47];
            else if (deathTime == 5) Image = (int)direction < 5 ? images[43] : images[48];
            else if (deathTime == 7) Image = (int)direction < 5 ? images[44] : images[49];
            else if (deathTime == 9) Image = (int)direction < 5 ? images[45] : images[50];
            else if (deathTime == 11) Image = (int)direction < 5 ? images[46] : images[51];
        }
    }
}
