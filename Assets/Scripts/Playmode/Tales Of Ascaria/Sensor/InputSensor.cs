using UnityEngine;

namespace TalesOfAscaria
{
    public abstract class InputSensor : GameScript
    {
        protected abstract class InputDevice : IInputDevice
        {
            public event UpEventHandler OnUp;
            public event DownEventHandler OnDown;
            public event ConfirmEventHandler OnConfirm;
            public event TogglePauseEventHandler OnTogglePause;
            public event LeftEventHandler OnLeft;
            public event RightEventHandler OnRight;
            public event NavigateLeftEventHandler OnNavigateLeft;
            public event NavigateRightEventHAndler OnNavigateRight;
            public event MoveEventHandler OnMove;
            public event RotateEventHandler OnRotate;
            public event SpellModeEventHandler OnSpellMode;
            public event ButtonYPressedEventHandler OnButtonYPressed;
            public event ButtonXPressedEventHandler OnButtonXPressed;
            public event ButtonAPressedEventHandler OnButtonAPressed;
            public event ButtonBPressedEventHandler OnButtonBPressed;
            public event ToggleMenuEventHandler OnToggleMenu;
            public event ToggleMiniMapEventHandler OnToggleMinimap;
            public event FirstConsummableEventHandler OnFirstConsummable;
            public event SecondConsummableEventHandler OnSecondConsummable;
            public event ThirdConsummableEventHandler OnThirdConsummable;
            public event FourthConsummableEventHandler OnFourthConsummable;


            public abstract IInputDevice this[int deviceIndex] { get; }

            protected virtual void NotifyUp()
            {
                if (OnUp != null) OnUp();
            }

            protected virtual void NotifyDown()
            {
                if (OnDown != null) OnDown();
            }

            protected virtual void NotifyLeft()
            {
                if (OnLeft != null) OnLeft();
            }

            protected virtual void NotifyRight()
            {
                if (OnRight != null) OnRight();
            }

            protected virtual void NotifyConfirm()
            {
                if (OnConfirm != null) OnConfirm();
            }

            protected virtual void NotifyTogglePause()
            {
                if (OnTogglePause != null) OnTogglePause();
            }

            protected virtual void NotifyNavigateLeft()
            {
                if (OnNavigateLeft != null) OnNavigateLeft();
            }

            protected virtual void NotifyNavigateRight()
            {
                if (OnNavigateRight != null) OnNavigateRight();
            }

            protected virtual void NotifyMove(Vector2 movementVector)
            {
                if (OnMove != null) OnMove(movementVector);
            }

            protected virtual void NotifyRotate()
            {
                if (OnRotate != null) OnRotate();
            }

            protected virtual void NotifySpellMode()
            {
                if (OnSpellMode != null) OnSpellMode();
            }

            protected virtual void NotifyButtonYPressed()
            {
                if (OnButtonYPressed != null) OnButtonYPressed();
            }

            protected virtual void NotifyButtonXPressed()
            {
                if (OnButtonXPressed != null) OnButtonXPressed();
            }

            protected virtual void NotifyButtonAPressed()
            {
                if (OnButtonAPressed != null) OnButtonAPressed();
            }

            protected virtual void NotifyButtonBPressed()
            {
                if (OnButtonBPressed != null) OnButtonBPressed();
            }

            protected virtual void NotifyToggleMenu()
            {
                if (OnToggleMenu != null) OnToggleMenu();
            }

            protected virtual void NotifyToggleMinimap()
            {
                if (OnToggleMinimap != null) OnToggleMinimap();
            }

            protected virtual void NotifyFirstConsummable()
            {
                if (OnFirstConsummable != null) OnFirstConsummable();
            }

            protected virtual void NotifySecondConsummable()
            {
                if (OnSecondConsummable != null) OnSecondConsummable();
            }

            protected virtual void NotifyThirdConsummable()
            {
                if (OnThirdConsummable != null) OnThirdConsummable();
            }

            protected virtual void NotifyFourthConsummable()
            {
                if (OnFourthConsummable != null) OnFourthConsummable();
            }
        }

        protected abstract class TriggerOncePerFrameInputDevice : InputDevice
        {
            bool upTriggered;
            bool DownTriggered;
            bool confirmTriggered;
            bool togglePauseTriggered;
            bool leftTriggered;
            bool rightTriggered;
            bool navigateLeftTriggered;
            bool navigateRightTriggered;
            bool moveTriggered;
            bool rotateTriggered;
            bool spellModeTriggered;
            bool buttonYTriggered;
            bool buttonXTriggered;
            bool buttonATriggered;
            bool buttonBTriggered;
            bool toggleMenuTriggered;
            bool toggleMinimapTriggered;
            bool firstConsummableTriggered;
            bool secondConsummableTriggered;
            bool thirdConsummableTriggered;
            bool fourthConsummableTriggered;

