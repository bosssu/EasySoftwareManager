using UnityEngine;
using System;
using System.Net.Sockets;

public class MyTcpClient
{
    private int port = 5999;
    private string serverIp = "192.168.1.134";

    private const int BufferSize = 1024;
    private byte[] buffer;
    private TcpClient client;
    private NetworkStream streamToServer;

    public enum ClientStateCode
    {
        ConnectError,
        ConnectUnknowError,
        ConnectSuccesss,

        SendError,
        SendUnknowError,
        SendSuccess,

        RecvError,
        RecvUnknowError,
        RecvSuccess
    }

    public delegate void OnOneMsgReceived(byte[] bfs);
    private OnOneMsgReceived onOneMsgReceived;
    public delegate void CodeCallback(bool isSuccess, ClientStateCode errorCode, string msg);
    private CodeCallback connectCallback;
    private CodeCallback sendCallback;
    private CodeCallback recvCallback;
    private StickClass stickClass;

    public MyTcpClient(string serverIp, int port, CodeCallback connectCallback, CodeCallback sendCallback, CodeCallback recvCallback,OnOneMsgReceived oneMsgReceived)
    {
        stickClass = new global::StickClass(4, OneMsgReceived);

        this.onOneMsgReceived = oneMsgReceived;
        this.connectCallback = connectCallback;
        this.sendCallback = sendCallback;
        this.recvCallback = recvCallback;

        client = new TcpClient();
        client.SendTimeout = 10000;
        client.ReceiveTimeout = 10000;

        this.serverIp = serverIp;
        this.port = port;

        buffer = new byte[BufferSize];
    }

    public bool IsConnected()
    {
        if (client != null) return client.Connected;

        return false;
    }

    private void OneMsgReceived(byte[] bfs)
    {
        if (onOneMsgReceived != null) onOneMsgReceived(bfs);
    }

    public void RunClient()
    {
      
        try
        {
            if (client != null && client.Connected)
            {
                if (connectCallback != null) connectCallback(false, ClientStateCode.ConnectError, "已经连接");
                return;
            }

            client = new TcpClient();
            client.Connect(serverIp, port);      // 与服务器连接
        }
        catch (Exception ex)
        {
            if (connectCallback != null) connectCallback(false, ClientStateCode.ConnectUnknowError, "连接失败" + ex.Message);
            return;
        }

        if (connectCallback != null) connectCallback(true, ClientStateCode.ConnectSuccesss, "");

        // 打印连接到的服务端信息
        Debug.LogFormat("Server Connected！{0} --> {1}",
            client.Client.LocalEndPoint, client.Client.RemoteEndPoint);

        streamToServer = client.GetStream();

        BeginReadMessage();
    }

    public void StopClient()
    {
        OnDispose();
    }

    #region ReadMsg

    private void BeginReadMessage()
    {
        lock (streamToServer)
        {
            AsyncCallback callBack = new AsyncCallback(ReadComplete);
            streamToServer.BeginRead(buffer, 0, BufferSize, callBack, null);
        }
    }

    private void ReadComplete(IAsyncResult ar)
    {
        int bytesRead;

        try
        {
            lock (streamToServer)
            {
                bytesRead = streamToServer.EndRead(ar);
                stickClass.ProcessRecvData(buffer, bytesRead);
                if (recvCallback != null) recvCallback(true, ClientStateCode.RecvSuccess,"");
            }

            //Debug.LogFormat("Received: {0}", bytesRead);
            Array.Clear(buffer, 0, buffer.Length);      // 清空缓存，避免脏读

            lock (streamToServer)
            {
                AsyncCallback callBack = new AsyncCallback(ReadComplete);
                streamToServer.BeginRead(buffer, 0, BufferSize, callBack, null);
            }
        }
        catch (Exception ex)
        {
            if (streamToServer != null)
                streamToServer.Dispose();
            client.Close();

            if (recvCallback != null) recvCallback(false, ClientStateCode.RecvUnknowError, "接受消息异常" + ex.Message);
        }
    }

    #endregion


    #region SendMsg

    public void SendMessage(byte[] msg)
    {
        try
        {
            streamToServer.Write(msg, 0, msg.Length); // 发往服务器
            if (sendCallback != null) sendCallback(true, ClientStateCode.SendSuccess, "");
        }
        catch (Exception ex)
        {
            if (sendCallback != null) sendCallback(false, ClientStateCode.SendUnknowError, "发送消息失败" + ex.Message);
            return;
        }
    }

    #endregion


    void OnDispose()
    {
        if (client != null)
            client.Close();
        client = null;
    }
}
