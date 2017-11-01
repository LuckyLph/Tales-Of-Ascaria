using UnityEngine;
using Harmony;

namespace TalesOfAscaria
{
  public class MainMenuActivityController : GameScript, IActivityController
  {
    [SerializeField]
    private Menu mainMenu;

    private ActivityStack activityStack;

    private void Awake()
    {
      InjectDependencies("InjectMainMenuActivityController");
    }

    private void InjectMainMenuActivityController([ApplicationScope] ActivityStack activityStack)
    {
      this.activityStack = activityStack;
    }

    public void OnCreate()
    {
      activityStack.StartMenu(mainMenu);
      Debug.Log("On create");
    }

    public void OnStop()
    {
      Debug.Log("OnStop");
    }
  }
}