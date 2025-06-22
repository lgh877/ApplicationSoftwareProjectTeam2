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
        private float animFactor = 0; // 애니메이션 프레임 조절 변수
        public List<PartEntity> parts = new List<PartEntity>(7);
        public GiantWeirdGuy(GamePanel level) : base(level)
        {
            this.visualSize = 1.0f;
            cost = 8;
            this.width = 50;
            this.height = 85;
            this.weight = 60;
            maxHealth = 250; currentHealth = 250; finalMaxHealth = maxHealth;
            this.pushPower = 60; moveSpeed = 2;
            attackDamage = 15; finalAttackDamage = attackDamage;
            this.renderType = 2; // default shadow
            direction = level.usualRandom.Next(2) == 0 ? Direction.Right : Direction.Left;
            parts.Add(new PartEntity(level, this, -18, 37, 3) { Image = images[0] }); // Head
            parts.Add(new PartEntity(level, this, 4, 28, 4) { Image = images[2] }); // Upper Body
            parts.Add(new PartEntity(level, this, -20, 8, 11) { Image = images[3] }); // LeftArm
            parts.Add(new PartEntity(level, this, 18, 8, 0) { Image = images[3] }); // RightArm
            parts.Add(new PartEntity(level, this, 4, 15, 6) { Image = images[1] }); // Lower Body
            parts.Add(new PartEntity(level, this, -15, 0, 10) { Image = images[4] }); // Left Leg
            parts.Add(new PartEntity(level, this, 12, 0, 2) { Image = images[5] }); // Right Leg
            foreach (PartEntity part in parts)
            {
                level.addFreshEntity(part);
            }
            scaleEntity(1.5f);
        }
        #region 캐릭터 아이디 기록
        public override byte getLivingEntityId()
        {
            return (byte) 2;
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
                            if (getTarget() == null)
                            {
                                if (hadTarget)
                                {
                                    hadTarget = false;
                                }
                                LivingEntity foundTarget = detectTargetManhattan(2000);
                                if (foundTarget != null)
                                {
                                    target = foundTarget;
                                    hadTarget = true;
                                }
                            }
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
                                    float ydiff = target.y - y;
                                    if ((target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.4 * (width * width + (target.width + width) * (target.width + width))
                                        && ydiff < height
                                        && ydiff > -target.height)
                                    {
                                        entityState = 1;
                                        walkTicks = 0;
                                        animFactor = 0;
                                    }
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
                    #region 캐릭터가 실제로 움직이고 있는 지 결정하는 부분으로, 애니매이션 재생에 쓰임
                    if (deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z > 4)
                    {
                        walkTicks++;
                        isActuallyMoving = true;

                        float f = (float)Math.Sin((double)tickCount * 0.17 * moveSpeed);
                        float f3 = (float)Math.Cos((double)tickCount * 0.17 * moveSpeed);
                        float f2 = (float)Math.Sin((double)tickCount * 0.34 * moveSpeed);
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
                                        pe.offsetX = f * 12f;
                                        pe.offsetY = f3 * 1.5f;
                                        pe.desiredAngle = (int)(-f * 15);
                                        break;
                                    case 3:
                                        pe.Image = images[3];
                                        pe.offsetX = -f * 12f;
                                        pe.offsetY = -f3 * 1.5f;
                                        pe.desiredAngle = (int)(f * 15);
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
                                        pe.offsetX = f * 12f;
                                        pe.offsetY = f3 * 1.5f;
                                        pe.desiredAngle = (int)(-f * 15);
                                        break;
                                    case 3:
                                        pe.Image = images[12];
                                        pe.offsetX = - f * 12f;
                                        pe.offsetY = -f3 * 1.5f;
                                        pe.desiredAngle = (int)(f * 15);
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
                            resetOffset();
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
                case 1:
                    walkTicks++;
                    if (walkTicks == 2)
                    {
                        resetOffset();
                    }
                    else if (walkTicks < 6)
                    {
                        direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                        bool isright = (int)this.direction < 5;
                        if (isright)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                PartEntity pe = parts[i];
                                switch (i)
                                {
                                    case 0:
                                        pe.Image = images[0];
                                        break;
                                    case 1:
                                        pe.Image = images[2];
                                        break;
                                    case 2:
                                        pe.Image = images[3];
                                        break;
                                    case 3:
                                        pe.Image = images[6];
                                        break;
                                    case 4:
                                        pe.Image = images[1];
                                        break;
                                    case 5:
                                        pe.Image = images[4];
                                        break;
                                    case 6:
                                        pe.Image = images[5];
                                        break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                PartEntity pe = parts[i];
                                switch (i)
                                {
                                    case 0:
                                        pe.Image = images[9];
                                        break;
                                    case 1:
                                        pe.Image = images[11];
                                        break;
                                    case 2:
                                        pe.Image = images[12];
                                        break;
                                    case 3:
                                        pe.Image = images[15];
                                        break;
                                    case 4:
                                        pe.Image = images[10];
                                        break;
                                    case 5:
                                        pe.Image = images[13];
                                        break;
                                    case 6:
                                        pe.Image = images[14];
                                        break;
                                }
                            }
                        }
                        animFactor = level.Lerp(animFactor, 20, 0.7f);
                        float offset = isright ? -animFactor : animFactor;
                        parts[3].offsetX = offset;
                        parts[0].offsetX = offset * 0.3f;
                        parts[1].offsetX = offset * 0.3f;
                        parts[2].offsetX = offset * 0.3f;
                        parts[4].offsetX = offset * 0.15f;
                    }
                    else if (walkTicks < 16)
                    {
                        bool isright = (int)this.direction < 5;
                        if (isright)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                PartEntity pe = parts[i];
                                switch (i)
                                {
                                    case 0:
                                        pe.Image = images[0];
                                        break;
                                    case 1:
                                        pe.Image = images[2];
                                        break;
                                    case 2:
                                        pe.Image = images[3];
                                        break;
                                    case 3:
                                        pe.Image = images[7];
                                        break;
                                    case 4:
                                        pe.Image = images[1];
                                        break;
                                    case 5:
                                        pe.Image = images[4];
                                        break;
                                    case 6:
                                        pe.Image = images[5];
                                        break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                PartEntity pe = parts[i];
                                switch (i)
                                {
                                    case 0:
                                        pe.Image = images[9];
                                        break;
                                    case 1:
                                        pe.Image = images[11];
                                        break;
                                    case 2:
                                        pe.Image = images[12];
                                        break;
                                    case 3:
                                        pe.Image = images[16];
                                        break;
                                    case 4:
                                        pe.Image = images[10];
                                        break;
                                    case 5:
                                        pe.Image = images[13];
                                        break;
                                    case 6:
                                        pe.Image = images[14];
                                        break;
                                }
                            }
                        }
                        animFactor = 1 - (walkTicks - 6) * 0.1f;
                        animFactor = (float) (Math.Pow(animFactor, 8) * Math.PI); // easing function
                        animFactor = (float) Math.Sin(animFactor) * 75;
                        float offset = isright ? animFactor : -animFactor;
                        parts[3].offsetX = offset;
                        parts[0].offsetX = offset * 0.4f;
                        parts[1].offsetX = offset * 0.4f;
                        parts[2].offsetX = offset * 0.4f;
                        parts[4].offsetX = offset * 0.2f;
                        if(walkTicks == 8)
                        {
                            level.playSound(Boxer.sounds[level.getRandomInteger(4)]);
                            if (target != null &&
                            (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.5 * (width * width + (target.width + width) * (target.width + width)) &&
                            target.y - y < height && target.y - y > -target.height)
                            {
                                doHurtTarget(target);
                            }
                        }
                        else if(walkTicks < 8) direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    }
                    else if(walkTicks == 16)
                    {
                        animFactor = 0;
                        resetOffset();
                        resetImages();
                        entityState = 0;
                        walkTicks = 0;
                    }
                    break;
            }
        }
        public override void setDeath(object? sender, EventArgs e)
        {
            base.setDeath(sender, e);
            resetOffset();
            parts[0].Image = (int)direction < 5 ? images[8] : images[17];
            parts[2].Image = (int)direction < 5 ? images[6] : images[15];
            parts[3].Image = (int)direction < 5 ? images[6] : images[15];
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
        private void resetOffset()
        {
            foreach (PartEntity part in parts)
            {
                part.offsetX = 0;
                part.offsetY = 0;
                part.offsetZ = 0;
                part.desiredAngle = 0;
            }
        }
        private void resetImages()
        {
            if ((int)direction < 5)
            {
                for (int i = 0; i < 7; i++)
                {
                    PartEntity pe = parts[i];
                    switch (i)
                    {
                        case 0:
                            pe.Image = images[0];
                            break;
                        case 1:
                            pe.Image = images[2];
                            break;
                        case 2:
                            pe.Image = images[3];
                            break;
                        case 3:
                            pe.Image = images[3];
                            break;
                        case 4:
                            pe.Image = images[1];
                            break;
                        case 5:
                            pe.Image = images[4];
                            break;
                        case 6:
                            pe.Image = images[5];
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    PartEntity pe = parts[i];
                    switch (i)
                    {
                        case 0:
                            pe.Image = images[9];
                            break;
                        case 1:
                            pe.Image = images[11];
                            break;
                        case 2:
                            pe.Image = images[12];
                            break;
                        case 3:
                            pe.Image = images[12];
                            break;
                        case 4:
                            pe.Image = images[10];
                            break;
                        case 5:
                            pe.Image = images[13];
                            break;
                        case 6:
                            pe.Image = images[14];
                            break;
                    }
                }
            }
        }
        public override void scaleEntity(float scale)
        {
            base.scaleEntity(scale);
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
