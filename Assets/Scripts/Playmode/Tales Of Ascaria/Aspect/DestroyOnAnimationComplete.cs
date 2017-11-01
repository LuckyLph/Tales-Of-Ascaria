using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class DestroyOnAnimationComplete : GameScript
  {
    private EntityDestroyer entityDestroyer;

    private void InjectDestroyOnAnimationComplete([SiblingsScope] EntityDestroyer entityDestroyer)
    {
      this.entityDestroyer = entityDestroyer;
    }

    private void Awake()
    {
      InjectDependencies("InjectDestroyOnAnimationComplete");
    }

    public void OnAnimationComplete()
    {
      entityDestroyer.Destroy();
    }
  }
}