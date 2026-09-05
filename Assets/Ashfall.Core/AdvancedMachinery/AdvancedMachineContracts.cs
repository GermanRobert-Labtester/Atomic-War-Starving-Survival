// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.AdvancedMachinery
{
    /// <summary>
    /// Lifecycle state for advanced industrial and specialized expedition processes.
    /// Prevents ambiguous or conflicting boolean state combinations.
    /// </summary>
    public enum ProcessState
    {
        Idle,
        Queued,
        Running,
        Paused,
        Completed,
        Failed,
        MaintenanceRequired
    }

    /// <summary>
    /// Common operational contract for advanced machinery across industrial and expedition subsystems.
    /// </summary>
    public interface IAdvancedMachineOperation
    {
        bool IsOperational { get; }
        float Condition01 { get; }
        string ActiveJobId { get; }
        ProcessState State { get; }
    }

    /// <summary>
    /// Host-projected operator capability delivered to Core simulation engines.
    /// Core engines evaluate numeric capability without direct coupling to host survivor lifecycle.
    /// </summary>
    [Serializable]
    public sealed class AdvancedMachineOperatorContext
    {
        public string SurvivorId { get; set; } = string.Empty;
        public float RelevantSkillProgress01 { get; set; } = 0.5f;
        public List<string> TraitIds { get; set; } = new List<string>();
        public float FatigueModifier { get; set; } = 1.0f;
        public float InjuryModifier { get; set; } = 1.0f;

        public float EffectiveEfficiency =>
            Math.Clamp(RelevantSkillProgress01 * FatigueModifier * InjuryModifier, 0.1f, 2.5f);
    }

    /// <summary>
    /// Power supply context projected from PowerGrid to heavy shelter machines.
    /// </summary>
    [Serializable]
    public sealed class PowerSupplyContext
    {
        public float AvailablePowerKw { get; set; }
        public bool PowerStable { get; set; } = true;
        public float BrownoutSeverity { get; set; } = 0.0f; // 0.0 = none, 1.0 = total blackout

        public static PowerSupplyContext Full(float kw) => new PowerSupplyContext
        {
            AvailablePowerKw = kw,
            PowerStable = true,
            BrownoutSeverity = 0.0f
        };

        public static PowerSupplyContext Throttled(float availableKw, float requiredKw) => new PowerSupplyContext
        {
            AvailablePowerKw = availableKw,
            PowerStable = false,
            BrownoutSeverity = Math.Clamp(1.0f - (availableKw / Math.Max(requiredKw, 0.001f)), 0.0f, 1.0f)
        };

        public static PowerSupplyContext Unpowered() => new PowerSupplyContext
        {
            AvailablePowerKw = 0f,
            PowerStable = false,
            BrownoutSeverity = 1.0f
        };
    }

    /// <summary>
    /// One line of a machine job's inventory demand. Passed as a complete list so the
    /// host inventory can validate every input before consuming anything (all-or-nothing).
    /// </summary>
    [Serializable]
    public sealed class InventoryDemand
    {
        public string ItemId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;

        public InventoryDemand() { }

        public InventoryDemand(string itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}
