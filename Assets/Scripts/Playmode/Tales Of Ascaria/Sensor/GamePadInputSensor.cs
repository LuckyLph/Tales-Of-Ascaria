using Harmony;
using UnityEngine;
using XInputDotNetPure;
using GamePad = Harmony.GamePad;
using GamePadState = Harmony.GamePadState;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/Input/GamePadInputSensor")]
  public class GamePadInputSensor : InputSensor
  {
    private GamePad gamePadInput;

    private GamePadsInputDevice gamePadsInputDevice;

    public virtual IInputDevice GamePads
    {
      get { return gamePadsInputDevice; }
    }

    public void InjectGamePadInputDevice([ApplicationScope] GamePad gamePadInput)
    {
      this.gamePadInput = gamePadInput;
    }

    public void Awake()
    {
      InjectDependencies("InjectGamePadInputDevice");

      gamePadsInputDevice = new GamePadsInputDevice(gamePadInput);
    }

    public void Update()
    {
      gamePadsInputDevice.Update();
    }

    public IInputDevice GetGamePad(int playerIndex)
    {
      return gamePadsInputDevice[playerIndex];
    }

    /* probably useless: never called, causes compilation errors
    public GamePadState GetGamepadState()
    {
      return GetGamepadState(PlayerIndex.One) +
             GetGamepadState(PlayerIndex.Two) +
             GetGamepadState(PlayerIndex.Three) +
             GetGamepadState(PlayerIndex.Four);
    }

    public GamePadState GetGamepadState(PlayerIndex playerIndex)
    {
      return GamePad.GetState(playerIndex);
    }
    */

    private class GamePadInputDevice : InputDevice
    {
      private readonly GamePad gamePadInput;
      private readonly PlayerIndex playerIndex;
      private readonly bool isAllPlayers;

      private GamePadState currentFrameGamePadState;
      private GamePadState previousFrameGamePadState;

      public GamePadInputDevice(GamePad gamePadInput) : this(gamePadInput, PlayerIndex.One, true)
      {
      }

      public GamePadInputDevice(GamePad gamePadInput, PlayerIndex playerIndex) : this(gamePadInput, playerIndex, false)
      {
      }

      private GamePadInputDevice(GamePad gamePadInput, PlayerIndex playerIndex, bool isAllPlayers)
      {
        this.gamePadInput = gamePadInput;
        this.playerIndex = playerIndex;
        this.isAllPlayers = isAllPlayers;

        previousFrameGamePadState = GetCurrentGamePadState();
      }

      public override IInputDevice this[int deviceIndex]
      {
        get { return this; }
      }

      public virtual void Update()
      {
        currentFrameGamePadState = GetCurrentGamePadState();

        HandleUiInput();
        HandleActionInput();
        HandleDirectionInput();
        HandleRotationInput();

        previousFrameGamePadState = currentFrameGamePadState;
      }

      private void HandleUiInput()
      {
        if (IsPressedSinceLastFrame(previousFrameGamePadState.DPad.Up, currentFrameGamePadState.DPad.Up))
        {
          NotifyUp();
        }
        if (IsPressedSinceLastFrame(previousFrameGamePadState.DPad.Down, currentFrameGamePadState.DPad.Down))
        {
          NotifyDown();
        }
        if (IsPressedSinceLastFrame(previousFrameGamePadState.DPad.Right, currentFrameGamePadState.DPad.Right))
        {
          NotifyRight();
        }
        if (IsPressedSinceLastFrame(previousFrameGamePadState.DPad.Left, currentFrameGamePadState.DPad.Left))
        {
          NotifyLeft();
        }
        if (IsPressedSinceLastFrame(previousFrameGamePadState.Buttons.A, currentFrameGamePadState.Buttons.A))
        {
          NotifyConfirm();
        }
        if (IsPressedSinceLastFrame(previousFrameGamePadState.Buttons.LeftShoulder, currentFrameGamePadState.Buttons.LeftShoulder))
        {
          NotifyToggleMenu();
        }
      }

      private void HandleActionInput()
      {
        if (IsPressedSinceLastFrame(previousFrameGamePadState.Buttons.Start, currentFrameGamePadState.Buttons.Start))
        {
          NotifyTogglePause();
        }
        if (IsPressedSinceLastFrame(previousFrameGamePadState.Buttons.B, currentFrameGamePadState.Buttons.B))
        {
          NotifyButtonBPressed();
        }
        if (IsPressedSinceLastFrame(previousFrameGamePadState.Buttons.X, currentFrameGamePadState.Buttons.X))
        {
          NotifyButtonXPressed();
        }
        if (IsPressedSinceLastFrame(previousFrameGamePadState.Buttons.A, currentFrameGamePadState.Buttons.A))
        {
          NotifyButtonAPressed();
        }
        if (IsPressedSinceLastFrame(previousFrameGamePadState.Buttons.Y, currentFrameGamePadState.Buttons.Y))
        {
          NotifyButtonYPressed();
        }
        if (IsPressed(currentFrameGamePadState.Buttons.RightShoulder))
        {
          NotifySpellMode();
        }
      }

      private void HandleDirectionInput()
      {
        Vector2 leftStickAxis = GetLeftThumbstickAxis();
        if (leftStickAxis != Vector2.zero)
        {
          NotifyMove(leftStickAxis);
        }
      }

      private void HandleRotationInput()
      {
        //if (IsPressed(currentFrameGamePadState.DPad.Left))
        //{
        //  NotifyRotateLeft();
        //}
        //if (IsPressed(currentFrameGamePadState.DPad.Right))
        //{
        //  NotifyRotateRight();
        //}
      }

      private bool IsPressed(ButtonState currentState)
      {
        return currentState == ButtonState.Pressed;
      }

      private bool IsPressedSinceLastFrame(ButtonState previousState, ButtonState currentState)
      {
        return previousState == ButtonState.Released && currentState == ButtonState.Pressed;
      }

      private GamePadState GetCurrentGamePadState()
      {
        return isAllPlayers ? gamePadInput.GetGamepadState(GamePadDeadZone.Circular) : gamePadInput.GetGamepadState(playerIndex, GamePadDeadZone.Circular);
      }

      private Vector2 GetLeftThumbstickAxis()
      {
        return new Vector2(currentFrameGamePadState.ThumbSticks.Left.X, currentFrameGamePadState.ThumbSticks.Left.Y);
      }

      private Vector2 GetRightThumbstickAxis()
      {
        return new Vector2(currentFrameGamePadState.ThumbSticks.Right.X, currentFrameGamePadState.ThumbSticks.Right.Y);
      }
    }

    private class GamePadsInputDevice : GamePadInputDevice
    {
      private readonly GamePadInputDevice[] gamePads;

      public GamePadsInputDevice(GamePad gamePadInput) : base(gamePadInput)
      {
        gamePads = new[]
        {
                    new GamePadInputDevice(gamePadInput, PlayerIndex.One),
                    new GamePadInputDevice(gamePadInput, PlayerIndex.Two),
                    new GamePadInputDevice(gamePadInput, PlayerIndex.Three),
                    new GamePadInputDevice(gamePadInput, PlayerIndex.Four)
                };
      }

      public override IInputDevice this[int deviceIndex]
      {
        get { return gamePads[deviceIndex]; }
      }

      public override void Update()
      {
        base.Update();

        foreach (GamePadInputDevice gamePadInputDevice in gamePads)
        {
          gamePadInputDevice.Update();
        }
      }
    }
  }
}