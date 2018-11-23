using UnityEngine;
using UnityEngine.UI;

public class TestPanel : SingletonBaseWindow<TestPanel> {

    public Text _Text_Show { get; set; }
    public Button _Btn_Send { get; set; }
    public Button _Btn_Connect { get; set; }
    public Button _Btn_Disconnect { get; set; }
    public InputField _InputField { get; set; }
    public InputField _IpInput { get; set; }
    public InputField _PortInput { get; set; }

    protected override void OnOpen(params object[] paramArray)
    {
        base.OnOpen(paramArray);
        _IpInput.text = "192.168.1.179";
        _PortInput.text = "8000";
        NetManager.Instance._netType = NetManager.NetType.TCP;
        UDPReceive.Instance.MessageRecevdEvent = (str) => { Debug.Log(str.Length); };

    }

    protected override void AddListeners()
    {
        base.AddListeners();
        _Btn_Send.onClick.AddListener(OnBtnSendClick);
        _Btn_Connect.onClick.AddListener(OnBtnConnect);
        _Btn_Disconnect.onClick.AddListener(OnBtnDisconnectClick);
        EventBus.Instance.AddEventHandler<byte[]>(EventID.NETWORK_MESSAGE_RECEIVED, RecvTest);
            
    }

    private void RecvTest(byte[] obj)
    {
        Debug.Log(obj.Length);
    }

    protected override void RemoveListensers()
    {
        base.RemoveListensers();
        _Btn_Send.onClick.RemoveListener(OnBtnSendClick);
        _Btn_Connect.onClick.RemoveListener(OnBtnConnect);
        _Btn_Disconnect.onClick.RemoveListener(OnBtnDisconnectClick);
        EventBus.Instance.RemoveEventHandler<byte[]>(EventID.NETWORK_MESSAGE_RECEIVED, RecvTest);
    }

    private void OnBtnSendClick()
    {
        Debug.Log("尝试发送");
        byte[] bfs = System.Text.Encoding.Default.GetBytes("fdfdfdfdfd");
        NetManager.Instance.UDPSend(bfs, int.Parse(_PortInput.text), _IpInput.text);

    }

    private void OnBtnConnect()
    {
        Debug.Log("尝试连接");
        string ip = _IpInput.text;
        ushort port = ushort.Parse(_PortInput.text);
        NetManager.Instance.NetworkReady(port,ip);
    }

    private void OnBtnDisconnectClick()
    {
        Debug.Log("尝试断开连接");
        //NetManager.Instance._tcpController.Disconnect();
    }

}
