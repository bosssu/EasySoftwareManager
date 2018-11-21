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

    public UDPClient(ushort port,string remoteIp, int bufferLength,BindCallback callback)
    {

        UDPBind(port,remoteIp);

        this.bindCallback = callback;

        recvData = new byte[bufferLength];

        recvThread = new Thread(UDPReceive);

    }

    private void UDPBind(ushort port,string ip)
    {
        udpIp = new IPEndPoint(IPAddress.Parse(ip), port);
        udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udpSocket.Bind(udpIp);
        Debug.Log("UDP 绑定");
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
        return udpSocket.Send(data, data.Length, SocketFlags.None);
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
