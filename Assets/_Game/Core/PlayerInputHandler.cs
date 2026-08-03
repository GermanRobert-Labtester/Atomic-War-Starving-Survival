using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Keyboard/mouse input handler that routes player actions to GameBootstrap.
    /// Attach to the same GameObject as GameBootstrap.
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

        /// <summary>Exposed for tests / rebinding docs.</summary>
        public KeyCode WorkbenchKey => _workbenchKey;
        public KeyCode HatchDefenseKey => _hatchDefenseKey;
        public KeyCode MapKey => _mapKey;
        public KeyCode ParleyKey => _parleyKey;
        public KeyCode RadioInterceptKey => _radioInterceptKey;
        public KeyCode RadioTunerPrevKey => _radioTunerPrevKey;
        public KeyCode RadioTunerNextKey => _radioTunerNextKey;
        public KeyCode JournalKey => _journalKey;

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
