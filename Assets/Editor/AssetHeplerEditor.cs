using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetHeplerEditor {

	public static bool SetAssetBundleName(string dir, string nameBundle, List<string> names = null) {
		string[] files = Directory.GetFiles(dir);
		bool has = false;
		for (int i=0; i<files.Length; i++) {
			string name = files[i].Replace("\\", "/");
			if (files[i].IndexOf (".meta") > 0)
				continue;
			var mi = AssetImporter.GetAtPath(name);
			if (mi != null) {
				mi.assetBundleName = nameBundle.ToLower();
				if (names != null) {
					names.Add (FileTools.EraseExtension (FileTools.GetFullName (name)));
				}
				has = true;
			}
		}
		return has;
	}

	public static string GetPlatformName()
	{
		#if UNITY_EDITOR
		return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
		#else
		return GetPlatformForAssetBundles(Application.platform);
		#endif
	}

	#if UNITY_EDITOR
	private static string GetPlatformForAssetBundles(BuildTarget target)
	{
		switch(target)
		{
		case BuildTarget.Android:
			return "Android";
		case BuildTarget.iOS:
			return "iOS";
		case BuildTarget.WebGL:
			return "WebGL";
		case BuildTarget.StandaloneWindows:
		case BuildTarget.StandaloneWindows64:
			return "Windows";
		case BuildTarget.StandaloneOSXIntel:
		case BuildTarget.StandaloneOSXIntel64:
		case BuildTarget.StandaloneOSXUniversal:
			return "OSX";
			// Add more build targets for your own.
			// If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
		default:
			return null;
		}
	}
	#endif

	private static string GetPlatformForAssetBundles(RuntimePlatform platform)
	{
		switch(platform)
		{
		case RuntimePlatform.Android:
			return "Android";
		case RuntimePlatform.IPhonePlayer:
			return "iOS";
		case RuntimePlatform.WebGLPlayer:
			return "WebGL";
		case RuntimePlatform.WindowsPlayer:
			return "Windows";
		case RuntimePlatform.OSXPlayer:
			return "OSX";
			// Add more build targets for your own.
			// If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
		default:
			return null;
		}
	}

	public static void AddSymbolsForGroup(BuildTargetGroup platform, string Symbol) {
		string str = PlayerSettings.GetScriptingDefineSymbolsForGroup (platform);
		List<string> list = StringUtils.Split (str, ";", true, true);
		if (list.Contains (Symbol))
			return;
		list.Add (Symbol);
		PlayerSettings.SetScriptingDefineSymbolsForGroup (platform, StringUtils.CollectionToDelimitedString<string>(list, ";"));
	}

	public static void RemoveSymbolsForGroup(BuildTargetGroup platform, string Symbol) {
		string str = PlayerSettings.GetScriptingDefineSymbolsForGroup (platform);
		List<string> list = StringUtils.Split (str, ";", true, true);
		if (list.Contains (Symbol)) {
			list.Remove (Symbol);
			PlayerSettings.SetScriptingDefineSymbolsForGroup (platform, StringUtils.CollectionToDelimitedString<string>(list, ";"));
		}
	}
}
