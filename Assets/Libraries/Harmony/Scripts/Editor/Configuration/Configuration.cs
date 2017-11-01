using UnityEngine;

namespace Harmony
{
    /// <summary>
    /// Représente la configuration de Harmony.
    /// </summary>
    public class Configuration : ScriptableObject
    {
        [SerializeField]
        private R.E.Scene startingScene = R.E.Scene.None;

        [SerializeField]
        private R.E.Scene[] utilitaryScenes = { };

        public R.E.Scene StartingScene
        {
            get { return startingScene; }
        }

        public R.E.Scene[] UtilitaryScenes
        {
            get { return utilitaryScenes; }
        }
    }
}