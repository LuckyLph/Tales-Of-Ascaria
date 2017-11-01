using UnityEditor;

namespace TalesOfAscaria
{
    [CustomEditor(typeof(PrefabInstanceCollection), true)]
    public class PrefabInstanceCollectionInspector : GameScriptInspector
    {
        private PrefabInstanceCollection prefabInstanceCollection;

        private new void Awake()
        {
            base.Awake();
            prefabInstanceCollection = target as PrefabInstanceCollection;
        }

        private new void OnDestroy()
        {
            base.OnDestroy();
            prefabInstanceCollection = null;
        }

        protected override void OnDraw()
        {
            base.OnDraw();
            if (EditorApplication.isPlaying)
            {
                DrawInfoBox("Number of elements : " + prefabInstanceCollection.Count);
            }
        }
    }
}