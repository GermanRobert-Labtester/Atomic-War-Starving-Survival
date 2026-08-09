using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Slave Labor Camps (Prompt #77). Heavily fortified Faction nodes where
    /// the player can trade supplies to buy the freedom of slaves. These
    /// survivors start with permanent TraumaAfflictions and 0 Morale, requiring
    /// massive rehabilitation. A late-game resource sink to save lives.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class LaborCampSystem
    {
        /// <summary>Node ids that are labor camp sites.</summary>
        private readonly HashSet<string> _laborCampNodeIds = new HashSet<string>();

        /// <summary>Slaves freed per camp.</summary>
        private readonly Dictionary<string, int> _slavesFreedPerCamp = new Dictionary<string, int>();
        private int _totalSlavesFreed;

        /// <summary>Cost to free one slave: clean water units.</summary>
        public const int SlaveCostWater = 20;

        /// <summary>Cost to free one slave: food units.</summary>
        public const int SlaveCostFood = 10;

        /// <summary>Cost to free one slave: anti-rad units.</summary>
        public const int SlaveCostAntiRad = 5;

        /// <summary>Slaves available per camp (initial).</summary>
        public const int SlavesPerCamp = 3;

        /// <summary>Morale boost to bunker when a slave is freed.</summary>
        public const float FreedSlaveMoraleBoost = 10f;

        /// <summary>Traumas applied to freed slaves.</summary>
        public static readonly string[] SlavesTraumas = { "whipping_scars", "forced_labor_trauma", "starvation_trauma" };

        /// <summary>Event id for slave purchase event.</summary>
        public const string SlavePurchaseEventId = "labor_camp_purchase";

        // -- Events --
        public event Action<string, int> OnSlavesFreed; // campNodeId, count
        public event Action<Survivors.Survivor> OnSlaveRecruited;

        public IReadOnlyCollection<string> LaborCampNodeIds => _laborCampNodeIds;
        public int TotalSlavesFreed => _totalSlavesFreed;

        public LaborCampSystem() { }

        /// <summary>Mark a node as a labor camp.</summary>
        public void SetLaborCamp(string nodeId, bool isCamp)
        {
            if (string.IsNullOrEmpty(nodeId)) return;
            if (isCamp) _laborCampNodeIds.Add(nodeId);
            else _laborCampNodeIds.Remove(nodeId);
        }

        public bool IsLaborCamp(string nodeId)
        {
            return !string.IsNullOrEmpty(nodeId) && _laborCampNodeIds.Contains(nodeId);
        }

        /// <summary>Slaves remaining at a camp node.</summary>
        public int GetSlavesRemaining(string nodeId)
        {
            if (!IsLaborCamp(nodeId)) return 0;
            int freed = _slavesFreedPerCamp.TryGetValue(nodeId, out int f) ? f : 0;
            return Mathf.Max(0, SlavesPerCamp - freed);
        }

        /// <summary>
        /// Purchase one slave's freedom from a labor camp.
        /// Returns true if successful. The caller creates the survivor.
        /// </summary>
        public bool PurchaseSlave(string nodeId,
            Func<string, int, bool> consumeItem)
        {
            if (!IsLaborCamp(nodeId)) return false;
            if (GetSlavesRemaining(nodeId) <= 0) return false;
            if (consumeItem == null) return false;

            // Pay the cost.
            if (!consumeItem("clean_water", SlaveCostWater)) return false;
            if (!consumeItem("canned_food", SlaveCostFood)) return false;
            if (!consumeItem("anti_rad", SlaveCostAntiRad)) return false;

            // Track.
            int existing = _slavesFreedPerCamp.TryGetValue(nodeId, out int f) ? f : 0;
            _slavesFreedPerCamp[nodeId] = existing + 1;
            _totalSlavesFreed++;

            OnSlavesFreed?.Invoke(nodeId, 1);
            return true;
        }

        /// <summary>
        /// Create a freed slave survivor with appropriate traumas and 0 morale.
        /// </summary>
        public Survivors.Survivor CreateFreedSlave(string name, string id)
        {
            var sv = new Survivors.Survivor
            {
                Id = id,
                DisplayName = name,
                State = Survivors.SurvivorState.Idle,
                RiskBias = Survivors.RiskBiasTrait.Fatalist
            };
            sv.Needs.Hunger = 80f;
            sv.Needs.Thirst = 70f;
            sv.Needs.Fatigue = 90f;
            sv.Needs.Warmth = 40f;
            sv.Needs.Morale = 0f;
            SurvivorNeedWrite.SetHealth(sv, 40f);

            if (sv.Traumas == null) sv.Traumas = new List<string>();
            for (int i = 0; i < SlavesTraumas.Length; i++)
                sv.Traumas.Add(SlavesTraumas[i]);

            OnSlaveRecruited?.Invoke(sv);
            return sv;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public LaborCampSave CaptureState()
        {
            var nodeIds = new string[_laborCampNodeIds.Count];
            _laborCampNodeIds.CopyTo(nodeIds);
            var campKeys = new string[_slavesFreedPerCamp.Count];
            var campValues = new int[_slavesFreedPerCamp.Count];
            int i = 0;
            foreach (var kv in _slavesFreedPerCamp) { campKeys[i] = kv.Key; campValues[i] = kv.Value; i++; }
            return new LaborCampSave
            {
                LaborCampNodeIds = nodeIds,
                FreedKeys = campKeys,
                FreedValues = campValues,
                TotalSlavesFreed = _totalSlavesFreed
            };
        }

        public void RestoreState(LaborCampSave save)
        {
            _laborCampNodeIds.Clear();
            _slavesFreedPerCamp.Clear();
            _totalSlavesFreed = 0;
            if (save == null) return;
            _totalSlavesFreed = save.TotalSlavesFreed;
            if (save.LaborCampNodeIds != null)
                for (int i = 0; i < save.LaborCampNodeIds.Length; i++)
                    if (!string.IsNullOrEmpty(save.LaborCampNodeIds[i]))
                        _laborCampNodeIds.Add(save.LaborCampNodeIds[i]);
            if (save.FreedKeys != null)
                for (int i = 0; i < save.FreedKeys.Length; i++)
                    if (!string.IsNullOrEmpty(save.FreedKeys[i]))
                        _slavesFreedPerCamp[save.FreedKeys[i]] =
                            save.FreedValues != null && i < save.FreedValues.Length
                                ? save.FreedValues[i] : 0;
        }
    }

    [Serializable]
    public class LaborCampSave
    {
        public string[] LaborCampNodeIds;
        public string[] FreedKeys;
        public int[] FreedValues;
        public int TotalSlavesFreed;
    }
}
