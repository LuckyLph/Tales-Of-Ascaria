using Harmony;
using XInputDotNetPure;
using UnityEngine;

namespace TalesOfAscaria
{
  public delegate void PlayerDeathHandler(PlayerIndex index);

  [AddComponentMenu("Game/Aspect/PlayerInput")]
  public class PlayerInput : GameScript
  {
    public event PlayerDeathHandler OnPlayerDeath;

    public event MoveEventHandler OnMove
    {
      add
      {
        playerInputSensor.Players[(int)Index].OnMove += value;
      }
      remove
      {
        playerInputSensor.Players[(int)Index].OnMove -= value;
      }
    }

    public event SpellModeEventHandler OnSpellMode
    {
      add
      {
        playerInputSensor.Players[(int)Index].OnSpellMode += value;
      }
      remove
      {
        playerInputSensor.Players[(int)Index].OnSpellMode -= value;
      }
    }

    public event ButtonYPressedEventHandler OnButtonYPressed
    {
      add
      {
        playerInputSensor.Players[(int)Index].OnButtonYPressed += value;
      }
      remove
      {
        playerInputSensor.Players[(int)Index].OnButtonYPressed -= value;
      }
    }

    public event ButtonXPressedEventHandler OnButtonXPressed
    {
      add
      {
        playerInputSensor.Players[(int)Index].OnButtonXPressed += value;
      }
      remove
      {
        playerInputSensor.Players[(int)Index].OnButtonXPressed -= value;
      }
    }

    public event ButtonAPressedEventHandler OnButtonAPressed
    {
      add
      {
        playerInputSensor.Players[(int)Index].OnButtonAPressed += value;
      }
      remove
      {
        playerInputSensor.Players[(int)Index].OnButtonAPressed -= value;
      }
    }

    public event ButtonBPressedEventHandler OnButtonBPressed
    {
      add
      {
        playerInputSensor.Players[(int)Index].OnButtonBPressed += value;
      }
      remove
      {
        playerInputSensor.Players[(int)Index].OnButtonBPressed -= value;
      }
    }

    [Tooltip("The index of the player")]
    [SerializeField]
    private PlayerIndex index;

    public PlayerIndex Index
    {
      get { return index; }
      set { index = value; }
    }

    private PlayerInputSensor playerInputSensor;
    private Health health;

    private void InjectPlayerInput([ApplicationScope] PlayerInputSensor playerInputSensor,
                                   [GameObjectScope] Health health)
    {
      this.playerInputSensor = playerInputSensor;
      this.health = health;
    }

    private void Awake()
    {
      InjectDependencies("InjectPlayerInput");
    }

    private void OnEnable()
    {
      health.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
      health.OnDeath -= OnDeath;
    }

    private void OnDeath()
    {
      if (OnPlayerDeath != null) OnPlayerDeath(index);
    }
  }
}