using System;

namespace Ashfall.Core.Disease
{
    /// <summary>
    /// Plan 60 / D1 — the clinical stage of an infection, <em>derived</em> from the
    /// authored catalog bounds. There is deliberately no authored phase list:
    /// <see cref="DiseaseSystem.ResolveOutcomes"/> already treats
    /// <c>incubation_days</c> and <c>illness_days</c> as the only progression
    /// bindings, so a second authored timeline would be a parallel authority that
    /// nothing drives.
    /// </summary>
    public enum DiseaseClinicalStage
    {
        /// <summary>No active infection (or the definition is unknown).</summary>
        None = 0,
        /// <summary>Infected, still inside the incubation window: not yet contagious.</summary>
        Incubating = 1,
        /// <summary>Clinically ill and contagious, still inside the treatable window.</summary>
        Ill = 2,
        /// <summary>
        /// Ill, past the terminal-prognosis threshold: comfort care becomes the
        /// honest plan and triage urgency rises.
        /// </summary>
        Terminal = 3,
        /// <summary>
        /// Illness window elapsed; the engine resolves death or recovery on the
        /// next tick. Kept distinct from <see cref="Terminal"/> because the
        /// outcome is pending, not foretold.
        /// </summary>
        OutcomePending = 4,
    }

    /// <summary>
    /// Plan 60 / D2 — everything a clinical surface is allowed to say about one
    /// patient, assembled in one place from the authored catalog and the derived
    /// stage. A panel that composed its own wording from raw fields is how the ward
    /// and the sick list start disagreeing about the same person.
    /// </summary>
    public sealed class DiseaseClinicalPicture
    {
        public string DiseaseId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public DiseaseClinicalStage Stage { get; set; }
        public string StageToken { get; set; } = string.Empty;
        public string Tell { get; set; } = string.Empty;
        public string SecondaryTell { get; set; } = string.Empty;
        public string TimingClue { get; set; } = string.Empty;
        public string Guidance { get; set; } = string.Empty;
        public string Vector { get; set; } = string.Empty;
        public int DaysSick { get; set; }
        public int DaysUntilOutcome { get; set; }
        public float AuthoredLethality { get; set; }
        public float EffectiveLethality { get; set; }
        public bool HasTreatmentPath { get; set; }
        public bool HasCure { get; set; }
        public bool Terminal { get; set; }
        public int DosesGiven { get; set; }

        /// <summary>
        /// Whether the illness has been read by anybody yet. An undiagnosed patient is
        /// not a list of answers: the tell is what the player has, not what the engine
        /// knows, so surfaces say "unidentified" rather than spoiling the diagnosis.
        /// </summary>
        public bool Diagnosed { get; set; }
    }

    /// <summary>
    /// Plan 60 / D1 + D5 — the single mapping from infection state to clinical
    /// stage, sick-list band, and palliative plan. Pure, deterministic, and
    /// shared by every surface so no consumer re-derives clinical truth.
    ///
    /// Band values are <see cref="DoseLedgerSystem"/> band constants: the sick
    /// list keeps ONE urgency ladder, and <c>SickBand.severitySource</c> records
    /// which fact produced the band (dose vs illness) so the two never silently
    /// mean the same thing.
    /// </summary>
    public static class DiseaseTriage
    {
        /// <summary>
        /// Fraction of the illness window after which a high-lethality infection is
        /// a terminal prognosis rather than an acute one. 0.75 leaves a real
        /// comfort-care window before the outcome roll.
        /// </summary>
        public const float TerminalWindowFraction = 0.75f;

        /// <summary>
        /// A disease must be at least this lethal to be called terminal. A
        /// self-limiting infection should never be routed to palliative care just
        /// because it is late.
        /// </summary>
        public const float TerminalLethalityFloor = 0.25f;

        /// <summary>
        /// Lethality at or above which the terminal plan escalates from comfort
        /// rounds to the morphine tray.
        /// </summary>
        public const float HeavySedationLethalityFloor = 0.5f;

        /// <summary>
        /// Authored palliative plan ids. These are the Expansion 07 register plans
        /// in <c>dose_registers.json</c> (<c>plans[]</c>) — never invented here, and
        /// rendered by the register's own labels.
        /// </summary>
        public static class Plans
        {
            public const string ComfortRounds = "plan_comfort_rounds";
            public const string MorphineTray = "plan_morphine_tray";
        }

        /// <summary>
        /// Derive the clinical stage for <paramref name="daysSick"/> under
        /// <paramref name="def"/>. Null/empty definitions resolve to
        /// <see cref="DiseaseClinicalStage.None"/> rather than guessing, so an
        /// unauthorised disease cannot manufacture a prognosis.
        /// </summary>
        public static DiseaseClinicalStage StageOf(DiseaseDefinition def, int daysSick)
        {
            if (def == null || daysSick < 0) return DiseaseClinicalStage.None;

            int incubation = Math.Max(0, def.incubation_days);
            int illness = Math.Max(1, def.illness_days);

            if (daysSick >= illness) return DiseaseClinicalStage.OutcomePending;
            if (daysSick < incubation) return DiseaseClinicalStage.Incubating;
            return IsTerminalPrognosis(def, daysSick)
                ? DiseaseClinicalStage.Terminal
                : DiseaseClinicalStage.Ill;
        }

