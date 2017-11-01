using UnityEngine;

namespace TalesOfAscaria
{
    //Menu actions
    public delegate void UpEventHandler();

    public delegate void DownEventHandler();

    public delegate void LeftEventHandler();

    public delegate void RightEventHandler();

    public delegate void ConfirmEventHandler();

    public delegate void CancelEventHandler();

    public delegate void NavigateLeftEventHandler();

    public delegate void NavigateRightEventHAndler();

    //Game actions
    public delegate void TogglePauseEventHandler();

    public delegate void MoveEventHandler(Vector2 movementVector);

    public delegate void RotateEventHandler();

    public delegate void SpellModeEventHandler();

    public delegate void ButtonYPressedEventHandler();

    public delegate void ButtonXPressedEventHandler();

    public delegate void ButtonBPressedEventHandler();

    public delegate void ButtonAPressedEventHandler();

    public delegate void ToggleMenuEventHandler();

    public delegate void FirstConsummableEventHandler();

    public delegate void SecondConsummableEventHandler();

    public delegate void ThirdConsummableEventHandler();

    public delegate void FourthConsummableEventHandler();

    public delegate void ToggleMiniMapEventHandler();

    public interface IInputDevice
    {
        event UpEventHandler OnUp;
        event DownEventHandler OnDown;
        event LeftEventHandler OnLeft;
        event RightEventHandler OnRight;
        event ConfirmEventHandler OnConfirm;
        event TogglePauseEventHandler OnTogglePause;
        event NavigateLeftEventHandler OnNavigateLeft;
        event NavigateRightEventHAndler OnNavigateRight;
        event MoveEventHandler OnMove;
        event RotateEventHandler OnRotate;
        event SpellModeEventHandler OnSpellMode;
        event ButtonYPressedEventHandler OnButtonYPressed;
        event ButtonXPressedEventHandler OnButtonXPressed;
        event ButtonAPressedEventHandler OnButtonAPressed;
        event ButtonBPressedEventHandler OnButtonBPressed;
        event ToggleMenuEventHandler OnToggleMenu;
        event ToggleMiniMapEventHandler OnToggleMinimap;
        event FirstConsummableEventHandler OnFirstConsummable;
        event SecondConsummableEventHandler OnSecondConsummable;
        event ThirdConsummableEventHandler OnThirdConsummable;
        event FourthConsummableEventHandler OnFourthConsummable;

        IInputDevice this[int deviceIndex] { get; }
    }
}