using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// UI Element #07 — Ration Allocation Dial.
    /// Centre modal: survivor name, 3 food slot options, calorie count, +/- adjust.
    /// Raises OnRationConfirmed / OnRationSkipped.
    /// </summary>
    public class RationAllocationDial : MonoBehaviour
    {
        public event Action<string, string, int> OnRationConfirmed; // (survivor_id, food_id, kcal)
        public event Action<string> OnRationSkipped;                 // (survivor_id)

        [SerializeField] private UIDocument _document;

        private VisualElement _root;

        /// <summary>Tree the callbacks are already wired to; guards against double-subscription.</summary>
        private VisualElement _wiredRoot;

        private Label _survivorLabel;
        private Label _calorieLabel;
        private Label[] _slotLabels  = new Label[3];
        private Button[] _slotButtons = new Button[3];
        private Button _confirmButton;
        private Label _skipLabel;

        private string _survivorId;
        private string[] _foodIds    = new string[3];
        private string[] _foodNames  = new string[3];
        private int[]    _foodKcal   = new int[3];
        private int      _selectedSlot = 0;
        private int      _multiplier   = 1;

        [Serializable]
        public struct SaveState
        {
            public string survivorId;
            public string[] foodIds;
            public string[] foodNames;
            public int[] foodKcal;
            public int selectedSlot;
            public int multiplier;
        }
        public SaveState CaptureState() => new SaveState
        {
            survivorId = _survivorId, foodIds = _foodIds,
            foodNames = _foodNames, foodKcal = _foodKcal,
            selectedSlot = _selectedSlot, multiplier = _multiplier
        };

        /// <summary>Restores the dial exactly as saved: food slots, selection, multiplier, visibility.</summary>
        public void RestoreState(SaveState s)
        {
            if (s.foodIds == null || s.foodIds.Length == 0)
            {
                Hide();
                return;
            }

            _survivorId = s.survivorId;
            _foodIds = new string[3];
            _foodNames = new string[3];
            _foodKcal = new int[3];
            for (int i = 0; i < 3 && i < s.foodIds.Length; i++)
            {
                _foodIds[i] = s.foodIds[i];
                _foodNames[i] = s.foodNames != null && i < s.foodNames.Length ? s.foodNames[i] : null;
                _foodKcal[i] = s.foodKcal != null && i < s.foodKcal.Length ? s.foodKcal[i] : 0;
                if (_slotLabels[i] != null)
                    _slotLabels[i].text = !string.IsNullOrEmpty(_foodIds[i])
                        ? $"{(_foodNames[i] ?? _foodIds[i]).ToUpper()}  {_foodKcal[i]} kcal"
                        : "— EMPTY SLOT —";
            }
            _selectedSlot = Mathf.Clamp(s.selectedSlot, 0, 2);
            _multiplier = Mathf.Clamp(s.multiplier, 1, 3);
            RefreshSelection();
            RefreshCalorie();
            Show();
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement?.Q("ration-dial-root");
            if (_root == null) return;

            _survivorLabel = _root.Q<Label>("ration-survivor-label");
            _calorieLabel  = _root.Q<Label>("ration-calorie-label");

            for (int i = 0; i < 3; i++)
            {
                _slotLabels[i]  = _root.Q<Label>($"ration-slot-label-{i}");
                _slotButtons[i] = _root.Q<Button>($"ration-slot-btn-{i}");
            }

            _confirmButton = _root.Q<Button>("ration-confirm-btn");
            _skipLabel     = _root.Q<Label>("ration-skip-label");

            WireCallbacks();
            Hide();
        }

        /// <summary>
        /// UIDocument keeps the same visual tree across a disable/enable cycle,
        /// so subscribing on every OnEnable would stack a second handler on the
        /// same Button and raise OnRationConfirmed twice for one click —
        /// consuming the ration twice. Subscribe only when the tree is new.
        /// </summary>
        private void WireCallbacks()
        {
            if (ReferenceEquals(_wiredRoot, _root)) return;
            _wiredRoot = _root;

            for (int i = 0; i < 3; i++)
            {
                int idx = i;
                if (_slotButtons[i] != null)
                    _slotButtons[i].clicked += () => SelectSlot(idx);
            }

            var incBtn = _root.Q<Button>("ration-inc-btn");
            var decBtn = _root.Q<Button>("ration-dec-btn");
            if (incBtn != null) incBtn.clicked += () => { _multiplier = Mathf.Min(_multiplier + 1, 3); RefreshCalorie(); };
            if (decBtn != null) decBtn.clicked += () => { _multiplier = Mathf.Max(_multiplier - 1, 1); RefreshCalorie(); };

            if (_confirmButton != null)
                _confirmButton.clicked += Confirm;

            if (_skipLabel != null)
                _skipLabel.RegisterCallback<ClickEvent>(_ => Skip());
        }

        public void Open(string survivorId, string survivorName,
                         (string id, string name, int kcal)[] slots)
        {
            _survivorId = survivorId;
            if (_survivorLabel != null) _survivorLabel.text = survivorName?.ToUpper() ?? "";
            for (int i = 0; i < 3; i++)
            {
                if (i < slots.Length)
                {
                    _foodIds[i]   = slots[i].id;
                    _foodNames[i] = slots[i].name;
                    _foodKcal[i]  = slots[i].kcal;
                    if (_slotLabels[i] != null)
                        _slotLabels[i].text = $"{slots[i].name?.ToUpper()}  {slots[i].kcal} kcal";
                }
                else if (_slotLabels[i] != null)
                {
                    _slotLabels[i].text = "— EMPTY SLOT —";
                }
            }
            _selectedSlot = 0;
            _multiplier   = 1;
            RefreshSelection();
            RefreshCalorie();
            Show();
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void SelectSlot(int idx)
        {
            _selectedSlot = idx;
            RefreshSelection();
            RefreshCalorie();
        }

        private void RefreshSelection()
        {
            for (int i = 0; i < 3; i++)
                if (_slotButtons[i] != null)
                    _slotButtons[i].EnableInClassList("ration-slot--selected", i == _selectedSlot);
        }

        private void RefreshCalorie()
        {
            int total = _selectedSlot < 3 ? _foodKcal[_selectedSlot] * _multiplier : 0;
            if (_calorieLabel != null) _calorieLabel.text = $"TOTAL: {total} kcal  ×{_multiplier}";
        }

        private void Confirm()
        {
            int total = _selectedSlot < 3 ? _foodKcal[_selectedSlot] * _multiplier : 0;
            OnRationConfirmed?.Invoke(_survivorId, _foodIds[_selectedSlot], total);
            Hide();
        }

        private void Skip()
        {
            OnRationSkipped?.Invoke(_survivorId);
            Hide();
        }
    }
}
