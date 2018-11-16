using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public enum ModuleType
{
    测试,
}

public class ModuleAssetItem
{
    public string Name;
    public int CodeVersion;
    public int ResVersion;
}

public class ModuleAssetBundle : EditorWindow {

	private const string ModulePath = "AssetBundleModule";

	private static List<string> BundleNames = new List<string> ();

    private ModuleType moduleType;

    private ModuleType uploadModuleType;

    private Dictionary<ModuleType, ModuleAssetItem> items = new Dictionary<ModuleType, ModuleAssetItem>();

    public static List<ModuleItemInfo> ModuleList = new List<ModuleItemInfo> () {

        new ModuleItemInfo() {Name = ModuleConst.testName, CodeVersion = ModuleConst.testCodeVersion},

    };

	static string GetOutPath(string path) {
		return FileTools.CombinePath (path, AssetHeplerEditor.GetPlatformName ());
	}

    private void Awake()
    {
        items.Add(ModuleType.测试, new ModuleAssetItem()
        {
            Name = ModuleConst.testName,
            CodeVersion = ModuleConst.testCodeVersion,
            ResVersion = ModuleConst.testResVersion
        });

    }

    [MenuItem("发布/资源打包上传")]
    public static void ModulePack()
    {
        Rect wr = new Rect(0, 0, 300, 300);
        ModuleAssetBundle window = (ModuleAssetBundle)EditorWindow.GetWindowWithRect(typeof(ModuleAssetBundle), wr, true, "打包上传资源");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        moduleType = (ModuleType)EditorGUILayout.EnumPopup(moduleType);
        if (GUILayout.Button("开始打包"))
        {
            CreateAsset(ModulePath, items[moduleType].Name, CreateAssetBundle.AppV, items[moduleType].CodeVersion, items[moduleType].ResVersion);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
        uploadModuleType = (ModuleType)EditorGUILayout.EnumPopup(uploadModuleType);
        if (GUILayout.Button("打包上传"))
        {
            CreateAsset(ModulePath, items[uploadModuleType].Name, CreateAssetBundle.AppV, items[uploadModuleType].CodeVersion, items[uploadModuleType].ResVersion);
            UploadAsset(ModulePath, items[uploadModuleType].Name, items[uploadModuleType].CodeVersion, items[uploadModuleType].ResVersion);
        }
        GUILayout.EndHorizontal();
    }

    static void CreateAsset(string rootName, string name, int appV, int codeV, int resV) {
        PublishEditor.MoveResToModuleAsset(name);
        BundleNames.Clear();
		ResourceInfo resInfo = new ResourceInfo ();
		resInfo.AppVersion = appV;
		resInfo.CodeVersion = codeV;
		resInfo.ResVersion = resV;
		resInfo.ResList = new List<ResourceItemInfo> ();
		ResourceItemInfo item = new ResourceItemInfo ();
		item.Path = name;
		item.Name = name;
		item.IsResident = false;
        GetResItemInfo(resInfo, item.Path, false, BundleNames);
		item.ResNames = new List<string> ();
		resInfo.ResList.Add (item);
		item.IsAsset = AssetHeplerEditor.SetAssetBundleName ("Assets/" + rootName + "/" + name, item.Name, item.ResNames);

		string outP = GetOutPath (CreateAssetBundle.AssetPath);

		//FileTools.ClearDirectory (outP);
		FileTools.CreateDirectory (outP);

		BuildPipeline.BuildAssetBundles (outP, BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);

		CreateAssetBundle.SetDependencies (resInfo, outP); 

		CreateAssetBundle.SaveResInfoFile (FileTools.CombinePath(outP, name + CreateAssetBundle.ResInfoName), resInfo);

		CreateAssetBundle.ClearAssetName (rootName, resInfo);

        //CopyToSteaming(rootName, name);
        PublishEditor.MoveModuleAssetToRes(name);
		AssetDatabase.Refresh();
		//EditorUtility.DisplayDialog("提示", "打包AssetBundle完成！", "确定");
	}

	static void UploadAsset(string rootName, string name, int codeV, int resV) {

        string ResPath = "D:\\Game\\" + name + "\\" + codeV;
		FileTools.CreateDirectory (ResPath);

		ZipAndCopy (ResPath, name);

		string outP = GetOutPath (CreateAssetBundle.AssetPath);
		string resP = Path.Combine (outP, name + CreateAssetBundle.ResInfoName);
		FileTools.CopyFile (resP, FileTools.CombinePath (ResPath, name + CreateAssetBundle.ResInfoName));

		EditorUtility.DisplayDialog ("提示", "上传完成！", "确定");
	}

	static void CopyToSteaming(string rootName, string name) {
		string ResPath = FileTools.CombinePath (Application.streamingAssetsPath, CreateAssetBundle.AssetPath);
		FileTools.CreateDirectory (ResPath);

		ZipAndCopy (ResPath, name);
	}

	static void ZipAndCopy(string outPath, string name) {
		string outP = GetOutPath (CreateAssetBundle.AssetPath);
		string resP = Path.Combine (outP, name + CreateAssetBundle.ResInfoName);

		string info = File.ReadAllText (Path.Combine (outP, name+CreateAssetBundle.ResInfoName));
		ResourceInfo itemInfo = JsonUtility.FromJson<ResourceInfo> (info);
		BundleNames.Clear ();
		for (int i = 0; i < itemInfo.ResList.Count; i++) {
			if (itemInfo.ResList [i].IsAsset) {
				BundleNames.Add (itemInfo.ResList [i].Name);
                string filePath = Path.Combine(outP, itemInfo.ResList[i].Name);
                long size = FileTools.GetFileSize(filePath);
                itemInfo.ResLength += size;
            }
		}
        CreateAssetBundle.SaveResInfoFile(FileTools.CombinePath(outP, name + CreateAssetBundle.ResInfoName), itemInfo);
        BundleNames.Add (name + CreateAssetBundle.ResInfoName);
		string zipPath = Path.Combine(outPath, name + ".zip");
		ZipUtils.CreateZipFile (zipPath, outP, BundleNames);
	}

    public static void GetResItemInfo(ResourceInfo resInfo, string itemPath, bool isResident, List<string> assetNames)
    {
        string[] Dirs = Directory.GetDirectories("Assets/" + ModulePath + "/" + itemPath);
        for (int i = 0; i < Dirs.Length; i++)
        {
            string path = Dirs[i].Replace("\\", "/");
            string name = FileTools.GetFullName(path);
            ResourceItemInfo item = new ResourceItemInfo();
            item.Path = itemPath + "/" + name;
            item.Name = name;
            item.IsResident = isResident;
            item.ResNames = new List<string>();
            resInfo.ResList.Add(item);
            assetNames.Add(item.Name);
            item.IsAsset = AssetHeplerEditor.SetAssetBundleName(path, item.Name, item.ResNames);
        }
    }
}
