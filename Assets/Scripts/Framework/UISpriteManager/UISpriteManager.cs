using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System;

public class UISpriteManager : Singleton<UISpriteManager> {

	private Dictionary<emUIAltas, SpriteAtlas> atlas = new Dictionary<emUIAltas, SpriteAtlas>();

	private Dictionary<string, SpriteAtlas> nameAtlas = new Dictionary<string, SpriteAtlas>();

	public override void Init ()
	{
		base.Init ();

		SpriteAtlasManager.atlasRequested += OnAtlasRequested;
	}

	public void LoadAtlas(emUIAltas al) {
		//foreach (emUIAltas al in Enum.GetValues(typeof(emUIAltas))) {
			string fullName = EnumUtils.GetString<emUIAltas> (al);
			//Debug.LogError (fullName);
			Resource res = ResourceManager.Instance.GetResource (fullName, typeof(SpriteAtlas), enResourceType.UIPrefab, false, false);
			//Debug.LogError (res.m_content);
			if (res.m_content != null) {
				atlas [al] = res.m_content as SpriteAtlas;
				string name = FileTools.GetFullName (fullName);
				//Debug.LogError (name);
				nameAtlas [name] = atlas [al];
			}
		//}
	}

	public override void UnInit ()
	{
		base.UnInit ();
		SpriteAtlasManager.atlasRequested -= OnAtlasRequested;
	}

	private void OnAtlasRequested(string tag, Action<SpriteAtlas> action) {
		Debug.Log (tag);
		if (nameAtlas.ContainsKey (tag)) {
			action (nameAtlas [tag]);
		}
	}

	public Sprite GetSprite(emUIAltas alt, string name) {
		if (atlas.ContainsKey (alt)) {
			return atlas [alt].GetSprite (name);
		}
		return null;
	}
}
