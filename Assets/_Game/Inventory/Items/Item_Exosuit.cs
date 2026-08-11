using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class ExosuitState
    {
        public string itemId = "item_exosuit";
        public float carryWeightBonus = 500f;
        public float ballisticProtection = 0.95f;
        public bool isLockedUp = false;
        // Equipped survivor tracking
        public List<string> equippedSurvivorIds = new List<string>();
        public List<bool> entombedFlags = new List<bool>();
    }

    /// <summary>
    /// Salvaged Power Armor — a pre-war exosuit providing massive carry-weight
    /// and ballistic protection. Requires FusionCores to operate; if the core
    /// dies while worn the suit locks up and the survivor is entombed inside.
    /// Prompt #789: Item_Exosuit
    /// </summary>
    public class Item_Exosuit
    {
        // -- Constants --
        public const float CarryWeightBonus = 500f;
        public const float BallisticProtection = 0.95f;

        // -- Events --
        public event Action<string> OnExosuitEquipped;   // survivorId
        public event Action<string> OnCoreDepleted;      // survivorId
        public event Action<string> OnSurvivorEntombed;  // survivorId

        // -- State --
        // survivorId -> entombed flag
        private Dictionary<string, bool> _entombedSurvivors = new Dictionary<string, bool>();
        private bool _isLockedUp = false;

        // -- Public API --

        /// <summary>
        /// Equips the exosuit on a survivor. Requires a functional fusion core.
        /// </summary>
        public void Equip(string survivorId, bool hasFusionCore)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!hasFusionCore)
            {
                Debug.LogWarning("[Item_Exosuit] Cannot equip without a fusion core.");
                return;
            }
            if (!_entombedSurvivors.ContainsKey(survivorId))
            {
                _entombedSurvivors[survivorId] = false;
            }
            OnExosuitEquipped?.Invoke(survivorId);
        }

        /// <summary>
        /// Called each hour while the suit is worn. If the core charge drops to
        /// zero or below, the suit locks up and the survivor is entombed.
        /// </summary>
        public void TickHour(string survivorId, float coreCharge)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (coreCharge <= 0f)
            {
                _isLockedUp = true;
                _entombedSurvivors[survivorId] = true;
                OnCoreDepleted?.Invoke(survivorId);
                OnSurvivorEntombed?.Invoke(survivorId);
            }
        }

        /// <summary>
        /// Rescues an entombed survivor by forcibly removing the exosuit.
        /// </summary>
        public void Rescue(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_entombedSurvivors.ContainsKey(survivorId) || !_entombedSurvivors[survivorId])
            {
                Debug.LogWarning($"[Item_Exosuit] Survivor '{survivorId}' is not entombed.");
                return;
            }
            _entombedSurvivors[survivorId] = false;
            _isLockedUp = false;
        }

        /// <summary>
        /// Returns true if the given survivor is currently entombed in the suit.
        /// </summary>
        public bool IsEntombed(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            return _entombedSurvivors.TryGetValue(survivorId, out var entombed) && entombed;
        }

        /// <summary>Returns the carry-weight bonus provided by the exosuit.</summary>
        public float GetCarryWeightBonus() => CarryWeightBonus;

        /// <summary>Returns the ballistic protection factor (0–1).</summary>
        public float GetBallisticProtection() => BallisticProtection;

        // -- Save / Load --

        public ExosuitState CaptureState()
        {
            var state = new ExosuitState
            {
                itemId = "item_exosuit",
                carryWeightBonus = CarryWeightBonus,
                ballisticProtection = BallisticProtection,
                isLockedUp = _isLockedUp,
                equippedSurvivorIds = new List<string>(),
                entombedFlags = new List<bool>()
            };
            foreach (var kvp in _entombedSurvivors)
            {
                state.equippedSurvivorIds.Add(kvp.Key);
                state.entombedFlags.Add(kvp.Value);
            }
            return state;
        }

        public void RestoreState(ExosuitState saved)
        {
            _entombedSurvivors.Clear();
            if (saved == null) return;
            _isLockedUp = saved.isLockedUp;
            // Either list is null when the save omitted it explicitly; guard before Count.
            if (saved.equippedSurvivorIds == null || saved.entombedFlags == null) return;
            int count = Mathf.Min(saved.equippedSurvivorIds.Count, saved.entombedFlags.Count);
            for (int i = 0; i < count; i++)
            {
                string id = saved.equippedSurvivorIds[i];
                if (string.IsNullOrEmpty(id)) continue;
                _entombedSurvivors[id] = saved.entombedFlags[i];
            }
        }
    }
}
