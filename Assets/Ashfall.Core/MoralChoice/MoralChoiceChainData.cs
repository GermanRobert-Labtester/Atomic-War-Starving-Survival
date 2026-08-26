using System.Collections.Generic;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>
    /// Wire shape for moral_choice_chains.json — the branching architecture.
    /// Defines 4 permanent branches (Mercy Road, Iron Way, Listener's Thread,
    /// Broken Compact), merge-back rules, lockout rules, quest gates, and
    /// echo quest triggers.
    /// </summary>
    public sealed class MoralChoiceChainData
    {
        public List<MoralBranchDefinition> Branches { get; set; } = new List<MoralBranchDefinition>();
        public MoralMergeRules MergeRules { get; set; } = new MoralMergeRules();
        public MoralLockoutRules LockoutRules { get; set; } = new MoralLockoutRules();
        public List<MoralQuestGate> QuestGates { get; set; } = new List<MoralQuestGate>();
        public List<MoralEchoQuestDefinition> EchoQuests { get; set; } = new List<MoralEchoQuestDefinition>();
    }

    public sealed class MoralBranchDefinition
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int LockThreshold { get; set; }
        public List<string> LocksOut { get; set; } = new List<string>();
        public List<string> MergeAllowed { get; set; } = new List<string>();
        public List<string> EntryQuests { get; set; } = new List<string>();
        public string LockedFlag { get; set; } = string.Empty;
    }

    public sealed class MoralMergeRules
    {
        public string Description { get; set; } = string.Empty;
        public string MergeQuestPrefix { get; set; } = string.Empty;
        public int MergeQuestsRequireMinProgress { get; set; }
        public bool MergeNeverUnlocksExclusive { get; set; }
    }

    public sealed class MoralLockoutRules
    {
        public string Description { get; set; } = string.Empty;
        public bool LockoutIsPermanent { get; set; }
        public bool LockoutFiresJournalEntry { get; set; }
        public string LockoutJournalTemplate { get; set; } = string.Empty;
    }

    /// <summary>
    /// Gate for a chain quest: prerequisites (prior quest in chain), optional
    /// moral/empathy/flag requirements, and branch ownership.
    /// </summary>
    public sealed class MoralQuestGate
    {
        public string QuestId { get; set; } = string.Empty;
        public List<string> Requires { get; set; } = new List<string>();
        public int? RequiresChoiceIndex { get; set; }
        public int? RequiresMinMoral { get; set; }
        public int? RequiresMaxMoral { get; set; }
        public int? RequiresMinEmpathy { get; set; }
        public string RequiresFlag { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
    }

    /// <summary>
    /// Echo quest: fires when a specific earlier quest was resolved with a
    /// specific choice, after a minimum number of days have passed.
    /// </summary>
    public sealed class MoralEchoQuestDefinition
    {
        public string QuestId { get; set; } = string.Empty;
        public string TriggeredBy { get; set; } = string.Empty;
        public int TriggeredByChoice { get; set; }
        public int MinDaysAfter { get; set; }
        public string Branch { get; set; } = string.Empty;
    }
}
