// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.AdvancedMachinery;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Completed record of an EB-PVD coated component.
    /// Can be consumed or installed into generators or vehicle engines.
    /// </summary>
    [Serializable]
    public sealed class EbPvdCoatedRecord
    {
        public string InstanceId { get; set; } = string.Empty;
        public string SubstrateTag { get; set; } = string.Empty;
        public string CoatingId { get; set; } = string.Empty;
        public float ThicknessUm { get; set; } = 125.0f;
        public float Uniformity01 { get; set; } = 0.95f;
        public float SpallationRisk01 { get; set; } = 0.05f;
        public int CompletedDay { get; set; } = 1;
        public float ThermalResistanceBonus { get; set; } = 0.35f;
        public float MaxTempBonusC { get; set; } = 180.0f;
        public float DurabilityBonus { get; set; } = 0.40f;

        public EbPvdCoatedRecord Clone() => new EbPvdCoatedRecord
        {
            InstanceId = InstanceId,
            SubstrateTag = SubstrateTag,
            CoatingId = CoatingId,
            ThicknessUm = ThicknessUm,
            Uniformity01 = Uniformity01,
            SpallationRisk01 = SpallationRisk01,
            CompletedDay = CompletedDay,
            ThermalResistanceBonus = ThermalResistanceBonus,
            MaxTempBonusC = MaxTempBonusC,
            DurabilityBonus = DurabilityBonus
        };
    }

    /// <summary>
    /// Active coating operation inside the EB-PVD chamber.
    /// </summary>
    [Serializable]
    public sealed class EbPvdCoatingJob
    {
        public string JobId { get; set; } = string.Empty;
        public string SubstrateTag { get; set; } = string.Empty;
        public string CoatingId { get; set; } = string.Empty;
        public string OperatorId { get; set; } = string.Empty;
        public int StartedDay { get; set; } = 1;
        public float ProgressHours { get; set; } = 0f;
        public float RequiredHours { get; set; } = 4f;
        public float BeamVoltageKv { get; set; } = 20f;
        public float BeamCurrentA { get; set; } = 0.5f;
        public float BeamPowerKw { get; set; } = 10f;
        public float VacuumMbar { get; set; } = 0.00001f;
        public float SubstrateRotationRpm { get; set; } = 12f;
        public float RasterFrequencyHz { get; set; } = 500f;
        public float CoatingThicknessUm { get; set; } = 0f;
        public bool BondCoatApplied { get; set; } = true;
        public ProcessState Status { get; set; } = ProcessState.Running;
        public string FailureCode { get; set; } = string.Empty;

        public EbPvdCoatingJob Clone() => new EbPvdCoatingJob
        {
            JobId = JobId,
            SubstrateTag = SubstrateTag,
            CoatingId = CoatingId,
            OperatorId = OperatorId,
            StartedDay = StartedDay,
            ProgressHours = ProgressHours,
            RequiredHours = RequiredHours,
            BeamVoltageKv = BeamVoltageKv,
            BeamCurrentA = BeamCurrentA,
            BeamPowerKw = BeamPowerKw,
            VacuumMbar = VacuumMbar,
            SubstrateRotationRpm = SubstrateRotationRpm,
            RasterFrequencyHz = RasterFrequencyHz,
            CoatingThicknessUm = CoatingThicknessUm,
            BondCoatApplied = BondCoatApplied,
            Status = Status,
            FailureCode = FailureCode
        };
    }

    /// <summary>
    /// Persistent state DTO for the EB-PVD Coater machine.
    /// </summary>
    [Serializable]
    public sealed class EbPvdCoatingState
    {
        public int SchemaVersion { get; set; } = 1;
        public float MachineCondition01 { get; set; } = 1.0f;
        public float OperatingHours { get; set; } = 0.0f;
        public float FilamentHours { get; set; } = 0.0f;
        public float VacuumPumpHours { get; set; } = 0.0f;
        public float ChamberShieldingCondition01 { get; set; } = 1.0f;
        public EbPvdCoatingJob? ActiveJob { get; set; }
        public List<EbPvdCoatedRecord> CompletedRecords { get; set; } = new List<EbPvdCoatedRecord>();
        public List<string> MaintenanceFlags { get; set; } = new List<string>();

        public EbPvdCoatingState Clone()
        {
            var copy = new EbPvdCoatingState
            {
                SchemaVersion = SchemaVersion,
                MachineCondition01 = MachineCondition01,
                OperatingHours = OperatingHours,
                FilamentHours = FilamentHours,
                VacuumPumpHours = VacuumPumpHours,
                ChamberShieldingCondition01 = ChamberShieldingCondition01,
                ActiveJob = ActiveJob?.Clone(),
                MaintenanceFlags = new List<string>(MaintenanceFlags)
            };
            foreach (var r in CompletedRecords)
            {
                copy.CompletedRecords.Add(r.Clone());
            }
            return copy;
        }
    }

    /// <summary>
    /// Catalog definition of a TBC recipe.
    /// </summary>
    public sealed class EbPvdCoatingDef
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string CeramicTargetItemId { get; set; } = "item_ebpvd_ceramic_target_ingot";
        public string BondCoatItemId { get; set; } = "item_mcraly_bond_coat_powder";
        public List<string> ValidSubstrateTags { get; set; } = new List<string>();
        public float TargetThicknessUm { get; set; } = 125.0f;
        public float BaseDurationHours { get; set; } = 4.0f;
        public float BasePowerKw { get; set; } = 10.0f;
        public float ThermalResistanceBonus { get; set; } = 0.35f;
        public float MaxTemperatureBonusC { get; set; } = 180.0f;
        public float DurabilityBonus { get; set; } = 0.40f;
        public float SpallationBaseRisk { get; set; } = 0.05f;
    }

    /// <summary>
    /// Authored machine failure profile: probability gate per hour once the
    /// associated maintenance metric passes its threshold.
    /// </summary>
    public sealed class EbPvdFailureProfile
    {
        public string Code { get; set; } = string.Empty;
        public float BaseRisk { get; set; }
        public float ThresholdHours { get; set; }
    }

    /// <summary>
    /// Core simulation engine for Plan 146 — Electron Beam Physical Vapor Deposition (EB-PVD) Coater.
    /// Pure C# model with zero engine dependencies.
    /// </summary>
    public sealed class EbPvdCoatingEngine : IAdvancedMachineOperation
    {
        private readonly EbPvdCoatingState _state;
        private readonly Dictionary<string, EbPvdCoatingDef> _coatings =
            new Dictionary<string, EbPvdCoatingDef>(StringComparer.Ordinal);
        private List<EbPvdFailureProfile> _failureProfiles = new List<EbPvdFailureProfile>();

        public event Action<EbPvdCoatingState>? OnStateChanged;
        public event Action<EbPvdCoatingJob>? OnJobStarted;
        public event Action<EbPvdCoatedRecord>? OnJobCompleted;
        public event Action<string>? OnMachineHazard;

        public bool IsOperational =>
            _state.MachineCondition01 > 0.2f &&
            _state.ChamberShieldingCondition01 >= 0.5f &&
            !_state.MaintenanceFlags.Contains("filament_blown") &&
            !_state.MaintenanceFlags.Contains("vacuum_seal_breached");

        public float Condition01 => _state.MachineCondition01;
        public string ActiveJobId => _state.ActiveJob?.JobId ?? string.Empty;
        public ProcessState State => _state.ActiveJob?.Status ?? ProcessState.Idle;

        public EbPvdCoatingState StateDto => _state;
        public IEnumerable<EbPvdCoatingDef> Coatings => _coatings.Values;

        public EbPvdCoatingEngine(EbPvdCoatingState? initialState = null)
        {
            _state = initialState?.Clone() ?? new EbPvdCoatingState();
            SeedDefaultCoatings();
            SeedDefaultFailureProfiles();
        }

        private void SeedDefaultFailureProfiles()
        {
            // Mirrors ebpvd_coating_catalog.json failure_profiles so a bare engine
            // matches the data authority; the host loader overrides via SetFailureProfiles.
            _failureProfiles = new List<EbPvdFailureProfile>
            {
                new EbPvdFailureProfile { Code = "filament_blowout", BaseRisk = 0.04f, ThresholdHours = 80.0f },
                new EbPvdFailureProfile { Code = "vacuum_loss", BaseRisk = 0.03f, ThresholdHours = 100.0f },
                new EbPvdFailureProfile { Code = "thermal_overheat", BaseRisk = 0.05f, ThresholdHours = 120.0f }
            };
        }

        /// <summary>Replaces the machine failure profiles (catalog authority).</summary>
        public void SetFailureProfiles(IEnumerable<EbPvdFailureProfile>? profiles)
        {
            _failureProfiles = profiles != null
                ? new List<EbPvdFailureProfile>(profiles)
                : new List<EbPvdFailureProfile>();
        }

        public IReadOnlyList<EbPvdFailureProfile> FailureProfiles => _failureProfiles;

        private void SeedDefaultCoatings()
        {
            RegisterCoating(new EbPvdCoatingDef
            {
                Id = "ebpvd_tbc_yttria_stabilized_zirconia",
                DisplayName = "7YSZ Thermal Barrier Coating",
                ValidSubstrateTags = new List<string> { "superalloy_blade", "combustor_liner", "diesel_injector" },
                TargetThicknessUm = 125.0f,
                BaseDurationHours = 4.0f,
                BasePowerKw = 10.0f,
                ThermalResistanceBonus = 0.35f,
                MaxTemperatureBonusC = 180.0f,
                DurabilityBonus = 0.40f,
                SpallationBaseRisk = 0.05f
            });
            RegisterCoating(new EbPvdCoatingDef
            {
                Id = "ebpvd_tbc_gadolinium_zirconate",
                DisplayName = "Pyrochlore Gd2Zr2O7 Advanced TBC",
                ValidSubstrateTags = new List<string> { "superalloy_blade", "combustor_liner" },
                TargetThicknessUm = 150.0f,
                BaseDurationHours = 6.0f,
                BasePowerKw = 11.0f,
                ThermalResistanceBonus = 0.50f,
                MaxTemperatureBonusC = 260.0f,
                DurabilityBonus = 0.30f,
                SpallationBaseRisk = 0.08f
            });
            RegisterCoating(new EbPvdCoatingDef
            {
                Id = "ebpvd_tbc_alumina_barrier",
                DisplayName = "Alpha-Alumina Diffusion Barrier",
                ValidSubstrateTags = new List<string> { "diesel_injector", "superalloy_blade" },
                TargetThicknessUm = 50.0f,
                BaseDurationHours = 3.0f,
                BasePowerKw = 9.5f,
                ThermalResistanceBonus = 0.25f,
                MaxTemperatureBonusC = 120.0f,
                DurabilityBonus = 0.55f,
                SpallationBaseRisk = 0.03f
            });
        }

        public void RegisterCoating(EbPvdCoatingDef def)
        {
            if (def != null && !string.IsNullOrEmpty(def.Id))
            {
                _coatings[def.Id] = def;
            }
        }

        public EbPvdCoatingDef? GetCoating(string coatingId)
        {
            _coatings.TryGetValue(coatingId, out var def);
            return def;
        }

        public static float ComputeBeamPowerKw(float voltageKv, float currentA) =>
            voltageKv * currentA;

        public bool CanStartJob(string coatingId, string substrateTag, out string reason)
        {
            if (!IsOperational)
            {
                reason = "machine_not_operational";
                return false;
            }
            if (_state.ActiveJob != null && _state.ActiveJob.Status == ProcessState.Running)
            {
                reason = "job_already_active";
                return false;
            }
            if (_state.ActiveJob != null && _state.ActiveJob.Status == ProcessState.Paused)
            {
                // A paused job still holds its consumed materials and progress;
                // starting over it would silently destroy both.
                reason = "job_paused_resume_or_fail";
                return false;
            }
            if (!_coatings.TryGetValue(coatingId, out var def))
            {
                reason = "invalid_coating_id";
                return false;
            }
            if (!def.ValidSubstrateTags.Contains(substrateTag))
            {
                reason = "incompatible_substrate";
                return false;
            }
            reason = "ok";
            return true;
        }

        public bool StartJob(
            string jobId,
            string coatingId,
            string substrateTag,
            string operatorId,
            int currentDay,
            bool applyBondCoat,
            Func<IReadOnlyList<InventoryDemand>, bool> consumeInventoryCallback,
            out string failureReason)
        {
            if (!CanStartJob(coatingId, substrateTag, out failureReason))
            {
                return false;
            }

            var def = _coatings[coatingId];

            // Atomic inventory transaction: the complete demand list is validated and
            // consumed by the host in one all-or-nothing call — a failed start
            // consumes nothing.
            var demands = new List<InventoryDemand> { new InventoryDemand(def.CeramicTargetItemId, 1) };
            if (applyBondCoat)
            {
                demands.Add(new InventoryDemand(def.BondCoatItemId, 1));
            }
            if (!consumeInventoryCallback(demands))
            {
                failureReason = "insufficient_coating_inputs";
                return false;
            }

            float voltageKv = 20.0f;
            float currentA = 0.5f;
            float powerKw = ComputeBeamPowerKw(voltageKv, currentA);

            _state.ActiveJob = new EbPvdCoatingJob
            {
                JobId = jobId,
                SubstrateTag = substrateTag,
                CoatingId = coatingId,
                OperatorId = operatorId,
                StartedDay = currentDay,
                ProgressHours = 0.0f,
                RequiredHours = def.BaseDurationHours,
                BeamVoltageKv = voltageKv,
                BeamCurrentA = currentA,
                BeamPowerKw = powerKw,
                VacuumMbar = 0.00001f,
                SubstrateRotationRpm = 12.0f,
                RasterFrequencyHz = 500.0f,
                CoatingThicknessUm = 0.0f,
                BondCoatApplied = applyBondCoat,
                Status = ProcessState.Running
            };

            OnJobStarted?.Invoke(_state.ActiveJob);
            OnStateChanged?.Invoke(_state);
            return true;
        }

        public void Tick(float hours, PowerSupplyContext power, AdvancedMachineOperatorContext? op, ISeededRng rng)
        {
            if (_state.ActiveJob == null || _state.ActiveJob.Status != ProcessState.Running)
                return;

            if (power.BrownoutSeverity >= 0.8f || power.AvailablePowerKw < _state.ActiveJob.BeamPowerKw * 0.5f)
            {
                _state.ActiveJob.Status = ProcessState.Paused;
                OnStateChanged?.Invoke(_state);
                return;
            }

            float powerEfficiency = power.PowerStable ? 1.0f : (1.0f - (power.BrownoutSeverity * 0.5f));
            float operatorMod = op?.EffectiveEfficiency ?? 1.0f;
            float machineMod = Math.Clamp(_state.MachineCondition01, 0.2f, 1.0f);

            float effectiveHours = hours * powerEfficiency * operatorMod * machineMod;
            _state.ActiveJob.ProgressHours += effectiveHours;
            _state.OperatingHours += hours;
            _state.FilamentHours += hours;
            _state.VacuumPumpHours += hours;

            // Wear down machine condition gradually
            _state.MachineCondition01 = Math.Clamp(_state.MachineCondition01 - (hours * 0.002f), 0.1f, 1.0f);

            var def = _coatings[_state.ActiveJob.CoatingId];
            float progressRatio = Math.Clamp(_state.ActiveJob.ProgressHours / _state.ActiveJob.RequiredHours, 0f, 1f);
            _state.ActiveJob.CoatingThicknessUm = def.TargetThicknessUm * progressRatio;

            // Machine hazard checks driven by the registered failure profiles
            // (catalog authority). Only profiles past their hour threshold draw RNG.
            bool hazardFired = false;
            foreach (var profile in _failureProfiles)
            {
                if (FailureMetricHours(profile.Code) > profile.ThresholdHours &&
                    rng.NextFloat() < profile.BaseRisk * hours)
                {
                    _state.MaintenanceFlags.Add(FailureMaintenanceFlag(profile.Code));
                    _state.ActiveJob.Status = ProcessState.Failed;
                    _state.ActiveJob.FailureCode = profile.Code;
                    OnMachineHazard?.Invoke(profile.Code);
                    hazardFired = true;
                    break;
                }
            }
            if (!hazardFired && _state.ActiveJob.ProgressHours >= _state.ActiveJob.RequiredHours)
            {
                CompleteJob(rng);
            }

            OnStateChanged?.Invoke(_state);
        }

        private float FailureMetricHours(string failureCode)
        {
            switch (failureCode)
            {
                case "filament_blowout": return _state.FilamentHours;
                case "vacuum_loss": return _state.VacuumPumpHours;
                default: return _state.OperatingHours;
            }
        }

        private static string FailureMaintenanceFlag(string failureCode)
        {
            switch (failureCode)
            {
                case "filament_blowout": return "filament_blown";
                case "vacuum_loss": return "vacuum_seal_breached";
                default: return failureCode;
            }
        }

        public bool ResumeJob()
        {
            if (_state.ActiveJob != null && _state.ActiveJob.Status == ProcessState.Paused && IsOperational)
            {
                _state.ActiveJob.Status = ProcessState.Running;
                OnStateChanged?.Invoke(_state);
                return true;
            }
            return false;
        }

        public void CompleteJob(ISeededRng rng)
        {
            if (_state.ActiveJob == null || _state.ActiveJob.Status != ProcessState.Running)
                return;

            var job = _state.ActiveJob;
            var def = _coatings[job.CoatingId];

            float uniformity = Math.Clamp(0.95f * _state.MachineCondition01, 0.4f, 1.0f);
            float baseRisk = def.SpallationBaseRisk;
            if (!job.BondCoatApplied) baseRisk += 0.25f;
            if (uniformity < 0.8f) baseRisk += 0.15f;
            float finalSpallationRisk = Math.Clamp(baseRisk, 0.02f, 0.95f);

            bool spalled = rng.NextFloat() < finalSpallationRisk;
            if (spalled)
            {
                job.Status = ProcessState.Failed;
                job.FailureCode = "coating_spallation";
                OnMachineHazard?.Invoke("coating_spallation");
            }
            else
            {
                job.Status = ProcessState.Completed;
                var record = new EbPvdCoatedRecord
                {
                    InstanceId = $"coated_{job.SubstrateTag}_{_state.CompletedRecords.Count + 1}",
                    SubstrateTag = job.SubstrateTag,
                    CoatingId = job.CoatingId,
                    ThicknessUm = job.CoatingThicknessUm,
                    Uniformity01 = uniformity,
                    SpallationRisk01 = finalSpallationRisk,
                    CompletedDay = job.StartedDay,
                    ThermalResistanceBonus = def.ThermalResistanceBonus,
                    MaxTempBonusC = def.MaxTemperatureBonusC,
                    DurabilityBonus = def.DurabilityBonus
                };
                _state.CompletedRecords.Add(record);
                OnJobCompleted?.Invoke(record);
            }

            OnStateChanged?.Invoke(_state);
        }

        public void PerformMaintenance(string maintenanceType)
        {
            switch (maintenanceType)
            {
                case "replace_filament":
                    _state.FilamentHours = 0.0f;
                    _state.MaintenanceFlags.Remove("filament_blown");
                    _state.MachineCondition01 = Math.Clamp(_state.MachineCondition01 + 0.2f, 0.1f, 1.0f);
                    break;
                case "service_vacuum_pump":
                    _state.VacuumPumpHours = 0.0f;
                    _state.MaintenanceFlags.Remove("vacuum_seal_breached");
                    _state.MachineCondition01 = Math.Clamp(_state.MachineCondition01 + 0.2f, 0.1f, 1.0f);
                    break;
                case "realign_chamber_shields":
                    _state.ChamberShieldingCondition01 = 1.0f;
                    _state.MachineCondition01 = Math.Clamp(_state.MachineCondition01 + 0.15f, 0.1f, 1.0f);
                    break;
            }
            OnStateChanged?.Invoke(_state);
        }

        public EbPvdCoatingState CaptureState() => _state.Clone();

        public void RestoreState(EbPvdCoatingState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.MachineCondition01 = saved.MachineCondition01;
            _state.OperatingHours = saved.OperatingHours;
            _state.FilamentHours = saved.FilamentHours;
            _state.VacuumPumpHours = saved.VacuumPumpHours;
            _state.ChamberShieldingCondition01 = saved.ChamberShieldingCondition01;
            _state.ActiveJob = saved.ActiveJob?.Clone();
            _state.CompletedRecords = new List<EbPvdCoatedRecord>();
            foreach (var r in saved.CompletedRecords)
            {
                _state.CompletedRecords.Add(r.Clone());
            }
            _state.MaintenanceFlags = new List<string>(saved.MaintenanceFlags);
            OnStateChanged?.Invoke(_state);
        }
    }
}
