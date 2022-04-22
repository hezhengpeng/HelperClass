using Newtonsoft.Json;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreadSys.Common
{
    public class VRProtocolSession : AppSession<VRProtocolSession, StringRequestInfo>
    {
        // <summary>  
        /// 新连接  
        /// </summary>  
        protected override void OnSessionStarted()
        {  
            

        }

        /// <summary>  
        /// 未知的Command  
        /// </summary>  
        /// <param name="requestInfo"></param>  
        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            this.Send("\n\rNotKnow");
        }

        /// <summary>  
        /// 捕捉异常并输出  
        /// </summary>  
        /// <param name="e"></param>  
        protected override void HandleException(Exception e)
        {
            this.Send("\n\r异常: {0}", e.Message);
        }

        /// <summary>  
        /// 连接关闭  
        /// </summary>  
        /// <param name="reason"></param>  
        protected override void OnSessionClosed(CloseReason reason)
        {
            base.OnSessionClosed(reason);
            Console.WriteLine("session已关闭："+reason.ToString());
        }
    }  
}
