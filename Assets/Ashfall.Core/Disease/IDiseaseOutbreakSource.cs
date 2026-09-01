using System.Collections.Generic;

namespace Ashfall.Core.Disease
{
    /// <summary>
    /// Plan 09 / 9A follow-up — world-trigger arrival surface.
    /// Implementing types are host adapters for SumpFloodingSystem,
    /// ExcavationSystem, and any other system that observes a discrete
    /// "this happened, so a disease lands" event (a flood receded, a
    /// dig completed, a depot collapsed). The Core engine does not
    /// subscribe to host events directly; instead, host subscribers
    /// carry an <see cref="IDiseaseOutbreakSource"/> instance whose
    /// <see cref="AuthoredDiseaseIds"/> is the contract of which
    /// diseases it is allowed to seed.
    ///
    /// Engine-agnostic. Implementations live in the host, not in Core.
    /// </summary>
    public interface IDiseaseOutbreakSource
    {
        /// <summary>
        /// Stable identifier for the source system. Convention: matches
        /// <c>SystemId</c> on the producing Core system (e.g. "sump_flooding",
        /// "excavation"). Used by the disease engine to attribute the
        /// resulting infections via the source-tag on the trigger event.
        /// </summary>
        string SourceId { get; }

        /// <summary>
        /// Disease ids this source is contracted to seed. Outside this list,
        /// <see cref="DiseaseSystem.TriggerOutbreak"/> rejects the call so
        /// a flood adapter cannot accidentally seed a spore outbreak.
        /// </summary>
        IReadOnlyList<string> AuthoredDiseaseIds { get; }
    }

    /// <summary>
    /// Result envelope returned by <see cref="DiseaseSystem.TriggerOutbreak"/>.
    /// Engine-agnostic; sent up to the host adapter so it can route
    /// follow-up effects (broadcast, narrative) without re-querying the
    /// disease system.
    /// </summary>
    public sealed class DiseaseOutbreakResult
    {
        /// <summary>Diseases ever seeded through this author's contract.</summary>
        public int InfectionsApplied { get; }

        /// <summary>Diseases rejected because they were not in the source's contract.</summary>
        public int RejectedByContract { get; }

        /// <summary>Diseases rejected because the catalog did not know the id.</summary>
        public int UnknownDisease { get; }

        /// <summary>Diseases rejected because the candidate list was empty.</summary>
        public int NoCandidates { get; }

        public DiseaseOutbreakResult(
            int infectionsApplied,
            int rejectedByContract,
            int unknownDisease,
            int noCandidates)
        {
            InfectionsApplied = infectionsApplied;
            RejectedByContract = rejectedByContract;
            UnknownDisease = unknownDisease;
            NoCandidates = noCandidates;
        }

        public static DiseaseOutbreakResult Empty => new DiseaseOutbreakResult(0, 0, 0, 0);

        public static DiseaseOutbreakResult Merge(
            DiseaseOutbreakResult a, DiseaseOutbreakResult b)
            => new DiseaseOutbreakResult(
                a.InfectionsApplied + b.InfectionsApplied,
                a.RejectedByContract + b.RejectedByContract,
                a.UnknownDisease + b.UnknownDisease,
                a.NoCandidates + b.NoCandidates);
    }
}
