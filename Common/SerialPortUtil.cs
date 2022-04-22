using System;
using System.Text;
using System.IO.Ports;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Threading;
using TreadSys.Common;
using System.ComponentModel;


namespace TreadSys
{
    /// <summary>
    /// 串口开发辅助类
    /// </summary>
    public class SerialPortUtil
    {
        /// <summary>
        /// 接收事件是否有效 false表示有效
        /// </summary>
        public bool ReceiveEventFlag = false;
        /// <summary>
        /// 结束符比特
        /// </summary>
        public byte EndByte = 0x23;//string End = "#";

        /// <summary>
        /// 完整协议的记录处理事件
        /// </summary>
        public event DataReceivedEventHandler DataReceived;
        public event SerialErrorReceivedEventHandler Error;

        #region 变量属性
        
        public string _portName = ConfigurationClass.GetSettingString("Com");//串口号
        private SerialPortBaudRates _baudRate = SerialPortBaudRates.BaudRate_115200;//波特率115200
        private Parity _parity = Parity.None;//校验位
        private StopBits _stopBits = StopBits.One;//停止位
        private SerialPortDatabits _dataBits = SerialPortDatabits.EightBits;//数据位

        private SerialPort comPort = new SerialPort();
      
        public bool EnableA8 = false;
        public bool Enable58 = false;
        public bool EnableF8 = false;
        #endregion

        #region 构造函数

        

       
        public int pollcount = 0;
        public List<byte[]> sendData = null;//轮询数据
       /// <summary>
       /// 根据需要进行传感器收发
       /// </summary>

        public SerialPortUtil()
        {

            //comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
            comPort.ErrorReceived += new SerialErrorReceivedEventHandler(comPort_ErrorReceived);
        } 

	    #endregion

        /// <summary>
        /// 端口是否已经打开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return comPort.IsOpen;
            }
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        public void OpenPort()
        {

            if (comPort.IsOpen) 
                comPort.Close();

            try
            {
                comPort.PortName = _portName;
                comPort.BaudRate = (int)_baudRate;
                comPort.Parity = _parity;
                comPort.DataBits = (int)_dataBits;
                comPort.StopBits = _stopBits;

                comPort.Open();
            }
            catch(Exception ex)
            {
                 LogClass.WriteLog(ex.Message);
            }
          
        }

        /// <summary>
        /// 关闭端口
        /// </summary>
        public void ClosePort()
        {
            if (comPort.IsOpen) 
                comPort.Close();
        }

        /// <summary>
        /// 丢弃来自串行驱动程序的接收和发送缓冲区的数据
        /// </summary>
        public void DiscardBuffer()
        {
            if(comPort.IsOpen)
            {
                comPort.DiscardInBuffer();
                comPort.DiscardOutBuffer();
            }
            
        }

    
        /// <summary>
        /// 数据接收处理
        /// </summary>
        //void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    //禁止接收事件时直接退出
        //    if (ReceiveEventFlag ) 
        //        return;

        //    #region 根据结束字节来判断是否全部获取完成
        //    if (comPort.BytesToRead <= 0)
        //    {
        //        return;
        //    }
        //    //开辟接收缓冲区
        //    int countCache = comPort.BytesToRead;
        //    byte[] ReDatas = new byte[countCache];
        //    if (countCache < 7)
        //    {
        //        //从串口读取数据
        //        comPort.Read(ReDatas, 0, ReDatas.Length);
        //        //实现数据的解码与显示
        //        for (int i = 0; i < ReDatas.Length; i++)
        //        {
        //            ld.Add(ReDatas[i]);
        //        }
        //        if (ld.Count == 7)
        //        {
        //            string LTem = ToHexString(ld.ToArray());
        //            //触发整条记录的处理
        //            if (DataReceived != null)
        //            {
        //                DataReceived(new DataReceivedEventArgs(LTem));
        //            }
        //            ld.Clear();
        //        }

        //    }
        //    else if (countCache == 7)
        //    {
        //        //从串口读取数据
        //        comPort.Read(ReDatas, 0, ReDatas.Length);
        //        //实现数据的解码与显示
        //        string LTem = ToHexString(ReDatas);
        //        //触发整条记录的处理
        //        if (DataReceived != null)
        //        {
        //            DataReceived(new DataReceivedEventArgs(LTem));
        //        }  
        //    }
        //    else
        //    {
        //        comPort.DiscardInBuffer();
        //    }

        //    if(sendData.Count!=0)
        //    {
        //        Thread.Sleep(100);
        //        SendData();          
        //    }
        //    else
        //    {
        //        int a = 1;
        //    }

       
        //    #endregion
           
           
        //    //字符转换
        //    //string readString = System.Text.Encoding.Default.GetString(_byteData.ToArray(), 0, _byteData.Count);
            
           
        //}

