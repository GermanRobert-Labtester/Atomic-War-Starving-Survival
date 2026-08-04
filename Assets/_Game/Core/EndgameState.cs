using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Campaign terminal outcome. Ongoing until win/loss fires.
    /// </summary>
    public enum EndgameState
    {
        Ongoing,
        /// <summary>Loss — hunger / collapse dominant (includes breakdown death screen).</summary>
        Starved,
        /// <summary>Loss — radiation dominant.</summary>
        Irradiated,
        /// <summary>Win — radio extraction / chopper (all living).</summary>
        Rescued,
        /// <summary>Win — vehicle drive-out.</summary>
        Escaped,
        /// <summary>Win (bittersweet) — Lifeboat Transmission: one extracted, rest left behind.</summary>
        Lifeboat
    }

    /// <summary>
    /// Death-screen flavor when all survivors are gone (Rads / Hunger / Breakdowns).
    /// Independent of <see cref="EndgameState"/> so Breakdowns can still map to Starved.
    /// </summary>
    public enum DeathScreenKind
    {
        None,
        Hunger,
        Radiation,
        Breakdowns,
        Mixed
    }

    /// <summary>
    /// Frozen post-game statistics for the endgame summary screen.
    /// Built from live systems or from a <see cref="SaveData"/> snapshot.
    /// </summary>
    [Serializable]
    public class EndgameSummaryData
    {
        public EndgameState State = EndgameState.Ongoing;
        public DeathScreenKind DeathScreen = DeathScreenKind.None;
        public string OutcomeTitle = string.Empty;
        public string OutcomeBody = string.Empty;
        public string Reason = string.Empty;

        public int DaysSurvived;
        public float TotalRadiationAbsorbed;
        public int MoralChoicesMade;
        public int MilitaryIntelDecrypted;
        public bool ExtractionUnlocked;
        public bool VehicleEscapeUsed;

        public int LivingCount;
        public int DeadCount;
        public string PrimaryAuthorName = string.Empty;

        public bool IsTerminal => State != EndgameState.Ongoing;

        public string StatusLine
        {
            get
            {
                if (!IsTerminal) return "ENDGAME: ongoing";
                return $"ENDGAME: {State}  Day {DaysSurvived}  RAD {TotalRadiationAbsorbed:0}  choices {MoralChoicesMade}";
            }
        }
    }
}
