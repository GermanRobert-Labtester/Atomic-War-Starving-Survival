// SPDX-License-Identifier: MIT
// Task #132 — Canonical survivor entity store and lifecycle transactions.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>A committed lifecycle transition. Raised only after state is coherent.</summary>
    public readonly struct SurvivorLifecycleTransition
    {
        public SurvivorId Id { get; }
        public SurvivorTransition Transition { get; }
        public SurvivorLifecycleState From { get; }
        public SurvivorLifecycleState To { get; }
        public int Day { get; }
        public long Revision { get; }

        public SurvivorLifecycleTransition(
            SurvivorId id,
            SurvivorTransition transition,
            SurvivorLifecycleState from,
            SurvivorLifecycleState to,
            int day,
            long revision)
        {
            Id = id;
            Transition = transition;
            From = from;
            To = to;
            Day = day;
            Revision = revision;
        }

        public override string ToString() => $"{Id}: {From} -> {To} via {Transition} on day {Day} (rev {Revision})";
    }

    /// <summary>Serialized canonical survivor section.</summary>
    [Serializable]
    public sealed class SurvivorEntityStoreState
    {
        public int schema_version = SurvivorEntityStore.SchemaVersion;
        public string system_id = SurvivorEntityStore.SystemId;
        public List<SurvivorAggregateState> survivors = new List<SurvivorAggregateState>();
    }

    /// <summary>
    /// What <see cref="SurvivorEntityStore.RestoreState"/> made of a save.
    ///
    /// <para>A restore never silently discards a survivor. Anything dropped or
    /// corrected is reported here with a reason, so a contradictory legacy save
    /// produces an audit trail rather than a campaign that is quietly missing
    /// somebody.</para>
    /// </summary>
    public sealed class SurvivorRestoreReport
    {
        /// <summary>Aggregates successfully restored.</summary>
        public int Accepted { get; internal set; }

        /// <summary>Rows dropped, with the reason for each.</summary>
        public List<string> Rejected { get; } = new List<string>();

        /// <summary>Rows kept after a deterministic correction, with the correction described.</summary>
        public List<string> Repaired { get; } = new List<string>();

        /// <summary>True when the save could not be restored at all.</summary>
        public bool IsFatal { get; internal set; }

        /// <summary>Fatal reason, when <see cref="IsFatal"/>.</summary>
        public string FatalReason { get; internal set; } = string.Empty;

        /// <summary>True when nothing was dropped or corrected.</summary>
        public bool IsClean => !IsFatal && Rejected.Count == 0 && Repaired.Count == 0;

        public override string ToString()
            => IsFatal
                ? $"[SurvivorRestore] FATAL: {FatalReason}"
                : $"[SurvivorRestore] accepted={Accepted} rejected={Rejected.Count} repaired={Repaired.Count}";
    }

    /// <summary>
    /// The campaign's single answer to "who is a survivor, and where do they stand?"
    ///
    /// <para><b>What it owns:</b> the set of <see cref="SurvivorAggregate"/>s,
    /// uniqueness of <see cref="SurvivorId"/>, deterministic ordering, and the
    /// lifecycle transitions. That is all. It is not a general-purpose repository —
    /// it holds no needs, dose, wounds, skills, gear, duties, or relationships, and
    /// gains no ability to read them by registering a component store.</para>
    ///
    /// <para><b>Transaction discipline.</b> Every <c>Try*</c> method validates all
    /// preconditions before the first mutation. A refused transaction is therefore
    /// guaranteed to have changed nothing, and events fire only after the committed
    /// state is coherent — never partway through. Because
    /// <see cref="SurvivorAggregate"/> is immutable and transitions swap in a new
    /// instance, a half-applied aggregate is not merely avoided but
    /// unrepresentable.</para>
    ///
    /// <para><b>Determinism.</b> <see cref="Survivors"/> and <see cref="Ids"/>
    /// enumerate in <see cref="SurvivorId"/> ordinal order, never dictionary order,
    /// so the same seed and the same commands produce the same simulation
    /// regardless of join order (AGENTS.md Invariant 4).</para>
    ///
    /// <para>Engine-agnostic: no <c>UnityEngine</c>, no <c>Godot</c>, no
    /// <c>System.Random</c>, no <c>Guid.NewGuid</c>.</para>
    /// </summary>
    public sealed class SurvivorEntityStore
    {
        public const string SystemId = "survivor_entity_store";
        public const int SchemaVersion = 1;

        // Default comparer: SurvivorId implements IEquatable<SurvivorId>, so the
        // default comparer already uses ordinal semantics without boxing.
        private readonly Dictionary<SurvivorId, SurvivorAggregate> _byId =
            new Dictionary<SurvivorId, SurvivorAggregate>();

        private readonly List<SurvivorAggregate> _ordered = new List<SurvivorAggregate>();
        private bool _orderDirty;

        private readonly List<ISurvivorComponentStore> _componentStores = new List<ISurvivorComponentStore>();

        /// <summary>Raised after a survivor joins the campaign.</summary>
        public event Action<SurvivorAggregate>? OnJoined;

        /// <summary>Raised after any committed lifecycle transition, including joins and departures.</summary>
        public event Action<SurvivorLifecycleTransition>? OnLifecycleChanged;

        /// <summary>Raised after a living survivor is removed from the campaign.</summary>
        public event Action<SurvivorAggregate>? OnLeft;

        /// <summary>Raised after any change to the store. Intended for read models.</summary>
        public event Action? OnChanged;

        // ── Queries ────────────────────────────────────────────────────

        /// <summary>Number of survivors in the campaign, in any lifecycle state.</summary>
        public int Count => _byId.Count;

        /// <summary>Survivors who are alive — resident or deployed.</summary>
        public int LivingCount => CountInStates(SurvivorLifecycle.IsAlive);

        /// <summary>Survivors physically in the shelter.</summary>
        public int ResidentCount => CountInStates(SurvivorLifecycle.IsInShelter);

        /// <summary>Survivors out on expeditions.</summary>
        public int DeployedCount => CountInStates(SurvivorLifecycle.IsDeployed);

        /// <summary>Survivors who have died, memorialized or not.</summary>
        public int DeceasedCount => CountInStates(SurvivorLifecycle.IsDeceased);

        private int CountInStates(Func<SurvivorLifecycleState, bool> predicate)
        {
            int n = 0;
            foreach (var kv in _byId)
                if (predicate(kv.Value.Lifecycle)) n++;
            return n;
        }

        /// <summary>
        /// Every survivor, in <see cref="SurvivorId"/> ordinal order. The canonical
        /// simulation order. Allocation-free once ordering is settled.
        /// </summary>
        public IReadOnlyList<SurvivorAggregate> Survivors
        {
            get
            {
                EnsureOrdered();
                return _ordered;
            }
        }

        /// <summary>Every survivor id, in ordinal order.</summary>
        public IReadOnlyList<SurvivorId> Ids
        {
            get
            {
                EnsureOrdered();
                var ids = new List<SurvivorId>(_ordered.Count);
                for (int i = 0; i < _ordered.Count; i++) ids.Add(_ordered[i].Id);
                return ids;
            }
        }

        /// <summary>Registered domain component stores, in registration order.</summary>
        public IReadOnlyList<ISurvivorComponentStore> ComponentStores => _componentStores;

        public bool Contains(SurvivorId id) => !id.IsEmpty && _byId.ContainsKey(id);

        /// <summary>Resolve a survivor. Returns false rather than fabricating a default.</summary>
        public bool TryGet(SurvivorId id, out SurvivorAggregate survivor)
        {
            if (id.IsEmpty)
            {
                survivor = null!;
                return false;
            }
            return _byId.TryGetValue(id, out survivor!);
        }

        /// <summary>
        /// Resolve a survivor that must exist. Throws with the id and the campaign
        /// size rather than returning null and deferring the crash.
        /// </summary>
        /// <exception cref="KeyNotFoundException">No such survivor in this campaign.</exception>
        public SurvivorAggregate GetRequired(SurvivorId id)
        {
            if (TryGet(id, out var survivor)) return survivor;
            throw new KeyNotFoundException(
                $"Survivor '{id}' does not exist in this campaign ({_byId.Count} survivor(s) present).");
        }

        /// <summary>
        /// The compatibility boundary for callers still holding a raw string
        /// (Phase 61). Validates once here so no normalization happens at scattered
        /// call sites. An unparseable or unknown id resolves to false — it is never
        /// coerced into an identity.
        /// </summary>
        public bool TryResolve(string? rawId, out SurvivorAggregate survivor)
        {
            if (!SurvivorId.TryParse(rawId, out var id))
            {
                survivor = null!;
                return false;
            }
            return TryGet(id, out survivor);
        }

        /// <summary>Register a domain component store for referential-integrity checks.</summary>
        public void RegisterComponentStore(ISurvivorComponentStore store)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));
            for (int i = 0; i < _componentStores.Count; i++)
            {
                if (string.Equals(_componentStores[i].ComponentName, store.ComponentName, StringComparison.Ordinal))
                    throw new ArgumentException(
                        $"A survivor component store named '{store.ComponentName}' is already registered.",
                        nameof(store));
            }
            _componentStores.Add(store);
        }

        // ── Transactions ───────────────────────────────────────────────

        /// <summary>
        /// Bring a survivor into the campaign as a resident.
        ///
        /// <para>Refuses an id already present, so a double join cannot produce two
        /// aggregates for one survivor.</para>
        /// </summary>
        public SurvivorLifecycleResult TryJoin(SurvivorId id, string? definitionId, int day)
        {
            if (id.IsEmpty)
            {
                return SurvivorLifecycleResult.Blocked(
                    id, SurvivorTransition.Join, SurvivorLifecycleState.Unknown,
                    SurvivorLifecycleFailure.IdInvalid,
                    "Cannot join a survivor with an empty SurvivorId.");
            }

            if (_byId.TryGetValue(id, out var existing))
            {
                return SurvivorLifecycleResult.Blocked(
                    id, SurvivorTransition.Join, existing.Lifecycle,
                    SurvivorLifecycleFailure.AlreadyExists,
                    $"Survivor '{id}' is already in this campaign as {existing.Lifecycle}; join refused.",
                    existing.Revision);
            }

            // Commit.
            var joined = SurvivorAggregate.Joined(id, definitionId, day);
            _byId[id] = joined;
            _orderDirty = true;

            OnJoined?.Invoke(joined);
            OnLifecycleChanged?.Invoke(new SurvivorLifecycleTransition(
                id, SurvivorTransition.Join, SurvivorLifecycleState.Unknown,
                SurvivorLifecycleState.Resident, day, joined.Revision));
            OnChanged?.Invoke();

            return SurvivorLifecycleResult.Committed(
                id, SurvivorTransition.Join, SurvivorLifecycleState.Unknown,
                SurvivorLifecycleState.Resident, joined.Revision);
        }

        /// <summary>
        /// Send a resident out on an expedition.
        ///
        /// <para>Requires a non-empty expedition id: <see cref="SurvivorLifecycleState.Away"/>
        /// without one would be exactly the untraceable "somewhere else" state this
        /// task removes.</para>
        /// </summary>
        public SurvivorLifecycleResult TryDeploy(SurvivorId id, string expeditionId, int day)
        {
            if (!TryGet(id, out var survivor))
                return UnknownSurvivor(id, SurvivorTransition.Deploy);

            if (string.IsNullOrEmpty(expeditionId))
            {
                return SurvivorLifecycleResult.Blocked(
                    id, SurvivorTransition.Deploy, survivor.Lifecycle,
                    SurvivorLifecycleFailure.ExpeditionIdRequired,
                    $"Survivor '{id}' cannot be deployed without an expedition id.",
                    survivor.Revision);
            }

            if (survivor.Lifecycle == SurvivorLifecycleState.Away &&
                string.Equals(survivor.ActiveExpeditionId, expeditionId, StringComparison.Ordinal))
            {
                return SurvivorLifecycleResult.AlreadyIn(id, SurvivorTransition.Deploy, survivor.Lifecycle, survivor.Revision);
            }

            var to = SurvivorLifecycle.Destination(survivor.Lifecycle, SurvivorTransition.Deploy);
            if (!to.HasValue)
                return IllegalTransition(survivor, SurvivorTransition.Deploy);

            return Commit(survivor, SurvivorTransition.Deploy, to.Value, day, expeditionId);
        }

        /// <summary>Bring a deployed survivor home.</summary>
        public SurvivorLifecycleResult TryReturn(SurvivorId id, int day)
        {
            if (!TryGet(id, out var survivor))
                return UnknownSurvivor(id, SurvivorTransition.Return);

            if (survivor.Lifecycle == SurvivorLifecycleState.Resident)
                return SurvivorLifecycleResult.AlreadyIn(id, SurvivorTransition.Return, survivor.Lifecycle, survivor.Revision);

            var to = SurvivorLifecycle.Destination(survivor.Lifecycle, SurvivorTransition.Return);
            if (!to.HasValue)
                return IllegalTransition(survivor, SurvivorTransition.Return);

            return Commit(survivor, SurvivorTransition.Return, to.Value, day, expeditionId: string.Empty);
        }

        /// <summary>
        /// Mark a survivor dead.
        ///
        /// <para>Idempotent: a second report returns
        /// <see cref="SurvivorLifecycleResult.StatusKind.AlreadyInState"/> without
        /// mutating anything, which is what lets several death sources report the
        /// same death without re-running <see cref="SurvivorFateSystem"/>'s cascade.
        /// Death from <see cref="SurvivorLifecycleState.Away"/> clears the active
        /// expedition in the same commit, so a corpse can never remain an active
        /// expedition participant.</para>
        ///
        /// <para>The cause and day of death stay with the fate ledger. This records
        /// only the fact.</para>
        /// </summary>
        public SurvivorLifecycleResult TryDie(SurvivorId id, int day)
        {
            if (!TryGet(id, out var survivor))
                return UnknownSurvivor(id, SurvivorTransition.Die);

            if (SurvivorLifecycle.IsDeceased(survivor.Lifecycle))
                return SurvivorLifecycleResult.AlreadyIn(id, SurvivorTransition.Die, survivor.Lifecycle, survivor.Revision);

            var to = SurvivorLifecycle.Destination(survivor.Lifecycle, SurvivorTransition.Die);
            if (!to.HasValue)
                return IllegalTransition(survivor, SurvivorTransition.Die);

            // Death clears deployment: no corpse stays on the expedition roster.
            return Commit(survivor, SurvivorTransition.Die, to.Value, day, expeditionId: string.Empty);
        }

        /// <summary>
        /// Memorialize a dead survivor. Requires <see cref="SurvivorLifecycleState.Dead"/>;
        /// the living are never memorialized. Idempotent once memorialized.
        /// </summary>
        public SurvivorLifecycleResult TryMemorialize(SurvivorId id, int day)
        {
            if (!TryGet(id, out var survivor))
                return UnknownSurvivor(id, SurvivorTransition.Memorialize);

            if (survivor.Lifecycle == SurvivorLifecycleState.Memorialized)
                return SurvivorLifecycleResult.AlreadyIn(id, SurvivorTransition.Memorialize, survivor.Lifecycle, survivor.Revision);

            var to = SurvivorLifecycle.Destination(survivor.Lifecycle, SurvivorTransition.Memorialize);
            if (!to.HasValue)
                return IllegalTransition(survivor, SurvivorTransition.Memorialize);

            return Commit(survivor, SurvivorTransition.Memorialize, to.Value, day, expeditionId: string.Empty);
        }

        /// <summary>
        /// Remove a living resident from the campaign — the inverse of
        /// <see cref="TryJoin"/>.
        ///
        /// <para>Only a resident may leave. A deployed survivor must be recalled or
        /// resolved first, because dropping the aggregate while an expedition still
        /// references it would create the dangling participant this task exists to
        /// prevent. The dead do not leave; they die and are memorialized.</para>
        ///
        /// <para>Active records in registered component stores are released;
        /// stores that declare <see cref="ISurvivorComponentStore.RetainsHistoryAfterDeath"/>
        /// keep their history. Because release cannot fail by contract, the whole
        /// operation is all-or-nothing.</para>
        ///
        /// <para><b>No production caller yet.</b> It exists so the store's membership
        /// is closed under both directions and so reset and rollback paths have a
        /// sanctioned removal route. The game currently has no mechanic by which a
        /// living survivor departs the shelter.</para>
        /// </summary>
        public SurvivorLifecycleResult TryLeave(SurvivorId id, int day)
        {
            if (!TryGet(id, out var survivor))
                return UnknownSurvivor(id, SurvivorTransition.Leave);

            if (!SurvivorLifecycle.IsLegalTransition(survivor.Lifecycle, SurvivorTransition.Leave))
                return IllegalTransition(survivor, SurvivorTransition.Leave);

            // Commit: release active component records, then drop the aggregate.
            for (int i = 0; i < _componentStores.Count; i++)
            {
                var store = _componentStores[i];
                if (store.RetainsHistoryAfterDeath) continue;
                store.Release(id);
            }

            _byId.Remove(id);
            _orderDirty = true;

            OnLeft?.Invoke(survivor);
            OnLifecycleChanged?.Invoke(new SurvivorLifecycleTransition(
                id, SurvivorTransition.Leave, survivor.Lifecycle,
                SurvivorLifecycleState.Unknown, day, survivor.Revision + 1L));
            OnChanged?.Invoke();

            return SurvivorLifecycleResult.Committed(
                id, SurvivorTransition.Leave, survivor.Lifecycle,
                SurvivorLifecycleState.Unknown, survivor.Revision + 1L);
        }

        // ── Commit helpers ─────────────────────────────────────────────

        private SurvivorLifecycleResult Commit(
            SurvivorAggregate from,
            SurvivorTransition transition,
            SurvivorLifecycleState to,
            int day,
            string expeditionId)
        {
            var updated = new SurvivorAggregate(
                from.Id,
                from.DefinitionId,
                to,
                from.JoinedDay,
                lifecycleDay: day,
                revision: from.Revision + 1L,
                activeExpeditionId: to == SurvivorLifecycleState.Away ? expeditionId : string.Empty);

            _byId[from.Id] = updated;
            _orderDirty = true;

            OnLifecycleChanged?.Invoke(new SurvivorLifecycleTransition(
                from.Id, transition, from.Lifecycle, to, day, updated.Revision));
            OnChanged?.Invoke();

            return SurvivorLifecycleResult.Committed(from.Id, transition, from.Lifecycle, to, updated.Revision);
        }

        private static SurvivorLifecycleResult UnknownSurvivor(SurvivorId id, SurvivorTransition transition)
            => SurvivorLifecycleResult.Blocked(
                id, transition, SurvivorLifecycleState.Unknown,
                SurvivorLifecycleFailure.Unknown,
                $"Survivor '{id}' does not exist in this campaign; {transition} refused.");

        private static SurvivorLifecycleResult IllegalTransition(SurvivorAggregate survivor, SurvivorTransition transition)
            => SurvivorLifecycleResult.Blocked(
                survivor.Id, transition, survivor.Lifecycle,
                SurvivorLifecycleFailure.TransitionIllegal,
                SurvivorLifecycle.DescribeIllegal(survivor.Id, survivor.Lifecycle, transition),
                survivor.Revision);

        private void EnsureOrdered()
        {
            if (!_orderDirty) return;
            _ordered.Clear();
            foreach (var kv in _byId) _ordered.Add(kv.Value);
            _ordered.Sort(static (a, b) => a.Id.CompareTo(b.Id));
            _orderDirty = false;
        }

        // ── Save / Load ────────────────────────────────────────────────

        /// <summary>
        /// Snapshot the store. Rows are ordered by <see cref="SurvivorId"/> ordinal
        /// order so the bytes are stable for checksums and diffing, and the returned
        /// objects are copies — mutating them cannot reach live state.
        /// </summary>
        public SurvivorEntityStoreState CaptureState()
        {
            EnsureOrdered();
            var state = new SurvivorEntityStoreState
            {
                schema_version = SchemaVersion,
                system_id = SystemId
            };
            for (int i = 0; i < _ordered.Count; i++)
                state.survivors.Add(_ordered[i].CaptureState());
            return state;
        }

        /// <summary>
        /// Replace the store's contents from a save.
        ///
        /// <para>Validates every row and reports what it did. A row is dropped only
        /// for a reason recorded in <see cref="SurvivorRestoreReport.Rejected"/>;
        /// deterministic corrections are recorded in
        /// <see cref="SurvivorRestoreReport.Repaired"/>. A save written by a newer
        /// schema is refused outright rather than partially understood.</para>
        ///
        /// <para>Conflict rules applied here, in order:</para>
        /// <list type="number">
        /// <item><description>unparseable or empty id — reject the row</description></item>
        /// <item><description>duplicate id — first row wins, later rows rejected</description></item>
        /// <item><description>lifecycle outside the legal set — reject the row, because
        /// guessing a state is how contradictory campaigns are born</description></item>
        /// <item><description><c>Away</c> with no expedition id — repair to
        /// <c>Resident</c>; a survivor away on no expedition is unrecoverable as
        /// deployed, and stranding them is worse than bringing them home</description></item>
        /// <item><description>expedition id present but not <c>Away</c> — repair by
        /// clearing it</description></item>
        /// <item><description>revision below 1 — repair to 1</description></item>
        /// </list>
        /// </summary>
        public SurvivorRestoreReport RestoreState(SurvivorEntityStoreState? saved)
        {
            var report = new SurvivorRestoreReport();

            if (saved == null)
            {
                Reset();
                return report;
            }

            if (saved.schema_version > SchemaVersion)
            {
                report.IsFatal = true;
                report.FatalReason =
                    $"Survivor store save schema_version {saved.schema_version} is newer than this build supports ({SchemaVersion}). Refusing to load rather than guess.";
                return report;
            }

            _byId.Clear();
            _ordered.Clear();
            _orderDirty = true;

            if (saved.survivors == null)
            {
                OnChanged?.Invoke();
                return report;
            }

            for (int i = 0; i < saved.survivors.Count; i++)
            {
                var row = saved.survivors[i];
                if (row == null)
                {
                    report.Rejected.Add($"row {i}: null entry");
                    continue;
                }

                if (!SurvivorId.TryParse(row.survivor_id, out var id, out string idError))
                {
                    report.Rejected.Add($"row {i}: {idError}");
                    continue;
                }

                if (_byId.ContainsKey(id))
                {
                    report.Rejected.Add($"row {i}: duplicate survivor '{id}' — first row wins.");
                    continue;
                }

                var lifecycle = (SurvivorLifecycleState)row.lifecycle;
                if (!SurvivorLifecycle.IsLegalState(lifecycle))
                {
                    report.Rejected.Add(
                        $"row {i}: survivor '{id}' has lifecycle value {row.lifecycle}, which is not a legal state.");
                    continue;
                }

                string expedition = row.active_expedition_id ?? string.Empty;

                if (lifecycle == SurvivorLifecycleState.Away && string.IsNullOrEmpty(expedition))
                {
                    lifecycle = SurvivorLifecycleState.Resident;
                    report.Repaired.Add(
                        $"survivor '{id}': Away with no expedition id — restored as Resident.");
                }
                else if (lifecycle != SurvivorLifecycleState.Away && !string.IsNullOrEmpty(expedition))
                {
                    report.Repaired.Add(
                        $"survivor '{id}': expedition id '{expedition}' on a {lifecycle} survivor — cleared.");
                    expedition = string.Empty;
                }

                long revision = row.revision;
                if (revision < 1L)
                {
                    report.Repaired.Add($"survivor '{id}': revision {row.revision} raised to 1.");
                    revision = 1L;
                }

                _byId[id] = new SurvivorAggregate(
                    id,
                    row.definition_id,
                    lifecycle,
                    row.joined_day,
                    row.lifecycle_day,
                    revision,
                    expedition);

                report.Accepted++;
            }

            OnChanged?.Invoke();
            return report;
        }

        /// <summary>
        /// Empty the store. Used when switching save slots so one campaign's
        /// survivors can never bleed into another's.
        /// </summary>
        public void Reset()
        {
            _byId.Clear();
            _ordered.Clear();
            _orderDirty = false;
            OnChanged?.Invoke();
        }
    }
}
