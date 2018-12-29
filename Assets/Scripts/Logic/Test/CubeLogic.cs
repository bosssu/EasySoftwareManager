using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class CubeLogic : MonoBehaviour {

    public enum EnumType
    {
        Click,DoubleClick,LongDown
    }

	// Use this for initialization
	void Start () {

        ClickCheck(EnumType.Click, () => { Debug.Log("单机"); });
        ClickCheck(EnumType.DoubleClick, () => { Debug.Log("双击"); });

        Plane p = new Plane(Vector3.forward, transform.position);


        var mouse_stream = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButton(0))
            
            .Subscribe((x) => {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                
                    float dis;
                    if (p.Raycast(ray, out dis))
                    {
                        Vector3 point = ray.GetPoint(dis);
                        transform.position = point;
                    }
                }
            });

    }

    public void ClickCheck(EnumType type,Action callback)
    {
        var stream = Observable.EveryUpdate().Where(_x => Input.GetMouseButtonDown(0));

        if (type == EnumType.Click)
        {
            stream.Subscribe((x) =>
            {
                if (callback != null) callback();
            });

        }
        else if (type == EnumType.DoubleClick)
        {
            stream.Buffer(stream.Throttle(TimeSpan.FromMilliseconds(250)))
            .Where(_x => _x.Count == 2)
            .Subscribe((x) =>
            {
                if (callback != null) callback();
            });

        }
        else if (type == EnumType.LongDown)
        {

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
