using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public class CharacterArcProgressPanel : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private VisualElement _root, _timeline;
        private Label _charName, _charProfession, _charBio;
        private VisualElement[] _stageCircles = new VisualElement[4];
        private Label[] _stageDescs = new Label[4];
        private VisualElement _branchIndicators, _branchA, _branchB;
        private Label _branchALabel, _branchBLabel;
        private Label _rewardName;
        private bool _bound;

        public void BindDocument(UIDocument doc) { _document = doc; _bound = false; }
        private void EnsureBound()
        {
            if (_bound || _document == null) return;
            _root = _document.rootVisualElement?.Q<VisualElement>("character-arc-panel");
            if (_root == null) return;
            _charName = _root.Q<Label>("character-name");
            _charProfession = _root.Q<Label>("character-profession");
            _charBio = _root.Q<Label>("character-bio");
            _timeline = _root.Q<VisualElement>("arc-timeline");
            for (int i = 0; i < 4; i++)
            {
                _stageCircles[i] = _root.Q<VisualElement>($"stage-{i+1}-circle");
                _stageDescs[i] = _root.Q<Label>($"stage-{i+1}-desc");
            }
            _branchIndicators = _root.Q<VisualElement>("branch-indicators");
            _branchA = _root.Q<VisualElement>("branch-a");
            _branchB = _root.Q<VisualElement>("branch-b");
            _branchALabel = _root.Q<Label>("branch-a-label");
            _branchBLabel = _root.Q<Label>("branch-b-label");
            _rewardName = _root.Q<Label>("reward-trait-name");
            _bound = true;
        }

        public void ShowCharacter(string survivorId, string displayName,
            string profession, string bio, string[] startingTraits,
            int currentMilestone, string branchTaken)
        {
            EnsureBound();
            if (_root == null) return;
            _root.style.display = DisplayStyle.Flex;
            if (_charName != null) _charName.text = displayName;
            if (_charProfession != null) _charProfession.text = profession;
            if (_charBio != null) _charBio.text = bio;
            for (int i = 0; i < 4; i++)
            {
                if (_stageCircles[i] != null)
                {
                    _stageCircles[i].Clear();
                    if (i < currentMilestone) { _stageCircles[i].style.backgroundColor = new StyleColor(new Color(0.3f, 0.69f, 0.31f)); _stageCircles[i].Add(new Label("✓") { style = { color = Color.white } }); }
                    else if (i == currentMilestone) _stageCircles[i].style.backgroundColor = new StyleColor(new Color(1f, 0.76f, 0.03f));
                    else _stageCircles[i].style.backgroundColor = new StyleColor(new Color(0.46f, 0.46f, 0.46f));
                }
            }
            if (currentMilestone == 2) { if (_branchIndicators != null) _branchIndicators.style.display = DisplayStyle.Flex; }
            else { if (_branchIndicators != null) _branchIndicators.style.display = DisplayStyle.None; }
        }

        public void SetStageComplete(int stageIndex, string description)
        {
            if (stageIndex >= 0 && stageIndex < 4 && _stageDescs[stageIndex] != null)
                _stageDescs[stageIndex].text = description;
        }

        public void SetBranchChosen(string branchId)
        {
            if (_branchA != null) _branchA.style.opacity = branchId == "a" ? 1f : 0.3f;
            if (_branchB != null) _branchB.style.opacity = branchId == "b" ? 1f : 0.3f;
        }

        public void SetArcComplete(string rewardTraitName, string rewardTraitIconId)
        {
            if (_rewardName != null) _rewardName.text = rewardTraitName;
        }

        public void SetStressLevel(float stress) { /* simplified */ }
        public void Hide() { if (_root != null) _root.style.display = DisplayStyle.None; }
    }
}
