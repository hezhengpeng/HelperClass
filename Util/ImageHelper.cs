using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

public class ImageHelper
{
    /// <summary>
    /// 根据图片文件位置获取图片对象，用于显示
    /// </summary>
    /// <param name="imagePath"></param>
    /// <returns></returns>
    public BitmapImage GetImage(string imagePath)
    {
        BitmapImage bitmap = new BitmapImage();
        if (File.Exists(imagePath))
        {
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            using (Stream ms = new MemoryStream(File.ReadAllBytes(imagePath)))
            {
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze();
            }
        }
        return bitmap;
    }

    /// <summary>
    /// 把多张图片纵向拼接为一张
    /// </summary>
    /// <param name="filePath1">图片1文件位置</param>
    /// <param name="filePath2">图片2文件位置</param>
    /// <returns></returns>
    public string JoinImage(string filePath1,string filePath2)
    {
        Image ima1 = System.Drawing.Image.FromFile(filePath1);
        Image ima2 = System.Drawing.Image.FromFile(filePath2);
        List<System.Drawing.Image> imageList = new List<System.Drawing.Image>();
        imageList.Add(ima1);
        imageList.Add(ima2);
        //纵向拼接
        int height = 0;
        //计算总长度
        foreach (Image i in imageList)
        {
            height += i.Height;
        }
        //宽度不变
        int width = imageList.Max(x => x.Width);
        //构造最终的图片白板
        Bitmap tableChartImage = new Bitmap(width, height);
        Graphics graph = Graphics.FromImage(tableChartImage);
        //初始化这个大图
        graph.DrawImage(tableChartImage, width, height);
        //初始化当前宽
        int currentHeight = 0;
        foreach (System.Drawing.Image i in imageList)
        {
            //拼图
            graph.DrawImage(i, 0, currentHeight);
            //拼接改图后，当前宽度
            currentHeight += i.Height;
            i.Dispose();

        }
        string path = AppDomain.CurrentDomain.BaseDirectory + 
                            "/cache/" + DateTime.Now.ToString("yyyyMMddHHmmss")+".jpg";
        tableChartImage.Save(path);
        return path;

    }

    [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteObject([In] IntPtr hObject);

    public System.Windows.Media.ImageSource ImageSourceForBitmap(System.Drawing.Bitmap bmp)
    {
        var handle = bmp.GetHbitmap();
        try
        {
            System.Windows.Media.ImageSource newSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(handle);
            return newSource;
        }
        catch (Exception)
        {
            DeleteObject(handle);
            return null;
        }
    }
}