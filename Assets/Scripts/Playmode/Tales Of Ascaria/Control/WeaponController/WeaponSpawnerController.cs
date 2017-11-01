using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class WeaponSpawnerController : GameScript, IWeaponHandler
  {
    private Inventory inventory;

    private void Awake()
    {
      InjectDependencies("InjectWeaponSpawnerController");
    }

    private void InjectWeaponSpawnerController([EntityScope] Inventory inventory)
    {
      this.inventory = inventory;
    }

    #region ActionMethods

    public void PrimaryAction(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      float angleInDegree = Vector2.SignedAngle(Vector2.up, playerDirection);
      GameObject attackGameObject = Instantiate(inventory.GetWeaponPrefab(), playerPosition, Quaternion.Euler(0f,0f,angleInDegree));
      WeaponHitController weaponHitController = attackGameObject.GetComponentInChildren<WeaponHitController>();
      FollowTransform followTransform = attackGameObject.GetComponent<FollowTransform>();
      ProjectileController projectileController = attackGameObject.GetComponentInChildren<ProjectileController>();

      if (followTransform != null)
      {
        followTransform.target = transform.root;
      }

      if (projectileController != null)
      {
        projectileController.Direction = playerDirection;
      }

      weaponHitController.SetParameters(statsSnapshot,inventory.GetWeaponNumerics());

      weaponHitController.ExecuteAttack();
    }

    public void SecondaryAction(StatsSnapshot statsSnapshot, Vector2 playerDirection, Vector2 playerPosition)
    {
      switch (inventory.GetWeaponId())
      {
          
      }
    }

    #endregion
  }
}