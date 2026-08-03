using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Post-game statistics screen. Frozen campaign outcome + tallies
    /// (days, radiation, moral choices). No modal tutorial language.
    /// </summary>
    public class EndgameSummaryUI : MonoBehaviour
    {
        public bool IsVisible { get; private set; }
        public bool IsLoaded { get; private set; }

        public string OutcomeTitle { get; private set; } = string.Empty;
        public string OutcomeBody { get; private set; } = string.Empty;
        public string DeathScreenLabel { get; private set; } = string.Empty;
        public int DaysSurvived { get; private set; }
        public float TotalRadiationAbsorbed { get; private set; }
        public int MoralChoicesMade { get; private set; }
        public int MilitaryIntelDecrypted { get; private set; }
        public bool ExtractionUnlocked { get; private set; }
        public bool VehicleEscapeUsed { get; private set; }
        public string StateName { get; private set; } = "Ongoing";
        public string StatusLine { get; private set; } = "ENDGAME: —";
        public string DetailSummary { get; private set; } = string.Empty;

        /// <summary>
        /// Load and show a post-game summary. Call when campaign ends or after load of a terminal save.
        /// </summary>
        public void Show(
            string stateName,
            string outcomeTitle,
            string outcomeBody,
            string deathScreenLabel,
            int daysSurvived,
            float totalRadiationAbsorbed,
            int moralChoicesMade,
            int militaryIntelDecrypted = 0,
            bool extractionUnlocked = false,
            bool vehicleEscapeUsed = false)
        {
            StateName = stateName ?? "Ongoing";
            OutcomeTitle = outcomeTitle ?? string.Empty;
            OutcomeBody = outcomeBody ?? string.Empty;
            DeathScreenLabel = deathScreenLabel ?? string.Empty;
            DaysSurvived = daysSurvived > 0 ? daysSurvived : 1;
            TotalRadiationAbsorbed = Mathf.Max(0f, totalRadiationAbsorbed);
            MoralChoicesMade = Mathf.Max(0, moralChoicesMade);
            MilitaryIntelDecrypted = Mathf.Max(0, militaryIntelDecrypted);
            ExtractionUnlocked = extractionUnlocked;
            VehicleEscapeUsed = vehicleEscapeUsed;
            IsLoaded = true;
            IsVisible = true;
            Refresh();
        }

        public void Hide()
        {
            IsVisible = false;
            Refresh();
        }

        public void Clear()
        {
            IsVisible = false;
            IsLoaded = false;
            OutcomeTitle = string.Empty;
            OutcomeBody = string.Empty;
            DeathScreenLabel = string.Empty;
            DaysSurvived = 0;
            TotalRadiationAbsorbed = 0f;
            MoralChoicesMade = 0;
            MilitaryIntelDecrypted = 0;
            ExtractionUnlocked = false;
            VehicleEscapeUsed = false;
            StateName = "Ongoing";
            Refresh();
        }

        public void Refresh()
        {
            if (!IsLoaded)
            {
                StatusLine = "ENDGAME: —";
                DetailSummary = string.Empty;
                return;
            }

            StatusLine = IsVisible
                ? $"ENDGAME [{StateName}]  Day {DaysSurvived}  RAD {TotalRadiationAbsorbed:0}  choices {MoralChoicesMade}"
                : $"ENDGAME [{StateName}] (hidden)";

            var sb = new StringBuilder();
            sb.AppendLine("=== ENDGAME SUMMARY ===");
            sb.AppendLine(OutcomeTitle);
            if (!string.IsNullOrEmpty(DeathScreenLabel))
                sb.AppendLine($"Death screen: {DeathScreenLabel}");
            sb.AppendLine(OutcomeBody);
            sb.AppendLine("---");
            sb.AppendLine($"Days survived: {DaysSurvived}");
            sb.AppendLine($"Total radiation absorbed: {TotalRadiationAbsorbed:0.0}");
            sb.AppendLine($"Moral choices made: {MoralChoicesMade}");
            sb.AppendLine($"Military intel decrypted: {MilitaryIntelDecrypted}");
            if (ExtractionUnlocked)
                sb.AppendLine("Extraction coordinates: known");
            if (VehicleEscapeUsed)
                sb.AppendLine("Vehicle project: completed");
            DetailSummary = sb.ToString().TrimEnd();
        }
    }
}
