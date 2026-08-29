// SPDX-License-Identifier: MIT
// Task #132 — Typed Needs component and detached persistence boundary.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// A detached, snake_case save row for one survivor's Needs component.
    ///
    /// <para>The runtime state remains the existing <see cref="SurvivorNeedsState"
    /// /> so a host can dual-run this component beside <see cref="NeedsSystem" />
    /// without introducing a second set of need fields. The row is a copy: changing
    /// it after capture cannot mutate a live simulation state.</para>
    /// </summary>
    [Serializable]
    public sealed class NeedsComponentState
    {
        public string survivor_id = string.Empty;
        public float hunger;
        public float thirst;
        public float fatigue;
        public float warmth = 100f;
        public float morale = 50f;
        public float health = 100f;
        public float hygiene = 100f;

        public bool was_hunger_critical;
        public bool was_thirst_critical;
        public bool was_warmth_critical;

        public float max_health_cap = 100f;
        public bool is_alive = true;
        public bool is_dead;

        internal static NeedsComponentState Capture(SurvivorId owner, SurvivorNeedsState source)
        {
            return new NeedsComponentState
            {
                survivor_id = owner.Value,
                hunger = source.Hunger,
                thirst = source.Thirst,
                fatigue = source.Fatigue,
                warmth = source.Warmth,
                morale = source.Morale,
                health = source.Health,
                hygiene = source.Hygiene,
                was_hunger_critical = source.WasHungerCritical,
                was_thirst_critical = source.WasThirstCritical,
                was_warmth_critical = source.WasWarmthCritical,
                max_health_cap = source.MaxHealthCap,
                is_alive = source.IsAlive,
                is_dead = source.IsDead
            };
        }

        internal SurvivorNeedsState ToRuntimeState()
        {
            return new SurvivorNeedsState
            {
                Id = survivor_id,
                Hunger = hunger,
                Thirst = thirst,
                Fatigue = fatigue,
                Warmth = warmth,
                Morale = morale,
                Health = health,
                Hygiene = hygiene,
                WasHungerCritical = was_hunger_critical,
                WasThirstCritical = was_thirst_critical,
                WasWarmthCritical = was_warmth_critical,
                MaxHealthCap = max_health_cap,
                IsAlive = is_alive,
                IsDead = is_dead
            };
        }
    }

    /// <summary>Detached persisted state for the typed Needs component.</summary>
    [Serializable]
    public sealed class NeedsComponentStoreState
    {
        public const string CurrentSystemId = NeedsComponentStore.SystemId;
        public const int CurrentSchemaVersion = NeedsComponentStore.SchemaVersion;

        public int schema_version = CurrentSchemaVersion;
        public string system_id = CurrentSystemId;
        public List<NeedsComponentState> survivors = new List<NeedsComponentState>();
    }

    /// <summary>Diagnostics produced while replacing a Needs component from detached state.</summary>
    public sealed class NeedsComponentRestoreReport
    {
        public int Accepted { get; internal set; }
        public List<string> Rejected { get; } = new List<string>();
        public bool IsFatal { get; internal set; }
        public string FatalReason { get; internal set; } = string.Empty;

        public bool IsClean => !IsFatal && Rejected.Count == 0;

        public override string ToString()
            => IsFatal
                ? $"[NeedsRestore] FATAL: {FatalReason}"
                : $"[NeedsRestore] accepted={Accepted} rejected={Rejected.Count}";
    }

    /// <summary>
    /// The typed, host-independent owner of Needs component records.
    ///
    /// <para>This is intentionally a component store, not a replacement simulation
    /// system. During migration the existing <see cref="NeedsSystem" /> continues
    /// to apply gameplay rules; this store supplies one canonical typed key,
    /// deterministic owner ordering, and a detached persistence boundary for the
    /// dual-run/parity phase.</para>
    ///
    /// <para>Initial cardinality is <see cref="SurvivorComponentCardinality.ZeroOrOne" />.
    /// A missing record is legal while composition is staged. The component may be
    /// promoted to <c>OnePerEligible</c> only after every host guarantees creation
    /// for every eligible survivor.</para>
    /// </summary>
    public sealed class NeedsComponentStore : ISurvivorComponentStore
    {
        public const string SystemId = "needs_component";
        public const int SchemaVersion = 1;

        private readonly Dictionary<SurvivorId, SurvivorNeedsState> _byOwner =
            new Dictionary<SurvivorId, SurvivorNeedsState>();

        public string ComponentName => "needs";
        public SurvivorComponentCardinality Cardinality => SurvivorComponentCardinality.ZeroOrOne;
        public bool RetainsHistoryAfterDeath => false;

        /// <summary>
        /// Owner ids in canonical ordinal order. A fresh list prevents callers from
        /// mutating store ordering or observing dictionary iteration order.
        /// </summary>
        public IEnumerable<SurvivorId> OwnerIds => OrderedOwnerIds();

        public int Count => _byOwner.Count;

        public bool Contains(SurvivorId owner)
            => !owner.IsEmpty && _byOwner.ContainsKey(owner);

        public bool TryGet(SurvivorId owner, out SurvivorNeedsState? state)
        {
            if (owner.IsEmpty)
            {
                state = null;
                return false;
            }
            return _byOwner.TryGetValue(owner, out state);
        }

        /// <summary>
        /// Upsert a state under a typed owner. The raw id carried by the legacy
        /// runtime object must parse and equal the typed key exactly; otherwise the
        /// operation is refused without changing the existing record.
        /// </summary>
        public bool TryUpsert(SurvivorId owner, SurvivorNeedsState? state, out string error)
        {
            if (owner.IsEmpty)
            {
                error = "Needs component owner cannot be empty.";
                return false;
            }

            if (state == null)
            {
                error = $"Needs component owner '{owner}' cannot register a null state.";
                return false;
            }

            if (!SurvivorId.TryParse(state.Id, out var embeddedId, out string idError))
            {
                error = $"Needs state for owner '{owner}' carries an invalid raw id: {idError}";
                return false;
            }

            if (embeddedId != owner)
            {
                error = $"Needs state raw id '{embeddedId}' does not match typed owner '{owner}'.";
                return false;
            }

            _byOwner[owner] = state;
            error = string.Empty;
            return true;
        }

        /// <summary>Alias emphasizing that the typed registration is an upsert.</summary>
        public bool TryRegister(SurvivorId owner, SurvivorNeedsState? state, out string error)
            => TryUpsert(owner, state, out error);

        /// <summary>
        /// Compatibility boundary for callers that only have the legacy raw id.
        /// Parsing happens once here, then registration uses the typed overload.
        /// </summary>
        public bool TryUpsert(SurvivorNeedsState? state, out string error)
        {
            if (state == null)
            {
                error = "Needs component cannot register a null state.";
                return false;
            }

            if (!SurvivorId.TryParse(state.Id, out var owner, out string idError))
            {
                error = idError;
                return false;
            }

            return TryUpsert(owner, state, out error);
        }

        /// <summary>Release the active record for an owner, if present.</summary>
        public bool Release(SurvivorId owner)
        {
            if (owner.IsEmpty) return false;
            return _byOwner.Remove(owner);
        }

        /// <summary>Drop all active records before a slot switch or full restore.</summary>
        public void Reset() => _byOwner.Clear();

        /// <summary>
        /// Capture every record in ordinal owner order. The returned rows are
        /// detached copies, so serialization or mutation of the snapshot cannot
        /// affect the live state held by the dual-run simulation.
        /// </summary>
        public NeedsComponentStoreState CaptureState()
        {
            var state = new NeedsComponentStoreState
            {
                schema_version = SchemaVersion,
                system_id = SystemId
            };

            foreach (var owner in OrderedOwnerIds())
                state.survivors.Add(NeedsComponentState.Capture(owner, _byOwner[owner]));

            return state;
        }

        /// <summary>
        /// Replace the store from detached state. Future schemas are rejected
        /// before the current campaign is changed. For a supported schema, invalid
        /// rows are reported and omitted; duplicate ids use first-row-wins, matching
        /// the canonical entity restore rule.
        /// </summary>
        public NeedsComponentRestoreReport RestoreState(NeedsComponentStoreState? saved)
        {
            var report = new NeedsComponentRestoreReport();

            if (saved == null)
            {
                Reset();
                return report;
            }

            if (saved.schema_version > SchemaVersion)
            {
                report.IsFatal = true;
                report.FatalReason =
                    $"Needs component save schema_version {saved.schema_version} is newer than this build supports ({SchemaVersion}).";
                return report;
            }

            if (!string.IsNullOrEmpty(saved.system_id) &&
                !string.Equals(saved.system_id, SystemId, StringComparison.Ordinal))
            {
                report.IsFatal = true;
                report.FatalReason =
                    $"Needs component save system_id '{saved.system_id}' does not match '{SystemId}'.";
                return report;
            }

            var restored = new Dictionary<SurvivorId, SurvivorNeedsState>();
            if (saved.survivors != null)
            {
                for (int i = 0; i < saved.survivors.Count; i++)
                {
                    var row = saved.survivors[i];
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

                    restored.Add(owner, row.ToRuntimeState());
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
