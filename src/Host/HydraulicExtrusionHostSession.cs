// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : HydraulicExtrusionHostSession
// Core Source  : Ashfall.Core.Foundry.HydraulicExtrusionEngine
// Purpose      : Thin Godot adapter — commands + availability reads; no math.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Foundry;

namespace AtomicWar.GodotApp
{
    public sealed class HydraulicExtrusionHostSession : HostSessionBase
    {
        public HydraulicExtrusionEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>Power availability 0..100 from the canonical grid. Null-safe 100.</summary>
        public Func<int>? EnergyAvailableBpProvider { get; set; }
        /// <summary>Cooling/water availability 0..100 from the water authority. Null-safe 100.</summary>
        public Func<int>? CoolingAvailableBpProvider { get; set; }

        public HydraulicExtrusionHostSession(HydraulicExtrusionEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public int EnergyAvailableBp() => Math.Clamp(EnergyAvailableBpProvider?.Invoke() ?? 100, 0, 100);
        public int CoolingAvailableBp() => Math.Clamp(CoolingAvailableBpProvider?.Invoke() ?? 100, 0, 100);

        /// <summary>Billet quality for a product, read from its authored material tag.</summary>
        public int BilletQualityBp(string productProfileId)
        {
            var product = System.Catalog.GetProduct(productProfileId);
            if (product == null) return 60;
            return System.Catalog.GetBillet(product.billet_material_tag)?.quality_bp ?? 60;
        }

        public string StartBatch(string productProfileId, string machineId, int units, int day)
        {
            var result = System.StartBatch(
                productProfileId, machineId, units, BilletQualityBp(productProfileId),
                EnergyAvailableBp(), CoolingAvailableBp(), day);
            RaiseStateChanged();
            LastEvent = result.IsSuccess
                ? $"Extrusion batch started ({productProfileId} x{units})."
                : $"Cannot start batch ({result.FailureCode}).";
            return LastEvent;
        }

        public string AdvanceBatch(string batchId)
        {
            var result = System.AdvanceBatch(batchId);
            RaiseStateChanged();
            var batch = System.FindBatch(batchId);
            LastEvent = result.IsSuccess
                ? $"Batch {batchId} phase: {batch?.Phase ?? "unknown"}."
                : $"Cannot advance batch ({result.FailureCode}).";
            return LastEvent;
        }

        public string CompleteBatch(string batchId, double operatorSkill)
        {
            var result = System.CompleteBatch(batchId, operatorSkill);
            RaiseStateChanged();
            var batch = System.FindBatch(batchId);
            LastEvent = result.IsSuccess
                ? $"Batch {batchId} complete: {batch?.QualityClass} ({batch?.FinalQualityScore})."
                : $"Cannot complete batch ({result.FailureCode}).";
            return LastEvent;
        }

        public HydraulicExtrusionState CaptureSave() => System.CaptureState();

        public void RestoreSave(HydraulicExtrusionState? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "Hydraulic extrusion restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            HydraulicExtrusionSaveStore.TrySave(CaptureSave());
        }
    }
}
