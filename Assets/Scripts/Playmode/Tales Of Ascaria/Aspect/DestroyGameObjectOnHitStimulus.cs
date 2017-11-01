using Harmony;

namespace TalesOfAscaria 
{
	public class DestroyGameObjectOnHitStimulus : GameScript
	{
	  private HitStimulus hitStimulus;

		private void Awake() 
		{
		  InjectDependencies("InjectToggleGameObjectOnHitStimulus");
		  hitStimulus.OnHit += DestroyGameObject;
		}

	  private void InjectToggleGameObjectOnHitStimulus([SiblingsScope] HitStimulus hitStimulus)
	  {
	    this.hitStimulus = hitStimulus;
	  }

	  public void DestroyGameObject(ActorAction actorAction)
	  {
	    Destroy(transform.root.gameObject);
	  }

	  private void OnDestroy()
	  {
	    hitStimulus.OnHit -= DestroyGameObject;
    }
  }
}