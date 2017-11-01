using System;
using System.Collections;
using UnityEngine;
using Harmony;
using UnityEngine.SceneManagement;

namespace TalesOfAscaria
{
  [Obsolete("Ben a probablement une meilleure solution",false)]
  public class SceneSwitcher : GameScript
  {
    [Tooltip("La scène qui sera loadée apres le unload de la scène actuelle")] [SerializeField]
    private R.E.Scene sceneToLoad;

    [Tooltip("La scène qui sera déchargée")][SerializeField]
    private R.E.Scene sceneToUnload;

    private void Awake()
    {
    }

    public void SwitchScene()
    {
    }

    private AsyncOperation UnloadScene()
    {
      return SceneManager.UnloadSceneAsync(R.S.Scene.ToString(sceneToUnload));
    }

    private AsyncOperation LoadScene()
    {
      return SceneManager.LoadSceneAsync(R.S.Scene.ToString(sceneToLoad));
    }

    private IEnumerator UnloadThenLoad()
    {
      AsyncOperation operation = UnloadScene();
      yield return new WaitUntil(()=> operation.isDone);
      operation = LoadScene();
      yield return new WaitUntil(()=> operation.isDone);
      SceneManager.SetActiveScene(SceneManager.GetSceneByName(R.S.Scene.ToString(sceneToLoad)));
    }
  }
}