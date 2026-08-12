using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>Phase 11 — moral branch direction on survivor portrait.</summary>
    public class MoralBranchDisplay : MonoBehaviour
    {
        public event Action<string, MoralBranchDirection> OnBranchChanged;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private VisualElement _icon;
        private Label _label;
        private readonly Dictionary<string, MoralBranchDirection> _branches = new();
        private string _focusedSurvivorId;

        [Serializable]
        public struct SaveState
        {
            public string focusedSurvivorId;
            public MoralBranchDirection focusedBranch;
        }

        public SaveState CaptureState()
        {
            var dir = MoralBranchDirection.Neutral;
            if (!string.IsNullOrEmpty(_focusedSurvivorId))
                _branches.TryGetValue(_focusedSurvivorId, out dir);
            return new SaveState { focusedSurvivorId = _focusedSurvivorId, focusedBranch = dir };
        }

        public void RestoreState(SaveState s)
        {
            if (!string.IsNullOrEmpty(s.focusedSurvivorId))
                SetBranch(s.focusedSurvivorId, s.focusedBranch);
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement.Q("moral-branch-root");
            _icon = _root?.Q("moral-branch-icon");
            _label = _root?.Q<Label>("moral-branch-label");
            Refresh();
        }

        public void SetFocusedSurvivor(string survivorId)
        {
            _focusedSurvivorId = survivorId;
            Refresh();
        }

        public void SetBranch(string survivorId, MoralBranchDirection direction)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _branches[survivorId] = direction;
            if (survivorId == _focusedSurvivorId || string.IsNullOrEmpty(_focusedSurvivorId))
            {
                _focusedSurvivorId = survivorId;
                Refresh();
            }
            OnBranchChanged?.Invoke(survivorId, direction);
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh()
        {
            if (_root == null) return;
            var dir = MoralBranchDirection.Neutral;
            if (!string.IsNullOrEmpty(_focusedSurvivorId))
                _branches.TryGetValue(_focusedSurvivorId, out dir);

            if (dir == MoralBranchDirection.Neutral)
            {
                Hide();
                return;
            }

            Show();
            if (_icon != null)
            {
                _icon.text = dir == MoralBranchDirection.NumbedResilience ? "⛨" : "♥";
                _icon.EnableInClassList("moral-branch-icon--compassion",
                    dir == MoralBranchDirection.BurdenedCompassion);
            }
            if (_label != null)
            {
                _label.text = dir == MoralBranchDirection.NumbedResilience
                    ? "Numbed" : "Compassionate";
            }
        }
    }
}
