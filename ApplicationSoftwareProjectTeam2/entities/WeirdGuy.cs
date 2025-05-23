using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities
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
            Properties.Resources.weirdGuy_walk1,
            Properties.Resources.weirdGuy_walk2,
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_walk1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_walk2),
            Properties.Resources.weirdGuy_died,
            ImageUtils.FlipImageHorizontally(Properties.Resources.weirdGuy_died),
        };
        public WeirdGuy(GamePanel level) : base(level)
        {
            visualSize = 44; width = 20; height = 44;
            Image = images[0];
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            currentHealth = 100;
            attackDamage = 50;
            moveSpeed = 3;
        }

        public override void tickAlive()
        {
            base.tickAlive();
            if(level.getRandomInteger(10) == 0)
            {
                isMoving = !isMoving;
                if(tickCount % 16 == 0)
                {
                    ProjectileEntity test = new ProjectileEntity(level);
                    test.setPosition(x, y + height, z);
                    test.team = team;
                    test.push(deltaMovement.X * 50, 30, deltaMovement.Z * 50);
                    level.addFreshEntity(test);
                }
            }
            if (isMoving)
            {
                int dir = (int)direction;
                dir = dir + (level.getRandomInteger(2) == 0 ? 1 : -1);
                dir = dir > 9 ? 0 : dir < 0 ? 9 : dir;
                direction = (Direction)dir;

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
            if((deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z) > 4)
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
        }
        public override void tickDeath()
        {
            base.tickDeath();
            if (deathTime == 1) { width = 40; Image = (int)direction < 5 ? images[8] : images[9]; }
        }
    }
}
