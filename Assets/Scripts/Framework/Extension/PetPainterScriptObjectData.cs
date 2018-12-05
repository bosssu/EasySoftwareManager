using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PetPainterScriptObjectData : ScriptableObject
{
    public uint Id;

    public string ImageName;

    public List<PetPainterPathAttr> paths = new List<PetPainterPathAttr>();
}

[Serializable]
public class PetPainterPathAttr {
    public string path_name;
    public int path_id;
    public float playspeed = 300;
    public bool is_close;
    public List<Vector3> pathPoints = new List<Vector3>();
}

[Serializable]
public class PetPainterTableData
{

    public uint Id;

    public string ImageName;

    public List<PetPainterPathAttr> paths = new List<PetPainterPathAttr>();
}

