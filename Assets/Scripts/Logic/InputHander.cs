using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class InputHander : Singleton<InputHander> {


    public int double_click_dur = 250;
    bool is_double_click = false;
    IDisposable tapDispose;
    IDisposable doubleTapDispose;
    IDisposable dragDispose;

    public override void Init()
    {
        base.Init();

        ClickCheck();

    }


    private void DragCheck()
    {
        var drag_stream = Observable.EveryUpdate().Where(_x => Input.GetMouseButton(0) && Input.touchCount > 1);
        drag_stream.Subscribe((_x) => {

        });
    }

    private void ClickCheck()
    {
        // 每一帧运行，点击鼠标左键的时候触发
        var clickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
        var obser = clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(double_click_dur)));
        tapDispose = obser.Where(xs => xs.Count >= 2)
            .Subscribe((_xs) => {
                EventBus.Instance.BroadCastEvent(EventIDEx.DOUBLE_TAP);
            });
        doubleTapDispose = obser.Where(xs => xs.Count == 1)
            .Subscribe((_x)=> {
                EventBus.Instance.BroadCastEvent(EventIDEx.SIMPLE_TAP);
            });
    }

    public override void UnInit()
    {
        base.UnInit();
        tapDispose.Dispose();
        doubleTapDispose.Dispose();
    }

}
