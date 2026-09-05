using System;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// F17 — micro-location hazard integration contract (flagship plan §5/§6).
    ///
    /// Pure, engine-agnostic coordinator that routes an authored micro-location
    /// hazard world flag into the subsystem that already owns the consequence.
    /// The flag is the authored content channel (<c>setWorldFlag</c> on the
    /// encounter choice); this registry is the integration policy that maps it
    /// onto the canonical disease authority. The disease system itself stays
    /// ignorant of encounter IDs — it only ever sees
    /// <c>Infect(survivorId, diseaseId, day)</c>.
    ///
    /// Authoritative mapping (not invented): the disease catalog's own
    /// <c>source_note</c> for <see cref="DeadLivestockDiseaseId"/> reads
    /// "Carried in from scavenged bedding and dead livestock" — the data
    /// authority already binds dead-livestock scavenging to zoonotic flu.
    ///
    /// Idempotence (flagship plan §5.3/§14.4): the hazard fires only on the
    /// flag's unset→set transition (a re-processed persistent flag reports
    /// AlreadyKnown and never re-infects), and the canonical
    /// <c>DiseaseSystem.Infect</c> is itself a no-op for an already-infected
    /// survivor. Two independent exactly-once gates guard the replay exploit.
    /// </summary>
    public static class MicroLocationHazardRegistry
    {
        /// <summary>Authored flag set by <c>micro_dead_livestock / scavenge_livestock</c>.</summary>
        public const string ContaminationExposureFlag = "micro_contamination_exposure";

        /// <summary>Canonical biological-contamination consequence for
        /// dead-livestock scavenging. Sourced from disease_catalog.json —
        /// never a parallel contamination counter.</summary>
        public const string DeadLivestockDiseaseId = "disease_zoonotic_flu";

        /// <summary>Outcome of one hazard-application attempt.</summary>
        public enum HazardStatus
        {
            /// <summary>No flag on the resolution, or the flag has no registered hazard.</summary>
            NotApplicable,
            /// <summary>The flag was already set (replay/revisit) — hazard deliberately not reapplied.</summary>
            AlreadyKnown,
            /// <summary>No survivor id could be resolved (no active expedition at the site).</summary>
            SkippedNoSurvivor,
            /// <summary>No disease authority delegate was wired (headless degrade).</summary>
            SkippedNoAuthority,
            /// <summary>The canonical authority received the consequence exactly once.</summary>
            Applied
        }

        /// <summary>Result payload of one hazard-application attempt (observability for UI/tests).</summary>
        public readonly struct HazardApplicationResult
        {
            public readonly HazardStatus Status;
            public readonly string FlagId;
            public readonly string DiseaseId;
            public readonly string SurvivorId;

            public HazardApplicationResult(HazardStatus status, string flagId, string diseaseId, string survivorId)
            {
                Status = status;
                FlagId = flagId ?? string.Empty;
                DiseaseId = diseaseId ?? string.Empty;
                SurvivorId = survivorId ?? string.Empty;
            }

            public bool IsApplied => Status == HazardStatus.Applied;
            /// <summary>Applied or already-consumed — both are healthy outcomes.</summary>
            public bool IsSuccess => Status == HazardStatus.Applied || Status == HazardStatus.AlreadyKnown;
        }

        /// <summary>
        /// Canonical hazard consequence for a hazard world flag, or null when
        /// the flag carries no registered hazard (ordinary world flags pass
        /// through untouched). One lookup point — audit before extending.
        /// </summary>
        public static string? TryGetFlagDiseaseId(string? flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return null;
            if (flagId == ContaminationExposureFlag) return DeadLivestockDiseaseId;
            return null;
        }

        /// <summary>
        /// Route a freshly-set hazard flag into the owning disease authority.
        /// Called by the host consequence applier AFTER the flag ledger commits:
        /// <paramref name="flagWasAlreadySet"/> is the ledger's own verdict, so
        /// a persistent flag can never re-trigger the consequence on revisit,
        /// save/reload, or event replay (flagship plan §14.4).
        /// The <paramref name="infectDisease"/> delegate is the host's wire into
        /// the canonical <c>DiseaseSystem.Infect</c> — this method never
        /// touches contamination state itself.
        /// </summary>
        public static HazardApplicationResult ApplyFlagHazard(
            string? flagId,
            bool flagWasAlreadySet,
            string? survivorId,
            int day,
            Action<string, string, int>? infectDisease)
        {
            string flag = flagId ?? string.Empty;
            string? diseaseId = TryGetFlagDiseaseId(flag);
            if (diseaseId == null)
                return new HazardApplicationResult(HazardStatus.NotApplicable, flag, string.Empty, survivorId ?? string.Empty);

            if (flagWasAlreadySet)
                return new HazardApplicationResult(HazardStatus.AlreadyKnown, flag, diseaseId, survivorId ?? string.Empty);

            if (string.IsNullOrEmpty(survivorId))
                return new HazardApplicationResult(HazardStatus.SkippedNoSurvivor, flag, diseaseId, string.Empty);

            if (infectDisease == null)
                return new HazardApplicationResult(HazardStatus.SkippedNoAuthority, flag, diseaseId, survivorId);

            infectDisease(survivorId, diseaseId, day);
            return new HazardApplicationResult(HazardStatus.Applied, flag, diseaseId, survivorId);
        }
    }
}
