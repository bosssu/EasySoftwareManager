
using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class LoadTest : MonoBehaviour {

    public int frame;

    public string color_html = "FFFFFFFF";
    public Color col;

    public MeshRenderer render;

    void Start()
    {
        //StartCoroutine(WaitWhileTest());

        //原生交互
        //AndroidJavaClass javaclass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaProxy androidJavaProxy;
        //javaclass.CallStatic
        //CreatePlanes();

        //ImageConverTest();

    }

    public void ImageConverTest()
    {
        //注意：这里不能为null
        Texture2D t2d = new Texture2D(2,2);
        byte[] bfs = File.ReadAllBytes(FileTools.CombinePath(Application.streamingAssetsPath, "test.jpg"));
        if (ImageConversion.LoadImage(t2d, bfs))
        {
            render.material.mainTexture = t2d;
        }
        else
        {
            Debug.Log("解析失败");
        }
    }

    public void CreatePlanes()
    {
        // Calculate the planes from the main camera's view frustum
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // Create a "Plane" GameObject aligned to each of the calculated planes
        for (int i = 0; i < 6; ++i)
        {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
            p.name = "Plane " + i.ToString();
            p.transform.position = -planes[i].normal * planes[i].distance;
            p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
        }
    }

    [ContextMenu("ColorConvertTest")]
    public void ConvertColorTest()
    {
        //color_html = ColorUtility.ToHtmlStringRGBA(col);
        if (ColorUtility.TryParseHtmlString(color_html, out col))
        {
            Debug.Log("转换成功");
        }
    }

    //waitWhile
    IEnumerator WaitWhileTest()
    {
        Debug.Log("Waiting for prince/princess to rescue me...");
        yield return new WaitWhile(() => frame < 10);
        Debug.Log("Finally I have been rescued!");
    }

    //waitUntil
    IEnumerator WaitUntilTest()
    {
        Debug.Log("Waiting for prince/princess to rescue me...");
        yield return new WaitUntil(() => frame >= 10);
        Debug.Log("Finally I have been rescued!");
    }

    void Update()
    {
        if (frame <= 10)
        {
            Debug.Log("Frame: " + frame);
            frame++;
        }
    }

}
