using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Net;

public class UDPController : Singleton<UDPController>
{

    private UDPClient udpClient;

    public override void Init()
    {
        base.Init();
    }

    public override void UnInit()
    {
        base.UnInit();

        if (udpClient != null)
            udpClient.Dispose();
    }

    public void Bind(ushort port,int bufferLength)
    {
        if (udpClient == null)
            udpClient = new UDPClient();

        udpClient.BindSocket(port, bufferLength, UDPRecvCallback);
    }

    public void SendData(byte[] data)
    {
        udpClient.UDPSend(data);
    }

    private void UDPRecvCallback(byte[] buffer, IPEndPoint endpoint)
    {
        NetManager.Instance.ProcessData(buffer);
    }
}
