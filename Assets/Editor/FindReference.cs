using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class FindReferences
{

    private static string[] files;
    private static string withoutExtensions = "*.prefab*.unity*.mat*.asset";
    private static string[] meta_data_text;

    [MenuItem("Assets/FindReference/Find References DataPreload", false)]
    static void PreloadBuffer()
    {
        files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
            .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

        meta_data_text = new string[files.Length];
        for (int i = 0; i < files.Length; i++)
        {
            meta_data_text[i] = File.ReadAllText(files[i]);
        }
    }

    [MenuItem("Assets/FindReference/Find References", false)]
    static private void Find()
    {
        EditorSettings.serializationMode = SerializationMode.ForceText;
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!string.IsNullOrEmpty(path))
        {
            Debug.Log("匹配开始");
            string guid = AssetDatabase.AssetPathToGUID(path);
            if (files == null) PreloadBuffer();

            int startIndex = 0;

            EditorApplication.update = delegate ()
            {
                string file = files[startIndex];

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("匹配资源中", file, (float)startIndex / (float)files.Length);

                if (Regex.IsMatch(meta_data_text[startIndex], guid))
                {
                    Debug.LogError(file, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));
                }

                startIndex++;
                if (isCancel || startIndex >= files.Length)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;
                }

            };
        }
    }

    [MenuItem("Assets/Find References", true,1000)]
    static private bool VFind()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        return (!string.IsNullOrEmpty(path));
    }

    static private string GetRelativeAssetsPath(string path)
    {
        return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }
}