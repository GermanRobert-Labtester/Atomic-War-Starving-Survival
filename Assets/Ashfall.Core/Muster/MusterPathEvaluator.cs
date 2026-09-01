using System;
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>The three political contexts a Muster gathering can convene under.
    /// Stored on MusterState.musterPath; empty string = never evaluated.</summary>
    public static class MusterPaths
    {
        public const string Negotiated = "negotiated";
        public const string Victors = "victors";
        public const string Unsettled = "unsettled";
    }

    /// <summary>
    /// Plain snapshot of the political world at evaluation time. Pure data — the
    /// HOST maps live systems (FactionWarSystem, RegionalTreatySystem, flag ledger,
    /// CoalitionCampSystem) into this struct; the evaluator stays engine-agnostic
    /// and owns no reference to any war/treaty type (Muster must not couple to
    /// YearOfAsh namespaces).
    /// </summary>
    public class MusterPathInput
    {
        /// <summary>FactionWarSystem.dominantFactionId (empty when no dominance).</summary>
        public string DominantFactionId = string.Empty;

        /// <summary>FactionWarSystem.WarTension (0..100).</summary>
        public int WarTension;

        /// <summary>War factions at standing ≤ −50 toward the player or each other.</summary>
        public int HostileFactionCount;

        /// <summary>War factions at standing ≥ +50.</summary>
        public int AlliedFactionCount;

        /// <summary>Major factions still operational (guild/hydro/raiders active,
        /// camp formed, or war factions not fragmented).</summary>
        public int SurvivingMajorFactions;

        public int ActiveTreatyCount;
        public int ViolatedTreatyCount;

        /// <summary>Any unresolved flag_grievance_* is present.</summary>
        public bool GrievanceUnresolved;

        /// <summary>Any flag_peace_* pressure flag is present (E-R weariness events).</summary>
        public bool PeacePressure;

        public bool CampFormed;
        public int CampMembers;
    }

    /// <summary>
    /// Plan 25 · 25B.14/25C.15-16 — derives the political character of the Muster
    /// gathering from state other systems already own. Pure, deterministic,
    /// idempotent: the same input always yields the same path, and re-evaluation
    /// after war-state changes is safe. It NEVER mutates war/treaty state.
    ///
    /// Rules (first match wins):
    ///   victors    — one faction is dominant AND (broad hostility ≥ 2 factions
    ///                OR war tension ≥ 60): the gathering convenes under asymmetric
    ///                power.
    ///   negotiated — no dominance, ≥ 2 major factions survive, at least one
    ///                working channel (active treaty or peace pressure), and the
    ///                coalition camp is available as neutral ground.
    ///   unsettled  — everything else: the gathering happens (Muster opening is
    ///                day-canon) but its political character stays unresolved;
    ///                scenes/testimony treat it as tense neutrality.
    /// </summary>
    public static class MusterPathEvaluator
    {
        public const int DominanceTensionThreshold = 60;

        public static string Evaluate(MusterPathInput input)
        {
            if (input == null) return MusterPaths.Unsettled;

            bool dominant = !string.IsNullOrEmpty(input.DominantFactionId);
            if (dominant && (input.HostileFactionCount >= 2 || input.WarTension >= DominanceTensionThreshold))
                return MusterPaths.Victors;

            if (input.SurvivingMajorFactions >= 2
                && input.CampFormed
                && (input.ActiveTreatyCount >= 1 || input.PeacePressure))
                return MusterPaths.Negotiated;

            return MusterPaths.Unsettled;
        }
    }
}
