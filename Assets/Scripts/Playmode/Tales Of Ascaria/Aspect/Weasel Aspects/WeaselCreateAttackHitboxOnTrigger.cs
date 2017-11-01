using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class WeaselCreateAttackHitboxOnTrigger : CreateHitboxOnTrigger
  {
    WeaselCore mobCore;

    private void InjectWeaselCreateAttackHitboxOnTrigger([SiblingsScope] WeaselCore mobCore)
    {
      this.mobCore = mobCore;
    }

    private void Awake()
    {
      InjectDependencies("InjectWeaselCreateAttackHitboxOnTrigger");
    }

    protected override void CreateHitbox()
    {
      mobCore.CreatePrimaryAttackHitbox();
    }
  }
}