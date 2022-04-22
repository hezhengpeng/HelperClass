using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UpperLimbRehabilitationSys.Common
{
    class Serhelp
    {   /// <summary>
        /// 对象转字符串（序列化后转Base64编码字符串）
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>字符串</returns>
        public static string ObjectToString<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return Convert.ToBase64String(buffer);
            }
        }

        /// <summary>
        /// 字符串转对象（Base64编码字符串反序列化为对象）
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>对象</returns>
        public static T StringToObject<T>(string str)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] bytes = Convert.FromBase64String(str);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;
                IFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
