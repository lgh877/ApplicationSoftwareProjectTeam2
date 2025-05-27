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
        private int walkTicks;
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
            visualSize = 1; width = 40; height = 88; weight = 10;
            Image = images[0];
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            currentHealth = 100;
            attackDamage = 50;
            moveSpeed = 3;
        }

        public override void tickAlive()
        {
            base.tickAlive();
            switch (entityState)
            {
                case 0:
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
                                if ((target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 256 + (target.width + width) * (target.width + width) * 0.5
                                    && ydiff < height
                                    && ydiff > -target.height)
                                {
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

                        //이동 코드
                        switch (direction)
                        {
                            case Direction.UpperRight:
                                push(moveSpeed * 0.2588f, 0, moveSpeed * 0.9659f);
                                break;
                            case Direction.UpRight:
                                push(moveSpeed * 0.7071f, 0, moveSpeed * 0.7071f);
                                break;
                            case Direction.Right:
                                push(moveSpeed, 0, 0);
                                break;
                            case Direction.DownRight:
                                push(moveSpeed * 0.7071f, 0, -moveSpeed * 0.7071f);
                                break;
                            case Direction.LowerRight:
                                push(moveSpeed * 0.2588f, 0, -moveSpeed * 0.9659f);
                                break;
                            case Direction.UpperLeft:
                                push(-moveSpeed * 0.2588f, 0, moveSpeed * 0.9659f);
                                break;
                            case Direction.UpLeft:
                                push(-moveSpeed * 0.7071f, 0, moveSpeed * 0.7071f);
                                break;
                            case Direction.Left:
                                push(-moveSpeed, 0, 0);
                                break;
                            case Direction.DownLeft:
                                push(-moveSpeed * 0.7071f, 0, -moveSpeed * 0.7071f);
                                break;
                            case Direction.LowerLeft:
                                push(-moveSpeed * 0.2588f, 0, -moveSpeed * 0.9659f);
                                break;
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
                            if (target != null && (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 256 + (target.width + width) * (target.width + width) * 0.5
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
            }
            
        }

        public override void tickDeath()
        {
            base.tickDeath();
            if (deathTime == 1) { width *= 2; height /= 2; Image = (int)direction < 5 ? images[8] : images[9]; }
        }
    }
}
