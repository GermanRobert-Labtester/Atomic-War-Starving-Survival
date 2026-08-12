using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>Phase 11 — personal keepsake slot on survivor detail panel.</summary>
    public class KeepsakeSlotUI : MonoBehaviour
    {
        public event Action<string, string, bool, float> OnKeepsakeChanged;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private Label _iconLabel;
        private VisualElement _crackOverlay;
        private readonly Dictionary<string, KeepsakeState> _states = new();
        private string _focusedSurvivorId;

        [Serializable]
        public struct KeepsakeState
        {
            public string itemId;
            public bool lost;
            public float griefLevel;
        }

        [Serializable]
        public struct SaveState
        {
            public string focusedSurvivorId;
            public KeepsakeState focused;
        }

        public SaveState CaptureState()
        {
            var st = new KeepsakeState();
            if (!string.IsNullOrEmpty(_focusedSurvivorId))
                _states.TryGetValue(_focusedSurvivorId, out st);
            return new SaveState { focusedSurvivorId = _focusedSurvivorId, focused = st };
        }

        public void RestoreState(SaveState s)
        {
            if (!string.IsNullOrEmpty(s.focusedSurvivorId))
                SetKeepsake(s.focusedSurvivorId, s.focused.itemId, s.focused.lost, s.focused.griefLevel);
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement.Q("keepsake-slot-root");
            _iconLabel = _root?.Q<Label>("keepsake-slot-icon");
            _crackOverlay = _root?.Q("keepsake-crack-overlay");
            Refresh();
        }

        public void SetFocusedSurvivor(string survivorId)
        {
            _focusedSurvivorId = survivorId;
            Refresh();
        }

        public void SetKeepsake(string survivorId, string itemId, bool lost, float griefLevel)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _states[survivorId] = new KeepsakeState
            {
                itemId = itemId,
                lost = lost,
                griefLevel = Mathf.Clamp01(griefLevel)
            };
            if (survivorId == _focusedSurvivorId || string.IsNullOrEmpty(_focusedSurvivorId))
            {
                _focusedSurvivorId = survivorId;
                Refresh();
            }
            OnKeepsakeChanged?.Invoke(survivorId, itemId, lost, griefLevel);
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh()
        {
            if (_root == null) return;
            var st = new KeepsakeState();
            if (!string.IsNullOrEmpty(_focusedSurvivorId))
                _states.TryGetValue(_focusedSurvivorId, out st);

            if (string.IsNullOrEmpty(st.itemId))
            {
                Hide();
                return;
            }

            Show();
            if (_iconLabel != null)
            {
                string shortId = st.itemId.Length > 8 ? st.itemId.Substring(0, 8) : st.itemId;
                _iconLabel.text = shortId.ToUpper();
            }
            _root.EnableInClassList("keepsake-slot--lost", st.lost);
            if (_crackOverlay != null)
            {
                _crackOverlay.style.opacity = st.lost ? st.griefLevel : 0f;
                _crackOverlay.EnableInClassList("keepsake-crack-overlay--visible", st.lost);
            }
        }
    }
}
