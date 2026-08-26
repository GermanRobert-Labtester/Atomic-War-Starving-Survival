using System;
using System.Collections.Generic;

namespace Ashfall.Core.MoralChoice
{
    // ── JSON wire records (snake_case, deserialized by IJsonSerializer) ──

    [Serializable]
    public sealed class MoralChoiceChainCatalogContainer
    {
        public int schema_version = 1;
        public string description = string.Empty;
        public List<MoralBranchRecord> branches = new List<MoralBranchRecord>();
        public MoralMergeRulesRecord merge_rules = new MoralMergeRulesRecord();
        public MoralLockoutRulesRecord lockout_rules = new MoralLockoutRulesRecord();
        public List<MoralQuestGateRecord> quest_gates = new List<MoralQuestGateRecord>();
        public MoralEchoQuestsContainer echo_quests = new MoralEchoQuestsContainer();
    }

    [Serializable]
    public sealed class MoralBranchRecord
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public int lock_threshold;
        public List<string> locks_out = new List<string>();
        public List<string> merge_allowed = new List<string>();
        public List<string> entry_quests = new List<string>();
        public string locked_flag = string.Empty;
    }

    [Serializable]
    public sealed class MoralMergeRulesRecord
    {
        public string description = string.Empty;
        public string merge_quest_prefix = string.Empty;
        public int merge_quests_require_min_progress;
        public bool merge_never_unlocks_exclusive;
    }

    [Serializable]
    public sealed class MoralLockoutRulesRecord
    {
        public string description = string.Empty;
        public bool lockout_is_permanent;
        public bool lockout_fires_journal_entry;
        public string lockout_journal_template = string.Empty;
    }

    [Serializable]
    public sealed class MoralQuestGateRecord
    {
        public string quest_id = string.Empty;
        public List<string> requires = new List<string>();
        public int? requires_choice_index;
        public int? requires_min_moral;
        public int? requires_max_moral;
        public int? requires_min_empathy;
        public string requires_flag = string.Empty;
        public string branch = string.Empty;
    }

    [Serializable]
    public sealed class MoralEchoQuestsContainer
    {
        public string description = string.Empty;
        public List<MoralEchoQuestRecord> quests = new List<MoralEchoQuestRecord>();
    }

    [Serializable]
    public sealed class MoralEchoQuestRecord
    {
        public string quest_id = string.Empty;
        public string triggered_by = string.Empty;
        public int triggered_by_choice;
        public int min_days_after;
        public string branch = string.Empty;
    }

    /// <summary>
    /// Loads moral_choice_chains.json — the branching architecture (4 branches,
    /// merge rules, lockout rules, quest gates, echo quest triggers).
    /// Engine-agnostic: IFileIO + IJsonSerializer ports.
    /// </summary>
    public static class MoralChoiceChainCatalogLoader
    {
        public const string DefaultFileName = "moral_choice_chains.json";

        public static MoralChoiceChainData Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new MoralChoiceChainData();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new MoralChoiceChainData();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new MoralChoiceChainData();

            var container = json.Deserialize<MoralChoiceChainCatalogContainer>(raw);
            if (container == null)
                return new MoralChoiceChainData();

            return Map(container);
        }

        private static MoralChoiceChainData Map(MoralChoiceChainCatalogContainer c)
        {
            var data = new MoralChoiceChainData
            {
                MergeRules = new MoralMergeRules
                {
                    Description = c.merge_rules?.description ?? string.Empty,
                    MergeQuestPrefix = c.merge_rules?.merge_quest_prefix ?? string.Empty,
                    MergeQuestsRequireMinProgress = c.merge_rules?.merge_quests_require_min_progress ?? 0,
                    MergeNeverUnlocksExclusive = c.merge_rules?.merge_never_unlocks_exclusive ?? false
                },
                LockoutRules = new MoralLockoutRules
                {
                    Description = c.lockout_rules?.description ?? string.Empty,
                    LockoutIsPermanent = c.lockout_rules?.lockout_is_permanent ?? false,
                    LockoutFiresJournalEntry = c.lockout_rules?.lockout_fires_journal_entry ?? false,
                    LockoutJournalTemplate = c.lockout_rules?.lockout_journal_template ?? string.Empty
                }
            };

            if (c.branches != null)
            {
                foreach (var b in c.branches)
                {
                    if (b == null) continue;
                    data.Branches.Add(new MoralBranchDefinition
                    {
                        Id = b.id,
                        DisplayName = b.display_name,
                        Description = b.description,
                        LockThreshold = b.lock_threshold,
                        LocksOut = b.locks_out ?? new List<string>(),
                        MergeAllowed = b.merge_allowed ?? new List<string>(),
                        EntryQuests = b.entry_quests ?? new List<string>(),
                        LockedFlag = b.locked_flag ?? string.Empty
                    });
                }
            }

            if (c.quest_gates != null)
            {
                foreach (var g in c.quest_gates)
                {
                    if (g == null) continue;
                    data.QuestGates.Add(new MoralQuestGate
                    {
                        QuestId = g.quest_id,
                        Requires = g.requires ?? new List<string>(),
                        RequiresChoiceIndex = g.requires_choice_index,
                        RequiresMinMoral = g.requires_min_moral,
                        RequiresMaxMoral = g.requires_max_moral,
                        RequiresMinEmpathy = g.requires_min_empathy,
                        RequiresFlag = g.requires_flag ?? string.Empty,
                        Branch = g.branch ?? string.Empty
                    });
                }
            }

            if (c.echo_quests?.quests != null)
            {
                foreach (var e in c.echo_quests.quests)
                {
                    if (e == null) continue;
                    data.EchoQuests.Add(new MoralEchoQuestDefinition
                    {
                        QuestId = e.quest_id,
                        TriggeredBy = e.triggered_by,
                        TriggeredByChoice = e.triggered_by_choice,
                        MinDaysAfter = e.min_days_after,
                        Branch = e.branch ?? string.Empty
                    });
                }
            }

            return data;
        }
    }
}
