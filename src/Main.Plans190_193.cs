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
            };

            _amputation.OnGangreneDeclared += (survivorId, limb) =>
            {
                _journal?.TryAddRawEntry("gangrene_warning", $"Critical medical emergency: {survivorId}'s {limb} wound has turned gangrenous!", null!, _simDay);
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

            _justice = new JusticeSystem(rng, inv, needs, new GodotLog());

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
            _fungi?.TickDay(currentDay);
            _justice?.TickDay(currentDay);
        }
    }
}
