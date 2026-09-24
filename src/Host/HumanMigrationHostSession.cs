// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : HumanMigrationSaveStore
// Core State : Ashfall.Core.Economy.SeasonalMigrationSaveState
// Host Caller: Main.HumanMigration (SetupHumanMigration / SaveHumanMigration)
// Purpose    : Plan 199 / C3-199 — Seasonal human migration schedule engine:
//              regional human population weights, seasonal dwell hysteresis,
//              and season phase transition handling.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class HumanMigrationSaveStore
    {
        public const string FileName = "human_migration_save.json";
        public const string SectionName = "human_migration";

        private static readonly SaveStore<SeasonalMigrationSaveState> s_store =
            SaveStoreHub.Checksummed<SeasonalMigrationSaveState>(FileName, nameof(HumanMigrationSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(SeasonalMigrationSaveState state) => s_store.CaptureBare(state);
        public static SeasonalMigrationSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(SeasonalMigrationSaveState state) => s_store.TrySave(state);
        public static SeasonalMigrationSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 199 (Seasonal Human Migration Engine).
    /// Binds seasonal migration schedules from seasonal_human_migration.json,
    /// tracks regional human population weights across settlements and regions,
    /// and enforces deterministic 10-day dwell hysteresis on season phase changes.
    /// Explicitly distinct from wildlife ecosystems; never mutates animal populations.
    /// </summary>
    public sealed class HumanMigrationHostSession : HostSessionBase
    {
        private SeasonalHumanMigrationEngine _engine;
        private SeasonalMigrationCatalog? _catalog;
        private string _lastEvent = string.Empty;

        public SeasonalHumanMigrationEngine Engine => _engine;
        public string LastEvent => _lastEvent;
        public HumanMigrationCensus Census => _engine.GetCensus();
        public IReadOnlyDictionary<string, int> RegionWeights => _engine.RegionWeights;
        public string LastAppliedPhase => _engine.LastAppliedPhase;
        public int LastTransitionDay => _engine.LastTransitionDay;

        private static readonly string[] DefaultRegions = new[]
        {
            "settlement",
            "iron_basin",
            "ash_flats",
            "deep_coast",
            "industrial_belt"
        };

        public HumanMigrationHostSession(string? dataDir = null, SeasonalHumanMigrationEngine? engine = null)
        {
            if (engine != null)
            {
                _engine = engine;
            }
            else
            {
                _engine = new SeasonalHumanMigrationEngine(knownRegions: DefaultRegions);
            }

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalogs(dataDir);
            }
        }

        public static HumanMigrationHostSession Create(string dataDir, SeasonalHumanMigrationEngine? engine = null)
        {
            return new HumanMigrationHostSession(dataDir, engine);
        }

        public void LoadCatalogs(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;

            try
            {
                var result = SeasonalMigrationCatalogLoader.Load(dataDir);
                if (result.Success && result.Catalog != null)
                {
                    _catalog = result.Catalog;
                    var state = _engine.CaptureState();
                    _engine = new SeasonalHumanMigrationEngine(_catalog, DefaultRegions);
                    _engine.RestoreState(state);
                    _lastEvent = $"Loaded migration catalog with {_catalog.Factions.Count} factions.";
                    RaiseStateChanged();
                }
            }
            catch (Exception ex)
            {
                _lastEvent = $"Failed to load migration catalog: {ex.Message}";
            }
        }

        public int GetRegionPopulationWeight(string regionId) => _engine.GetRegionPopulationWeight(regionId);

        /// <summary>
        /// Daily tick for seasonal human migration.
        /// Applies scheduled regional delta adjustments if a new season phase is reached
        /// and the 10-day dwell hysteresis period has elapsed.
        /// </summary>
        public bool TickDay(int currentDay, string seasonPhase)
        {
            bool applied = _engine.TickDay(currentDay, seasonPhase);
            if (applied)
            {
                _lastEvent = $"Migration shift applied for season phase '{seasonPhase}' on day {currentDay}.";
                RaiseStateChanged();
            }
            return applied;
        }

        public SeasonalMigrationSaveState CaptureState() => _engine.CaptureState();

        public void RestoreState(SeasonalMigrationSaveState? state)
        {
            _engine.RestoreState(state);
            RaiseStateChanged();
        }

        public void Reset()
        {
            _engine = new SeasonalHumanMigrationEngine(_catalog, DefaultRegions);
            _lastEvent = string.Empty;
            RaiseStateChanged();
        }
    }
}
