using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    [Serializable]
    public class CraftQueueSlotData
    {
        public string SlotId;
        public string ItemName = "MEDKIT";
        public string IconText = "[MED]";
        public float Progress01 = 0.5f;
        public float EtaSeconds = 30f;
    }

    public class CraftQueueStrip : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private List<CraftQueueSlotData> _queue = new List<CraftQueueSlotData>();

        private VisualElement _root;
        private ScrollView _scroll;

        public event Action<int> OnSlotCancelled;
        public event Action<List<CraftQueueSlotData>> OnStateChanged;

        public IReadOnlyList<CraftQueueSlotData> Queue => _queue;

        private void OnEnable()
        {
            if (_document == null)
                _document = GetComponent<UIDocument>();

            if (_document != null && _document.rootVisualElement != null)
            {
                _root = _document.rootVisualElement.Q("craft-queue-root") 
                      ?? _document.rootVisualElement.Q("craft_queue_root");
                Bind();
                RefreshUI();
            }
        }

        private void Bind()
        {
            if (_root == null) return;
            _scroll = _root.Q<ScrollView>("craft_queue_scroll");
        }

        public void SetQueue(List<CraftQueueSlotData> queue)
        {
            _queue = queue ?? new List<CraftQueueSlotData>();
            RefreshUI();
            OnStateChanged?.Invoke(_queue);
        }

        public void AddItem(CraftQueueSlotData item)
        {
            if (item == null) return;
            _queue.Add(item);
            RefreshUI();
            OnStateChanged?.Invoke(_queue);
        }

        public void CancelSlot(int index)
        {
            if (index >= 0 && index < _queue.Count)
            {
                _queue.RemoveAt(index);
                RefreshUI();
                OnSlotCancelled?.Invoke(index);
                OnStateChanged?.Invoke(_queue);
            }
        }

        private void RefreshUI()
        {
            if (_scroll == null) return;

            _scroll.Clear();

            for (int i = 0; i < _queue.Count; i++)
            {
                int slotIndex = i;
                CraftQueueSlotData slotData = _queue[i];

                VisualElement slotElem = new VisualElement();
                slotElem.AddToClassList("craft-slot");

                VisualElement topRow = new VisualElement();
                topRow.AddToClassList("craft-slot-top");

                Label iconLabel = new Label(slotData.IconText);
                iconLabel.AddToClassList("craft-icon-tag");

                Button cancelBtn = new Button(() => CancelSlot(slotIndex));
                cancelBtn.text = "[X]";
                cancelBtn.AddToClassList("btn-cancel");

                topRow.Add(iconLabel);
                topRow.Add(cancelBtn);

                Label nameLabel = new Label(slotData.ItemName);
                nameLabel.AddToClassList("craft-item-name");

                VisualElement track = new VisualElement();
                track.AddToClassList("craft-progress-track");

                VisualElement fill = new VisualElement();
                fill.AddToClassList("craft-progress-fill");
                fill.style.width = Length.Percent(Mathf.Clamp01(slotData.Progress01) * 100f);
                track.Add(fill);

                TimeSpan ts = TimeSpan.FromSeconds(Mathf.Max(0, slotData.EtaSeconds));
                Label etaLabel = new Label($"{ts.Minutes:D2}:{ts.Seconds:D2}");
                etaLabel.AddToClassList("craft-eta-label");

                slotElem.Add(topRow);
                slotElem.Add(nameLabel);
                slotElem.Add(track);
                slotElem.Add(etaLabel);

                _scroll.Add(slotElem);
            }
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");
    }
}
