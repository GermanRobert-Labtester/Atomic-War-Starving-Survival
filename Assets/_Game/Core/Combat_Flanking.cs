using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public enum CombatLane
    {
        Left,
        Center,
        Right
    }

    [Serializable]
    public class FlankingState
    {
        public string mechanicId = "combat_flanking";
        public float flankingDamageBonus = 0.5f;
        // Serialized positions: parallel lists for save safety
        public List<string> positionKeys = new List<string>();
        public List<int> positionLanes = new List<int>(); // cast to/from CombatLane
    }

    public class Combat_Flanking
    {
        // Events
        public event Action<string, CombatLane> OnLaneChanged;          // survivorId, lane
        public event Action<string, float> OnFlankingBonusApplied;      // survivorId, bonus

        // Internal state
        private readonly FlankingState _state;
        private readonly Dictionary<string, CombatLane> _positions = new Dictionary<string, CombatLane>();

        public Combat_Flanking()
        {
            _state = new FlankingState();
        }

        /// <summary>
        /// Move a survivor to a new combat lane. Takes 1 turn (action cost handled externally).
        /// </summary>
        public void MoveToLane(string survivorId, CombatLane lane)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                Debug.LogWarning("[Combat_Flanking] Invalid survivor id.");
                return;
            }

            _positions[survivorId] = lane;
            OnLaneChanged?.Invoke(survivorId, lane);
        }

        /// <summary>
        /// Returns the current lane for a survivor, or Center if not assigned.
        /// </summary>
        public CombatLane GetLane(string survivorId)
        {
            if (!string.IsNullOrEmpty(survivorId) && _positions.TryGetValue(survivorId, out CombatLane lane))
            {
                return lane;
            }
            return CombatLane.Center;
        }

        /// <summary>
        /// A lane is contested if any enemy occupies the same lane.
        /// </summary>
        public bool IsLaneContested(CombatLane lane, List<string> enemyLanes)
        {
            if (enemyLanes == null) return false;

            string laneName = lane.ToString();
            for (int i = 0; i < enemyLanes.Count; i++)
            {
                if (string.Equals(enemyLanes[i], laneName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns 1.5x if the survivor's lane is uncontested (flanking), 1.0x if contested.
        /// </summary>
        public float GetDamageMultiplier(string survivorId, List<string> enemyLanes)
        {
            CombatLane lane = GetLane(survivorId);
            bool contested = IsLaneContested(lane, enemyLanes);

            float multiplier = contested ? 1.0f : (1.0f + _state.flankingDamageBonus);

            if (!contested)
            {
                OnFlankingBonusApplied?.Invoke(survivorId, multiplier);
            }

            return multiplier;
        }

        public FlankingState CaptureState()
        {
            var saved = new FlankingState
            {
                mechanicId = _state.mechanicId,
                flankingDamageBonus = _state.flankingDamageBonus,
                positionKeys = new List<string>(),
                positionLanes = new List<int>()
            };

            foreach (var kvp in _positions)
            {
                saved.positionKeys.Add(kvp.Key);
                saved.positionLanes.Add((int)kvp.Value);
            }

            return saved;
        }

        public void RestoreState(FlankingState saved)
        {
            if (saved == null) return;

            _state.mechanicId = saved.mechanicId;
            _state.flankingDamageBonus = saved.flankingDamageBonus;

            _positions.Clear();
            if (saved.positionKeys != null && saved.positionLanes != null)
            {
                int count = Mathf.Min(saved.positionKeys.Count, saved.positionLanes.Count);
                for (int i = 0; i < count; i++)
                {
                    _positions[saved.positionKeys[i]] = (CombatLane)saved.positionLanes[i];
                }
            }
        }
    }
}
