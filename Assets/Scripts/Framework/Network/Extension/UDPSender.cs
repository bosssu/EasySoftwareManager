
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSender : MonoSingleton<UDPSender>
{
    public int remotePort = 7009;  //远端端口号
    public string remoteIpStr = "127.0.0.1"; //

    UdpClient client = null;

    IPAddress remoteIP;
    IPEndPoint remotePoint;

    protected override void Init()
    {
        base.Init();
        client = new UdpClient();
    }

    public void Send(byte[] data)
    {
        remoteIP = IPAddress.Parse(remoteIpStr);
        remotePoint = new IPEndPoint(remoteIP, remotePort);//实例化一个远程端点 
        client.Send(data, data.Length, remotePoint);//将数据发送到远程端点 
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        client.Close();
        client = null;
    }

}