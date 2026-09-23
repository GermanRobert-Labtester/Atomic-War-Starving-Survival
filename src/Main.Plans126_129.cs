// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 126-129 Host Wire & Orchestration
// Subsystems   : Plan 126 — Subterranean Biological Fermentation (this wave)
//                Plans 127-129 are not tracked in this file; see the census
//                (no drone/caster/lidar implementation exists at this HEAD).
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private BioFermentationEngine? _bioFermentation;
        private BioFermentationHostSession _bioFermentationSession = null!;
        private bool _bioFermentationDirty;

        // ── Plan 126: Biological Fermentation ─────────────────────────────

        public BioFermentationEngine EnsureBioFermentation()
        {
            if (_bioFermentation != null) return _bioFermentation;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("bio_fermentation") : new SeededRng(1260);
            // `Inventory` is both the namespace imported above and the runtime
            // container type.  Qualify the type so this partial cannot resolve
            // the second `Inventory` as a nested type on the first one.
            var inv = _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory();

            _bioFermentation = new BioFermentationEngine(inv, rng, new GodotLog());

            string catalogPath = CatalogPath.ResolveCatalog("bio_fermentation_catalog.json");
            var _catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (_catalogIo.FileExists(catalogPath))
            {
                string json = _catalogIo.ReadAllText(catalogPath);
                {
                    try
                    {
                        var catalog = System.Text.Json.JsonSerializer.Deserialize<BioFermentationCatalog>(json);
                        if (catalog != null)
                            _bioFermentation.BindCatalog(catalog);
                    }
                    catch (Exception ex)
                    {
                        GD.PrintErr($"[Main.BioFermentation] Failed to parse {catalogPath}: {ex.Message}");
                    }
                }
            }

            // Power predicate: the fermenter bay stalls without grid power.
            _bioFermentation.IsRoomPowered = () =>
                _powerGrid == null || _powerGrid.System == null || _powerGrid.System.IsRoomPowered(BioFermentationEngine.RoomId);

            // Operator traits: no over-roster trait-activation seam exists yet;
            // the roster trait seam is a follow-up (engine contract already
            // pinned by BioFermentationEngineTests — snapshot, once per batch).
            _bioFermentation.TraitsOf = _ => Array.Empty<string>();
            _bioFermentation.OperatorSkillProvider = _ => 0.5f;
            _bioFermentation.RoomTempC = () => 14f; // fermenter gallery, thermostatically kept cool

            var saved = BioFermentationSaveStore.TryLoad();
            if (saved != null)
                _bioFermentation.RestoreState(saved);

            _bioFermentationSession = new BioFermentationHostSession(_bioFermentation);

            _bioFermentation.OnBatchCompleted += processId =>
            {
                _journal?.TryAddRawEntry("fermentation_batch_complete",
                    $"The fermenter reports a finished batch — output ready to draw off ({processId}).",
                    null!, _simDay);
            };
            _bioFermentation.OnContaminationEvent += (before, after) =>
            {
                if (after == "spoiled")
                    _journal?.TryAddRawEntry("fermentation_spoiled",
                        "The fermentation batch has spoiled. The reactor needs servicing before it can run again.",
                        null!, _simDay);
            };
            _bioFermentation.OnFaultChange += fault =>
            {
                if (fault == "filter_clogged")
                    _journal?.TryAddRawEntry("fermentation_filter_clogged",
                        "The fermenter's breather filter has clogged — the next batch will stall without service.",
                        null!, _simDay);
            };

            return _bioFermentation;
        }

        /// <summary>Panel-facing session wrapper (LastEvent feedback strip).</summary>
        public BioFermentationHostSession EnsureBioFermentationSession()
        {
            EnsureBioFermentation();
            return _bioFermentationSession;
        }

        private void SetupBioFermentation()
        {
            EnsureBioFermentation();
        }

        private void SaveBioFermentation()
        {
            if (_bioFermentation != null)
            {
                CaptureSection("bio_fermentation", BioFermentationSaveStore.TryCapturePersisted(_bioFermentation.CaptureState()));
            }
        }

        /// <summary>Daily tick for the flagship tranche (Plan 126 active this wave).</summary>
        private void TickPlans126_129(int currentDay)
        {
            _bioFermentation?.TickDay(currentDay);
        }
    }
}
