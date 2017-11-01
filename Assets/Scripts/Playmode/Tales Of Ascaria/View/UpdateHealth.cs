using UnityEngine;
using UnityEngine.UI;
using Harmony;

namespace TalesOfAscaria
{
  public class UpdateHealth : GameScript
  {
    private Text text;
    private LivingEntity livingEntity;

    private void InjectUpdateHealth([ChildrensScope] Text text,
                                    [SiblingsScope] LivingEntity livingEntity)
    {
      this.livingEntity = livingEntity;
      this.text = text;
    }

    private void Awake()
    {
      InjectDependencies("InjectUpdateHealth");
    }

    private void Update()
    {
      text.text = livingEntity.GetHealth().HealthPoints.ToString();
    }
  }
}