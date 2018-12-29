using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class WayCreator : MonoBehaviour {

    //[HideInInspector]
    public PetPainterPathAttr _currentEditSinglePath;
    public PetPainterTableData painterTableData = new PetPainterTableData();
    public float point_z_value = -0.1f;

    [Tooltip("是否自动切换到新添加的路径进行编辑")]
    public bool auto_change_to_newadd_path = true;

    [Space(3)]
    List<Vector3> pathPoints;

    GameObject _play_obj;
    GameObject PlayObj
    {
        get {
            if (_play_obj == null)
            {
                _play_obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                _play_obj.transform.localScale = Vector3.one * .1f;
                _play_obj.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            return _play_obj;
        }
    }

    private void OnEnable()
    {

    }

    public void GetCurrentSinglePathWorldPoints(ref List<Vector3> worldPoints)
    {
        if (_currentEditSinglePath == null) return;

        pathPoints = _currentEditSinglePath.pathPoints;
        if (pathPoints == null || pathPoints.Count == 0) return;

        worldPoints.Clear();
        foreach (var point in pathPoints)
        {
            worldPoints.Add(transform.TransformPoint(point));
        }
    }

    public void AddNewPath(int id,bool is_path_close)
    {
        PetPainterPathAttr pet_single_path = new PetPainterPathAttr();
        pet_single_path.path_id = id;
        pet_single_path.path_name = string.Format("路径{0}", id);
        pet_single_path.is_close = is_path_close;

        if (auto_change_to_newadd_path)
            _currentEditSinglePath = pet_single_path;

        AddCurrentWayPoint(new Vector3(-200f, 0, 0));
        AddCurrentWayPoint(new Vector3(200f, 0, 0));

        painterTableData.paths.Add(pet_single_path);
    }

    public void RemovePath(int id)
    {
        if (painterTableData.paths.Count > 0)
        {
            foreach (var singlepath in painterTableData.paths)
            {
                if (singlepath.path_id == id)
                {
                    if (singlepath.path_id == _currentEditSinglePath.path_id)
                    {
                        painterTableData.paths.Remove(singlepath);
                        _currentEditSinglePath = GetSinglePathByIndex(0);
                    }
                    else
                    {
                        painterTableData.paths.Remove(singlepath);
                    }
                    return;
                }

            }
        }
    }

    public void AddCurrentWayPoint(Vector3 point)
    {
        if (_currentEditSinglePath != null)
        {
            _currentEditSinglePath.pathPoints.Add(point);
        }
    }

    public void RemoveCurrentWayPoint()
    {
        pathPoints = _currentEditSinglePath.pathPoints;
        if (_currentEditSinglePath != null && pathPoints.Count > 0)
            pathPoints.RemoveAt(pathPoints.Count - 1);
    }

    public void ChangeToEditPath(int id)
    {
        PetPainterPathAttr attr = GetSinglePath(id);
        if (attr != null)
        {
            _currentEditSinglePath = attr;
        }
    }

    private bool IsContainSinglePath(int id)
    {
        if (painterTableData.paths.Count > 0)
        {
            foreach (var singlepath in painterTableData.paths)
            {
                if (singlepath.path_id == id)
                    return true;
            }
        }

        return false;
    }

    public PetPainterPathAttr GetSinglePath(int id)
    {
        if (painterTableData.paths.Count > 0)
        {
            foreach (var singlepath in painterTableData.paths)
            {
                if (singlepath.path_id == id)
                    return singlepath;
            }
        }

        return null;
    }

    public PetPainterPathAttr GetSinglePathByIndex(int index)
    {
        if (painterTableData.paths.Count > 0)
        {
            return painterTableData.paths[index];
        }

        return null;
    }

    //播放当前编辑的路径
    public void PlayCurrentPath()
    {
       
    }

    //修正宽高比
    public void AutoSizeFixed()
    {
        RawImage _rt = GetComponentInChildren<RawImage>();
        _rt.SetNativeSize();
    }

    public void ClearAllPathData()
    {
        _currentEditSinglePath = null;
        painterTableData.paths.Clear();
    }

    public void SetData(PetPainterScriptObjectData data)
    {
        painterTableData.Id = data.Id;
        painterTableData.ImageName = data.ImageName;
        painterTableData.paths = data.paths;
        _currentEditSinglePath = painterTableData.paths[0];
    }

    List<Vector3> points_to_draw;
    private void OnDrawGizmos()
    {
        points_to_draw = new List<Vector3>();
        GetCurrentSinglePathWorldPoints(ref points_to_draw);
        if (pathPoints.Count > 0)
        {
            PathHelper.DrawPathHelper(points_to_draw.ToArray(), Color.green, "gizmos", _currentEditSinglePath.is_close);
        }
    }
}
