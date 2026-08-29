// SPDX-License-Identifier: MIT
// Task #132 — Domain component contract for referential integrity.
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>How many component records one survivor may own.</summary>
    public enum SurvivorComponentCardinality
    {
        /// <summary>
        /// At most one record per survivor, and its absence is legal. Needs and
        /// radiation are like this while a survivor is being composed.
        /// </summary>
        ZeroOrOne = 0,

        /// <summary>
        /// Exactly one record for every survivor whose lifecycle makes the
        /// component applicable. A missing record is an integrity error.
        /// </summary>
        OnePerEligible = 1,

        /// <summary>
        /// Many records per survivor, keyed further by something domain-specific.
        /// Trauma bonds, afflictions and admissions are like this.
        /// </summary>
        Many = 2
    }

    /// <summary>
    /// The narrow view <see cref="SurvivorEntityStore"/> and
    /// <see cref="SurvivorIntegrityValidator"/> need of a domain's per-survivor
    /// state. Deliberately tiny.
    ///
    /// <para><b>Why this shape.</b> Referential integrity — "no component may exist
    /// for a survivor who does not" — has to be checkable centrally, but the data
    /// itself must stay with the domain that owns its rules. This interface exposes
    /// only ownership and release, so the canonical store learns which survivors a
    /// domain has records for without gaining any ability to read or edit needs,
    /// dose, wounds, or skills. The canonical store never becomes a generic
    /// repository, and no domain has to hand over its state to be validated.</para>
    ///
    /// <para>A domain implementing this does <b>not</b> surrender authority over its
    /// own values; it only agrees that a survivor it has never heard of is not a
    /// survivor it may invent.</para>
    /// </summary>
    public interface ISurvivorComponentStore
    {
        /// <summary>
        /// Stable snake_case component name for diagnostics, e.g. <c>needs</c>,
        /// <c>radiation</c>, <c>medical</c>. Appears verbatim in integrity findings.
        /// </summary>
        string ComponentName { get; }

        /// <summary>How many records one survivor may own.</summary>
        SurvivorComponentCardinality Cardinality { get; }

        /// <summary>
        /// True when records may legitimately outlive their survivor's death.
        ///
        /// <para>Memorials, trauma bonds, journal authorship and cause-of-death
        /// records are history and must survive; needs decay and duty assignments
        /// are active state and must not. The distinction is per domain, so each
        /// domain declares it rather than having a blanket rule imposed.</para>
        /// </summary>
        bool RetainsHistoryAfterDeath { get; }

        /// <summary>
        /// Every survivor this store holds a record for, in deterministic order.
        /// Implementations must not expose raw dictionary iteration; sort by
        /// <see cref="SurvivorId"/> ordinal order.
        /// </summary>
        IEnumerable<SurvivorId> OwnerIds { get; }

        /// <summary>True when this store holds at least one record for <paramref name="owner"/>.</summary>
        bool Contains(SurvivorId owner);

        /// <summary>
        /// Drop every active record for <paramref name="owner"/>; returns whether
        /// anything was removed.
        ///
        /// <para><b>Contract: must not throw and must not fail.</b> Releasing an
        /// owner that is absent is a no-op returning false. This is what lets
        /// <see cref="SurvivorEntityStore.TryLeave"/> stay atomic — it validates the
        /// transition first, and release can then never leave the campaign
        /// half-dismantled.</para>
        ///
        /// <para>Stores with <see cref="RetainsHistoryAfterDeath"/> set are asked to
        /// release only their active records and may keep their history.</para>
        /// </summary>
        bool Release(SurvivorId owner);
    }
}
