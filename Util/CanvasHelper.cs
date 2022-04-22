using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TreadSys.Common
{
    /// <summary>
    /// 画布帮助类
    /// </summary>
    public class CanvasHelper
    {
        /// <summary>
        /// 保存整个页面
        /// </summary>
        /// <param name="window"></param>
        /// <param name="dpi"></param>
        /// <param name="filename"></param>
        public static void SaveWindow(Page window, int dpi, string fileName, string foldName)
        {

            var rtb = new RenderTargetBitmap(
                (int)window.Width, //width
                (int)window.Height, //height
                dpi, //dpi x
                dpi, //dpi y
                PixelFormats.Pbgra32 // pixelformat
                );
            rtb.Render(window);

            SaveRTBAsPNG(rtb, fileName, foldName);

        }

        /// <summary>
        /// 保存指定Canvas
        /// </summary>
        /// <param name="window"></param>
        /// <param name="canvas"></param>
        /// <param name="dpi"></param>
        /// <param name="filename"></param>
        public static void SaveCanvas(Page window, Canvas canvas, int dpi, string fileName, string foldName)
        {
            Size size = new Size(window.Width, window.Height);
            canvas.Measure(size);
            //canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap(
                (int)window.Width, //width
                (int)window.Height, //height
                dpi, //dpi x
                dpi, //dpi y
                PixelFormats.Pbgra32 // pixelformat
                );
            rtb.Render(canvas);

            SaveRTBAsPNG(rtb, fileName, foldName);
        }

        /// <summary>
        /// 保存图片的方法
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="filename"></param>
        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string fileName, string foldName)
        {
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bmp));
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(fileName);
            }
            using (var stm = File.Create(fileName + @"/" + foldName))
            {
                enc.Save(stm);
            }
        }

    }
}
