using System;
using System.Collections.Generic;
using System.Globalization;

namespace Ashfall.Core.Quests
{
    /// <summary>Quest type classification used by Holdfast quest definitions.</summary>
    public enum QuestType
    {
        Crisis,
        Expedition,
        Exploration,
        Faction,
        Shelter,
        Story
    }

    /// <summary>One objective within a quest definition.</summary>
    public class QuestObjective
    {
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CompletionText { get; set; } = string.Empty;
        public string? RequiresItem { get; set; }
        public List<string>? RequiresItems { get; set; }
        public string? RequiresKnowledge { get; set; }
        public string? RequiresLocation { get; set; }
        public string? RequiresNpcInteraction { get; set; }
        public bool RequiresPlayerChoice { get; set; }
        public bool RequiresSurvivorSelection { get; set; }

        public int Quantity { get; set; }

        public QuestObjective() { }

        public QuestObjective(
            string id,
            string description,
            string completionText,
            string? requiresItem = null,
            List<string>? requiresItems = null,
            string? requiresKnowledge = null,
            string? requiresLocation = null,
            string? requiresNpcInteraction = null,
            bool requiresPlayerChoice = false,
            bool requiresSurvivorSelection = false,
            int quantity = 0)
        {
            Id = id;
            Description = description;
            CompletionText = completionText;
            RequiresItem = requiresItem;
            RequiresItems = requiresItems;
            RequiresKnowledge = requiresKnowledge;
            RequiresLocation = requiresLocation;
            RequiresNpcInteraction = requiresNpcInteraction;
            RequiresPlayerChoice = requiresPlayerChoice;
            RequiresSurvivorSelection = requiresSurvivorSelection;
            Quantity = quantity;
        }
    }

    /// <summary>Reward type classification used by Holdfast quest definitions.</summary>
    public enum QuestRewardType
    {
        AccessLost,
        AfflictionKnowledge,
        Antiseptic,
        BootsAsWarmthItems,
        BranchFlags,
        ClinicAccess,
        ClinicSaltFriction,
        ClusterIndoorTemp,
        Codex,
        CodexDump,
        CompanionLockIn,
        CompanionUnlock,
        CutterTrust,
        EndingFlag,
        FactionAccess,
        FactionTrust,
        GuestHousing,
        HistorySecondParagraph,
        InjuryRisk,
        Item,
        Knowledge,
        LocationFlag,
        LocationUnlock,
        Morale,
        MoraleEvent,
        MoraleSplit,
        NoItems,
        PlantState,
        PossibleRetrievalEvent,
        RebuildersHegemonyDelta,
        Recipe,
        RoadSafety,
        SilentNameplateFlag,
        SteamStability,
        StressEvent,
        ThreateningProse,
        TollReceipt,
        TradeRates,
        TradeUnlock,
        TravelTimeHint,
        VictorySlide,
        VossOrmundTriangle,
        WorksPriceShock,
        WrenTruthFlag
    }

    /// <summary>One reward granted on quest completion.</summary>
    public class QuestReward
    {
        public QuestRewardType Type { get; set; }
        public string Id { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public float Value { get; set; }
        public string ValueString { get; set; } = string.Empty;

        public QuestReward() { }

        public QuestReward(
            QuestRewardType type,
            string id,
            int quantity = 0,
object? value = null)
        {
            Type = type;
            Id = id;
            Quantity = quantity;
            switch (value)
            {
                case null:
                    Value = 0f;
                    ValueString = string.Empty;
                    break;
                case string s:
                    Value = 0f;
                    ValueString = s;
                    break;
                case float f:
                    Value = f;
                    ValueString = f.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    break;
                case double d:
                    Value = (float)d;
                    ValueString = d.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    break;
                case int i:
                    Value = i;
                    ValueString = i.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    break;
                default:
                    Value = 0f;
                    ValueString = value.ToString() ?? string.Empty;
                    break;
            }
        }
    }

    /// <summary>
    /// Holdfast quest definition — a static registry entry describing one quest.
    /// Runtime quest logic lives in <see cref="HoldfastQuestSystem"/>; this class
    /// is the data contract only.
    /// </summary>
    public class QuestDefinition
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public QuestType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<QuestObjective> Objectives { get; set; } = new List<QuestObjective>();
        public List<QuestReward> Rewards { get; set; } = new List<QuestReward>();
        public List<string> FailureConsequences { get; set; } = new List<string>();
        public List<string> HostileElements { get; set; } = new List<string>();

        public QuestDefinition() { }

        public QuestDefinition(
            string id,
            string displayName,
            QuestType type,
            string description,
            List<QuestObjective>? objectives = null,
            List<QuestReward>? rewards = null,
            List<string>? failureConsequences = null,
            List<string>? hostileElements = null)
        {
            Id = id;
            DisplayName = displayName;
            Type = type;
            Description = description;
            Objectives = objectives ?? new List<QuestObjective>();
            Rewards = rewards ?? new List<QuestReward>();
            FailureConsequences = failureConsequences ?? new List<string>();
            HostileElements = hostileElements ?? new List<string>();
        }
    }
}
