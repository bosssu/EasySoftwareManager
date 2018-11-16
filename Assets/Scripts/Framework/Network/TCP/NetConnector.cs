using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class NetConnector {

    public Queue<byte[]> msgToSends;
    public Queue<byte[]> msgToReceives;

    private MyTcpClient tcpClient;

    Thread sendThread;
    Thread recvThread;

    public NetConnector(string ip, int port)
    {
        tcpClient = new MyTcpClient(ip, port, ConnectCallback, SendCallback, RecvCallback, OnOneMsgReceived);

        msgToReceives = new Queue<byte[]>();
        msgToSends = new Queue<byte[]>();
    }

    public void Connect()
    {
        tcpClient.RunClient();
    }

    public void Disconnect()
    {
        tcpClient.StopClient();
    }

    public void SendMessage(byte[] data)
    {
        msgToSends.Enqueue(data);
    }


    private void OnOneMsgReceived(byte[] data)
    {
        msgToReceives.Enqueue(data);
        //Debug.Log(msg.GetMsgID());
    }

    private void RecvCallback(bool isSuccess, MyTcpClient.ClientStateCode errorCode, string msg)
    {
        //接受消息的回调
        if (isSuccess)
        {
            if (recvThread == null)
            {
                recvThread = new Thread(ReceiveData);
                recvThread.Start();
            }
        }
    }

    private void SendCallback(bool isSuccess, MyTcpClient.ClientStateCode errorCode, string msg)
    {
        //发送消息的会调
        //Debug.Log("send: " +msg);
    }

    private void ConnectCallback(bool isSuccess, MyTcpClient.ClientStateCode errorCode, string msg)
    {
        if (isSuccess)
        {
            sendThread = new Thread(SendData);
            sendThread.Start();
        }
    }

    private void ReceiveData()
    {
        while (tcpClient.IsConnected())
        {
            if (msgToReceives.Count > 0)
            {
                //消息框架发送
                byte[] data_recv = msgToReceives.Dequeue();
                NetManager.Instance.ProcessData(data_recv);

                Thread.Sleep(100);
            }
        }
    }

    private void SendData()
    {
        while (tcpClient.IsConnected())
        {
            if (msgToSends.Count > 0)
            {
                lock (msgToSends)
                {
                    byte[] msg = msgToSends.Dequeue();
                    tcpClient.SendMessage(msg);
                }

                Thread.Sleep(100);
            }
        }
    }

    public void Dispose()
    {
        if (sendThread != null) sendThread.Abort();
        if (tcpClient != null) tcpClient.StopClient();
        sendThread = null;
        tcpClient = null;
    }
}
