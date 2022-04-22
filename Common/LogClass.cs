using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreadSys.Common
{
    // example:
    // LogClass.WriteLog("start...");
    // catch (Exception ex){   LogClass.WriteExceptionLog(ex);

    class LogClass
    {

        public static object locker = new object();
        public static string path = Environment.CurrentDirectory + "\\Log";
        public static Int64 time = 2592000000;//30天的毫秒数

        public static void CreateLog()
        {
            deleteFilesByTime();//删除30天前的日志文件
            string fileName = "\\LOG" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            try
            {
                if (!Directory.Exists(path))
                {
                    //不存在则创建           
                    Directory.CreateDirectory(path);
                }
                string logFileName = path + fileName;//生成日志文件全名称
                if (!File.Exists(logFileName))
                {
                    //不存在则创建
                    File.Create(logFileName).Close();
                }

                StreamWriter writer = File.AppendText(logFileName);//文件中添加文件流  
                writer.WriteLine("");
                writer.Flush();
                writer.Close();
            }
            catch (Exception ex)
            {
                LogClass.WriteExceptionLog(ex);

                string path = Path.Combine("./omsLog");
                if (!Directory.Exists(path))
                {
                    //不存在则创建           
                    Directory.CreateDirectory(path);
                }
                string logFileName = path + fileName;//生成日志文件全名称  
                if (!File.Exists(logFileName))
                {
                    //不存在则创建
                    File.Create(logFileName).Close();
                }
                StreamWriter writer = File.AppendText(logFileName);
                writer.WriteLine("");
                writer.Flush();
                writer.Close();
            }
        }

        public static void WriteLog(string logMsg)
        {
            string fileName = "\\LOG-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            try
            {
                if (!Directory.Exists(path))
                {
                    //不存在则创建           
                    Directory.CreateDirectory(path);
                }
                string logFileName = path + fileName;//生成日志文件全名称  
                if (!File.Exists(logFileName))
                {
                    //不存在则创建
                    File.Create(logFileName).Close();
                }
                /*文件超过10MB则重命名,同时创建新的文件*/
                FileInfo finfo = new FileInfo(logFileName);
                if (finfo.Length > 1024 * 1024 * 10)
                {
                    string newFileName = path + "\\LOG" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".log";
                    File.Move(logFileName, newFileName);
                    File.Create(logFileName).Close();//创建文件  
                }

                StreamWriter writer = File.AppendText(logFileName);//文件中添加文件流  
                writer.WriteLine("");
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + logMsg);
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                string path = Path.Combine("./omsLog");
                if (!Directory.Exists(path))
                {
                    //不存在则创建           
                    Directory.CreateDirectory(path);
                }
                string logFileName = path + fileName;//生成日志文件全名称 
                if (!File.Exists(logFileName))
                {
                    //不存在则创建
                    File.Create(logFileName).Close();
                }
                StreamWriter writer = File.AppendText(logFileName);
                writer.WriteLine("");
                writer.WriteLine(DateTime.Now.ToString("日志记录错误HH:mm:ss") + " " + e.Message + " " + logMsg);
                writer.Flush();
                writer.Close();
            }
        }

        public static void WriteExceptionLog(Exception ex)
        {
            StringBuilder exMsg = new StringBuilder();
            exMsg.Append("调用堆栈：" + ex.StackTrace.Trim()).Append("触发方法：" + ex.TargetSite).Append("异常信息：" + ex.Message);
            WriteLog(exMsg.ToString());
        }

        public static void deleteFilesByTime()
        {
            if (Directory.Exists(path))
            {
                //获取文件夹下所有的文件
                DirectoryInfo dyInfo = new DirectoryInfo(path);
                foreach (FileInfo feInfo in dyInfo.GetFiles())
                {
                    //判断文件日期是否小于定义的日期，是则删除
                    TimeSpan ts = System.DateTime.Now.Subtract(feInfo.CreationTime);
                    if (ts.TotalMilliseconds > time)
                    {
                        feInfo.Delete();
                    }
                }
            }
        }

    }
}
