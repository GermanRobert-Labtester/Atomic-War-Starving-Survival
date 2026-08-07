using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Keyboard/mouse input handler that routes player actions to GameBootstrap.
    /// Attach to the same GameObject as GameBootstrap.
    /// Internal Horror: inventory activate → corpse dispose; [C] quick dispose;
    /// fire/corpse panels steal [1]/[2]/Esc while open.
    /// </summary>
    public class PlayerInputHandler : MonoBehaviour
    {
        private GameBootstrap _bootstrap;

        [Header("Key Bindings")]
        [SerializeField] private KeyCode _pauseKey = KeyCode.Space;
        [SerializeField] private KeyCode _quickSaveKey = KeyCode.F5;
        [SerializeField] private KeyCode _quickLoadKey = KeyCode.F9;
        [SerializeField] private KeyCode _consumeFoodKey = KeyCode.F1;
        [SerializeField] private KeyCode _consumeWaterKey = KeyCode.F2;
        [SerializeField] private KeyCode _consumeIodineKey = KeyCode.F3;
        [SerializeField] private KeyCode _consumeAntiRadKey = KeyCode.F4;
        [SerializeField] private KeyCode _eventChoice1Key = KeyCode.Alpha1;
        [SerializeField] private KeyCode _eventChoice2Key = KeyCode.Alpha2;
        [SerializeField] private KeyCode _eventChoice3Key = KeyCode.Alpha3;
        [SerializeField] private KeyCode _workbenchKey = KeyCode.B;
        [SerializeField] private KeyCode _hatchDefenseKey = KeyCode.H;
        [SerializeField] private KeyCode _mapKey = KeyCode.M;
        [SerializeField] private KeyCode _parleyKey = KeyCode.P;
        [SerializeField] private KeyCode _radioInterceptKey = KeyCode.R;
        [SerializeField] private KeyCode _radioTunerPrevKey = KeyCode.LeftBracket;
        [SerializeField] private KeyCode _radioTunerNextKey = KeyCode.RightBracket;
        [SerializeField] private KeyCode _journalKey = KeyCode.J;
        [SerializeField] private KeyCode _fastForwardKey = KeyCode.F;
        [Header("Internal Horror / Inventory")]
        [SerializeField] private KeyCode _corpseDisposeKey = KeyCode.C;
        [SerializeField] private KeyCode _inventoryCycleKey = KeyCode.I;
        [SerializeField] private KeyCode _inventoryActivateKey = KeyCode.E;
        [SerializeField] private KeyCode _closePanelKey = KeyCode.Escape;

        /// <summary>Exposed for tests / rebinding docs.</summary>
        public KeyCode WorkbenchKey => _workbenchKey;
        public KeyCode FastForwardKey => _fastForwardKey;
        public KeyCode HatchDefenseKey => _hatchDefenseKey;
        public KeyCode MapKey => _mapKey;
        public KeyCode ParleyKey => _parleyKey;
        public KeyCode RadioInterceptKey => _radioInterceptKey;
        public KeyCode RadioTunerPrevKey => _radioTunerPrevKey;
        public KeyCode RadioTunerNextKey => _radioTunerNextKey;
        public KeyCode JournalKey => _journalKey;
        public KeyCode CorpseDisposeKey => _corpseDisposeKey;
        public KeyCode InventoryCycleKey => _inventoryCycleKey;
        public KeyCode InventoryActivateKey => _inventoryActivateKey;
        public KeyCode ClosePanelKey => _closePanelKey;

        private void Awake()
        {
            _bootstrap = GetComponent<GameBootstrap>();
        }

        private void Update()
        {
            if (_bootstrap == null) return;

            // Pause toggle
            if (Input.GetKeyDown(_pauseKey))
            {
                if (_bootstrap.GameState.Phase == GamePhase.Running)
                    _bootstrap.PauseGame();
                else if (_bootstrap.GameState.Phase == GamePhase.Paused)
                    _bootstrap.ResumeGame();
            }

            // Save/Load
            if (Input.GetKeyDown(_quickSaveKey))
                _bootstrap.SaveGame("quicksave");
            if (Input.GetKeyDown(_quickLoadKey))
                _bootstrap.LoadGame("quicksave");

            // Internal Horror panels take number/Esc priority over workbench & events.
            if (HandleInternalHorrorPanelInput())
                return;

            // Screens
            if (Input.GetKeyDown(_workbenchKey))
                _bootstrap.ToggleWorkbench();
            if (Input.GetKeyDown(_hatchDefenseKey))
                _bootstrap.ToggleHatchDefense();
            if (Input.GetKeyDown(_mapKey))
                _bootstrap.OpenMapScreen();
            if (Input.GetKeyDown(_radioInterceptKey))
                _bootstrap.ToggleRadioInterceptLog();
            if (Input.GetKeyDown(_radioTunerPrevKey))
                _bootstrap.CycleRadioTunerPrev();
            if (Input.GetKeyDown(_radioTunerNextKey))
                _bootstrap.CycleRadioTunerNext();
            if (Input.GetKeyDown(_journalKey))
                _bootstrap.ToggleJournalBook();

            // Fast-forward toggle: 1x <-> 3x simulation speed
            if (Input.GetKeyDown(_fastForwardKey))
                _bootstrap.ToggleFastForward();

            // Inventory strip focus path:
            //   [I] next · [Shift+I] prev · [E]/Enter activate · [Esc] clear tooltip
            // Selected ammo shows military-exclusive tooltip in the diegetic HUD.
            if (Input.GetKeyDown(_inventoryCycleKey))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    _bootstrap.SelectPrevInventoryIcon();
                else
                    _bootstrap.SelectNextInventoryIcon();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                _bootstrap.SelectPrevInventoryIcon();
            if (Input.GetKeyDown(KeyCode.RightArrow))
                _bootstrap.SelectNextInventoryIcon();
            if (Input.GetKeyDown(_inventoryActivateKey) || Input.GetKeyDown(KeyCode.Return))
                _bootstrap.ActivateSelectedInventoryIcon();
            if (Input.GetKeyDown(_closePanelKey)
                && !_bootstrap.IsCorpseDisposePanelOpen()
                && !_bootstrap.IsFirePanelOpen())
            {
                _bootstrap.ClearInventorySelection();
            }

            // Quick open corpse dispose when a body is in stores [C].
            if (Input.GetKeyDown(_corpseDisposeKey))
                _bootstrap.OpenCorpseDisposePanel();

            // Trade: demand parley / surrender after a hatch repel
            if (Input.GetKeyDown(_parleyKey) && IsTradeOpen())
            {
                _bootstrap.DemandTradeParley();
                return;
            }

            // Workbench lines: 1-9 execute when workbench is open (also hatch installs)
            if (IsWorkbenchOpen())
            {
                for (int i = 0; i < 9; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
                    {
                        _bootstrap.ExecuteWorkbenchLine(i);
                        return;
                    }
                }
            }

            // Consumables (primary survivor)
            if (_bootstrap.Survivors != null && _bootstrap.Survivors.Count > 0)
            {
                var primary = _bootstrap.Survivors[0];
                if (primary.IsAlive)
                {
                    if (Input.GetKeyDown(_consumeFoodKey))
                        TryConsumeByType(primary, ItemType.Food);
                    if (Input.GetKeyDown(_consumeWaterKey))
                        TryConsumeByType(primary, ItemType.Water);
                    if (Input.GetKeyDown(_consumeIodineKey))
                        TryConsumeByType(primary, ItemType.Iodine);
                    if (Input.GetKeyDown(_consumeAntiRadKey))
                        TryConsumeByType(primary, ItemType.AntiRad);
                }
            }

            // Event choices (when modal open; workbench numbers take priority above)
            if (Input.GetKeyDown(_eventChoice1Key)) TrySelectChoice(0);
            if (Input.GetKeyDown(_eventChoice2Key)) TrySelectChoice(1);
            if (Input.GetKeyDown(_eventChoice3Key)) TrySelectChoice(2);
        }

        /// <summary>
        /// Corpse dispose / fire fight-seal panels: [1] [2] [Esc].
        /// Returns true only when a key was consumed (digits while open, or Esc).
        /// Other hotkeys (pause, journal, …) still work with a panel up.
        /// </summary>
        private bool HandleInternalHorrorPanelInput()
        {
            bool corpseOpen = _bootstrap.IsCorpseDisposePanelOpen();
            bool fireOpen = _bootstrap.IsFirePanelOpen();
            if (!corpseOpen && !fireOpen) return false;

            if (Input.GetKeyDown(_closePanelKey))
            {
                _bootstrap.CloseInternalHorrorPanels();
                return true;
            }

            // Digit keys: panel choices first; swallow 3–9 so workbench/events don't fire.
            bool digitPressed = false;
            int digit = -1;
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
                {
                    digitPressed = true;
                    digit = i + 1;
                    break;
                }
            }
            if (!digitPressed) return false;

            if (corpseOpen)
            {
                if (digit == 1)
                    _bootstrap.SelectCorpseDispose(CorpseDisposeChoice.Bury);
                else if (digit == 2)
                    _bootstrap.SelectCorpseDispose(CorpseDisposeChoice.ProcessFertilizer);
                return true;
            }

            // Fire panel (when corpse panel is not also open).
            if (digit == 1)
                _bootstrap.SelectFightFire();
            else if (digit == 2)
                _bootstrap.SelectSealBulkhead();
            return true;
        }

        private bool IsWorkbenchOpen()
        {
            var wb = _bootstrap != null ? _bootstrap.GetComponentInChildren<UI.WorkbenchUI>(true) : null;
            if (wb == null && _bootstrap != null)
            {
                // HUD may own the WorkbenchUI
                var hud = _bootstrap.GetComponentInChildren<UI.HUD>(true);
                wb = hud != null ? hud.WorkbenchUI : null;
            }
            return wb != null && wb.IsOpen;
        }

        private bool IsTradeOpen()
        {
            if (_bootstrap == null) return false;
            var hud = _bootstrap.GetComponentInChildren<UI.HUD>(true);
            var trade = hud != null ? hud.TradeScreenUI : null;
            if (trade == null)
                trade = _bootstrap.GetComponentInChildren<UI.TradeScreenUI>(true);
            return trade != null && trade.IsOpen;
        }

        private void TryConsumeByType(Survivor sv, ItemType type)
        {
            if (_bootstrap.Inventory == null) return;
            foreach (var slot in _bootstrap.Inventory.Slots)
            {
                if (slot.Item != null && slot.Item.type == type && slot.Amount > 0)
                {
                    _bootstrap.ConsumeItem(sv, slot.Item);
                    return;
                }
            }
        }

        private void TrySelectChoice(int index)
        {
            if (_bootstrap.EventRunner == null) return;
            // Find the EventModalUI on the HUD
            var hud = GetComponentInChildren<UI.EventModalUI>();
            if (hud != null && hud.IsOpen)
            {
                hud.SelectChoice(index, _bootstrap.EventRunner);
            }
        }
    }
}
