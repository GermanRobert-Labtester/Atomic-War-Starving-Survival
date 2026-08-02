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

            // Event choices
            if (Input.GetKeyDown(_eventChoice1Key)) TrySelectChoice(0);
            if (Input.GetKeyDown(_eventChoice2Key)) TrySelectChoice(1);
            if (Input.GetKeyDown(_eventChoice3Key)) TrySelectChoice(2);
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
