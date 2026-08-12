using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// UI Element #06 — Moral Decay Meter.
    /// Left sidebar: 10-pip vertical stack depleting as morale falls.
    /// Bottom 3 pips turn critical red. 'BREAKING POINT' text below 20%.
    /// Raises OnMoraleChanged on state change.
    /// </summary>
    public class MoralDecayMeter : MonoBehaviour
    {
        public event Action<float> OnMoraleChanged;

        [SerializeField] private UIDocument _document;
        private const int PipCount = 10;
        private const float BreakingPointThreshold = 0.2f;

        private VisualElement _root;
        private VisualElement[] _pips = new VisualElement[PipCount];
        private Label _breakingLabel;
        private Label _moraleLabel;

        private float _moralePct; // 0-1

        [Serializable]
        public struct SaveState { public float moralePct; }
        public SaveState CaptureState() => new SaveState { moralePct = _moralePct };
        public void RestoreState(SaveState s) { _moralePct = s.moralePct; Refresh(); }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement.Q("moral-decay-root");
            if (_root == null) return;
            _breakingLabel = _root.Q<Label>("moral-breaking-label");
            _moraleLabel   = _root.Q<Label>("moral-pct-label");
            for (int i = 0; i < PipCount; i++)
                _pips[i] = _root.Q($"moral-pip-{i:D2}");
            Refresh();
        }

        public void SetMorale(float moralePct)
        {
            _moralePct = Mathf.Clamp01(moralePct);
            Refresh();
            OnMoraleChanged?.Invoke(_moralePct);
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh()
        {
            if (_root == null) return;
            bool atBreaking = _moralePct <= BreakingPointThreshold;
            if (_moraleLabel   != null) _moraleLabel.text   = $"MORALE: {_moralePct * 100f:F0}%";
            if (_breakingLabel != null)
            {
                _breakingLabel.text = atBreaking ? "BREAKING POINT" : "";
                _breakingLabel.style.display = atBreaking ? DisplayStyle.Flex : DisplayStyle.None;
            }

            // Pips fill from bottom (pip 09) upward
            int activePips = Mathf.RoundToInt(_moralePct * PipCount);
            for (int i = 0; i < PipCount; i++)
            {
                if (_pips[i] == null) continue;
                // pip index 0 = top, 9 = bottom; active from bottom up
                bool active = i >= (PipCount - activePips);
                _pips[i].EnableInClassList("moral-pip--active",   active);
                // Bottom 3 pips (indices 7,8,9) are always danger-colored when active
                _pips[i].EnableInClassList("moral-pip--critical", active && i >= (PipCount - 3));
            }
            _root.EnableInClassList("diegetic-panel--critical", atBreaking);
        }
    }
}
