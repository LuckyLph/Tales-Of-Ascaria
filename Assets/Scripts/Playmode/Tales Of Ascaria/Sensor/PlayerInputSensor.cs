using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/Input/PlayerInputSensor")]
  public class PlayerInputSensor : InputSensor
  {
    private KeyboardInputSensor keyboardInputSensor;
    private GamePadInputSensor gamePadInputSensor;

    private PlayersInputDevice playersInputDevice;

    public virtual IInputDevice Players
    {
      get { return playersInputDevice; }
    }

    public void InjectGameInputDevice([ApplicationScope] KeyboardInputSensor keyboardInputSensor,
                                      [ApplicationScope] GamePadInputSensor gamePadInputSensor)
    {
      this.keyboardInputSensor = keyboardInputSensor;
      this.gamePadInputSensor = gamePadInputSensor;
    }

    public void Awake()
    {
      InjectDependencies("InjectGameInputDevice");

      playersInputDevice = new PlayersInputDevice(keyboardInputSensor, gamePadInputSensor);
    }

    public void LateUpdate()
    {
      playersInputDevice.Reset();
    }

    private class PlayerInputDevice : TriggerOncePerFrameInputDevice
    {
      public PlayerInputDevice(KeyboardInputSensor keyboardInputSensor, GamePadInputSensor gamePadInputSensor)
          : this(keyboardInputSensor.Keyboards, gamePadInputSensor.GamePads)
      {
      }

      public PlayerInputDevice(IInputDevice keyboardInputDevice, IInputDevice gamePadInputDevice)
      {
        if (keyboardInputDevice != null)
        {
          keyboardInputDevice.OnUp += NotifyUp;
          keyboardInputDevice.OnDown += NotifyDown;
          keyboardInputDevice.OnLeft += NotifyLeft;
          keyboardInputDevice.OnRight += NotifyRight;
          keyboardInputDevice.OnConfirm += NotifyConfirm;
          keyboardInputDevice.OnTogglePause += NotifyTogglePause;
          keyboardInputDevice.OnNavigateLeft += NotifyNavigateLeft;
          keyboardInputDevice.OnNavigateRight += NotifyNavigateRight;
          keyboardInputDevice.OnMove += NotifyMove;
          keyboardInputDevice.OnRotate += NotifyRotate;
          keyboardInputDevice.OnSpellMode += NotifySpellMode;
          keyboardInputDevice.OnButtonYPressed += NotifyButtonYPressed;
          keyboardInputDevice.OnButtonAPressed += NotifyButtonAPressed;
          keyboardInputDevice.OnButtonXPressed += NotifyButtonXPressed;
          keyboardInputDevice.OnButtonBPressed += NotifyButtonBPressed;
          keyboardInputDevice.OnToggleMenu += NotifyToggleMenu;
          keyboardInputDevice.OnToggleMinimap += NotifyToggleMinimap;
          keyboardInputDevice.OnFirstConsummable += NotifyFirstConsummable;
          keyboardInputDevice.OnSecondConsummable += NotifySecondConsummable;
          keyboardInputDevice.OnThirdConsummable += NotifyThirdConsummable;
          keyboardInputDevice.OnFourthConsummable += NotifyFourthConsummable;
        }

        if (gamePadInputDevice != null)
        {
          gamePadInputDevice.OnUp += NotifyUp;
          gamePadInputDevice.OnDown += NotifyDown;
          gamePadInputDevice.OnLeft += NotifyLeft;
          gamePadInputDevice.OnRight += NotifyRight;
          gamePadInputDevice.OnConfirm += NotifyConfirm;
          gamePadInputDevice.OnTogglePause += NotifyTogglePause;
          gamePadInputDevice.OnNavigateLeft += NotifyNavigateLeft;
          gamePadInputDevice.OnNavigateRight += NotifyNavigateRight;
          gamePadInputDevice.OnMove += NotifyMove;
          gamePadInputDevice.OnRotate += NotifyRotate;
          gamePadInputDevice.OnSpellMode += NotifySpellMode;
          gamePadInputDevice.OnButtonYPressed += NotifyButtonYPressed;
          gamePadInputDevice.OnButtonAPressed += NotifyButtonAPressed;
          gamePadInputDevice.OnButtonXPressed += NotifyButtonXPressed;
          gamePadInputDevice.OnButtonBPressed += NotifyButtonBPressed;
          gamePadInputDevice.OnToggleMenu += NotifyToggleMenu;
          gamePadInputDevice.OnToggleMinimap += NotifyToggleMinimap;
          gamePadInputDevice.OnFirstConsummable += NotifyFirstConsummable;
          gamePadInputDevice.OnSecondConsummable += NotifySecondConsummable;
          gamePadInputDevice.OnThirdConsummable += NotifyThirdConsummable;
          gamePadInputDevice.OnFourthConsummable += NotifyFourthConsummable;
        }
      }

      public override IInputDevice this[int deviceIndex]
      {
        get { return this; }
      }
    }

    private class PlayersInputDevice : PlayerInputDevice
    {
      private readonly PlayerInputDevice[] players;

      public PlayersInputDevice(KeyboardInputSensor keyboardInputSensor, GamePadInputSensor gamePadInputSensor) :
          base(keyboardInputSensor, gamePadInputSensor)
      {
        players = new[]
        {
                    new PlayerInputDevice(keyboardInputSensor.Keyboards[0], gamePadInputSensor.GamePads[0]),
                    new PlayerInputDevice(keyboardInputSensor.Keyboards[1], gamePadInputSensor.GamePads[1]),
                    new PlayerInputDevice(keyboardInputSensor.Keyboards[2], gamePadInputSensor.GamePads[2]),
                    new PlayerInputDevice(keyboardInputSensor.Keyboards[3], gamePadInputSensor.GamePads[3])
                };
      }

      public override void Reset()
      {
        base.Reset();

        players[0].Reset();
        players[1].Reset();
        players[2].Reset();
        players[3].Reset();
      }

      public override IInputDevice this[int deviceIndex]
      {
        get { return players[deviceIndex]; }
      }
    }
  }
}