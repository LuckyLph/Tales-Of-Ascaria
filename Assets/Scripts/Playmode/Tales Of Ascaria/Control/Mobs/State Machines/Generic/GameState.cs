using UnityEngine;

namespace TalesOfAscaria
{
  public delegate void OnNewStateHandler(GameStates state);

  public abstract class GameState
  {
    protected const float proximityRadius = 0.4f;
    protected const float singleAxisProximityRadius = 0.2f;

    public event OnNewStateHandler OnNewState;

    public MobController MobController { get; private set; }

    protected Vector2 currentPosition;
    protected Vector2 destination;
    protected bool xSmallestDistance;

    public GameState(MobController mobController)
    {
      MobController = mobController;
    }

    public virtual void Update()
    {
      UpdatePosition();
    }

    protected void UpdatePosition()
    {
      currentPosition = MobController.transform.parent.transform.position;
    }

    protected virtual void Move(bool isPatrolling)
    {
      Vector2 distance = destination - currentPosition;
      Vector2 movementVector = new Vector2();

      if (xSmallestDistance && Mathf.Abs(distance.x) < singleAxisProximityRadius)
      {
        xSmallestDistance = false;
      }
      if (!xSmallestDistance && Mathf.Abs(distance.y) < singleAxisProximityRadius)
      {
        xSmallestDistance = true;
      }

      if (xSmallestDistance)
      {
        movementVector = new Vector2(distance.x, 0);
      }
      else
      {
        movementVector = new Vector2(0, distance.y);
      }

      MobController.Move(movementVector.normalized, isPatrolling);
    }

    protected virtual void CheckIfXSmallestDistance()
    {
      Vector2 distance = destination - currentPosition;
      xSmallestDistance =  distance.x < distance.y;
    }

    protected void InvokeOnNewState(GameStates state)
    {
      if (OnNewState != null) OnNewState(state);
    }
  }
}