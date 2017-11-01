using Harmony;
using UnityEngine;

namespace TalesOfAscaria
{
  [AddComponentMenu("Game/Input/KeyboardInputSensor")]
  public class KeyboardInputSensor : InputSensor
  {
    public const float DiagonalMovementRatio = 1.41f;

    private Keyboard keyboard;

    private KeyboardsInputDevice keyboardsInputDevice;

    public virtual IInputDevice Keyboards
    {
      get { return keyboardsInputDevice; }
    }

    public void InjectKeyboardInputDevice([ApplicationScope] Keyboard keyboard)
    {
      this.keyboard = keyboard;
    }

    public void Awake()
    {
      InjectDependencies("InjectKeyboardInputDevice");

      keyboardsInputDevice = new KeyboardsInputDevice(keyboard);
    }

    public void Update()
    {
      keyboardsInputDevice.Update();
    }

    private class KeyboardsInputDevice : InputDevice
    {
      private readonly Keyboard keyboard;

      public KeyboardsInputDevice(Keyboard keyboard)
      {
        this.keyboard = keyboard;
      }

      public void Update()
      {
        HandleUiInput();
        HandleActionInput();
        HandleDirectionInput();
        HandleRotationInput();
      }

      public override IInputDevice this[int deviceIndex]
      {
        get { return this; }
      }

      private void HandleUiInput()
      {
        //if (keyboard.GetKeyDown(KeyCode.UpArrow))
        //{
        //  NotifyUp();
        //}
        //if (keyboard.GetKeyDown(KeyCode.DownArrow))
        //{
        //  NotifyDown();
        //}
        //if (keyboard.GetKeyDown(KeyCode.Return))
        //{
        //  NotifyConfirm();
        //}
      }

      private void HandleActionInput()
      {
        if (keyboard.GetKeyDown(KeyCode.E))
        {
          NotifyButtonBPressed();
        }
        if (keyboard.GetKey(KeyCode.LeftShift))
        {
          NotifySpellMode();
        }
      }

      private void HandleDirectionInput()
      {
        Vector2 movementVector = Vector2.zero;
        bool keyA = keyboard.GetKey(KeyCode.A);
        bool keyD = keyboard.GetKey(KeyCode.D);
        bool keyS = keyboard.GetKey(KeyCode.S);
        bool keyW = keyboard.GetKey(KeyCode.W);

        if (keyW && keyA)
        {
          if (!keyD)
            movementVector.x -= 1 / DiagonalMovementRatio;
          if (!keyS)
            movementVector.y += 1 / DiagonalMovementRatio;
        }
        else if (keyS && keyA)
        {
          if (!keyD)
            movementVector.x -= 1 / DiagonalMovementRatio;
          if (!keyW)
            movementVector.y -= 1 / DiagonalMovementRatio;
        }
        else if (keyW && keyD)
        {
          if (!keyA)
            movementVector.x += 1 / DiagonalMovementRatio;
          if (!keyS)
            movementVector.y += 1 / DiagonalMovementRatio;
        }
        else if (keyS && keyD)
        {
          if (!keyA)
            movementVector.x += 1 / DiagonalMovementRatio;
          if (!keyW)
            movementVector.y -= 1 / DiagonalMovementRatio;
        }
        else if (keyA && !keyD)
        {
          movementVector.x -= 1;
        }
        else if (keyD && !keyA)
        {
          movementVector.x += 1;
        }
        else if (keyW && !keyS)
        {
          movementVector.y += 1;
        }
        else if (keyS && !keyW)
        {
          movementVector.y -= 1;
        }

        if (movementVector != Vector2.zero)
        {
          NotifyMove(movementVector);
        }
      }

      private void HandleRotationInput()
      {
        //if (keyboard.GetKey(KeyCode.LeftArrow))
        //{
        //  NotifyRotateLeft();
        //}
        //if (keyboard.GetKey(KeyCode.RightArrow))
        //{
        //  NotifyRotateRight();
        //}
      }
    }
  }
}
