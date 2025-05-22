using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.utils
{
    public static class ImageUtils
    {
        public static Image RotateImage(Image img, float angle)
        {
            Bitmap rotatedBmp = new Bitmap(img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                g.TranslateTransform(img.Width / 2, img.Height / 2); // 중심점 이동
                g.RotateTransform(angle); // 회전 적용
                g.TranslateTransform(-img.Width / 2, -img.Height / 2); // 원래 위치 복귀
                g.DrawImage(img, new Point(0, 0)); // 이미지 그리기
            }
            return rotatedBmp;
        }
        public static Image FlipImageHorizontally(Image img)
        {
            Bitmap flippedBmp = new Bitmap(img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(flippedBmp))
            {
                // 좌우 반전을 위해 X축을 -1로 스케일 변환
                g.TranslateTransform(img.Width, 0); // 오른쪽 끝으로 이동
                g.ScaleTransform(-1, 1); // 좌우 반전
                g.DrawImage(img, 0, 0, img.Width, img.Height); // 이미지 그리기
            }
            return flippedBmp;
        }

    }
}
