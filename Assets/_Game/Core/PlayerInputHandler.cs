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

        // M-6: GetComponentInChildren walks the whole hierarchy, and every
        // Handle*Input method below used to pay that cost fresh every frame
        // via Update(). These panels are fixed scene children that are only
        // ever shown/hidden, never recreated, so each lookup is cached after
        // its first successful resolution.
        private UI.HUD _hud;
        private UI.WorkbenchUI _workbenchUi;
        private UI.TradeScreenUI _tradeScreenUi;
        private UI.EventModalUI _eventModalUi;

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
        [SerializeField] private KeyCode _scavengeDispatchKey = KeyCode.G;
        [SerializeField] private KeyCode _overflowCrateKey = KeyCode.O;
        [SerializeField] private KeyCode _fieldGearLoadoutKey = KeyCode.L;
        [SerializeField] private KeyCode _bunkerRationingKey = KeyCode.T;
        [SerializeField] private KeyCode _waterPurificationKey = KeyCode.Y;
        [SerializeField] private KeyCode _airHeatManagementKey = KeyCode.K;
        [SerializeField] private KeyCode _bunkerMaintenanceKey = KeyCode.N;
        [SerializeField] private KeyCode _survivorTaskBoardKey = KeyCode.V;
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
        public KeyCode ScavengeDispatchKey => _scavengeDispatchKey;
        public KeyCode OverflowCrateKey => _overflowCrateKey;
        public KeyCode FieldGearLoadoutKey => _fieldGearLoadoutKey;
        public KeyCode BunkerRationingKey => _bunkerRationingKey;
        public KeyCode WaterPurificationKey => _waterPurificationKey;
        public KeyCode AirHeatManagementKey => _airHeatManagementKey;
        public KeyCode BunkerMaintenanceKey => _bunkerMaintenanceKey;
        public KeyCode SurvivorTaskBoardKey => _survivorTaskBoardKey;
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

        private UI.HUD GetHud()
        {
            if (_hud == null && _bootstrap != null)
                _hud = _bootstrap.GetComponentInChildren<UI.HUD>(true);
            return _hud;
        }

        private UI.WorkbenchUI GetWorkbenchUi()
        {
            if (_workbenchUi == null && _bootstrap != null)
            {
                _workbenchUi = _bootstrap.GetComponentInChildren<UI.WorkbenchUI>(true);
                if (_workbenchUi == null)
                {
                    // HUD may own the WorkbenchUI
                    var hud = GetHud();
                    _workbenchUi = hud != null ? hud.WorkbenchUI : null;
                }
            }
            return _workbenchUi;
        }

        private UI.TradeScreenUI GetTradeScreenUi()
        {
            if (_tradeScreenUi == null && _bootstrap != null)
            {
                var hud = GetHud();
                _tradeScreenUi = hud != null ? hud.TradeScreenUI : null;
                if (_tradeScreenUi == null)
                    _tradeScreenUi = _bootstrap.GetComponentInChildren<UI.TradeScreenUI>(true);
            }
            return _tradeScreenUi;
        }

        private UI.EventModalUI GetEventModalUi()
        {
            if (_eventModalUi == null)
                _eventModalUi = GetComponentInChildren<UI.EventModalUI>();
            return _eventModalUi;
        }

        /// <summary>
        /// Key dispatch, in strict precedence order. Each helper handles one family
        /// of bindings; the ones returning bool consume the frame when they fire, so
        /// a number key cannot reach both a workbench line and an event choice.
        /// </summary>
        private void Update()
        {
            if (_bootstrap == null) return;

            HandleSessionKeys();

            // Internal Horror panels take number/Esc priority over workbench & events.
            if (HandleInternalHorrorPanelInput()) return;

            HandleScreenKeys();
            if (HandleScavengeDispatchInput()) return;
            if (HandleOverflowCrateInput()) return;
            if (HandleFieldGearLoadoutInput()) return;
            if (HandleBunkerRationingInput()) return;
            if (HandleWaterPurificationInput()) return;
            if (HandleAirHeatManagementInput()) return;
            if (HandleBunkerMaintenanceInput()) return;
            if (HandleSurvivorTaskBoardInput()) return;
            HandleInventoryKeys();

            if (HandleModalNumberKeys()) return;

            HandleConsumableKeys();

            // Event choices (when modal open; workbench numbers take priority above)
            if (Input.GetKeyDown(_eventChoice1Key)) TrySelectChoice(0);
            if (Input.GetKeyDown(_eventChoice2Key)) TrySelectChoice(1);
            if (Input.GetKeyDown(_eventChoice3Key)) TrySelectChoice(2);
        }

        /// <summary>Pause toggle and quicksave/quickload.</summary>
        private void HandleSessionKeys()
        {
            if (Input.GetKeyDown(_pauseKey))
            {
                if (_bootstrap.GameState.Phase == GamePhase.Running)
                    _bootstrap.PauseGame();
                else if (_bootstrap.GameState.Phase == GamePhase.Paused)
                    _bootstrap.ResumeGame();
            }

            if (Input.GetKeyDown(_quickSaveKey))
                _bootstrap.SaveGame("quicksave");
            if (Input.GetKeyDown(_quickLoadKey))
                _bootstrap.LoadGame("quicksave");
        }

        /// <summary>Screen toggles and the simulation-speed toggle.</summary>
        private void HandleScreenKeys()
        {
            if (Input.GetKeyDown(_workbenchKey))
                _bootstrap.ToggleWorkbench();
            if (Input.GetKeyDown(_hatchDefenseKey))
                _bootstrap.ToggleHatchDefense();
            if (Input.GetKeyDown(_scavengeDispatchKey))
                _bootstrap.ToggleScavengeDispatch();
            if (Input.GetKeyDown(_overflowCrateKey))
                _bootstrap.ToggleOverflowCrate();
            if (Input.GetKeyDown(_fieldGearLoadoutKey))
                _bootstrap.ToggleFieldGearLoadout();
            if (Input.GetKeyDown(_bunkerRationingKey))
                _bootstrap.ToggleBunkerRationing();
            if (Input.GetKeyDown(_waterPurificationKey))
                _bootstrap.ToggleWaterPurification();
            if (Input.GetKeyDown(_airHeatManagementKey))
                _bootstrap.ToggleAirHeatManagement();
            if (Input.GetKeyDown(_bunkerMaintenanceKey))
                _bootstrap.ToggleBunkerMaintenance();
            if (Input.GetKeyDown(_survivorTaskBoardKey))
                _bootstrap.ToggleSurvivorTaskBoard();
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
        }

        /// <summary>
        /// The dispatch board owns comma/period/Enter while it is open. This
        /// prevents the inventory strip from also activating on Enter in the
        /// same frame, which would make sending someone outside ambiguous.
        /// </summary>
        private bool HandleScavengeDispatchInput()
        {
            var hud = GetHud();
            var dispatch = hud != null ? hud.ScavengeDispatchHUD : null;
            if (dispatch == null || !dispatch.IsOpen) return false;

            if (Input.GetKeyDown(_closePanelKey))
            {
                _bootstrap.ToggleScavengeDispatch();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                _bootstrap.SelectPreviousScavengeLocation();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                _bootstrap.SelectNextScavengeLocation();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    _bootstrap.SelectPreviousScavengeSurvivor();
                else
                    _bootstrap.SelectNextScavengeSurvivor();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                _bootstrap.DispatchSelectedScavenge();
                return true;
            }
            return false;
        }

        /// <summary>
        /// The receiving crate owns comma/period/Enter while open, preventing an
        /// Enter transfer from also activating the inventory-strip selection.
        /// </summary>
        private bool HandleOverflowCrateInput()
        {
            var hud = GetHud();
            var crate = hud != null ? hud.OverflowCrateHUD : null;
            if (crate == null || !crate.IsOpen) return false;

            if (Input.GetKeyDown(_closePanelKey))
            {
                _bootstrap.ToggleOverflowCrate();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                _bootstrap.SelectPreviousOverflowCrateItem();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                _bootstrap.SelectNextOverflowCrateItem();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                _bootstrap.TransferSelectedOverflowCrateItem();
                return true;
            }
            return false;
        }

        /// <summary>Field gear owns its selection/equip/stow bindings while open.</summary>
        private bool HandleFieldGearLoadoutInput()
        {
            var hud = GetHud();
            var loadout = hud != null ? hud.FieldGearLoadoutHUD : null;
            if (loadout == null || !loadout.IsOpen) return false;

            if (Input.GetKeyDown(_closePanelKey))
            {
                _bootstrap.ToggleFieldGearLoadout();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                _bootstrap.SelectPreviousFieldGearCandidate();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                _bootstrap.SelectNextFieldGearCandidate();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _bootstrap.ToggleFieldGearStowSlot();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                _bootstrap.UnequipSelectedFieldGear();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                _bootstrap.EquipSelectedFieldGear();
                return true;
            }
            return false;
        }

        /// <summary>The ration terminal owns tab and comma/period while it is open.</summary>
        private bool HandleBunkerRationingInput()
        {
            var hud = GetHud();
            var rationing = hud != null ? hud.BunkerRationingHUD : null;
            if (rationing == null || !rationing.IsOpen) return false;

            if (Input.GetKeyDown(_closePanelKey))
            {
                _bootstrap.ToggleBunkerRationing();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _bootstrap.ToggleSelectedRationResource();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                _bootstrap.DecreaseSelectedRationLevel();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                _bootstrap.IncreaseSelectedRationLevel();
                return true;
            }
            return false;
        }

        /// <summary>The water terminal owns comma/period while it is open.</summary>
        private bool HandleWaterPurificationInput()
        {
            var hud = GetHud();
            var terminal = hud != null ? hud.WaterPurificationHUD : null;
            if (terminal == null || !terminal.IsOpen) return false;

            if (Input.GetKeyDown(_closePanelKey))
            {
                _bootstrap.ToggleWaterPurification();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                _bootstrap.CycleWaterPurifierQueuePrevious();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                _bootstrap.CycleWaterPurifierQueueNext();
                return true;
            }
            return false;
        }

        /// <summary>The climate terminal owns Tab, comma/period, and Enter while it is open.</summary>
        private bool HandleAirHeatManagementInput()
        {
            var hud = GetHud();
            var terminal = hud != null ? hud.AirHeatManagementHUD : null;
            if (terminal == null || !terminal.IsOpen) return false;

            if (Input.GetKeyDown(_closePanelKey))
            {
                _bootstrap.ToggleAirHeatManagement();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _bootstrap.ToggleSelectedAirHeatLoad();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                _bootstrap.DecreaseSelectedAirHeatPriority();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                _bootstrap.IncreaseSelectedAirHeatPriority();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                _bootstrap.ToggleSelectedAirHeatRequest();
                return true;
            }
            return false;
        }

        /// <summary>The repair terminal owns target, worker, priority, and Enter while open.</summary>
        private bool HandleBunkerMaintenanceInput()
        {
            var hud = GetHud();
            var terminal = hud != null ? hud.BunkerMaintenanceHUD : null;
            if (terminal == null || !terminal.IsOpen) return false;

            if (Input.GetKeyDown(_closePanelKey))
            {
                _bootstrap.ToggleBunkerMaintenance();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                bool backwards = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                if (backwards)
                    _bootstrap.SelectPreviousBunkerMaintenanceSurvivor();
                else
                    _bootstrap.SelectNextBunkerMaintenanceTarget();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                _bootstrap.DecreaseBunkerMaintenancePriority();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                _bootstrap.IncreaseBunkerMaintenancePriority();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                _bootstrap.CancelBunkerMaintenanceRepair();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                _bootstrap.RepairSelectedBunkerMaintenanceTarget();
                return true;
            }
            return false;
        }

        /// <summary>The allocation board owns its priority and cancellation controls while open.</summary>
        private bool HandleSurvivorTaskBoardInput()
        {
            var hud = GetHud();
            var board = hud != null ? hud.SurvivorTaskBoardHUD : null;
            if (board == null || !board.IsOpen) return false;

            if (Input.GetKeyDown(_closePanelKey))
            {
                _bootstrap.ToggleSurvivorTaskBoard();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    _bootstrap.SelectPreviousTaskBoardShift();
                else
                    _bootstrap.SelectNextTaskBoardShift();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _bootstrap.SelectPreviousTaskBoardSurvivor();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _bootstrap.SelectNextTaskBoardSurvivor();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                _bootstrap.DecreaseTaskBoardPriority();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                _bootstrap.IncreaseTaskBoardPriority();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                _bootstrap.CancelTaskBoardActiveTask();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                _bootstrap.ApproveTopTaskBoardShiftRecommendation();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                _bootstrap.AssignSelectedTaskBoardShift();
                return true;
            }
            return false;
        }

        /// <summary>Inventory strip focus/activation, plus the corpse-dispose shortcut.</summary>
        private void HandleInventoryKeys()
        {
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
        }

        /// <summary>
        /// Bindings that belong to an open modal and consume the frame when they
        /// fire: parley during trade, and workbench/hatch-install lines 1-9.
        /// Returns true when the frame was consumed.
        /// </summary>
        private bool HandleModalNumberKeys()
        {
            // Trade: demand parley / surrender after a hatch repel
            if (Input.GetKeyDown(_parleyKey) && IsTradeOpen())
            {
                _bootstrap.DemandTradeParley();
                return true;
            }

            if (!IsWorkbenchOpen()) return false;

            for (int i = 0; i < 9; i++)
            {
                if (!Input.GetKeyDown(KeyCode.Alpha1 + i) && !Input.GetKeyDown(KeyCode.Keypad1 + i))
                    continue;
                _bootstrap.ExecuteWorkbenchLine(i);
                return true;
            }
            return false;
        }

        /// <summary>Direct-consume shortcuts, applied to the primary survivor.</summary>
        private void HandleConsumableKeys()
        {
            if (_bootstrap.Survivors == null || _bootstrap.Survivors.Count == 0) return;
            var primary = _bootstrap.Survivors[0];
            if (!primary.IsAlive) return;

            if (Input.GetKeyDown(_consumeFoodKey))
                TryConsumeByType(primary, ItemType.Food);
            if (Input.GetKeyDown(_consumeWaterKey))
                TryConsumeByType(primary, ItemType.Water);
            if (Input.GetKeyDown(_consumeIodineKey))
                TryConsumeByType(primary, ItemType.Iodine);
            if (Input.GetKeyDown(_consumeAntiRadKey))
                TryConsumeByType(primary, ItemType.AntiRad);
        }

        /// <summary>
        /// Corpse dispose / fire fight-seal-extinguish panels: [1] [2] [3] [Esc].
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
            else if (digit == 3)
                _bootstrap.SelectExtinguishFire();
            return true;
        }

        private bool IsWorkbenchOpen()
        {
            var wb = GetWorkbenchUi();
            return wb != null && wb.IsOpen;
        }

        private bool IsTradeOpen()
        {
            var trade = GetTradeScreenUi();
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
            var hud = GetEventModalUi();
            if (hud != null && hud.IsOpen)
            {
                hud.SelectChoice(index, _bootstrap.EventRunner);
            }
        }
    }
}
