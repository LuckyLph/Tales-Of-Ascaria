using UnityEngine;

namespace TalesOfAscaria
{
  public class Interactor : GameScript
  {
    public Maps Map
    {
      get { return map; }
      private set { map = value; }
    }

    public InteractionObjective.InteractionObjectives ID
    {
      get { return id; }
      set { id = value; }
    }

    [SerializeField]
    private InteractionObjective.InteractionObjectives id;

    [SerializeField]
    private Maps map;
  }
}