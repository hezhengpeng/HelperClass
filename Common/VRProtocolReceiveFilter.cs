using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreadSys.Common
{
    public class VRProtocolReceiveFilter : FixedHeaderReceiveFilter<StringRequestInfo>
    {
        public VRProtocolReceiveFilter()
            : base(5)
        {

        }

        /// <summary>
        /// 获取数据域和尾长度
        /// </summary>
        /// <param name="header"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            var DataLength = new byte[4];
            Array.Copy(header, offset + 1, DataLength, 0, 4);
            int lengthtotal = BitConverter.ToInt32(DataLength, 0) + 5;
            return lengthtotal;
        }

        /// <summary>
        /// 实现帧内容解析
        /// </summary>
        /// <param name="header"></param>
        /// <param name="bodyBuffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override StringRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            byte[] actionbt=new byte[4];;
            Array.ConstrainedCopy(bodyBuffer, offset, actionbt, 0, 4);
            //返回有效数据
            int action = System.BitConverter.ToInt32(actionbt, 0); 
            string data = Encoding.UTF8.GetString(bodyBuffer, offset + 4, length - 9);
            return new StringRequestInfo(action.ToString(), data, null); ;

        }

    }
}
