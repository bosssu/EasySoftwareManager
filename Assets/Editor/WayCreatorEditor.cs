using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WayCreator))]
public class WayCreatorEditor : Editor {

    string savepath = "Assets/Resources/test";
    WayCreator creator;
    List<Vector3> current_pathpoints;
    string way_id_to_change;
    bool is_path_add_closed;
    Object hudBar;
    GUIStyle style;

    void OnEnable()
    {
        creator = (WayCreator)target;
        style = new GUIStyle();
        style.fontSize = 30;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginVertical();

        if (GUILayout.Button("修正宽高比"))
        {
            creator.AutoSizeFixed();
        }

        if (GUILayout.Button("当前路径后面添加一个点"))
        {
            creator.AddCurrentWayPoint(current_pathpoints[current_pathpoints.Count - 1]);
        }

        if (GUILayout.Button("当前路径后面删除一个点"))
        {
            creator.RemoveCurrentWayPoint();
        }

        GUILayout.BeginHorizontal();
        way_id_to_change = GUILayout.TextField(way_id_to_change);
        is_path_add_closed = GUILayout.Toggle(is_path_add_closed, "isPathClose");
        if (GUILayout.Button("跳转到路径编辑"))
        {
            int way_id;
            if (int.TryParse(way_id_to_change, out way_id))
            {
                creator.ChangeToEditPath(way_id);
            }
        }

        if (GUILayout.Button("添加一条新路径"))
        {
            //id默认递增
            creator.AddNewPath(creator.painterTableData.paths.Count, is_path_add_closed);
        }

        if (GUILayout.Button("删除指定路径"))
        {
            int way_id;
            if (int.TryParse(way_id_to_change, out way_id))
            {
                creator.RemovePath(way_id);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(1);
        if (GUILayout.Button("播放当前路径"))
        {
            creator.PlayCurrentPath();
        }

        GUILayout.BeginHorizontal();

        hudBar = (Object)EditorGUILayout.ObjectField(hudBar, typeof(Object), false);
        if (GUILayout.Button("加载路径资源"))
        {
            PetPainterScriptObjectData data = hudBar as PetPainterScriptObjectData;
            creator.SetData(data);
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("清除所有路径"))
        {
            creator.ClearAllPathData();
        }

        savepath = GUILayout.TextField(savepath);

        if (GUILayout.Button("保存"))
        {
            PetPainterScriptObjectData data = ScriptableObject.CreateInstance<PetPainterScriptObjectData>();
            data.Id = creator.painterTableData.Id;
            data.ImageName = creator.painterTableData.ImageName;
            data.paths = creator.painterTableData.paths;

            AssetDatabase.CreateAsset(data, GetFullAssetPath(data.Id));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("提示", "保存成功！", "确定");
        }


        GUILayout.EndVertical();

    }

    public void OnSceneGUI()
    {
        //编辑器面板编辑路径
        Event e = Event.current;

        if (e.type == EventType.mouseDown && e.button == 0 && e.alt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                e.Use();
                Vector3 local_point = creator.transform.InverseTransformPoint(hit.point);
                creator.AddCurrentWayPoint(local_point);
            }
        }

        Handles.BeginGUI();
        Handles.Label(creator.transform.position,string.Format("当前路径：{0}", creator._currentEditSinglePath.path_id),style);
        Handles.EndGUI();


        //获取point列表
        PetPainterPathAttr _currentEditSinglePath = creator._currentEditSinglePath;
        if(_currentEditSinglePath != null)
        {
            current_pathpoints = _currentEditSinglePath.pathPoints;
            if (creator == null || current_pathpoints.Count <= 0) return;

            //绘制hander
            for (int i = 0; i < current_pathpoints.Count; i++)
            {
                Vector3 worldPoint = creator.transform.TransformPoint(current_pathpoints[i]);
                worldPoint = Handles.PositionHandle(worldPoint, Quaternion.identity);              //为每一个节点画positionHandle
                Vector3 localPoint = creator.transform.InverseTransformPoint(worldPoint);
                localPoint.z = creator.point_z_value;
                current_pathpoints[i] = localPoint;

                Handles.BeginGUI();
                Handles.Label(worldPoint, i.ToString(),style);
                Handles.EndGUI();
            }

        }
    }

    private string GetFullAssetPath(uint id)
    {
        return string.Format("{0}/{1}.asset", this.savepath, id);
    }


}
