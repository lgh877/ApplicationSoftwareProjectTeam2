using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.resources.sounds;
using WMPLib;

namespace ApplicationSoftwareProjectTeam2.entities.miscellaneous
{
    public class GroundExplosion : Explosion
    {
        public static List<WindowsMediaPlayer> sounds = new List<WindowsMediaPlayer>()
        {
            SoundCache.explosion1, SoundCache.explosion2, SoundCache.explosion3, SoundCache.explosion4
        };
        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.groundExplosion1,
            Properties.Resources.groundExplosion2,
            Properties.Resources.groundExplosion3,
            Properties.Resources.groundExplosion4,
            Properties.Resources.groundExplosion5,
            Properties.Resources.groundExplosion6,
            Properties.Resources.groundExplosion7,
            Properties.Resources.groundExplosion8,
            Properties.Resources.groundExplosion9,
        };
        public GroundExplosion(GamePanel level) : base(level)
        {
            expImages = images;
            this.width = 256;
            Image = images[0];
        }
    }
}
