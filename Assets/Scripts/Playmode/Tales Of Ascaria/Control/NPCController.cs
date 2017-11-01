using Harmony;

namespace TalesOfAscaria
{
  public delegate void OnInteractionSensorTriggeredHandler(InteractionObjective.InteractionObjectives interactorIndex,
                                                           XInputDotNetPure.PlayerIndex index);

  public class NPCController : GameScript
  {
    public event OnInteractionSensorTriggeredHandler OnInteractionSensorTriggered;

    private InteractionSensor interactionSensor;
    private Interactor interactor;

    public void InjectNPCController([SiblingsScope] InteractionSensor interactionSensor,
                                    [GameObjectScope] Interactor interactor)
    {
      this.interactionSensor = interactionSensor;
      this.interactor = interactor;
    }

    private void Awake()
    {
      InjectDependencies("InjectNPCController");
    }

    private void Start()
    {
      interactionSensor.OnInteractionTrigger += OnSensorTriggered;
    }

    private void OnDestroy()
    {
      interactionSensor.OnInteractionTrigger -= OnSensorTriggered;
    }

    private void OnSensorTriggered(XInputDotNetPure.PlayerIndex PlayerIndex)
    {
      if (OnInteractionSensorTriggered != null) OnInteractionSensorTriggered(interactor.ID, PlayerIndex);
    }
  }
}
