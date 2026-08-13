using System.Text;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Formats Holdfast catalog fields for the Godot host. No quest logic —
    /// Core already stores briefing / min_day / target on <see cref="HoldfastQuestEntry"/>.
    /// </summary>
    public static class HoldfastBriefingView
    {
        public static string FormatQuest(HoldfastQuestEntry? quest, HoldfastCatalog? catalog)
        {
            if (quest == null)
                return "No Holdfast quest loaded.";

            string locId = quest.target_location_id ?? string.Empty;
            string locName = catalog?.GetLocation(locId)?.displayName ?? locId;
            if (string.IsNullOrEmpty(locName))
                locName = "(no target)";

            var sb = new StringBuilder();
            sb.AppendLine(string.IsNullOrEmpty(quest.display_name) ? quest.id : quest.display_name);
            sb.Append("id=").Append(quest.id);
            sb.Append("  min_day=").Append(quest.min_day);
            sb.Append("  type=").Append(quest.type ?? string.Empty);
            sb.AppendLine();
            sb.Append("target=").AppendLine(locName);
            if (!string.IsNullOrEmpty(quest.briefing))
            {
                sb.AppendLine();
                sb.AppendLine(quest.briefing);
            }

            if (quest.stages != null && quest.stages.Length > 0 && quest.stages[0] != null
                && !string.IsNullOrEmpty(quest.stages[0].text))
            {
                sb.AppendLine();
                sb.Append("stage_1: ").AppendLine(quest.stages[0].text);
            }

            return sb.ToString().TrimEnd();
        }

        public static string FormatCatalogDump(HoldfastCatalog catalog)
        {
            var sb = new StringBuilder();
            sb.Append("HoldfastCatalog locations=").Append(catalog.Locations.Count);
            sb.Append(" quests=").Append(catalog.Quests.Count);
            sb.AppendLine();
            for (int i = 0; i < catalog.Quests.Count; i++)
            {
                var q = catalog.Quests[i];
                if (q == null) continue;
                sb.AppendLine();
                sb.Append("--- ").Append(q.id).AppendLine(" ---");
                sb.AppendLine(FormatQuest(q, catalog));
            }

            return sb.ToString().TrimEnd();
        }

        public static string PreviewLine(HoldfastQuestEntry? quest)
        {
            if (quest == null) return "Quest briefing: (none)";
            string title = string.IsNullOrEmpty(quest.display_name) ? quest.id : quest.display_name;
            return $"Quest briefing: {title} (min day {quest.min_day})";
        }
    }
}
