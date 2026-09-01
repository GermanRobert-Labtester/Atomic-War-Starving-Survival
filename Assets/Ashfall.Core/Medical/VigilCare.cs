using System;
using Ashfall.Core.Flags;
using Ashfall.Core.Memorial;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Plan 60 / D6 — what a kept vigil means to the record of a death.
    ///
    /// <para>The bedside vigil is deliberately <em>real time</em> (see
    /// <see cref="VigilStateMachine"/>: a few quiet minutes with a dying person), and that
    /// intention is preserved here rather than converted into a day tick. The rule that keeps
    /// it safe for a deterministic simulation is narrow: <strong>only the boolean outcome of
    /// a vigil ever reaches the simulation.</strong> Elapsed seconds, frame rate, and how long
    /// the player actually sat are presence, not arithmetic — so a 30 fps laptop and a 144 fps
    /// one must produce identical campaigns, which is verified by test.</para>
    ///
    /// <para>The kept flag rides the consequence ledger (<see cref="IFlagLedger"/>), the
    /// authority the campaign already persists, so a vigil recorded before a save is still
    /// recorded after a load and no new save shape is introduced.</para>
    /// </summary>
    public static class VigilCare
    {
        /// <summary>snake_case flag id prefix, per the project's id rules.</summary>
        public const string FlagPrefix = "flag_vigil_kept_";

        /// <summary>Origin system recorded with the flag, for audit and support triage.</summary>
        public const string OriginSystem = "medical_vigil";

        public static string FlagFor(string survivorId) =>
            string.IsNullOrEmpty(survivorId) ? string.Empty : FlagPrefix + survivorId;

        public static void RecordKept(IFlagLedger? flags, string survivorId, int day)
        {
            string flag = FlagFor(survivorId);
            if (flags == null || flag.Length == 0) return;
            flags.Set(flag, OriginSystem, "vigil_kept", day, survivorId);
        }

        public static bool IsKept(IFlagLedger? flags, string survivorId) =>
            flags != null && flags.IsSet(FlagFor(survivorId));

        /// <summary>
        /// How the death was managed, from three facts that already exist: someone
        /// was attending (caregiver, ward bed, or the vigil itself), whether the
        /// person's final wish stands resolved, and whether a vigil was kept.
        /// <see cref="DeathQuality.Peaceful"/> requires both care and one of
        /// wish-resolved or vigil-kept — a watched death with no comfort is
        /// <see cref="DeathQuality.Rushed"/>, and an alone death is
        /// <see cref="DeathQuality.Unattended"/>.
        /// </summary>
        public static DeathQuality ResolveQuality(
            bool attended, bool wishResolved, bool vigilKept)
        {
            bool cared = attended || vigilKept;
            if (!cared) return DeathQuality.Unattended;
            if (wishResolved || vigilKept) return DeathQuality.Peaceful;
            return DeathQuality.Rushed;
        }
    }
}
