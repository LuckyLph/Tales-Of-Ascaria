using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public abstract class MobStateMachine : GameScript
  {
    [Tooltip("Distance in units from which the mob will go from its patrol point in patrol state")]
    [SerializeField]
    protected float patrolRange;

    [Tooltip("Distance in units from which the mob will go from its patrol point in attack state")]
    [SerializeField]
    protected float attackStateRange;

    [Tooltip("The Mob's primary attack range")]
    [SerializeField]
    protected float primaryAttackRange;

    [Tooltip("The mob's patrol point")]
    [SerializeField]
    protected Transform patrolPoint;

    protected MobController mobController;
    protected GameState currentState;

    private void InjectMobStateMachine([GameObjectScope] MobController mobController)
    {
      this.mobController = mobController;
    }

    protected virtual void Awake()
    {
      InjectDependencies("InjectMobStateMachine");
    }

    protected virtual void Update()
    {
      currentState.Update();
    }

    protected virtual void OnMobDeath()
    {
      OnNewState(GameStates.DeadState);
    }

    protected abstract void OnNewState(GameStates state);
  }
}