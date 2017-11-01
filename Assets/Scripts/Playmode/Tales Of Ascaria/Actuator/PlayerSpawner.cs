using UnityEngine;

namespace TalesOfAscaria
{
  public class PlayerSpawner : GameScript
  {
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject playerSpawnPoint;

    public virtual void Spawn()
    {
      GameObject player = Instantiate(playerPrefab,
                                      playerSpawnPoint.transform.position,
                                      Quaternion.Euler(Vector3.zero));

      Configure(player, playerSpawnPoint.transform.position);
    }

    private void Configure(GameObject player, Vector3 position)
    {
      player.transform.position = position;
      player.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
  }
}