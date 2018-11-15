using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

public class CreateTableProto {

	[MenuItem("工具/创建表格数据")]
	static void CreateTable() {
		ExcuteCmd ("/Tools/ExcelToProto/Table/Run.bat");
	}

	[MenuItem("工具/创建网络数据")]
	static void CreateNetwork() {
		ExcuteCmd ("/Tools/ProtocolNetwork/Bin/CMDProtobufBuild.bat");
	}

	static void ExcuteCmd(string path) {
		Process process = new Process();
		DirectoryInfo info = new DirectoryInfo(Application.dataPath);
		process.StartInfo.FileName = info.Parent + path;
		process.StartInfo.UseShellExecute = true;

		//这里相当于传参数 
		//process.StartInfo.Arguments = name;
		process.Start();
		process.WaitForExit();
	}
}
