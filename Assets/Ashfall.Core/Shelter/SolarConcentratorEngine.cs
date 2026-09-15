// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Shelter
{
    public enum SolarTrackingMode
    {
        Manual,
        Mechanical,
        Motorized
    }

    [Serializable]
    public sealed class SolarConcentratorDef
    {
        public string concentrator_id = string.Empty;
        public string display_name = string.Empty;
        public int dish_size_tier = 1;
        public float optical_efficiency = 0.7f;
        public float tracking_quality = 0.8f;
        public float max_thermal_kw = 7.0f;
        public float stirling_output_kw = 1.5f;
        public float reflectivity_decay_per_storm = 0.03f;
        public float cleaning_restore = 0.2f;
        public List<string> deployment_tags = new List<string>();
    }

    [Serializable]
    public sealed class SolarConcentratorCatalog
    {
        public int schema_version = 1;
        public List<SolarConcentratorDef> concentrators = new List<SolarConcentratorDef>();
    }

    [Serializable]
    public sealed class SolarConcentratorState
    {
        public string concentratorId = "solar_dish_medium";
        public bool isDeployed = true;
        public float mirrorReflectivity = 1.0f;
        public float surfaceCondition = 1.0f;
        public float alignmentQuality = 0.85f;
        public SolarTrackingMode trackingMode = SolarTrackingMode.Mechanical;
        public bool stirlingAttached = true;

        /// <summary>B5–B8 Phase 2: grid-tie inverter wired (additive field —
        /// legacy saves restore false, i.e. no grid feed). The engine computes
        /// electrical output either way; only a connected inverter lets the
        /// host publish it into the power grid.</summary>
        public bool gridTieConnected;

        public float currentThermalKw;
        public float currentElectricalKw;
        public int daysCleaned;
        public int lastProcessedDay = -1;
    }

    public static class SolarConcentratorCatalogLoader
    {
        public const string DefaultFileName = "solar_concentrator_catalog.json";

        public static SolarConcentratorCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null)
        {
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                log?.Warn($"[SolarConcentrator] catalog not found at {path}");
                return new SolarConcentratorCatalog();
            }

            try
            {
                string text = fileIO.ReadAllText(path);
                var cat = json.Deserialize<SolarConcentratorCatalog>(text);
                return cat ?? new SolarConcentratorCatalog();
            }
            catch (Exception ex)
            {
                log?.Error($"[SolarConcentrator] failed loading catalog: {ex.Message}");
                return new SolarConcentratorCatalog();
            }
        }
    }

    public sealed class SolarConcentratorEngine
    {
        public const string SystemId = "solar_concentrator";

        /// <summary>
        /// B5–B8 Phase 2 (Plan 65): stable grid-contribution source id.
        /// The host publishes <see cref="AvailableElectricalKw"/> × 1000 under
        /// this id once a grid-tie inverter is wired — same contract as
        /// <see cref="NuclearCoreLifecycleSystem.PowerSourceId"/>.
        /// </summary>
        public const string PowerSourceId = "solar_concentrator";

        public const string ItemStirlingGenerator = "item_focal_stirling_engine_generator";
        public const string ItemDishSegment = "item_parabolic_aluminum_dish_segment";
        public const string ItemTrackingGimbal = "item_dual_axis_tracking_gimbal";

        /// <summary>B5–B8 Phase 2: canonical grid-tie inverter item (recipe
        /// <c>assemble_pure_sine_solar_inverter</c>, gated by
        /// <c>knowledge_solar_advanced</c>). The physical item is the gate —
        /// research alone never enables a grid feed.</summary>
        public const string ItemGridTieInverter = "item_solar_inverter";

        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly Func<float>? _solarAvailabilityQuery;
        private readonly ILog? _log;

        private SolarConcentratorCatalog _catalog = new SolarConcentratorCatalog();
        private SolarConcentratorState _state = new SolarConcentratorState();

        public event Action<SolarConcentratorState>? OnStateChanged;
        public event Action<float, float>? OnSolarOutputChanged; // thermalKw, electricalKw
        public event Action<float>? OnDishFouled; // newReflectivity

        public SolarConcentratorState State => _state;
        public SolarConcentratorCatalog Catalog => _catalog;
        public float AvailableThermalKw => _state.currentThermalKw;
        public float AvailableElectricalKw => _state.currentElectricalKw;

        public SolarConcentratorEngine(
            Inventory.Inventory inventory,
            ISeededRng? rng = null,
            Func<float>? solarAvailabilityQuery = null,
            ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? new SeededRng(111);
            _solarAvailabilityQuery = solarAvailabilityQuery;
            _log = log;
        }

        public void LoadCatalog(SolarConcentratorCatalog catalog)
        {
            _catalog = catalog ?? new SolarConcentratorCatalog();
        }

        public SolarConcentratorDef? GetConcentratorDef(string concentratorId)
        {
            return _catalog.concentrators.FirstOrDefault(c => c.concentrator_id == concentratorId);
        }

        public void TickDay(int currentDay, float solarAvailability = 1.0f, bool isAshStormActive = false)
        {
            _state.lastProcessedDay = currentDay;

            if (!_state.isDeployed)
            {
                _state.currentThermalKw = 0f;
                _state.currentElectricalKw = 0f;
                OnSolarOutputChanged?.Invoke(0f, 0f);
                OnStateChanged?.Invoke(_state);
                return;
            }

            float effectiveSun = _solarAvailabilityQuery != null ? _solarAvailabilityQuery() : solarAvailability;
            effectiveSun = Math.Clamp(effectiveSun, 0f, 1f);

            var def = GetConcentratorDef(_state.concentratorId);
            float maxThermal = def?.max_thermal_kw ?? 7.0f;
            float maxStirling = def?.stirling_output_kw ?? 1.5f;
            float stormDecay = def?.reflectivity_decay_per_storm ?? 0.03f;

            if (isAshStormActive)
            {
                _state.mirrorReflectivity = Math.Max(0.05f, _state.mirrorReflectivity - stormDecay);
                _state.surfaceCondition = Math.Max(0.50f, _state.surfaceCondition - 0.005f); // long-term abrasion
                OnDishFouled?.Invoke(_state.mirrorReflectivity);
                _log?.Info($"[SolarConcentrator] Ash storm fouled mirror surface. Reflectivity down to {_state.mirrorReflectivity:P0}.");
            }

            // Calculate thermal output
            if (effectiveSun <= 0.01f)
            {
                _state.currentThermalKw = 0f;
                _state.currentElectricalKw = 0f;
            }
            else
            {
                _state.currentThermalKw = (float)Math.Round(maxThermal * effectiveSun * _state.mirrorReflectivity * _state.alignmentQuality, 2);
                if (_state.stirlingAttached)
                {
                    float thermalFraction = _state.currentThermalKw / Math.Max(0.1f, maxThermal);
                    _state.currentElectricalKw = (float)Math.Round(maxStirling * thermalFraction, 2);
                }
                else
                {
                    _state.currentElectricalKw = 0f;
                }
            }

            OnSolarOutputChanged?.Invoke(_state.currentThermalKw, _state.currentElectricalKw);
            OnStateChanged?.Invoke(_state);
        }

        public void RecalculateOutputs(float solarAvailability = 1.0f)
        {
            TickDay(_state.lastProcessedDay >= 0 ? _state.lastProcessedDay : 1, solarAvailability, false);
        }

        public void ApplyAshStormFouling(float amount = 0.05f)
        {
            _state.mirrorReflectivity = Math.Max(0.05f, _state.mirrorReflectivity - amount);
            OnDishFouled?.Invoke(_state.mirrorReflectivity);
            OnStateChanged?.Invoke(_state);
        }

        public ActionResult CleanMirrors()
        {
            var def = GetConcentratorDef(_state.concentratorId);
            float restore = def?.cleaning_restore ?? 0.20f;

            float oldReflectivity = _state.mirrorReflectivity;
            _state.mirrorReflectivity = Math.Min(_state.surfaceCondition, _state.mirrorReflectivity + restore);
            _state.daysCleaned++;

            _log?.Info($"[SolarConcentrator] Mirrors wiped. Reflectivity restored from {oldReflectivity:P0} to {_state.mirrorReflectivity:P0}.");
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("mirrors_cleaned");
        }

        public ActionResult CalibrateTracking(float alignmentBoost = 0.15f)
        {
            _state.alignmentQuality = Math.Clamp(_state.alignmentQuality + alignmentBoost, 0.1f, 1.0f);
            _log?.Info($"[SolarConcentrator] Tracking gimbal calibrated. Alignment quality now {_state.alignmentQuality:P0}.");
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("calibrated");
        }

        public ActionResult UpgradeTrackingGimbal()
        {
            if (_inventory.CountById(ItemTrackingGimbal) < 1)
                return ActionResult.Blocked("missing_gimbal", "Requires 1x Dual-Axis Tracking Gimbal.");

            _inventory.TryConsumeById(ItemTrackingGimbal, 1);
            _state.trackingMode = SolarTrackingMode.Motorized;
            _state.alignmentQuality = 1.0f;
            _log?.Info("[SolarConcentrator] Installed Dual-Axis Tracking Gimbal. Motorized solar lock active.");
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("gimbal_upgraded");
        }

        public ActionResult AttachStirlingEngine()
        {
            if (_state.stirlingAttached)
                return ActionResult.Blocked("already_attached", "Stirling engine is already mounted at focus.");

            if (_inventory.CountById(ItemStirlingGenerator) < 1)
                return ActionResult.Blocked("missing_stirling", "Requires 1x Focal Stirling Engine Generator.");

            _inventory.TryConsumeById(ItemStirlingGenerator, 1);
            _state.stirlingAttached = true;
            _log?.Info("[SolarConcentrator] Mounted Focal Stirling Engine Generator.");
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("stirling_attached");
        }

        /// <summary>
        /// B5–B8 Phase 2 (Plan 65): wire a Pure Sine Solar Inverter so the
        /// concentrator's electrical output can feed the shelter grid. Requires
        /// the canonical <see cref="ItemGridTieInverter"/> item (consumed once,
        /// on committed success) and the mounted Stirling generator. Research
        /// gates the item through the inverter recipe; the engine checks the
        /// physical item only — the same discipline as AttachStirlingEngine.
        /// </summary>
        public ActionResult ConnectGridTie()
        {
            if (_state.gridTieConnected)
                return ActionResult.Blocked("already_connected", "Grid-tie inverter is already wired.");
            if (!_state.stirlingAttached)
                return ActionResult.Blocked("no_stirling", "Mount the Focal Stirling Engine Generator before wiring a grid-tie inverter.");
            if (_inventory.CountById(ItemGridTieInverter) < 1)
                return ActionResult.Blocked("missing_inverter", "Requires 1x Pure Sine Solar Inverter.");

            _inventory.TryConsumeById(ItemGridTieInverter, 1);
            _state.gridTieConnected = true;
            _log?.Info("[SolarConcentrator] Pure-sine inverter wired. Electrical output now feeds the shelter grid.");
            OnSolarOutputChanged?.Invoke(_state.currentThermalKw, _state.currentElectricalKw);
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("grid_tie_connected");
        }

        /// <summary>
        /// B5–B8 Phase 2: grid-tie feed watts for the host to publish under
        /// <see cref="PowerSourceId"/>. Deterministic from saved state: 0
        /// unless an inverter is connected; the day's electrical output is
        /// already weather-derived (never rolls RNG here).
        /// </summary>
        public float GridFeedWatts => _state.gridTieConnected
            ? MathF.Max(0f, _state.currentElectricalKw) * 1000f
            : 0f;

        public ActionResult PerformSolarDistillation(WaterTreatmentSystem waterSystem, float inputBrackishWater)
        {
            if (waterSystem == null)
                return ActionResult.Failed("null_water_system", "solar.null_water_system");

            if (_state.currentThermalKw < 2.0f)
                return ActionResult.Blocked("insufficient_thermal_energy", $"Requires at least 2.0 kW solar thermal energy (current: {_state.currentThermalKw:F1} kW).");

            if (waterSystem.BrackishWater < inputBrackishWater)
                return ActionResult.Blocked("insufficient_brackish_water", "Not enough brackish water to distill.");

            // Consume brackish water and produce clean water at 75% efficiency without consuming coal/fuel!
            waterSystem.RemoveWater(WaterType.Brackish, inputBrackishWater);
            float cleanYield = (float)Math.Round(inputBrackishWater * 0.75f, 1);
            waterSystem.AddWater(WaterType.Clean, cleanYield);

            _log?.Info($"[SolarConcentrator] Solar distillation completed: {inputBrackishWater} brackish -> {cleanYield} clean water.");
            return ActionResult.Success("solar_distillation_complete");
        }

        public SolarConcentratorState CaptureState()
        {
            return new SolarConcentratorState
            {
                concentratorId = _state.concentratorId,
                isDeployed = _state.isDeployed,
                mirrorReflectivity = _state.mirrorReflectivity,
                surfaceCondition = _state.surfaceCondition,
                alignmentQuality = _state.alignmentQuality,
                trackingMode = _state.trackingMode,
                stirlingAttached = _state.stirlingAttached,
                gridTieConnected = _state.gridTieConnected,
                currentThermalKw = _state.currentThermalKw,
                currentElectricalKw = _state.currentElectricalKw,
                daysCleaned = _state.daysCleaned,
                lastProcessedDay = _state.lastProcessedDay
            };
        }

        public void RestoreState(SolarConcentratorState? state)
        {
            if (state == null) return;
            _state = new SolarConcentratorState
            {
                concentratorId = state.concentratorId,
                isDeployed = state.isDeployed,
                mirrorReflectivity = state.mirrorReflectivity,
                surfaceCondition = state.surfaceCondition,
                alignmentQuality = state.alignmentQuality,
                trackingMode = state.trackingMode,
                stirlingAttached = state.stirlingAttached,
                gridTieConnected = state.gridTieConnected,
                currentThermalKw = state.currentThermalKw,
                currentElectricalKw = state.currentElectricalKw,
                daysCleaned = state.daysCleaned,
                lastProcessedDay = state.lastProcessedDay
            };
            OnStateChanged?.Invoke(_state);
        }
    }
}
