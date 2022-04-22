using System;
using System.Configuration;
using System.IO;
using System.Text;

/// <summary>
/// 系统写日志帮助类
/// </summary>
public sealed class LogHelper
{
    private static volatile LogHelper instance;
    private static object syncRoot = new object();
    #region 日志相关配置
    private string logPath = "";//日志文件路径
    private int maxFileSize = 1024 * 1024 * 10;//单个日志文件最大容量
    #endregion
    public static LogHelper Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new LogHelper();
                    
                }
            }
            return instance;
        }
    }
    public LogHelper()
    {
        if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Logs"))
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Logs");
        }
        DeleteHisFile();
        CreateLogFile();
    }
    /// <summary>
    /// 创建日志文件
    /// </summary>
    /// <param name="logPath">日志文件路径</param>
    private void CreateLogFile()
    {
        logPath = AppDomain.CurrentDomain.BaseDirectory + @"Logs\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
        FileStream fs = new FileStream(logPath, FileMode.Create);
        fs.Flush();
        fs.Close();
    }
    /// <summary>
    /// 删除超过一定时间的历史日志文件,系统每次启动都会执行调用
    /// </summary>
    private void DeleteHisFile()
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + @"Logs";
        DirectoryInfo root = new DirectoryInfo(path);
        FileInfo[] files = root.GetFiles();
        int logSaveTime=int.Parse(ConfigurationManager.AppSettings["logSaveTime"] + "");
        foreach (FileInfo f in files)
        {
            int jiange = DateTime.Now.Subtract(f.LastWriteTime).Days;
            if (jiange > logSaveTime)
            {
                File.Delete(f.FullName);
            }
        }
    }
    /// <summary>
    /// 输出日志
    /// </summary>
    /// <param name="info">日志内容</param>
    public void LogInfo(string info)
    {
        try
        {
            lock (syncRoot)
            {
                if (!File.Exists(logPath))
                {
                    CreateLogFile();
                }
                string oldLogInfo = File.ReadAllText(logPath);
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(oldLogInfo);
                stringBuilder.AppendLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")  + " " + info);
                FileStream fs = new FileStream(logPath, FileMode.Create, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);
                sr.Write(stringBuilder.ToString());//开始写入值
                sr.Close();
                fs.Close();
            }
        }
        catch (Exception e)
        {

        }

    }
}
