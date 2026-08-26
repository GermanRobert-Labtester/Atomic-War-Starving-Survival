using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>Wire shape for moral_choice_quests.json (the authority).</summary>
    [Serializable]
    public sealed class MoralChoiceQuestCatalogContainer
    {
        public int schema_version = 1;
        public List<MoralChoiceQuestRecord> quests = new List<MoralChoiceQuestRecord>();
    }

    [Serializable]
    public sealed class MoralChoiceQuestRecord
    {
        public string id = string.Empty;
        public string display_name = string.Empty;

        /// <summary>share | listen | comfort | dead | trust</summary>
        public string category = string.Empty;

        public string trigger = string.Empty;
        public string discovery = string.Empty;
        public string location_id = string.Empty;
        public int min_day;
        public int max_day;
        public List<MoralChoiceOptionRecord> choices = new List<MoralChoiceOptionRecord>();
    }

    [Serializable]
    public sealed class MoralChoiceOptionRecord
    {
        public string label = string.Empty;
        public int moral_delta;
        public int empathy_delta;
        public string outcome_text = string.Empty;
        public string epitaph = string.Empty;
    }

    /// <summary>
    /// Loads the 60 moral choice quests from JSON. Engine-agnostic:
    /// IFileIO + IJsonSerializer ports, same pattern as the other catalog
    /// loaders. Gossip propagation delays are owned by MoralChoiceSystem
    /// (seeded at resolution time), not by this data.
    /// </summary>
    public static class MoralChoiceCatalogLoader
    {
        public const string DefaultFileName = "moral_choice_quests.json";

        public static List<MoralChoiceQuestDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<MoralChoiceQuestDefinition>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<MoralChoiceQuestDefinition>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<MoralChoiceQuestDefinition>();

            var container = json.Deserialize<MoralChoiceQuestCatalogContainer>(rawText);
            if (container?.quests == null)
                return new List<MoralChoiceQuestDefinition>();

            var definitions = new List<MoralChoiceQuestDefinition>(container.quests.Count);
            foreach (var record in container.quests)
            {
                if (record == null) continue;
                definitions.Add(Map(record));
            }
            return definitions;
        }

        private static MoralChoiceQuestDefinition Map(MoralChoiceQuestRecord r) => new MoralChoiceQuestDefinition
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
