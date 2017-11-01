using System.Collections;
using UnityEngine;

namespace TalesOfAscaria
{
  public abstract class PatrolState : GameState
  {
    protected const float mobStuckDelayInSeconds = 0.5f;
    protected const float destinationReachedDelayInSeconds = 1.5f;

    protected Transform patrolPoint;
    protected Vector2 lastPosition;

    protected float patrolRange;
    protected float mobStuckTimer;
    protected float destinationReachedTimer;

    protected bool mobIsStuck;
    protected bool destinationReached;

    public PatrolState(MobController mobController, Transform patrolPoint, float patrolRange) : base(mobController)
    {
      this.patrolRange = patrolRange;
      this.patrolPoint = patrolPoint;
      GenerateNewDestination();
    }

    protected void GenerateNewDestination()
    {
      UpdatePosition();
      do
      {
        destination = new Vector2(Random.Range(patrolPoint.position.x - patrolRange, patrolPoint.position.x + patrolRange),
                                  Random.Range(patrolPoint.position.y - patrolRange, patrolPoint.position.y + patrolRange));
      }
      while (Vector2.Distance(currentPosition, destination) < patrolRange / 3 * 2);
      CheckIfXSmallestDistance();
    }

    public override void Update()
    {
      base.Update();

      if (MobController.PlayersInRange.Count > 0)
      {
        InvokeOnNewState(GameStates.AttackingState);
        return;
      }

      if (!destinationReached && Vector2.Distance(currentPosition, destination) < proximityRadius)
      {
        destinationReached = true;
        destinationReachedTimer = Time.time;
      }
      else if (destinationReached && Time.time - destinationReachedTimer > destinationReachedDelayInSeconds)
      {
        destinationReached = false;
        GenerateNewDestination();
      }

      if (!mobIsStuck && !destinationReached && currentPosition == lastPosition)
      {
        mobIsStuck = true;
        mobStuckTimer = Time.time;
      }
      else if (mobIsStuck && Time.time - mobStuckTimer > mobStuckDelayInSeconds)
      {
        mobIsStuck = false;
        GenerateNewDestination();
      }
      else if (mobIsStuck && currentPosition != lastPosition)
      {
        mobIsStuck = false;
      }

      if (!destinationReached)
      {
        lastPosition = currentPosition;
        Move(true);
      }
    }

    protected override void Move(bool isPatrolling)
    {
      base.Move(isPatrolling);
    }

    protected override void CheckIfXSmallestDistance()
    {
      base.CheckIfXSmallestDistance();
    }
  }
}