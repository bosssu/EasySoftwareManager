
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class UDPReceive : MonoSingleton<UDPReceive> {

    public Action<byte[]> MessageRecevdEvent;

    Thread receiveThread;
    UdpClient client;
    public int port = 7008;
    bool _thread_flag;

    public bool IsAlive
    {
        get {
            return _thread_flag;
        }
        set {
            _thread_flag = value;
            if (value)
            {
                receiveThread = new Thread(new ThreadStart(ReceiveData));
                receiveThread.IsBackground = true;
                receiveThread.Start();
                _thread_flag = true;
            }
            else
            {
                _thread_flag = false;
                client.Close();
                receiveThread.Abort();
                client = null;
                receiveThread = null;
            }
        }
    }

    protected override void Init()
    {
        base.Init();
        IsAlive = true;
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (_thread_flag)
        {
            if (client == null || client.Available < 1)
            {
                Thread.Sleep(100);
                continue;
            }

            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                if (data.Length > 0)
                    if (MessageRecevdEvent != null) MessageRecevdEvent(data);
            }
            catch (Exception err)
            {
                Debug.LogError(err.Message);
                return;
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        IsAlive = false;
    }
}