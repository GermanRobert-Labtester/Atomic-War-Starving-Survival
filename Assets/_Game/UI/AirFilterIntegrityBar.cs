using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// UI Element #03 — Air Filter Integrity Bar.
    /// Horizontal bar: filter integrity %, change countdown (days), toxicity level.
    /// Green >60%, amber <60%, red <20%.
    /// Raises OnFilterStateChanged on state change.
    /// </summary>
    public class AirFilterIntegrityBar : MonoBehaviour
    {
        public enum FilterState { Good, Warning, Critical }
        public event Action<float, FilterState> OnFilterStateChanged;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private VisualElement _fillBar;
        private Label _integrityLabel;
        private Label _changeCountdownLabel;
        private Label _toxicityLabel;
        private Label _stateLabel;

        private float _integrity;
        private float _changeCountdownDays;
        private float _toxicityPct;
        private FilterState _state;

        [Serializable]
        public struct SaveState { public float integrity; public float changeCountdownDays; public float toxicityPct; }

        public SaveState CaptureState() => new SaveState
        {
            integrity = _integrity, changeCountdownDays = _changeCountdownDays, toxicityPct = _toxicityPct
        };
        public void RestoreState(SaveState s)
        {
            _integrity = s.integrity; _changeCountdownDays = s.changeCountdownDays; _toxicityPct = s.toxicityPct;
            Refresh();
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement?.Q("air-filter-root");
            if (_root == null) return;
            _fillBar              = _root.Q("air-filter-fill");
            _integrityLabel       = _root.Q<Label>("air-filter-integrity-label");
            _changeCountdownLabel = _root.Q<Label>("air-filter-countdown-label");
            _toxicityLabel        = _root.Q<Label>("air-filter-toxicity-label");
            _stateLabel           = _root.Q<Label>("air-filter-state-label");
            Refresh();
        }

        public void SetFilterData(float integrityPct, float changeCountdownDays, float toxicityPct)
        {
            _integrity            = Mathf.Clamp01(integrityPct);
            _changeCountdownDays  = changeCountdownDays;
            _toxicityPct          = Mathf.Clamp01(toxicityPct);
            _state = _integrity > 0.6f ? FilterState.Good :
                     _integrity > 0.2f ? FilterState.Warning : FilterState.Critical;
            Refresh();
            OnFilterStateChanged?.Invoke(_integrity, _state);
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh()
        {
            if (_root == null) return;
            if (_fillBar != null)
                _fillBar.style.width = Length.Percent(_integrity * 100f);
            if (_integrityLabel != null)
                _integrityLabel.text = $"FILTER: {_integrity * 100f:F0}%";
            if (_changeCountdownLabel != null)
                _changeCountdownLabel.text = $"CHANGE IN: {_changeCountdownDays:F1} days";
            if (_toxicityLabel != null)
                _toxicityLabel.text = $"AIR TOXICITY: {_toxicityPct * 100f:F0}%";
            if (_stateLabel != null)
            {
                _stateLabel.text = _state == FilterState.Critical ? "FILTER CRITICAL — REPLACE NOW" :
                                   _state == FilterState.Warning   ? "FILTER DEGRADED"               : "OPERATIONAL";
                _stateLabel.EnableInClassList("air-filter-state--critical", _state == FilterState.Critical);
                _stateLabel.EnableInClassList("air-filter-state--warning",  _state == FilterState.Warning);
            }
            if (_fillBar != null)
            {
                _fillBar.EnableInClassList("air-filter-fill--warning",  _state == FilterState.Warning);
                _fillBar.EnableInClassList("air-filter-fill--critical", _state == FilterState.Critical);
            }
            _root.EnableInClassList("diegetic-panel--critical", _state == FilterState.Critical);
            _root.EnableInClassList("diegetic-panel--warning",  _state == FilterState.Warning);
        }
    }
}
