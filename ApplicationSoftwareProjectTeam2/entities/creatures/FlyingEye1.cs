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
            // idle/walk 공유 (0~3)
            Properties.Resources.sprite_FlyingEye1_idlewalk1,
            Properties.Resources.sprite_FlyingEye1_idlewalk2,
            Properties.Resources.sprite_FlyingEye1_idlewalk3,
            Properties.Resources.sprite_FlyingEye1_idlewalk4,
            Properties.Resources.sprite_FlyingEye1_idlewalk5,
            Properties.Resources.sprite_FlyingEye1_idlewalk6,
            Properties.Resources.sprite_FlyingEye1_idlewalk7,
            Properties.Resources.sprite_FlyingEye1_idlewalk8,

            // attack (8~14)
            Properties.Resources.sprite_FlyingEye1_attack1,
            Properties.Resources.sprite_FlyingEye1_attack2,
            Properties.Resources.sprite_FlyingEye1_attack3,
            Properties.Resources.sprite_FlyingEye1_attack4,
            Properties.Resources.sprite_FlyingEye1_attack5,
            Properties.Resources.sprite_FlyingEye1_attack6,
            Properties.Resources.sprite_FlyingEye1_attack7
        };

        public FlyingEye1(GamePanel level) : base(level)
        {
            cost = 2;
            visualSize = 1.4f;
            width = 40;
            height = 40;
            weight = 4;
            pushPower = 10;
            Image = images[0];
            direction = level.usualRandom.Next(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 80;
            currentHealth = 80;
            finalMaxHealth = maxHealth;
            attackDamage = 45;
            finalAttackDamage = attackDamage;
            moveSpeed = 4;
        }

        public override byte getLivingEntityId() => 12;
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
                                    int dir = (int)direction;
                                    dir = dir + (level.getRandomInteger(2) == 0 ? 1 : -1);
                                    dir = dir > 9 ? 0 : dir < 0 ? 9 : dir;
                                    direction = (Direction)dir;
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
                                    if ((target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 90000)
                                    {
                                        entityState = 1;
                                        walkTicks = 0;
                                    }
                                }
                            }

                            if (isOnGround()) move(moveSpeed);
                        }
                    }

                    walkTicks++;
                    Image = images[walkTicks / 4 % 8]; // idle/walk 애니 공유
                    break;

                case 1:
                    direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    walkTicks++;

                    if (walkTicks >= 1 && walkTicks <= 7)
                        Image = images[7 + walkTicks]; // attack1~7

                    if (walkTicks == 5)
                    {
                        Vector3 targetVec = Vector3.Normalize(new Vector3(target.x - x, (target.y - y) * 1.5f, target.z - z));
                        float dist = (float)Math.Cbrt((target.x - x) * (target.x - x) + (target.z - z) * (target.z - z)) * 1.2f;

                        FlyingEyeShot shot = new FlyingEyeShot(level);
                        shot.Owner = this;
                        shot.attackDamage = finalAttackDamage;
                        shot.pushPower = pushPower;
                        shot.x = x;
                        shot.y = y + height / 2;
                        shot.z = z;
                        shot.deltaMovement = targetVec * dist;
                        shot.team = team;
                        for (int j = 0; j < entityLevel; j++) shot.scaleEntity(1.2f);
                        level.addFreshEntity(shot);
                    }

                    if (walkTicks > 12)
                    {
                        walkTicks = 0;
                        entityState = 0;
                    }
                    break;

                case 2: // 사망
                    walkTicks++;
                    if (walkTicks == 3) Image = Properties.Resources.sprite_FlyingEye1_death1;
                    else if (walkTicks == 6) Image = Properties.Resources.sprite_FlyingEye1_death2;
                    else if (walkTicks == 9) Image = Properties.Resources.sprite_FlyingEye1_death3;
                    else if (walkTicks == 12) Image = Properties.Resources.sprite_FlyingEye1_death4;
                    else if (walkTicks > 15) discard();
                    break;
            }
        }

        public override void setDeath(object? sender, EventArgs e)
        {
            base.setDeath(sender, e);
            entityState = 2;
            walkTicks = 0;
        }
    }
}
