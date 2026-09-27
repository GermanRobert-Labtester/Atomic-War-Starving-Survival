// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : MigrationConsequenceHostSession
// Purpose      : XP-08-F6 — binds MigrationConsequenceEngine to the LIVE hosted
//                SeasonalHumanMigrationEngine instance the campaign already
//                ticks. No second migration engine, no direct price mutation.
//                Market consequences route through MarketSystem.ApplyShock so
//                the market owner keeps its own clamp and per-source idempotence.
// ============================================================================

using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    /// <summary>Immutable projection of one region's migration consequence surface.</summary>
    public sealed class MigrationConsequenceProjection
    {
        public string RegionId = string.Empty;
        public int PopulationWeight;
        public int FoodDemandMultiplierPermille;
        public int LaborPoolMultiplierPermille;
        public int TerritorialFrictionMultiplierPermille;
        public string CaravanDemandPriority = string.Empty;
        public int AppliedConsequenceCount;
        public string LastEvent = string.Empty;
    }

    public sealed class MigrationConsequenceHostSession : HostSessionBase
    {
        private readonly MigrationConsequenceEngine _engine;
        private string _lastEvent = string.Empty;

        public MigrationConsequenceEngine Engine => _engine;

        /// <summary>The live migration engine the consequence engine derives from.</summary>
        public SeasonalHumanMigrationEngine MigrationEngine => _engine.MigrationEngine;

        public string LastEvent => _lastEvent;

        /// <summary>
        /// Constructed over the live migration engine. The host never builds a
        /// second SeasonalHumanMigrationEngine.
        /// </summary>
        public MigrationConsequenceHostSession(SeasonalHumanMigrationEngine migrationEngine)
        {
            _engine = new MigrationConsequenceEngine(migrationEngine);
        }

        public int GetRegionPopulationWeight(string regionId) =>
            _engine.GetRegionPopulationWeight(regionId);

        public int GetMarketDemandMultiplierPermille(string regionId, string itemCategory) =>
            _engine.GetMarketDemandMultiplierPermille(regionId, itemCategory);

        public int GetLaborPoolMultiplierPermille(string regionId) =>
            _engine.GetLaborPoolSizeMultiplierPermille(regionId);

        public int GetTerritorialFrictionMultiplierPermille(string regionId) =>
            _engine.GetTerritorialFrictionMultiplierPermille(regionId);

        public string GetCaravanDemandPriority(string regionId) =>
            _engine.GetCaravanDemandPriority(regionId);

        /// <summary>
        /// Applies one seasonal phase consequence. Exactly-once is enforced by the
        /// engine's own applied-key ledger, which survives save/load.
        /// </summary>
        public bool TryApplyPhaseConsequence(int currentDay, string seasonPhase, string regionId)
        {
            bool applied = _engine.TryApplyPhaseConsequence(currentDay, seasonPhase, regionId);
            if (applied)
            {
                _lastEvent = $"Migration consequence applied for '{regionId}' at phase '{seasonPhase}' on day {currentDay}.";
                RaiseStateChanged();
            }
            return applied;
        }

        /// <summary>
        /// Source id the market owner uses for its per-source idempotence, so a
        /// migration consequence can never be applied twice across a save/load.
        /// </summary>
        public static string MarketShockSourceId(string regionId, string seasonPhase) =>
            $"migration_{regionId}_{seasonPhase}";

        public MigrationConsequenceProjection GetProjection(string regionId) => new MigrationConsequenceProjection
        {
            RegionId = regionId ?? string.Empty,
            PopulationWeight = _engine.GetRegionPopulationWeight(regionId),
            FoodDemandMultiplierPermille = _engine.GetMarketDemandMultiplierPermille(regionId, "food"),
            LaborPoolMultiplierPermille = _engine.GetLaborPoolSizeMultiplierPermille(regionId),
            TerritorialFrictionMultiplierPermille = _engine.GetTerritorialFrictionMultiplierPermille(regionId),
            CaravanDemandPriority = _engine.GetCaravanDemandPriority(regionId),
            AppliedConsequenceCount = _engine.AppliedConsequenceKeys.Count,
            LastEvent = _lastEvent
        };

        // ── Save / Load ──────────────────────────────────────────────────

        public MigrationConsequenceSaveState CaptureState() => _engine.CaptureState();

        public void RestoreState(MigrationConsequenceSaveState? state)
        {
            _engine.RestoreState(state);
            _lastEvent = "Migration consequence ledger restored.";
            RaiseStateChanged();
        }

        public void Reset()
        {
            _engine.RestoreState(null);
            _lastEvent = string.Empty;
            RaiseStateChanged();
        }
    }
}
