// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// SOFC operating modes (Plan 122). No instantaneous on/off: the stack
    /// must preheat, stabilize, and cool down. Derated is Online with a worn
    /// stack; Faulted requires maintenance before any restart.
    /// </summary>
    public enum SofcOperatingMode
    {
        Offline,
        Preheating,
        Stabilizing,
        Online,
        Derated,
        CoolingDown,
        Faulted
    }

    /// <summary>Typed failure codes (plan §12). Host supplies localized prose.</summary>
    public static class SofcFailureCodes
    {
        public const string FuelUnavailable = "fuel_unavailable";
        public const string FuelQualityInsufficient = "fuel_quality_insufficient";
        public const string StackUnavailable = "stack_unavailable";
        public const string StackTooCold = "stack_too_cold";
        public const string StackFaulted = "stack_faulted";
        public const string SealIntegrityCritical = "seal_integrity_critical";
        public const string PowerDispatchRejected = "power_dispatch_rejected";
        public const string MaintenanceRequired = "maintenance_required";
    }

    /// <summary>Player-facing stack health classes (bp: 10000 = pristine).</summary>
    public static class SofcStackHealth
    {
        public const string Excellent = "excellent";
        public const string Good = "good";
        public const string Worn = "worn";
        public const string Critical = "critical";

        public static string Classify(int healthBp) => healthBp >= 9000 ? Excellent
            : healthBp >= 7000 ? Good
            : healthBp >= 4000 ? Worn
            : Critical;
    }

    /// <summary>Authoritative SOFC plant state. Owned by this engine only.</summary>
    public sealed class SolidOxideFuelCellState
    {
        public string StackProfileId { get; set; } = string.Empty;
        public SofcOperatingMode Mode { get; set; } = SofcOperatingMode.Offline;
        public int TicksInMode { get; set; }
        /// <summary>Normalized stack thermal level 0..1 against the profile band.</summary>
        public float ThermalLevel { get; set; }
        public int StackHealthBp { get; set; } = 10000;
        public int SealIntegrityBp { get; set; } = 10000;
        public int OperatingTicks { get; set; }
        public int ThermalCycles { get; set; }
        public string FaultCode { get; set; } = string.Empty;
        public string LastFuelQualityClass { get; set; } = string.Empty;
        public float LastAvailableOutputKw { get; set; }
        public float LastFuelUnitsRequested { get; set; }
        public float LastWasteHeatUnits { get; set; }
        public float LastWasteHeatKw { get; set; }
    }

    /// <summary>Per-tick inputs supplied by the host. No inventory truth here.</summary>
    public sealed class SofcTickInput
    {
        public bool FuelAvailable { get; set; }
        /// <summary>Closed vocabulary: dirty | treated | clean (host derives from consumed items).</summary>
        public string FuelQualityClass { get; set; } = SofcPowerCatalog.QualityClean;
        /// <summary>Dispatch request in kW; clamped to available output by the engine.</summary>
        public float RequestedOutputKw { get; set; }
        /// <summary>Operator engineering skill 0..100 — bounded modifier, never immunity.</summary>
        public float EngineeringSkillLevel { get; set; }
    }

    /// <summary>Typed per-tick outcome. Host applies grid/thermal/seam effects.</summary>
    public sealed class SofcGenerationResult
    {
        public SofcOperatingMode Mode { get; set; }
        public float AvailableOutputKw { get; set; }
        public float DispatchedOutputKw { get; set; }
        public bool DispatchClamped { get; set; }
        public float FuelUnitsRequested { get; set; }
        public float WasteHeatUnits { get; set; }
        public float WasteHeatKw { get; set; }
        public string? FailureCode { get; set; }
        public string AcousticSignatureClass { get; set; } = string.Empty;
        public string StackHealthClass { get; set; } = string.Empty;
        /// <summary>Host feedback: the canonical fuel owner confirmed the draw was covered.</summary>
        public bool FuelSatisfiedByHost { get; set; }
        public float ThermalModifier { get; set; }
        public float FuelQualityModifier { get; set; }
        public float StackHealthModifier { get; set; }
        public float SealIntegrityModifier { get; set; }
    }

    /// <summary>
    /// Subterranean solid-oxide fuel cell plant (Plan 122 Phase 3).
    ///
    /// Gameplay-abstract electrochemistry: a bounded multiplicative output
    /// model over rated power — thermal band, fuel quality, stack health,
    /// seal integrity — plus slow degradation, seeded fault risk, and a
    /// waste-heat stream routed by the host through the canonical thermal
    /// seam. The engine never mutates fuel inventory or grid state and never
    /// touches kitchen/greenhouse stats directly; the power grid remains the
    /// dispatch authority and registers this plant via its contribution seam.
    ///
    /// Deterministic: all fault rolls consume ISeededRng only; same state +
    /// same seed + same command sequence = identical outcomes. No
    /// System.Random, no wall-clock, no real operating procedures.
    /// </summary>
    public sealed class SofcElectrochemistryEngine
    {
        public const string SystemId = "sofc_power";

        /// <summary>Fraction of rated fuel draw burned per preheat tick (burner duty).</summary>
        public const float StartupFuelFractionPerTick = 0.15f;
        /// <summary>Extra degradation per thermal cycle, in ticks' worth.</summary>
        public const int ThermalCycleDegradationTicksWorth = 10;
        /// <summary>Seal loss per thermal cycle (bp).</summary>
        public const int SealLossPerCycleBp = 150;
        /// <summary>Thermal decay per unfueled tick (preheat uses the profile band).</summary>
        public const float ThermalDecayPerTick = 0.08f;
        /// <summary>Thermal overshoot above band maximum before fault risk applies (absolute 0..1).</summary>
        public const float ThermalOvershootFaultMargin = 0.04f;
        /// <summary>Max fault-risk reduction from skill (bp) — bounded, never immunity.</summary>
        public const int MaxSkillFaultReductionBp = 2000;
        /// <summary>Max degradation reduction from skill (fraction of 10000).</summary>
        public const int MaxSkillDegradationReductionBp = 2500;
        /// <summary>Health below which the plant runs Derated (bp).</summary>
        public const int DeratedHealthThresholdBp = 4000;
        /// <summary>Health at which the stack can no longer run (bp).</summary>
        public const int DeadStackHealthBp = 1000;
        /// <summary>Seal below which startup is refused (bp).</summary>
        public const int SealCriticalStartupBp = 1500;

        private SolidOxideFuelCellState _state = new SolidOxideFuelCellState();
        private readonly SofcPowerCatalog _catalog;
        private readonly ILog _log;

        public ISeededRng? Rng { get; set; }

        public SolidOxideFuelCellState State => _state;
        public SofcPowerCatalog Catalog => _catalog;

        public SofcElectrochemistryEngine(SofcPowerCatalog catalog, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        public SofcStackProfile? Profile =>
            _catalog.GetStack(string.IsNullOrEmpty(_state.StackProfileId) ? "" : _state.StackProfileId);

        /// <summary>Installs a stack profile; requires the authored install kit.</summary>
        public ActionResult Install(string profileId, bool partsAvailable, int skillBpBonus = 0)
        {
            var profile = _catalog.GetStack(profileId ?? string.Empty);
            if (profile == null)
                return ActionResult.Blocked(SofcFailureCodes.StackUnavailable, "sofc.stack_profile_unknown");
            if (!partsAvailable)
                return ActionResult.Blocked("sofc_install_parts_missing", "sofc.install_parts_missing");
            if (_state.Mode != SofcOperatingMode.Offline && _state.Mode != SofcOperatingMode.Faulted)
                return ActionResult.Blocked("sofc_install_not_offline", "sofc.install_not_offline");

            _state = new SolidOxideFuelCellState
            {
                StackProfileId = profile.id,
                StackHealthBp = ClampBp(10000 + skillBpBonus),
                SealIntegrityBp = 10000
            };
            return ActionResult.Success("sofc_installed", eventId: "sofc.install");
        }

        /// <summary>Begins the preheat cycle. Refused while faulted or critically sealed.</summary>
        public ActionResult StartPreheat(bool fuelAvailable)
        {
            if (Profile == null)
                return ActionResult.Blocked(SofcFailureCodes.StackUnavailable, "sofc.no_stack_installed");
            if (_state.Mode == SofcOperatingMode.Faulted)
                return ActionResult.Blocked(SofcFailureCodes.MaintenanceRequired, "sofc.maintenance_required");
            if (_state.SealIntegrityBp < SealCriticalStartupBp)
                return ActionResult.Blocked(SofcFailureCodes.SealIntegrityCritical, "sofc.seal_integrity_critical");
            if (_state.Mode != SofcOperatingMode.Offline && _state.Mode != SofcOperatingMode.CoolingDown)
                return ActionResult.Blocked("sofc_already_active", "sofc.already_active");
            if (!fuelAvailable)
                return ActionResult.Blocked(SofcFailureCodes.FuelUnavailable, "sofc.fuel_unavailable");

            _state.Mode = SofcOperatingMode.Preheating;
            _state.TicksInMode = 0;
            return ActionResult.Success("sofc_preheating", eventId: "sofc.start_preheat");
        }

        /// <summary>Controlled shutdown: the stack must cool before going Offline.</summary>
        public ActionResult Shutdown()
        {
            if (_state.Mode is not (SofcOperatingMode.Online or SofcOperatingMode.Derated or SofcOperatingMode.Preheating))
                return ActionResult.Blocked("sofc_not_running", "sofc.not_running");
            _state.Mode = _state.ThermalLevel > 0.05f ? SofcOperatingMode.CoolingDown : SofcOperatingMode.Offline;
            _state.TicksInMode = 0;
            return ActionResult.Success("sofc_shutdown", eventId: "sofc.shutdown");
        }

        /// <summary>Clears a fault. Requires maintenance parts; costs a small health toll.</summary>
        public ActionResult PerformMaintenance(bool partsAvailable, float engineeringSkillLevel)
        {
            if (_state.Mode != SofcOperatingMode.Faulted && _state.Mode != SofcOperatingMode.Online
                && _state.Mode != SofcOperatingMode.Derated)
                return ActionResult.Blocked("sofc_maintenance_not_needed", "sofc.maintenance_not_needed");
            if (!partsAvailable)
                return ActionResult.Blocked("sofc_maintenance_parts_missing", "sofc.maintenance_parts_missing");

            bool wasFaulted = _state.Mode == SofcOperatingMode.Faulted;
            if (wasFaulted)
            {
                _state.FaultCode = string.Empty;
                // Skill reduces the maintenance health toll, bounded to zero toll
                // at mastery — never a repair of degradation itself.
                int tollBp = 200 - (int)Math.Clamp(engineeringSkillLevel * 2f, 0f, 200f);
                _state.StackHealthBp = ClampBp(_state.StackHealthBp - tollBp);
            }
            _state.Mode = SofcOperatingMode.Offline;
            _state.TicksInMode = 0;
            _state.ThermalLevel = 0f;
            return ActionResult.Success("sofc_maintained", eventId: "sofc.maintenance");
        }

        /// <summary>Full stack rebuild with real crafted components. Any offline state.</summary>
        public ActionResult RebuildStack(string gradeProfileId, bool partsAvailable)
        {
            if (_state.Mode != SofcOperatingMode.Offline)
                return ActionResult.Blocked("sofc_rebuild_not_offline", "sofc.rebuild_not_offline");
            if (!partsAvailable)
                return ActionResult.Blocked("sofc_install_parts_missing", "sofc.install_parts_missing");
            var grade = string.IsNullOrEmpty(gradeProfileId) ? null : _catalog.GetGrade(gradeProfileId);
            if (grade == null)
                return ActionResult.Blocked("sofc_grade_unknown", "sofc.grade_profile_unknown");

            _state.StackHealthBp = ClampBp(10000 + grade.stack_health_bonus_bp);
            _state.SealIntegrityBp = ClampBp(10000 + grade.seal_integrity_bonus_bp);
            _state.ThermalCycles = 0;
            _state.FaultCode = string.Empty;
            return ActionResult.Success("sofc_rebuilt", eventId: "sofc.rebuild");
        }

        /// <summary>
        /// Advances one operating tick. Deterministic; the only stochastic
        /// behavior is the fault roll, consumed from ISeededRng.
        /// </summary>
        public SofcGenerationResult AdvanceTick(SofcTickInput input)
        {
            input ??= new SofcTickInput();
            var profile = Profile;
            if (profile == null)
                return Result(SofcOperatingMode.Offline, input, failure: SofcFailureCodes.StackUnavailable);

            _state.LastFuelQualityClass = input.FuelQualityClass ?? string.Empty;
            var fuelProfile = _catalog.GetFuel(profile.fuel_profile_id);
            // Fuel-quality modifiers come from the STACK's authored fuel
            // profile (the stack's tolerance), modulated by the supplied
            // quality class of what actually burned this tick.
            var suppliedQuality = ResolveFuelQuality(input.FuelQualityClass);
            var quality = _catalog.GetFuel(FuelProfileIdForQuality(suppliedQuality)) ?? fuelProfile;
            float qualityModifier = quality != null ? quality.output_modifier_bp / 10000f : 0.75f;
            int qualityDegradationMultiplierBp = quality != null ? quality.degradation_multiplier_bp : 20000;
            int qualityFaultRiskBp = quality != null ? quality.fault_risk_bp : 400;

            switch (_state.Mode)
            {
                case SofcOperatingMode.Preheating:
                    return TickPreheating(profile, input);
                case SofcOperatingMode.Stabilizing:
                    _state.Mode = SofcOperatingMode.Online;
                    _state.TicksInMode = 0;
                    return TickOnline(profile, input, qualityModifier, qualityDegradationMultiplierBp, qualityFaultRiskBp, stabilizingEntry: true);
                case SofcOperatingMode.Online:
                case SofcOperatingMode.Derated:
                    return TickOnline(profile, input, qualityModifier, qualityDegradationMultiplierBp, qualityFaultRiskBp, stabilizingEntry: false);
                case SofcOperatingMode.CoolingDown:
                    return TickCooling(profile, input);
                case SofcOperatingMode.Faulted:
                    return Result(SofcOperatingMode.Faulted, input, failure: _state.FaultCode.Length > 0 ? _state.FaultCode : SofcFailureCodes.StackFaulted);
                default:
                    return Result(SofcOperatingMode.Offline, input);
            }
        }

        private SofcGenerationResult TickPreheating(SofcStackProfile profile, SofcTickInput input)
        {
            _state.TicksInMode++;
            if (!input.FuelAvailable)
            {
                _state.ThermalLevel = Math.Max(0f, _state.ThermalLevel - ThermalDecayPerTick);
                var stalled = Result(SofcOperatingMode.Preheating, input, failure: SofcFailureCodes.FuelUnavailable);
                return stalled;
            }

            var band = profile.thermal_band!;
            // Rise so the stack reaches optimal_min in exactly startup_ticks.
            float risePerTick = band.optimal_min / Math.Max(1, profile.startup_ticks);
            _state.ThermalLevel = Math.Min(band.maximum, _state.ThermalLevel + risePerTick);
            _state.LastFuelUnitsRequested = profile.rated_power_kw / Math.Max(0.05f, profile.fuel_efficiency) * StartupFuelFractionPerTick;

            if (_state.ThermalLevel >= band.optimal_min)
            {
                _state.Mode = SofcOperatingMode.Stabilizing;
                _state.TicksInMode = 0;
            }
            var r = Result(SofcOperatingMode.Preheating, input);
            r.FuelUnitsRequested = _state.LastFuelUnitsRequested;
            return r;
        }

        private SofcGenerationResult TickOnline(
            SofcStackProfile profile, SofcTickInput input,
            float qualityModifier, int qualityDegradationMultiplierBp, int qualityFaultRiskBp,
            bool stabilizingEntry)
        {
            var band = profile.thermal_band!;
            _state.TicksInMode++;

            // ── Thermal band behavior ────────────────────────────────────
            if (!input.FuelAvailable)
            {
                _state.ThermalLevel = Math.Max(0f, _state.ThermalLevel - ThermalDecayPerTick);
                if (_state.ThermalLevel < band.minimum)
                {
                    _state.Mode = SofcOperatingMode.CoolingDown;
                    _state.TicksInMode = 0;
                    return Result(_state.Mode, input, failure: SofcFailureCodes.FuelUnavailable);
                }
                var noFuel = Result(_state.Mode, input, failure: SofcFailureCodes.FuelUnavailable);
                noFuel.AvailableOutputKw = 0f;
                return noFuel;
            }

            // Hold the stack in the optimal band while fueled (plant-controlled).
            _state.ThermalLevel = Math.Min(band.maximum, band.optimal_min + 0.5f * (band.optimal_max - band.optimal_min));
            float thermalModifier;
            if (_state.ThermalLevel < band.minimum)
                thermalModifier = 0f;
            else if (_state.ThermalLevel <= band.optimal_max)
                thermalModifier = 1f;
            else
                thermalModifier = 0.85f; // overheated derate, still bounded

            // ── Bounded multiplicative output model (plan §4.4) ──────────
            float stackHealthModifier = 0.5f + 0.5f * _state.StackHealthBp / 10000f;
            float sealIntegrityModifier = 0.5f + 0.5f * _state.SealIntegrityBp / 10000f;
            float available = profile.rated_power_kw
                * thermalModifier
                * qualityModifier
                * stackHealthModifier
                * sealIntegrityModifier;

            float requested = Math.Max(0f, input.RequestedOutputKw);
            float dispatched = Math.Min(requested, available);
            bool clamped = requested > available + 0.0001f;

            // Fuel draw = dispatched output / effective efficiency. Grid fuel
            // stays grid-owned; the host applies this request via AddFuel.
            float effectiveEfficiency = Math.Max(0.05f, profile.fuel_efficiency * stackHealthModifier);
            float fuelRequested = dispatched / effectiveEfficiency;

            // ── Waste heat (CHP): scales with output, capped by profile ──
            float wasteHeatUnits = dispatched > 0f
                ? profile.waste_heat_units_per_tick * (dispatched / Math.Max(0.01f, profile.rated_power_kw))
                : 0f;
            var heatProfile = _catalog.GetWasteHeat(profile.waste_heat_profile_id ?? string.Empty);
            float wasteHeatKw = heatProfile != null
                ? Math.Min(heatProfile.max_allocatable_kw, wasteHeatUnits * heatProfile.heat_kw_per_unit)
                : 0f;

            // ── Degradation (operating hours + fuel quality + cycles) ────
            ApplyDegradation(profile, input, qualityDegradationMultiplierBp, cycleEvent: stabilizingEntry);

            // ── Seeded fault risk (dirty fuel, overheated stack) ─────────
            string? failure = null;
            int faultRiskBp = qualityFaultRiskBp;
            if (_state.ThermalLevel > band.maximum - ThermalOvershootFaultMargin)
                faultRiskBp += 500;
            faultRiskBp = ReduceBySkill(faultRiskBp, input.EngineeringSkillLevel, MaxSkillFaultReductionBp);
            if (Rng != null && Rng.Next(0, 10000) < faultRiskBp)
            {
                _state.Mode = SofcOperatingMode.Faulted;
                _state.FaultCode = SofcFailureCodes.StackFaulted;
                failure = SofcFailureCodes.StackFaulted;
                dispatched = 0f;
                wasteHeatUnits = 0f;
                wasteHeatKw = 0f;
            }

            // ── Health-driven mode transitions ───────────────────────────
            if (failure == null)
            {
                if (_state.StackHealthBp <= DeadStackHealthBp)
                {
                    _state.Mode = SofcOperatingMode.Faulted;
                    _state.FaultCode = SofcFailureCodes.StackUnavailable;
                    failure = SofcFailureCodes.StackUnavailable;
                    dispatched = 0f;
                    wasteHeatUnits = 0f;
                    wasteHeatKw = 0f;
                }
                else if (_state.StackHealthBp < DeratedHealthThresholdBp && _state.Mode == SofcOperatingMode.Online)
                {
                    _state.Mode = SofcOperatingMode.Derated;
                }
                else if (_state.StackHealthBp >= DeratedHealthThresholdBp && _state.Mode == SofcOperatingMode.Derated)
                {
                    // Health recovered (rebuild) returns the plant to Online.
                    _state.Mode = SofcOperatingMode.Online;
                }
            }

            _state.OperatingTicks++;
            var result = Result(_state.Mode, input, failure);
            result.AvailableOutputKw = failure == null ? available : 0f;
            result.DispatchedOutputKw = dispatched;
            result.DispatchClamped = clamped && failure == null;
            result.FuelUnitsRequested = failure == null ? fuelRequested : 0f;
            result.WasteHeatUnits = wasteHeatUnits;
            result.WasteHeatKw = wasteHeatKw;
            result.ThermalModifier = thermalModifier;
            result.FuelQualityModifier = qualityModifier;
            result.StackHealthModifier = stackHealthModifier;
            result.SealIntegrityModifier = sealIntegrityModifier;
            return result;
        }

        private SofcGenerationResult TickCooling(SofcStackProfile profile, SofcTickInput input)
        {
            _state.TicksInMode++;
            _state.ThermalLevel = Math.Max(0f, _state.ThermalLevel - ThermalDecayPerTick);
            if (_state.ThermalLevel <= 0.001f)
            {
                _state.Mode = SofcOperatingMode.Offline;
                _state.TicksInMode = 0;
            }
            var r = Result(_state.Mode, input);
            r.AvailableOutputKw = 0f;
            return r;
        }

        private void ApplyDegradation(SofcStackProfile profile, SofcTickInput input,
            int qualityDegradationMultiplierBp, bool cycleEvent)
        {
            if (cycleEvent)
            {
                _state.ThermalCycles++;
                int cycleDegradationBp = ReduceBySkillBp(
                    profile.degradation_per_operating_tick_bp * ThermalCycleDegradationTicksWorth,
                    input.EngineeringSkillLevel);
                _state.StackHealthBp = ClampBp(_state.StackHealthBp - cycleDegradationBp);
                _state.SealIntegrityBp = ClampBp(_state.SealIntegrityBp - SealLossPerCycleBp);
            }

            // Per-tick operating degradation scaled by fuel quality multiplier.
            int tickDegradationBp = (profile.degradation_per_operating_tick_bp * qualityDegradationMultiplierBp) / 10000;
            tickDegradationBp = ReduceBySkillBp(tickDegradationBp, input.EngineeringSkillLevel);
            _state.StackHealthBp = ClampBp(_state.StackHealthBp - tickDegradationBp);

            // Dirty fuel scours seals faster than clean fuel; clean adds only
            // the base slow wear.
            if (qualityDegradationMultiplierBp > 10000)
            {
                int extraSealWearBp = (qualityDegradationMultiplierBp - 10000) / 20;
                _state.SealIntegrityBp = ClampBp(_state.SealIntegrityBp - extraSealWearBp);
            }
        }

        private string? ResolveFuelQuality(string? qualityClass)
        {
            return qualityClass switch
            {
                SofcPowerCatalog.QualityDirty => SofcPowerCatalog.QualityDirty,
                SofcPowerCatalog.QualityTreated => SofcPowerCatalog.QualityTreated,
                SofcPowerCatalog.QualityClean => SofcPowerCatalog.QualityClean,
                _ => SofcPowerCatalog.QualityTreated // unknown → conservative treated
            };
        }

        private string FuelProfileIdForQuality(string qualityClass)
        {
            // The stack's authored fuel profile pins its clean-baseline
            // tolerance; the quality class selects the modifier row by
            // quality_class field, resolved from the catalog.
            foreach (var fuel in _catalog.fuel_profiles)
                if (string.Equals(fuel.quality_class, qualityClass, StringComparison.Ordinal))
                    return fuel.id;
            return _catalog.fuel_profiles.Count > 0 ? _catalog.fuel_profiles[0].id : string.Empty;
        }

        private static int ReduceBySkill(int riskBp, float skill, int maxReductionBp)
            => Math.Max(0, riskBp - (int)Math.Clamp(skill / 100f * maxReductionBp, 0f, (float)maxReductionBp));

        private static int ReduceBySkillBp(int degradationBp, float skill)
            => Math.Max(0, degradationBp - (int)Math.Clamp(degradationBp * (skill / 100f) * (MaxSkillDegradationReductionBp / 10000f), 0f, (float)degradationBp));

        private static int ClampBp(int value) => Math.Clamp(value, 0, 10000);

        private SofcGenerationResult Result(SofcOperatingMode mode, SofcTickInput input, string? failure = null)
        {
            var profile = Profile;
            return new SofcGenerationResult
            {
                Mode = mode,
                FailureCode = failure,
                StackHealthClass = SofcStackHealth.Classify(_state.StackHealthBp),
                AcousticSignatureClass = profile?.acoustic_signature_class ?? string.Empty
            };
        }
    
        // ── Save round-trip (Plan 122 Phase 8): deep-clone via typed JSON. ──
        public SolidOxideFuelCellState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            return s.Deserialize<SolidOxideFuelCellState>(s.Serialize(_state)) ?? new SolidOxideFuelCellState();
        }

        public void RestoreState(SolidOxideFuelCellState? saved)
        {
            if (saved == null) return; // old saves: no SOFC section → stays Offline
            var s = new SystemTextJsonSerializer();
            _state = s.Deserialize<SolidOxideFuelCellState>(s.Serialize(saved)) ?? new SolidOxideFuelCellState();
            // Guard against out-of-range enums from foreign saves.
            if (_state.Mode < SofcOperatingMode.Offline || _state.Mode > SofcOperatingMode.Faulted)
                _state.Mode = SofcOperatingMode.Offline;
            _state.StackHealthBp = Math.Clamp(_state.StackHealthBp, 0, 10000);
            _state.SealIntegrityBp = Math.Clamp(_state.SealIntegrityBp, 0, 10000);
        }
}
}
