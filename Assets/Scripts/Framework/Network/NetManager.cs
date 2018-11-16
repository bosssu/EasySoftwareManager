using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NetManager : MonoSingleton<NetManager> {

    public enum NetType { TCP, UDP }

    public TCPController _tcpController;
    public UDPController _udpController;

    public NetType _netType;

    protected override void Init()
    {
        base.Init();

        _tcpController = TCPController.Instance;
        _udpController = UDPController.Instance;

    }

    public void ProcessData(byte[] data)
    {
        Debug.LogError(data.Length);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        TCPController.DestroyInstance();
        UDPController.DestroyInstance();

    }

}
