using Harmony;

namespace TalesOfAscaria
{
  public class LoseMoneyOnDeath : GameScript
  {
    private Inventory inventory;
    private Health health;

    private void InjectLoseMoneyOnDeath([GameObjectScope] Inventory inventory,
                                        [GameObjectScope] Health health)
    {
      this.inventory = inventory;
      this.health = health;
    }

    private void Awake()
    {
      InjectDependencies("InjectLoseMoneyOnDeath");
    }

    private void OnEnable()
    {
      health.OnDeath += LoseMoney;
    }

    private void OnDisable()
    {
      health.OnDeath -= LoseMoney;
    }

    private void LoseMoney()
    {
      inventory.RemoveMoney(inventory.Money / inventory.PercentageMoneyLostOnDeath);
    }
  }
}