using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : SingletonBaseWindow<TestPanel> {

    public Image _Image { get; set; }

    protected override void OnOpen(params object[] paramArray)
    {
        base.OnOpen(paramArray);

        _Image.color = Color.yellow;
    }

}
