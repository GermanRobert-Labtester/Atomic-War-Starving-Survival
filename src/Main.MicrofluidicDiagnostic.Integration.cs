// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private MicrofluidicDiagnosticHostSession? _microfluidicDiagnostic;
        private ISeededRng? _microfluidicRng;
        private MicrofluidicDiagnosticPanel? _microfluidicDiagnosticPanel;

        private void SetupMicrofluidicDiagnostic()
        {
            if (_microfluidicDiagnostic != null) return;
            SetupCampaignDay();
            var state = MicrofluidicDiagnosticSaveStore.TryLoad() ?? new MicrofluidicDiagnosticState();
            var engine = new MicrofluidicDiagnosticEngine(state);
            LoadMicrofluidicCatalogInto(engine);
            _microfluidicDiagnostic = new MicrofluidicDiagnosticHostSession(engine);
            _microfluidicRng = _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.MicrofluidicDiagnostics);
            // A finished manufacturing run mints the assay's cartridge item into
            // inventory; diagnostic runs consume one cartridge on start (host call).
            _microfluidicDiagnostic.System.OnCartridgeManufactured += (_, cartridgeItemId) =>
            {
                if (_inventory != null)
                {
                    _inventory.Inventory.AddById(cartridgeItemId, 1);
                }
            };
            // Assay results write into the medical diagnosis ledger (disease ids
            // are affliction definition ids). Presentation stays in LastEvent.
            _microfluidicDiagnostic.System.OnRunCompleted += ApplyMicrofluidicResultToDiagnosis;
        }

        private void ApplyMicrofluidicResultToDiagnosis(MicrofluidicDiagnosticResult res)
        {
            if (res == null || string.IsNullOrEmpty(res.PatientId) || string.IsNullOrEmpty(res.TargetDiseaseId))
                return;
            if (res.ResultKind == DiagnosticResultKind.Invalid
                || res.ResultKind == DiagnosticResultKind.Indeterminate
                || res.ResultKind == DiagnosticResultKind.Pending)
                return;

            EnsureMedicalPipeline();
            var pipeline = _medical?.Pipeline;
            if (pipeline == null) return;
            if (!Ashfall.Core.Survivors.SurvivorId.TryParse(res.PatientId, out var survivor))
                return;
            if (!Ashfall.Core.Medical.AfflictionId.IsValid(res.TargetDiseaseId, out _))
                return;

            var definition = new Ashfall.Core.Medical.AfflictionId(res.TargetDiseaseId);
            var episode = Ashfall.Core.Medical.AfflictionEpisodeId.Create(survivor, definition);
            int day = _simDay > 0 ? _simDay : 1;
            string detail = $"microfluidic:{res.AssayId}:{res.ResultKind}:{res.Confidence01:F2}";

            if (res.ResultKind == DiagnosticResultKind.Positive)
            {
                if (res.Confidence01 >= 0.85f)
                    pipeline.Diagnosis.Confirm(episode, day, detail);
                else
                    pipeline.SuspectFromEvidence(survivor, definition, day, detail);
            }
            else if (res.ResultKind == DiagnosticResultKind.Negative)
            {
                pipeline.Diagnosis.RuleOut(episode, day, detail);
            }
        }

        private void LoadMicrofluidicCatalogInto(MicrofluidicDiagnosticEngine engine)
        {
            try
            {
                string dataDir = ResolvePlans146DataDir();
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
                var catalog = MicrofluidicDiagnosticCatalogLoader.Load(dataDir, fileIO, new SystemTextJsonSerializer());
                if (catalog.assays.Count > 0)
                {
                    foreach (var def in catalog.ToAssayDefs())
                    {
                        engine.RegisterAssay(def);
                    }
                }
                if (catalog.machine.nominal_power_kw > 0f)
                {
                    engine.NominalPowerKw = catalog.machine.nominal_power_kw;
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Plans148] microfluidic_diagnostic_catalog.json load failed; engine seeds in force: {ex.Message}");
            }
        }

        /// <summary>
        /// Plans 146–149 MED: push living roster ids into the assay panel so
        /// START ASSAY RUN emits assayId|patientId (defaults to first living).
        /// </summary>
        private void SyncMicrofluidicPatientCandidates()
        {
            if (_microfluidicDiagnosticPanel == null) return;
            var patients = new System.Collections.Generic.List<string>();
            if (_survivors != null)
            {
                foreach (var s in _survivors.RosterState)
                {
                    if (s != null && s.IsAliveState && !string.IsNullOrEmpty(s.Id))
                        patients.Add(s.Id);
                }
            }
            if (patients.Count == 0)
                patients.Add(ResolvePlans146OperatorId());
            _microfluidicDiagnosticPanel.SetPatientCandidates(patients);
        }

        private void SaveMicrofluidicDiagnostic()
        {
            if (_microfluidicDiagnostic != null)
                CaptureSection("microfluidic_diagnostic", MicrofluidicDiagnosticSaveStore.TryCapturePersisted(_microfluidicDiagnostic.System.CaptureState()));
        }

        private void HandleMicrofluidicDiagnosticAction(string action, string param = "")
        {
            SetupMicrofluidicDiagnostic();
            EnsurePlans146To149Panels();
            if (_microfluidicDiagnostic == null) return;

            if (string.Equals(action, "CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                if (_microfluidicDiagnosticPanel != null) _microfluidicDiagnosticPanel.Visible = false;
                return;
            }

            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_microfluidicDiagnosticPanel != null)
                {
                    SyncMicrofluidicPatientCandidates();
                    ShowPanelLifecycle(_microfluidicDiagnosticPanel);
                    _microfluidicDiagnosticPanel.RefreshView();
                }
                return;
            }

            if (string.Equals(action, "start_manufacture", StringComparison.OrdinalIgnoreCase))
            {
                string assayId = string.IsNullOrEmpty(param) ? "microfluidic_assay_cholera" : param;
                SetupInventory();
                string jobId = $"mfg_{_simDay}_{assayId}";
                bool ok = _microfluidicDiagnostic.StartManufacturing(
                    jobId, assayId, ResolvePlans146OperatorId(), TryConsumePlans146Demands, out string reason);
                _microfluidicDiagnosticPanel?.ShowFeedback(
                    ok ? $"Cartridge casting started for {assayId}."
                       : $"Cannot manufacture cartridge: {reason}",
                    !ok);
            }
            else if (string.Equals(action, "select_patient", StringComparison.OrdinalIgnoreCase))
            {
                SyncMicrofluidicPatientCandidates();
                if (_microfluidicDiagnosticPanel != null && !string.IsNullOrEmpty(param))
                {
                    _microfluidicDiagnosticPanel.SelectPatient(param);
                    _microfluidicDiagnosticPanel.ShowFeedback($"Patient selected: {param}.", false);
                }
            }
            else if (string.Equals(action, "start_run", StringComparison.OrdinalIgnoreCase))
            {
                // param: assayId|patientId  (patient defaults to first living survivor)
                SyncMicrofluidicPatientCandidates();
                string assayId = "microfluidic_assay_cholera";
                string patientId = ResolvePlans146OperatorId();
                if (!string.IsNullOrEmpty(param))
                {
                    var parts = param.Split('|');
                    if (parts.Length >= 1 && !string.IsNullOrEmpty(parts[0])) assayId = parts[0];
                    if (parts.Length >= 2 && !string.IsNullOrEmpty(parts[1])) patientId = parts[1];
                }
                SetupInventory();
                string runId = $"run_{_simDay}_{assayId}_{patientId}";
                bool ok = _microfluidicDiagnostic.StartRun(
                    runId, patientId, assayId, ResolvePlans146OperatorId(),
                    _simDay > 0 ? _simDay : 1, 0f,
                    (itemId, qty) => _inventory != null && _inventory.Inventory.TryConsumeById(itemId, qty),
                    out string reason);
                _microfluidicDiagnosticPanel?.ShowFeedback(
                    ok ? $"Assay run started: {assayId} for {patientId}."
                       : $"Cannot start assay: {reason}",
                    !ok);
            }
            else if (string.Equals(action, "maintain", StringComparison.OrdinalIgnoreCase))
            {
                string maint = string.IsNullOrEmpty(param) ? "recalibrate_optics" : param;
                _microfluidicDiagnostic.PerformMaintenance(maint);
                _microfluidicDiagnosticPanel?.ShowFeedback($"Analyzer serviced: {maint}.", false);
            }

            _microfluidicDiagnosticPanel?.RefreshView();
        }

    }
}
