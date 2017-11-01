using UnityEngine;
using Harmony;
using System.Collections;

namespace TalesOfAscaria
{
  public class MovePlayerOnInput : GameScript
  {
    private PlayersList playersList;
    private PlayerMovementEventPublisher playerMovementEventPublisher;
    private PlayerMovementValidator playerMovementValidator;
    private PlayerTranslateMover playerTranslateMover;
    private TranslateAnimator translateAnimator;
    private PlayerController playerController;
    private PlayerInput playerInput;
    private LivingEntity livingEntity;

    private Coroutine teleportCoroutine;
    private bool hasMoved;
    private bool timerIsStarted;
    private bool timerMustStart;
    private bool timerMustStop;

    private void InjectMovePlayerOnInput([ApplicationScope] PlayersList playersList,
                                         [EntityScope] PlayerMovementEventPublisher playerMovementEventPublisher,
                                         [EntityScope] PlayerMovementValidator playerMovementValidator,
                                         [EntityScope] PlayerTranslateMover playerTranslateMover,
                                         [EntityScope] TranslateAnimator translateAnimator,
                                         [GameObjectScope] PlayerController playerController,
                                         [GameObjectScope] PlayerInput playerInput,
                                         [GameObjectScope] LivingEntity livingEntity)
    {
      this.playersList = playersList;
      this.playerMovementEventPublisher = playerMovementEventPublisher;
      this.playerMovementValidator = playerMovementValidator;
      this.playerTranslateMover = playerTranslateMover;
      this.translateAnimator = translateAnimator;
      this.playerController = playerController;
      this.playerInput = playerInput;
      this.livingEntity = livingEntity;
    }

    private void Awake()
    {
      InjectDependencies("InjectMovePlayerOnInput");
    }

    private void OnEnable()
    {
      playerInput.OnMove += OnMove;
    }

    private void Update()
    {
      if (!timerIsStarted)
      {
        timerMustStart = playerMovementValidator.IsPlayerOutsideCamera(gameObject.transform.position, playerController.PlayerSize);
      }
      else if (!playerMovementValidator.IsPlayerOutsideCamera(gameObject.transform.position, playerController.PlayerSize))
      {
        timerMustStop = true;
      }

      if (timerMustStart && !timerIsStarted)
      {
        timerIsStarted = true;
        timerMustStart = false;
        timerMustStop = false;
        teleportCoroutine = StartCoroutine(TeleportAfterDelay());
      }
      else if (timerMustStop && timerIsStarted)
      {
        StopCoroutine(teleportCoroutine);
        timerMustStop = false;
        timerIsStarted = false;
      }
    }

    private void LateUpdate()
    {
      if (!hasMoved)
      {
        translateAnimator.SetAnimationState(R.S.AnimatorParameter.IsWalking, false);
      }
      hasMoved = false;
    }

    private void OnDisable()
    {
      playerInput.OnMove -= OnMove;
    }

    private void OnMove(Vector2 movementVector)
    {
      if (livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        playerController.Direction = movementVector;
        if (livingEntity.GetCrowdControl().SnareCounter <= 0)
        {
          hasMoved = true;
          float speed = livingEntity.GetStats().Agility;
          bool canMoveUp = playerMovementValidator.ValidateUpMovement(transform.position, movementVector,
                                                                      playerController.PlayerSize.y, speed);
          bool canMoveDown = playerMovementValidator.ValidateDownMovement(transform.position, movementVector,
                                                                          playerController.PlayerSize.y, speed);
          bool canMoveRight = playerMovementValidator.ValidateRightMovement(transform.position, movementVector,
                                                                            playerController.PlayerSize.x, speed);
          bool canMoveLeft = playerMovementValidator.ValidateLeftMovement(transform.position, movementVector,
                                                                          playerController.PlayerSize.x, speed);

          if (playerTranslateMover.Move(movementVector, speed, canMoveUp, canMoveDown, canMoveRight, canMoveLeft))
          {
            playerMovementEventPublisher.Publish(gameObject, movementVector);
          }
          translateAnimator.SetAnimationState(R.S.AnimatorParameter.IsWalking, true);
          translateAnimator.Animate(movementVector);
        }
        else
        {
          translateAnimator.SetAnimationState(R.S.AnimatorParameter.IsWalking, false);
          translateAnimator.Animate(movementVector);
        }
      }
    }

    private IEnumerator TeleportAfterDelay()
    {
      yield return new WaitForSeconds(2);
      GameObject[] players = playersList.PlayersAlive.ToArray();
      for (int i = 0; i < players.Length; i++)
      {
        if (!playerMovementValidator.IsPlayerOutsideCamera(players[i].transform.position,
             playersList.SpriteRenderers[i].bounds.size))
        {
          transform.parent.position = players[i].transform.position;
          break;
        }
      }
    }
  }
}