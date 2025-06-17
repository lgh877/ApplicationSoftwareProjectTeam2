using System;
using System.Collections.Generic;
using WMPLib;

namespace ApplicationSoftwareProjectTeam2.resources.sounds
{
    public static class SoundCache
    {
        // 파일 이름과 WindowsMediaPlayer 객체를 매핑하는 캐시
        private static Dictionary<string, WindowsMediaPlayer> cache = new Dictionary<string, WindowsMediaPlayer>();

        /// <summary>
        /// 지정된 폴더 내의 WAV 파일을 미리 로드하여 캐시에 저장합니다.
        /// </summary>
        public static void PreloadSoundsFromFolder(string folderPath)
        {
            if (System.IO.Directory.Exists(folderPath))
            {
                var files = System.IO.Directory.GetFiles(folderPath, "*.mp3");
                foreach (var file in files)
                {
                    string soundName = System.IO.Path.GetFileNameWithoutExtension(file);

                    if (!cache.ContainsKey(soundName))
                    {
                        try
                        {
                            WindowsMediaPlayer player = new WindowsMediaPlayer();
                            player.URL = file;
                            player.settings.autoStart = false; // 자동 시작 방지
                            cache[soundName] = player;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error loading sound {file}: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Directory {folderPath} does not exist.");
            }
        }

        /// <summary>
        /// 캐시에서 음향 파일을 가져와 재생합니다.
        /// </summary>
        public static void PlaySound(string soundName)
        {
            if (cache.ContainsKey(soundName))
            {
                try
                {
                    cache[soundName].controls.play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error playing sound {soundName}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Sound '{soundName}' not found in cache.");
            }
        }
    }
}