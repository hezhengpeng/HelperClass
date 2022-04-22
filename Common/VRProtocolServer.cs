using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreadSys.Common
{
    public class VRProtocolServer : AppServer<VRProtocolSession, StringRequestInfo>
    {
        public VRProtocolServer()
            : base(new DefaultReceiveFilterFactory<VRProtocolReceiveFilter, StringRequestInfo>()) //使用默认的接受过滤器工厂 (DefaultReceiveFilterFactory)
        {
        }

        protected override void OnStarted() 
        {
            base.OnStarted();
            Console.WriteLine("服务器已启动");
        }


        /// <summary>  
        /// 输出断开连接信息  
        /// </summary>  
        /// <param name="session"></param>  
        /// <param name="reason"></param>  
       

        protected override void OnStopped()
        {
            base.OnStopped();
            Console.WriteLine("服务器已停止");
        }


    }
}
