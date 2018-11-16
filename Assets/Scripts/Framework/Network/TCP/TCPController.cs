using UnityEngine;
using System.Collections;
using System;

public class TCPController : Singleton<TCPController> {

    private NetConnector netConnector;

    public override void Init()
    {
        base.Init();
    }

    public override void UnInit()
    {
        base.UnInit();

        if (netConnector != null)
            netConnector.Dispose();
        netConnector = null;
    }

    public void Connect(string ip,int port)
    {
        if(netConnector == null)
            netConnector = new NetConnector(ip, port);

        netConnector.Connect();
    }

    public void Disconnect()
    {
        netConnector.Disconnect();
    }

    public void SendData(byte[] data)
    {
        netConnector.SendMessage(data);
    }

}
