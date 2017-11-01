using UnityEngine;

namespace TalesOfAscaria
{
  [CreateAssetMenu(fileName = "New Taunt Effect", menuName = "Game/Effect/Taunt")]
  public class Taunt : Effect
  {
    //private MobController mobController;

    [Tooltip("Temps que l'effet provoke persiste sur une entité")]
    [SerializeField]
    private float provokeDuration;

    private void Start()
    {

    }

    public override void ApplyOn(LivingEntity entity)
    {
      //TODO: faire que l'ennemi target le caster et lock le target
    }
  }
}