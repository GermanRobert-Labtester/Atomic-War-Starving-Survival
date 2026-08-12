using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>Phase 11 — terminal prognosis countdown banner.</summary>
    public class TerminalPrognosisBanner : MonoBehaviour
    {
        public enum WishOutcome { Active, Completed, Failed }

        public event Action<string, float, string> OnBannerShown;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private Label _textLabel;
        private readonly Dictionary<string, BannerState> _states = new();
        private string _focusedSurvivorId;

        [Serializable]
        public struct BannerState
        {
            public float daysRemaining;
            public string wishId;
            public WishOutcome outcome;
            public bool visible;
        }

        [Serializable]
        public struct SaveState
        {
            public string focusedSurvivorId;
            public BannerState focused;
        }

        public SaveState CaptureState()
        {
            var st = new BannerState();
            if (!string.IsNullOrEmpty(_focusedSurvivorId))
                _states.TryGetValue(_focusedSurvivorId, out st);
            return new SaveState { focusedSurvivorId = _focusedSurvivorId, focused = st };
        }

        public void RestoreState(SaveState s)
        {
            if (!string.IsNullOrEmpty(s.focusedSurvivorId) && s.focused.visible)
                Show(s.focusedSurvivorId, s.focused.daysRemaining, s.focused.wishId);
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
            _root = _document.rootVisualElement.Q("terminal-prognosis-root");
            _textLabel = _root?.Q<Label>("terminal-prognosis-text");
        }

        public void SetFocusedSurvivor(string survivorId)
        {
            _focusedSurvivorId = survivorId;
            Refresh();
        }

        public void Show(string survivorId, float daysRemaining, string wishId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _states[survivorId] = new BannerState
            {
                daysRemaining = daysRemaining,
                wishId = wishId,
                outcome = WishOutcome.Active,
                visible = true
            };
            if (survivorId == _focusedSurvivorId || string.IsNullOrEmpty(_focusedSurvivorId))
            {
                _focusedSurvivorId = survivorId;
                Refresh();
            }
            OnBannerShown?.Invoke(survivorId, daysRemaining, wishId);
        }

        public void SetWishOutcome(string survivorId, WishOutcome outcome)
        {
            if (string.IsNullOrEmpty(survivorId) || !_states.TryGetValue(survivorId, out var st)) return;
            st.outcome = outcome;
            st.visible = true;
            _states[survivorId] = st;
            if (survivorId == _focusedSurvivorId) Refresh();
        }

        /// <summary>Plan alias for SetWishOutcome(Completed).</summary>
        public void MarkWishCompleted(string survivorId) =>
            SetWishOutcome(survivorId, WishOutcome.Completed);

        /// <summary>Plan alias for SetWishOutcome(Failed).</summary>
        public void MarkWishFailed(string survivorId) =>
            SetWishOutcome(survivorId, WishOutcome.Failed);

        public void HideForSurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId) || !_states.TryGetValue(survivorId, out var st)) return;
            st.visible = false;
            _states[survivorId] = st;
            if (survivorId == _focusedSurvivorId) Refresh();
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh()
        {
            if (_root == null || _textLabel == null) return;
            if (string.IsNullOrEmpty(_focusedSurvivorId) ||
                !_states.TryGetValue(_focusedSurvivorId, out var st) || !st.visible)
            {
                Hide();
                return;
            }

            Show();
            _root.EnableInClassList("terminal-prognosis-banner--completed", st.outcome == WishOutcome.Completed);
            _root.EnableInClassList("terminal-prognosis-banner--failed", st.outcome == WishOutcome.Failed);

            _textLabel.text = st.outcome switch
            {
                WishOutcome.Completed => "Their wish was fulfilled",
                WishOutcome.Failed => "Time ran out",
                _ => $"{Mathf.CeilToInt(st.daysRemaining)} days remaining"
            };
        }
    }
}
