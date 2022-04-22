using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class HttpHelper
{
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="url">服务端地址</param>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    public bool UploadFile(string url, string path)
    {
        try
        {
            WebClient webClient = new WebClient();  // 初始化WebClient
            webClient.UploadFile(url, path);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}