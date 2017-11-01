using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class InitializeGameObject : GameScript
  {
    private SceneInitializationInfo sceneinitializationInfo;
    private SpriteRenderer spriteRenderer;

    private void InjectInitializeGameObject([SceneScope] SceneInitializationInfo sceneinitializationInfo,
                                            [SiblingsScope] SpriteRenderer spriteRenderer)
    {
      this.sceneinitializationInfo = sceneinitializationInfo;
      this.spriteRenderer = spriteRenderer;
    }

    private void Awake()
    {
      InjectDependencies("InjectInitializeGameObject");
    }

    private void Start()
    {
      spriteRenderer.sortingOrder = sceneinitializationInfo.OrderInLayer;
    }
  }
}