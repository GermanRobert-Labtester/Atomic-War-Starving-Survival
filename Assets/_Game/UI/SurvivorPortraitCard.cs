using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// UI Element #05 — Survivor Portrait Card.
    /// Reusable 180x240 card: name, role, 4 stat bars, status label, blood type badge.
    /// Designed to be cloned/pooled as a sub-widget inside larger panels.
    /// Raises OnCardClicked when interactive.
    /// </summary>
    public class SurvivorPortraitCard : MonoBehaviour
    {
        public enum SurvivorStatus { Healthy, Stressed, Ill, Critical, Deceased }
        public event Action<string> OnCardClicked; // survivor_id

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private Label _nameLabel;
        private Label _roleLabel;
        private Label _statusLabel;
        private Label _bloodTypeLabel;
        private VisualElement _healthFill;
        private VisualElement _moraleFill;
        private VisualElement _fatigueFill;
        private VisualElement _radFill;

        private string _survivorId;
        private SurvivorStatus _status;

        [Serializable]
        public struct SaveState
        {
            public string survivorId;
            public string name;
            public string role;
            public float health;
            public float morale;
            public float fatigue;
            public float radiation;
            public string bloodType;
            public SurvivorStatus status;
        }

        public SaveState CaptureState() => _lastState;
        private SaveState _lastState;

        public void RestoreState(SaveState s) => Bind(
            s.survivorId, s.name, s.role, s.health, s.morale, s.fatigue, s.radiation, s.bloodType, s.status);

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement.Q("survivor-portrait-root");
            if (_root == null) return;
            _nameLabel      = _root.Q<Label>("survivor-name-label");
            _roleLabel      = _root.Q<Label>("survivor-role-label");
            _statusLabel    = _root.Q<Label>("survivor-status-label");
            _bloodTypeLabel = _root.Q<Label>("survivor-blood-type-label");
            _healthFill     = _root.Q("survivor-health-fill");
            _moraleFill     = _root.Q("survivor-morale-fill");
            _fatigueFill    = _root.Q("survivor-fatigue-fill");
            _radFill        = _root.Q("survivor-rad-fill");
            if (_root != null)
                _root.RegisterCallback<ClickEvent>(_ => OnCardClicked?.Invoke(_survivorId));
        }

        public void Bind(string survivorId, string name, string role,
                         float health, float morale, float fatigue, float radiation,
                         string bloodType, SurvivorStatus status)
        {
            _survivorId = survivorId;
            _status     = status;
            _lastState  = new SaveState
            {
                survivorId = survivorId, name = name, role = role,
                health = health, morale = morale, fatigue = fatigue, radiation = radiation,
                bloodType = bloodType, status = status
            };

            if (_nameLabel      != null) _nameLabel.text      = name?.ToUpper() ?? "";
            if (_roleLabel      != null) _roleLabel.text      = role?.ToUpper()  ?? "";
            if (_bloodTypeLabel != null) _bloodTypeLabel.text = bloodType ?? "??";

            SetFill(_healthFill,  health  / 100f);
            SetFill(_moraleFill,  morale  / 100f);
            SetFill(_fatigueFill, fatigue / 100f);
            SetFill(_radFill,     radiation / 100f);

            if (_statusLabel != null)
            {
                _statusLabel.text = status.ToString().ToUpper();
                _statusLabel.EnableInClassList("survivor-status--critical",  status == SurvivorStatus.Critical || status == SurvivorStatus.Deceased);
                _statusLabel.EnableInClassList("survivor-status--ill",       status == SurvivorStatus.Ill);
                _statusLabel.EnableInClassList("survivor-status--stressed",  status == SurvivorStatus.Stressed);
            }
            if (_root != null)
                _root.EnableInClassList("survivor-card--critical", status == SurvivorStatus.Critical || status == SurvivorStatus.Deceased);
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private static void SetFill(VisualElement fill, float t)
        {
            if (fill == null) return;
            fill.style.width = Length.Percent(Mathf.Clamp01(t) * 100f);
        }
    }
}
