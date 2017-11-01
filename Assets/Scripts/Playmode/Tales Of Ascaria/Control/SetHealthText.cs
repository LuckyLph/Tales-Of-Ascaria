using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class set : GameScript
  {
    private MeshRenderer meshRenderer;

    public void InjectSetHealthText([GameObjectScope] MeshRenderer meshRenderer)
    {
      this.meshRenderer = meshRenderer;
    }

    private void Awake()
    {
      InjectDependencies("InjectSetHealthText");
      meshRenderer.sortingLayerName = "Default";
      meshRenderer.sortingOrder = int.MaxValue;
    }
  }
}