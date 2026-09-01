// SPDX-License-Identifier: MIT
// Task #132 — Legacy Memorial to typed component import boundary.
using System;
using System.Collections.Generic;
using Ashfall.Core.Memorial;

namespace Ashfall.Core.Survivors
{
    /// <summary>Stable categories emitted by the one-way Memorial import.</summary>
    public static class MemorialImportCode
    {
        public const string LegacyRowNull = "legacy_row_null";
        public const string LegacyIdInvalid = "legacy_id_invalid";
        public const string DuplicateId = "duplicate_id";
        public const string OwnerUnknown = "owner_unknown";
        public const string OwnerLiving = "owner_living";
    }

    /// <summary>Outcome of importing a legacy Memorial entry list into a typed store.</summary>
    public sealed class MemorialComponentImportReport
    {
        public int LegacyRows { get; internal set; }
        public int Accepted { get; internal set; }
        public List<string> Rejected { get; } = new List<string>();

        public bool IsClean => Rejected.Count == 0;

        public override string ToString()
            => $"[MemorialImport] legacy={LegacyRows} accepted={Accepted} rejected={Rejected.Count}";
    }

    /// <summary>
    /// One-way compatibility adapter from the existing raw Memorial ledger to the
    /// typed component. It never calls a lifecycle transition and never becomes a
    /// second save authority; the caller may use it during a later dual-run phase.
    /// </summary>
    public static class MemorialComponentAdapter
    {
        /// <summary>
        /// Import legacy entries into <paramref name="target"/>. The candidate rows
        /// are fully staged before the target is replaced. Invalid, null, duplicate,
        /// unknown, and living-owner rows are reported and omitted; valid historical
        /// rows are still imported with first-row-wins semantics.
        /// </summary>
        public static MemorialComponentImportReport ImportLegacy(
            IReadOnlyList<MemorialEntry> legacyEntries,
            MemorialComponentStore target,
            SurvivorEntityStore? survivors = null)
        {
            if (legacyEntries == null) throw new ArgumentNullException(nameof(legacyEntries));
            if (target == null) throw new ArgumentNullException(nameof(target));

            var report = new MemorialComponentImportReport
            {
                LegacyRows = legacyEntries.Count
            };
            var candidate = new MemorialComponentStoreState();
            var seen = new HashSet<SurvivorId>();

            for (int i = 0; i < legacyEntries.Count; i++)
            {
                var entry = legacyEntries[i];
                if (entry == null)
                {
                    report.Rejected.Add(
                        $"{MemorialImportCode.LegacyRowNull}: row {i}: null memorial entry");
                    continue;
                }

                // The raw id is parsed exactly once at this compatibility edge;
                // no trimming, lowercasing, or other identity normalization occurs.
                if (!SurvivorId.TryParse(entry.SurvivorId, out var owner, out string idError))
                {
                    report.Rejected.Add(
                        $"{MemorialImportCode.LegacyIdInvalid}: row {i}: {idError}");
                    continue;
                }

                if (!seen.Add(owner))
                {
                    report.Rejected.Add(
                        $"{MemorialImportCode.DuplicateId}: row {i}: duplicate survivor '{owner}' — first row wins.");
                    continue;
                }

                if (survivors != null)
                {
                    if (!survivors.TryGet(owner, out var aggregate))
                    {
                        report.Rejected.Add(
                            $"{MemorialImportCode.OwnerUnknown}: row {i}: survivor '{owner}' is not present in the canonical entity store");
                        continue;
                    }

                    if (aggregate.IsAlive)
                    {
                        report.Rejected.Add(
                            $"{MemorialImportCode.OwnerLiving}: row {i}: survivor '{owner}' is {aggregate.Lifecycle}; Memorial history may only be imported for a deceased survivor");
                        continue;
                    }
                }

                // MemorialRecord normalizes nullable legacy strings to the typed
                // empty-string default. Parity separately reports that source defect.
                candidate.records.Add(new MemorialRecordState
                {
                    survivor_id = entry.SurvivorId ?? string.Empty,
                    cause = entry.Cause ?? string.Empty,
                    day = entry.Day,
                    survived_days = entry.SurvivedDays,
                    final_wish_resolved = entry.FinalWishResolved,
                    epitaph = entry.Epitaph ?? string.Empty,
                    heirloom_item_id = entry.HeirloomItemId ?? string.Empty,
                    heirloom_recipient_id = entry.HeirloomRecipientId ?? string.Empty,
                    morale_delta = entry.MoraleDelta
                });
            }

            // Restore performs one final detached validation and replaces the target
            // only after candidate construction has completed.
            var restore = target.RestoreState(candidate);
            report.Accepted = restore.Accepted;
            for (int i = 0; i < restore.Rejected.Count; i++)
                report.Rejected.Add(restore.Rejected[i]);
            if (restore.IsFatal)
                report.Rejected.Add($"fatal: {restore.FatalReason}");

            return report;
        }

        /// <summary>Target-first overload for composition code that reads more naturally that way.</summary>
        public static MemorialComponentImportReport ImportLegacy(
            MemorialComponentStore target,
            IReadOnlyList<MemorialEntry> legacyEntries,
            SurvivorEntityStore? survivors = null)
            => ImportLegacy(legacyEntries, target, survivors);
    }
}