        /// <summary>
        /// True when the infection is late enough and lethal enough that comfort
        /// care, not curative effort, is the honest plan. Never true for a
        /// low-lethality disease, whatever the day.
        /// </summary>
        public static bool IsTerminalPrognosis(DiseaseDefinition def, int daysSick)
        {
            if (def == null || daysSick < 0) return false;
            if (def.lethality < TerminalLethalityFloor) return false;

            int illness = Math.Max(1, def.illness_days);
            int incubation = Math.Min(illness - 1, Math.Max(0, def.incubation_days));
            int window = Math.Max(1, illness - incubation);
            int terminalAt = incubation + (int)Math.Ceiling(window * TerminalWindowFraction);

            // A disease whose window rounds out to "terminal on its first ill day"
            // is not a terminal prognosis yet; keep at least one day of acute
            // treatment room so palliative routing is never the default.
            terminalAt = Math.Max(incubation + 1, terminalAt);
            return daysSick >= terminalAt;
        }

        /// <summary>
        /// Map infection state onto the shared sick-list band ladder
        /// (<see cref="DoseLedgerSystem.BandGreen"/> … <see cref="DoseLedgerSystem.BandBlack"/>).
        ///
        /// Incubating cases are deliberately <em>not</em> named into the sick list
        /// (see <see cref="ShouldNameToSickList"/>) — the list is the named sick,
        /// and incubation is a quarantine question, not a triage one.
        /// </summary>
        public static int SickBandFor(DiseaseDefinition def, int daysSick)
        {
            switch (StageOf(def, daysSick))
            {
                case DiseaseClinicalStage.Ill:
                    return DoseLedgerSystem.BandAmber;
                case DiseaseClinicalStage.Terminal:
                    return DoseLedgerSystem.BandRed;
                case DiseaseClinicalStage.OutcomePending:
                    return DoseLedgerSystem.BandBlack;
                default:
                    return DoseLedgerSystem.BandGreen;
            }
        }

        /// <summary>
        /// Whether this infection belongs on the sick list at all.
        /// </summary>
        public static bool ShouldNameToSickList(DiseaseDefinition def, int daysSick) =>
            StageOf(def, daysSick) != DiseaseClinicalStage.None
            && StageOf(def, daysSick) != DiseaseClinicalStage.Incubating;

        /// <summary>
        /// The comfort-care plan for a terminal infection, or null when no
        /// palliative plan is honest (acute, self-limiting, or incubating).
        /// Returns authored register plan ids only.
        /// </summary>
        public static string PalliativePlanFor(DiseaseDefinition def, int daysSick)
        {
            if (StageOf(def, daysSick) != DiseaseClinicalStage.Terminal) return null;
            return def.lethality >= HeavySedationLethalityFloor
                ? Plans.MorphineTray
                : Plans.ComfortRounds;
        }

        /// <summary>
        /// Stable, human-readable stage token for logs, reports and tests. Not a
        /// player-facing string — player text goes through the catalog's authored
        /// fields and the localization keys.
        /// </summary>
        public static string StageToken(DiseaseClinicalStage stage)
        {
            switch (stage)
            {
                case DiseaseClinicalStage.Incubating: return "incubating";
                case DiseaseClinicalStage.Ill: return "ill";
                case DiseaseClinicalStage.Terminal: return "terminal";
                case DiseaseClinicalStage.OutcomePending: return "outcome_pending";
                default: return "none";
            }
        }

        /// <summary>
        /// Assemble the clinical picture for one patient. <paramref name="diagnosed"/>
        /// gates the naming of the illness: the signs are always visible (that is what
        /// a medic sees), the identification is earned by diagnosing.
        /// </summary>
        public static DiseaseClinicalPicture PictureOf(
            DiseaseDefinition def, int daysSick, float effectiveLethality = float.NaN,
            int dosesGiven = 0, bool diagnosed = true)
        {
            // NaN means "nobody told me the patient's own odds", so the projection
            // falls back to the disease's authored lethality — never to zero, which
            // would read on a surface as "this illness cannot kill anyone".
            float odds = float.IsNaN(effectiveLethality) ? 0f : effectiveLethality;
            var picture = new DiseaseClinicalPicture
            {
                DaysSick = daysSick < 0 ? 0 : daysSick,
                DosesGiven = dosesGiven < 0 ? 0 : dosesGiven,
                Diagnosed = diagnosed,
                EffectiveLethality = odds,
            };
            if (def == null) return picture;

            var stage = StageOf(def, picture.DaysSick);
            picture.DiseaseId = def.id ?? string.Empty;
            picture.DisplayName = diagnosed ? (def.display_name ?? string.Empty) : string.Empty;
            picture.Vector = def.vector ?? string.Empty;
            picture.Stage = stage;
            picture.StageToken = StageToken(stage);
            picture.Tell = def.tell ?? string.Empty;
            picture.SecondaryTell = def.tell_secondary ?? string.Empty;
            picture.TimingClue = def.timing_clue ?? string.Empty;
            picture.Guidance = def.guidance ?? string.Empty;
            picture.AuthoredLethality = def.lethality;
            if (odds <= 0f) picture.EffectiveLethality = Math.Max(0f, def.lethality);
            picture.DaysUntilOutcome = Math.Max(0, def.illness_days - picture.DaysSick);
            picture.Terminal = stage == DiseaseClinicalStage.Terminal;
            if (def.treatments != null && def.treatments.Count > 0)
            {
                picture.HasTreatmentPath = true;
                for (int i = 0; i < def.treatments.Count; i++)
                {
                    var t = def.treatments[i];
                    if (t != null && DiseaseTreatmentRoles.IsCurative(t.role)) { picture.HasCure = true; break; }
                }
            }
            return picture;
        }
    }
}
