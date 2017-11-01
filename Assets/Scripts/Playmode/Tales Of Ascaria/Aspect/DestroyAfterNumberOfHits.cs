using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class DestroyAfterNumberOfHits : GameScript
  {
    [Tooltip("Number of hits this entity can take before it is destroyed")]
    [SerializeField]
    private int numberOfHits;

    private EntityDestroyer destroyer;

    private void InjectDestroyAfterNumberOfHits([EntityScope] EntityDestroyer destroyer)
    {
      this.destroyer = destroyer;
    }

    private void Awake()
    {
      InjectDependencies("InjectDestroyAfterNumberOfHits");
    }

    public void TakeHit()
    {
      numberOfHits--;
      if (numberOfHits <= 0)
      {
        DestroyEntity();
      }
    }

    private void DestroyEntity()
    {
      destroyer.Destroy();
    }
  }
}