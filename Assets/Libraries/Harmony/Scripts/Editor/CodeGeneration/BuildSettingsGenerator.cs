using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Harmony
{
  public static class BuildSettingsGenerator
  {
    [MenuItem("Harmony/Generate Build Settings From Activities", priority = 10)]
    public static void GenerateBuildSettings()
    {
      Configuration configuration = ConfigurationInspector.GetConfiguration();

      //This HashSet prevent from adding the same scene twice.
      HashSet<string> scenePaths = new HashSet<string>();

      //Add startup scene
      if (configuration.StartingScene != R.E.Scene.None)
      {
        string sceneName = R.S.Scene.ToString(configuration.StartingScene);
        scenePaths.Add(AssetsExtensions.FindScenePath(sceneName));
      }
      else
      {
        Debug.LogError("No scene configured as Starting Scene. Open \"Harmony/Settings\" to set it.");
      }

      //Add utilitary scenes
      foreach (R.E.Scene utilitaryScene in configuration.UtilitaryScenes)
      {
        string sceneName = R.S.Scene.ToString(utilitaryScene);
        scenePaths.Add(AssetsExtensions.FindScenePath(sceneName));
      }

      //Add Activity scenes
      foreach (Activity activity in AssetsExtensions.FindAssets<Activity>())
      {
        if (activity.Scene != R.E.Scene.None)
        {
          scenePaths.Add(AssetsExtensions.FindScenePath(R.S.Scene.ToString(activity.Scene)));
        }

        foreach (Fragment fragment in activity.Fragments)
        {
          if (fragment.Scene != R.E.Scene.None)
          {
            scenePaths.Add(AssetsExtensions.FindScenePath(R.S.Scene.ToString(fragment.Scene)));
          }
        }

        foreach (Menu menu in activity.Menus)
        {
          if (menu.Scene != R.E.Scene.None)
          {
            scenePaths.Add(AssetsExtensions.FindScenePath(R.S.Scene.ToString(menu.Scene)));
          }
        }
      }

      //Set the Editor Build Settings
      List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
      foreach (string scenePath in scenePaths)
      {
        scenes.Add(new EditorBuildSettingsScene(scenePath, true));
      }
      EditorBuildSettings.scenes = scenes.ToArray();
    }
  }
}