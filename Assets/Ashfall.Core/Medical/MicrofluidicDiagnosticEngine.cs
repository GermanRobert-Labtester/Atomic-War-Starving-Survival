// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.AdvancedMachinery;

namespace Ashfall.Core.Medical
{
    public enum DiagnosticResultKind
    {
        Pending,
        Positive,
        Negative,
        Indeterminate,
        Invalid
    }

    /// <summary>
    /// Clinical evidence outcome from a microfluidic assay run.
    /// </summary>
    [Serializable]
    public sealed class MicrofluidicDiagnosticResult
    {
        public string RunId { get; set; } = string.Empty;
        public string PatientId { get; set; } = string.Empty;
        public string AssayId { get; set; } = string.Empty;
        public string TargetDiseaseId { get; set; } = string.Empty;
        public DiagnosticResultKind ResultKind { get; set; } = DiagnosticResultKind.Pending;
        public float Confidence01 { get; set; } = 0.0f;
        public int CompletedDay { get; set; } = 1;
        public string Detail { get; set; } = string.Empty;

        public MicrofluidicDiagnosticResult Clone() => new MicrofluidicDiagnosticResult
        {
            RunId = RunId,
            PatientId = PatientId,
            AssayId = AssayId,
            TargetDiseaseId = TargetDiseaseId,
            ResultKind = ResultKind,
            Confidence01 = Confidence01,
            CompletedDay = CompletedDay,
            Detail = Detail
        };
    }

    /// <summary>
    /// Active diagnostic assay run in the fluorescence micro-reader.
    /// </summary>
    [Serializable]
    public sealed class MicrofluidicDiagnosticRun
    {
        public string RunId { get; set; } = string.Empty;
        public string PatientId { get; set; } = string.Empty;
        public string AssayId { get; set; } = string.Empty;
        public string OperatorId { get; set; } = string.Empty;
        public int StartedDay { get; set; } = 1;
        public float StartedMinute { get; set; } = 0f;
        public float ProgressMinutes { get; set; } = 0f;
        public float RequiredMinutes { get; set; } = 30f;
        public float SampleQuality01 { get; set; } = 0.95f;
        public float CapillaryFill01 { get; set; } = 1.0f;
        public bool ReagentReconstituted { get; set; } = true;
        public float ReaderIntensity01 { get; set; } = 1.0f;
        public ProcessState Status { get; set; } = ProcessState.Running;
        public DiagnosticResultKind ResultKind { get; set; } = DiagnosticResultKind.Pending;
        public float Confidence01 { get; set; } = 0f;
        public string InvalidReason { get; set; } = string.Empty;

        public MicrofluidicDiagnosticRun Clone() => new MicrofluidicDiagnosticRun
        {
            RunId = RunId,
            PatientId = PatientId,
            AssayId = AssayId,
            OperatorId = OperatorId,
            StartedDay = StartedDay,
            StartedMinute = StartedMinute,
            ProgressMinutes = ProgressMinutes,
            RequiredMinutes = RequiredMinutes,
            SampleQuality01 = SampleQuality01,
            CapillaryFill01 = CapillaryFill01,
            ReagentReconstituted = ReagentReconstituted,
            ReaderIntensity01 = ReaderIntensity01,
            Status = Status,
            ResultKind = ResultKind,
            Confidence01 = Confidence01,
            InvalidReason = InvalidReason
        };
    }

    /// <summary>
    /// Active cartridge soft-lithography molding job.
    /// </summary>
    [Serializable]
    public sealed class MicrofluidicManufacturingJob
    {
        public string JobId { get; set; } = string.Empty;
        public string AssayId { get; set; } = string.Empty;
        public string OperatorId { get; set; } = string.Empty;
        public float ProgressMinutes { get; set; } = 0f;
        public float RequiredMinutes { get; set; } = 60f;
        public ProcessState Status { get; set; } = ProcessState.Running;

        public MicrofluidicManufacturingJob Clone() => new MicrofluidicManufacturingJob
        {
            JobId = JobId,
            AssayId = AssayId,
            OperatorId = OperatorId,
            ProgressMinutes = ProgressMinutes,
            RequiredMinutes = RequiredMinutes,
            Status = Status
        };
    }

