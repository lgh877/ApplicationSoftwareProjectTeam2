using System;
using System.Collections.Generic;
using System.Linq;
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
            this.visualSize = 2.0f;
            this.width = 76;
            this.height = 78;
            this.weight = 100;
            maxHealth = 100; currentHealth = 100;
            this.pushPower = 10;
            this.renderType = 2; // default shadow
            parts.Add(new PartEntity(level, x + 14, y + 44, z + 3) { Owner = this, offsetX = 14, offsetY = 44, offsetZ = 3, Image = images[0] }); // Head
            parts.Add(new PartEntity(level, x, y + 30, z + 5) { Owner = this, offsetX = 0, offsetY = 30, offsetZ = 5, Image = images[2] }); // Upper Body
            parts.Add(new PartEntity(level, x + 24, y + 18, z + 7) { Owner = this, offsetX = 24, offsetY = 18, offsetZ = 7, Image = images[3] }); // LeftArm
            parts.Add(new PartEntity(level, x - 26, y + 14, z) { Owner = this, offsetX = -26, offsetY = 14, offsetZ = 0, Image = images[3] }); // RightArm
            parts.Add(new PartEntity(level, x, y + 18, z + 6) { Owner = this, offsetX = 0, offsetY = 18, offsetZ = 6, Image = images[1] }); // Lower Body
            parts.Add(new PartEntity(level, x + 14, y, z + 12) { Owner = this, offsetX = 14, offsetY = 0, offsetZ = 12, Image = images[4] }); // Left Leg
            parts.Add(new PartEntity(level, x - 20, y, z + 2) { Owner = this, offsetX = -20, offsetY = 0, offsetZ = 2, Image = images[5] }); // Right Leg
            foreach (PartEntity part in parts)
            {
                level.addFreshEntity(part);
            }
        }
        public override void tickAlive()
        {
            base.tickAlive();
        }
        public override void discard()
        {
            base.discard();
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
