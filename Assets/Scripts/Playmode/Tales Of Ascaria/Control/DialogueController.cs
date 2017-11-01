using UnityEngine;
using UnityEngine.UI;
using Harmony;

namespace TalesOfAscaria
{
  public delegate void OnDialogueInteractionTriggeredHandler(XInputDotNetPure.PlayerIndex playerIndex);

  public class DialogueController : GameScript
  {
    public event OnDialogueInteractionTriggeredHandler OnDialogueInteractionTriggered;

    [Tooltip("The dialogue box of the gameobject")]
    [SerializeField]
    private GameObject dialogueBox;

    [Tooltip("The dialogue Text of the gameobject")]
    [SerializeField]
    private Text dialogueText;

    [Tooltip("The array of dialogues before a quest")]
    [SerializeField]
    private string[] preQuestDialogues;

    public string[] Dialogues { get; set; }
    public bool QuestDialogueFinished { get; set; }
    public int IndexDialogue { get; set; }
    public Text DialogueText
    {
      get
      {
        return dialogueText;
      }
      set
      {
        dialogueText = value;
      }
    }

    private bool dialogActive;
    
    private InteractionSensor interactionSensor;

    private void InjectDialogueController([SiblingsScope] InteractionSensor interactionSensor)
    {
      this.interactionSensor = interactionSensor;
    }

    private void Awake()
    {
      InjectDependencies("InjectDialogueController");

      interactionSensor.OnInteractionTrigger += OnInteract;
    }

    private void Start()
    {
      dialogueBox.SetActive(false);
      Dialogues = preQuestDialogues;
    }

    private void Update()
    {
      if (interactionSensor.HasExitedInteraction)
      {
        HideBox();
        interactionSensor.HasExitedInteraction = false;
      }
    }

    private void OnDestroy()
    {
      interactionSensor.OnInteractionTrigger -= OnInteract;
    }

    public void BeginDialogue()
    {
      if (!dialogActive)
      {
        ShowBox();
      }     
    }

    private void ShowBox()
    {
      ResetDialogue();
      dialogueBox.SetActive(true);
      dialogActive = true;
    }

    public void HideBox()
    {
      QuestDialogueFinished = false;
      dialogueBox.SetActive(false);
      dialogActive = false;
    }

    private void ResetDialogue()
    {
      IndexDialogue = 0;
      dialogueText.text = Dialogues[IndexDialogue];
    }

    private void OnInteract(XInputDotNetPure.PlayerIndex playerIndex)
    {
      if (OnDialogueInteractionTriggered != null) OnDialogueInteractionTriggered(playerIndex);
    }
  }
}