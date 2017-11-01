using Harmony;

namespace TalesOfAscaria
{
  public class ActorActionApplier : GameScript
  {
    private LivingEntity entity;

    public void InjectActionApplier([SiblingsScope] LivingEntity entity)
    {
      this.entity = entity;
    }

    private void Awake()
    {
      InjectDependencies("InjectActionApplier");
    }


    public void OnHit(ActorAction actorAction)
    {
      actorAction.ApplyOn(entity);
    }
  }
}