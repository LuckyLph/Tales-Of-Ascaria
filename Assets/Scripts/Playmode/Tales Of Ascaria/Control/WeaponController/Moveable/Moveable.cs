using UnityEngine;
using Harmony;

namespace TalesOfAscaria 
{
	public abstract class Moveable : GameScript
	{
	  public delegate void MoveEventHandler();

	  [Tooltip("Le temps que prendra le mouvement")] [SerializeField] protected float timeTaken;

    public event MoveEventHandler OnMoveStart;
	  public event MoveEventHandler OnMoveEnd;

	  protected void TriggerStart()
	  {
	    if (OnMoveStart != null) OnMoveStart();
    }

	  protected void TriggerEnd()
	  {
	    if (OnMoveEnd != null) OnMoveEnd();
	  }
    public abstract void ExecuteMove ();
	}
}