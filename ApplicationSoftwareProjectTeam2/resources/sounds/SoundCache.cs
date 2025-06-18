using System;
using System.Collections.Generic;
using WMPLib;

namespace ApplicationSoftwareProjectTeam2.resources.sounds
{
    public static class SoundCache
    {
        private static WindowsMediaPlayer cachePlayer = new WindowsMediaPlayer();

        public static IWMPMedia bone_crack1 = cachePlayer.newMedia("sounds//bone_crack1.mp3");
        public static IWMPMedia bone_crack2 = cachePlayer.newMedia("sounds//bone_crack2.mp3");
        public static IWMPMedia swosh = cachePlayer.newMedia("sounds//swosh.mp3");
        public static IWMPMedia skulls_bite = cachePlayer.newMedia("sounds//skulls_bite.mp3");
        public static IWMPMedia bonely_punch1 = cachePlayer.newMedia("sounds//bonely_punch1.mp3");
        public static IWMPMedia bonely_punch2 = cachePlayer.newMedia("sounds//bonely_punch2.mp3");
        public static IWMPMedia bonely_punch3 = cachePlayer.newMedia("sounds//bonely_punch3.mp3");

    }
}