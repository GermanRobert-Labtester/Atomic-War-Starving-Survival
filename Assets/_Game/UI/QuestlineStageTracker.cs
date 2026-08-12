using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public class QuestlineStageTracker : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private VisualElement _root, _questList, _questDetail;
        private Label _objectiveText;
        private VisualElement _objectiveCheckboxes;
        private Button _dispatchButton;
        private readonly Dictionary<string, VisualElement> _questEntries = new Dictionary<string, VisualElement>();
        private bool _bound;

        public void BindDocument(UIDocument doc) { _document = doc; _bound = false; }
        private void EnsureBound()
        {
            if (_bound || _document == null) return;
            _root = _document.rootVisualElement?.Q<VisualElement>("questline-stage-tracker");
            if (_root == null) return;
            _questList = _root.Q<ScrollView>("quest-list");
            _questDetail = _root.Q<VisualElement>("quest-detail");
            _objectiveText = _root.Q<Label>("objective-text");
            _objectiveCheckboxes = _root.Q<VisualElement>("objective-checkboxes");
            _dispatchButton = _root.Q<Button>("dispatch-button");
            _bound = true;
        }

        public void AddQuest(string questId, string title, int currentStage, int totalStages,
            string currentObjective, float objectiveProgress)
        {
            EnsureBound();
            if (_questList == null) return;
            var entry = new VisualElement { name = $"quest-{questId}" };
            entry.AddToClassList("quest-entry");
            var nameLabel = new Label(title) { style = { fontSize = 14, color = new StyleColor(new Color(0.88f, 0.88f, 0.88f)) } };
            var stageLabel = new Label($"Stage {currentStage}/{totalStages}") { style = { fontSize = 11, color = new StyleColor(new Color(1f, 0.76f, 0.03f)) } };
            var progBar = new ProgressBar { value = objectiveProgress * 100f, highValue = 100f, style = { height = 8 } };
            entry.Add(nameLabel); entry.Add(stageLabel); entry.Add(progBar);
            entry.RegisterCallback<ClickEvent>(_ => SelectQuest(questId, currentObjective));
            _questList.Add(entry);
            _questEntries[questId] = entry;
        }

        public void UpdateQuestStage(string questId, int newStage, string newObjective)
        {
            if (_questEntries.TryGetValue(questId, out var entry))
            {
                var stageLabel = entry.Q<Label>(className: "stage-label");
                if (stageLabel == null) { stageLabel = (Label)entry[1]; }
                // simplified update
            }
        }

        private void SelectQuest(string questId, string objective)
        {
            if (_objectiveText != null) _objectiveText.text = objective;
            if (_questDetail != null) _questDetail.style.display = DisplayStyle.Flex;
        }

        public void OnDispatchRequested(string questId) { /* host callback */ }
        public void RemoveQuest(string questId)
        {
            if (_questEntries.TryGetValue(questId, out var entry))
            {
                _questList?.Remove(entry);
                _questEntries.Remove(questId);
            }
        }
        public void Hide() { if (_root != null) _root.style.display = DisplayStyle.None; }
    }
}
