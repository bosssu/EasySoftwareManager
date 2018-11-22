using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TableProto;

[GameState]
public class MainState : BaseState {

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        SceneLoader.Instance.LoadScene(SceneLoader.MainState,()=> {

            //加载物体
            //GameObject g = ResourceManager.Instance.GetResource("test/test_prefab/Cube", typeof(GameObject), enResourceType.Prefab).m_content as GameObject;
            //GameObject x = GameObject.Instantiate<GameObject>(g);

            //GameObject g = GameObjectPool.Instance.GetGameObject("test/test_prefab/Cube", enResourceType.Prefab);
            //GameObjectPool.Instance.PrepareGameObject("test/test_prefab/Cube", enResourceType.Prefab, 10);

            //加载ui
            WindowManager.Instance.OpenWindow(WinNames.Test_Panel);
            //WindowManager.Instance.OpenWindow(WinNames.Test_Server_Panel);

            //protobuf
            //Debug.Log(TableProtoLoader.MainItemInfoDict[1001].index);

            Debug.LogError(Application.persistentDataPath);

        });

    }

    public override void OnStateLeave()
    {
        base.OnStateLeave();
    }

}
