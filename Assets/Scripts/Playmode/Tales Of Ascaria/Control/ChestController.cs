using Harmony;

namespace TalesOfAscaria
{
  public delegate void OnChestInteractionTriggeredHandler(XInputDotNetPure.PlayerIndex playerIndex);

  public class ChestController : GameScript
  {
    public event OnChestInteractionTriggeredHandler OnChestInteractionSensorTriggered;

    public XInputDotNetPure.PlayerIndex PlayerIndex { get; private set; }
    public bool IsOpen { get; set; }

    private InteractionSensor interactionSensor;

    private void InjectChestController([SiblingsScope] InteractionSensor interactionSensor)
    {
      this.interactionSensor = interactionSensor;
    }

    private void Awake()
    {
      InjectDependencies("InjectChestController");
    }

    private void OnEnable()
    {
      interactionSensor.OnInteractionTrigger += OnSensorTriggered;
    }

    private void OnDisable()
    {
      interactionSensor.OnInteractionTrigger -= OnSensorTriggered;
    }

    private void OnSensorTriggered(XInputDotNetPure.PlayerIndex playerIndex)
    {
      if (OnChestInteractionSensorTriggered != null) OnChestInteractionSensorTriggered(playerIndex);
    }
  }
}