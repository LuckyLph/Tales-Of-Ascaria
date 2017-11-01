using System.Collections;
using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  public class CircleAOEController : GameScript
  {
    [Tooltip("Duration of the attack in seconds")]
    [SerializeField]
    private float durationInSeconds;

    private EntityDestroyer entityDestroyer;

    private void InjectCircleAOEController([GameObjectScope] EntityDestroyer entityDestroyer)
    {
      this.entityDestroyer = entityDestroyer;
    }

    private void Awake()
    {
      InjectDependencies("InjectCircleAOEController");
      entityDestroyer.Destroy(durationInSeconds, gameObject.transform.parent.gameObject);
    }

    public void ActivateSpell()
    {
      StartCoroutine(DeactivateAfter(durationInSeconds));
    }

    private IEnumerator DeactivateAfter(float duration)
    {
      yield return new WaitForSeconds(duration);

      yield return null;
    }
  }
}