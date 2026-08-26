using System.Collections.Generic;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>
    /// Wire shape for moral_choice_faction_reactions.json — faction NPC
    /// dialogues triggered by moral threshold events. Each event fires once
    /// per save when the player crosses a moral band boundary overnight.
    /// </summary>
    public sealed class MoralChoiceFactionReactionsData
    {
        public Dictionary<string, MoralThresholdReaction> ThresholdReactions { get; set; }
            = new Dictionary<string, MoralThresholdReaction>();
    }

    public sealed class MoralThresholdReaction
    {
        public string EventDescription { get; set; } = string.Empty;
        public List<MoralFactionDialogue> PeacekeeperDialogue { get; set; } = new List<MoralFactionDialogue>();
        public List<MoralFactionDialogue> RaiderDialogue { get; set; } = new List<MoralFactionDialogue>();
        public List<MoralFactionDialogue> KnowledgeKeeperDialogue { get; set; } = new List<MoralFactionDialogue>();
        public List<MoralFactionDialogue> CivilianDialogue { get; set; } = new List<MoralFactionDialogue>();
        public string JournalEntry { get; set; } = string.Empty;
    }

    public sealed class MoralFactionDialogue
    {
        public string Speaker { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<string> Lines { get; set; } = new List<string>();
    }
}
