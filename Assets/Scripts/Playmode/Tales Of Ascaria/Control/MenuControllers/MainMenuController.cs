using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TalesOfAscaria 
{
	public class MainMenuController : GameScript, IMenuController
	{
	  [SerializeField]
    private Button defaultButton;

    [SerializeField]
    private Menu newTaleMenu;

    [SerializeField]
    private Menu continueTaleMenu;

    [SerializeField]
    private Menu optionsMenu;

    private ActivityStack activityStack;

    private void InjectMainMenuController([ApplicationScope] ActivityStack activityStack)
    {
      this.activityStack = activityStack;
    }


    private void Awake()
    {
      InjectDependencies("InjectMainMenuController");
    }

	  public void NewTaleButtonClicked()
    {
      activityStack.StartMenu(newTaleMenu);
    }


	  public void ContinueTaleButtonClicked()
	  {
      activityStack.StartMenu(continueTaleMenu);
    }


	  public void OptionsButtonClicked()
	  {
      activityStack.StartMenu(optionsMenu);
    }


	  public void QuitBtn()
	  {
      Application.Quit();
	  }


	  public void OnCreate(params object[] parameters)
	  {
	    
	  }


	  public void OnResume()
	  {
      defaultButton.Select();
	  }


	  public void OnPause()
	  {
	    Debug.Log("On pause called");
	  }


	  public void OnStop()
	  {
      Debug.Log("On stop called");
    }
	}
}