
using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class UDPReceive : MonoBehaviour {

    public static UDPReceive _Instance;

    public Action<byte[]> MessageRecevdEvent;

    Thread receiveThread;
    UdpClient client;
    public int port = 7008;

    void Awake()
    {
        _Instance = this;
    }

    void Start()
    {
        Init();
    }

    public void Init()
    {
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
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

    public void OnApplicationQuit()
    {
        
        if (receiveThread.IsAlive)
        {
            receiveThread.Abort();
            client.Close();
        }
        
    }
}