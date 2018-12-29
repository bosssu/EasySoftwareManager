using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowRegister : Singleton<WindowRegister>{

    public override void Init()
    {
        base.Init();

        WindowManager mgr = WindowManager.Instance;

        mgr.RegisterWindow(WinNames.Test_Panel, new TestPanel());
        mgr.RegisterWindow(WinNames.Test_Server_Panel, new TestServerPanel());
        mgr.RegisterWindow(WinNames.Pop_test_Panel, new Poptest());

    }
}
