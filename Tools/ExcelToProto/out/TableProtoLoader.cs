using System.Collections.Generic;
using TableProto;
using UnityEngine;
using System.IO;
using ProtoBuf;

public class TableProtoLoader {

    static string TABLE_PATH = "table_data";

    static Resource resource;
    static BinaryObject binaryObject;
    static MemoryStream stream;
    static string path;
	public static Dictionary<uint, GoodsInfo> GoodsInfoDict = new Dictionary<uint, GoodsInfo>();
	public static Dictionary<uint, ItemsInfo> ItemsInfoDict = new Dictionary<uint, ItemsInfo>();
	public static void Load() {
		path = FileTools.CombinePath (TABLE_PATH, "goodsinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		GoodsInfo_ARRAY GoodsInfo_items = Serializer.Deserialize<GoodsInfo_ARRAY>(stream);
		for (int i = 0; i < GoodsInfo_items.items.Count; i++) {
			GoodsInfoDict.Add (GoodsInfo_items.items [i].id, GoodsInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "itemsinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		ItemsInfo_ARRAY ItemsInfo_items = Serializer.Deserialize<ItemsInfo_ARRAY>(stream);
		for (int i = 0; i < ItemsInfo_items.items.Count; i++) {
			ItemsInfoDict.Add (ItemsInfo_items.items [i].id, ItemsInfo_items.items [i]);
		}
	}
}