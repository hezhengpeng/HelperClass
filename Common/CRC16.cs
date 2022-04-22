using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreadSys.Common
{
    class CRC16
    {
        /// <summary>
        /// 判断数据中crc是否正确
        /// </summary>
        /// <param name="datas">传入的数据后两位是crc</param>
        /// <returns></returns>
        public static bool IsCrcOK(byte[] datas)
        {
            int length = datas.Length - 2;

            byte[] bytes = new byte[length];
            Array.Copy(datas, 0, bytes, 0, length);
            byte[] getCrc = GetModbusCrc16(bytes);

            if (getCrc[0] == datas[length] && getCrc[1] == datas[length + 1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //返回高低位
        public static byte[] GetModbusCrc16T(byte[] bytes)

        {

            byte crcRegister_H = 0xFF, crcRegister_L = 0xFF;// 预置一个值为 0xFFFF 的 16 位寄存器



            byte polynomialCode_H = 0xA0, polynomialCode_L = 0x01;// 多项式码 0xA001



            for (int i = 0; i < bytes.Length; i++)

            {

                crcRegister_L = (byte)(crcRegister_L ^ bytes[i]);



                for (int j = 0; j < 8; j++)

                {

                    byte tempCRC_H = crcRegister_H;

                    byte tempCRC_L = crcRegister_L;



                    crcRegister_H = (byte)(crcRegister_H >> 1);

                    crcRegister_L = (byte)(crcRegister_L >> 1);

                    // 高位右移前最后 1 位应该是低位右移后的第 1 位：如果高位最后一位为 1 则低位右移后前面补 1

                    if ((tempCRC_H & 0x01) == 0x01)

                    {

                        crcRegister_L = (byte)(crcRegister_L | 0x80);

                    }



                    if ((tempCRC_L & 0x01) == 0x01)

                    {

                        crcRegister_H = (byte)(crcRegister_H ^ polynomialCode_H);

                        crcRegister_L = (byte)(crcRegister_L ^ polynomialCode_L);

                    }

                }

            }



            return new byte[] { crcRegister_L, crcRegister_H };

        }

        /// <summary>
        /// CRC16 
        /// </summary>
        /// <param name="data">要进行计算的数组</param>
        /// <returns>计算后的数组</returns>
        private static byte[] TurnCRC16(byte[] data)
        {
            byte[] returnVal = new byte[2];
            byte CRC16Lo, CRC16Hi, CL, CH, SaveHi, SaveLo;
            int i, Flag;
            CRC16Lo = 0xFF;
            CRC16Hi = 0xFF;
            CL = 0x86;
            CH = 0x68;
            for (i = 0; i < data.Length; i++)
            {
                CRC16Lo = (byte)(CRC16Lo ^ data[i]);//每一个数据与CRC寄存器进行异或
                for (Flag = 0; Flag <= 7; Flag++)
                {
                    SaveHi = CRC16Hi;
                    SaveLo = CRC16Lo;
                    CRC16Hi = (byte)(CRC16Hi >> 1);//高位右移一位
                    CRC16Lo = (byte)(CRC16Lo >> 1);//低位右移一位
                    if ((SaveHi & 0x01) == 0x01)//如果高位字节最后一位为
                    {
                        CRC16Lo = (byte)(CRC16Lo | 0x80);//则低位字节右移后前面补 否则自动补0
                    }
                    if ((SaveLo & 0x01) == 0x01)//如果LSB为1，则与多项式码进行异或
                    {
                        CRC16Hi = (byte)(CRC16Hi ^ CH);
                        CRC16Lo = (byte)(CRC16Lo ^ CL);
                    }
                }
            }
            returnVal[0] = CRC16Hi;//CRC高位
            returnVal[1] = CRC16Lo;//CRC低位
            return returnVal;
        }
        /// <summary>
        /// 传入数据添加两位crc
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static byte[] GetCRCDatas(byte[] header, byte[] datas)
        {

            int length = datas.Length;
            byte[] crc16 = CRC16t3(datas);
            byte[] crcDatas = new byte[length + 5];
            Array.Copy(header,0,  crcDatas,0,  3);
            Array.Copy(datas,0,crcDatas,3, length);
            Array.Copy(crc16,0,  crcDatas,3+length, 2);
            return crcDatas;
        }
        public static byte[] CRC16t3(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;
                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8); //高位置
                byte lo = (byte)(crc & 0x00FF); //低位置
                return new byte[] { lo, hi };
            }
            return new byte[] { 0, 0 };
        }


        private static byte[] GetModbusCrc16(byte[] bytes)
        {
            byte crcRegister_H = 0xFF, crcRegister_L = 0xFF;// 预置一个值为 0xFFFF 的 16 位寄存器

            byte polynomialCode_H = 0xA0, polynomialCode_L = 0x01;// 多项式码 0xA001

            for (int i = 0; i < bytes.Length; i++)
            {
                crcRegister_L = (byte)(crcRegister_L ^ bytes[i]);

                for (int j = 0; j < 8; j++)
                {
                    byte tempCRC_H = crcRegister_H;
                    byte tempCRC_L = crcRegister_L;

                    crcRegister_H = (byte)(crcRegister_H >> 1);
                    crcRegister_L = (byte)(crcRegister_L >> 1);
                    // 高位右移前最后 1 位应该是低位右移后的第 1 位：如果高位最后一位为 1 则低位右移后前面补 1
                    if ((tempCRC_H & 0x01) == 0x01)
                    {
                        crcRegister_L = (byte)(crcRegister_L | 0x80);
                    }

                    if ((tempCRC_L & 0x01) == 0x01)
                    {
                        crcRegister_H = (byte)(crcRegister_H ^ polynomialCode_H);
                        crcRegister_L = (byte)(crcRegister_L ^ polynomialCode_L);
                    }
                }
            }

            return new byte[] { crcRegister_L, crcRegister_H };
        }


    }
}
