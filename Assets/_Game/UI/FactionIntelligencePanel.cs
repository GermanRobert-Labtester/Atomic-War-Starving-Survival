using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public class FactionIntelligencePanel : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private VisualElement _root, _intelSection, _agentSection;
        private ProgressBar _standingBar;
        private Label _standingValue, _factionName, _tributeInfo;
        private Button _sendAgentBtn, _broadcastBtn;
        private bool _bound;

        public void BindDocument(UIDocument doc) { _document = doc; _bound = false; }
        private void EnsureBound()
        {
            if (_bound || _document == null) return;
            _root = _document.rootVisualElement?.Q<VisualElement>("faction-intelligence-panel");
            if (_root == null) return;
            _standingBar = _root.Q<ProgressBar>("standing-bar");
            _standingValue = _root.Q<Label>("standing-value");
            _factionName = _root.Q<Label>("faction-name");
            _tributeInfo = _root.Q<Label>("tribute-info");
            _intelSection = _root.Q<VisualElement>("intel-section");
            _agentSection = _root.Q<VisualElement>("agent-section");
            _sendAgentBtn = _root.Q<Button>("send-agent-button");
            _broadcastBtn = _root.Q<Button>("propaganda-button");
            _bound = true;
        }

        public void SetFaction(string factionId, string name, float standing, bool hasAlliance)
        {
            EnsureBound();
            if (_root == null) return;
            _root.style.display = DisplayStyle.Flex;
            if (_factionName != null) _factionName.text = name;
            if (_standingBar != null) _standingBar.value = (standing + 100f) / 2f;
            if (_standingValue != null) _standingValue.text = $"{standing:+0;-0}";
        }

        public void AddIntelEntry(string description, float hoursRemaining)
        {
            if (_intelSection == null) return;
            var entry = new Label($"[{hoursRemaining:F0}h] {description}") { style = { fontSize = 12, color = new StyleColor(new Color(0.88f, 0.88f, 0.88f)), marginBottom = 4 } };
            _intelSection.Add(entry);
        }
        public void ClearIntel() { _intelSection?.Clear(); }

        public void SetTributeDemand(string resourceType, int amount, int dueInDays)
        {
            if (_tributeInfo != null) _tributeInfo.text = $"Tribute due in {dueInDays} days: {amount} {resourceType}";
        }

        public void SetActionsAvailable(bool canSendAgent, bool canBroadcast)
        {
            if (_sendAgentBtn != null) _sendAgentBtn.SetEnabled(canSendAgent);
            if (_broadcastBtn != null) _broadcastBtn.SetEnabled(canBroadcast);
        }

        public void Hide() { if (_root != null) _root.style.display = DisplayStyle.None; }
    }
}
