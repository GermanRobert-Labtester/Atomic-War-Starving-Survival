// SPDX-License-Identifier: MIT
// ASHFALL XP-08-F6 — seasonal migration consequence host wiring.
//
// Constructs MigrationConsequenceEngine over the LIVE hosted
// SeasonalHumanMigrationEngine the campaign already ticks (never a second
// migration engine), and routes regional market consequences through the
// canonical market owner's idempotent shock seam.

using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private MigrationConsequenceHostSession? _migrationConsequence;
        private bool _migrationConsequenceDirty;
        private int _migrationConsequenceDay;

        public MigrationConsequenceHostSession? MigrationConsequence => _migrationConsequence;

        public void SetupMigrationConsequence()
        {
            if (_migrationConsequence != null)
            {
                BindMigrationConsequenceEngine();
                return;
            }

            // The migration engine must exist first; SetupHumanMigration owns it.
            SetupHumanMigration();
            if (_humanMigration == null) return;

            _migrationConsequence = new MigrationConsequenceHostSession(_humanMigration.Engine);
            var saved = MigrationConsequenceSaveStore.TryLoad();
            if (saved != null) _migrationConsequence.RestoreState(saved);

            _migrationConsequence.StateChanged += () => _migrationConsequenceDirty = true;
            BindMigrationConsequenceEngine();
        }

        /// <summary>
        /// Re-binds to the live migration instance. Setup order is not guaranteed,
        /// so the consequence engine is re-created over the live engine whenever
        /// the migration owner is (re)constructed. The persisted ledger is carried
        /// across so exactly-once survives the rebind.
        /// </summary>
        private void BindMigrationConsequenceEngine()
        {
            if (_humanMigration == null) return;
            if (_migrationConsequence == null) return;

            var restored = _migrationConsequence.CaptureState();
            _migrationConsequence = new MigrationConsequenceHostSession(_humanMigration.Engine);
            _migrationConsequence.RestoreState(restored);
            _migrationConsequence.StateChanged += () => _migrationConsequenceDirty = true;
        }

        /// <summary>
        /// One canonical day: apply the seasonal consequence for every authored
        /// region the live migration owner tracks, and route the market leg
        /// through the canonical market owner's idempotent shock seam.
        /// </summary>
        public void TickMigrationConsequence(int day)
        {
            SetupMigrationConsequence();
            if (_migrationConsequence == null || _humanMigration == null) return;
            if (day == _migrationConsequenceDay) return;
            _migrationConsequenceDay = day;

            string phase = SeasonPhaseForDay(day);
            foreach (var region in _humanMigration.RegionWeights.Keys)
            {
                if (string.IsNullOrEmpty(region)) continue;
                if (!_migrationConsequence.TryApplyPhaseConsequence(day, phase, region)) continue;
                ApplyMigrationMarketConsequence(region, phase);
            }
        }

        internal static string SeasonPhaseForDay(int day) => (day % 360) switch
        {
            < 90 => "deep_winter",
            < 180 => "thaw",
            < 270 => "dry_heat",
            _ => "ash_winds"
        };

        /// <summary>
        /// Routes the food-demand leg through the canonical market owner. The
        /// market owner applies its own clamp and per-source idempotence; the
        /// host supplies only the derived multiplier and the region/phase source id.
        /// </summary>
        private void ApplyMigrationMarketConsequence(string regionId, string phase)
        {
            var market = _economy?.Market;
            if (market == null) return;

            int demandPermille = _migrationConsequence!.GetMarketDemandMultiplierPermille(regionId, "food");
            if (demandPermille <= 0) return;

            // The market owner's clamp is the authority; the host only projects
            // the engine's deviation into its basis-point range.
            int deviationBp = Math.Abs((demandPermille - MigrationConsequenceEngine.PermilleScale)) * 10;
            if (deviationBp == 0) return;

            bool isShortage = demandPermille > MigrationConsequenceEngine.PermilleScale;
            float severityBp = MarketSystem.ShockSeverityMinBp
                + (MarketSystem.ShockSeverityMaxBp - MarketSystem.ShockSeverityMinBp)
                  * Math.Clamp(deviationBp / 1000f, 0f, 1f);
            string sourceId = MigrationConsequenceHostSession.MarketShockSourceId(regionId, phase);

            var shock = market.ApplyShock(
                "food", isShortage, severityBp, _migrationConsequenceDay, 1,
                MigrationConsequenceHostSession.MarketShockSourceId(regionId, phase));
            if (shock == null)
            {
                // Unknown category or unbound commodity catalog: the consequence is
                // still recorded in the engine's exactly-once ledger, but no price
                // leg is fabricated.
                return;
            }
        }

        public MigrationConsequenceProjection? GetMigrationConsequenceProjection(string regionId)
        {
            SetupMigrationConsequence();
            return _migrationConsequence?.GetProjection(regionId);
        }

        public void SaveMigrationConsequence()
        {
            if (_migrationConsequence == null) return;
            var state = _migrationConsequence.CaptureState();
            if (CaptureSection("migration_consequence", MigrationConsequenceSaveStore.TryCapturePersisted(state)))
                _migrationConsequenceDirty = false;
        }

        public void FlushMigrationConsequenceIfDirty()
        {
            if (_migrationConsequenceDirty) SaveMigrationConsequence();
        }

        public void ResetMigrationConsequence()
        {
            _migrationConsequence = null;
            _migrationConsequenceDirty = false;
            _migrationConsequenceDay = 0;
        }
    }
}
