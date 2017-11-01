using System.IO;
using UnityEditor;
using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Inspecteur de la configuration de Harmmony.
    /// </summary>
    [CustomEditor(typeof(Configuration), true)]
    public class ConfigurationInspector : Inspector
    {
        private const string ConfigurationFolderPathRelativeAssets = "Data/Configuration";
        private const string ConfigurationFilePathRelativeAssets = ConfigurationFolderPathRelativeAssets + "/Harmony.asset";
        private const string ConfigurationFilePathRelativeProject = "Assets/" + ConfigurationFilePathRelativeAssets;

        private Texture2D logo;

        private SerializedProperty productNameProperty;
        private SerializedProperty companyNameProperty;
        private SerializedProperty iconProperty;

        private EnumProperty startingScene;
        private SerializedProperty utilitaryScenes;

        private SerializedProperty tags;
        private SerializedProperty layers;

        private Physics2DLayerMatrixGuiProperty physics2DLayerMatrixProperty;

        [MenuItem("Harmony/Settings", priority = 100)]
        public static void OpenConfiguration()
        {
            AssetDatabase.OpenAsset(GetConfiguration());
        }

        public static Configuration GetConfiguration()
        {
            CreateConfigurationFileIfNotExists();

            return AssetDatabase.LoadAssetAtPath<Configuration>(ConfigurationFilePathRelativeProject);
        }

        private static void CreateConfigurationFileIfNotExists()
        {
            if (IsConfigurationFileDoesntExists())
            {
                Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, ConfigurationFolderPathRelativeAssets)));
                AssetDatabase.CreateAsset(CreateInstance<Configuration>(), ConfigurationFilePathRelativeProject);
            }
        }

        private static string GetConfigurationFilePath()
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, ConfigurationFilePathRelativeAssets));
        }

        private static bool IsConfigurationFileExists()
        {
            return File.Exists(GetConfigurationFilePath());
        }

        private static bool IsConfigurationFileDoesntExists()
        {
            return !IsConfigurationFileExists();
        }

        private void Awake()
        {
            logo = GetLogo();

            productNameProperty = GetProductNameProperty();
            companyNameProperty = GetCompanyNameProperty();
            iconProperty = GetIconProperty();

            startingScene = GetEnumProperty("startingScene", typeof(R.E.Scene));
            utilitaryScenes = GetListProperty("utilitaryScenes");

            tags = GetTagsProperty();
            layers = GetLayersProperty();

            physics2DLayerMatrixProperty = GetPhysics2DLayerMatrixProperty();
        }

        private void OnDestroy()
        {
            logo = null;

            productNameProperty = null;
            companyNameProperty = null;
            iconProperty = null;

            startingScene = null;
            utilitaryScenes = null;

            tags = null;
            layers = null;

            physics2DLayerMatrixProperty = null;
        }

        protected override void OnDraw()
        {
            DrawImage(logo);

            DrawSection("Application");
            DrawBasicProperty(productNameProperty);
            DrawBasicProperty(companyNameProperty);
            DrawBasicProperty(iconProperty);

            DrawSection("Startup");
            DrawEnumPropertyDropDown(startingScene);
            DrawListProperty(utilitaryScenes);

            DrawSection("Tags and Layers");
            DrawListProperty(tags);
            DrawListProperty(layers, true, 8);

            DrawSection("Physics");
            DrawTitleLabel("Physics 2D Layer Collison Matrix");
            DrawCustomProperty(physics2DLayerMatrixProperty);
        }

        private Texture2D GetLogo()
        {
            return (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Libraries/Harmony/Visuals/Editor/Icons/Harmony.png", typeof(Texture2D));
        }

        private SerializedProperty GetProductNameProperty()
        {
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
            return serializedObject.FindProperty("productName");
        }

        private SerializedProperty GetCompanyNameProperty()
        {
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
            return serializedObject.FindProperty("companyName");
        }

        private SerializedProperty GetIconProperty()
        {
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
            SerializedProperty allIcons = serializedObject.FindProperty("m_BuildTargetIcons").GetArrayElementAtIndex(0);
            return allIcons.FindPropertyRelative("m_Icons").GetArrayElementAtIndex(0).FindPropertyRelative("m_Icon");
        }

        private SerializedProperty GetTagsProperty()
        {
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            return serializedObject.FindProperty("tags");
        }

        private SerializedProperty GetLayersProperty()
        {
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            return serializedObject.FindProperty("layers");
        }

        private Physics2DLayerMatrixGuiProperty GetPhysics2DLayerMatrixProperty()
        {
            return new Physics2DLayerMatrixGuiProperty();
        }

        private void DrawCustomProperty(Physics2DLayerMatrixGuiProperty property)
        {
            if (property != null)
            {
                property.Draw();
            }
        }

        private sealed class Physics2DLayerMatrixGuiProperty
        {
            private bool GetValue(int layerA, int layerB)
            {
                return !Physics2D.GetIgnoreLayerCollision(layerA, layerB);
            }

            private void SetValue(int layerA, int layerB, bool isCollides)
            {
                Physics2D.IgnoreLayerCollision(layerA, layerB, !isCollides);
            }

            public void Draw()
            {
                /*
                 * Nb layers to draw
                 */
                int nbLayers = 0;
                for (int layer = 0; layer < 32; ++layer)
                {
                    if (LayerMask.LayerToName(layer) != "")
                    {
                        ++nbLayers;
                    }
                }
                /*
                 * Max layer label width
                 */
                int maxLayerNameWidth = 100;
                for (int layer = 0; layer < 32; ++layer)
                {
                    Vector2 vector2 = GUI.skin.label.CalcSize(new GUIContent(LayerMask.LayerToName(layer)));
                    if (maxLayerNameWidth < vector2.x)
                    {
                        maxLayerNameWidth = (int) vector2.x;
                    }
                }
                maxLayerNameWidth = maxLayerNameWidth > 150 ? 150 : maxLayerNameWidth;

                /*
                 * Vertical Labels
                 */
                int currentCollumPosition = 0;
                Rect labelsRect = GUILayoutUtility.GetRect(16 * nbLayers + maxLayerNameWidth, maxLayerNameWidth);
                GUIUtility.RotateAroundPivot(90, new Vector2(labelsRect.x, labelsRect.y));
                for (int layer = 0; layer < 32; layer++)
                {
                    if (LayerMask.LayerToName(layer) != "")
                    {
                        GUI.Label(new Rect(labelsRect.x,
                                           labelsRect.y - maxLayerNameWidth - 45 - 16 * currentCollumPosition,
                                           maxLayerNameWidth,
                                           16f),
                                  LayerMask.LayerToName(layer),
                                  "RightLabel");
                        currentCollumPosition++;
                    }
                }
                GUI.matrix = Matrix4x4.identity;

                /*
                 * Horrizontal Labels and Toggles
                 */
                int currentGridYPosition = 0;
                for (int layer = 0; layer < 32; layer++)
                {
                    if (LayerMask.LayerToName(layer) != "")
                    {
                        Rect lineRectangle = GUILayoutUtility.GetRect(30 + 16 * nbLayers + maxLayerNameWidth, 16f);

                        GUI.Label(new Rect(lineRectangle.x + 30f,
                                           lineRectangle.y,
                                           maxLayerNameWidth,
                                           16f),
                                  LayerMask.LayerToName(layer),
                                  "RightLabel");

                        int currentGridXPosition = 0;
                        for (int otherLayer = 31; otherLayer >= 0; otherLayer--)
                        {
                            if (LayerMask.LayerToName(otherLayer) != "")
                            {
                                if (currentGridXPosition < nbLayers - currentGridYPosition)
                                {
                                    GUIContent content = new GUIContent("", LayerMask.LayerToName(layer) + "/" + LayerMask.LayerToName(otherLayer));
                                    bool layersCollides = GetValue(layer, otherLayer);
                                    bool layersCollidesNew = GUI.Toggle(new Rect(maxLayerNameWidth + 30 + lineRectangle.x + currentGridXPosition * 16,
                                                                                 lineRectangle.y, 16f, 16f),
                                                                        layersCollides,
                                                                        content);
                                    if (layersCollidesNew != layersCollides)
                                    {
                                        SetValue(layer, otherLayer, layersCollidesNew);
                                    }
                                }
                                currentGridXPosition++;
                            }
                        }
                        currentGridYPosition++;
                    }
                }
            }
        }
    }
}