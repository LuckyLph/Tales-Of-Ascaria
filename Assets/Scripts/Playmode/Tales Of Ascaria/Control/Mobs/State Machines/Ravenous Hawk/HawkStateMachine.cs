using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public class HawkStateMachine : MobStateMachine 
	{

    protected override void Awake() 
		{
      base.Awake();
      currentState = new HawkPatrolState(mobController, patrolPoint, patrolRange);
		}

    protected void OnEnable()
    {
      currentState.OnNewState += OnNewState;
      mobController.OnMobDeath += OnMobDeath;
    }

    protected void OnDisable()
    {
      currentState.OnNewState -= OnNewState;
      mobController.OnMobDeath -= OnMobDeath;
    }

    protected override void Update()
    {
      base.Update();
    }

    protected override void OnMobDeath()
    {
      base.OnMobDeath();
    }

    protected override void OnNewState(GameStates state)
    {
      currentState.OnNewState -= OnNewState;
      switch (state)
      {
        case GameStates.IdleState:
          currentState = new HawkIdleState(mobController);
          break;

        case GameStates.DeadState:
          currentState = new HawkDeadState(mobController);
          break;

        case GameStates.PatrolState:
          currentState = new HawkPatrolState(mobController, patrolPoint, patrolRange);
          break;

        case GameStates.ReturningToPatrolState:
          currentState = new HawkReturnToPatrolState(mobController, patrolPoint);
          break;

        case GameStates.AttackingState:
          currentState = new HawkAttackingState(mobController, patrolPoint, attackStateRange, primaryAttackRange);
          break;
      }
      currentState.OnNewState += OnNewState;
    }
  }
}