using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    [Serializable]
    public class LootItemSnapshot
    {
        public string ItemId;
        public string ItemName = "CANNED MILITARY RATIONS";
        public string IconText = "[RATION]";
        public int Quantity = 15;
        public float DisplayDuration = 3.0f;
    }

    public class LootHaulTicker : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private LootItemSnapshot _currentLoot = new LootItemSnapshot();

        private VisualElement _root;
        private Label _iconBox;
        private Label _qtyLabel;
        private Label _nameLabel;
        private VisualElement _dismissFill;

        private float _timer;
        private float _maxDuration = 3.0f;

        public event Action<LootItemSnapshot> OnLootDisplayed;
        public event Action OnStateChanged;

        public LootItemSnapshot CurrentLoot => _currentLoot;

        private void OnEnable()
        {
            if (_document == null)
                _document = GetComponent<UIDocument>();

            if (_document != null && _document.rootVisualElement != null)
            {
                _root = _document.rootVisualElement.Q("loot-haul-root") 
                      ?? _document.rootVisualElement.Q("loot_haul_root");
                Bind();
                RefreshUI();
            }
        }

        private void Bind()
        {
            if (_root == null) return;
            _iconBox = _root.Q<Label>("loot_icon_box");
            _qtyLabel = _root.Q<Label>("loot_quantity_label");
            _nameLabel = _root.Q<Label>("loot_name_label");
            _dismissFill = _root.Q<VisualElement>("loot_dismiss_fill");
        }

        private void Update()
        {
            if (_root != null && !_root.ClassListContains("hidden") && _timer > 0)
            {
                _timer -= Time.deltaTime;
                if (_dismissFill != null && _maxDuration > 0)
                {
                    float pct = Mathf.Clamp01(_timer / _maxDuration);
                    _dismissFill.style.width = Length.Percent(pct * 100f);
                }

                if (_timer <= 0)
                {
                    Hide();
                    OnStateChanged?.Invoke();
                }
            }
        }

        public void TriggerLoot(string name, string icon, int qty, float duration = 3.0f)
        {
            _currentLoot.ItemName = name;
            _currentLoot.IconText = icon;
            _currentLoot.Quantity = qty;
            _currentLoot.DisplayDuration = duration;

            _timer = duration;
            _maxDuration = duration;

            Show();
            RefreshUI();

            OnLootDisplayed?.Invoke(_currentLoot);
            OnStateChanged?.Invoke();
        }

        private void RefreshUI()
        {
            if (_root == null) return;

            if (_iconBox != null) _iconBox.text = _currentLoot.IconText;
            if (_qtyLabel != null) _qtyLabel.text = $"+{_currentLoot.Quantity}";
            if (_nameLabel != null) _nameLabel.text = _currentLoot.ItemName;

            if (_dismissFill != null)
            {
                _dismissFill.style.width = Length.Percent(100f);
            }
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");
    }
}
