using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>Phase 11 — memorial wall modal with dog tags and pay respects.</summary>
    public class MemorialWallUI : MonoBehaviour
    {
        public event Action<string> OnPayRespectsRequested;
        public event Action OnClosed;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private VisualElement _entryList;
        private Label _emptyLabel;
        private Button _payBtn;
        private string _activeSurvivorId;
        private readonly List<MemorialEntry> _entries = new();

        [Serializable]
        public struct SaveState
        {
            public bool isOpen;
            public int entryCount;
        }

        public SaveState CaptureState() => new SaveState
        {
            isOpen = _root != null && !_root.ClassListContains("hidden"),
            entryCount = _entries.Count
        };

        public void RestoreState(SaveState s) { /* entries rebuilt from system */ }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement.Q("memorial-wall-root");
            _entryList = _root?.Q("memorial-entry-list");
            _emptyLabel = _root?.Q<Label>("memorial-wall-empty");
            _payBtn = _root?.Q<Button>("memorial-pay-respects-btn");
            if (_payBtn != null)
                _payBtn.clicked += () =>
                {
                    if (!string.IsNullOrEmpty(_activeSurvivorId))
                        OnPayRespectsRequested?.Invoke(_activeSurvivorId);
                };
            Hide();
        }

        public void SetActiveSurvivor(string survivorId) => _activeSurvivorId = survivorId;

        public void AddEntry(MemorialEntry entry)
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].SurvivorId == entry.SurvivorId)
                {
                    _entries[i] = entry;
                    RebuildList();
                    return;
                }
            }
            _entries.Add(entry);
            RebuildList();
        }

        public void ClearEntries()
        {
            _entries.Clear();
            RebuildList();
        }

        public void SetComfortActive(bool active)
        {
            _root?.EnableInClassList("memorial-wall--comfort", active);
        }

        private void RebuildList()
        {
            if (_entryList == null) return;
            _entryList.Clear();
            if (_entries.Count == 0)
            {
                if (_emptyLabel != null) _emptyLabel.style.display = DisplayStyle.Flex;
                return;
            }
            if (_emptyLabel != null) _emptyLabel.style.display = DisplayStyle.None;

            for (int i = 0; i < _entries.Count; i++)
            {
                var e = _entries[i];
                var row = new VisualElement();
                row.AddToClassList("memorial-entry-row");
                var name = new Label(e.DisplayName ?? e.SurvivorId);
                name.AddToClassList("memorial-entry-name");
                var day = new Label(e.DeathDay > 0 ? $"DAY {e.DeathDay}" : "FALLEN");
                day.AddToClassList("memorial-entry-day");
                row.Add(name);
                row.Add(day);
                _entryList.Add(row);
            }
        }

        public void Show()
        {
            if (_root == null) return;
            _root.RemoveFromClassList("hidden");
            if (_emptyLabel != null && _entries.Count == 0)
                _emptyLabel.text = "The wall is bare. No one has been remembered yet.";
        }

        public void Hide()
        {
            _root?.AddToClassList("hidden");
            OnClosed?.Invoke();
        }
    }
}
