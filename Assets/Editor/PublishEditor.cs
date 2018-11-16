using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
#if UNITY_2018
using UnityEditor.Build.Reporting;
#endif

public class PublishEditor
{

    public enum PublishType
    {
        Park,
        My,
    }

    private const string AndroidPath = "D:/Chanel/";

    public static string ResPath
    {
        get
        {
            return "Assets/Resources/";
        }
    }

    public static string AssetPath
    {
        get
        {
            return "Assets/AssetBundle/";
        }
    }

    public static string ModuleAssetPath
    {
        get
        {
            return "Assets/AssetBundleModule/";
        }
    }

    public static string GetAPKPath(string name)
    {
        DirectoryInfo info = new DirectoryInfo(Application.dataPath);
        return info.Parent.FullName + "/" + name;
    }

    public static List<string> ResList = new List<string>()
    {
        //"table_data"
    };

    public static List<string> ModuleResList = new List<string>() {
        ModuleConst.testName,
    };

    public static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    public static void Publish()
    {
#if UNITY_2018
        BuildReport res = BuildPipeline.BuildPlayer(FindEnabledEditorScenes(), AndroidPath.Substring(0, AndroidPath.Length-1), BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
#else
        string res = BuildPipeline.BuildPlayer(FindEnabledEditorScenes(), GetAPKPath("SuperWings.apk"), BuildTarget.Android, BuildOptions.None);//出包
        if (res.Length > 0)
        {
            throw new Exception("BuildPlayer failure: " + res);
        }
#endif
    }

    private static void ExportAndroid(PublishType t)
    {
        string fileName = "Superwings";
        if (t == PublishType.My)
        {
            fileName = "Superwings_My";
        }
        string name = "Superwings_Unity";

        PlayerSettings.companyName = "xxxxxxxxx";
        PlayerSettings.productName = name;
        PlayerSettings.applicationIdentifier = "com.xxx.xxxxxx";
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.Android.bundleVersionCode = 100;


        DirectoryInfo info = new DirectoryInfo(Application.dataPath);
        FileUtil.DeleteFileOrDirectory(AndroidPath + name);

#if UNITY_2018
        BuildReport res = BuildPipeline.BuildPlayer(FindEnabledEditorScenes(), AndroidPath.Substring(0, AndroidPath.Length-1), BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
#else
        string res = BuildPipeline.BuildPlayer(FindEnabledEditorScenes(), AndroidPath.Substring(0, AndroidPath.Length - 1), BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
        if (res.Length > 0)
        {
            throw new Exception("BuildPlayer failure: " + res);
        }
#endif

        FileUtil.DeleteFileOrDirectory(AndroidPath + fileName + "/assets/bin");
        FileUtil.DeleteFileOrDirectory(AndroidPath + fileName + "/assets/AssetBundle");
        //FileUtil.DeleteFileOrDirectory(info.Parent.FullName + "/Android/" + fileName + "/res/drawable/unity_static_splash.png");
        //FileUtil.CopyFileOrDirectory(info.Parent.FullName + "/Android/" + name + "/res/drawable/unity_static_splash.png", info.Parent.FullName + "/Android/" + fileName + "/res/drawable/unity_static_splash.png");
        CopyDirectory(AndroidPath + name + "/assets/bin", AndroidPath + fileName + "/assets/bin");
        CopyDirectory(AndroidPath + name + "/assets/AssetBundle", AndroidPath + fileName + "/assets/AssetBundle");
        if (File.Exists(AndroidPath + name + "/assets/AppConfig.json"))
            File.Copy(AndroidPath + name + "/assets/AppConfig.json", AndroidPath + fileName + "/assets/AppConfig.json", true);
    }

    [MenuItem("发布/导出我的超级飞侠")]
    public static void PublishAndroidMy()
    {
        AssetHeplerEditor.RemoveSymbolsForGroup(BuildTargetGroup.Android, "SUPER_PARK");
        AssetHeplerEditor.AddSymbolsForGroup(BuildTargetGroup.Android, "SUPER_MY");
        PublishAndroid(PublishType.My);
        AssetHeplerEditor.RemoveSymbolsForGroup(BuildTargetGroup.Android, "SUPER_MY");
    }

    public static void PublishAndroid(PublishType t)
    {
        //OpenDebugMode();
        if (t == PublishType.My)
        {
            PackageEditor.ToMy();
        }
        else if (t == PublishType.Park)
        {
            PackageEditor.ToPack();
        }
        CopyResToAsset();
        CreateAssetBundle.CreateAsset();
        ExportAndroid(t);
        CopyAssetToRes();
        //CloseDebugMode();
        EditorUtility.DisplayDialog("提示", "导出完成！", "确定");
    }

    [MenuItem("发布/复制Resources到AssetBundle")]
    public static void CopyResToAsset()
    {
        foreach (ModuleItemInfo p in ModuleAssetBundle.ModuleList)
        {
            AssetDatabase.MoveAsset(ResPath + p.Name, ModuleAssetPath + p.Name);
            //FileTools.ClearDirectory(ResPath + p);
            //Directory.Delete(ResPath + p);
        }
        AssetDatabase.Refresh();

    }

    [MenuItem("发布/恢复AssetBundle到Resources")]
    public static void CopyAssetToRes()
    {
        foreach (ModuleItemInfo p in ModuleAssetBundle.ModuleList)
        {
            AssetDatabase.MoveAsset(ModuleAssetPath + p.Name, ResPath + p.Name);
            //FileTools.ClearDirectory(ModuleAssetPath + p);
            //Directory.Delete(ModuleAssetPath + p);
        }
        AssetDatabase.Refresh();
    }

    public static void MoveResToAsset()
    {
        foreach (string p in ResList)
        {
            AssetDatabase.MoveAsset(ResPath + p, AssetPath + p);
        }
    }

    public static void MoveAssetToRes()
    {
        foreach (string p in ResList)
        {
            AssetDatabase.MoveAsset(AssetPath + p, ResPath + p);
        }
    }

    public static void MoveResToModuleAsset(string name)
    {
        AssetDatabase.MoveAsset(ResPath + name, ModuleAssetPath + name);
    }

    public static void MoveModuleAssetToRes(string name)
    {
        AssetDatabase.MoveAsset(ModuleAssetPath + name, ResPath + name);
    }

    [MenuItem("发布/发布模式开关/开")]
    static void OpenDebugMode()
    {
        //PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, "LOCAL_DEBUG");
        AssetHeplerEditor.AddSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, "LOCAL_RELEASE");
    }

    [MenuItem("发布/发布模式开关/关")]
    static void CloseDebugMode()
    {
        //PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.Android, "");
        AssetHeplerEditor.RemoveSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, "LOCAL_RELEASE");
    }

    public static void CopyDirectory(String sourcePath, String destinationPath)
    {
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        if (!Directory.Exists(destinationPath))
        {
            Directory.CreateDirectory(destinationPath);
        }
        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            String destName = Path.Combine(destinationPath, fsi.Name);

            if (fsi is System.IO.FileInfo)          //如果是文件，复制文件
                File.Copy(fsi.FullName, destName, true);
            else                                    //如果是文件夹，新建文件夹，递归
            {
                if (!Directory.Exists(destName))
                {
                    Directory.CreateDirectory(destName);
                }
                CopyDirectory(fsi.FullName, destName);
            }
        }
    }

    [MenuItem("发布/清空PlayerPrefs数据")]
    public static void CleanPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

}
