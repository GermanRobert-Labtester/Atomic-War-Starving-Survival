using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public class QuestlineProgressTracker : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private VisualElement _root, _stagesContainer, _choicesContainer;
        private Label _questTitle, _stageDescription;
        private int _currentStage = -1;
        private bool _bound;

        public void BindDocument(UIDocument doc) { _document = doc; _bound = false; }
        private void EnsureBound()
        {
            if (_bound || _document == null) return;
            _root = _document.rootVisualElement?.Q<VisualElement>("questline-tracker");
            if (_root == null) return;
            _questTitle = _root.Q<Label>("quest-title");
            _stagesContainer = _root.Q<VisualElement>("stages-container");
            _stageDescription = _root.Q<Label>("stage-description");
            _choicesContainer = _root.Q<VisualElement>("choices-container");
            _bound = true;
        }

        public void ShowQuest(string questId, string title, string[] stageDescriptions, int currentStage)
        {
            EnsureBound();
            if (_root == null) return;
            _root.style.display = DisplayStyle.Flex;
            _currentStage = currentStage;
            if (_questTitle != null) _questTitle.text = title;
            if (_stageDescription != null && currentStage >= 0 && currentStage < stageDescriptions.Length)
                _stageDescription.text = stageDescriptions[currentStage];

            if (_stagesContainer != null)
            {
                _stagesContainer.Clear();
                for (int i = 0; i < 4; i++)
                {
                    var circle = new VisualElement { style = { width = 24, height = 24, borderRadius = 12, marginRight = 8 } };
                    if (i < currentStage) { circle.style.backgroundColor = new StyleColor(new Color(0.3f, 0.69f, 0.31f)); circle.Add(new Label("✓") { style = { fontSize = 12, color = Color.white, unityTextAlign = TextAnchor.MiddleCenter } }); }
                    else if (i == currentStage) circle.style.backgroundColor = new StyleColor(new Color(1f, 0.76f, 0.03f));
                    else circle.style.backgroundColor = new StyleColor(new Color(0.46f, 0.46f, 0.46f));
                    _stagesContainer.Add(circle);
                    if (i < 3) _stagesContainer.Add(new VisualElement { style = { width = 32, height = 2, backgroundColor = i < currentStage ? new StyleColor(new Color(0.3f, 0.69f, 0.31f)) : new StyleColor(new Color(0.46f, 0.46f, 0.46f)), marginTop = 11 } });
                }
            }
        }

        public void ShowChoices(string branchA, string branchB, System.Action<string> onChoice)
        {
            if (_choicesContainer == null) return;
            _choicesContainer.Clear();
            _choicesContainer.style.display = DisplayStyle.Flex;
            var btnA = new Button(() => onChoice?.Invoke("a")) { text = branchA };
            var btnB = new Button(() => onChoice?.Invoke("b")) { text = branchB };
            _choicesContainer.Add(btnA); _choicesContainer.Add(btnB);
        }

        public void Hide() { if (_root != null) _root.style.display = DisplayStyle.None; }
    }
}
