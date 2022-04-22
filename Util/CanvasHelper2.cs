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

/// <summary>
/// 画布帮助类
/// </summary>
public class CanvasHelper
{
    /// <summary>
    /// 保存页面canvas标签的内容为jpg图片
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public void SaveCanvas(Canvas c1, Rect rect ,string foldPath,string fileName)
    {
        c1.Arrange(rect);
        c1.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));

        var rtb = new RenderTargetBitmap(
(int)c1.ActualWidth, //width
(int)c1.ActualHeight, //height
            96, //dpi x
            96, //dpi y
            PixelFormats.Pbgra32);
        rtb.Render(c1);//界面渲染内容

        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(rtb));
        if (!Directory.Exists(foldPath))
        {
            Directory.CreateDirectory(foldPath);
        }
        using (var fileStream = File.Create(foldPath + @"\" + fileName))
        {
            encoder.Save(fileStream);
        }
    }


}