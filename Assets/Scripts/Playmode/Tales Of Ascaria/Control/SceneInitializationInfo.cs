using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class SceneInitializationInfo : GameScript
  {
    public int OrderInLayer
    {
      get { return orderInLayer; }
      private set { orderInLayer = value; }
    }

    [Tooltip("The order in layer the scene's gameobjects' sprite renderers must be set to.")]
    [SerializeField]
    private int orderInLayer;
  }
}