using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Deep geothermal boreholes and clean aquifer pumping.
    /// Owns borehole depth/progress, strata crossing, casing, drill wear,
    /// scale, steam/pressure engineering state, and aquifer tap state.
    /// </summary>
    public sealed class GeothermalAquiferSystem
    {
        public const string SystemId = "geothermal_aquifer";
        public const float NominalTurbineOutputKw = 1500f;
        public const float DescalingChemicalCost = 2f;

        private GeothermalAquiferState _state;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Dictionary<string, GeothermalStrataDef> _strata = new(StringComparer.Ordinal);
        private readonly PowerGridSystem? _powerGrid;
        private readonly WaterTreatmentSystem? _waterTreatment;
        private readonly Ashfall.Core.Inventory.Inventory? _inventory;
        private int _currentDay;

        public GeothermalAquiferState State => _state;
        public IReadOnlyDictionary<string, GeothermalStrataDef> Strata => _strata;
        public bool IsTurbineCommissioned => _state.turbineCommissioned;
        public bool IsAquiferTapped => _state.aquiferTapped;

        public event Action<string>? OnStrataReached;       // strataId
        public event Action? OnSteamPocketReached;
        public event Action? OnAquiferReached;
        public event Action? OnBasementReached;
        public event Action<string>? OnPressureEvent;      // severity description
        public event Action? OnTurbineCommissioned;
        public event Action? OnTurbineOffline;

        public GeothermalAquiferSystem(
            GeothermalAquiferState? state,
            ISeededRng rng,
            ILog? log = null,
            PowerGridSystem? powerGrid = null,
            WaterTreatmentSystem? waterTreatment = null,
            Ashfall.Core.Inventory.Inventory? inventory = null)
        {
            _state = state ?? new GeothermalAquiferState();
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            _powerGrid = powerGrid;
            _waterTreatment = waterTreatment;
            _inventory = inventory;
        }

        // ── Catalog ──────────────────────────────────────────────────

        public void RegisterStrata(GeothermalStrataDef def)
        {
            if (def != null && !string.IsNullOrEmpty(def.StrataId))
                _strata[def.StrataId] = def;
        }

        public void LoadCatalog(GeothermalDrillingCatalog? catalog)
        {
            if (catalog?.Strata == null) return;
            foreach (var def in catalog.Strata)
                RegisterStrata(def);
        }

        // ── Queries ──────────────────────────────────────────────────

        public GeothermalStrataDef? GetCurrentStrata()
        {
            if (_strata.Count == 0) return null;
            GeothermalStrataDef? current = null;
            foreach (var s in _strata.Values.OrderBy(s => s.StartDepthM))
            {
                if (_state.currentDepthMeters >= s.StartDepthM - 0.01f)
                    current = s;
            }
            return current;
        }

        public float GetEffectiveTurbineOutputKw()
        {
            if (!_state.turbineCommissioned) return 0f;
            float nominal = _state.currentStrataId == "strata_magmatic_basement_1000m" ? NominalTurbineOutputKw * 2f : NominalTurbineOutputKw;
            float scaleFactor = 1f - Math.Clamp(_state.mineralScaling / 100f, 0f, 0.9f);
            float pressureFactor = 1f - Math.Clamp(_state.pressureReliefState, 0f, 0.5f);
            float healthFactor = Math.Clamp(_state.generatorHealth / 100f, 0f, 1f);
            return Math.Max(0f, nominal * scaleFactor * pressureFactor * healthFactor);
        }

        // ── Actions ──────────────────────────────────────────────────

        public ActionResult StartDrilling()
        {
            if (_state.projectActive) return ActionResult.Blocked("already_active", "geothermal.already_active");
            if (_state.drillBitCondition <= 0f) return ActionResult.Blocked("no_drill_bit", "geothermal.no_drill_bit");

            _state.projectActive = true;
            _log.Info("[Geothermal] Drilling started");
            return ActionResult.Success("geothermal.drilling_started");
        }

        public DrillingResult AdvanceDrilling(float workTicks)
        {
            if (!_state.projectActive)
                return DrillingResult.Failed("not_active", "geothermal.not_active");
            if (_state.drillBitCondition <= 0f)
                return DrillingResult.Failed("bit_destroyed", "geothermal.bit_destroyed");

            var current = GetCurrentStrata();
            if (current == null)
                return DrillingResult.Failed("no_strata", "geothermal.no_strata");

            float hardness = current.RockHardness;
            float wearPerMeter = current.DrillWearPerMeter;
            float advance = workTicks * 1.5f / Math.Max(0.5f, hardness);
            float wear = advance * wearPerMeter;

            _state.drillBitCondition = Math.Max(0f, _state.drillBitCondition - wear);
            _state.currentDepthMeters += advance;

            // Check strata boundaries
            foreach (var s in _strata.Values.OrderBy(s => s.StartDepthM))
            {
                if (_state.currentDepthMeters >= s.StartDepthM && !_state.crossedStrataIds.Contains(s.StrataId))
                {
                    _state.crossedStrataIds.Add(s.StrataId);
                    _state.currentStrataId = s.StrataId;
                    OnStrataReached?.Invoke(s.StrataId);

                    if (s.StrataId.Contains("steam"))
                    {
                        OnSteamPocketReached?.Invoke();
                        _state.steamPressurePsi += 50f;
                    }
                    else if (s.StrataId.Contains("aquifer"))
                    {
                        OnAquiferReached?.Invoke();
                    }
                    else if (s.StrataId.Contains("magmatic"))
                    {
                        OnBasementReached?.Invoke();
                        _state.steamPressurePsi += 100f;
                    }
                }
            }

            // Seismic hazard
            if (_rng.NextDouble() < current.SeismicHazard * 0.01f)
            {
                _state.drillBitCondition = Math.Max(0f, _state.drillBitCondition - 10f);
                OnPressureEvent?.Invoke($"micro_tremor_at_{_state.currentDepthMeters:F0}m");
            }

            if (_state.drillBitCondition <= 0f)
                _state.projectActive = false;

            return DrillingResult.Success(advance, _state.drillBitCondition, _state.currentDepthMeters);
        }

        public ActionResult CommissionTurbine()
        {
            if (_state.turbineCommissioned) return ActionResult.Success("geothermal.turbine_already_commissioned");
            if (!_state.crossedStrataIds.Contains("strata_steam_pocket_500m"))
                return ActionResult.Blocked("no_steam", "geothermal.no_steam");
            if (_state.installedCasingDepth < 300f)
                return ActionResult.Blocked("insufficient_casing", "geothermal.insufficient_casing");

            _state.turbineCommissioned = true;
            _state.generatorHealth = 100f;
            OnTurbineCommissioned?.Invoke();
            _log.Info("[Geothermal] Turbine commissioned");
            return ActionResult.Success("geothermal.turbine_commissioned");
        }

        public ActionResult Descale()
        {
            if (_state.mineralScaling <= 0f)
                return ActionResult.Blocked("no_scale", "geothermal.no_scale");

            if (_inventory == null || !_inventory.HasSufficient("item_scrap_chemical", (int)DescalingChemicalCost))
                return ActionResult.Blocked("insufficient_chemicals", "geothermal.insufficient_chemicals");

            _inventory.TryConsume("item_scrap_chemical", (int)DescalingChemicalCost);
            _state.mineralScaling = Math.Max(0f, _state.mineralScaling - 25f);
            _log.Info("[Geothermal] Descaling performed");
            return ActionResult.Success("geothermal.descaling_complete");
        }

        public ActionResult TapAquifer()
        {
            if (_state.aquiferTapped) return ActionResult.Blocked("already_tapped", "geothermal.already_tapped");
            if (!_state.crossedStrataIds.Contains("strata_artesian_aquifer_750m"))
                return ActionResult.Blocked("no_aquifer", "geothermal.no_aquifer");
            if (_state.installedCasingDepth < 500f)
                return ActionResult.Blocked("insufficient_casing", "geothermal.insufficient_casing");

            _state.aquiferTapped = true;
            _log.Info("[Geothermal] Aquifer tapped");
            return ActionResult.Success("geothermal.aquifer_tapped");
        }

        public ActionResult VentPressure()
        {
            _state.pressureReliefState = Math.Min(1f, _state.pressureReliefState + 0.3f);
            _state.steamPressurePsi = Math.Max(0f, _state.steamPressurePsi - 20f);
            _log.Info("[Geothermal] Pressure vented");
            return ActionResult.Success("geothermal.pressure_vented");
        }

        public ActionResult InstallCasing(float depth)
        {
            if (depth <= _state.installedCasingDepth)
                return ActionResult.Blocked("casing_already_present", "geothermal.casing_already_present");
            if (_state.currentDepthMeters < depth)
                return ActionResult.Blocked("cannot_case_above_bit", "geothermal.cannot_case_above_bit");

            _state.installedCasingDepth = Math.Min(_state.currentDepthMeters, depth);
            _log.Info($"[Geothermal] Casing installed to {depth}m");
            return ActionResult.Success("geothermal.casing_installed");
        }

        // ── Daily Tick ───────────────────────────────────────────────

        public void TickDay(int day)
        {
            _currentDay = day;
            if (day <= _state.lastProcessedDay) return;
            _state.lastProcessedDay = day;

            if (_state.turbineCommissioned)
            {
                // Scale accumulation
                _state.mineralScaling = Math.Min(100f, _state.mineralScaling + 0.1f);
                _state.generatorHealth = Math.Max(0f, _state.generatorHealth - 0.02f);

                if (_state.generatorHealth <= 0f)
                {
                    _state.turbineCommissioned = false;
                    OnTurbineOffline?.Invoke();
                }

                // Pressure dynamics
                _state.steamPressurePsi = Math.Max(0f, _state.steamPressurePsi - 0.5f);
                if (_state.steamPressurePsi > 200f && _rng.NextDouble() < 0.02f)
                {
                    OnPressureEvent?.Invoke("high_pressure_event");
                }
            }
        }

        // ── Persistence ──────────────────────────────────────────────

        public GeothermalAquiferState CaptureState() => CloneState(_state);

        public void RestoreState(GeothermalAquiferState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static GeothermalAquiferState CloneState(GeothermalAquiferState src)
        {
            if (src == null) return new GeothermalAquiferState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<GeothermalAquiferState>(json) ?? new GeothermalAquiferState();
        }
    }

    // ── Result DTOs ────────────────────────────────────────────────

    public sealed class DrillingResult
    {
        public bool IsSuccess { get; }
        public string FailureCode { get; }
        public string MessageKey { get; }
        public float AdvancedMeters { get; }
        public float BitCondition { get; }
        public float Depth { get; }

        private DrillingResult(bool success, string failureCode, string messageKey, float advanced, float bitCondition, float depth)
        {
            IsSuccess = success;
            FailureCode = failureCode;
            MessageKey = messageKey;
            AdvancedMeters = advanced;
            BitCondition = bitCondition;
            Depth = depth;
        }

        public static DrillingResult Failed(string code, string key) => new DrillingResult(false, code, key, 0f, 0f, 0f);
        public static DrillingResult Success(float advanced, float bitCondition, float depth)
            => new DrillingResult(true, string.Empty, string.Empty, advanced, bitCondition, depth);
    }
}
