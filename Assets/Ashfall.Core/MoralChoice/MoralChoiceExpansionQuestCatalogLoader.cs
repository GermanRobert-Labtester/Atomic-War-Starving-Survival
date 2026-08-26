using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>
    /// Wire shape for moral_choice_quests_expansion.json — additional standalone
    /// moral quests that expand the base catalog. Same quest schema.
    /// </summary>
    [Serializable]
    public sealed class MoralChoiceExpansionQuestContainer
    {
        public int schema_version = 1;
        public List<MoralChoiceQuestRecord> quests = new List<MoralChoiceQuestRecord>();
    }

    /// <summary>
    /// Loads the expansion moral quests from moral_choice_quests_expansion.json.
    /// Returns definitions that merge into the base catalog.
    /// </summary>
    public static class MoralChoiceExpansionQuestCatalogLoader
    {
        public const string DefaultFileName = "moral_choice_quests_expansion.json";

        public static List<MoralChoiceQuestDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<MoralChoiceQuestDefinition>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<MoralChoiceQuestDefinition>();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<MoralChoiceQuestDefinition>();

            var container = json.Deserialize<MoralChoiceExpansionQuestContainer>(raw);
            if (container?.quests == null)
                return new List<MoralChoiceQuestDefinition>();

            return container.quests.Where(r => r != null).Select(MapRecord).ToList();
        }

        private static MoralChoiceQuestDefinition MapRecord(MoralChoiceQuestRecord r) => new MoralChoiceQuestDefinition
        {
            Id = r.id,
            DisplayName = r.display_name,
            Category = r.category,
            Trigger = r.trigger,
            Discovery = r.discovery,
            LocationId = r.location_id,
            MinDay = r.min_day,
            MaxDay = r.max_day,
            Choices = r.choices?.Select(MapOption).ToList() ?? new List<MoralChoiceOption>()
        };

        private static MoralChoiceOption MapOption(MoralChoiceOptionRecord o) => new MoralChoiceOption
        {
            Label = o.label,
            MoralDelta = o.moral_delta,
            EmpathyDelta = o.empathy_delta,
            OutcomeText = o.outcome_text,
            Epitaph = o.epitaph
        };
    }
}
