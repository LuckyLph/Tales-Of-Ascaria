using UnityEngine;

namespace TalesOfAscaria
{
  public class ExplorationNode : GameScript
  {
    public Maps Map
    {
      get { return map; }
      private set { map = value; }
    }

    public ExplorationObjective.ExplorationObjectives ID
    {
      get { return id; }
      set { id = value; }
    }

    [SerializeField]
    private Maps map;

    [SerializeField]
    private ExplorationObjective.ExplorationObjectives id;
  }
}