        /// <summary>
        /// 错误处理函数
        /// </summary>
        void comPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (Error != null)
            {
                Error(sender, e);
            }
        }

        #region 数据写入操作



        /// <summary>
        /// 开始轮询
        /// </summary>
        /// <param name="msg"></param>
        public void SendData(byte[] data)
        {
            if (IsOpen && comPort != null)
            {
                sendData = new List<byte[]>();
                //byte[] bt = new byte[] { 0x58, 0x03, 0x00, 0x62, 0x00, 0x01, 0x29, 0x1D };
                sendData.Add(data);
               


                if (sendData.Count != 0)
                {

                    pollcount++;
                    pollcount = pollcount % sendData.Count;

                    try
                    {
                        //将消息传递给串口
                        comPort.Write(sendData[pollcount], 0, sendData[pollcount].Length);
                        return;
                    }
                    catch (Exception ex)
                    {
                        LogClass.WriteLog(ex.Message);
                    }
                }



            }
            else
            {
                Console.WriteLine("串口未开启", "错误");
            }
            return;
        }

        /// <summary>
        /// 开始轮询
        /// </summary>
        /// <param name="msg">写入端口的字节数组</param>
        public void Start()
        {
            if (!(comPort.IsOpen))
                comPort.Open();
        }   
        List<byte> ld = new List<byte>();
        void DataReceivedR() 
        {
            while (comPort.IsOpen)
            {
                //开辟接收缓冲区
                int countCache = comPort.BytesToRead;
                byte[] ReDatas = new byte[countCache];
       
                if (countCache < 7&&countCache>0)
                {
                    //从串口读取数据
                    comPort.Read(ReDatas, 0, ReDatas.Length);
                    //实现数据的解码与显示
                    for (int i = 0; i < ReDatas.Length; i++)
                    {
                        ld.Add(ReDatas[i]);
                    }
                    if (ld.Count == 7)
                    {
                        string LTem = ToHexString(ld.ToArray());
                        //触发整条记录的处理
                        if (DataReceived != null)
                        {
                            DataReceived(new DataReceivedEventArgs(LTem));
                        }
                        ld.Clear();
                    }

                }
                else if (countCache == 7)
                {
                    try
                    {
                        //从串口读取数据
                        comPort.Read(ReDatas, 0, ReDatas.Length);
                        //实现数据的解码与显示
                        string LTem = ToHexString(ReDatas);
                        //触发整条记录的处理
                        if (DataReceived != null)
                        {
                            DataReceived(new DataReceivedEventArgs(LTem));
                        }
                    }
                    catch(Exception ex)
                    {
                        LogClass.WriteLog(ex.Message);
                    }
                    
                }
             
                else
                {
                    comPort.DiscardInBuffer();
                }

                if (sendData.Count != 0)
                {
                    Thread.Sleep(55);
                    //SendData();
                }
                else
                {
                    int a = 1;//
                }

            }
            LogClass.WriteLog("串口线程已经关闭;");

            
        }
       

        #endregion

        #region 常用的列表数据获取和绑定操作

        /// <summary>
        /// 封装获取串口号列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// 设置串口号
        /// </summary>
        /// <param name="obj"></param>
        public static void SetPortNameValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (string str in SerialPort.GetPortNames())
            {
                obj.Items.Add(str);
            }
        }

        /// <summary>
        /// 设置波特率
        /// </summary>
        public static void SetBauRateValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (SerialPortBaudRates rate in Enum.GetValues(typeof(SerialPortBaudRates)))
            {
                obj.Items.Add(((int)rate).ToString());
            }
        }

        /// <summary>
        /// 设置数据位
        /// </summary>
        public static void SetDataBitsValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (SerialPortDatabits databit in Enum.GetValues(typeof(SerialPortDatabits)))
            {
                obj.Items.Add(((int)databit).ToString());
            }
        }

        /// <summary>
        /// 设置校验位列表
        /// </summary>
        public static  void SetParityValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (string str in Enum.GetNames(typeof(Parity)))
            {
                obj.Items.Add(str);
            }
            //foreach (Parity party in Enum.GetValues(typeof(Parity)))
            //{
            //    obj.Items.Add(((int)party).ToString());
            //}
        }

        /// <summary>
        /// 设置停止位
        /// </summary>
        public static void SetStopBitValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (string str in Enum.GetNames(typeof(StopBits)))
            {
                obj.Items.Add(str);
            }
            //foreach (StopBits stopbit in Enum.GetValues(typeof(StopBits)))
            //{
            //    obj.Items.Add(((int)stopbit).ToString());
            //}   
        }

        #endregion

        #region 格式转换

        private byte[] strToHexByte(string hexString)// AE00CF => "AE00CF "
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0) hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Replace(" ", ""), 16);
            return returnBytes;
        }
        private string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;

            if (bytes != null)
            {

                StringBuilder strB = new StringBuilder();
                StringBuilder strA = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    strA.Append(bytes[i].ToString());
                    strB.Append(bytes[i].ToString("X2"));

                }
                hexString = strB.ToString();

            } return hexString;

        }

      
        #endregion

      

     
    }

    public class DataReceivedEventArgs : EventArgs
    {
        public string DataReceived;
        public DataReceivedEventArgs(string m_DataReceived)
        {
            this.DataReceived = m_DataReceived;
        }
    }

    public delegate void DataReceivedEventHandler(DataReceivedEventArgs e);


    /// <summary>
    /// 串口数据位列表（5,6,7,8）
    /// </summary>
    public enum SerialPortDatabits : int
    {
        FiveBits = 5,
        SixBits = 6,
        SeventBits = 7,
        EightBits = 8
    }

    /// <summary>
    /// 串口波特率列表。
    /// 75,110,150,300,600,1200,2400,4800,9600,14400,19200,28800,38400,56000,57600,
    /// 115200,128000,230400,256000
    /// </summary>
    public enum SerialPortBaudRates : int
    {
        BaudRate_75 = 75,
        BaudRate_110 = 110,
        BaudRate_150 = 150,
        BaudRate_300 = 300,
        BaudRate_600 = 600,
        BaudRate_1200 = 1200,
        BaudRate_2400 = 2400,
        BaudRate_4800 = 4800,
        BaudRate_9600 = 9600,
        BaudRate_14400 = 14400,
        BaudRate_19200 = 19200,
        BaudRate_28800 = 28800,
        BaudRate_38400 = 38400,
        BaudRate_56000 = 56000,
        BaudRate_57600 = 57600,
        BaudRate_115200 = 115200,
        BaudRate_128000 = 128000,
        BaudRate_230400 = 230400,
        BaudRate_256000 = 256000
    }
}
