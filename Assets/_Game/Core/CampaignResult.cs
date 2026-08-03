using System;

namespace AtomicWar._Game.Core
{
    public enum GameModeKind
    {
        Story,
        Custom,
        Expert
    }

    /// <summary>
    /// System tracking campaign day count, victory/defeat status, mode configuration,
    /// start calendar date, and terminal cause (Prompt #41).
    /// </summary>
    [Serializable]
    public class CampaignResult
    {
        public const string DefaultStartMonthName = "August";
        public const int DefaultStartDayOfMonth = 25; // August last week

        public GameModeKind Mode = GameModeKind.Story;
        public int TargetDurationDays = 120;
        public int DaysSurvived;
        public bool IsVictory;
        public bool IsDefeat;
        public EndgameConditionKind ConditionKind = EndgameConditionKind.None;
        public string OutcomeSummary = string.Empty;
        public DateTime StartCalendarDate = new DateTime(2026, 8, 25);

        public bool IsTerminal => IsVictory || IsDefeat;

        public static int GetDefaultDurationForMode(GameModeKind mode, int customDays = 120)
        {
            return mode switch
            {
                GameModeKind.Story => 120,
                GameModeKind.Expert => 180,
                GameModeKind.Custom => ClampCustomDays(customDays),
                _ => 120
            };
        }

        public static int ClampCustomDays(int days)
        {
            if (days <= 60) return 60;
            if (days <= 100) return 100;
            if (days <= 120) return 120;
            return 180;
        }
    }
}
