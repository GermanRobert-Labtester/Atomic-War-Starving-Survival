using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.MoralChoice
{
    // ── JSON wire records ──

    [Serializable]
    public sealed class MoralChoiceFactionReactionsContainer
    {
        public int schema_version = 1;
        public string description = string.Empty;
        public Dictionary<string, MoralThresholdReactionRecord> threshold_reactions
            = new Dictionary<string, MoralThresholdReactionRecord>();
    }

    [Serializable]
    public sealed class MoralThresholdReactionRecord
    {
        public string event_description = string.Empty;
        public List<MoralFactionDialogueRecord> peacekeeper_dialogue = new List<MoralFactionDialogueRecord>();
        public List<MoralFactionDialogueRecord> raider_dialogue = new List<MoralFactionDialogueRecord>();
        public List<MoralFactionDialogueRecord> knowledge_keeper_dialogue = new List<MoralFactionDialogueRecord>();
        public List<MoralFactionDialogueRecord> civilian_dialogue = new List<MoralFactionDialogueRecord>();
        public string journal_entry = string.Empty;
    }

    [Serializable]
    public sealed class MoralFactionDialogueRecord
    {
        public string speaker = string.Empty;
        public string location = string.Empty;
        public List<string> lines = new List<string>();
    }

    /// <summary>
    /// Loads moral_choice_faction_reactions.json — faction NPC dialogues for
    /// each moral threshold event. Engine-agnostic.
    /// </summary>
    public static class MoralChoiceFactionReactionsCatalogLoader
    {
        public const string DefaultFileName = "moral_choice_faction_reactions.json";

        public static MoralChoiceFactionReactionsData Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new MoralChoiceFactionReactionsData();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new MoralChoiceFactionReactionsData();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new MoralChoiceFactionReactionsData();

            var container = json.Deserialize<MoralChoiceFactionReactionsContainer>(raw);
            if (container?.threshold_reactions == null)
                return new MoralChoiceFactionReactionsData();

            var data = new MoralChoiceFactionReactionsData();
            foreach (var kvp in container.threshold_reactions)
            {
                if (kvp.Value == null) continue;
                data.ThresholdReactions[kvp.Key] = MapReaction(kvp.Value);
            }
            return data;
        }

        private static MoralThresholdReaction MapReaction(MoralThresholdReactionRecord r) =>
            new MoralThresholdReaction
            {
                EventDescription = r.event_description ?? string.Empty,
                PeacekeeperDialogue = r.peacekeeper_dialogue?.Select(MapDialogue).ToList()
                    ?? new List<MoralFactionDialogue>(),
                RaiderDialogue = r.raider_dialogue?.Select(MapDialogue).ToList()
                    ?? new List<MoralFactionDialogue>(),
                KnowledgeKeeperDialogue = r.knowledge_keeper_dialogue?.Select(MapDialogue).ToList()
                    ?? new List<MoralFactionDialogue>(),
                CivilianDialogue = r.civilian_dialogue?.Select(MapDialogue).ToList()
                    ?? new List<MoralFactionDialogue>(),
                JournalEntry = r.journal_entry ?? string.Empty
            };

        private static MoralFactionDialogue MapDialogue(MoralFactionDialogueRecord d) =>
            new MoralFactionDialogue
            {
                Speaker = d.speaker ?? string.Empty,
                Location = d.location ?? string.Empty,
                Lines = d.lines ?? new List<string>()
            };
    }
}
