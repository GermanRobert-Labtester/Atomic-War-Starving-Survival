// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Shelter
{
    public enum GeothermalLeakState
    {
        Normal = 0,
        MinorLeak = 1,
        MajorLeak = 2,
        EmergencyIsolation = 3
    }

    [Serializable]
    public sealed class GeothermalStratumDefinition
    {
        [JsonPropertyName("stratum_id")]
        public string StratumId { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("depth_m")]
        public float DepthM { get; set; }

        [JsonPropertyName("base_rock_temperature_c")]
        public float BaseRockTemperatureC { get; set; }

        [JsonPropertyName("thermal_capacity_factor")]
        public float ThermalCapacityFactor { get; set; } = 1f;

        [JsonPropertyName("enthalpy_kj_per_kg")]
        public float EnthalpyKjPerKg { get; set; } = 1000f;

        [JsonPropertyName("brine_mineral_index")]
        public float BrineMineralIndex { get; set; }

        [JsonPropertyName("silica_scaling_factor")]
        public float SilicaScalingFactor { get; set; } = 1f;

        [JsonPropertyName("corrosion_factor")]
        public float CorrosionFactor { get; set; } = 1f;

        [JsonPropertyName("recovery_rate_per_day")]
        public float RecoveryRatePerDay { get; set; } = 1f;

        [JsonPropertyName("max_safe_extraction_kw")]
        public float MaxSafeExtractionKw { get; set; } = 100f;

        [JsonPropertyName("required_excavation_tier")]
        public int RequiredExcavationTier { get; set; }

        [JsonPropertyName("room_id")]
        public string RoomId { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class GeothermalStrataCatalog
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("strata")]
        public List<GeothermalStratumDefinition> Strata { get; set; } =
            new List<GeothermalStratumDefinition>();
    }

    [Serializable]
    public sealed class GeothermalLoopState
    {
        public string LoopId = string.Empty;
        public string StratumId = string.Empty;
        public string RoomId = string.Empty;
        public float BoreholeDepthM;
        public float BrineInletTempC;
        public float BrineOutletTempC;
        public float BrineFlowLPerMin;
        public float WorkingFluidCharge = 100f; // percent of authored charge
        public float HeatExchangerFoulingPct;
        public float TurbineConditionPct = 100f;
        public float BearingConditionPct = 100f;
        public float VibrationIndex;
        public float BedrockThermalReserve = 100f; // normalized percent
        public float OperatingHours;
        public bool Active;
        public bool BypassOpen;
        public GeothermalLeakState LeakState;
        public int LastMaintenanceDay = -1;
        public int LastLeakDay = -1;
        public float LastElectricalOutputKw;
        public float LastWasteHeatKw;
        public float WasteHeatAllocatedKw;
        public int LastProcessedDay = -1;
    }

    [Serializable]
    public sealed class GeothermalOrcState
    {
        public string SystemId = GeothermalOrcSystem.SystemId;
        public List<GeothermalLoopState> Loops = new List<GeothermalLoopState>();
        public string ActiveLoopId = string.Empty;
        public int LastProcessedDay = -1;
    }

    public sealed class GeothermalOrcSnapshot
    {
        public string LoopId = string.Empty;
        public bool Active;
        public float ElectricalOutputKw;
        public float WasteHeatKw;
        public float AvailableWasteHeatKw;
        public float FoulingPct;
        public float ThermalReservePct;
        public float TurbineConditionPct;
        public float BearingConditionPct;
        public float WorkingFluidChargePct;
        public GeothermalLeakState LeakState;
    }

    /// <summary>
    /// Deterministic game-scale geothermal ORC model. It owns loop state and
    /// thermodynamic abstraction only. Power, heating, water, and hazard systems
    /// remain authorities and consume its projections through host adapters.
    /// </summary>
    public sealed class GeothermalOrcSystem
    {
        public const string SystemId = "geothermal_orc";
        public const string PowerSourceId = "geothermal_orc";
        public const float OrcEfficiency = 0.12f;
        public const float CondenserRecoveryFraction = 0.82f;
        public const float MaxFlowLPerMin = 500f;
        public const string DescaleItemId = "item_geothermal_descaling_kit";

        private GeothermalOrcState _state;
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Inventory.Inventory? _inventory;
        private readonly Dictionary<string, GeothermalStratumDefinition> _strata =
            new Dictionary<string, GeothermalStratumDefinition>(StringComparer.Ordinal);

        public GeothermalOrcSystem(
            ISeededRng rng,
            GeothermalOrcState? state = null,
            Inventory.Inventory? inventory = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _state = state ?? new GeothermalOrcState();
            _inventory = inventory;
            _log = log ?? NullLog.Instance;
            NormalizeState();
        }

        public GeothermalOrcState State => _state;
        public IReadOnlyDictionary<string, GeothermalStratumDefinition> Strata => _strata;
        public string ActiveLoopId => _state.ActiveLoopId;
        public GeothermalLoopState? ActiveLoop => FindLoop(_state.ActiveLoopId);
        public float ElectricalOutputKw => ActiveLoop?.LastElectricalOutputKw ?? 0f;
        public float AvailableWasteHeatKw
        {
            get
            {
                var loop = ActiveLoop;
                return loop == null
                    ? 0f
                    : Math.Max(0f, loop.LastWasteHeatKw - loop.WasteHeatAllocatedKw);
            }
        }

        public event Action<GeothermalLoopState>? OnLoopChanged;
        public event Action<string>? OnLeak;
        public event Action<string>? OnLoopIsolated;

        public void LoadCatalog(GeothermalStrataCatalog? catalog)
        {
            if (catalog?.Strata == null) return;
            _strata.Clear();
            foreach (var definition in catalog.Strata)
            {
                if (definition == null || string.IsNullOrWhiteSpace(definition.StratumId))
                    continue;
                if (definition.DepthM <= 0f || definition.EnthalpyKjPerKg <= 0f ||
                    definition.MaxSafeExtractionKw <= 0f)
                    continue;
                _strata[definition.StratumId] = definition;
            }
        }

        public void RegisterStratum(GeothermalStratumDefinition definition)
        {
            if (definition == null || string.IsNullOrWhiteSpace(definition.StratumId))
                return;
            _strata[definition.StratumId] = definition;
        }

        public bool AddLoop(string loopId, string stratumId, string roomId = "")
        {
            if (string.IsNullOrWhiteSpace(loopId) || string.IsNullOrWhiteSpace(stratumId))
                return false;
            if (FindLoop(loopId) != null) return false;
            if (_strata.Count > 0 && !_strata.ContainsKey(stratumId)) return false;

            _state.Loops.Add(new GeothermalLoopState
            {
                LoopId = loopId,
                StratumId = stratumId,
                RoomId = roomId ?? string.Empty,
                BedrockThermalReserve = 100f,
                WorkingFluidCharge = 100f
            });
            if (string.IsNullOrEmpty(_state.ActiveLoopId))
                _state.ActiveLoopId = loopId;
            return true;
        }

        public ActionResult CommissionLoop(string loopId)
        {
            var loop = FindLoop(loopId);
            if (loop == null)
                return ActionResult.Failed("unknown_loop", "geothermal_orc.unknown_loop");
            if (_strata.Count > 0 && !_strata.ContainsKey(loop.StratumId))
                return ActionResult.Blocked("unknown_stratum", "geothermal_orc.unknown_stratum");
            if (loop.WorkingFluidCharge <= 0f)
                return ActionResult.Blocked("no_working_fluid", "geothermal_orc.no_working_fluid");
            loop.Active = false;
            loop.BypassOpen = false;
            loop.LeakState = GeothermalLeakState.Normal;
            _state.ActiveLoopId = loop.LoopId;
            loop.Active = true;
            RaiseChanged(loop);
            return ActionResult.Success("geothermal_orc.loop_commissioned");
        }

        public ActionResult SelectActiveLoop(string loopId)
        {
            var loop = FindLoop(loopId);
            if (loop == null)
                return ActionResult.Failed("unknown_loop", "geothermal_orc.unknown_loop");
            foreach (var other in _state.Loops)
                other.Active = false;
            _state.ActiveLoopId = loopId;
            loop.Active = true;
            RaiseChanged(loop);
            return ActionResult.Success("geothermal_orc.loop_selected");
        }

        public ActionResult SetFlow(string loopId, float flowLPerMin)
        {
            var loop = FindLoop(loopId);
            if (loop == null)
                return ActionResult.Failed("unknown_loop", "geothermal_orc.unknown_loop");
            if (float.IsNaN(flowLPerMin) || float.IsInfinity(flowLPerMin))
                return ActionResult.Failed("invalid_flow", "geothermal_orc.invalid_flow");
            loop.BrineFlowLPerMin = Math.Clamp(flowLPerMin, 0f, MaxFlowLPerMin);
            RaiseChanged(loop);
            return ActionResult.Success("geothermal_orc.flow_set");
        }

        public ActionResult SetBypass(string loopId, bool open)
        {
            var loop = FindLoop(loopId);
            if (loop == null)
                return ActionResult.Failed("unknown_loop", "geothermal_orc.unknown_loop");
            loop.BypassOpen = open;
            RaiseChanged(loop);
            return ActionResult.Success("geothermal_orc.bypass_set");
        }

        /// <summary>Run one campaign-day operating milestone.</summary>
        public GeothermalOrcSnapshot OperateDay(
            int day,
            float operatorEfficiency = 1f,
            float specialistWearModifier = 1f)
        {
            if (day <= _state.LastProcessedDay)
                return Snapshot();
            _state.LastProcessedDay = day;

            var active = ActiveLoop;
            foreach (var loop in _state.Loops)
            {
                if (!ReferenceEquals(loop, active))
                {
                    var definition = GetDefinition(loop);
                    float recovery = definition?.RecoveryRatePerDay ?? 1f;
                    loop.BedrockThermalReserve = Math.Min(100f,
                        loop.BedrockThermalReserve + Math.Max(0f, recovery));
                    loop.WasteHeatAllocatedKw = 0f;
                    loop.LastElectricalOutputKw = 0f;
                    loop.LastWasteHeatKw = 0f;
                    loop.LastProcessedDay = day;
                    continue;
                }

                OperateLoop(loop, day, operatorEfficiency, specialistWearModifier);
            }

            if (active != null) RaiseChanged(active);
            return Snapshot();
        }

        public GeothermalOrcSnapshot TickDay(int day) => OperateDay(day);

        /// <summary>
        /// Allocate condenser heat exactly once for the current operating
        /// milestone. The returned value is what a thermal/water adapter may
        /// pass to its own authority.
        /// </summary>
        public float RequestHeatAllocation(float requestedKw)
        {
            var loop = ActiveLoop;
            if (loop == null || requestedKw <= 0f) return 0f;
            float allocation = Math.Min(requestedKw, AvailableWasteHeatKw);
            loop.WasteHeatAllocatedKw += allocation;
            return allocation;
        }

        public ActionResult Descale(string loopId, float amount = 25f)
        {
            var loop = FindLoop(loopId);
            if (loop == null)
                return ActionResult.Failed("unknown_loop", "geothermal_orc.unknown_loop");
            if (loop.HeatExchangerFoulingPct <= 0f)
                return ActionResult.Blocked("no_fouling", "geothermal_orc.no_fouling");
            int units = Math.Max(1, (int)Math.Ceiling(Math.Max(0f, amount) / 25f));
            if (_inventory != null && !_inventory.TryConsume(DescaleItemId, units))
                return ActionResult.Blocked("missing_descaler", "geothermal_orc.missing_descaler");
            loop.HeatExchangerFoulingPct = Math.Max(0f,
                loop.HeatExchangerFoulingPct - Math.Max(0f, amount));
            loop.LastMaintenanceDay = _state.LastProcessedDay;
            RaiseChanged(loop);
            return ActionResult.Success("geothermal_orc.descaled");
        }

        public ActionResult Repair(string loopId, float amount = 20f)
        {
            var loop = FindLoop(loopId);
            if (loop == null)
                return ActionResult.Failed("unknown_loop", "geothermal_orc.unknown_loop");
            loop.TurbineConditionPct = Math.Clamp(loop.TurbineConditionPct + amount, 0f, 100f);
            loop.BearingConditionPct = Math.Clamp(loop.BearingConditionPct + amount, 0f, 100f);
            loop.VibrationIndex = Math.Max(0f, loop.VibrationIndex - amount * 0.5f);
            loop.LastMaintenanceDay = _state.LastProcessedDay;
            if (loop.LeakState == GeothermalLeakState.EmergencyIsolation &&
                loop.WorkingFluidCharge > 10f)
                loop.LeakState = GeothermalLeakState.MinorLeak;
            RaiseChanged(loop);
            return ActionResult.Success("geothermal_orc.repaired");
        }

        public GeothermalOrcSnapshot Snapshot()
        {
            var loop = ActiveLoop;
            if (loop == null) return new GeothermalOrcSnapshot();
            return new GeothermalOrcSnapshot
            {
                LoopId = loop.LoopId,
                Active = loop.Active,
                ElectricalOutputKw = loop.LastElectricalOutputKw,
                WasteHeatKw = loop.LastWasteHeatKw,
                AvailableWasteHeatKw = AvailableWasteHeatKw,
                FoulingPct = loop.HeatExchangerFoulingPct,
                ThermalReservePct = loop.BedrockThermalReserve,
                TurbineConditionPct = loop.TurbineConditionPct,
                BearingConditionPct = loop.BearingConditionPct,
                WorkingFluidChargePct = loop.WorkingFluidCharge,
                LeakState = loop.LeakState
            };
        }

        public GeothermalOrcState CaptureState()
        {
            var serializer = new SystemTextJsonSerializer();
            return serializer.Deserialize<GeothermalOrcState>(serializer.Serialize(_state))
                ?? new GeothermalOrcState();
        }

        public void RestoreState(GeothermalOrcState? saved)
        {
            if (saved == null) return;
            var serializer = new SystemTextJsonSerializer();
            _state = serializer.Deserialize<GeothermalOrcState>(serializer.Serialize(saved))
                ?? new GeothermalOrcState();
            NormalizeState();
        }

        private void OperateLoop(
            GeothermalLoopState loop,
            int day,
            float operatorEfficiency,
            float specialistWearModifier)
        {
            loop.WasteHeatAllocatedKw = 0f;
            loop.LastProcessedDay = day;
            if (!loop.Active || loop.BypassOpen || loop.LeakState == GeothermalLeakState.EmergencyIsolation ||
                loop.WorkingFluidCharge <= 0f)
            {
                loop.LastElectricalOutputKw = 0f;
                loop.LastWasteHeatKw = 0f;
                loop.BrineOutletTempC = loop.BrineInletTempC;
                return;
            }

            var definition = GetDefinition(loop);
            if (definition == null)
            {
                loop.LastElectricalOutputKw = 0f;
                loop.LastWasteHeatKw = 0f;
                return;
            }

            float flow = Math.Clamp(loop.BrineFlowLPerMin, 0f, MaxFlowLPerMin);
            float inlet = loop.BrineInletTempC > 0f
                ? loop.BrineInletTempC
                : definition.BaseRockTemperatureC;
            float deltaC = Math.Max(0f, definition.BaseRockTemperatureC - inlet * 0.25f);
            loop.BrineInletTempC = inlet;
            loop.BrineOutletTempC = Math.Max(inlet, definition.BaseRockTemperatureC - deltaC * 0.35f);

            // Flow is L/min, treated as kg/L. Dividing by 60 converts kJ/min
            // to kW. This is a game abstraction, not a fluid-property solver.
            float availableThermalKw = flow * deltaC * definition.EnthalpyKjPerKg / 60f
                * Math.Clamp(definition.ThermalCapacityFactor, 0f, 2f)
                * Math.Clamp(loop.BedrockThermalReserve / 100f, 0f, 1f);
            float foulingModifier = 1f - Math.Clamp(loop.HeatExchangerFoulingPct / 100f, 0f, 0.85f);
            float turbineModifier = Math.Clamp(loop.TurbineConditionPct / 100f, 0f, 1f);
            float bearingModifier = Math.Clamp(loop.BearingConditionPct / 100f, 0f, 1f);
            float operatorModifier = Math.Clamp(operatorEfficiency, 0.75f, 1.15f);
            float chargeModifier = Math.Clamp(loop.WorkingFluidCharge / 100f, 0f, 1f);
            float electrical = availableThermalKw * OrcEfficiency * foulingModifier *
                turbineModifier * bearingModifier * operatorModifier * chargeModifier;
            electrical = Math.Min(Math.Max(0f, electrical), definition.MaxSafeExtractionKw);
            loop.LastElectricalOutputKw = electrical;
            loop.LastWasteHeatKw = Math.Max(0f, availableThermalKw - electrical) *
                CondenserRecoveryFraction;

            float load = definition.MaxSafeExtractionKw <= 0f
                ? 0f
                : Math.Clamp(electrical / definition.MaxSafeExtractionKw, 0f, 1f);
            float highLoad = Math.Max(0f, load - 0.75f);
            loop.OperatingHours += 24f;
            loop.HeatExchangerFoulingPct = Math.Min(100f,
                loop.HeatExchangerFoulingPct +
                (0.15f + definition.BrineMineralIndex * 0.03f +
                 definition.SilicaScalingFactor * 0.05f) * Math.Max(0.25f, load));
            loop.BedrockThermalReserve = Math.Max(0f,
                loop.BedrockThermalReserve -
                (0.04f + load * 0.16f) * Math.Max(0.1f, flow / 100f));
            loop.VibrationIndex = Math.Clamp(
                highLoad * 70f + (100f - loop.BearingConditionPct) * 0.4f +
                loop.HeatExchangerFoulingPct * 0.15f, 0f, 100f);
            float wear = (0.04f + load * 0.18f) *
                Math.Clamp(specialistWearModifier, 0.65f, 1.25f);
            loop.TurbineConditionPct = Math.Max(0f, loop.TurbineConditionPct - wear);
            loop.BearingConditionPct = Math.Max(0f, loop.BearingConditionPct - wear * 0.8f);
            loop.WorkingFluidCharge = Math.Max(0f,
                loop.WorkingFluidCharge - loop.VibrationIndex * 0.0025f *
                Math.Clamp(definition.CorrosionFactor, 0.25f, 2f));

            if (loop.LastLeakDay != day && loop.LeakState == GeothermalLeakState.Normal)
            {
                float leakRisk = Math.Clamp(
                    (100f - loop.TurbineConditionPct) * 0.0015f +
                    loop.VibrationIndex * 0.0008f +
                    loop.OperatingHours * 0.000002f, 0f, 0.35f);
                if (_rng.NextDouble() < leakRisk)
                {
                    loop.LastLeakDay = day;
                    loop.LeakState = leakRisk > 0.18f
                        ? GeothermalLeakState.MajorLeak
                        : GeothermalLeakState.MinorLeak;
                    loop.WorkingFluidCharge = Math.Max(0f,
                        loop.WorkingFluidCharge - (loop.LeakState == GeothermalLeakState.MajorLeak ? 25f : 8f));
                    if (loop.LeakState == GeothermalLeakState.MajorLeak)
                    {
                        loop.Active = false;
                        loop.LastElectricalOutputKw = 0f;
                        loop.LastWasteHeatKw = 0f;
                        OnLoopIsolated?.Invoke(loop.LoopId);
                    }
                    OnLeak?.Invoke(loop.LoopId);
                }
            }
        }

        private GeothermalStratumDefinition? GetDefinition(GeothermalLoopState loop) =>
            _strata.TryGetValue(loop.StratumId, out var definition) ? definition : null;

        private GeothermalLoopState? FindLoop(string loopId)
        {
            if (string.IsNullOrEmpty(loopId)) return null;
            for (int i = 0; i < _state.Loops.Count; i++)
                if (string.Equals(_state.Loops[i].LoopId, loopId, StringComparison.Ordinal))
                    return _state.Loops[i];
            return null;
        }

        private void RaiseChanged(GeothermalLoopState loop) => OnLoopChanged?.Invoke(loop);

        private void NormalizeState()
        {
            _state.Loops ??= new List<GeothermalLoopState>();
            for (int i = _state.Loops.Count - 1; i >= 0; i--)
            {
                var loop = _state.Loops[i];
                if (loop == null || string.IsNullOrWhiteSpace(loop.LoopId))
                {
                    _state.Loops.RemoveAt(i);
                    continue;
                }
                loop.WorkingFluidCharge = Math.Clamp(loop.WorkingFluidCharge, 0f, 100f);
                loop.HeatExchangerFoulingPct = Math.Clamp(loop.HeatExchangerFoulingPct, 0f, 100f);
                loop.TurbineConditionPct = Math.Clamp(loop.TurbineConditionPct, 0f, 100f);
                loop.BearingConditionPct = Math.Clamp(loop.BearingConditionPct, 0f, 100f);
                loop.BedrockThermalReserve = Math.Clamp(loop.BedrockThermalReserve, 0f, 100f);
                loop.WasteHeatAllocatedKw = Math.Max(0f, loop.WasteHeatAllocatedKw);
            }

            if (FindLoop(_state.ActiveLoopId) == null && _state.Loops.Count > 0)
                _state.ActiveLoopId = _state.Loops[0].LoopId;
            foreach (var loop in _state.Loops)
                loop.Active = string.Equals(loop.LoopId, _state.ActiveLoopId, StringComparison.Ordinal) &&
                    loop.Active;
        }
    }

    public static class GeothermalStrataCatalogLoader
    {
        public const string FileName = "geothermal_strata_catalog.json";

        public static GeothermalStrataCatalog? Load(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(dataDir))
                throw new ArgumentNullException(nameof(dataDir));
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            string path = Path.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return null;
            try
            {
                return serializer.Deserialize<GeothermalStrataCatalog>(fileIO.ReadAllText(path));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load {FileName}: {ex.Message}", ex);
            }
        }
    }
}
