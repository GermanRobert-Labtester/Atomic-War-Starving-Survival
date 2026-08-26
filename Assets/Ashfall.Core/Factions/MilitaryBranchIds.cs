using System;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Canonical snake_case ids for the Military faction branch of "The Weight
    /// of Choices" branching system. Pinned 1:1 against
    /// military_faction_branch.json by MilitaryBranchCatalogTests. Rebel,
    /// Independent and the hidden Peace Reputation Protection Forces (PRPF)
    /// faction get their own id classes when those slices are built — this
    /// file owns Military only.
    ///
    /// Naming: branch_mil_&lt;n&gt;_&lt;slug&gt; for the eight base branches,
    /// ending_mil_&lt;n&gt;&lt;letter&gt;_&lt;slug&gt; for the resolved
    /// morality-gated endings, flag_branch_mil_&lt;n&gt;_ponr for each
    /// branch's point-of-no-return lock.
    /// </summary>
    public static class MilitaryBranchIds
    {
        public const string FactionId = "faction_military";
        public const int BranchCount = 8;

        // ── Base branches ──────────────────────────────────────────────
        public const string BranchLoyalSoldier = "branch_mil_1_loyal_soldier";
        public const string BranchDefector = "branch_mil_2_defector";
        public const string BranchOpportunist = "branch_mil_3_opportunist";
        public const string BranchMartyr = "branch_mil_4_martyr";
        public const string BranchTyrant = "branch_mil_5_tyrant";
        public const string BranchReformer = "branch_mil_6_reformer";
        public const string BranchDeserter = "branch_mil_7_deserter";
        public const string BranchBrokenChain = "branch_mil_8_broken_chain";

        public static readonly string[] AllBranches =
        {
            BranchLoyalSoldier, BranchDefector, BranchOpportunist, BranchMartyr,
            BranchTyrant, BranchReformer, BranchDeserter, BranchBrokenChain
        };

        // ── Point-of-no-return flags (one per branch, set once, irreversible) ──
        public const string FlagPonrLoyalSoldier = "flag_branch_mil_1_ponr";
        public const string FlagPonrDefector = "flag_branch_mil_2_ponr";
        public const string FlagPonrOpportunist = "flag_branch_mil_3_ponr";
        public const string FlagPonrMartyr = "flag_branch_mil_4_ponr";
        public const string FlagPonrTyrant = "flag_branch_mil_5_ponr";
        public const string FlagPonrReformer = "flag_branch_mil_6_ponr";
        public const string FlagPonrDeserter = "flag_branch_mil_7_ponr";
        public const string FlagPonrBrokenChain = "flag_branch_mil_8_ponr";

        /// <summary>PoNR flag id for a given base branch id, in branch declaration order.</summary>
        public static string PonrFlagFor(string branchId) => branchId switch
        {
            BranchLoyalSoldier => FlagPonrLoyalSoldier,
            BranchDefector => FlagPonrDefector,
            BranchOpportunist => FlagPonrOpportunist,
            BranchMartyr => FlagPonrMartyr,
            BranchTyrant => FlagPonrTyrant,
            BranchReformer => FlagPonrReformer,
            BranchDeserter => FlagPonrDeserter,
            BranchBrokenChain => FlagPonrBrokenChain,
            _ => throw new ArgumentException($"Unknown Military branch id '{branchId}'.", nameof(branchId))
        };

        // ── Endings: ending_mil_<n><letter>_<slug> ─────────────────────
        // Sub-letter (A/B/C) is never stored on its own — it is derived at
        // resolution time from MoralChoiceSystem.CurrentBand via
        // MilitaryBranchSystem.ResolveEnding, then looked up here.
        public const string EndingLoyalSoldierA = "ending_mil_1a_benevolent_dictator";
        public const string EndingLoyalSoldierB = "ending_mil_1b_iron_fist";
        public const string EndingLoyalSoldierC = "ending_mil_1c_survivor_king";

        public const string EndingDefectorA = "ending_mil_2a_reformer_of_rebels";
        public const string EndingDefectorB = "ending_mil_2b_warlord";
        public const string EndingDefectorC = "ending_mil_2c_survivor";

        public const string EndingOpportunistA = "ending_mil_3a_tyrant";
        public const string EndingOpportunistB = "ending_mil_3b_benevolent_warlord";
        public const string EndingOpportunistC = "ending_mil_3c_survivor_king";

        public const string EndingMartyrA = "ending_mil_4a_saint_of_wasteland";
        public const string EndingMartyrB = "ending_mil_4b_fallen_hero";
        public const string EndingMartyrC = "ending_mil_4c_broken_martyr";

        public const string EndingTyrantA = "ending_mil_5a_tyrant_king";
        public const string EndingTyrantB = "ending_mil_5b_benevolent_warlord";
        public const string EndingTyrantC = "ending_mil_5c_survivor_king";

        public const string EndingReformerA = "ending_mil_6a_visionary";
        public const string EndingReformerB = "ending_mil_6b_reformer";
        public const string EndingReformerC = "ending_mil_6c_survivor";

        public const string EndingDeserterA = "ending_mil_7a_lone_survivor";
        public const string EndingDeserterB = "ending_mil_7b_traitor";
        public const string EndingDeserterC = "ending_mil_7c_idealist";

        public const string EndingBrokenChainA = "ending_mil_8a_new_leader";
        public const string EndingBrokenChainB = "ending_mil_8b_survivor";
        public const string EndingBrokenChainC = "ending_mil_8c_warlord";
    }
}
