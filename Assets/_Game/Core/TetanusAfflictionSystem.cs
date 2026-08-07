using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TetanusState
    {
        public string survivorId;
        public bool hasTetanus = false;
        public bool hasLockjaw = false;
        public bool isSolidFoodBlocked = false;
    }

    /// <summary>
    /// Prompt #393: System: Tetanus (Lockjaw).
    /// Contracted from ScrapWeapons or CityRuin damage. Progresses to Lockjaw,
    /// blocking solid food consumption (requires liquid Soup feeding to prevent starvation).
    /// </summary>
    
    [Serializable]
    public class TetanusAfflictionSystemSave
    {
        public string systemId = "tetanus_affliction_system";

        public List<TetanusState> tetanusMap = new List<TetanusState>();
    }
public class TetanusAfflictionSystem
    {
        private readonly Dictionary<string, TetanusState> _tetanusMap = new Dictionary<string, TetanusState>();

        public event Action<string> OnTetanusContracted;
        public event Action<string> OnLockjawProgressed;

        public IReadOnlyDictionary<string, TetanusState> TetanusMap => _tetanusMap;

        public void ContractTetanus(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            var state = new TetanusState { survivorId = survivorId, hasTetanus = true };
            _tetanusMap[survivorId] = state;

            OnTetanusContracted?.Invoke(survivorId);
        }

        public void ProgressToLockjaw(string survivorId)
        {
            if (_tetanusMap.TryGetValue(survivorId, out var state) && state.hasTetanus)
            {
                state.hasLockjaw = true;
                state.isSolidFoodBlocked = true;

                OnLockjawProgressed?.Invoke(survivorId);
            }
        }

        public bool CanEatFoodItem(string survivorId, bool isLiquidSoup)
        {
            if (_tetanusMap.TryGetValue(survivorId, out var state) && state.hasLockjaw)
            {
                return isLiquidSoup; // Blocked unless liquid soup
            }
            return true;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public TetanusAfflictionSystemSave CaptureState() => new TetanusAfflictionSystemSave
        {
            tetanusMap = SaveMap.Capture(_tetanusMap),
        };

        public void RestoreState(TetanusAfflictionSystemSave saved) =>
            SaveMap.Restore(_tetanusMap, saved?.tetanusMap, e => e.survivorId);

}
}
