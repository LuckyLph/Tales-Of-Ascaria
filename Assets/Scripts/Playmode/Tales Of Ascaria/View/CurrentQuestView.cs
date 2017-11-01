using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace TalesOfAscaria
{
    public class CurrentQuestView : GameScript
    {
        public bool QuestEnabled
        {
            get { return textView.enabled; }
            set { textView.enabled = value; }
        }

        private Text textView;

        public void InjectCurrentQuestView([GameObjectScope] Text textView)
        {
            this.textView = textView;
        }

        public void Awake()
        {
            InjectDependencies("InjectCurrentQuestView");
        }

        public void SetQuest(string quest)
        {
            textView.text = quest;
        }

        public void UpdatePosition(Vector2 sizeDelta)
        {
        }
    }
}