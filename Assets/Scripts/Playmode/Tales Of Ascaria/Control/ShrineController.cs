using Harmony;
using UnityEngine;
namespace TalesOfAscaria
{
  public delegate void ShrineInteractionTriggeredHandler(XInputDotNetPure.PlayerIndex playerIndex);

  public class ShrineController : GameScript
  {
    public event ShrineInteractionTriggeredHandler OnShrineInteractionTriggered;

    public Transform[] PlayersRespawnPositions
    {
      get { return playersRespawnPositions; }
      private set { playersRespawnPositions = value; }
    }

    [Tooltip("Respawn position of all the players")]
    [SerializeField]
    private Transform[] playersRespawnPositions;

    private InteractionSensor interactionSensor;

    private void InjectShrineController([SiblingsScope] InteractionSensor interactionSensor)
    {
      this.interactionSensor = interactionSensor;
    }

    private void Awake()
    {
      InjectDependencies("InjectShrineController");
    }

    private void OnEnable()
    {
      interactionSensor.OnInteractionTrigger += OnPlayerSensorTriggered;
    }

    private void OnDisable()
    {
      interactionSensor.OnInteractionTrigger -= OnPlayerSensorTriggered;
    }
  
    private void OnPlayerSensorTriggered(XInputDotNetPure.PlayerIndex playerIndex)
    {
      if (OnShrineInteractionTriggered != null) OnShrineInteractionTriggered(playerIndex);
    }
  }
}