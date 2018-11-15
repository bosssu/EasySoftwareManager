using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoSingleton<SceneLoader> {

	public static string EmptyScene = "Empty";

    public static string MainState = "MainState";

    public delegate void LoadCompletedDelegate();

	private string sceneName;

	private LoadCompletedDelegate finishDel;

	public void LoadScene(string name, SceneLoader.LoadCompletedDelegate finishDelegate = null)
	{
		this.sceneName = name;
		this.finishDel = finishDelegate;
		StartCoroutine (load ());
	}

	protected override void OnDestroy ()
	{
		StopCoroutine (load ());
		base.OnDestroy ();
	}

	private IEnumerator load() {
		SceneManager.LoadScene(sceneName);

        yield return null;

        if (finishDel != null)
        {
            finishDel();

            finishDel = null;
        }
	}
}
