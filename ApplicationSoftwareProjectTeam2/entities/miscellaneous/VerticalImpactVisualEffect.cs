using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.miscellaneous
{
    public class VerticalImpactVisualEffect : Entity
    {
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.sprite_ColEffVert0,
            Properties.Resources.sprite_ColEffVert1,
            Properties.Resources.sprite_ColEffVert2,
            Properties.Resources.sprite_ColEffVert3,
            Properties.Resources.sprite_ColEffVert4,
            Properties.Resources.sprite_ColEffVert5,
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_ColEffVert0),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_ColEffVert1),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_ColEffVert2),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_ColEffVert3),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_ColEffVert4),
            ImageUtils.FlipImageHorizontally(Properties.Resources.sprite_ColEffVert5),
        };
        bool isRight;
        public VerticalImpactVisualEffect(GamePanel level, bool isRight, float x, float y, float z) : base(level)
        {
            this.visualSize = 2.0f;
            this.weight = 1000;
            this.hasGravity = false;
            renderType = 0; // no shadow
            this.isRight = isRight;
            setPosition(x, y, z);
        }
        public override void tick()
        {
            base.tick();
            if (tickCount < images.Count - 5)
            {
                Image = images[isRight ? tickCount - 1 : tickCount + 4];
            }
            else
            {
                shouldRemove = true;
            }
        }
    }
}
