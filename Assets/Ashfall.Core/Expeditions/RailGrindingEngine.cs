// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.World;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Active rail corridor reprofiling job.
    /// </summary>
    [Serializable]
    public sealed class ActiveRailGrindingJob
    {
        public string RouteId { get; set; } = string.Empty;
        public string SegmentId { get; set; } = string.Empty;
        public string OperatorId { get; set; } = string.Empty;
        public float ProgressKm { get; set; } = 0f;
        public float TargetKm { get; set; } = 10f;
        public int PassesCompleted { get; set; } = 0;
        public float CurrentRoughness { get; set; } = 0.85f;
        public ProcessState Status { get; set; } = ProcessState.Running;
        public string FailureCode { get; set; } = string.Empty;

        public ActiveRailGrindingJob Clone() => new ActiveRailGrindingJob
        {
            RouteId = RouteId,
            SegmentId = SegmentId,
            OperatorId = OperatorId,
            ProgressKm = ProgressKm,
            TargetKm = TargetKm,
            PassesCompleted = PassesCompleted,
            CurrentRoughness = CurrentRoughness,
            Status = Status,
            FailureCode = FailureCode
        };
    }

    /// <summary>
    /// Persistent state DTO for the Rail Grinding module mounted on a draisine or railcar.
    /// </summary>
    [Serializable]
    public sealed class RailGrindingEngineState
    {
        public int SchemaVersion { get; set; } = 1;
        public string ModuleInstanceId { get; set; } = "grinder_inst_01";
        public string VehicleId { get; set; } = "locomotive_draisine";
        public string GrindingHeadId { get; set; } = "grinding_head_corundum_m4";
        public float Condition01 { get; set; } = 1.0f;
        public float MotorRpm { get; set; } = 3600.0f;
        public float DownforceBar { get; set; } = 6.0f;
        public float StoneDiameterMm { get; set; } = 250.0f;
        public float WaterSuppressorCondition01 { get; set; } = 1.0f;
        public ActiveRailGrindingJob? ActiveJob { get; set; }
        public List<string> MaintenanceFlags { get; set; } = new List<string>();

        public RailGrindingEngineState Clone() => new RailGrindingEngineState
        {
            SchemaVersion = SchemaVersion,
            ModuleInstanceId = ModuleInstanceId,
            VehicleId = VehicleId,
            GrindingHeadId = GrindingHeadId,
            Condition01 = Condition01,
            MotorRpm = MotorRpm,
            DownforceBar = DownforceBar,
            StoneDiameterMm = StoneDiameterMm,
            WaterSuppressorCondition01 = WaterSuppressorCondition01,
            ActiveJob = ActiveJob?.Clone(),
            MaintenanceFlags = new List<string>(MaintenanceFlags)
        };
    }

    /// <summary>
    /// Catalog definition of a rail grinding head.
    /// </summary>
    public sealed class RailGrindingHeadDef
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public List<string> CompatibleVehicleTags { get; set; } = new List<string>();
        public float NominalRpm { get; set; } = 3600.0f;
        public float BaseWorkRateKmPerHour { get; set; } = 4.0f;
        public float StoneWearPerKm { get; set; } = 1.5f;
        public float WaterSuppressionPerKm { get; set; } = 25.0f;
        public float TargetRoughness { get; set; } = 0.20f;
        public float MaxSpeedUpgradeKph { get; set; } = 80.0f;
        public float SparkHazardBase { get; set; } = 0.12f;
        public float StoneShatterBase { get; set; } = 0.04f;
        public float RoughnessReductionPerPass { get; set; } = 0.35f;
        public float SpeedBonusKph { get; set; } = 30.0f;
    }

    /// <summary>
    /// Core simulation engine for Plan 149 — Rail Grinding & Strategic Corridor Rehabilitation.
    /// Pure C# model with zero engine dependencies.
    /// </summary>
    public sealed class RailGrindingEngine : IAdvancedMachineOperation
    {
        private readonly RailGrindingEngineState _state;
        private readonly Dictionary<string, RailGrindingHeadDef> _heads =
            new Dictionary<string, RailGrindingHeadDef>(StringComparer.Ordinal);

        public event Action<RailGrindingEngineState>? OnStateChanged;
        public event Action<ActiveRailGrindingJob>? OnJobStarted;
        public event Action<ActiveRailGrindingJob>? OnJobCompleted;
        public event Action<string>? OnGrindingHazard;

        public bool IsOperational =>
            _state.Condition01 > 0.2f &&
            _state.StoneDiameterMm >= 120.0f &&
            _state.DownforceBar >= 3.0f &&
            !_state.MaintenanceFlags.Contains("stone_shattered");

        public float Condition01 => _state.Condition01;
        public string ActiveJobId => _state.ActiveJob != null ? $"{_state.ActiveJob.RouteId}:{_state.ActiveJob.SegmentId}" : string.Empty;
        public ProcessState State => _state.ActiveJob?.Status ?? ProcessState.Idle;

        public RailGrindingEngineState StateDto => _state;
        public IEnumerable<RailGrindingHeadDef> Heads => _heads.Values;
        public RailGrindingHeadDef? GetHead(string id) => _heads.TryGetValue(id, out var def) ? def : null;

        public RailGrindingEngine(RailGrindingEngineState? initialState = null)
        {
            _state = initialState?.Clone() ?? new RailGrindingEngineState();
            SeedDefaultDefinitions();
        }

        private void SeedDefaultDefinitions()
        {
            RegisterHead(new RailGrindingHeadDef
            {
                Id = "grinding_head_corundum_m4",
                DisplayName = "M4 Corundum Track-Reprofiling Head",
                CompatibleVehicleTags = new List<string> { "locomotive", "handcar", "draisine" },
                NominalRpm = 3600.0f,
                BaseWorkRateKmPerHour = 4.0f,
                StoneWearPerKm = 1.5f,
                WaterSuppressionPerKm = 25.0f,
                TargetRoughness = 0.20f,
                MaxSpeedUpgradeKph = 80.0f,
                SparkHazardBase = 0.12f,
                StoneShatterBase = 0.04f
            });
        }

        public void RegisterHead(RailGrindingHeadDef def)
        {
            if (def != null && !string.IsNullOrEmpty(def.Id))
            {
                _heads[def.Id] = def;
            }
        }

        public bool CanStartGrinding(string routeId, string segmentId, RouteInfrastructureSystem routeSystem, out string reason)
        {
            if (!IsOperational)
            {
                reason = "grinder_inoperable";
                return false;
            }
            if (_state.ActiveJob != null && _state.ActiveJob.Status == ProcessState.Running)
            {
                reason = "grinding_job_already_active";
                return false;
            }
            var def = GetHead(_state.GrindingHeadId);
            if (def == null)
            {
                reason = "unknown_grinding_head";
                return false;
            }
            var seg = routeSystem.FindSegment(routeId, segmentId);
            if (seg == null || seg.Mode != "rail")
            {
                reason = "segment_is_not_rail";
                return false;
            }
            if (seg.RailCondition.RoughnessIndex <= def.TargetRoughness + 0.02f)
            {
                reason = "rail_already_at_target_smoothness";
                return false;
            }
            reason = "ok";
            return true;
        }

        public bool StartGrindingJob(
            string routeId,
            string segmentId,
            string operatorId,
            float segmentLengthKm,
            RouteInfrastructureSystem routeSystem,
            out string failureReason)
        {
            if (!CanStartGrinding(routeId, segmentId, routeSystem, out failureReason))
            {
                return false;
            }

            var seg = routeSystem.FindSegment(routeId, segmentId)!;
            _state.ActiveJob = new ActiveRailGrindingJob
            {
                RouteId = routeId,
                SegmentId = segmentId,
                OperatorId = operatorId,
                ProgressKm = 0f,
                TargetKm = segmentLengthKm,
                PassesCompleted = 0,
                CurrentRoughness = seg.RailCondition.RoughnessIndex,
                Status = ProcessState.Running
            };

            OnJobStarted?.Invoke(_state.ActiveJob);
            OnStateChanged?.Invoke(_state);
            return true;
        }

        public void TickGrinding(
            float hours,
            int currentDay,
            RouteInfrastructureSystem routeSystem,
            AdvancedMachineOperatorContext? op,
            ISeededRng rng)
        {
            if (_state.ActiveJob == null || _state.ActiveJob.Status != ProcessState.Running)
                return;

            if (!_heads.TryGetValue(_state.GrindingHeadId, out var def))
            {
                def = _heads["grinding_head_corundum_m4"];
            }

            float downforceFactor = Math.Clamp(_state.DownforceBar / 6.0f, 0.5f, 1.2f);
            float rpmFactor = Math.Clamp(_state.MotorRpm / def.NominalRpm, 0.5f, 1.3f);
            float stoneFactor = Math.Clamp(_state.StoneDiameterMm / 250.0f, 0.4f, 1.0f);
            float opFactor = op?.EffectiveEfficiency ?? 1.0f;

            float rateKmH = def.BaseWorkRateKmPerHour * downforceFactor * rpmFactor * stoneFactor * opFactor;
            float advanceKm = rateKmH * hours;

            float oldProg = _state.ActiveJob.ProgressKm;
            _state.ActiveJob.ProgressKm = Math.Min(_state.ActiveJob.TargetKm, oldProg + advanceKm);
            float actualDeltaKm = _state.ActiveJob.ProgressKm - oldProg;

            // Wear stone diameter
            _state.StoneDiameterMm = Math.Max(100.0f, _state.StoneDiameterMm - (def.StoneWearPerKm * actualDeltaKm));

            // Hazard checks: spark hazard and stone shatter
            float sparkRisk = def.SparkHazardBase * (1.5f - _state.WaterSuppressorCondition01);
            if (rng.NextFloat() < sparkRisk * actualDeltaKm)
            {
                OnGrindingHazard?.Invoke("spark_flare_hazard");
            }

            if (_state.StoneDiameterMm < 140.0f && rng.NextFloat() < def.StoneShatterBase * actualDeltaKm)
            {
                _state.MaintenanceFlags.Add("stone_shattered");
                _state.ActiveJob.Status = ProcessState.Failed;
                _state.ActiveJob.FailureCode = "stone_shattered";
                OnGrindingHazard?.Invoke("stone_shattered");
                OnStateChanged?.Invoke(_state);
                return;
            }

            // Route Infrastructure Mutation
            routeSystem.ApplyRailGrinding(
                _state.ActiveJob.RouteId,
                _state.ActiveJob.SegmentId,
                actualDeltaKm,
                _state.ActiveJob.TargetKm,
                def.RoughnessReductionPerPass,
                def.SpeedBonusKph,
                def.MaxSpeedUpgradeKph,
                currentDay);

            var seg = routeSystem.FindSegment(_state.ActiveJob.RouteId, _state.ActiveJob.SegmentId);
            if (seg != null)
            {
                _state.ActiveJob.CurrentRoughness = seg.RailCondition.RoughnessIndex;
            }

            if (_state.StoneDiameterMm < 120.0f)
            {
                _state.ActiveJob.Status = ProcessState.MaintenanceRequired;
                OnGrindingHazard?.Invoke("stone_exhausted");
            }
            else if (_state.ActiveJob.ProgressKm >= _state.ActiveJob.TargetKm)
            {
                _state.ActiveJob.PassesCompleted++;
                _state.ActiveJob.Status = ProcessState.Completed;
                OnJobCompleted?.Invoke(_state.ActiveJob);
            }

            OnStateChanged?.Invoke(_state);
        }

        public void PerformMaintenance(string type)
        {
            switch (type)
            {
                case "replace_stones":
                    _state.StoneDiameterMm = 250.0f;
                    _state.MaintenanceFlags.Remove("stone_shattered");
                    _state.Condition01 = Math.Clamp(_state.Condition01 + 0.2f, 0.1f, 1.0f);
                    break;
                case "refill_water_suppression":
                    _state.WaterSuppressorCondition01 = 1.0f;
                    break;
                case "calibrate_cylinders":
                    _state.DownforceBar = 6.0f;
                    break;
            }
            OnStateChanged?.Invoke(_state);
        }

        public RailGrindingEngineState CaptureState() => _state.Clone();

        public void RestoreState(RailGrindingEngineState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.ModuleInstanceId = saved.ModuleInstanceId;
            _state.VehicleId = saved.VehicleId;
            _state.GrindingHeadId = saved.GrindingHeadId;
            _state.Condition01 = saved.Condition01;
            _state.MotorRpm = saved.MotorRpm;
            _state.DownforceBar = saved.DownforceBar;
            _state.StoneDiameterMm = saved.StoneDiameterMm;
            _state.WaterSuppressorCondition01 = saved.WaterSuppressorCondition01;
            _state.ActiveJob = saved.ActiveJob?.Clone();
            _state.MaintenanceFlags = new List<string>(saved.MaintenanceFlags);
            OnStateChanged?.Invoke(_state);
        }
    }
}
