using UnityEngine;

namespace TalesOfAscaria
{
  public delegate void PlayerSensorEnteredHandler(GameObject player);

  public delegate void PlayerSensorExitedHandler(GameObject player);

  public class PlayerSensor : GameScript
  {
    public event PlayerSensorEnteredHandler OnPlayerSensorEntered;
    public event PlayerSensorExitedHandler OnPlayerSensorExited;

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (OnPlayerSensorEntered != null) OnPlayerSensorEntered(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
      if (OnPlayerSensorExited != null) OnPlayerSensorExited(collision.gameObject);
    }
  }
}