// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.World;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Active minefield breach operation underway along a specific route corridor.
    /// </summary>
    [Serializable]
    public sealed class ActiveBreachState
    {
        public string RouteId { get; set; } = string.Empty;
        public string SegmentId { get; set; } = string.Empty;
        public float ProgressMeters { get; set; } = 0f;
        public float TargetMeters { get; set; } = 1000f;
        public float MineDensity01 { get; set; } = 0.8f;
        public float ResidualMineRisk01 { get; set; } = 0.8f;
        public int StartedDay { get; set; } = 1;
        public string OperatorId { get; set; } = string.Empty;
        public ProcessState Status { get; set; } = ProcessState.Running;

        public ActiveBreachState Clone() => new ActiveBreachState
        {
            RouteId = RouteId,
            SegmentId = SegmentId,
            ProgressMeters = ProgressMeters,
            TargetMeters = TargetMeters,
            MineDensity01 = MineDensity01,
            ResidualMineRisk01 = ResidualMineRisk01,
            StartedDay = StartedDay,
            OperatorId = OperatorId,
            Status = Status
        };
    }

    /// <summary>
    /// Persistent state DTO for a vehicle-mounted Mine-Clearing Flail module.
    /// </summary>
    [Serializable]
    public sealed class MineClearingFlailState
    {
        public int SchemaVersion { get; set; } = 1;
        public string ModuleInstanceId { get; set; } = "flail_inst_01";
        public string VehicleId { get; set; } = "vehicle_cargo_truck";
        public string ModuleDefinitionId { get; set; } = "module_heavy_flail_m1";
        public float Condition01 { get; set; } = 1.0f;
        public float DrumRpm { get; set; } = 300.0f;
        public float HydraulicPressureBar { get; set; } = 180.0f;
        public int ChainLinksRemaining { get; set; } = 40;
        public float BlastShieldIntegrity01 { get; set; } = 1.0f;
        public float TotalClearedDistanceKm { get; set; } = 0.0f;
        public ActiveBreachState? ActiveBreach { get; set; }
        public List<string> MaintenanceFlags { get; set; } = new List<string>();

        public MineClearingFlailState Clone()
        {
            return new MineClearingFlailState
            {
                SchemaVersion = SchemaVersion,
                ModuleInstanceId = ModuleInstanceId,
                VehicleId = VehicleId,
                ModuleDefinitionId = ModuleDefinitionId,
                Condition01 = Condition01,
                DrumRpm = DrumRpm,
                HydraulicPressureBar = HydraulicPressureBar,
                ChainLinksRemaining = ChainLinksRemaining,
                BlastShieldIntegrity01 = BlastShieldIntegrity01,
                TotalClearedDistanceKm = TotalClearedDistanceKm,
                ActiveBreach = ActiveBreach?.Clone(),
                MaintenanceFlags = new List<string>(MaintenanceFlags)
            };
        }
    }

    /// <summary>
    /// Catalog definition of a mine-clearing flail module.
    /// </summary>
    public sealed class MineClearingFlailDef
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public List<string> CompatibleVehicleTags { get; set; } = new List<string>();
        public float NominalRpm { get; set; } = 300.0f;
        public float ClearedWidthM { get; set; } = 3.5f;
        public float NominalBreachSpeedKph { get; set; } = 8.0f;
        public int ChainLinkCapacity { get; set; } = 40;
        public float ChainWearPerKm { get; set; } = 2.0f;
        public float HydraulicPressureNominal { get; set; } = 180.0f;
        public float MineClearanceEfficiency { get; set; } = 0.94f;
        public float ObstacleStallRisk { get; set; } = 0.05f;
        // Authored minimum residual risk after full clearance (mechanical flails
        // rarely certify a field to zero).
        public float ResidualRiskFloor { get; set; } = 0.02f;
    }

    /// <summary>
    /// Core simulation engine for Plan 147 — Mine-Clearing Flail Vehicle Module.
    /// Pure C# model with zero engine dependencies.
    /// </summary>
    public sealed class MineClearingFlailEngine : IAdvancedMachineOperation
    {
        private readonly MineClearingFlailState _state;
        private readonly Dictionary<string, MineClearingFlailDef> _modules =
            new Dictionary<string, MineClearingFlailDef>(StringComparer.Ordinal);

        public event Action<MineClearingFlailState>? OnStateChanged;
        public event Action<ActiveBreachState>? OnBreachStarted;
        public event Action<ActiveBreachState>? OnBreachCompleted;
        public event Action<string>? OnFlailIncident;

        public bool IsOperational =>
            _state.Condition01 > 0.2f &&
            _state.ChainLinksRemaining >= 8 &&
            _state.HydraulicPressureBar >= 100.0f &&
            _state.BlastShieldIntegrity01 >= 0.2f &&
            !_state.MaintenanceFlags.Contains("drum_stalled");

        public float Condition01 => _state.Condition01;
        public string ActiveJobId => _state.ActiveBreach != null ? $"{_state.ActiveBreach.RouteId}:{_state.ActiveBreach.SegmentId}" : string.Empty;
        public ProcessState State => _state.ActiveBreach?.Status ?? ProcessState.Idle;

        public MineClearingFlailState StateDto => _state;

        public MineClearingFlailEngine(MineClearingFlailState? initialState = null)
        {
            _state = initialState?.Clone() ?? new MineClearingFlailState();
            SeedDefaultDefinitions();
        }

        private void SeedDefaultDefinitions()
        {
            RegisterModule(new MineClearingFlailDef
            {
                Id = "module_heavy_flail_m1",
                DisplayName = "M1 Heavy Demining Flail Rotor",
                CompatibleVehicleTags = new List<string> { "cargo_truck", "steam_halftrack", "armored_mobile_base" },
                NominalRpm = 300.0f,
                ClearedWidthM = 3.5f,
                NominalBreachSpeedKph = 8.0f,
                ChainLinkCapacity = 40,
                ChainWearPerKm = 2.0f,
                HydraulicPressureNominal = 180.0f,
                MineClearanceEfficiency = 0.94f,
                ObstacleStallRisk = 0.05f
            });
            RegisterModule(new MineClearingFlailDef
            {
                Id = "module_light_scout_flail",
                DisplayName = "Scout Wire-Rupture Flail",
                CompatibleVehicleTags = new List<string> { "utility_quad", "steam_halftrack" },
                NominalRpm = 400.0f,
                ClearedWidthM = 2.0f,
                NominalBreachSpeedKph = 12.0f,
                ChainLinkCapacity = 24,
                ChainWearPerKm = 3.5f,
                HydraulicPressureNominal = 140.0f,
                MineClearanceEfficiency = 0.85f,
                ObstacleStallRisk = 0.08f
            });
        }

        public void RegisterModule(MineClearingFlailDef def)
        {
            if (def != null && !string.IsNullOrEmpty(def.Id))
            {
                _modules[def.Id] = def;
            }
        }

        public bool CanStartBreach(string routeId, string segmentId, RouteInfrastructureSystem routeSystem, out string reason)
        {
            if (!IsOperational)
            {
                reason = "flail_module_inoperable";
                return false;
            }
            if (_state.ActiveBreach != null && _state.ActiveBreach.Status == ProcessState.Running)
            {
                reason = "breach_already_in_progress";
                return false;
            }
            var seg = routeSystem.FindSegment(routeId, segmentId);
            if (seg == null || seg.MinefieldState.Density01 <= 0f)
            {
                reason = "no_minefield_detected_on_segment";
                return false;
            }
            if (seg.MinefieldState.ClearedFraction01 >= 0.99f)
            {
                reason = "segment_already_cleared";
                return false;
            }
            reason = "ok";
            return true;
        }

        public bool StartBreach(
            string routeId,
            string segmentId,
            string operatorId,
            int currentDay,
            float segmentLengthMeters,
            RouteInfrastructureSystem routeSystem,
            out string failureReason)
        {
            if (!CanStartBreach(routeId, segmentId, routeSystem, out failureReason))
            {
                return false;
            }

            var seg = routeSystem.FindSegment(routeId, segmentId)!;
            _state.ActiveBreach = new ActiveBreachState
            {
                RouteId = routeId,
                SegmentId = segmentId,
                ProgressMeters = seg.MinefieldState.ClearedFraction01 * segmentLengthMeters,
                TargetMeters = segmentLengthMeters,
                MineDensity01 = seg.MinefieldState.Density01,
                ResidualMineRisk01 = seg.MinefieldState.ResidualRisk01,
                StartedDay = currentDay,
                OperatorId = operatorId,
                Status = ProcessState.Running
            };

            OnBreachStarted?.Invoke(_state.ActiveBreach);
            OnStateChanged?.Invoke(_state);
            return true;
        }

        public void TickBreach(
            float hours,
            int currentDay,
            RouteInfrastructureSystem routeSystem,
            AdvancedMachineOperatorContext? op,
            ISeededRng rng)
        {
            if (_state.ActiveBreach == null || _state.ActiveBreach.Status != ProcessState.Running)
                return;

            if (!_modules.TryGetValue(_state.ModuleDefinitionId, out var def))
            {
                def = _modules["module_heavy_flail_m1"];
            }

            float chainFactor = Math.Clamp((float)_state.ChainLinksRemaining / def.ChainLinkCapacity, 0.2f, 1.0f);
            float hydraulicFactor = Math.Clamp(_state.HydraulicPressureBar / Math.Max(1.0f, def.HydraulicPressureNominal), 0.3f, 1.0f);
            float opFactor = op?.EffectiveEfficiency ?? 1.0f;

            float speedKph = def.NominalBreachSpeedKph * chainFactor * hydraulicFactor * opFactor;
            float metersCleared = speedKph * hours * 1000.0f;

            float oldProgress = _state.ActiveBreach.ProgressMeters;
            _state.ActiveBreach.ProgressMeters = Math.Min(_state.ActiveBreach.TargetMeters, oldProgress + metersCleared);
            float actualDeltaMeters = _state.ActiveBreach.ProgressMeters - oldProgress;
            float actualDeltaKm = actualDeltaMeters / 1000.0f;

            _state.TotalClearedDistanceKm += actualDeltaKm;

            // Wear chains and hydraulic pressure
            float chainLossRoll = def.ChainWearPerKm * actualDeltaKm;
            if (rng.NextFloat() < chainLossRoll && _state.ChainLinksRemaining > 0)
            {
                _state.ChainLinksRemaining--;
                OnFlailIncident?.Invoke("chain_link_severed");
            }

            // Deflagrations chip blast shield
            if (_state.ActiveBreach.MineDensity01 > 0.3f && rng.NextFloat() < _state.ActiveBreach.MineDensity01 * actualDeltaKm)
            {
                _state.BlastShieldIntegrity01 = Math.Clamp(_state.BlastShieldIntegrity01 - 0.05f, 0.0f, 1.0f);
                OnFlailIncident?.Invoke("mine_detonated");
            }

            // Route Infrastructure Mutation Authority
            routeSystem.ApplyMineClearance(
                _state.ActiveBreach.RouteId,
                _state.ActiveBreach.SegmentId,
                actualDeltaMeters,
                _state.ActiveBreach.TargetMeters,
                def.MineClearanceEfficiency,
                currentDay,
                def.ResidualRiskFloor);

            var seg = routeSystem.FindSegment(_state.ActiveBreach.RouteId, _state.ActiveBreach.SegmentId);
            if (seg != null)
            {
                _state.ActiveBreach.ResidualMineRisk01 = seg.MinefieldState.ResidualRisk01;
            }

            if (_state.ChainLinksRemaining < 8)
            {
                _state.ActiveBreach.Status = ProcessState.MaintenanceRequired;
                OnFlailIncident?.Invoke("chains_depleted");
            }
            else if (_state.ActiveBreach.ProgressMeters >= _state.ActiveBreach.TargetMeters)
            {
                _state.ActiveBreach.Status = ProcessState.Completed;
                OnBreachCompleted?.Invoke(_state.ActiveBreach);
            }

            OnStateChanged?.Invoke(_state);
        }

        public void PerformMaintenance(string type)
        {
            if (!_modules.TryGetValue(_state.ModuleDefinitionId, out var def))
            {
                def = _modules["module_heavy_flail_m1"];
            }
            switch (type)
            {
                case "replace_chains":
                    _state.ChainLinksRemaining = def.ChainLinkCapacity;
                    _state.Condition01 = Math.Clamp(_state.Condition01 + 0.2f, 0.1f, 1.0f);
                    break;
                case "service_hydraulics":
                    _state.HydraulicPressureBar = def.HydraulicPressureNominal;
                    _state.MaintenanceFlags.Remove("drum_stalled");
                    break;
                case "repair_blast_shield":
                    _state.BlastShieldIntegrity01 = 1.0f;
                    break;
            }
            OnStateChanged?.Invoke(_state);
        }

        public MineClearingFlailState CaptureState() => _state.Clone();

        public void RestoreState(MineClearingFlailState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.ModuleInstanceId = saved.ModuleInstanceId;
            _state.VehicleId = saved.VehicleId;
            _state.ModuleDefinitionId = saved.ModuleDefinitionId;
            _state.Condition01 = saved.Condition01;
            _state.DrumRpm = saved.DrumRpm;
            _state.HydraulicPressureBar = saved.HydraulicPressureBar;
            _state.ChainLinksRemaining = saved.ChainLinksRemaining;
            _state.BlastShieldIntegrity01 = saved.BlastShieldIntegrity01;
            _state.TotalClearedDistanceKm = saved.TotalClearedDistanceKm;
            _state.ActiveBreach = saved.ActiveBreach?.Clone();
            _state.MaintenanceFlags = new List<string>(saved.MaintenanceFlags);
            OnStateChanged?.Invoke(_state);
        }
    }
}
