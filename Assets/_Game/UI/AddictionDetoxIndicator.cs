using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>Phase 11 — addiction / withdrawal / detox badge on portrait.</summary>
    public class AddictionDetoxIndicator : MonoBehaviour
    {
        public enum DetoxState { Clean, Dependent, Withdrawal, ManagedDetox, Recovered }

        public event Action<string, DetoxState, string> OnStateChanged;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private Label _iconLabel;
        private readonly Dictionary<string, IndicatorState> _states = new();
        private string _focusedSurvivorId;

        [Serializable]
        public struct IndicatorState
        {
            public DetoxState state;
            public string itemId;
            public float recoveredFadeHours;
        }

        [Serializable]
        public struct SaveState
        {
            public string focusedSurvivorId;
            public IndicatorState focused;
        }

        public SaveState CaptureState()
        {
            var st = new IndicatorState { state = DetoxState.Clean };
            if (!string.IsNullOrEmpty(_focusedSurvivorId))
                _states.TryGetValue(_focusedSurvivorId, out st);
            return new SaveState { focusedSurvivorId = _focusedSurvivorId, focused = st };
        }

        public void RestoreState(SaveState s)
        {
            if (!string.IsNullOrEmpty(s.focusedSurvivorId))
                ShowDependency(s.focusedSurvivorId, s.focused.itemId, s.focused.state);
        }

        /// <summary>Bind to the shared DiegeticHud UIDocument.</summary>
        public void BindDocument(UIDocument document)
        {
            _document = document;
            BindElements();
            Refresh();
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            BindElements();
            Refresh();
        }

        private void BindElements()
        {
            if (_document == null || _document.rootVisualElement == null) return;
            _root = _document.rootVisualElement.Q("addiction-detox-root");
            _iconLabel = _root?.Q<Label>("addiction-detox-icon");
        }

        public void SetFocusedSurvivor(string survivorId)
        {
            _focusedSurvivorId = survivorId;
            Refresh();
        }

        public void ShowDependency(string survivorId, string itemId, DetoxState state)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _states[survivorId] = new IndicatorState
            {
                state = state,
                itemId = itemId,
                recoveredFadeHours = state == DetoxState.Recovered ? 24f : 0f
            };
            if (survivorId == _focusedSurvivorId || string.IsNullOrEmpty(_focusedSurvivorId))
            {
                _focusedSurvivorId = survivorId;
                Refresh();
            }
            OnStateChanged?.Invoke(survivorId, state, itemId);
        }

        /// <summary>Plan alias — cold-turkey withdrawal badge.</summary>
        public void ShowWithdrawal(string survivorId) =>
            ShowDependency(survivorId, "", DetoxState.Withdrawal);

        /// <summary>Plan alias — managed detox progress (0..1 unused visually beyond state).</summary>
        public void ShowDetoxProgress(string survivorId, float progress) =>
            ShowDependency(survivorId, "", DetoxState.ManagedDetox);

        /// <summary>Plan alias — clear badge for a survivor.</summary>
        public void Hide(string survivorId) =>
            ShowDependency(survivorId, "", DetoxState.Clean);

        public void TickRecoveredFade(string survivorId, float gameHours)
        {
            if (!_states.TryGetValue(survivorId, out var st) || st.state != DetoxState.Recovered) return;
            st.recoveredFadeHours -= gameHours;
            if (st.recoveredFadeHours <= 0f)
            {
                st.state = DetoxState.Clean;
                st.recoveredFadeHours = 0f;
            }
            _states[survivorId] = st;
            if (survivorId == _focusedSurvivorId) Refresh();
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh()
        {
            if (_root == null || _iconLabel == null) return;
            var st = new IndicatorState { state = DetoxState.Clean };
            if (!string.IsNullOrEmpty(_focusedSurvivorId))
                _states.TryGetValue(_focusedSurvivorId, out st);

            if (st.state == DetoxState.Clean)
            {
                Hide();
                return;
            }

            Show();
            _root.EnableInClassList("addiction-detox-indicator--withdrawal", st.state == DetoxState.Withdrawal);
            _root.EnableInClassList("addiction-detox-indicator--detox", st.state == DetoxState.ManagedDetox);
            _root.EnableInClassList("addiction-detox-indicator--recovered", st.state == DetoxState.Recovered);

            _iconLabel.text = st.state switch
            {
                DetoxState.Dependent => "Rx",
                DetoxState.Withdrawal => "!",
                DetoxState.ManagedDetox => "⧗",
                DetoxState.Recovered => "✓",
                _ => ""
            };
        }
    }
}
