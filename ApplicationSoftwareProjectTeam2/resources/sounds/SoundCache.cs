using System;
using System.Collections.Generic;
using WMPLib;

namespace ApplicationSoftwareProjectTeam2.resources.sounds
{
    public static class SoundCache
    {
        public static WindowsMediaPlayer bone_crack1 = new WindowsMediaPlayer() { URL = "sounds//bone_crack1.mp3", settings = { volume = 0 } };
        public static WindowsMediaPlayer bone_crack2 = new WindowsMediaPlayer() { URL = "sounds//bone_crack2.mp3", settings = { volume = 0 } };
        public static WindowsMediaPlayer swosh = new WindowsMediaPlayer() { URL = "sounds//swosh.mp3", settings = { volume = 0 } };
    }
}