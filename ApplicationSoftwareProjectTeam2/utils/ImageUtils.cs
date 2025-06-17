using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.utils
{
    public static class ImageUtils
    {
        public static Image RotateImage(Image image, int angle)
        {
            // 회전 각도를 0~360도로 정규화
            angle = angle % 360;
            if (angle < 0) angle += 360;

            // 새로운 빈 비트맵 생성 (이미지의 크기 고려)
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                g.TranslateTransform((float)image.Width / 2, (float)image.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)image.Width / 2, -(float)image.Height / 2);
                g.InterpolationMode = InterpolationMode.Low;
                g.DrawImage(image, new Point(0, 0));
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
