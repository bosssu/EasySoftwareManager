using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TestServerPanel : SingletonBaseWindow<TestServerPanel> {

    public Text _TText { get; set; }
    public InputField _InputField { get; set; }
    public Button _BtnSend { get; set; }

    protected override void OnOpen(params object[] paramArray)
    {
        base.OnOpen(paramArray);
        _InputField.text = "192.168.1.179:8000";
        TCPServerController.Instance.OpenServer(8000);


    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        _TText.text = "";
        if (TCPServerController.Instance.Dict_ConnectedClients.Count > 0)
        {
            foreach (var item in TCPServerController.Instance.Dict_ConnectedClients.Keys)
            {
                _TText.text += item;
            }
        }
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        _BtnSend.onClick.AddListener(OnBtnSendClick);
    }

    protected override void RemoveListensers()
    {
        base.RemoveListensers();
        _BtnSend.onClick.RemoveListener(OnBtnSendClick);
    }

    private void OnBtnSendClick()
    {
        Debug.Log("尝试发送");
        byte[] bfs = Encoding.Default.GetBytes("服务器发送测试");
        TCPServerController.Instance.SendData(_TText.text, bfs);

    }

}