            public abstract override IInputDevice this[int deviceIndex] { get; }

            public virtual void Reset()
            {
                upTriggered = false;
                DownTriggered = false;
                confirmTriggered = false;
                togglePauseTriggered = false;
                leftTriggered = false;
                rightTriggered = false;
                navigateLeftTriggered = false;
                navigateRightTriggered = false;
                moveTriggered = false;
                rotateTriggered = false;
                spellModeTriggered = false;
                buttonYTriggered = false;
                buttonXTriggered = false;
                buttonATriggered = false;
                buttonBTriggered = false;
                toggleMenuTriggered = false;
                toggleMinimapTriggered = false;
                firstConsummableTriggered = false;
                secondConsummableTriggered = false;
                thirdConsummableTriggered = false;
                fourthConsummableTriggered = false;
            }

            protected override void NotifyUp()
            {
                if (!upTriggered)
                {
                    base.NotifyUp();
                    upTriggered = true;
                }
            }

            protected override void NotifyLeft()
            {
                if (!leftTriggered)
                {
                    base.NotifyLeft();
                    leftTriggered = true;
                }
            }

            protected override void NotifyRight()
            {
                if (!rightTriggered)
                {
                    base.NotifyRight();
                    rightTriggered = true;
                }
            }

            protected override void NotifyNavigateLeft()
            {
                if (!navigateLeftTriggered)
                {
                    base.NotifyNavigateLeft();
                    navigateLeftTriggered = true;
                }
            }

            protected override void NotifyNavigateRight()
            {
                if (!navigateRightTriggered)
                {
                    base.NotifyNavigateRight();
                    navigateRightTriggered = true;
                }
            }

            protected override void NotifyMove(Vector2 movementVector)
            {
                if (!moveTriggered)
                {
                    base.NotifyMove(movementVector);
                    moveTriggered = true;
                }
            }

            protected override void NotifyRotate()
            {
                if (!rotateTriggered)
                {
                    base.NotifyRotate();
                    rotateTriggered = true;
                }
            }

            //Since spell mode requires to HOLD the button, we should fire the event non-stop to record the input
            //Properly (AGagne)
            /*protected override void NotifySpellMode()
            {
              if (!spellModeTriggered)
              {
                base.NotifySpellMode();
                spellModeTriggered = true;
              }
            }
            */

            protected override void NotifyButtonYPressed()
            {
                if (!buttonYTriggered)
                {
                    base.NotifyButtonYPressed();
                    buttonYTriggered = true;
                }
            }

            protected override void NotifyButtonXPressed()
            {
                if (!buttonXTriggered)
                {
                    base.NotifyButtonXPressed();
                    buttonXTriggered = true;
                }
            }

            protected override void NotifyButtonAPressed()
            {
                if (!buttonATriggered)
                {
                    base.NotifyButtonAPressed();
                    buttonATriggered = true;
                }
            }

            protected override void NotifyButtonBPressed()
            {
                if (!buttonBTriggered)
                {
                    base.NotifyButtonBPressed();
                    buttonBTriggered = true;
                }
            }

            protected override void NotifyToggleMenu()
            {
                if (!toggleMenuTriggered)
                {
                    base.NotifyToggleMenu();
                    toggleMenuTriggered = true;
                }
            }

            protected override void NotifyToggleMinimap()
            {
                if (!toggleMenuTriggered)
                {
                    base.NotifyToggleMinimap();
                    toggleMenuTriggered = true;
                }
            }

            protected override void NotifyFirstConsummable()
            {
                if (!firstConsummableTriggered)
                {
                    base.NotifyFirstConsummable();
                    firstConsummableTriggered = true;
                }
            }

            protected override void NotifySecondConsummable()
            {
                if (!buttonXTriggered)
                {
                    base.NotifySecondConsummable();
                    buttonXTriggered = true;
                }
            }

            protected override void NotifyThirdConsummable()
            {
                if (!thirdConsummableTriggered)
                {
                    base.NotifyThirdConsummable();
                    thirdConsummableTriggered = true;
                }
            }

            protected override void NotifyFourthConsummable()
            {
                if (!fourthConsummableTriggered)
                {
                    base.NotifyFourthConsummable();
                    fourthConsummableTriggered = true;
                }
            }
        }
    }
}