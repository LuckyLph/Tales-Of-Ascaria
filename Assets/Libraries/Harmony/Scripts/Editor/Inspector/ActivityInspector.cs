using UnityEditor;
using UnityEditor.SceneManagement;

namespace Harmony
{
    /// <summary>
    /// Inspecteur pour les Activités dans l'éditeur Unity.
    /// </summary>
    [CustomEditor(typeof(Activity), true)]
    public class ActivityInspector : Inspector
    {
        private EnumProperty scene;
        private EnumProperty controller;
        private SerializedProperty fragments;
        private SerializedProperty menus;
        private SerializedProperty activeFragmentOnLoad;

        private void Awake()
        {
            scene = GetEnumProperty("scene", typeof(R.E.Scene));
            controller = GetEnumProperty("controller", typeof(R.E.GameObject));
            fragments = GetListProperty("fragments");
            menus = GetListProperty("menus");
            activeFragmentOnLoad = GetBasicProperty("activeFragmentOnLoad");
        }

        private void OnDestroy()
        {
            scene = null;
            controller = null;
            fragments = null;
            menus = null;
            activeFragmentOnLoad = null;
        }

        protected override void OnDraw()
        {
            DrawTitleLabel("Activity tools");
            BeginHorizontal();
            if (!EditorApplication.isPlaying)
            {
                DrawButton("Open Activity", OpenActivityInEditor);
            }
            else
            {
                DrawDisabledButton("Open Activity");
            }
            EndHorizontal();

            DrawEnumPropertyGrid(scene);
            DrawEnumPropertyGrid(controller);
            DrawListProperty(fragments);
            DrawListProperty(menus);
            DrawBasicPropertyWithTitleLabel(activeFragmentOnLoad);
        }

        private void OpenActivityInEditor()
        {
            EditorSceneManager.SaveOpenScenes();

            //The Main scene must allways be loaded
            OpenSceneInEditor(R.E.Scene.Main, OpenSceneMode.Single);

            //Load Activity controller scene
            var scene = (R.E.Scene) GetEnumProperty("scene", typeof(R.E.Scene)).CurrentValue;
            if (scene != R.E.Scene.None)
            {
                OpenSceneInEditor(scene, OpenSceneMode.Additive);
            }

            //Load Activity fragments
            var fragments = GetListProperty("fragments");
            for (int i = 0; i < fragments.arraySize; i++)
            {
                OpenFragmentInEditor(fragments.GetArrayElementAtIndex(i).objectReferenceValue as Fragment,
                                     OpenSceneMode.Additive);
            }

            //Load Activity menus
            var menus = GetListProperty("menus");
            for (int i = 0; i < menus.arraySize; i++)
            {
                OpenMenuInEditor(menus.GetArrayElementAtIndex(i).objectReferenceValue as Menu,
                                 OpenSceneMode.Additive);
            }
        }

        private void OpenSceneInEditor(R.E.Scene scene, OpenSceneMode mode)
        {
            foreach (EditorBuildSettingsScene builtScene in EditorBuildSettings.scenes)
            {
                //The "/" in the front is to prevent some issues with similar scene names.
                //EX :
                //      NewHighScoreScene.unity and HighScoreScene.unity 
                if (builtScene.path.EndsWith("/" + R.S.Scene.ToString(scene) + ".unity"))
                {
                    EditorSceneManager.OpenScene(builtScene.path, mode);
                    break;
                }
            }
        }

        private void OpenFragmentInEditor(Fragment fragment, OpenSceneMode mode)
        {
            if (fragment != null && fragment.Scene != R.E.Scene.None)
            {
                OpenSceneInEditor(fragment.Scene, mode);
            }
        }

        private void OpenMenuInEditor(Menu menu, OpenSceneMode mode)
        {
            if (menu != null && menu.Scene != R.E.Scene.None)
            {
                OpenSceneInEditor(menu.Scene, mode);
            }
        }
    }
}