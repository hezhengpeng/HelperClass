using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpperLimbRehabilitationSys.Common
{
    class SoceketHelp 
    {
        public static void socektSend(VRProtocolSession session,string action, dynamic myData)
        {
             var F = Encoding.UTF8.GetBytes("!");//协议命令只占4位,如果占的位数长过协议，那么协议解析肯定会出错的
            string myDataStr = JsonConvert.SerializeObject(myData);
            byte[] dataBody = System.Text.Encoding.UTF8.GetBytes(myDataStr);
            var dataAction = System.Text.Encoding.UTF8.GetBytes(action);
            var dataLen = BitConverter.GetBytes(dataBody.Length + dataAction.Length);//int类型占4位，根据协议这里也只能4位，否则会出错

            var T = Encoding.UTF8.GetBytes("$");
            var sendData = new byte[14 + dataBody.Length];//命令加内容长度

            //校验和
            int csInt = 0;
            for (int i = 0; i < dataAction.Length; i++)
            {
                csInt += dataAction[i];
            }
            for (int i = 0; i < dataBody.Length; i++)
            {
                csInt += dataBody[i];
            }
            for (int i = 0; i < dataLen.Length; i++)
            {
                csInt += dataLen[i];
            }
            var cs = BitConverter.GetBytes(csInt);


            Array.ConstrainedCopy(F, 0, sendData, 0, 1);
            Array.ConstrainedCopy(dataLen, 0, sendData, 1, 4);
            Array.ConstrainedCopy(dataAction, 0, sendData, 5, 4);
            Array.ConstrainedCopy(dataBody, 0, sendData, 9, dataBody.Length);
            Array.ConstrainedCopy(cs, 0, sendData, 9 + dataBody.Length, 4);
            Array.ConstrainedCopy(T, 0, sendData, 13 + dataBody.Length, 1);

            ArraySegment<byte> datalast = new ArraySegment<byte>(sendData, 0, sendData.Count());

            session.Send(datalast);
        }
    }
}
