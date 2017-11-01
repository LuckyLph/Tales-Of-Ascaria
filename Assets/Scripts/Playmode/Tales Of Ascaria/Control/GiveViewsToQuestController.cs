using Harmony;

namespace TalesOfAscaria
{
  public class GiveViewsToQuestController : GameScript
  {
    private QuestManagerController questManagerController;
    private CurrentQuestView currentQuestView;
    private CurrentObjectiveView currentObjective1View;
    private CurrentObjectiveView currentObjective2View;
    private CurrentObjectiveView currentObjective3View;

    private void InjectGiveViewsToQuestController([TagScope(R.S.Tag.QuestManager)] QuestManagerController questManagerController,
                                                 [EntityScope] CurrentQuestView currentQuestView,
                                                 [TagScope(R.S.Tag.Objective1)] CurrentObjectiveView currentObjective1View,
                                                 [TagScope(R.S.Tag.Objective2)] CurrentObjectiveView currentObjective2View,
                                                 [TagScope(R.S.Tag.Objective3)] CurrentObjectiveView currentObjective3View)
    {
      this.questManagerController = questManagerController;
      this.currentQuestView = currentQuestView;
      this.currentObjective1View = currentObjective1View;
      this.currentObjective2View = currentObjective2View;
      this.currentObjective3View = currentObjective3View;
    }

    private void Awake()
    {
      InjectDependencies("InjectGiveViewsToQuestController");
    }

    private void Start()
    {
      questManagerController.SetViews(currentQuestView, currentObjective1View, currentObjective2View, currentObjective3View);
    }
  }
}