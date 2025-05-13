using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.resources.textures
{
    public static class ImageCache
    {
        // 이미지 경로와 Image 객체를 매핑하는 캐시
        private static Dictionary<string, Image> cache = new Dictionary<string, Image>();

        /// <summary>
        /// 지정된 파일 경로들의 이미지를 미리 로드합니다.
        /// </summary>
        public static void PreloadImages(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                if (!cache.ContainsKey(path) && File.Exists(path))
                {
                    try
                    {
                        cache[path] = Image.FromFile(path);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading image {path}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 지정한 폴더 내의 이미지 파일들을 자동으로 불러와 캐시에 저장합니다.
        /// 지원하는 확장자: .png, .jpg, .jpeg, .bmp, .gif
        /// </summary>
        public static void PreloadImagesFromFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                // 지원할 이미지 파일 확장자 목록 (소문자로 변환하여 비교)
                string[] validExtensions = new string[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };

                // 폴더 내 모든 파일 검색
                var files = Directory.GetFiles(folderPath);
                foreach (var file in files)
                {
                    string ext = Path.GetExtension(file).ToLower();
                    if (validExtensions.Contains(ext) && !cache.ContainsKey(file))
                    {
                        try
                        {
                            cache[file] = Image.FromFile(file);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error loading image {file}: {ex.Message}");
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
        /// 캐시에서 이미지 객체를 반환합니다. 캐시에 없으면 파일에서 로드합니다.
        /// </summary>
        public static Image GetImage(string path)
        {
            if (cache.ContainsKey(path))
                return cache[path];
            else if (File.Exists(path))
            {
                try
                {
                    Image img = Image.FromFile(path);
                    cache[path] = img;
                    return img;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading image {path}: {ex.Message}");
                }
            }
            return null;
        }
    }


}
