// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Farming;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private AmputationSystem? _amputation;

        // ── Plan 190: Infection & Amputation Mechanics ───────────────────

        public AmputationSystem EnsureAmputation()
        {
            if (_amputation != null) return _amputation;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("amputation") : new SeededRng(190);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
            var needs = _survivors?.Needs;

            _amputation = new AmputationSystem(rng, inv, needs, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("surgical_procedures.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<SurgicalProcedureCatalog>(json);
                        if (catalog?.procedures != null)
                        {
                            foreach (var p in catalog.procedures)
                                _amputation.RegisterProcedure(p);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Amputation] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = AmputationSaveStore.TryLoad();
            if (saved != null)
            {
                _amputation.RestoreState(saved);
            }

            _amputation.OnAmputationComplete += (survivorId, limb, condition) =>
            {
                _journal?.TryAddRawEntry("amputation_performed", $"Emergency amputation performed on {survivorId}'s {limb} (State: {condition}).", null!, _simDay);
                // T18a — a committed lower-limb loss is a permanent mobility
                // fact: record the chronic limp so the accommodation decision
                // becomes reachable. Upper-limb loss does not produce a limp.
                if (limb == Ashfall.Core.Medical.LimbId.LeftLeg || limb == Ashfall.Core.Medical.LimbId.RightLeg)
                    RecordChronicConditionFact(survivorId, ChronicConditionIds.Limp, "amputation");
            };

            _amputation.OnGangreneDeclared += (survivorId, limb) =>
            {
                _journal?.TryAddRawEntry("gangrene_warning", $"Critical medical emergency: {survivorId}'s {limb} wound has turned gangrenous!", null!, _simDay);
            };

            _amputation.OnProstheticFitted += (survivorId, prostheticId) =>
            {
                _journal?.TryAddRawEntry("prosthetic_fitted", $"Prosthetic '{prostheticId}' fitted to {survivorId}.", null!, _simDay);
            };

            _amputation.OnPhantomPainEpisode += (survivorId, stressAmount) =>
            {
                _journal?.TryAddRawEntry("phantom_pain_episode", $"{survivorId} experienced a severe phantom-pain episode.", null!, _simDay);
            };

            return _amputation;
        }

        private void SetupAmputation()
        {
            EnsureAmputation();
        }

        private void SaveAmputation()
        {
            if (_amputation != null)
            {
                CaptureSection("amputation", AmputationSaveStore.TryCapturePersisted(_amputation.CaptureState()));
            }
        }

        // ── UI-07 closeout: amputation / tribunal / railway / archaeology consoles ──

        private void HandleAmputationAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseAmputationTriagePanel(); return; }
            if (_amputationTriagePanel == null || _amputation == null) return;

            var parts = param.Split(':');
            switch (action)
            {
                case "amputate":
                {
                    if (parts.Length != 2 || !System.Enum.TryParse<LimbId>(parts[1], out var limb)) break;
                    string procedureId = (limb == LimbId.LeftArm || limb == LimbId.RightArm)
                        ? "procedure_amputation_arm_field" : "procedure_amputation_leg_field";
                    var result = _amputation.PerformAmputation(parts[0], limb, procedureId);
                    _amputationTriagePanel.ShowFeedback(
                        result.Success
                            ? (result.SurvivorDied
                                ? "The procedure was done, but the patient did not survive the shock."
                                : "Amputation complete. Recovery will be long; phantom pain is possible.")
                            : "The surgery could not proceed — check tools and supplies.",
                        !result.Success || result.SurvivorDied);
                    break;
                }
                case "treat":
                {
                    if (parts.Length != 2 || !System.Enum.TryParse<LimbId>(parts[1], out var limb2)) break;
                    _amputation.TreatWound(parts[0], limb2, cleaningEfficacy: 0.5f);
                    _amputationTriagePanel.ShowFeedback("Wound cleaned and dressed. Infection risk is reduced.", false);
                    break;
                }
                case "prosthetic":
                {
                    if (parts.Length != 2 || !System.Enum.TryParse<LimbId>(parts[1], out var limb3)) break;
                    string prostheticItem = (limb3 == LimbId.LeftArm || limb3 == LimbId.RightArm)
                        ? "prosthetic_wooden_arm" : "prosthetic_wooden_leg";
                    var res = _amputation.FitProsthetic(parts[0], limb3, prostheticItem);
                    _survivorDowntimePanel?.RefreshView();
                    _amputationTriagePanel.ShowFeedback(
                        res.IsSuccess ? "Prosthetic fitted. Some function returns — never all of it."
                                      : "Fitting failed — the limb or the workshop isn't ready.",
                        !res.IsSuccess);
                    break;
                }
            }
            _amputationTriagePanel.RefreshView();
        }

        private void CloseAmputationTriagePanel() { _amputationTriagePanel?.Visible = false; }

    }
}
