using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.weirdos
{
    public class WeirdGuy : LivingEntity
    {
        private int walkTicks, mana;
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.weirdGuy_idle1,
            Properties.Resources.weirdGuy_idle2,
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_idle1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_idle2),
            //0 ~ 3번 인덱스
            Properties.Resources.weirdGuy_walk1,
            Properties.Resources.weirdGuy_walk2,
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_walk1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_walk2),
            //4 ~ 7번 인덱스
            Properties.Resources.weirdGuy_died,
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_died),
            //8 ~ 9번 인덱스
            Properties.Resources.weirdGuy_meleeattack1,
            Properties.Resources.weirdGuy_meleeattack2,
            Properties.Resources.weirdGuy_meleeattack3,
            Properties.Resources.weirdGuy_meleeattack4,
            Properties.Resources.weirdGuy_meleeattack5,
            Properties.Resources.weirdGuy_meleeattack6,
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_meleeattack1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_meleeattack2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_meleeattack3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_meleeattack4),
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_meleeattack5),
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_meleeattack6)
            //10 ~ 21번 인덱스
        };
        public WeirdGuy(GamePanel level) : base(level)
        {
            visualSize = 1f; width = 40; height = 70; weight = 10; pushPower = 30;
            Image = images[0];
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            maxHealth = 100;  currentHealth = 100;
            attackDamage = 50;
            moveSpeed = 3;
            mana = 0;
        }
        public override void scaleEntity(float scale)
        {
            base.scaleEntity(scale);
            maxHealth *= scale; currentHealth = maxHealth;
            attackDamage *= scale; pushPower = (int)(pushPower * scale); moveSpeed = (int)(moveSpeed * Math.Sqrt(scale));
        }
        public override EntityTypes getEntityType()
        {
            return EntityTypes.Weirdos;
        }
        public override void tickAlive()
        {
            base.tickAlive();
            switch (entityState)
            {
                case 0:
                    if (hasAi)
                    {
                        if (level.getRandomInteger(10) == 0)
                        {
                            if (getTarget() == null)
                            {
                                if (hadTarget)
                                {
                                    hadTarget = false;
                                }
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
                                mana++;
                                if (getTarget() == null)
                                {
                                    int dir = (int)direction;
                                    dir = dir + (level.getRandomInteger(2) == 0 ? 1 : -1);
                                    dir = dir > 9 ? 0 : dir < 0 ? 9 : dir;
                                    direction = (Direction)dir;
                                }
                                else
                                {
                                    float ydiff = target.y - y;

                                    //공격 범위 안에 들어왔는지 확인
                                    if (mana < 20)
                                    {
                                        if ((target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.25 * (width * width + (target.width + width) * (target.width + width))
                                        && ydiff < height
                                        && ydiff > -target.height)
                                        {
                                            entityState = 1;
                                            walkTicks = 0;
                                        }
                                    }
                                    else
                                    {
                                        if ((target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < width * width + 0.25 * (target.width + width) * (target.width + width)
                                        && ydiff < height
                                        && ydiff > -target.height)
                                        {
                                            mana = 0;
                                            entityState = 2;
                                            walkTicks = 0;
                                        }
                                    }

                                    //공격 시도가 실패한 경우 상대 방향으로 이동
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

                        //걷는 애니메이션
                        switch (walkTicks)
                        {
                            case 2:
                                Image = (int)direction < 5 ? images[4] : images[6];
                                break;
                            case 4:
                                Image = (int)direction < 5 ? images[5] : images[7];
                                break;
                            case 6:
                                Image = (int)direction < 5 ? images[4] : images[6];
                                break;
                            case 8:
                                Image = (int)direction < 5 ? images[0] : images[2];
                                walkTicks = 0;
                                break;
                        }
                    }
                    else
                    {
                        if (isActuallyMoving)
                        {
                            isActuallyMoving = false;
                            Image = (int)direction < 5 ? images[0] : images[2];
                        }
                        walkTicks = 0;
                        switch (tickCount % 16)
                        {
                            case 0:
                                Image = (int)direction < 5 ? images[0] : images[2];
                                break;
                            case 8:
                                Image = (int)direction < 5 ? images[1] : images[3];
                                break;
                        }
                    }
                    break;
                case 1:
                    direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    walkTicks++;
                    switch (walkTicks)
                    {
                        case 1:
                            Image = (int)direction < 5 ? images[10] : images[16];
                            break;
                        case 3:
                            Image = (int)direction < 5 ? images[11] : images[17];
                            break;
                        case 5:
                            Image = (int)direction < 5 ? images[12] : images[18];
                            break;
                        case 7:
                            Image = (int)direction < 5 ? images[13] : images[19];
                            if (target != null && (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.25 * (width * width + (target.width + width) * (target.width + width))
                                && target.y - y < height && target.y - y > -target.height)
                            {
                                doHurtTarget(target);
                            }
                            break;
                        case 9:
                            Image = (int)direction < 5 ? images[14] : images[20];
                            break;
                        case 11:
                            Image = (int)direction < 5 ? images[15] : images[21];
                            break;
                        case 13:
                            walkTicks = 0;
                            entityState = 0;
                            break;
                    }
                    break;
                case 2:
                    direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    walkTicks++;
                    switch (walkTicks)
                    {
                        case 1:
                            Image = (int)direction < 5 ? images[0] : images[2];
                            break;
                        case 3:
                            Image = (int)direction < 5 ? images[15] : images[21];
                            break;
                        case 5:
                            Image = (int)direction < 5 ? images[10] : images[16];
                            break;
                        case 7:
                            Image = (int)direction < 5 ? images[11] : images[17];
                            float distance = (float)Math.Cbrt((target.x - x) * (target.x - x) + (target.z - z) * (target.z - z));
                            push(0, moveSpeed * 5, 0); // 위로 점프
                            move((int) Math.Min(moveSpeed * 10, distance)); //돌진
                            break;
                        case 9:
                            Image = (int)direction < 5 ? images[12] : images[18];
                            break;
                        case 11:
                            Image = (int)direction < 5 ? images[13] : images[19];
                            push(0, -moveSpeed * 5, 0);
                            if (target != null && (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.25 * (width * width + (target.width + width) * (target.width + width))
                                && target.y - y < height && target.y - y > -target.height)
                            {
                                doHurtTarget(target, attackDamage * 1.5f, pushPower * 3);
                            }
                            break;
                        case 13:
                            Image = (int)direction < 5 ? images[14] : images[20];
                            break;
                        case 15:
                            Image = (int)direction < 5 ? images[15] : images[21];
                            break;
                        case 17:
                            walkTicks = 0;
                            entityState = 0;
                            break;
                    }
                    break;
            }
        }
        public override void tickDeath()
        {
            base.tickDeath();
            if (deathTime == 1) { width *= 2; height /= 2; Image = (int)direction < 5 ? images[8] : images[9]; }
        }
    }
}
