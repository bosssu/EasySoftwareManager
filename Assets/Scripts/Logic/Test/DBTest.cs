using DB;
using UnityEngine;

public class DBTest : MonoBehaviour
{

    private string appDBPath;
    private DbAccess db;

    private void Start()
    {
        OnAllPageLoaded();
    }

    private void OnAllPageLoaded()
    {
        appDBPath = Application.persistentDataPath + "/zyc.db";
        db = new DbAccess("URI=file:" + appDBPath);
        Debug.Log(appDBPath);

        //建立数据库table
        //db.DropTable("BedO2O");
        //系列 分格名 风格价格 分格原价 尺寸名 材质 织造工艺 货号 面料密度 套装码(记录套装由 哪几个单独部分组成）
        db.CreateTable("BedO2O", new string[] {"series","styleTitle",
                "sizeTitle","Mat","gongyi","huohao","midu","suoshu","tao"}, new string[] { "text", "text", "text", "text", "text", "text", "text", "text", "text" });
        //db.InsertInto("BedO2O", new string[] { "'" + item.good.title + "'", styleStr.ToString(), sizeStr.ToString(), matValue, gongyiValue, huohaoValue, miduValue, suoshuValue, taoValue });
        Debug.Log("Load finished");

        db.CloseSqlConnection();
    }
}
