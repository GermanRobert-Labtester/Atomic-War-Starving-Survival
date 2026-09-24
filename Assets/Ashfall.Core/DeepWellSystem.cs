// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Shelter;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>B5–B8 Phase 6 (Plan 66 §9.8): deep-well source state. Smallest
    /// bounded source representation for the shelter — built/unbuilt,
    /// enabled/disabled, bounded yield, pump condition, cumulative yield.
    /// Aquifer topology and depletion stay with Plan 189.</summary>
    [Serializable]
    public sealed class DeepWellState
    {
        public string systemId = DeepWellSystem.SystemId;
        public int schemaVersion = DeepWellSystem.CurrentSchemaVersion;
        public bool built;
        public bool enabled = true;
        public float condition = 100f;
        public long totalYieldLiters;
        public int lastPumpDay = -1;
    }

    /// <summary>
    /// ASHFALL B5–B8 Phase 6 (Plan 66 §9.8): the shelter deep well.
    ///
    /// A single sub-bedrock submersible pump pushing a bounded daily yield of
    /// RAW water into the canonical treatment authority through the Plan 189
    /// intake seam (<see cref="WaterTreatmentSystem.TryAddWaterFromSource"/>) —
    /// the well is a source identity, never a second water counter, and never
    /// produces potable water directly (sub-bedrock aquifers need treatment;
    /// purification stays wholly with the treatment authority).
    ///
    /// Power: the pump is a registered critical grid load (§6.3 contract —
    /// <see cref="PowerGridSystem.RegisterLoadRoom"/>); the well reads its
    /// served state and owns no grid mutation. No served power → no yield.
    ///
    /// Maintenance: pump condition wears only on pumping days; service
    /// restores it (the host consumes the canonical machine oil — the grid and
    /// the well never touch inventory).
    ///
    /// Research: the capability gate is a parameter of <see cref="TryBuild"/> —
    /// the host passes the live capability query result (research is
    /// permission; the build is the physical act).
    /// </summary>
    public sealed class DeepWellSystem
    {
        public const string SystemId = "deep_well";
        public const int CurrentSchemaVersion = 1;

        /// <summary>Plan 189 intake source id for deep-well water.</summary>
        public const string SourceId = "source_deep_well";

        /// <summary>Research capability that gates the build (host queries live).</summary>
        public const string RequiredKnowledgeId = "knowledge_deep_well_hydraulics";

        /// <summary>Canonical build component (the node's breakthrough item).</summary>
        public const string BuildItemId = "item_hydraulic_actuator";

        /// <summary>Canonical pump-service consumable (same lubricant the
        /// subgrid repair and generator service use).</summary>
        public const string MaintenanceItemId = "machine_oil";

        /// <summary>Bounded daily yield at full condition (liters of raw water).</summary>
        public const float RatedYieldLitersPerDay = 40f;

        /// <summary>High-pressure submersible pump draw, registered as a
        /// critical load (InfrastructureEssential — water infrastructure).</summary>
        public const float PumpDrawWatts = 120f;

        /// <summary>Stable grid load id (registered with the power grid).</summary>
        public const string PowerRoomId = "room_deep_well_pump";

        /// <summary>Pump wear per pumping day. 100 → 0 over 500 pumping days.</summary>
        public const float WearPerPumpingDay = 0.2f;

        private DeepWellState _state = new DeepWellState();
        private bool _pumpWornWarned;

        private readonly PowerGridSystem _powerGrid;
        private readonly WaterTreatmentSystem _waterTreatment;
        private readonly ILog _log;

        public event Action<DeepWellState>? OnStateChanged;

        /// <summary>B5–B8 expansion (§27): the pump crossed below its wear
        /// threshold — yield is now derating. Fires once per wear cycle
        /// (runtime latch; re-seeded on restore so reloads never replay).</summary>
        public event Action<DeepWellState>? OnPumpWorn;

        /// <summary>Condition below which the pump warning fires (yield is
        /// scaling linearly already; this is the actionable warn edge).</summary>
        public const float PumpWornThreshold = 40f;

        public DeepWellSystem(PowerGridSystem powerGrid, WaterTreatmentSystem waterTreatment, ILog? log = null)
        {
            _powerGrid = powerGrid ?? throw new ArgumentNullException(nameof(powerGrid));
            _waterTreatment = waterTreatment ?? throw new ArgumentNullException(nameof(waterTreatment));
            _log = log ?? NullLog.Instance;
        }

        public DeepWellState State => CaptureState();
        public bool IsBuilt => _state.built;
        public bool IsEnabled => _state.enabled;
        public float Condition => _state.condition;
        public long TotalYieldLiters => _state.totalYieldLiters;

        /// <summary>
        /// Build the well. <paramref name="hasRequiredCapability"/> must come
        /// from a live capability query — research permission and the physical
        /// build remain distinct: the host consumes the canonical actuator bill
        /// before calling this, and unlocking the research alone never grants
        /// the well (flagship §15.3).
        /// </summary>
        public bool TryBuild(bool hasRequiredCapability, out string reason)
        {
            reason = string.Empty;
            if (_state.built)
            {
                reason = "already_built";
                return false;
            }
            if (!hasRequiredCapability)
            {
                reason = "missing_knowledge";
                return false;
            }

            _state.built = true;
            _state.enabled = true;
            _state.condition = 100f;
            _state.totalYieldLiters = 0L;
            _state.lastPumpDay = -1;
            RegisterPumpLoad();
            _log.Info("[DeepWell] Deep-well pump built and registered as a critical grid load.");
            OnStateChanged?.Invoke(CaptureState());
            return true;
        }

        public bool SetEnabled(bool enabled, out string reason)
        {
            reason = string.Empty;
            if (!_state.built)
            {
                reason = "not_built";
                return false;
            }
            if (_state.enabled == enabled) return true; // idempotent toggle
            _state.enabled = enabled;
            OnStateChanged?.Invoke(CaptureState());
            return true;
        }

        /// <summary>Service the pump back to full condition. Blocked when
        /// already healthy — wasted effort is a blocked action.</summary>
        public bool PerformMaintenance(out string reason)
        {
            reason = string.Empty;
            if (!_state.built)
            {
                reason = "not_built";
                return false;
            }
            if (_state.condition >= 100f)
            {
                reason = "condition_full";
                return false;
            }

            _state.condition = 100f;
            _pumpWornWarned = false; // fresh service: a new wear cycle may warn again
            OnStateChanged?.Invoke(CaptureState());
            return true;
        }

        /// <summary>
        /// Daily tick: when built, enabled, powered (served load), and healthy,
        /// push the bounded raw-water yield into the treatment authority's
        /// intake. A blocked intake (advisory isolation or full pool) leaves
        /// the water in the aquifer — conserved, never lost or stored locally
        /// (no private counter). Wear only on pumping days. Deterministic.
        /// </summary>
        public void TickDay(int day)
        {
            if (day < 0)
                throw new ArgumentOutOfRangeException(nameof(day), "Campaign day cannot be negative.");

            if (!_state.built || !_state.enabled) return;
            if (day <= _state.lastPumpDay) return; // exactly once per successful campaign day
            if (!_powerGrid.IsRoomServed(PowerRoomId)) return; // unserved pump is silent — power owns availability

            float condition = SanitizeCondition(_state.condition);
            float yield = RatedYieldLitersPerDay * (condition / 100f);
            if (!IsFinite(yield) || yield <= 0f) return;

            var add = _waterTreatment.TryAddWaterFromSource(SourceId, WaterType.Raw, yield);
            if (add.Status != ActionResult.StatusKind.Success)
                return; // intake blocked (advisory/full pool): water stays in the aquifer

            long amount = (long)MathF.Round(yield);
            _state.totalYieldLiters = SaturatingAdd(_state.totalYieldLiters, amount);
            _state.condition = SanitizeCondition(_state.condition - WearPerPumpingDay);
            _state.lastPumpDay = day;
            if (!_pumpWornWarned && _state.condition < PumpWornThreshold)
            {
                _pumpWornWarned = true;
                OnPumpWorn?.Invoke(CaptureState());
            }
            OnStateChanged?.Invoke(CaptureState());
        }

        /// <summary>§6.3: expose the pump as a named grid load. Idempotent;
        /// catalog rooms take precedence.</summary>
        private void RegisterPumpLoad()
        {
            _powerGrid.RegisterLoadRoom(new PowerGridRoom(
                PowerRoomId, "Deep Well Pump", PumpDrawWatts,
                PowerGridRoomPriority.Critical, "fx_deep_well_off"));
        }

        public DeepWellState CaptureState() => new DeepWellState
        {
            systemId = SystemId,
            schemaVersion = CurrentSchemaVersion,
            built = _state.built,
            enabled = _state.enabled,
            condition = SanitizeCondition(_state.condition),
            totalYieldLiters = Math.Max(0L, _state.totalYieldLiters),
            lastPumpDay = _state.built ? Math.Max(-1, _state.lastPumpDay) : -1
        };

        public void RestoreState(DeepWellState? saved)
        {
            if (saved == null) return;
            _state = new DeepWellState
            {
                systemId = SystemId,
                schemaVersion = CurrentSchemaVersion,
                built = saved.built,
                enabled = saved.enabled,
                condition = SanitizeCondition(saved.condition),
                totalYieldLiters = Math.Max(0L, saved.totalYieldLiters),
                lastPumpDay = saved.built ? Math.Max(-1, saved.lastPumpDay) : -1
            };
            // The grid's room list is not persisted — re-expose the pump load
            // after restore (idempotent, deterministic, no events replayed).
            if (_state.built)
                RegisterPumpLoad();
            // B5–B8 expansion: re-seed the warn latch from the restored state
            // (an already-worn restored pump never replays its warning).
            _pumpWornWarned = _state.built && _state.condition < PumpWornThreshold;
            OnStateChanged?.Invoke(CaptureState());
        }

        private static bool IsFinite(float value) =>
            !float.IsNaN(value) && !float.IsInfinity(value);

        private static float SanitizeCondition(float value) =>
            IsFinite(value) ? Math.Clamp(value, 0f, 100f) : 0f;

        private static long SaturatingAdd(long current, long amount)
        {
            if (amount <= 0L) return Math.Max(0L, current);
            long normalized = Math.Max(0L, current);
            return normalized > long.MaxValue - amount ? long.MaxValue : normalized + amount;
        }
    }
}
