using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using UniRx.Triggers;

public class Poptest : SingletonBaseWindow<Poptest> {

    public InputField _InputField { get; set; }
    public Text _SText { get; set; }
    public Button _Button { get; set; }
    public Image _Progress { get; set; }
    public Image _CImage { get; set; }

    protected override void OnOpen(params object[] paramArray)
    {
        base.OnOpen(paramArray);
        Action act = paramArray[0] as Action;
        _Button.onClick.AddListener(()=> {
            WindowManager.Instance.CloseWindow(WinNames.Pop_test_Panel);
            if (act != null) act();
        });

        int[] ss = new int[] { 1,2,3,4,6,5,42,23};
        //ss.ToObservable()
        //    .Where(_x=>_x > 2)
        //    .Subscribe(_x => Debug.Log(_x));

        //var stream = ss.ToObservable()
        //    .Where(_x => _x > 2);
        //var stream1 = ss.ToObservable()
        //    .Where(_x => _x < 2);
        //Observable.Merge(stream, stream1)
        //    .Subscribe(_x => Debug.Log(_x));

        //_InputField.OnValueChangedAsObservable()
        //    .SubscribeToText(_SText);

        _InputField.OnValueChangedAsObservable()
            .Delay(TimeSpan.FromSeconds(1f))
            .Subscribe((_x) =>
            {
                _SText.text = _x;
                Debug.Log(_x);
            });

        //Observable.Merge(
        //        ss.ToObservable().Where(_x => _x > 5),
        //        ss.ToObservable().Where(_x => _x < 2)
        //        )
        //        .Subscribe(_x => Debug.Log(_x));

        //_CImage.OnPointerClickAsObservable()
        //    .Subscribe(x => Debug.Log(x.pointerId));



        //transform.LocalScale(Vector3.one * 2)
        //    .LocalPosition(Vector3.zero)
        //    .Name("zhangsan")
        //    .Parent(WindowManager.Instance.canvas.transform);

        //Observable.EveryUpdate()
        //    //.Subscribe(x => Debug.Log(Time.timeSinceLevelLoad));
        //    .Where(x => Input.GetMouseButton(0))
        //    .Subscribe(x => Debug.Log("mouse button down"));

        //var stream = Observable.EveryUpdate().Where(x => Input.GetMouseButtonDown(0));

        //stream.Buffer(stream.Throttle(TimeSpan.FromMilliseconds(250)))
        //    .Where(x => x.Count == 2)
        //    .Subscribe(xx => Debug.Log("double click"));

        //Observable.Start(() => { })
        //    .ObserveOnMainThread()
        //    .Subscribe(testc => _CImage.raycastTarget = false);


    }


    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.D))
        { // with Progress
            {
                // notifier for progress
                var progressNotifier = new ScheduledNotifier<float>();
                progressNotifier.Subscribe(x => _Progress.fillAmount = x); // write www.progress

                // pass notifier to WWW.Get/Post
                ObservableWWW.Get("http://google.com/", progress: progressNotifier).Subscribe();
            }
        }
    }
}