    /// <summary>
    /// Persistent state DTO for the Microfluidic Diagnostics System.
    /// </summary>
    [Serializable]
    public sealed class MicrofluidicDiagnosticState
    {
        public int SchemaVersion { get; set; } = 1;
        public float MachineCondition01 { get; set; } = 1.0f;
        public float MasterMoldCondition01 { get; set; } = 1.0f;
        public int CartridgesManufactured { get; set; } = 0;
        public MicrofluidicManufacturingJob? ActiveManufacturingJob { get; set; }
        public List<MicrofluidicDiagnosticRun> ActiveRuns { get; set; } = new List<MicrofluidicDiagnosticRun>();
        public List<MicrofluidicDiagnosticResult> CompletedResults { get; set; } = new List<MicrofluidicDiagnosticResult>();
        public List<string> MaintenanceFlags { get; set; } = new List<string>();

        public MicrofluidicDiagnosticState Clone()
        {
            var copy = new MicrofluidicDiagnosticState
            {
                SchemaVersion = SchemaVersion,
                MachineCondition01 = MachineCondition01,
                MasterMoldCondition01 = MasterMoldCondition01,
                CartridgesManufactured = CartridgesManufactured,
                ActiveManufacturingJob = ActiveManufacturingJob?.Clone(),
                MaintenanceFlags = new List<string>(MaintenanceFlags)
            };
            foreach (var run in ActiveRuns) copy.ActiveRuns.Add(run.Clone());
            foreach (var res in CompletedResults) copy.CompletedResults.Add(res.Clone());
            return copy;
        }
    }

