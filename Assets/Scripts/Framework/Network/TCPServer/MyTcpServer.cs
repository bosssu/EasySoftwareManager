using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

/// <summary>
/// 服务端
/// </summary>
public class MyTcpServer
{

    private Socket ServerSocket = null;//服务端  
    public Dictionary<string, MySession> dic_ClientSocket = new Dictionary<string, MySession>();//tcp客户端字典
    private Dictionary<string, Thread> dic_ClientThread = new Dictionary<string, Thread>();//线程字典,每新增一个连接就添加一条线程
    private bool Flag_Listen = true;//监听客户端连接的标志

    public bool IsAlive { get; set; }

    /// <summary>
    /// 启动服务
    /// </summary>
    /// <param name="port">端口号</param>
    public bool OpenServer(int port)
    {
        try
        {
            Flag_Listen = true;
            // 创建负责监听的套接字，注意其中的参数；
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // 创建包含ip和端口号的网络节点对象；
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            try
            {
                // 将负责监听的套接字绑定到唯一的ip和端口上；
                ServerSocket.Bind(endPoint);
            }
            catch
            {
                return false;
            }
            // 设置监听队列的长度；
            ServerSocket.Listen(100);
            // 创建负责监听的线程；
            Thread Thread_ServerListen = new Thread(ListenConnecting);
            Thread_ServerListen.IsBackground = true;
            Thread_ServerListen.Start();

            IsAlive = true;
            return true;
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// 关闭服务
    /// </summary>
    public void CloseServer()
    {
        lock (dic_ClientSocket)
        {
            foreach (var item in dic_ClientSocket)
            {
                item.Value.Close();//关闭每一个连接
            }
            dic_ClientSocket.Clear();//清除字典
        }
        lock (dic_ClientThread)
        {
            foreach (var item in dic_ClientThread)
            {
                item.Value.Abort();//停止线程
            }
            dic_ClientThread.Clear();
        }
        Flag_Listen = false;
        IsAlive = false;
        //ServerSocket.Shutdown(SocketShutdown.Both);//服务端不能主动关闭连接,需要把监听到的连接逐个关闭
        if (ServerSocket != null)
            ServerSocket.Close();

    }
    /// <summary>
    /// 监听客户端请求的方法；
    /// </summary>
    private void ListenConnecting()
    {
        while (Flag_Listen)  // 持续不断的监听客户端的连接请求；
        {
            try
            {
                Socket sokConnection = ServerSocket.Accept(); // 一旦监听到一个客户端的请求，就返回一个与该客户端通信的 套接字；
                                                              // 将与客户端连接的 套接字 对象添加到集合中；
                string str_EndPoint = sokConnection.RemoteEndPoint.ToString();
                MySession myTcpClient = new MySession() { TcpSocket = sokConnection };
                //创建线程接收数据
                Thread th_ReceiveData = new Thread(ReceiveData);
                th_ReceiveData.IsBackground = true;
                th_ReceiveData.Start(myTcpClient);
                //把线程及客户连接加入字典
                dic_ClientThread.Add(str_EndPoint, th_ReceiveData);
                dic_ClientSocket.Add(str_EndPoint, myTcpClient);
                Debug.LogFormat("endPoint: {0} connected", str_EndPoint);
            }
            catch
            {

            }
            Thread.Sleep(200);
        }
    }
    /// <summary>
    /// 接收数据
    /// </summary>
    /// <param name="sokConnectionparn"></param>
    private void ReceiveData(object sokConnectionparn)
    {
        MySession tcpClient = sokConnectionparn as MySession;
        Socket socketClient = tcpClient.TcpSocket;
        bool Flag_Receive = true;

        while (Flag_Receive)
        {
            try
            {
                // 定义一个1k的缓存区；
                byte[] arrMsgRec = new byte[1024];
                // 将接受到的数据存入到输入  arrMsgRec中；
                int length = -1;
                try
                {
                    length = socketClient.Receive(arrMsgRec); // 接收数据，并返回数据的长度；
                }
                catch
                {
                    Flag_Receive = false;
                    // 从通信线程集合中删除被中断连接的通信线程对象；
                    string keystr = socketClient.RemoteEndPoint.ToString();
                    dic_ClientSocket.Remove(keystr);//删除客户端字典中该socket
                    dic_ClientThread[keystr].Abort();//关闭线程
                    dic_ClientThread.Remove(keystr);//删除字典中该线程

                    tcpClient = null;
                    socketClient = null;
                    break;
                }
                byte[] buf = new byte[length];
                Array.Copy(arrMsgRec, buf, length);
                lock (tcpClient.m_Buffer)
                {
                    tcpClient.AddQueue(buf);
                }
            }
            catch
            {

            }
            Thread.Sleep(100);
        }
    }
    /// <summary>
    /// 发送数据给指定的客户端
    /// </summary>
    /// <param name="_endPoint">客户端套接字</param>
    /// <param name="_buf">发送的数组</param>
    /// <returns></returns>
    public bool SendData(string _endPoint, byte[] _buf)
    {
        MySession myT = new MySession();
        if (dic_ClientSocket.TryGetValue(_endPoint, out myT))
        {
            myT.Send(_buf);
            return true;
        }
        else
        {
            return false;
        }
    }
}
