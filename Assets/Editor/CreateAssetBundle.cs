using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundle {

	public const int AppV = 1;
	public const int CodeV = 1;
	public const int ResV = 1;

	public static string AssetPath = "AssetBundle";

	private static string RootPath = "Assets/";

	public static string ResInfoName = "ResInfo.txt";

	public static string UpdateFileName = "Update.txt";

	private static List<string> BundleNames = new List<string> ();

	[MenuItem("发布/创建AssetBundle")]
	public static void CreateAsset() {
		EditorUtility.DisplayProgressBar ("创建AssetBundle", "请等待...", 0.5f);
        PublishEditor.MoveResToAsset();
		BundleNames.Clear ();
		ResourceInfo mResourceInfo = new ResourceInfo ();
		mResourceInfo.AppVersion = AppV;
		mResourceInfo.CodeVersion = CodeV;
		mResourceInfo.ResVersion = ResV;
		mResourceInfo.ResList = new List<ResourceItemInfo> ();
		mResourceInfo.ModuleList = ModuleAssetBundle.ModuleList;
		SetAssetName (mResourceInfo, AssetPath, BundleNames);

		//FileUtil.DeleteFileOrDirectory(AssetOutPath);
		if (!Directory.Exists (AssetOutPath)) {
			Directory.CreateDirectory (AssetOutPath);
		}

		BundleNames.Add (ResInfoName);
		BundleNames.Add (UpdateFileName);


        BuildPipeline.BuildAssetBundles (AssetOutPath, BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);

		CreateUpdateFile (AssetOutPath, mResourceInfo);
	
		SetDependencies (mResourceInfo, AssetOutPath); 

		SaveResInfoFile (Path.Combine (AssetOutPath, ResInfoName), mResourceInfo);

        ZipToStreaming();

		ClearAssetName (AssetPath, mResourceInfo);
        PublishEditor.MoveAssetToRes();
        AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar ();
		//EditorUtility.DisplayDialog("提示", "打包AssetBundle完成！", "确定");

    }

    [MenuItem("发布/压缩上传AssetBundle到服务器")]
	static void UploadToServer() {
		string serverPath = "D:\\wamp\\www\\SuperWings\\main\\" + AppV + "\\" + CodeV;
		string ResPath = serverPath + "\\" + ResV;
		if (FileTools.IsDirectoryExist (ResPath)) {
			EditorUtility.DisplayDialog ("提示", "资源版本" + AppV + "." + CodeV + "." + ResV + "已经存在！", "确定");
			return;
		}
		FileTools.CreateDirectory (ResPath);

		string info = File.ReadAllText (Path.Combine (AssetOutPath, ResInfoName));
		ResourceInfo itemInfo = JsonUtility.FromJson<ResourceInfo> (info);
		BundleNames.Clear ();
		for (int i = 0; i < itemInfo.ResList.Count; i++) {
			if (itemInfo.ResList [i].IsAsset) {
				BundleNames.Add (itemInfo.ResList [i].Name);
			}
			//AddBundleName (itemInfo.ResList [i]);
		}
        ZipFileToServer(ResPath);
        CreateUpdateFile (ResPath, itemInfo);
		FileTools.CopyFile (FileTools.CombinePath (AssetOutPath, ResInfoName), FileTools.CombinePath (serverPath, ResInfoName));
        
        EditorUtility.DisplayDialog("提示", "上传完成！", "确定");
    }

	public static void SetAssetName(ResourceInfo resInfo, string rootName, List<string> assetNames) {

		string[] Dirs = Directory.GetDirectories (RootPath + rootName);
		for (int i = 0; i < Dirs.Length; i++) {
			string path = Dirs[i].Replace("\\", "/");
			string name = FileTools.GetFullName (path);
			//Debug.LogWarning (path);
			//Debug.LogWarning (name);
			ResourceItemInfo item = new ResourceItemInfo ();
			item.Path = name;
			item.Name = name;
			if (name == "resident") {
				item.IsResident = true;
			}
			GetResItemInfo (resInfo, item.Path, item.IsResident, assetNames);
			//item.Dependencies = GetResItemInfo (item.Path, item.IsResident);
			item.ResNames = new List<string> ();
			resInfo.ResList.Add (item);
			assetNames.Add (item.Name);
			item.IsAsset = AssetHeplerEditor.SetAssetBundleName (path, item.Name, item.ResNames);
		}
	}

	public static void ClearAssetName(string path, ResourceInfo info) {
		//清除AssetBundle的名字
		for (int i = 0; i < info.ResList.Count; i++) {
			AssetHeplerEditor.SetAssetBundleName (FileTools.CombinePath(RootPath + path, info.ResList[i].Path), "");
			//SetAssetNameToNull (mResourceInfo.ResList[i]);
		}
	}

//	static void SetAssetNameToNull(ResourceItemInfo itemInfo) {
//		for (int i = 0; i < itemInfo.Dependencies.Count; i++) {
//			AssetHeplerEditor.SetAssetBundleName (FileTools.CombinePath(RootPath + AssetPath, itemInfo.Dependencies[i].Path), "");
//			SetAssetNameToNull (itemInfo.Dependencies[i]);
//		}
//	}

//	static void AddBundleName(ResourceItemInfo itemInfo) {
//		for (int i = 0; i < itemInfo.Dependencies.Count; i++) {
//			if (itemInfo.Dependencies [i].IsAsset) {
//				BundleNames.Add (itemInfo.Dependencies [i].Name);
//			}
//			AddBundleName (itemInfo.Dependencies [i]);
//		}
//	}

	public static void GetResItemInfo(ResourceInfo resInfo, string itemPath, bool isResident, List<string> assetNames) {
		//List<ResourceItemInfo> infos = new List<ResourceItemInfo> ();
		string[] Dirs = Directory.GetDirectories (RootPath + AssetPath + "/" + itemPath);
		for (int i = 0; i < Dirs.Length; i++) {
			string path = Dirs[i].Replace("\\", "/");
			string name = FileTools.GetFullName (path);
			ResourceItemInfo item = new ResourceItemInfo ();
			item.Path = itemPath + "/" + name;
			item.Name = name;
			item.IsResident = isResident;
			GetResItemInfo (resInfo, item.Path, isResident, assetNames);
			//item.Dependencies = GetResItemInfo (item.Path, isResident);
			item.ResNames = new List<string> ();
			//infos.Add (item);
			resInfo.ResList.Add (item);
			assetNames.Add (item.Name);
			item.IsAsset = AssetHeplerEditor.SetAssetBundleName (path, item.Name, item.ResNames);
		}
		//return infos;
	}

	static string AssetOutPath {
		get {
			return Path.Combine(AssetPath,  AssetHeplerEditor.GetPlatformName());
		}
	}

	static void ZipToStreaming() {
		string zipOutPath = Path.Combine (Application.streamingAssetsPath, AssetPath);
		FileTools.CreateDirectory (zipOutPath);
		zipOutPath = Path.Combine (zipOutPath, "Asset.zip");
		ZipUtils.CreateZipFile (zipOutPath, AssetOutPath, BundleNames);
	}

	static void CreateUpdateFile(string path, ResourceInfo resInfo) {
		string updatePath = FileTools.CombinePath (path, UpdateFileName);
		resInfo.ResLength = 0;
		using (StreamWriter streamWriter = new StreamWriter(updatePath, false)) {
			string[] files = Directory.GetFiles(path);
			for (int i = 0; i < files.Length; i++) {
				string fileName = Path.GetFileName(files[i]);
				if (fileName.IndexOf(AssetHeplerEditor.GetPlatformName()) >= 0 || fileName.IndexOf(".meta") >= 0 || fileName.IndexOf(".manifest") >= 0 || !BundleNames.Contains(fileName)) {
					//文件夹名称的AssetBundle排除
					continue;
				}
				if (fileName == ResInfoName || fileName == UpdateFileName) {
					continue;
				}
				string md5 = FileTools.GetMD5(Path.Combine (AssetOutPath, fileName));
				string filePath = Path.Combine (path, fileName);
				long size = FileTools.GetFileSize(filePath);
				resInfo.ResLength += size;
				string value = fileName + ";" + size + ";" + md5;
				streamWriter.WriteLine(value);
				streamWriter.Flush();
			}
		}
	}

	public static void SetDependencies(ResourceInfo itemInfo, string outPath) {
		//string info = File.ReadAllText (Path.Combine (AssetOutPath, ResInfoName));
		//ResourceInfo itemInfo = JsonUtility.FromJson<ResourceInfo> (info);

		AssetBundle ab = AssetBundle.LoadFromFile (FileTools.CombinePath(outPath, AssetHeplerEditor.GetPlatformName()));
		AssetBundleManifest m = ab.LoadAsset<AssetBundleManifest> ("AssetBundleManifest");
		//Debug.Log (m.GetAllDependencies("10001")[0]);
		for (int i = 0; i < itemInfo.ResList.Count; i++) {
			if (itemInfo.ResList [i].IsAsset) {
				itemInfo.ResList [i].Dependencies = m.GetAllDependencies (itemInfo.ResList [i].Name);
			}
		}
		ab.Unload (true);
//		string infoPath = Path.Combine (AssetOutPath, ResInfoName);
//		using (StreamWriter streamWriter = new StreamWriter(infoPath, false)) {
//			streamWriter.WriteLine(JsonUtility.ToJson(itemInfo));
//			streamWriter.Flush();
//		}
//		Debug.LogWarning (JsonUtility.ToJson(itemInfo));
	}

	public static void SaveResInfoFile(string path, ResourceInfo info) {
		string infoJson = JsonUtility.ToJson (info);
		Debug.Log (infoJson);
		//string infoPath = Path.Combine (path, ResInfoName);
		using (StreamWriter streamWriter = new StreamWriter(path, false)) {
			streamWriter.WriteLine(infoJson);
			streamWriter.Flush();
		}
	}

	static void ZipFileToServer(string serverPath) {
		string[] files = Directory.GetFiles(AssetOutPath);
		for (int i = 0; i < files.Length; i++) {
			string fileName = Path.GetFileName (files [i]);
			if (fileName.IndexOf (AssetHeplerEditor.GetPlatformName ()) >= 0 || fileName.IndexOf (".meta") >= 0 || fileName.IndexOf (".manifest") >= 0 || !BundleNames.Contains (fileName)) {
				//文件夹名称的AssetBundle排除
				continue;
			}
			if (fileName == ResInfoName) {
				continue;
			}
			string dp = FileTools.CombinePath(serverPath, fileName);
			ZipUtils.CompressFile(files[i], dp);
		}
	}

//	[MenuItem("Asset/打开开发模式")]
//	static void OpenDebugMode() {
//		//PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, "LOCAL_DEBUG");
//		AssetHeplerEditor.AddSymbolsForGroup (BuildTargetGroup.Android, "LOCAL_DEBUG");
//	}
//
//	[MenuItem("Asset/关闭开发模式")]
//	static void CloseDebugMode() {
//		//PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, "");
//		AssetHeplerEditor.RemoveSymbolsForGroup (BuildTargetGroup.Android, "LOCAL_DEBUG");
//	}

}
