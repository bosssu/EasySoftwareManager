using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;

public class TemplateEditor : EndNameEditAction
{

    [MenuItem("Assets/Create/MyCustomShader/DefaultUI_Shader",false,84)]
    public static void CreateUIDefault()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
       ScriptableObject.CreateInstance<TemplateEditor>(),
       GetSelectedPath() + "/NewDefaultUI.shader", null,
       "Assets/Editor/ShaderTemplates/1_UI-Default.shader.txt");
    }

    [MenuItem("Assets/Create/MyCustomShader/NewSurface_Shader", false, 84)]
    public static void CreateCustomSurface()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
       ScriptableObject.CreateInstance<TemplateEditor>(),
       GetSelectedPath() + "/NewSurface.shader", null,
       "Assets/Editor/ShaderTemplates/2_CustomSurface.shader.txt");
    }

    [MenuItem("Assets/Create/MyCustomShader/CustomLightShader", false, 84)]
    public static void CreateCustomLight()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
       ScriptableObject.CreateInstance<TemplateEditor>(),
       GetSelectedPath() + "/CustomLightShader.shader", null,
       "Assets/Editor/ShaderTemplates/3_CustomLightShader.shader.txt");
    }


    private static string GetSelectedPath()
    {
        //默认路径为Assets
        string selectedPath = "Assets";

        //获取选中的资源
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

        //遍历选中的资源以返回路径
        foreach (Object obj in selection)
        {
            selectedPath = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(selectedPath) && File.Exists(selectedPath))
            {
                selectedPath = Path.GetDirectoryName(selectedPath);
                break;
            }
        }

        return selectedPath;
    }

    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        //创建资源
        Object obj = CreateAssetFormTemplate(pathName, resourceFile);
        //高亮显示该资源
        ProjectWindowUtil.ShowCreatedAsset(obj);
    }

    internal static Object CreateAssetFormTemplate(string pathName, string resourceFile)
    {

        //获取要创建资源的绝对路径
        string fullName = Path.GetFullPath(pathName);
        //读取本地模版文件
        StreamReader reader = new StreamReader(resourceFile);
        string content = reader.ReadToEnd();
        reader.Close();

        //获取资源的文件名
        string fileName = Path.GetFileNameWithoutExtension(pathName);
        //替换默认的文件名
        content = content.Replace("#NAME", fileName);
        content = content.Replace("#AUTHOR", "xiaobing(819661664@qq.com)");
        content = content.Replace("#DATE", System.DateTime.Today.ToShortDateString());


        //写入新文件
        StreamWriter writer = new StreamWriter(fullName, false, System.Text.Encoding.UTF8);
        writer.Write(content);
        writer.Close();

        //刷新本地资源
        AssetDatabase.ImportAsset(pathName);
        AssetDatabase.Refresh();

        return AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
    }

}
