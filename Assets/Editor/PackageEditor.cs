using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PackageEditor
{

    public static string VariResPath
    {
        get
        {
            return "Assets/ResourcesVari/";
        }
    }

    private static string UpdatePath = "ui/ui_update/UpdateForm.prefab";

    [MenuItem("发布/游戏包切换/超级飞侠乐园")]
    public static void ToPack()
    {
        RemoveAllSymbols();
        AssetHeplerEditor.AddSymbolsForGroup(BuildTargetGroup.Android, "SUPER_PARK");
        AssetDatabase.CopyAsset(VariResPath + "ui_update/UpdateForm_park.prefab", PublishEditor.ResPath + UpdatePath);
        AssetDatabase.Refresh();
    }

    [MenuItem("发布/游戏包切换/我的超级飞侠")]
    public static void ToMy()
    {
        RemoveAllSymbols();
        AssetHeplerEditor.AddSymbolsForGroup(BuildTargetGroup.Android, "SUPER_MY");
        AssetDatabase.CopyAsset(VariResPath + "ui_update/UpdateForm_my.prefab", PublishEditor.ResPath + UpdatePath);
        AssetDatabase.Refresh();
    }

    [MenuItem("发布/游戏包切换/超级飞侠英语")]
    public static void ToEnglish()
    {
        RemoveAllSymbols();
        AssetHeplerEditor.AddSymbolsForGroup(BuildTargetGroup.Android, "PACKAGE_ENGLISH");
        AssetDatabase.CopyAsset(VariResPath + "ui_update/UpdateForm_english.prefab", PublishEditor.ResPath + UpdatePath);
        AssetDatabase.Refresh();
    }

    [MenuItem("发布/游戏包切换/超级飞侠数学")]
    public static void ToMath()
    {
        RemoveAllSymbols();
        AssetHeplerEditor.AddSymbolsForGroup(BuildTargetGroup.Android, "PACKAGE_MATH");
        AssetDatabase.CopyAsset(VariResPath + "ui_update/UpdateForm_math.prefab", PublishEditor.ResPath + UpdatePath);
        AssetDatabase.Refresh();
    }

    private static void RemoveAllSymbols()
    {
        AssetHeplerEditor.RemoveSymbolsForGroup(BuildTargetGroup.Android, "SUPER_MY");
        AssetHeplerEditor.RemoveSymbolsForGroup(BuildTargetGroup.Android, "SUPER_PARK");
        AssetHeplerEditor.RemoveSymbolsForGroup(BuildTargetGroup.Android, "PACKAGE_ENGLISH");
        AssetHeplerEditor.RemoveSymbolsForGroup(BuildTargetGroup.Android, "PACKAGE_MATH");

    }

}
