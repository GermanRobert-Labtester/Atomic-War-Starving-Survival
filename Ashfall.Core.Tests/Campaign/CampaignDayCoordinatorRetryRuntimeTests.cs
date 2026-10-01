// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    /// <summary>
    /// Plan 38 follow-up / coordinator retry hardening.
    ///
    /// The inventory rollback contract was previously pinned only by a static
    /// source gate plus an abstract counter test. This file proves the runtime
    /// behavior end to end with a realistic ration/producer ledger:
    ///
    ///  * a fail-closed day advance where a late owner throws mid-day,
    ///  * a retry of the SAME day after the fault is cleared,
    ///  * no double-consumed rations,
    ///  * no lost or doubled producer output,
    ///  * a final inventory checksum identical to a no-failure baseline.
    ///
    /// The models implement the same <see cref="IPreDaySnapshotRestore"/> seam
    /// the host owners (<c>inventory_custody</c>, <c>starting_level_rations</c>,
    /// <c>crafting_production</c>, ...) implement, so a regression that drops
    /// rollback from the contract fails here at runtime.
    /// </summary>
    public class CampaignDayCoordinatorRetryRuntimeTests
    {
        private const string Food = "canned_food";
        private const string Water = "clean_water";
        private const string Scrap = "scrap_metal";

        [Fact]
        public void RetryAfterMidDayFailure_DoesNotDoubleConsumeOrDuplicateProducerOutput()
        {
            // ── Control: a clean day with no fault ─────────────────────────
            var controlLedger = NewLedger();
            var control = BuildCoordinator(controlLedger, out var controlFault, out _);
            controlFault.Armed = false;
            var controlResult = control.Advance(1);
            Assert.NotNull(controlResult);
            Assert.True(controlResult!.Succeeded);
            string baselineChecksum = controlLedger.Checksum();

            // ── Fault run: an owner throws after the consumer/producer ran ──
            var retryLedger = NewLedger();
            var retry = BuildCoordinator(retryLedger, out var fault, out var rations);
            var firstAttempt = retry.Advance(1);
            Assert.NotNull(firstAttempt);
            Assert.True(firstAttempt!.HasFailures);
            Assert.Equal(-1, retry.LastAdvancedDay); // nothing committed

            // The first attempt already mutated the ledger (ration consumed,
            // scrap produced). Those mutations must not survive the retry.
            Assert.Equal(7, retryLedger.Count(Food));
            Assert.Equal(2, retryLedger.Count(Scrap));

            // ── Retry the SAME day after clearing the fault ─────────────────
            fault.Armed = false;
            var secondAttempt = retry.Advance(1);
            Assert.NotNull(secondAttempt);
            Assert.True(secondAttempt!.Succeeded);
            Assert.Equal(1, retry.LastAdvancedDay);
            Assert.Equal(1, rations.RestoreCount); // rolled back exactly once

            // No double consumption: 10 → 7, not 10 → 4.
            Assert.Equal(7, retryLedger.Count(Food));
            // Producer output neither lost nor doubled: 0 → 2, not 0 → 4.
            Assert.Equal(2, retryLedger.Count(Scrap));
            Assert.Equal(7, retryLedger.Count(Water));

            // The retried day must reach the exact no-failure baseline.
            Assert.Equal(baselineChecksum, retryLedger.Checksum());
        }

        [Fact]
        public void SnapshotPreflightFailure_AbortsBeforeAnyTick_ThenRetryLandsOneCleanDay()
        {
            // A snapshot-preflight failure aborts in fail-closed mode before a
            // single owner ticks, so the retry must land exactly one clean day.
            var ledger = NewLedger();
            var coordinator = new CampaignDayCoordinator();
            var rations = new RationsOwner(ledger);
            var captureFault = new CaptureFaultOwner { Armed = true };
            coordinator.Register("inventory_custody", new InventoryCustodyOwner(ledger), phase: 1);
            coordinator.Register("capture_fault", captureFault, phase: 1);
            coordinator.Register("starting_level_rations", rations, phase: 2);

            Assert.True(coordinator.Advance(3)!.HasFailures);
            Assert.Equal(10, ledger.Count(Food)); // preflight aborted before any tick

            captureFault.Armed = false;
            Assert.True(coordinator.Advance(3)!.Succeeded);
            Assert.Equal(7, ledger.Count(Food)); // exactly one consumption
            Assert.Equal(1, rations.RestoreCount); // preflight rollback ran before the retry
        }

        [Fact]
        public void PersistenceFailure_AfterOwnerTicks_RollsBackOnRetryAndPersistsOnce()
        {
            var ledger = NewLedger();
            var coordinator = BuildCoordinator(ledger, out var fault, out var rations);
            fault.Armed = false;
            var persistence = new FlakyPersistence { FailuresRemaining = 1 };

            var first = coordinator.Advance(2, persistence);
            Assert.NotNull(first);
            Assert.True(first!.HasFailures);
            Assert.Equal(-1, coordinator.LastAdvancedDay); // persistence failure must not commit
            Assert.Equal(7, ledger.Count(Food));

            var second = coordinator.Advance(2, persistence);
            Assert.NotNull(second);
            Assert.True(second!.Succeeded);
            Assert.Equal(2, coordinator.LastAdvancedDay);
            Assert.Equal(7, ledger.Count(Food)); // not double consumed
            Assert.Equal(2, ledger.Count(Scrap));
            Assert.Equal(1, rations.RestoreCount);
            Assert.Equal(2, persistence.PersistCalls); // attempted once per advance, succeeded once
        }

        private static Dictionary<string, int> NewLedger() => new(StringComparer.Ordinal)
        {
            [Food] = 10,
            [Water] = 7,
            [Scrap] = 0,
        };

        private static CampaignDayCoordinator BuildCoordinator(
            Dictionary<string, int> ledger,
            out FaultOwner fault,
            out RationsOwner rations)
        {
            var coordinator = new CampaignDayCoordinator();
            var custody = new InventoryCustodyOwner(ledger);
            rations = new RationsOwner(ledger);
            var producer = new ProducerOwner(ledger);
            fault = new FaultOwner { Armed = true };

            coordinator.Register("inventory_custody", custody, phase: 1);
            coordinator.Register("starting_level_rations", rations, phase: 2);
            coordinator.Register("crafting_production", producer, phase: 2);
            // Phase 5 sorts last: every mutating owner has already ticked when
            // the fault fires, so a retry must undo the whole day.
            coordinator.Register("late_fault", fault, phase: 5);
            return coordinator;
        }

        /// <summary>
        /// Minimal inventory authority: the real host owner snapshots and
        /// restores the canonical inventory container; this models that seam
        /// with a checksum-able ledger.
        /// </summary>
        private sealed class InventoryCustodyOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Dictionary<string, int> _ledger;
            private Dictionary<string, int>? _snapshot;
            public InventoryCustodyOwner(Dictionary<string, int> ledger) => _ledger = ledger;

            public void CapturePreDaySnapshot(int day) => _snapshot = new Dictionary<string, int>(_ledger, StringComparer.Ordinal);

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot == null) return;
                _ledger.Clear();
                foreach (var kvp in _snapshot) _ledger[kvp.Key] = kvp.Value;
            }

            public void TickDay(int day, List<DayStateChangeEvent> events) { }
        }

        private sealed class RationsOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Dictionary<string, int> _ledger;
            private Dictionary<string, int>? _snapshot;
            public int RestoreCount { get; private set; }
            public RationsOwner(Dictionary<string, int> ledger) => _ledger = ledger;

            public void CapturePreDaySnapshot(int day) => _snapshot = new Dictionary<string, int>(_ledger, StringComparer.Ordinal);

            public void RestorePreDaySnapshot(int day)
            {
                RestoreCount++;
                if (_snapshot == null) return;
                _ledger.Clear();
                foreach (var kvp in _snapshot) _ledger[kvp.Key] = kvp.Value;
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                Consume(Food, 3);
                Consume(Water, 0);
                events.Add(new DayStateChangeEvent("consumed_rations", "starting_level_rations", Food, null, 3));
            }

            private void Consume(string id, int amount)
            {
                _ledger.TryGetValue(id, out int have);
                _ledger[id] = Math.Max(0, have - amount);
            }
        }

        private sealed class ProducerOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            private readonly Dictionary<string, int> _ledger;
            private Dictionary<string, int>? _snapshot;
            public ProducerOwner(Dictionary<string, int> ledger) => _ledger = ledger;

            public void CapturePreDaySnapshot(int day) => _snapshot = new Dictionary<string, int>(_ledger, StringComparer.Ordinal);

            public void RestorePreDaySnapshot(int day)
            {
                if (_snapshot == null) return;
                _ledger.Clear();
                foreach (var kvp in _snapshot) _ledger[kvp.Key] = kvp.Value;
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _ledger.TryGetValue(Scrap, out int have);
                _ledger[Scrap] = have + 2;
                events.Add(new DayStateChangeEvent("crafting_completed", "crafting_production", Scrap, null, 2));
            }
        }

        private sealed class FaultOwner : IDayAdvanceOwner
        {
            public bool Armed;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                if (Armed) throw new InvalidOperationException("injected mid-advance fault");
            }
        }

        private sealed class CaptureFaultOwner : IDayAdvanceOwner
        {
            public bool Armed;
            public void CapturePreDaySnapshot(int day)
            {
                if (Armed) throw new InvalidOperationException("injected snapshot-preflight fault");
            }
            public void TickDay(int day, List<DayStateChangeEvent> events) { }
        }

        private sealed class FlakyPersistence : IDayAdvancePersistence
        {
            public int FailuresRemaining;
            public int PersistCalls;
            public void PersistBeforeBriefing(int day, IReadOnlyList<DayOwnerReport> ownerReports)
            {
                PersistCalls++;
                if (FailuresRemaining > 0)
                {
                    FailuresRemaining--;
                    throw new InvalidOperationException("injected persistence fault");
                }
            }
        }
    }

    internal static class LedgerChecksum
    {
        public static int Count(this Dictionary<string, int> ledger, string id)
            => ledger.TryGetValue(id, out int v) ? v : 0;

        public static string Checksum(this Dictionary<string, int> ledger)
        {
            var sb = new StringBuilder();
            foreach (var key in ledger.Keys.OrderBy(k => k, StringComparer.Ordinal))
                sb.Append(key).Append(':').Append(ledger[key].ToString(CultureInfo.InvariantCulture)).Append(';');
            return sb.ToString();
        }
    }
}
