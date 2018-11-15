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
	public static Dictionary<uint, ActionAudioInfo> ActionAudioInfoDict = new Dictionary<uint, ActionAudioInfo>();
	public static Dictionary<uint, ActorInfo> ActorInfoDict = new Dictionary<uint, ActorInfo>();
	public static Dictionary<uint, AudioEnglishInfo> AudioEnglishInfoDict = new Dictionary<uint, AudioEnglishInfo>();
	public static Dictionary<uint, AudioInfo> AudioInfoDict = new Dictionary<uint, AudioInfo>();
	public static Dictionary<uint, EnglishColoursInfo> EnglishColoursInfoDict = new Dictionary<uint, EnglishColoursInfo>();
	public static Dictionary<uint, EnglishLetterInfo> EnglishLetterInfoDict = new Dictionary<uint, EnglishLetterInfo>();
	public static Dictionary<uint, EnglishLevelInfo> EnglishLevelInfoDict = new Dictionary<uint, EnglishLevelInfo>();
	public static Dictionary<uint, EnglishMainItemInfo> EnglishMainItemInfoDict = new Dictionary<uint, EnglishMainItemInfo>();
	public static Dictionary<uint, ExpressPhotosInfo> ExpressPhotosInfoDict = new Dictionary<uint, ExpressPhotosInfo>();
	public static Dictionary<uint, MainAnimation> MainAnimationDict = new Dictionary<uint, MainAnimation>();
	public static Dictionary<uint, MainItemInfo> MainItemInfoDict = new Dictionary<uint, MainItemInfo>();
	public static Dictionary<uint, MatrixLevel> MatrixLevelDict = new Dictionary<uint, MatrixLevel>();
	public static Dictionary<uint, MatrixLevelInfo> MatrixLevelInfoDict = new Dictionary<uint, MatrixLevelInfo>();
	public static Dictionary<uint, NewBieInfo> NewBieInfoDict = new Dictionary<uint, NewBieInfo>();
	public static Dictionary<uint, NewBieScript> NewBieScriptDict = new Dictionary<uint, NewBieScript>();
	public static Dictionary<uint, SensitiveWord> SensitiveWordDict = new Dictionary<uint, SensitiveWord>();
	public static Dictionary<uint, SuperMarketGoodsInfo> SuperMarketGoodsInfoDict = new Dictionary<uint, SuperMarketGoodsInfo>();
	public static Dictionary<uint, TimelineInfo> TimelineInfoDict = new Dictionary<uint, TimelineInfo>();
	public static void Load() {
		path = FileTools.CombinePath (TABLE_PATH, "actionaudioinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		ActionAudioInfo_ARRAY ActionAudioInfo_items = Serializer.Deserialize<ActionAudioInfo_ARRAY>(stream);
		for (int i = 0; i < ActionAudioInfo_items.items.Count; i++) {
			ActionAudioInfoDict.Add (ActionAudioInfo_items.items [i].id, ActionAudioInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "actorinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		ActorInfo_ARRAY ActorInfo_items = Serializer.Deserialize<ActorInfo_ARRAY>(stream);
		for (int i = 0; i < ActorInfo_items.items.Count; i++) {
			ActorInfoDict.Add (ActorInfo_items.items [i].id, ActorInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "audioenglishinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		AudioEnglishInfo_ARRAY AudioEnglishInfo_items = Serializer.Deserialize<AudioEnglishInfo_ARRAY>(stream);
		for (int i = 0; i < AudioEnglishInfo_items.items.Count; i++) {
			AudioEnglishInfoDict.Add (AudioEnglishInfo_items.items [i].id, AudioEnglishInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "audioinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		AudioInfo_ARRAY AudioInfo_items = Serializer.Deserialize<AudioInfo_ARRAY>(stream);
		for (int i = 0; i < AudioInfo_items.items.Count; i++) {
			AudioInfoDict.Add (AudioInfo_items.items [i].id, AudioInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "englishcoloursinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		EnglishColoursInfo_ARRAY EnglishColoursInfo_items = Serializer.Deserialize<EnglishColoursInfo_ARRAY>(stream);
		for (int i = 0; i < EnglishColoursInfo_items.items.Count; i++) {
			EnglishColoursInfoDict.Add (EnglishColoursInfo_items.items [i].id, EnglishColoursInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "englishletterinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		EnglishLetterInfo_ARRAY EnglishLetterInfo_items = Serializer.Deserialize<EnglishLetterInfo_ARRAY>(stream);
		for (int i = 0; i < EnglishLetterInfo_items.items.Count; i++) {
			EnglishLetterInfoDict.Add (EnglishLetterInfo_items.items [i].id, EnglishLetterInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "englishlevelinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		EnglishLevelInfo_ARRAY EnglishLevelInfo_items = Serializer.Deserialize<EnglishLevelInfo_ARRAY>(stream);
		for (int i = 0; i < EnglishLevelInfo_items.items.Count; i++) {
			EnglishLevelInfoDict.Add (EnglishLevelInfo_items.items [i].id, EnglishLevelInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "englishmainiteminfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		EnglishMainItemInfo_ARRAY EnglishMainItemInfo_items = Serializer.Deserialize<EnglishMainItemInfo_ARRAY>(stream);
		for (int i = 0; i < EnglishMainItemInfo_items.items.Count; i++) {
			EnglishMainItemInfoDict.Add (EnglishMainItemInfo_items.items [i].id, EnglishMainItemInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "expressphotosinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		ExpressPhotosInfo_ARRAY ExpressPhotosInfo_items = Serializer.Deserialize<ExpressPhotosInfo_ARRAY>(stream);
		for (int i = 0; i < ExpressPhotosInfo_items.items.Count; i++) {
			ExpressPhotosInfoDict.Add (ExpressPhotosInfo_items.items [i].id, ExpressPhotosInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "mainanimation");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		MainAnimation_ARRAY MainAnimation_items = Serializer.Deserialize<MainAnimation_ARRAY>(stream);
		for (int i = 0; i < MainAnimation_items.items.Count; i++) {
			MainAnimationDict.Add (MainAnimation_items.items [i].id, MainAnimation_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "mainiteminfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		MainItemInfo_ARRAY MainItemInfo_items = Serializer.Deserialize<MainItemInfo_ARRAY>(stream);
		for (int i = 0; i < MainItemInfo_items.items.Count; i++) {
			MainItemInfoDict.Add (MainItemInfo_items.items [i].id, MainItemInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "matrixlevel");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		MatrixLevel_ARRAY MatrixLevel_items = Serializer.Deserialize<MatrixLevel_ARRAY>(stream);
		for (int i = 0; i < MatrixLevel_items.items.Count; i++) {
			MatrixLevelDict.Add (MatrixLevel_items.items [i].id, MatrixLevel_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "matrixlevelinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		MatrixLevelInfo_ARRAY MatrixLevelInfo_items = Serializer.Deserialize<MatrixLevelInfo_ARRAY>(stream);
		for (int i = 0; i < MatrixLevelInfo_items.items.Count; i++) {
			MatrixLevelInfoDict.Add (MatrixLevelInfo_items.items [i].id, MatrixLevelInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "newbieinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		NewBieInfo_ARRAY NewBieInfo_items = Serializer.Deserialize<NewBieInfo_ARRAY>(stream);
		for (int i = 0; i < NewBieInfo_items.items.Count; i++) {
			NewBieInfoDict.Add (NewBieInfo_items.items [i].id, NewBieInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "newbiescript");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		NewBieScript_ARRAY NewBieScript_items = Serializer.Deserialize<NewBieScript_ARRAY>(stream);
		for (int i = 0; i < NewBieScript_items.items.Count; i++) {
			NewBieScriptDict.Add (NewBieScript_items.items [i].id, NewBieScript_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "sensitiveword");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		SensitiveWord_ARRAY SensitiveWord_items = Serializer.Deserialize<SensitiveWord_ARRAY>(stream);
		for (int i = 0; i < SensitiveWord_items.items.Count; i++) {
			SensitiveWordDict.Add (SensitiveWord_items.items [i].id, SensitiveWord_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "supermarketgoodsinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		SuperMarketGoodsInfo_ARRAY SuperMarketGoodsInfo_items = Serializer.Deserialize<SuperMarketGoodsInfo_ARRAY>(stream);
		for (int i = 0; i < SuperMarketGoodsInfo_items.items.Count; i++) {
			SuperMarketGoodsInfoDict.Add (SuperMarketGoodsInfo_items.items [i].id, SuperMarketGoodsInfo_items.items [i]);
		}
		path = FileTools.CombinePath (TABLE_PATH, "timelineinfo");
		resource = ResourceManager.Instance.GetResource(path, typeof(TextAsset), enResourceType.TableProto);
		binaryObject = resource.m_content as BinaryObject;
		stream = new MemoryStream (binaryObject.m_data);
		TimelineInfo_ARRAY TimelineInfo_items = Serializer.Deserialize<TimelineInfo_ARRAY>(stream);
		for (int i = 0; i < TimelineInfo_items.items.Count; i++) {
			TimelineInfoDict.Add (TimelineInfo_items.items [i].id, TimelineInfo_items.items [i]);
		}
	}
}