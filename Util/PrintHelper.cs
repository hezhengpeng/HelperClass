using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
/// <summary>
/// 打印帮助类
/// </summary>
public class PrintHelper
{
    /// <summary>
    /// 打印画布
    /// </summary>
    /// <param name="can"></param>
    public void Print(Canvas can,Canvas qwe)
    {
        PrintDialog printDialog = new PrintDialog();
        if (printDialog.ShowDialog() == true)
        {
            qwe.Visibility = Visibility.Hidden;
            can.LayoutTransform = new ScaleTransform(4, 4);
            System.Windows.Size size = new System.Windows.Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
            qwe.Measure(size);
            qwe.Arrange(new Rect(0, 0, size.Width, size.Height));
            printDialog.PrintVisual(can, "123");
            // printDialog.PrintVisual(can2, "123");
            can.LayoutTransform = null;
            qwe.Visibility = Visibility.Visible;
        }
    }
}