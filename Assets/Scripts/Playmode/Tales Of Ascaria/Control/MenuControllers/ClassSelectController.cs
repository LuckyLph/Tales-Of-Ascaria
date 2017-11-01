using System.Collections.Generic;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TalesOfAscaria
{
  public class ClassSelectController : GameScript, IMenuController
  {
    [Tooltip("The default button to be selected upon starting the menu")]
    [SerializeField]
    private Button defaultButton;

    [Tooltip("The default button to be selected upon selecting the class")]
    [SerializeField]
    private Button confirmButton;
    [Tooltip("The button to cancel the class choice")]
    [SerializeField]
    private Button cancelButton;

    [Tooltip("The guardian's button")]
    [SerializeField]
    private Button guardianButton;
    [Tooltip("The ranger's button")]
    [SerializeField]
    private Button rangerButton;
    [Tooltip("The mage's button")]
    [SerializeField]
    private Button mageButton;
    [Tooltip("The priest's button")]
    [SerializeField]
    private Button priestButton;
    [Tooltip("The shotokan's button")]
    [SerializeField]
    private Button shotokanButton;

    private Button selectedClassButton;

    [Tooltip("The activity to launch upon confirming")]
    [SerializeField]
    private Activity gameActivity;

    private int hoveredButtonIndex = 0;
    private bool isConfirming;

    private ActivityStack activityStack;
    private ClassType selectedClass = ClassType.Guardian;
    private GameParametersFromMenu parameters;

    private void InjectMainMenuController([ApplicationScope] ActivityStack activityStack,
                                          [ApplicationScope] PlayerInputSensor inputs,
                                          [ApplicationScope] GameParametersFromMenu parameters)
    {
      this.activityStack = activityStack;
      this.parameters = parameters;
    }

    private void Awake()
    {
      InjectDependencies("InjectMainMenuController");
    }

    public void ClassButtonClicked(string classType)
    {
      if (classType == "Guardian")
      {
        selectedClass = ClassType.Guardian;
        selectedClassButton = guardianButton;
      }
      else if (classType == "Ranger")
      {
        selectedClass = ClassType.Ranger;
        selectedClassButton = rangerButton;
      }
      else if (classType == "Mage")
      {
        selectedClass = ClassType.Mage;
        selectedClassButton = mageButton;
      }
      else if (classType == "Priest")
      {
        selectedClass = ClassType.Priest;
        selectedClassButton = priestButton;
      }
      else if (classType == "Shotokan")
      {
        selectedClass = ClassType.Shotokan;
        selectedClassButton = shotokanButton;
      }
      Debug.Log("enabled");
      confirmButton.enabled = true;
      cancelButton.enabled = true;
      confirmButton.Select();
    }

    public void OnCancelClicked()
    {
      Debug.Log("disabled");
      selectedClassButton.Select();
      confirmButton.enabled = false;
      cancelButton.enabled = false;
    }


    public void OnConfirmClicked()
    {
      parameters.InitialPlayer = selectedClass;
      activityStack.StartActivity(gameActivity);
    }


    public void OnCreate(params object[] parameters)
    {
    }


    public void OnResume()
    {
      Debug.Log("Default selected");
      defaultButton.Select();
      Debug.Log("disabled");
      confirmButton.enabled = false;
      cancelButton.enabled = false;
    }


    public void OnPause()
    {

    }


    public void OnStop()
    {

    }
  }
}