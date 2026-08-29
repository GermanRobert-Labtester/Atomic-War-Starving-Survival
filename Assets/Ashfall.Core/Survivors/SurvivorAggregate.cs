// SPDX-License-Identifier: MIT
// Task #132 — Minimal survivor aggregate root.
using System;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// The canonical campaign record for one survivor: who they are and where they
    /// stand in the campaign. Nothing else.
    ///
    /// <para><b>Deliberately small.</b> This type answers exactly one question —
    /// "does this survivor exist in this campaign, and what is their lifecycle
    /// status?" It does not hold hunger, dose, wounds, skills, gear, duties,
    /// relationships, or quest state. Those are domain-owned components keyed by
    /// <see cref="Id"/>. Centralising them here would move the coupling rather
    /// than remove it, and produce the god object Task #132 exists to avoid.</para>
    ///
    /// <para><b>Immutable.</b> A transition produces a new instance rather than
    /// mutating this one, so a caller holding a reference holds a coherent
    /// snapshot. That is what makes snapshot isolation a property of the type
    /// instead of a convention: no reader can observe a half-applied transition,
    /// because a partially updated aggregate is unrepresentable.</para>
    ///
    /// <para><b>Field justification</b> — each field is here only because it must
    /// stay globally coherent across domains:</para>
    /// <list type="bullet">
    /// <item><description><see cref="Id"/> — the identity itself.</description></item>
    /// <item><description><see cref="DefinitionId"/> — the link to immutable authored
    /// content. Campaign state must never live in the definition.</description></item>
    /// <item><description><see cref="Lifecycle"/> — the single answer to "alive?",
    /// previously decided independently by five systems.</description></item>
    /// <item><description><see cref="JoinedDay"/> — needed by the memorial and eulogy
    /// paths, which currently read it back off the roster entry.</description></item>
    /// <item><description><see cref="LifecycleDay"/> — when the current state began;
    /// without it, "away since when" has no owner.</description></item>
    /// <item><description><see cref="Revision"/> — lets a caller detect that the
    /// survivor moved underneath a stale read.</description></item>
    /// <item><description><see cref="ActiveExpeditionId"/> — the only domain-adjacent
    /// field, and it earns its place: <see cref="SurvivorLifecycleState.Away"/> is
    /// meaningless without knowing which expedition, and the invariant "Away if and
    /// only if exactly one active expedition" can only be enforced atomically if
    /// both halves commit together.</description></item>
    /// </list>
    ///
    /// <para>Notably absent: death cause and reason. <see cref="SurvivorFateSystem"/>
    /// already owns those, and duplicating them here would recreate the divergence
    /// this task removes.</para>
    /// </summary>
    public sealed class SurvivorAggregate
    {
        /// <summary>Canonical identity.</summary>
        public SurvivorId Id { get; }

        /// <summary>
        /// The authored <see cref="SurvivorDefinition"/> this survivor instantiates.
        ///
        /// <para>Today this always equals <c>Id.Value</c>: <c>SurvivorRosterSystem.Join</c>
        /// uses the definition id as the survivor id and refuses a second join for the
        /// same definition, so definitions and campaign survivors are 1:1. It is kept
        /// as a separate field anyway, so that introducing generated or duplicate
        /// survivors later is a data change rather than an identity migration.</para>
        /// </summary>
        public string DefinitionId { get; }

        /// <summary>Current lifecycle state. Never <see cref="SurvivorLifecycleState.Unknown"/> for a stored aggregate.</summary>
        public SurvivorLifecycleState Lifecycle { get; }

        /// <summary>Campaign day the survivor joined.</summary>
        public int JoinedDay { get; }

        /// <summary>Campaign day <see cref="Lifecycle"/> was last changed.</summary>
        public int LifecycleDay { get; }

        /// <summary>
        /// Monotonic per-survivor lifecycle revision, starting at 1 on join and
        /// incrementing on every committed transition.
        /// </summary>
        public long Revision { get; }

        /// <summary>
        /// The expedition this survivor is out on, or empty when not deployed.
        /// Non-empty if and only if <see cref="Lifecycle"/> is
        /// <see cref="SurvivorLifecycleState.Away"/> — enforced by
        /// <see cref="SurvivorEntityStore"/> and checked by
        /// <see cref="SurvivorIntegrityValidator"/>.
        /// </summary>
        public string ActiveExpeditionId { get; }

        /// <summary>
        /// Construct an aggregate directly. Used by <see cref="SurvivorEntityStore"/>
        /// when committing a transition and when restoring a save. Gameplay code
        /// should go through the store's transactions instead, so that lifecycle
        /// rules and referential integrity are applied.
        /// </summary>
        public SurvivorAggregate(
            SurvivorId id,
            string definitionId,
            SurvivorLifecycleState lifecycle,
            int joinedDay,
            int lifecycleDay,
            long revision,
            string? activeExpeditionId = null)
        {
            if (id.IsEmpty)
                throw new ArgumentException("SurvivorAggregate requires a non-empty SurvivorId.", nameof(id));

            Id = id;
            DefinitionId = string.IsNullOrEmpty(definitionId) ? id.Value : definitionId;
            Lifecycle = lifecycle;
            JoinedDay = joinedDay;
            LifecycleDay = lifecycleDay;
            Revision = revision;
            ActiveExpeditionId = activeExpeditionId ?? string.Empty;
        }

        /// <summary>A newly joined resident at revision 1.</summary>
        public static SurvivorAggregate Joined(SurvivorId id, string? definitionId, int day)
            => new SurvivorAggregate(
                id,
                string.IsNullOrEmpty(definitionId) ? id.Value : definitionId!,
                SurvivorLifecycleState.Resident,
                joinedDay: day,
                lifecycleDay: day,
                revision: 1L,
                activeExpeditionId: string.Empty);

        /// <summary>Convenience mirror of <see cref="SurvivorLifecycle.IsAlive(SurvivorLifecycleState)"/>.</summary>
        public bool IsAlive => SurvivorLifecycle.IsAlive(Lifecycle);

        /// <summary>Convenience mirror of <see cref="SurvivorLifecycle.IsDeceased(SurvivorLifecycleState)"/>.</summary>
        public bool IsDeceased => SurvivorLifecycle.IsDeceased(Lifecycle);

        /// <summary>True when this survivor is out on an expedition.</summary>
        public bool IsDeployed => SurvivorLifecycle.IsDeployed(Lifecycle);

        /// <summary>Serialize to the save row.</summary>
        public SurvivorAggregateState CaptureState() => new SurvivorAggregateState
        {
            survivor_id = Id.Value,
            definition_id = DefinitionId,
            lifecycle = (int)Lifecycle,
            joined_day = JoinedDay,
            lifecycle_day = LifecycleDay,
            revision = Revision,
            active_expedition_id = ActiveExpeditionId
        };

        public override string ToString()
            => IsDeployed
                ? $"{Id} ({Lifecycle} on {ActiveExpeditionId}, day {LifecycleDay}, rev {Revision})"
                : $"{Id} ({Lifecycle}, day {LifecycleDay}, rev {Revision})";
    }

    /// <summary>
    /// Serialized survivor aggregate row.
    ///
    /// <para>Property names are snake_case per AGENTS.md Invariant 6. This is a new
    /// save section, so it starts in the target convention rather than inheriting
    /// the camelCase drift of the older survivor slices. The lifecycle is persisted
    /// as an <see cref="int"/> so a value written by a newer build round-trips
    /// through an older one as <see cref="SurvivorLifecycleState.Unknown"/> and is
    /// rejected loudly, instead of being coerced to a plausible-looking state.</para>
    /// </summary>
    [Serializable]
    public sealed class SurvivorAggregateState
    {
        public string survivor_id = string.Empty;
        public string definition_id = string.Empty;
        public int lifecycle = (int)SurvivorLifecycleState.Resident;
        public int joined_day;
        public int lifecycle_day;
        public long revision = 1L;
        public string active_expedition_id = string.Empty;

        public SurvivorAggregateState Clone() => new SurvivorAggregateState
        {
            survivor_id = survivor_id,
            definition_id = definition_id,
            lifecycle = lifecycle,
            joined_day = joined_day,
            lifecycle_day = lifecycle_day,
            revision = revision,
            active_expedition_id = active_expedition_id
        };
    }
}
