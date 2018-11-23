using UnityEngine;

public class LoadTest : MonoBehaviour {

    public MeshRenderer meshrender;

	// Use this for initialization
	void Start () {
      

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ResSlightly.Instance.LoadTexture2D("https://www.gamersky.com/showimage/id_gamersky.shtml?http://img1.gamersky.com/image2018/11/20181120_lxy_357_1/gamersky_01origin_01_20181120153933C.jpg",
         (tex) =>
         {
             Debug.Log("aaaaa");
             meshrender.material.mainTexture = tex;

         }, true,true);
        }
	}
}
