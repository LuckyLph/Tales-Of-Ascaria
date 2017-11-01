using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class WeaselController : MobController 
	{
    [Tooltip("The explosion to instanciate on the weasel's death")]
    [SerializeField]
    private GameObject explosion;

    private StaticAnimator explosionAnimator;

    private void InjectWeaselController([SiblingsScope] StaticAnimator explosionAnimator)
    {
      this.explosionAnimator = explosionAnimator;
    }

    protected override void Awake() 
		{
      base.Awake();
      InjectDependencies("InjectWeaselController");
		}

    public override void Attack(MobAttackIndex attacksIndex, Vector2 directionToFace)
    {
      if (livingEntity.GetCrowdControl().SnareCounter <= 0 && livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        directionAnimator.SetDirection(directionToFace);
        switch (attacksIndex)
        {
          case MobAttackIndex.PrimaryAttack:
            mobCore.UsePrimaryAttack(stats.GetStatsSnapshot(), directionToFace);
            break;
        }
      }
    }

    public void MobDeathComplete(bool mustExplode)
    {
      base.MobDeathComplete();
      if (mustExplode)
      {
        GameObject explosionClone = Instantiate(explosion, transform.parent.transform.position, Quaternion.identity);
        explosionClone.GetComponentInChildren<ExplosionController>().SetExplosionParameters(stats.GetStatsSnapshot());
      }
    }
  }
}