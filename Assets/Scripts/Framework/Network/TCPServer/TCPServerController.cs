using System;
using System.Collections;
using System.Collections.Generic;

public class TCPServerController : MonoSingleton<TCPServerController> {

    const int head_buffer_size = 4;

    private int _port;

    private string _ip;

    private MyTcpServer _tcpServer;

    public Dictionary<string, MySession> Dict_ConnectedClients
    {
        get {
            return this._tcpServer.dic_ClientSocket;
        }
    }

    protected override void Init()
    {
        base.Init();
        _tcpServer = new MyTcpServer();
    }

    public void OpenServer(int port)
    {
        this._port = port;
        if (!_tcpServer.IsAlive)
        {
            _tcpServer.OpenServer(port);
        }
    }

    public void StopServer()
    {
        _tcpServer.CloseServer();
    }

    public void SendData(string End_Point, byte[] bfs)
    {
        this._ip = End_Point;

        byte[] data_to_send = new byte[head_buffer_size + bfs.Length];
        byte[] headbuf = BitConverter.GetBytes(bfs.Length);
        Array.Copy(headbuf, 0, data_to_send, 0, head_buffer_size);
        Array.Copy(bfs, 0, data_to_send, headbuf.Length, bfs.Length);
        _tcpServer.SendData(End_Point, data_to_send);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StopServer();
        _tcpServer = null;
    }


}
