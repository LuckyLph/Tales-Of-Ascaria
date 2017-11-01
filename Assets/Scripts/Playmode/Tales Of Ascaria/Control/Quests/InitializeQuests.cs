using Harmony;

namespace TalesOfAscaria
{
  public class InitializeQuests : GameScript
  {
    private QuestManagerController questManagerController;

    private void InjectInitializeQuests([ApplicationScope] QuestManagerController questManagerController)
    {
      this.questManagerController = questManagerController;
    }

    private void Awake()
    {
      InjectDependencies("InjectInitializeQuests");
    }

    private void Start()
    {
    }
  }
}