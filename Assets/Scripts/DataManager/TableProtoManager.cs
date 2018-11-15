using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using TableProto;
using System.IO;

public class TableProtoManager : Singleton<TableProtoManager> {

	public override void Init ()
	{
		base.Init ();
	}

	public void PreLoadTableData() {
		TableProtoLoader.Load ();
		//Debug.LogError (TableProtoLoader.GoodsInfoDict.Count);
	}
}
