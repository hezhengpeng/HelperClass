using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// scoket通信帮助类(服务端)
/// </summary>
/// <param name="serverSocket"></param>
public delegate void ListenSuccessHanlder(Socket serverSocket);//监听socket成功
public delegate void ConnectSuccessHanlder(Socket clientSocket);//客户端socket连接
public delegate void ReceiveHanlder(string msg);//接收到socket消息
public delegate void SocketExitHanlder();//游戏客户端退出
public sealed class SocketHelper
{
    private static volatile SocketHelper instance;
    private static object syncRoot = new Object();
    private static string socketIP = "127.0.0.1";
    private static int port = 12356;
    private Socket serverSocket;//服务端socket
    private Socket clientSocket;//客户端socket
    private int socketFlag = 0;//socket连接状态(0:监听状态，1：未有客户端连接，2：已有客户端连接)
    private Thread listenThread;//监听线程
    private Thread receiveThread;//接收消息线程
    private Thread sendThread;//发送消息线程
    private static bool isSendFinsh = true;//发送完成
    #region 事件
    public event ListenSuccessHanlder ListenSuccessHanlder;
    public event ConnectSuccessHanlder ConnectSuccessHanlder;
    public event ReceiveHanlder ReceiveHanlder;
    public event SocketExitHanlder SocketExitHanlder;
    #endregion

    public static SocketHelper Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new SocketHelper();
                }
            }
            return instance;
        }
    }
    public void ConnectServer(string _socketIP, int _port)
    {
        try
        {

            socketIP = _socketIP;
            port = _port;
            IPAddress ip = IPAddress.Parse(socketIP);
            serverSocket = new Socket(AddressFamily.InterNetwork,
                                      SocketType.Stream,
                                      ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, port));
            serverSocket.Listen(1);
            socketFlag = 1;
            listenThread = new Thread(ListenClientConnect);
            listenThread.IsBackground = true;
            listenThread.Start();
            if (ListenSuccessHanlder != null)
                ListenSuccessHanlder(serverSocket);

        }
        catch (Exception ex)
        {

        }

    }

    public void Close()
    {
        ListenSuccessHanlder = null;
        ReceiveHanlder = null;
        ConnectSuccessHanlder = null;
        if (serverSocket == null)
            return;

        if (!serverSocket.Connected)
            return;

        try
        {
            serverSocket.Shutdown(SocketShutdown.Both);
        }
        catch
        {
        }

        try
        {
            serverSocket.Close();
        }
        catch
        {
        }
        try
        {
            if (receiveThread.IsAlive)
                receiveThread.Abort();
        }
        catch
        {

        }
        try
        {
            if (listenThread.IsAlive)
                listenThread.Abort();
        }
        catch
        {

        }
    }
    /// <summary>
    /// 监听线程
    /// </summary>
    private void ListenClientConnect()
    {
        try
        {
            clientSocket = serverSocket.Accept();
            receiveThread = new Thread(ReceiveMessage);
            receiveThread.IsBackground = true;
            receiveThread.Start(clientSocket);
            socketFlag = 2;
            if (ConnectSuccessHanlder != null)
                ConnectSuccessHanlder(serverSocket);
            //sendThread = new Thread(SendMessage);
            //sendThread.IsBackground = true;
            //sendThread.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }


    private void Send(string str)
    {
        if (socketFlag != 2)
        {
            Console.WriteLine("未连接上");
            return;
        }
        isSendFinsh = false;
        byte[] buffer = Encoding.UTF8.GetBytes(str);
        clientSocket.Send(buffer);
        Console.WriteLine("发送消息");
    }
    #region 等待接收socket信息

    private void ReceiveMessage(object o)
    {
        Socket mClientSocket = o as Socket;
        try
        {

            while (true)
            {

                //Console.Write("接收到数据");
                string recStr = "";
                byte[] recByte = new byte[8192 * 8];
                int receiveNumber = mClientSocket.Receive(recByte);
                UTF8Encoding utf8 = new UTF8Encoding();
                recStr += utf8.GetString(recByte, 0, receiveNumber);
                ReceiveHanlder?.Invoke(recStr);


            }
        }
        catch (Exception ex)
        {
          //  MessageBox.Show(ex.ToString());
            listenThread = new Thread(ListenClientConnect);
            listenThread.IsBackground = true;
            listenThread.Start();
            if (SocketExitHanlder != null)
            {
                SocketExitHanlder();
            }
        }



    }
    #endregion

    #region 发送消息
    public void SendSocketMsg(string str)
    {
        if (socketFlag != 2)
        {
            LogHelper.Instance.LogInfo("客户端未连接上");
            return;
        }
        //Console.WriteLine(str);
        byte[] buffer = Encoding.UTF8.GetBytes(str);
        clientSocket.Send(buffer);
        //socketTasks.Push(str);
    }
    #endregion



}

