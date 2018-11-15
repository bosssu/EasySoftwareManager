using UnityEngine;
using System.Collections;
using System;
using WCBG.ToolsForUnity.Tools;

public class Framework : MonoSingleton<Framework> {

    private bool isPause;

    private float curTimeScale = 1f;
    private float ratio = 0.5626f;

    public float ScreenRatio
    {
        get
        {
            return ratio;
        }
    }

    private bool Pause
    {
        set
        {
            this.isPause = value;
            if (this.isPause)
            {
                //Debug.LogError("game pause");
                EventBus.Instance.BroadCastEvent(EventID.GAME_PAUSE);
            }
            else
            {
                //Debug.LogError("game resume");
                EventBus.Instance.BroadCastEvent(EventID.GAME_RESUME);
            }
        }
    }

	protected override void Init ()
	{
		base.Init ();
        
        //SceneDebugger.LogEnable = false;
        Application.targetFrameRate = 60;
        ratio = (float)Screen.height / (float)Screen.width;
    }

	void Start() {
        //WechatInfo info = JsonUtility.FromJson<WechatInfo>("{\"name\": \"extraData.name\",\"uid\": \"extraData.uid\"}");
        //Debug.Log(JsonUtility.ToJson(info));
        //Debug.LogError (FileTools.GetFullDirectory(Application.dataPath));
        InitBaseSys ();
		
		GameStateManager.Instance.Initialize ();
		GameStateManager.Instance.GotoState (GameStateName.MAIN_STATE);
	}

	private void InitBaseSys() {

        //gameObject.AddComponentEx<AppConfig>();
		TimerManager.CreateInstance ();
        InputTimerManager.GetInstance();
		DownloadManager.CreateInstance ();
        WindowRegister.CreateInstance();
		//HUIManager.CreateInstance ();
		//ActorObjManager.CreateInstance ();
		GameObjectPool.CreateInstance ();
        TableProtoManager.Instance.PreLoadTableData();
		//UISpriteManager.CreateInstance ();

		//SDKManager.CreateInstance ();
		//UserManager.CreateInstance ();

        TimeUtils.CurUnixTime = TimeUtils.ToUtcSeconds(DateTime.Now);

        //RestManager.CreateInstance();
	}



	void Update() {
        ResourceManager.Instance.CustomUpdate();
		TimerManager.Instance.Update ();
		DownloadManager.Instance.Update ();
		//HUIManager.Instance.Update ();
		//ActorObjManager.Instance.Update ();
		GameObjectPool.Instance.Update ();

        //RestManager.Instance.Update();

        //if (Input.GetKeyUp(KeyCode.Escape))
        //{
        //    if (!SDKManager.Instance.ExitGame())
        //    {
        //        InputEventManager.Instance.InputFocused = false;
        //        TopNoticeManager.Instance.ShowOkCancelTip(CommonString.ExitStr, () =>
        //        {
        //            Application.Quit();
        //        },
        //        () => { InputEventManager.Instance.InputFocused = true; },
        //        () => { InputEventManager.Instance.InputFocused = true; }
        //        );
        //    }
        //}
    }

	void LateUpdate() {
		//HUIManager.Instance.LateUpdate ();
		//ActorObjManager.Instance.LateUpdate ();
	}

	public void QuitGame() {
		EventBus.Instance.BroadCastEvent (EventID.GAME_QUIT);
		Application.Quit ();
	}

    private void OnApplicationPause(bool pause)
    {
        if (!pause && isPause)
        {
            Pause = false;
        }
        else if (pause && !isPause)
        {
            Pause = true;
        }
    }

    private void OnApplicationFocus(bool focus)
	{
		if (focus && isPause)
		{
            Pause = false;
		}
		else if (!focus && !isPause)
        {
            Pause = true;
        }
	}
}
