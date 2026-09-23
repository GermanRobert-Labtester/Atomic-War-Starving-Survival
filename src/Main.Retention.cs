// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 55 — The Long Haul: retention, a save corpus, and the 400-year
// campaign, in the running game.
//
// The authored retention_policies.json table is overlaid onto the Core
// retention authority, which is then applied on the canonical day tick to the
// owners that hold unbounded campaign logs. Nothing here owns a log: every
// collection stays with its existing Core authority, and only the audit
// report is persisted through the registered "retention" section.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Records;
using Ashfall.Core.Survivors;
using Ashfall.Core.Save;
using Ashfall.Core.Verdict;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private RetentionHostSession? _retention;
        private bool _retentionDirty;

        public RetentionHostSession? Retention => _retention;

        /// <summary>
        /// Build the retention session and overlay the authored policy table.
        /// A missing or invalid authored table leaves the built-in defaults in
        /// force (logged), so a campaign is never unbounded for lack of data.
        /// </summary>
        private RetentionHostSession? EnsureRetention()
        {
            if (_retention != null) return _retention;
            var session = new RetentionHostSession();
            session.LoadAuthoredPolicies(_dataDir, new FileSystemIO());
            if (!session.UsingAuthoredPolicies)
            {
                GD.PrintErr($"[Retention] authored policy table unavailable: {session.LoadError}; "
                    + "using built-in retention defaults.");
            }
            _retention = session;
            return _retention;
        }

        private void SetupRetention()
        {
            var session = EnsureRetention();
            if (session == null) return;
            session.BindOwners(
                _kitchenNutrition?.System,
                _yearOfAsh?.FactionWar,
                _verdict?.MachineLog,
                _doseLedger?.Ledger);
            if (session.UsingAuthoredPolicies)
            {
                GD.Print($"[Retention] {session.Catalog.ActivePolicies.Count} retention polic(ies) active "
                    + "(authored overlay on built-in defaults).");
            }
        }

        private void SaveRetention()
        {
            var session = _retention;
            if (session == null) return;
            session.BindOwners(
                _kitchenNutrition?.System,
                _yearOfAsh?.FactionWar,
                _verdict?.MachineLog,
                _doseLedger?.Ledger);
            var report = session.ApplyRetention();
            CaptureSection(
                RetentionSaveStore.SectionName,
                RetentionSaveStore.TryCapturePersisted(
                    RetentionSaveStore.From(report, session.Passes, session.UsingAuthoredPolicies)));
            _retentionDirty = false;
        }

        private void FlushRetentionIfDirty()
        {
            if (_retentionDirty) SaveRetention();
        }

        private void ResetRetention()
        {
            _retention?.Dispose();
            _retention = null;
            _retentionDirty = false;
        }

        /// <summary>
        /// Canonical day tick for retention. Runs last (phase 5), after every
        /// owner that appends to a bounded collection for the day.
        /// </summary>
        internal void TickRetention(int day)
        {
            var session = EnsureRetention();
            if (session == null) return;
            session.BindOwners(
                _kitchenNutrition?.System,
                _yearOfAsh?.FactionWar,
                _verdict?.MachineLog,
                _doseLedger?.Ledger);

            var report = session.ApplyRetention();
            if (report.TotalEntriesPruned <= 0) return;

            _retentionDirty = true;
            _journal?.TryAddRawEntry(
                "retention_applied",
                $"Campaign memory pruned: {report.TotalEntriesPruned} entr(y/ies) across "
                + $"{report.PrunedCollectionKeys.Count} collection(s) ("
                + string.Join(", ", report.PrunedCollectionKeys) + "). "
                + $"{report.TotalProtectedCollectionsPreserved} protected obligation collection(s) preserved.",
                null!, Math.Max(1, day));
        }
    }
}
