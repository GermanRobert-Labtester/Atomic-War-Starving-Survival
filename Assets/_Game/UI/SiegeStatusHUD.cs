using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public class SiegeStatusHUD : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private VisualElement _root;
        private ProgressBar _hatchBar, _breachBar;
        private Label _siegeLabel;
        private VisualElement _reinforcementIcon, _activeEffects, _commandButtons;
        private bool _bound;

        public void BindDocument(UIDocument doc) { _document = doc; _bound = false; }
        private void EnsureBound()
        {
            if (_bound || _document == null) return;
            _root = _document.rootVisualElement?.Q<VisualElement>("siege-status");
            if (_root == null) return;
            _hatchBar = _root.Q<ProgressBar>("hatch-integrity-bar");
            _breachBar = _root.Q<ProgressBar>("breach-progress-bar");
            _siegeLabel = _root.Q<Label>("siege-label");
            _reinforcementIcon = _root.Q<VisualElement>("reinforcement-icon");
            _activeEffects = _root.Q<VisualElement>("active-effects");
            _commandButtons = _root.Q<VisualElement>("command-buttons");
            _bound = true;
        }

        public void ShowSiege(float hatchIntegrity, int tier, float breachProgress)
        {
            EnsureBound();
            if (_root == null) return;
            _root.style.display = DisplayStyle.Flex;
            UpdateIntegrity(hatchIntegrity);
            UpdateBreachProgress(breachProgress);
            if (_reinforcementIcon != null)
            {
                _reinforcementIcon.Clear();
                _reinforcementIcon.AddToClassList(tier switch { 2 => "steel", 3 => "composite", _ => "wood" });
            }
        }

        public void UpdateIntegrity(float v)
        {
            EnsureBound();
            if (_hatchBar != null) { _hatchBar.highValue = 100f; _hatchBar.value = v * 100f; }
        }
        public void UpdateBreachProgress(float v)
        {
            EnsureBound();
            if (_breachBar != null) { _breachBar.highValue = 100f; _breachBar.value = v * 100f; }
        }
        public void AddActiveEffect(string effectId, float duration)
        {
            EnsureBound();
            if (_activeEffects == null) return;
            var badge = new Label(effectId.Replace("_", " ").ToUpper()) { style = { fontSize = 10, color = new StyleColor(new Color(1f, 0.76f, 0.03f)), marginRight = 6 } };
            _activeEffects.Add(badge);
        }
        public void RemoveActiveEffect(string effectId)
        {
            if (_activeEffects == null) return;
            for (int i = _activeEffects.childCount - 1; i >= 0; i--)
                if (_activeEffects[i] is Label l && l.text == effectId.Replace("_", " ").ToUpper())
                    _activeEffects.RemoveAt(i);
        }

        public void SetAvailableCommands(string[] commands, System.Action<string> onCommand)
        {
            if (_commandButtons == null) return;
            _commandButtons.Clear();
            foreach (var cmd in commands)
            {
                var btn = new Button(() => onCommand?.Invoke(cmd)) { text = cmd.Replace("_", " ").ToUpper() };
                btn.AddToClassList("command-button");
                _commandButtons.Add(btn);
            }
        }

        public void HideSiege() { if (_root != null) _root.style.display = DisplayStyle.None; }
    }
}
