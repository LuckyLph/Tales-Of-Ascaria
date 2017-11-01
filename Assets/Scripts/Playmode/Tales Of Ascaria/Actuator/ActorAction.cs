using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace TalesOfAscaria
{
  public class ActorAction
  {
    private List<Effect> effects;


    public ActorAction(List<Effect> effects)
    {
      this.effects = effects;
    }


    public void ApplyOn(LivingEntity entity)
    {
      foreach (Effect effect in effects)
      {
        if (effect is StatsModifier)
        {
          StatsModifier modifier = effect as StatsModifier;
          Debug.Log("Applying : " + modifier.SpiritModifier);
        }
        effect.ApplyOn(entity);
      }
    }
  }
}