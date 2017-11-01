using Harmony;
using TalesOfAscaria;
using UnityEngine;
using UnityEngine.UI;

namespace TalesOfAscaria
{
  public class CurrentObjectiveView : GameScript
  {
    public bool ObjectiveEnabled
    {
      get { return textView.enabled; }
      set
      {
        textView.enabled = value;
        objectiveBackground.enabled = value;
      }
    }

    private Image objectiveBackground;
    private Text textView;

    public void InjectCurrentObjectiveView([GameObjectScope] Text textView,
                                           [SiblingsScope] Image objectiveBackground)
    {
      this.textView = textView;
      this.objectiveBackground = objectiveBackground;
    }

    public void Awake()
    {
      InjectDependencies("InjectCurrentObjectiveView");
    }

    public void SetObjective(string objective)
    {
      textView.text = objective;
      RectTransform backgroundRectTransform = objectiveBackground.rectTransform;
      float width = LayoutUtility.GetPreferredWidth(textView.rectTransform);
      backgroundRectTransform.sizeDelta = new Vector2(width * 1.57f, backgroundRectTransform.sizeDelta.y);
    }
  }
}