// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : SofcPowerHostSession
// Core Source  : Ashfall.Core.Shelter.SofcElectrochemistryEngine
// Purpose      : Thin Godot adapter — commissioning ticks + typed seams to the
//                canonical power-grid and thermal owners. No math, no truth.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public sealed class SofcPowerHostSession : HostSessionBase
    {
        public SofcElectrochemistryEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>Grid contribution registration seam — bound to PowerGridSystem.SetGenerationContribution.</summary>
        public Action<string, float>? ContributionApplier { get; set; }
        /// <summary>Fuel draw from the canonical fuel/inventory owner. Returns true when fuel was available.</summary>
        public Func<float, bool>? FuelConsumer { get; set; }
        /// <summary>Waste-heat routing seam — bound to ShelterThermalSystem.AddAuxiliaryHeat(roomId, kW).</summary>
        public Action<string, float>? WasteHeatRouter { get; set; }
        /// <summary>Room that receives CHP heat this tick (host-owned allocation policy).</summary>
        public Func<string>? WasteHeatTargetRoomProvider { get; set; }
        /// <summary>Fuel quality for this tick, derived from consumed item IDs by the inventory owner.</summary>
        public Func<string>? FuelQualityProvider { get; set; }
        /// <summary>Operator engineering skill 0..100 from the canonical survivor owners.</summary>
        public Func<float>? EngineeringSkillProvider { get; set; }

        public SofcGenerationResult? LastTickResult { get; private set; }

        public SofcPowerHostSession(SofcElectrochemistryEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public string Install(string profileId, bool partsAvailable)
        {
            var result = System.Install(profileId, partsAvailable);
            RaiseStateChanged();
            LastEvent = result.IsSuccess
                ? $"SOFC stack installed ({profileId})."
                : $"Cannot install SOFC stack ({result.FailureCode}).";
            return LastEvent;
        }

        public string StartPreheat(bool fuelAvailable)
        {
            var result = System.StartPreheat(fuelAvailable);
            RaiseStateChanged();
            LastEvent = result.IsSuccess
                ? "SOFC preheat cycle started."
                : $"Cannot start preheat ({result.FailureCode}).";
            return LastEvent;
        }

        public string Shutdown()
        {
            var result = System.Shutdown();
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "SOFC controlled shutdown started." : $"Cannot shut down ({result.FailureCode}).";
            return LastEvent;
        }

        public string PerformMaintenance(bool partsAvailable)
        {
            var result = System.PerformMaintenance(partsAvailable, EngineeringSkillProvider?.Invoke() ?? 0f);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "SOFC stack maintained." : $"Cannot maintain stack ({result.FailureCode}).";
            return LastEvent;
        }

        public string RebuildStack(string gradeProfileId, bool partsAvailable)
        {
            var result = System.RebuildStack(gradeProfileId, partsAvailable);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "SOFC stack rebuilt." : $"Cannot rebuild stack ({result.FailureCode}).";
            return LastEvent;
        }

        /// <summary>
        /// One shelter power tick: advances the plant, dispatches the
        /// contribution to the canonical grid, draws fuel from the canonical
        /// consumer, and routes waste heat through the canonical thermal seam.
        /// The grid remains the dispatch authority; this session only reports.
        /// </summary>
        public SofcGenerationResult? TickDay(int day)
        {
            bool fuelAvailable = FuelConsumer == null || FuelConsumer(0f);
            var input = new SofcTickInput
            {
                FuelAvailable = fuelAvailable,
                FuelQualityClass = FuelQualityProvider?.Invoke() ?? SofcPowerCatalog.QualityClean,
                // Baseload dispatch: request the plant's rated capacity;
                // the engine clamps the actual dispatch to what it can give.
                RequestedOutputKw = System.Profile?.rated_power_kw ?? 0f,
                EngineeringSkillLevel = EngineeringSkillProvider?.Invoke() ?? 0f
            };
            var result = System.AdvanceTick(input);
            LastTickResult = result;
            RaiseStateChanged();

            if (result.AvailableOutputKw > 0f && ContributionApplier != null)
                ContributionApplier(SofcElectrochemistryEngine.SystemId, result.DispatchedOutputKw * 1000f);

            // Fuel draw is reported for the next tick's availability probe;
            // the canonical consumer decides whether the fuel existed.
            float draw = result.FuelUnitsRequested;
            if (draw > 0f && FuelConsumer != null)
                result.FuelSatisfiedByHost = FuelConsumer(draw);

            if (result.WasteHeatKw > 0f && WasteHeatRouter != null)
            {
                var room = WasteHeatTargetRoomProvider?.Invoke();
                if (!string.IsNullOrEmpty(room))
                    WasteHeatRouter(room!, result.WasteHeatKw);
            }

            LastEvent = result.FailureCode != null
                ? $"SOFC tick: {result.FailureCode}"
                : $"SOFC {result.DispatchedOutputKw:0.0} kW dispatched, {result.WasteHeatKw:0.0} kW waste heat ({result.StackHealthClass}).";
            return result;
        }
    
        public SolidOxideFuelCellState CaptureSave() => System.CaptureState();

        public void RestoreSave(SolidOxideFuelCellState? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "State restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            SofcPowerSaveStore.TrySave(CaptureSave());
        }
}
}
