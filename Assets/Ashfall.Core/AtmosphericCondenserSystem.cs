// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>B5–B8 expansion (§9.9): atmospheric condenser state. Bounded
    /// shelter-level source — built/unbuilt, enabled/disabled, membrane
    /// integrity, bounded weather-dependent yield, cumulative ledger.
    /// No exhaust-network topology (that stays with the ventilation authority
    /// and Plan 189).</summary>
    [Serializable]
    public sealed class AtmosphericCondenserState
    {
        public string systemId = AtmosphericCondenserSystem.SystemId;
        public int schemaVersion = 1;
        public bool built;
        public bool enabled = true;
        public float membraneIntegrity = 100f;
        public long totalYieldLiters;
        public int lastCondenseDay = -1;
    }

    /// <summary>
    /// ASHFALL B5–B8 expansion (§9.9): the shelter atmospheric water
    /// condenser — a Peltier condensation array condensing humidity out of the
    /// shelter's exhaust flow.
    ///
    /// Source identity, never a second counter: pushes a bounded RAW-water
    /// yield into the canonical treatment authority through the Plan 189
    /// intake seam (<see cref="WaterTreatmentSystem.TryAddWaterFromSource"/>).
    /// Condensate is raw water — purification stays wholly with the treatment
    /// authority. Physically there is no brine: this is condensation, not
    /// salt-water desalination (the §9.15 defer-hold applies).
    ///
    /// Power: the Peltier array is a registered grid load (§6.3). The yield is
    /// deterministic from the weather authority's kind (a read-only humidity
    /// proxy — rain days carry humid exhaust, ash/fallout days dry one; the
    /// condenser never writes weather state) and the membrane condition.
    ///
    /// Build/maintenance: gated on the live
    /// <c>knowledge_water_condenser_blueprint</c> capability (host passes the
    /// query result); consumes the canonical <c>item_desal_membrane</c> as the
    /// core component on build and on membrane replacement — giving the
    /// flagged breakthrough item its real consumer.
    /// </summary>
    public sealed class AtmosphericCondenserSystem
    {
        public const string SystemId = "water_condenser";

        /// <summary>Plan 189 intake source id for condensate.</summary>
        public const string SourceId = "source_atmospheric_condenser";

        /// <summary>Research capability gating the build (host queries live).</summary>
        public const string RequiredKnowledgeId = "knowledge_water_condenser_blueprint";

        /// <summary>Canonical core component (the node's breakthrough item;
        /// also the replacement membrane).</summary>
        public const string MembraneItemId = "item_desal_membrane";

        /// <summary>Bounded daily yield at humidity index 1.0 and a perfect
        /// membrane (liters of raw water).</summary>
        public const float BaseYieldLitersPerDay = 15f;

        /// <summary>Peltier array draw, registered as a Standard-class load —
        /// a supplemental production source, shed-able by priority.</summary>
        public const float ArrayDrawWatts = 90f;

        /// <summary>Stable grid load id.</summary>
        public const string PowerRoomId = "room_water_condenser";

        /// <summary>Membrane wear per condensing day (linear lifetime: a
        /// 200-day membrane at full use). Wear is per working day, not per
        /// liter — per-liter wear decays exponentially with the yield it
        /// scales, so a fouling membrane would never actually reach spent.</summary>
        public const float MembraneWearPerCondensingDay = 0.5f;

        /// <summary>
        /// Deterministic humidity index for a weather kind (read-only
        /// projection of the weather authority's current kind; no weather
        /// mutation, no RNG). Rain/black-rain days carry the most humidity;
        /// blizzard air is frozen-dry.
        /// </summary>
        public static float HumidityIndexFor(WeatherKind kind) => kind switch
        {
            WeatherKind.Rain => 1.0f,
            WeatherKind.BlackRain => 0.8f,
            WeatherKind.Overcast => 0.7f,
            WeatherKind.Clear => 0.5f,
            WeatherKind.Ashfall => 0.4f,
            WeatherKind.FalloutStorm => 0.3f,
            WeatherKind.Blizzard => 0.2f,
            _ => 0.5f
        };

        private AtmosphericCondenserState _state = new AtmosphericCondenserState();
        private bool _membraneSpentWarned;
        private readonly PowerGridSystem _powerGrid;
        private readonly WaterTreatmentSystem _waterTreatment;
        private readonly WeatherSystem _weather;
        private readonly ILog _log;

        public event Action<AtmosphericCondenserState>? OnStateChanged;

        /// <summary>B5–B8 expansion (§27): the membrane is spent — the array
        /// produces nothing until replaced. Hard functional-failure edge
        /// (runtime latch; restore re-seeds it).</summary>
        public event Action<AtmosphericCondenserState>? OnMembraneSpent;

        public AtmosphericCondenserSystem(
            PowerGridSystem powerGrid, WaterTreatmentSystem waterTreatment,
            WeatherSystem weather, ILog? log = null)
        {
            _powerGrid = powerGrid ?? throw new ArgumentNullException(nameof(powerGrid));
            _waterTreatment = waterTreatment ?? throw new ArgumentNullException(nameof(waterTreatment));
            _weather = weather ?? throw new ArgumentNullException(nameof(weather));
            _log = log ?? NullLog.Instance;
        }

        public AtmosphericCondenserState State => _state;
        public bool IsBuilt => _state.built;
        public bool IsEnabled => _state.enabled;
        public float MembraneIntegrity => _state.membraneIntegrity;
        public long TotalYieldLiters => _state.totalYieldLiters;

        /// <summary>Build the array. The capability flag comes from a live
        /// query; the host consumes the canonical membrane bill first — the
        /// research unlock alone never grants the array (§15.3).</summary>
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
            _state.membraneIntegrity = 100f;
            RegisterArrayLoad();
            _log.Info("[WaterCondenser] Peltier condensation array built; registered as a grid load.");
            OnStateChanged?.Invoke(_state);
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
            OnStateChanged?.Invoke(_state);
            return true;
        }

        /// <summary>Replace the condensation membrane (restores integrity to
        /// 100). The host consumes one canonical
        /// <see cref="MembraneItemId"/> first; blocked when not needed —
        /// wasted parts are a blocked action, not a silent success.</summary>
        public bool ReplaceMembrane(out string reason)
        {
            reason = string.Empty;
            if (!_state.built)
            {
                reason = "not_built";
                return false;
            }
            if (_state.membraneIntegrity >= 100f)
            {
                reason = "membrane_integrity_full";
                return false;
            }

            _state.membraneIntegrity = 100f;
            _membraneSpentWarned = false;
            OnStateChanged?.Invoke(_state);
            return true;
        }

        /// <summary>
        /// Daily tick: when built, enabled, powered (served load), and the
        /// membrane holds charge, condense the bounded weather-indexed raw
        /// yield into the treatment intake. Blocked intake (advisory) leaves
        /// the vapor uncondensed — conserved, no private storage. Membrane
        /// wear only on condensing days. Deterministic.
        /// </summary>
        public void TickDay(int day)
        {
            if (!_state.built || !_state.enabled) return;
            if (!_powerGrid.IsRoomServed(PowerRoomId)) return; // power owns availability
            if (_state.membraneIntegrity <= 0f) return; // spent membrane: no exchange surface

            float humidityIndex = HumidityIndexFor(_weather.Current);
            float yield = BaseYieldLitersPerDay * humidityIndex * (_state.membraneIntegrity / 100f);
            if (yield <= 0f) return;

            var add = _waterTreatment.TryAddWaterFromSource(SourceId, WaterType.Raw, yield);
            if (add.Status != ActionResult.StatusKind.Success)
                return; // intake blocked: vapor stays in the exhaust stream

            _state.totalYieldLiters += (long)MathF.Round(yield);
            _state.membraneIntegrity = MathF.Max(0f,
                _state.membraneIntegrity - MembraneWearPerCondensingDay);
            _state.lastCondenseDay = day;
            if (!_membraneSpentWarned && _state.membraneIntegrity <= 0f)
            {
                _membraneSpentWarned = true;
                OnMembraneSpent?.Invoke(_state);
            }
            OnStateChanged?.Invoke(_state);
        }

        /// <summary>§6.3: expose the array as a named grid load. Idempotent;
        /// catalog rooms take precedence.</summary>
        private void RegisterArrayLoad()
        {
            _powerGrid.RegisterLoadRoom(new PowerGridRoom(
                PowerRoomId, "Water Condenser Array", ArrayDrawWatts,
                PowerGridRoomPriority.Standard, "fx_water_condenser_off"));
        }

        public AtmosphericCondenserState CaptureState() => new AtmosphericCondenserState
        {
            systemId = _state.systemId,
            schemaVersion = _state.schemaVersion,
            built = _state.built,
            enabled = _state.enabled,
            membraneIntegrity = _state.membraneIntegrity,
            totalYieldLiters = _state.totalYieldLiters,
            lastCondenseDay = _state.lastCondenseDay
        };

        public void RestoreState(AtmosphericCondenserState? saved)
        {
            if (saved == null) return;
            _state = new AtmosphericCondenserState
            {
                systemId = string.IsNullOrEmpty(saved.systemId) ? SystemId : saved.systemId,
                schemaVersion = saved.schemaVersion,
                built = saved.built,
                enabled = saved.enabled,
                membraneIntegrity = Math.Clamp(saved.membraneIntegrity, 0f, 100f),
                totalYieldLiters = Math.Max(0L, saved.totalYieldLiters),
                lastCondenseDay = saved.lastCondenseDay
            };
            if (_state.built)
                RegisterArrayLoad();
            _membraneSpentWarned = _state.built && _state.membraneIntegrity <= 0f;
            OnStateChanged?.Invoke(_state);
        }
    }
}
