using Harmony;

namespace TalesOfAscaria
{
  public class ResetPlayerOnRespawn : GameScript
  {
    private Health health;
    private LivingEntity livingEntity;
    private RespawnPlayerOnInput respawnPlayerOnInput;

    private void InjectResetPlayerOnRespawn([GameObjectScope] Health health,
                                            [GameObjectScope] LivingEntity livingEntity,
                                            [GameObjectScope] RespawnPlayerOnInput respawnPlayerOnInput)
    {
      this.health = health;
      this.livingEntity = livingEntity;
      this.respawnPlayerOnInput = respawnPlayerOnInput;
    }

    private void Awake()
    {
      InjectDependencies("InjectResetPlayerOnRespawn");
    }

    public void ResetPlayer()
    {
      transform.root.gameObject.SetActive(true);
      health.RegainHealth();
      livingEntity.GetCrowdControl().ResetCrowdControlCount();
      transform.root.transform.position = respawnPlayerOnInput.RespawnPosition.position;
    }
  }
}