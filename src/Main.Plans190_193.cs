// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 190-193 Host Wire & Orchestration
// Subsystems   : Infection & Amputation, Railways & Armored Trains,
//                Subterranean Fungi Cultivation, Wasteland Justice & Tribal Law
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Archaeology;
using Ashfall.Core.Farming;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private AmputationSystem? _amputation;
        private RailwaySystem? _railway;
        private FungiCultivationSystem? _fungi;
        private JusticeSystem? _justice;

        // ── Plan 190: Infection & Amputation Mechanics ───────────────────

        public AmputationSystem EnsureAmputation()
        {
            if (_amputation != null) return _amputation;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("amputation") : new SeededRng(190);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
            var needs = _survivors?.Needs;

            _amputation = new AmputationSystem(rng, inv, needs, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/surgical_procedures.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
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
                // TODO: Avatar integration — update survivor portrait variant, sprite attachment,
                // animation set, and equipment restrictions for the affected limb.
                RefreshSurvivorVisuals(survivorId);
            };

            _amputation.OnGangreneDeclared += (survivorId, limb) =>
            {
                _journal?.TryAddRawEntry("gangrene_warning", $"Critical medical emergency: {survivorId}'s {limb} wound has turned gangrenous!", null!, _simDay);
            };

            _amputation.OnProstheticFitted += (survivorId, prostheticId) =>
            {
                _journal?.TryAddRawEntry("prosthetic_fitted", $"Prosthetic '{prostheticId}' fitted to {survivorId}.", null!, _simDay);
                RefreshSurvivorVisuals(survivorId);
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
                CaptureSection("amputation", AmputationSaveStore.TryCapturePersisted(_amputation.State));
            }
        }

        // ── Plan 191: Railways & Armored Trains ──────────────────────────

        public RailwaySystem EnsureRailway()
        {
            if (_railway != null) return _railway;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("railway") : new SeededRng(191);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _railway = new RailwaySystem(rng, inv, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/rail_network.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<RailwayNetworkCatalog>(json);
                        if (catalog != null)
                        {
                            _railway.RegisterCatalog(catalog);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Railway] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            string logisticsPath = "res://Assets/StreamingAssets/Data/rail_logistics_catalog.json";
            if (Godot.FileAccess.FileExists(logisticsPath))
            {
                using var file = Godot.FileAccess.Open(logisticsPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    try
                    {
                        var container = System.Text.Json.JsonSerializer.Deserialize<RailLogisticsCatalogContainer>(json);
                        if (container != null && container.edges != null)
                        {
                            _railway.RegisterLogisticsCatalog(container.edges);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Railway] Failed to parse {logisticsPath}: {ex.Message}");
                    }
                }
            }

            var saved = RailwaySaveStore.TryLoad();
            if (saved != null)
            {
                _railway.RestoreState(saved);
            }

            _railway.OnTrainDispatched += (trainId, segmentId) =>
            {
                _journal?.TryAddRawEntry("train_dispatched", $"Armored train {trainId} departed onto rail segment {segmentId}.", null!, _simDay);
            };

            _railway.OnDerailment += (trainId, segmentId) =>
            {
                _journal?.TryAddRawEntry("train_derailment", $"Disaster! Train {trainId} derailed on degraded rail segment {segmentId}!", null!, _simDay);
            };

            _railway.OnTrainAmbushed += (trainId, segmentId) =>
            {
                _journal?.TryAddRawEntry("train_ambush", $"Train {trainId} came under heavy raider fire on segment {segmentId}!", null!, _simDay);
            };

            return _railway;
        }

        private void SetupRailway()
        {
            EnsureRailway();
        }

        private void SaveRailway()
        {
            if (_railway != null)
            {
                CaptureSection("railway", RailwaySaveStore.TryCapturePersisted(_railway.State));
            }
        }

        // ── Plan 192: Subterranean Fungi Cultivation ─────────────────────

        public FungiCultivationSystem EnsureFungi()
        {
            if (_fungi != null) return _fungi;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("fungi") : new SeededRng(192);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _fungi = new FungiCultivationSystem(rng, inv, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/underground_flora.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<UndergroundFloraCatalog>(json);
                        if (catalog != null)
                        {
                            _fungi.RegisterCatalog(catalog);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Fungi] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = FungiSaveStore.TryLoad();
            if (saved != null)
            {
                _fungi.RestoreState(saved);
            }

            _fungi.OnToxicBloom += (plotId, roomId) =>
            {
                _journal?.TryAddRawEntry("fungi_toxic_bloom", $"Toxic mold outbreak detected at plot {plotId} in {roomId}!", null!, _simDay);
            };

            _fungi.OnFungiHarvested += (plotId, strain, count) =>
            {
                _journal?.TryAddRawEntry("fungi_harvest", $"Harvested {count} units of {strain} from subterranean bed {plotId}.", null!, _simDay);
            };

            _fungi.OnSubstratePrepared += (plotId, preparation, usedHeat) =>
            {
                _journal?.TryAddRawEntry("fungi_substrate_prepared", $"Bed {plotId} substrate prepared ({preparation}{(usedHeat ? ", heated" : "")}).", null!, _simDay);
            };

            _fungi.OnSubstrateDisposed += (plotId, method) =>
            {
                _journal?.TryAddRawEntry("fungi_substrate_disposed", $"Contaminated substrate from bed {plotId} disposed ({method}).", null!, _simDay);
            };

            _fungi.OnContaminationSpread += (sourcePlotId, roomId) =>
            {
                _journal?.TryAddRawEntry("fungi_contamination_spread", $"Mold contamination is spreading from bed {sourcePlotId} to neighbouring beds in {roomId}.", null!, _simDay);
            };

            return _fungi;
        }

        private void SetupFungi()
        {
            EnsureFungi();
        }

        private void SaveFungi()
        {
            if (_fungi != null)
            {
                CaptureSection("fungi_cultivation", FungiSaveStore.TryCapturePersisted(_fungi.State));
            }
        }

        // ── Plan 193: Wasteland Justice & Tribal Law ─────────────────────

        public JusticeSystem EnsureJustice()
        {
            if (_justice != null) return _justice;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("justice") : new SeededRng(193);
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();
            var needs = _survivors?.Needs;

            _justice = new JusticeSystem(rng, inv, needs, _politics, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/wasteland_laws.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<WastelandLawsCatalog>(json);
                        if (catalog?.laws != null)
                        {
                            foreach (var l in catalog.laws)
                                _justice.RegisterLaw(l);
                        }
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.Justice] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            var saved = JusticeSaveStore.TryLoad();
            if (saved != null)
            {
                _justice.RestoreState(saved);
            }

            _justice.OnTrialConcluded += (incidentId, verdict, punishment) =>
            {
                _journal?.TryAddRawEntry("trial_concluded", $"Tribal tribunal reached verdict for {incidentId}: {verdict} (Sentence: {punishment}).", null!, _simDay);
            };

            _justice.OnBanishment += (survivorId, incidentId) =>
            {
                _journal?.TryAddRawEntry("survivor_banished", $"{survivorId} was formally banished from the shelter following tribunal proceedings.", null!, _simDay);
            };

            _justice.OnExecution += (survivorId, incidentId) =>
            {
                _journal?.TryAddRawEntry("execution_carried_out", $"Capital punishment carried out on {survivorId}.", null!, _simDay);
            };

            _justice.OnVigilanteOutbreak += (incidentId, accusedId) =>
            {
                _journal?.TryAddRawEntry("vigilante_mob", $"Shelter unrest boiled over! Vigilante mob enacted street justice on {accusedId}.", null!, _simDay);
            };

            return _justice;
        }

        private void SetupJustice()
        {
            EnsureJustice();
        }

        private void SaveJustice()
        {
            if (_justice != null)
            {
                CaptureSection("wasteland_justice", JusticeSaveStore.TryCapturePersisted(_justice.State));
            }
        }

        // ── Daily Tick Orchestration for Plans 190-193 ───────────────────

        private void TickPlans190_193(int currentDay)
        {
            _amputation?.TickDay(currentDay);
            _railway?.TickDay(currentDay);

            // Plan 204: project real per-room thermal state into the fungi beds when the
            // shelter thermal authority is live; otherwise the Core default band applies.
            if (_fungi != null)
            {
                var thermalRooms = _shelterThermal?.System.State.rooms;
                if (thermalRooms != null && thermalRooms.Count > 0)
                {
                    var temps = new Dictionary<string, float>(StringComparer.Ordinal);
                    for (int i = 0; i < thermalRooms.Count; i++)
                        temps[thermalRooms[i].roomId] = thermalRooms[i].currentTempC;
                    _fungi.TickDay(currentDay, roomTemperatureOverride: roomId =>
                        temps.TryGetValue(roomId, out var t) ? t : 15f);
                }
                else
                {
                    _fungi.TickDay(currentDay);
                }
            }

            _justice?.TickDay(currentDay);
        }

        /// <summary>
        /// Placeholder for avatar/visual refresh when survivor limb state changes.
        /// Future: update portrait variant, sprite attachments, animation sets,
        /// and equipment restrictions based on AmputationSystem limb conditions.
        /// </summary>
        private void RefreshSurvivorVisuals(string survivorId)
        {
            // TODO: Integrate with survivor portrait/avatar system when available.
            GD.Print($"[Main.Plans190_193] Visual refresh requested for survivor '{survivorId}' (avatar system not yet integrated).");
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

        private void HandleJusticeAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseJusticeTribunalPanel(); return; }
            if (_justiceTribunalPanel == null || _justice == null) return;

            switch (action)
            {
                case "report":
                {
                    var parts = param.Split(':');
                    if (parts.Length != 2 || string.IsNullOrEmpty(parts[0])) break;
                    if (!System.Enum.TryParse<CrimeType>(parts[1], out var crime)) break;
                    string incidentId = $"inc_{parts[0]}_{parts[1]}_{_simDay}";
                    var inc = _justice.ReportCrime(incidentId, crime, parts[0], victimId: null, _simDay);
                    _justiceTribunalPanel.ShowFeedback(
                        inc != null ? $"Report filed. The case joins the docket for the tribunal's day."
                                    : "The report could not be filed.",
                        inc == null);
                    break;
                }
            }
            _justiceTribunalPanel.RefreshView();
        }

        private void HandleRailwayAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseRailwayTerminalPanel(); return; }
            if (_railwayTerminalPanel == null || _railway == null) return;

            Ashfall.Core.ActionResult? res = action switch
            {
                "repair_track" => _railway.RepairTrack(param, integrityRestored: 0.25f),
                "repair_bridge" => _railway.RepairBridge(param),
                "clear_obstacle" => _railway.ClearTrackObstacle(param),
                "clear_derailment" => _railway.ClearDerailment(param),
                "service" => _railway.ServiceTransmission(param),
                _ => null
            };

            if (res != null)
                _railwayTerminalPanel.ShowFeedback(
                    res.Value.IsSuccess ? "Done. The line is one step closer to running."
                                  : "The crew couldn't do it — check what the terminal says is missing.",
                    !res.Value.IsSuccess);
            _railwayTerminalPanel.RefreshView();
        }

        private void HandleArchaeologyAction(string action, string param = "")
        {
            if (action == "CLOSE") { CloseArchaeologyExcavationPanel(); return; }
            if (_archaeologyExcavationPanel == null || _archaeology == null) return;

            switch (action)
            {
                case "decrypt":
                {
                    var res = _archaeology.ProgressDecryption(param, hours: 8f, engineerSkill: 0.5f, hasPower: true);
                    _archaeologyExcavationPanel.ShowFeedback(
                        res.IsSuccess ? "The decryption shift worked through the cipher layer."
                                      : "The shift made no headway — higher tiers need power or a keycard.",
                        !res.IsSuccess);
                    break;
                }
                case "sell":
                {
                    var res = _archaeology.SellArchiveToBroker(param);
                    _archaeologyExcavationPanel.ShowFeedback(
                        res.IsSuccess ? "The broker took the archive and paid in kind."
                                      : "The broker refused the archive.",
                        !res.IsSuccess);
                    break;
                }
            }
            _archaeologyExcavationPanel.RefreshView();
        }

        private void CloseAmputationTriagePanel() { _amputationTriagePanel?.Visible = false; }
        private void CloseJusticeTribunalPanel() { _justiceTribunalPanel?.Visible = false; }
        private void CloseRailwayTerminalPanel() { _railwayTerminalPanel?.Visible = false; }
        private void CloseArchaeologyExcavationPanel() { _archaeologyExcavationPanel?.Visible = false; }
    }
}
