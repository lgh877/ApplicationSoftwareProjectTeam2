using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class GiantWeirdGuy : LivingEntity
    {
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.giantWeirdGuyHead,
            Properties.Resources.giantWeirdGuyLowerBody,
            Properties.Resources.giantWeirdGuyUpperBody,
            Properties.Resources.giantWeirdGuyArm,
            Properties.Resources.giantWeirdGuyLeftLeg,
            Properties.Resources.giantWeirdGuyRightLeg,
            Properties.Resources.giantWeirdGuyArmPrepares,
            Properties.Resources.giantWeirdGuyArmRelease,
            Properties.Resources.giantWeirdGuyHeadDeath,
            ImageUtils.FlipImageHorizontally(Properties.Resources.giantWeirdGuyHead),
            ImageUtils.FlipImageHorizontally(Properties.Resources.giantWeirdGuyLowerBody),
            ImageUtils.FlipImageHorizontally(Properties.Resources.giantWeirdGuyUpperBody),
            ImageUtils.FlipImageHorizontally(Properties.Resources.giantWeirdGuyArm),
            ImageUtils.FlipImageHorizontally(Properties.Resources.giantWeirdGuyLeftLeg),
            ImageUtils.FlipImageHorizontally(Properties.Resources.giantWeirdGuyRightLeg),
            ImageUtils.FlipImageHorizontally(Properties.Resources.giantWeirdGuyArmPrepares),
            ImageUtils.FlipImageHorizontally(Properties.Resources.giantWeirdGuyArmRelease),
            ImageUtils.FlipImageHorizontally(Properties.Resources.giantWeirdGuyHeadDeath),
        };
        public List<PartEntity> parts = new List<PartEntity>(7);
        public GiantWeirdGuy(GamePanel level) : base(level)
        {
            this.visualSize = 1.0f;
            cost = 4;
            this.width = 50;
            this.height = 78;
            this.weight = 30;
            maxHealth = 300; currentHealth = 200; finalMaxHealth = maxHealth;
            this.pushPower = 20; moveSpeed = 3;
            this.renderType = 2; // default shadow
            direction = level.getRandomInteger(2) == 0 ? Direction.Right : Direction.Left;
            parts.Add(new PartEntity(level, this, -21, 44, 3) { Image = images[0] }); // Head
            parts.Add(new PartEntity(level, this, 5, 30, 5) { Image = images[2] }); // Upper Body
            parts.Add(new PartEntity(level, this, -25, 10, 14) { Image = images[3] }); // LeftArm
            parts.Add(new PartEntity(level, this, 21, 10, 0) { Image = images[3] }); // RightArm
            parts.Add(new PartEntity(level, this, 5, 18, 6) { Image = images[1] }); // Lower Body
            parts.Add(new PartEntity(level, this, -19, 0, 12) { Image = images[4] }); // Left Leg
            parts.Add(new PartEntity(level, this, 15, 0, 2) { Image = images[5] }); // Right Leg
            foreach (PartEntity part in parts)
            {
                level.addFreshEntity(part);
            }
            scaleEntity(1.5f);
        }
        #region 캐릭터 아이디 기록
        public override byte getLivingEntityId()
        {
            return 2;
        }
        #endregion
        public override void tickAlive()
        {
            base.tickAlive();
            switch (entityState)
            {
                case 0:
                    if (canStartTask())
                    {
                        effectWeather(); // 날씨 효과 적용
                        #region 평상시에 아무렇게 걸어다니기 + 타겟 탐색
                        if (level.getRandomInteger(10) == 0)
                        {
                            isMoving = !isMoving || getTarget() != null;
                        }
                        #endregion
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
                            }

                            if (isOnGround()) move(moveSpeed);
                        }
                    }
                    #region 캐릭터가 실제로 움직이고 있는 지 결정하는 부분으로, 애니매이션 재생에 쓰임
                    if (deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z > 4)
                    {
                        walkTicks++;
                        isActuallyMoving = true;

                        float f = (float)Math.Sin((double)tickCount * 0.5);
                        float f3 = (float)Math.Cos((double)tickCount * 0.5);
                        float f2 = (float)Math.Sin((double)tickCount);
                        if ((int)direction < 5)
                        {
                            for (int i = 0; i < parts.Count; i++)
                            {
                                PartEntity pe = parts[i];
                                switch (i)
                                {
                                    case 0:
                                        pe.Image = images[0];
                                        pe.offsetX = 0;
                                        pe.offsetY = f2 * 2;
                                        break;
                                    case 1:
                                        pe.Image = images[2];
                                        pe.offsetX = 0;
                                        pe.offsetY = f2 * 1.5f;
                                        break;
                                    case 2:
                                        pe.Image = images[3];
                                        pe.offsetX = f * 4f;
                                        pe.offsetY = f3 * 1.5f;
                                        break;
                                    case 3:
                                        pe.Image = images[3];
                                        pe.offsetX = -f * 4f;
                                        pe.offsetY = -f3 * 1.5f;
                                        break;
                                    case 4:
                                        pe.Image = images[1];
                                        pe.offsetX = 0;
                                        pe.offsetY = f2;
                                        break;
                                    case 5:
                                        pe.Image = images[4];
                                        pe.offsetX = - f * 7.5f;
                                        pe.offsetY = Math.Max(-f3 * 3f, 0);
                                        break;
                                    case 6:
                                        pe.Image = images[5];
                                        pe.offsetX = f * 7.5f;
                                        pe.offsetY = Math.Max(f3 * 3f, 0);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            f = -f;
                            for (int i = 0; i < 7; i++)
                            {
                                PartEntity pe = parts[i];
                                switch (i)
                                {
                                    case 0:
                                        pe.Image = images[9];
                                        pe.offsetX = 0;
                                        pe.offsetY = f2 * 2;
                                        break;
                                    case 1:
                                        pe.Image = images[11];
                                        pe.offsetX = 0;
                                        pe.offsetY = f2 * 1.5f;
                                        break;
                                    case 2:
                                        pe.Image = images[12];
                                        pe.offsetX = f * 4f;
                                        pe.offsetY = f3 * 1.5f;
                                        break;
                                    case 3:
                                        pe.Image = images[12];
                                        pe.offsetX = - f * 4f;
                                        pe.offsetY = -f3 * 1.5f;
                                        break;
                                    case 4:
                                        pe.Image = images[10];
                                        pe.offsetX = 0;
                                        pe.offsetY = f2;
                                        break;
                                    case 5:
                                        pe.Image = images[13];
                                        pe.offsetX = - f * 7.5f;
                                        pe.offsetY = Math.Max(-f3 * 3f, 0);
                                        break;
                                    case 6:
                                        pe.Image = images[14];
                                        pe.offsetX = f * 7.5f;
                                        pe.offsetY = Math.Max(f3 * 3f, 0);
                                        break;
                                }
                            }
                        }
                    }
                    #endregion
                    #region 평상시 애니매이션 재생 부분
                    else
                    {
                        if (isActuallyMoving)
                        {
                            isActuallyMoving = false;
                            Image = (int)direction < 5 ? images[0] : images[2];
                        }
                        float f = (float)Math.Sin((double)tickCount * 0.5);
                        if ((int)direction < 5)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                PartEntity pe = parts[i];
                                switch (i)
                                {
                                    case 0:
                                        pe.Image = images[0];
                                        pe.offsetX = 0;
                                        pe.offsetY = f * 2;
                                        break;
                                    case 1:
                                        pe.Image = images[2];
                                        pe.offsetX = 0;
                                        pe.offsetY = f * 1.5f;
                                        break;
                                    case 2:
                                        pe.Image = images[3];
                                        pe.offsetX = 0;
                                        pe.offsetY = f * 1.5f;
                                        break;
                                    case 3:
                                        pe.Image = images[3];
                                        pe.offsetX = 0;
                                        pe.offsetY = f * 1.5f;
                                        break;
                                    case 4:
                                        pe.Image = images[1];
                                        pe.offsetX = 0;
                                        pe.offsetY = f;
                                        break;
                                    case 5:
                                        pe.Image = images[4];
                                        pe.offsetX = 0;
                                        pe.offsetY = 0;
                                        break;
                                    case 6:
                                        pe.Image = images[5];
                                        pe.offsetX = 0;
                                        pe.offsetY = 0;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            f = -f;
                            for (int i = 0; i < 7; i++)
                            {
                                PartEntity pe = parts[i];
                                switch (i)
                                {
                                    case 0:
                                        pe.Image = images[9];
                                        pe.offsetX = 0;
                                        pe.offsetY = f * 2;
                                        break;
                                    case 1:
                                        pe.Image = images[11];
                                        pe.offsetX = 0;
                                        pe.offsetY = f * 1.5f;
                                        break;
                                    case 2:
                                        pe.Image = images[12];
                                        pe.offsetX = 0;
                                        pe.offsetY = f * 1.5f;
                                        break;
                                    case 3:
                                        pe.Image = images[12];
                                        pe.offsetX = 0;
                                        pe.offsetY = f * 1.5f;
                                        break;
                                    case 4:
                                        pe.Image = images[10];
                                        pe.offsetX = 0;
                                        pe.offsetY = f;
                                        break;
                                    case 5:
                                        pe.Image = images[13];
                                        pe.offsetX = 0;
                                        pe.offsetY = 0;
                                        break;
                                    case 6:
                                        pe.Image = images[14];
                                        pe.offsetX = 0;
                                        pe.offsetY = 0;
                                        break;
                                }
                            }
                        }
                    }
                    #endregion
                    break;
            }
        }
        public override void discard()
        {
            base.discard();
            foreach (PartEntity part in parts)
            {
                part.discard();
            }
            //parts.Clear();
        }
        public override void scaleEntity(float scale)
        {
            base.scaleEntity(scale);
            maxHealth *= scale * 1.4f; currentHealth = maxHealth * 1.4f;
            attackDamage *= scale * 1.4f; pushPower = (int)(pushPower * scale); moveSpeed = (int)(moveSpeed * Math.Sqrt(scale));
            foreach (PartEntity part in parts)
            {
                part.offsetX *= scale;
                part.offsetY *= scale;
                part.offsetZ *= scale;
                part.scaleEntity(scale);
            }
        }
    }
}