    /// <summary>
    /// Catalog definition of a microfluidic diagnostic assay.
    /// </summary>
    public sealed class MicrofluidicAssayDef
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public List<string> TargetDiseaseIds { get; set; } = new List<string>();
        public string CartridgeItemId { get; set; } = "item_microfluidic_cartridge_general";
        public string ReagentItemId { get; set; } = "item_assay_reagent_pack";
        public string ReaderItemId { get; set; } = "item_microfluidic_reader";
        public float BaseDurationMinutes { get; set; } = 30.0f;
        public float Sensitivity { get; set; } = 0.94f;
        public float Specificity { get; set; } = 0.98f;
        public float EarlyDetectionModifier { get; set; } = 0.40f;
        public float InvalidRunBaseChance { get; set; } = 0.04f;
        public float MinimumOperatorSkill { get; set; } = 0.35f;
    }

    /// <summary>
    /// Core simulation engine for Plan 148 — Microfluidic Diagnostics.
    /// Pure C# model with zero engine dependencies.
    /// </summary>
    public sealed class MicrofluidicDiagnosticEngine : IAdvancedMachineOperation
    {
        private readonly MicrofluidicDiagnosticState _state;
        private readonly Dictionary<string, MicrofluidicAssayDef> _assays =
            new Dictionary<string, MicrofluidicAssayDef>(StringComparer.Ordinal);

        public event Action<MicrofluidicDiagnosticState>? OnStateChanged;
        public event Action<MicrofluidicDiagnosticRun>? OnRunStarted;
        public event Action<MicrofluidicDiagnosticResult>? OnRunCompleted;
        public event Action<string>? OnDiagnosticHazard;

        /// <summary>Fires when a cartridge manufacturing job completes; carries the
        /// assay's cartridge item id so the host can mint the physical inventory item.</summary>
        public event Action<string, string>? OnCartridgeManufactured;

        /// <summary>Machine electrical draw (catalog machine.nominal_power_kw). Host
        /// loader overrides; drives the unpowered pause gate.</summary>
        public float NominalPowerKw { get; set; } = 2.5f;

        public bool IsOperational =>
            _state.MachineCondition01 > 0.2f &&
            !_state.MaintenanceFlags.Contains("mold_corrupted") &&
            !_state.MaintenanceFlags.Contains("laser_misaligned");

        public float Condition01 => _state.MachineCondition01;
        public string ActiveJobId => _state.ActiveManufacturingJob?.JobId ?? (_state.ActiveRuns.Count > 0 ? _state.ActiveRuns[0].RunId : string.Empty);
        public ProcessState State => _state.ActiveManufacturingJob?.Status ?? (_state.ActiveRuns.Count > 0 ? ProcessState.Running : ProcessState.Idle);

        public MicrofluidicDiagnosticState StateDto => _state;
        public IEnumerable<MicrofluidicAssayDef> Assays => _assays.Values;

        public MicrofluidicDiagnosticEngine(MicrofluidicDiagnosticState? initialState = null)
        {
            _state = initialState?.Clone() ?? new MicrofluidicDiagnosticState();
            SeedDefaultAssays();
        }

        private void SeedDefaultAssays()
        {
            RegisterAssay(new MicrofluidicAssayDef
            {
                Id = "microfluidic_assay_cholera",
                DisplayName = "Vibrio Cholerae Rapid Immunochip",
                TargetDiseaseIds = new List<string> { "disease_cholera" },
                BaseDurationMinutes = 30.0f,
                Sensitivity = 0.94f,
                Specificity = 0.98f,
                EarlyDetectionModifier = 0.40f
            });
            RegisterAssay(new MicrofluidicAssayDef
            {
                Id = "microfluidic_assay_zoonotic_flu",
                DisplayName = "Avian-Swine Zoonotic Multiplex Chip",
                TargetDiseaseIds = new List<string> { "disease_zoonotic_flu" },
                BaseDurationMinutes = 45.0f,
                Sensitivity = 0.92f,
                Specificity = 0.96f,
                EarlyDetectionModifier = 0.50f
            });
            RegisterAssay(new MicrofluidicAssayDef
            {
                Id = "microfluidic_assay_blood_fever",
                DisplayName = "Viral Hemorrhagic Blood Fever Panel",
                TargetDiseaseIds = new List<string> { "disease_blood_fever" },
                BaseDurationMinutes = 60.0f,
                Sensitivity = 0.95f,
                Specificity = 0.97f,
                EarlyDetectionModifier = 0.35f
            });
            RegisterAssay(new MicrofluidicAssayDef
            {
                Id = "microfluidic_assay_spore_blight",
                DisplayName = "Ashfall Spore Blight Diagnostic Assay",
                TargetDiseaseIds = new List<string> { "disease_spore_blight" },
                BaseDurationMinutes = 40.0f,
                Sensitivity = 0.90f,
                Specificity = 0.95f,
                EarlyDetectionModifier = 0.45f
            });
            RegisterAssay(new MicrofluidicAssayDef
            {
                Id = "microfluidic_assay_fungal_respiratory",
                DisplayName = "Deep-Bunker Aspergillus Respiratory Array",
                TargetDiseaseIds = new List<string> { "disease_fungal_respiratory" },
                BaseDurationMinutes = 50.0f,
                Sensitivity = 0.91f,
                Specificity = 0.96f,
                EarlyDetectionModifier = 0.40f
            });
            RegisterAssay(new MicrofluidicAssayDef
            {
                Id = "microfluidic_assay_typhoid",
                DisplayName = "Salmonella Typhi Serological Card",
                TargetDiseaseIds = new List<string> { "disease_typhoid_waterborne" },
                BaseDurationMinutes = 35.0f,
                Sensitivity = 0.93f,
                Specificity = 0.97f,
                EarlyDetectionModifier = 0.35f
            });
            RegisterAssay(new MicrofluidicAssayDef
            {
                Id = "microfluidic_assay_septic_fever",
                DisplayName = "Septic Rust-Wound Tetanic Micro-Screen",
                TargetDiseaseIds = new List<string> { "disease_septic_rust_wound_fever" },
                BaseDurationMinutes = 25.0f,
                Sensitivity = 0.96f,
                Specificity = 0.94f,
                EarlyDetectionModifier = 0.30f
            });
            RegisterAssay(new MicrofluidicAssayDef
            {
                Id = "microfluidic_assay_mold_lung",
                DisplayName = "Excavation Stachybotrys Mycotoxin Assay",
                TargetDiseaseIds = new List<string> { "disease_deep_excavation_mold_lung" },
                BaseDurationMinutes = 55.0f,
                Sensitivity = 0.89f,
                Specificity = 0.97f,
                EarlyDetectionModifier = 0.45f
            });
        }

        public void RegisterAssay(MicrofluidicAssayDef def)
        {
            if (def != null && !string.IsNullOrEmpty(def.Id))
            {
                // Probabilistic contract: sensitivity/specificity (and the other
                // rates) are probabilities; clamp out-of-range authored values once
                // at registration so resolution math never sees them raw.
                def.Sensitivity = Math.Clamp(def.Sensitivity, 0f, 1f);
                def.Specificity = Math.Clamp(def.Specificity, 0f, 1f);
                def.EarlyDetectionModifier = Math.Clamp(def.EarlyDetectionModifier, 0f, 1f);
                def.InvalidRunBaseChance = Math.Clamp(def.InvalidRunBaseChance, 0f, 1f);
                def.MinimumOperatorSkill = Math.Clamp(def.MinimumOperatorSkill, 0f, 1f);
                _assays[def.Id] = def;
            }
        }

        public MicrofluidicAssayDef? GetAssay(string assayId)
        {
            _assays.TryGetValue(assayId, out var def);
            return def;
        }

        public bool StartCartridgeManufacturing(
            string jobId,
            string assayId,
            string operatorId,
            Func<IReadOnlyList<InventoryDemand>, bool> consumeInventoryCallback,
            out string failureReason)
        {
            if (!IsOperational)
            {
                failureReason = "machine_not_operational";
                return false;
            }
            if (_state.ActiveManufacturingJob != null && _state.ActiveManufacturingJob.Status == ProcessState.Running)
            {
                failureReason = "manufacturing_already_active";
                return false;
            }
            if (!_assays.TryGetValue(assayId, out var def))
            {
                failureReason = "invalid_assay_id";
                return false;
            }

            // Atomic consumption: the host validates and consumes the complete
            // demand list in one all-or-nothing call.
            var demands = new List<InventoryDemand>
            {
                new InventoryDemand("item_pdms_silicone_kit", 1),
                new InventoryDemand(def.ReagentItemId, 1)
            };
            if (!consumeInventoryCallback(demands))
            {
                failureReason = "insufficient_manufacturing_inputs";
                return false;
            }

            _state.ActiveManufacturingJob = new MicrofluidicManufacturingJob
            {
                JobId = jobId,
                AssayId = assayId,
                OperatorId = operatorId,
                ProgressMinutes = 0f,
                RequiredMinutes = 60f,
                Status = ProcessState.Running
            };

            OnStateChanged?.Invoke(_state);
            failureReason = "ok";
            return true;
        }

        public bool StartDiagnosticRun(
            string runId,
            string patientId,
            string assayId,
            string operatorId,
            int currentDay,
            float currentMinute,
            Func<string, int, bool> consumeInventoryCallback,
            out string failureReason)
        {
            if (!IsOperational)
            {
                failureReason = "reader_not_operational";
                return false;
            }
            if (!_assays.TryGetValue(assayId, out var def))
            {
                failureReason = "invalid_assay_id";
                return false;
            }
            if (_state.ActiveRuns.Count >= 4) // max 4 channels
            {
                failureReason = "all_reader_channels_busy";
                return false;
            }

            // Consume 1 cartridge
            if (!consumeInventoryCallback(def.CartridgeItemId, 1))
            {
                failureReason = "missing_diagnostic_cartridge";
                return false;
            }

            var run = new MicrofluidicDiagnosticRun
            {
                RunId = runId,
                PatientId = patientId,
                AssayId = assayId,
                OperatorId = operatorId,
                StartedDay = currentDay,
                StartedMinute = currentMinute,
                ProgressMinutes = 0f,
                RequiredMinutes = def.BaseDurationMinutes,
                SampleQuality01 = 0.95f,
                CapillaryFill01 = 1.0f,
                ReagentReconstituted = true,
                ReaderIntensity01 = 1.0f,
                Status = ProcessState.Running
            };

            _state.ActiveRuns.Add(run);
            OnRunStarted?.Invoke(run);
            OnStateChanged?.Invoke(_state);
            failureReason = "ok";
            return true;
        }

        public void Tick(
            float deltaMinutes,
            PowerSupplyContext power,
            AdvancedMachineOperatorContext? op,
            ISeededRng rng,
            Func<string, string, bool> patientHasDisease)
        {
            // Power gate (mirrors EB-PVD): no grid authority means assume supply;
            // otherwise severe brownout or <50% nominal supply pauses all processes.
            bool gridPresent = power.AvailablePowerKw > 0f || power.BrownoutSeverity > 0f;
            bool unpowered = gridPresent &&
                (power.BrownoutSeverity >= 0.8f || power.AvailablePowerKw < NominalPowerKw * 0.5f);
            if (unpowered)
            {
                bool changed = false;
                if (_state.ActiveManufacturingJob != null && _state.ActiveManufacturingJob.Status == ProcessState.Running)
                {
                    _state.ActiveManufacturingJob.Status = ProcessState.Paused;
                    changed = true;
                }
                foreach (var run in _state.ActiveRuns)
                {
                    if (run.Status == ProcessState.Running)
                    {
                        run.Status = ProcessState.Paused;
                        changed = true;
                    }
                }
                if (changed) OnStateChanged?.Invoke(_state);
                return;
            }

            float powerFactor = power.PowerStable ? 1.0f : (1.0f - (power.BrownoutSeverity * 0.7f));
            float opFactor = op?.EffectiveEfficiency ?? 1.0f;
            float effectiveMins = deltaMinutes * powerFactor * opFactor;

            // Tick manufacturing (a brownout pause auto-resumes once power returns)
            if (_state.ActiveManufacturingJob != null && _state.ActiveManufacturingJob.Status == ProcessState.Paused)
            {
                _state.ActiveManufacturingJob.Status = ProcessState.Running;
            }
            if (_state.ActiveManufacturingJob != null && _state.ActiveManufacturingJob.Status == ProcessState.Running)
            {
                _state.ActiveManufacturingJob.ProgressMinutes += effectiveMins;
                if (_state.ActiveManufacturingJob.ProgressMinutes >= _state.ActiveManufacturingJob.RequiredMinutes)
                {
                    _state.ActiveManufacturingJob.Status = ProcessState.Completed;
                    _state.CartridgesManufactured++;
                    _state.MasterMoldCondition01 = Math.Clamp(_state.MasterMoldCondition01 - 0.02f, 0.1f, 1.0f);
                    OnCartridgeManufactured?.Invoke(_state.ActiveManufacturingJob.AssayId, _assays.TryGetValue(_state.ActiveManufacturingJob.AssayId, out var mfgDef) ? mfgDef.CartridgeItemId : "item_microfluidic_cartridge_general");
                }
            }

            // Tick active diagnostic runs (paused runs resume with power)
            for (int i = _state.ActiveRuns.Count - 1; i >= 0; i--)
            {
                var run = _state.ActiveRuns[i];
                if (run.Status == ProcessState.Paused)
                {
                    run.Status = ProcessState.Running;
                }
                if (run.Status != ProcessState.Running) continue;

                run.ProgressMinutes += effectiveMins;
                if (run.ProgressMinutes >= run.RequiredMinutes)
                {
                    ResolveDiagnosticRun(run, rng, patientHasDisease);
                    if (run.Status == ProcessState.Completed || run.Status == ProcessState.Failed)
                    {
                        _state.ActiveRuns.RemoveAt(i);
                    }
                }
            }

            OnStateChanged?.Invoke(_state);
        }

        private void ResolveDiagnosticRun(
            MicrofluidicDiagnosticRun run,
            ISeededRng rng,
            Func<string, string, bool> patientHasDisease)
        {
            var def = _assays[run.AssayId];
            string targetDisease = def.TargetDiseaseIds.Count > 0 ? def.TargetDiseaseIds[0] : "unknown";

            // Check invalid run chance
            float invalidChance = def.InvalidRunBaseChance * (2.0f - run.SampleQuality01);
            if (rng.NextFloat() < invalidChance)
            {
                run.Status = ProcessState.Failed;
                run.ResultKind = DiagnosticResultKind.Invalid;
                run.InvalidReason = "bubble_clog_or_capillary_stall";
                run.Confidence01 = 0.0f;
                OnDiagnosticHazard?.Invoke("assay_run_invalid");
                return;
            }

            // Without a clinical truth source the assay cannot resolve honestly;
            // yield an indeterminate (no evidence recorded) rather than guessing.
            if (patientHasDisease == null)
            {
                run.Status = ProcessState.Failed;
                run.ResultKind = DiagnosticResultKind.Indeterminate;
                run.InvalidReason = "no_clinical_context";
                run.Confidence01 = 0.0f;
                OnDiagnosticHazard?.Invoke("assay_run_indeterminate");
                return;
            }

            bool actuallyInfected = patientHasDisease(run.PatientId, targetDisease);
            float roll = rng.NextFloat();
            DiagnosticResultKind outcome;

            if (actuallyInfected)
            {
                // True positive rate = sensitivity
                outcome = roll < def.Sensitivity
                    ? DiagnosticResultKind.Positive
                    : DiagnosticResultKind.Negative;
            }
            else
            {
                // True negative rate = specificity
                outcome = roll < def.Specificity
                    ? DiagnosticResultKind.Negative
                    : DiagnosticResultKind.Positive;
            }

            // Confidence is drawn from ONE distribution shared by every branch and
            // parameterized only by sample quality — branch-specific bands would let
            // a reader deduce the hidden disease truth from the confidence value.
            float confidence = Math.Clamp(
                (0.60f + (0.35f * rng.NextFloat())) * Math.Clamp(0.85f + (0.15f * run.SampleQuality01), 0f, 1f),
                0.5f, 0.99f);

            run.Status = ProcessState.Completed;
            run.ResultKind = outcome;
            run.Confidence01 = confidence;

            var result = new MicrofluidicDiagnosticResult
            {
                RunId = run.RunId,
                PatientId = run.PatientId,
                AssayId = run.AssayId,
                TargetDiseaseId = targetDisease,
                ResultKind = outcome,
                Confidence01 = confidence,
                CompletedDay = run.StartedDay,
                Detail = $"Rapid micro-assay resolved {outcome} (Confidence: {confidence:P0}) for patient {run.PatientId}."
            };

            _state.CompletedResults.Add(result);
            OnRunCompleted?.Invoke(result);
        }

        public void PerformMaintenance(string maintenanceType)
        {
            switch (maintenanceType)
            {
                case "recalibrate_optics":
                    _state.MaintenanceFlags.Remove("laser_misaligned");
                    _state.MachineCondition01 = Math.Clamp(_state.MachineCondition01 + 0.2f, 0.1f, 1.0f);
                    break;
                case "recast_master_mold":
                    _state.MasterMoldCondition01 = 1.0f;
                    _state.MaintenanceFlags.Remove("mold_corrupted");
                    break;
            }
            OnStateChanged?.Invoke(_state);
        }

        public MicrofluidicDiagnosticState CaptureState() => _state.Clone();

        public void RestoreState(MicrofluidicDiagnosticState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.MachineCondition01 = saved.MachineCondition01;
            _state.MasterMoldCondition01 = saved.MasterMoldCondition01;
            _state.CartridgesManufactured = saved.CartridgesManufactured;
            _state.ActiveManufacturingJob = saved.ActiveManufacturingJob?.Clone();
            _state.ActiveRuns.Clear();
            foreach (var r in saved.ActiveRuns) _state.ActiveRuns.Add(r.Clone());
            _state.CompletedResults.Clear();
            foreach (var res in saved.CompletedResults) _state.CompletedResults.Add(res.Clone());
            _state.MaintenanceFlags = new List<string>(saved.MaintenanceFlags);
            OnStateChanged?.Invoke(_state);
        }
    }
}
