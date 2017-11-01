using UnityEngine;

namespace TalesOfAscaria
{
  public delegate void InteractionSensorEventHandler(XInputDotNetPure.PlayerIndex index);

  [AddComponentMenu("Game/World/Object/Sensor/InteractionSensor")]
  public class InteractionSensor : GameScript
  {
    public event InteractionSensorEventHandler OnInteractionTrigger;

    public XInputDotNetPure.PlayerIndex PlayerIndex { get; private set; }
    public bool HasExitedInteraction { get; set; }

    public void ReceiveInteraction(XInputDotNetPure.PlayerIndex playerIndex)
    {
      PlayerIndex = playerIndex;
      if (OnInteractionTrigger != null) OnInteractionTrigger(PlayerIndex);
    }
  }
}
