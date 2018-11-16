
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSender : MonoBehaviour
{
    public static UDPSender _Instance;

    public int remotePort = 7009;  //远端端口号
    public string remoteIpStr = "127.0.0.1"; //

    UdpClient client = null;

    IPAddress remoteIP;
    IPEndPoint remotePoint;

    void Awake()
    {
        _Instance = this;
    }

    void Start()
    {
        Console.WriteLine("初始化");

        client = new UdpClient();
    }

    public void Send(byte[] data)
    {
        remoteIP = IPAddress.Parse(remoteIpStr);
        remotePoint = new IPEndPoint(remoteIP, remotePort);//实例化一个远程端点 
        client.Send(data, data.Length, remotePoint);//将数据发送到远程端点 
    }

    void OnApplicationQuit()
    {
        client.Close();
    }

}