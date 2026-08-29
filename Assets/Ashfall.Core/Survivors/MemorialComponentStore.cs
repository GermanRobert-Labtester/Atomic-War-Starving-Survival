// SPDX-License-Identifier: MIT
// Task #132 — Typed Memorial component and detached persistence boundary.
using System;
using System.Collections.Generic;
using Ashfall.Core.Memorial;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// A detached, snake_case save row for one survivor's historical Memorial
    /// record. The legacy Memorial save remains authoritative during migration;
    /// this row is the future component-shaped representation of the same data.
    /// </summary>
    [Serializable]
    public sealed class MemorialRecordState
    {
        public string survivor_id = string.Empty;
        public string cause = string.Empty;
        public int day;
        public int survived_days;
        public bool final_wish_resolved;
        public string epitaph = string.Empty;
        public string heirloom_item_id = string.Empty;
        public string heirloom_recipient_id = string.Empty;
        public float morale_delta;

        internal MemorialRecord ToRecord(SurvivorId owner)
            => new MemorialRecord(
                owner,
                cause,
                day,
                survived_days,
                final_wish_resolved,
                epitaph,
                heirloom_item_id,
                heirloom_recipient_id,
                morale_delta);

        internal static MemorialRecordState Capture(MemorialRecord source)
            => new MemorialRecordState
            {
                survivor_id = source.SurvivorId.Value,
                cause = source.Cause,
                day = source.Day,
                survived_days = source.SurvivedDays,
                final_wish_resolved = source.FinalWishResolved,
                epitaph = source.Epitaph,
                heirloom_item_id = source.HeirloomItemId,
                heirloom_recipient_id = source.HeirloomRecipientId,
                morale_delta = source.MoraleDelta
            };
    }

    /// <summary>Detached persisted state for the typed Memorial component.</summary>
    [Serializable]
    public sealed class MemorialComponentStoreState
    {
        public const string CurrentSystemId = MemorialComponentStore.SystemId;
        public const int CurrentSchemaVersion = MemorialComponentStore.SchemaVersion;

        public int schema_version = CurrentSchemaVersion;
        public string system_id = CurrentSystemId;
        public List<MemorialRecordState> records = new List<MemorialRecordState>();
    }

    /// <summary>Diagnostics produced while replacing a Memorial component from detached state.</summary>
    public sealed class MemorialComponentRestoreReport
    {
        public int Accepted { get; internal set; }
        public List<string> Rejected { get; } = new List<string>();
        public bool IsFatal { get; internal set; }
        public string FatalReason { get; internal set; } = string.Empty;

        public bool IsClean => !IsFatal && Rejected.Count == 0;

        public override string ToString()
            => IsFatal
                ? $"[MemorialRestore] FATAL: {FatalReason}"
                : $"[MemorialRestore] accepted={Accepted} rejected={Rejected.Count}";
    }

    /// <summary>
    /// Immutable typed projection of one historical Memorial row.
    /// String values are non-null inside the typed boundary; null legacy strings
    /// are represented as empty strings by the import boundary and surfaced by
    /// the parity harness as malformed legacy fields.
    /// </summary>
    public sealed class MemorialRecord
    {
        public SurvivorId SurvivorId { get; }
        public SurvivorId OwnerId => SurvivorId;
        public string Cause { get; }
        public int Day { get; }
        public int SurvivedDays { get; }
        public bool FinalWishResolved { get; }
        public string Epitaph { get; }
        public string HeirloomItemId { get; }
        public string HeirloomRecipientId { get; }
        public float MoraleDelta { get; }

        public MemorialRecord(
            SurvivorId survivorId,
            string? cause,
            int day,
            int survivedDays,
            bool finalWishResolved,
            string? epitaph,
            string? heirloomItemId,
            string? heirloomRecipientId,
            float moraleDelta)
        {
            if (survivorId.IsEmpty)
                throw new ArgumentException("MemorialRecord requires a non-empty SurvivorId.", nameof(survivorId));

            SurvivorId = survivorId;
            Cause = cause ?? string.Empty;
            Day = day;
            SurvivedDays = survivedDays;
            FinalWishResolved = finalWishResolved;
            Epitaph = epitaph ?? string.Empty;
            HeirloomItemId = heirloomItemId ?? string.Empty;
            HeirloomRecipientId = heirloomRecipientId ?? string.Empty;
            MoraleDelta = moraleDelta;
        }
    }

    /// <summary>
    /// Typed, host-independent owner of historical Memorial records.
    ///
    /// <para>This is deliberately a ledger component, not a death system. It does
    /// not decide who dies, advance lifecycle state, resolve final wishes, move
    /// inventory, update morale, or raise host events. Those authorities remain in
    /// the existing Memorial and survivor domains until a later host cutover.</para>
    ///
    /// <para>Cardinality is ZeroOrOne because a survivor has at most one historical
    /// memorial record. The record is retained after death and the component is
    /// therefore not released by <see cref="SurvivorEntityStore.TryLeave"/>.</para>
    /// </summary>
    public sealed class MemorialComponentStore : ISurvivorComponentStore
    {
        public const string SystemId = "memorial_component";
        public const int SchemaVersion = 1;

        private readonly Dictionary<SurvivorId, MemorialRecord> _byOwner =
            new Dictionary<SurvivorId, MemorialRecord>();

        public string ComponentName => "memorial";
        public SurvivorComponentCardinality Cardinality => SurvivorComponentCardinality.ZeroOrOne;
        public bool RetainsHistoryAfterDeath => true;

        /// <summary>Owners in canonical ordinal order, never dictionary order.</summary>
        public IEnumerable<SurvivorId> OwnerIds => OrderedOwnerIds();

        public int Count => _byOwner.Count;

        public bool Contains(SurvivorId owner)
            => !owner.IsEmpty && _byOwner.ContainsKey(owner);

        public bool TryGet(SurvivorId owner, out MemorialRecord? record)
        {
            if (owner.IsEmpty)
            {
                record = null;
                return false;
            }
            return _byOwner.TryGetValue(owner, out record);
        }

        /// <summary>
        /// Record a historical row once. A later row for the same typed owner is
        /// ignored and the first canonical record is returned, matching the
        /// legacy MemorialSystem's first-entry-wins behavior.
        /// </summary>
        public MemorialRecord Record(MemorialRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            if (_byOwner.TryGetValue(record.SurvivorId, out var existing))
                return existing;

            _byOwner.Add(record.SurvivorId, record);
            return record;
        }

        /// <summary>Try-shaped alias for callers that need to know whether insertion occurred.</summary>
        public bool TryRecord(MemorialRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            if (_byOwner.ContainsKey(record.SurvivorId)) return false;
            _byOwner.Add(record.SurvivorId, record);
            return true;
        }

        /// <summary>
        /// Historical records are never released by an active-component teardown.
        /// The owner may leave the living roster, but the memorial remains history.
        /// </summary>
        public bool Release(SurvivorId owner) => false;

        /// <summary>Drop all records during an explicit slot reset or replacement.</summary>
        public void Reset() => _byOwner.Clear();

        /// <summary>
        /// Capture rows in ordinal owner order. Every row is a detached value copy;
        /// changing the returned state cannot change the immutable live records.
        /// </summary>
        public MemorialComponentStoreState CaptureState()
        {
            var state = new MemorialComponentStoreState
            {
                schema_version = SchemaVersion,
                system_id = SystemId
            };

            foreach (var owner in OrderedOwnerIds())
                state.records.Add(MemorialRecordState.Capture(_byOwner[owner]));

            return state;
        }

        /// <summary>
        /// Replace the component from detached state. Future schemas and foreign
        /// system ids are fatal and leave the current campaign untouched. Supported
        /// rows are staged first; malformed rows are rejected, and duplicate ids use
        /// first-row-wins just like the legacy ledger.
        /// </summary>
        public MemorialComponentRestoreReport RestoreState(MemorialComponentStoreState? saved)
        {
            var report = new MemorialComponentRestoreReport();

            if (saved == null)
            {
                // A missing component section is the explicit empty/reset form,
                // matching the existing typed survivor stores. It is not a partial
                // restore: the caller has supplied no rows to retain.
                Reset();
                return report;
            }

            if (saved.schema_version > SchemaVersion)
            {
                report.IsFatal = true;
                report.FatalReason =
                    $"Memorial component save schema_version {saved.schema_version} is newer than this build supports ({SchemaVersion}).";
                return report;
            }

            if (!string.IsNullOrEmpty(saved.system_id) &&
                !string.Equals(saved.system_id, SystemId, StringComparison.Ordinal))
            {
                report.IsFatal = true;
                report.FatalReason =
                    $"Memorial component save system_id '{saved.system_id}' does not match '{SystemId}'.";
                return report;
            }

            var restored = new Dictionary<SurvivorId, MemorialRecord>();
            if (saved.records != null)
            {
                for (int i = 0; i < saved.records.Count; i++)
                {
                    var row = saved.records[i];
                    if (row == null)
                    {
                        report.Rejected.Add($"row {i}: null entry");
                        continue;
                    }

                    if (!SurvivorId.TryParse(row.survivor_id, out var owner, out string idError))
                    {
                        report.Rejected.Add($"row {i}: {idError}");
                        continue;
                    }

                    if (restored.ContainsKey(owner))
                    {
                        report.Rejected.Add(
                            $"row {i}: duplicate survivor '{owner}' — first row wins.");
                        continue;
                    }

                    restored.Add(owner, row.ToRecord(owner));
                    report.Accepted++;
                }
            }

            _byOwner.Clear();
            foreach (var pair in restored)
                _byOwner.Add(pair.Key, pair.Value);

            return report;
        }

        private List<SurvivorId> OrderedOwnerIds()
        {
            var ordered = new List<SurvivorId>(_byOwner.Keys);
            ordered.Sort();
            return ordered;
        }
    }
}
