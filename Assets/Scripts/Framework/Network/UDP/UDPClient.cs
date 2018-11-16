using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Threading;

public class UDPClient
{
    public delegate void BindCallback(byte[] data, IPEndPoint endPoint);
    private BindCallback bindCallback;

    IPEndPoint udpIp;
    byte[] recvData;

    Socket udpSocket;
    Thread recvThread;

    public bool BindSocket(ushort port, int bufferLength,BindCallback callback)
    {
        udpIp = new IPEndPoint(IPAddress.Any, port);

        UDPConnect();

        this.bindCallback = callback;

        recvData = new byte[bufferLength];

        recvThread = new Thread(UDPReceive);

        return true;
    }

    private void UDPConnect()
    {
        udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udpSocket.Bind(udpIp);
    }

    private bool isRunning = true;
    public void UDPReceive()
    {
        while (isRunning)
        {
            if (udpSocket == null || udpSocket.Available <1)
            {
                Thread.Sleep(100);
                continue;
            }

            lock (this)
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remote = (IPEndPoint)sender;

                int recvCount = udpSocket.ReceiveFrom(recvData, ref remote);

                if (bindCallback != null) bindCallback(recvData, sender);
            }
        }
    }

    public int UDPSend(byte[] data)
    {
        if (!udpSocket.Connected)
        {
            UDPConnect();
        }

        int sendCount = udpSocket.Send(data, data.Length, SocketFlags.None);

        return sendCount;
    }

    public void Dispose()
    {
        isRunning = false;
        if (udpSocket != null) udpSocket.Close();
        if (recvThread != null) recvThread.Abort();
        udpSocket = null;
        recvThread = null;
    }
}
