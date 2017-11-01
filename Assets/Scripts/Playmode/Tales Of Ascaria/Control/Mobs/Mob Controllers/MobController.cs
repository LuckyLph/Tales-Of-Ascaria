using System.Collections.Generic;
using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public delegate void MobDeathHandler();

  [AddComponentMenu("Game/World/Object/Control/MobController")]
  public abstract class MobController : GameScript
  {
    public event MobDeathHandler OnMobDeath;

    public List<GameObject> PlayersInRange { get; private set; }
    public GameObject Target { get; set; }

    protected LivingEntity livingEntity;
    protected Health health;
    protected TranslateMover translateMover;
    protected TranslateAnimator translateAnimator;
    protected DirectionAnimator directionAnimator;
    protected PlayerSensor playerSensor;
    protected SpriteRenderer spriteRenderer;
    protected MobCore mobCore;
    protected Stats stats;

    protected EntityDestroyer entityDestroyer;

    protected Vector2 direction;
    protected Vector2 mobSize;

    protected bool hasMoved;

    private void InjectMobController([GameObjectScope] LivingEntity livingEntity,
                                     [GameObjectScope] Health health,
                                     [GameObjectScope] TranslateMover translateMover,
                                     [GameObjectScope] TranslateAnimator translateAnimator,
                                     [GameObjectScope] DirectionAnimator directionAnimator,
                                     [GameObjectScope] MobCore mobCore,
                                     [GameObjectScope] Stats stats,
                                     [SiblingsScope] PlayerSensor playerSensor,
                                     [SiblingsScope] SpriteRenderer spriteRenderer,
                                     [SiblingsScope] CreationEventPublisher creationEventPublisher,
                                     [SiblingsScope] DestroyEventPublisher destroyEventPublisher,
                                     [SiblingsScope] EntityDestroyer entityDestroyer)
    {
      this.livingEntity = livingEntity;
      this.health = health;
      this.translateMover = translateMover;
      this.translateAnimator = translateAnimator;
      this.directionAnimator = directionAnimator;
      this.playerSensor = playerSensor;
      this.spriteRenderer = spriteRenderer;
      this.mobCore = mobCore;
      this.entityDestroyer = entityDestroyer;
      this.stats = stats;
    }

    protected virtual void Awake()
    {
      InjectDependencies("InjectMobController");

      PlayersInRange = new List<GameObject>();
      mobSize = spriteRenderer.bounds.size;
    }

    protected virtual void OnEnable()
    {
      playerSensor.OnPlayerSensorEntered += OnPlayerSensorEntered;
      playerSensor.OnPlayerSensorExited += OnPlayerSensorExited;
      health.OnDeath += OnDeath;
    }

    protected virtual void OnDisable()
    {
      playerSensor.OnPlayerSensorEntered -= OnPlayerSensorEntered;
      playerSensor.OnPlayerSensorExited -= OnPlayerSensorExited;
      health.OnDeath -= OnDeath;
    }

    protected virtual void LateUpdate()
    {
      if (!hasMoved)
      {
        translateAnimator.SetAnimationState(R.S.AnimatorParameter.IsWalking, false);
      }
      hasMoved = false;
    }

    public virtual void Move(Vector2 movementVector, bool isPatrolling)
    {
      if (livingEntity.GetCrowdControl().StunCounter <= 0)
      {
        direction = movementVector;
        if (livingEntity.GetCrowdControl().SnareCounter <= 0)
        {
          hasMoved = true;
          movementVector = isPatrolling ? movementVector / 100 * 50 : movementVector;
          translateMover.Move(movementVector, livingEntity.GetStats().Agility);
          translateAnimator.SetAnimationState(R.S.AnimatorParameter.IsWalking, true);
        }
        else
        {
          translateAnimator.SetAnimationState(R.S.AnimatorParameter.IsWalking, false);  
        }
        translateAnimator.Animate(movementVector);
      }
    }

    public virtual void MobDeathComplete()
    {
      entityDestroyer.Destroy();
    }

    protected virtual void OnPlayerSensorEntered(GameObject player)
    {
      if (!PlayersInRange.Contains(player))
      {
        PlayersInRange.Add(player);
      }
    }

    protected virtual void OnPlayerSensorExited(GameObject player)
    {
      PlayersInRange.Remove(player);
    }

    protected void OnDeath()
    {
      if (OnMobDeath != null) OnMobDeath();
    }

    public abstract void Attack(MobAttackIndex attacksIndex, Vector2 directionToFace);
  }
}