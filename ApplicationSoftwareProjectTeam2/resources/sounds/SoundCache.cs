using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.resources.sounds
{
    public static class SoundCache
    {
        // 음향 파일 경로와 SoundPlayer 객체를 매핑하는 캐시
        private static Dictionary<string, SoundPlayer> cache = new Dictionary<string, SoundPlayer>();

        /// <summary>
        /// 지정된 파일 경로들의 음향 파일을 미리 로드하여 캐시에 저장합니다.
        /// </summary>
        public static void PreloadSounds(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                if (!cache.ContainsKey(path) && File.Exists(path))
                {
                    try
                    {
                        SoundPlayer player = new SoundPlayer(path);
                        player.Load();
                        cache[path] = player;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading sound {path}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 지정한 폴더 내의 WAV 음향 파일들을 자동으로 불러와 캐시에 저장합니다.
        /// SoundPlayer는 WAV 형식만 지원합니다.
        /// </summary>
        public static void PreloadSoundsFromFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                // WAV 파일만 검색 (SoundPlayer는 WAV 파일만 사용)
                var files = Directory.GetFiles(folderPath, "*.wav");
                foreach (var file in files)
                {
                    if (!cache.ContainsKey(file))
                    {
                        try
                        {
                            SoundPlayer player = new SoundPlayer(file);
                            player.Load();
                            cache[file] = player;
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
        /// 캐시에서 음향 파일 객체를 반환합니다. 캐시에 없으면 파일에서 로드합니다.
        /// </summary>
        public static SoundPlayer GetSound(string path)
        {
            if (cache.ContainsKey(path))
                return cache[path];
            else if (File.Exists(path))
            {
                try
                {
                    SoundPlayer player = new SoundPlayer(path);
                    player.Load();
                    cache[path] = player;
                    return player;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading sound {path}: {ex.Message}");
                }
            }
            return null;
        }
    }


}
