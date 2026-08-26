using System;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Canonical snake_case ids for the Rebel faction branch of "The Weight of
    /// Choices" branching system. Pinned 1:1 against rebel_faction_branch.json
    /// by RebelBranchCatalogTests. Mirrors MilitaryBranchIds' structure exactly
    /// (branch_rebel_&lt;n&gt;_&lt;slug&gt;, ending_rebel_&lt;n&gt;&lt;letter&gt;_&lt;slug&gt;,
    /// flag_branch_rebel_&lt;n&gt;_ponr) so the two factions' branch systems stay
    /// structurally identical even though their content differs.
    /// </summary>
    public static class RebelBranchIds
    {
        public const string FactionId = "faction_rebel";
        public const int BranchCount = 8;

        // ── Base branches ──────────────────────────────────────────────
        public const string BranchTrueRebel = "branch_rebel_1_true_rebel";
        public const string BranchDefector = "branch_rebel_2_defector";
        public const string BranchOpportunist = "branch_rebel_3_opportunist";
        public const string BranchMartyr = "branch_rebel_4_martyr";
        public const string BranchWarlord = "branch_rebel_5_warlord";
        public const string BranchReformer = "branch_rebel_6_reformer";
        public const string BranchLoneWolf = "branch_rebel_7_lone_wolf";
        public const string BranchRevolution = "branch_rebel_8_revolution";

        public static readonly string[] AllBranches =
        {
            BranchTrueRebel, BranchDefector, BranchOpportunist, BranchMartyr,
            BranchWarlord, BranchReformer, BranchLoneWolf, BranchRevolution
        };

        // ── Point-of-no-return flags (one per branch, set once, irreversible) ──
        public const string FlagPonrTrueRebel = "flag_branch_rebel_1_ponr";
        public const string FlagPonrDefector = "flag_branch_rebel_2_ponr";
        public const string FlagPonrOpportunist = "flag_branch_rebel_3_ponr";
        public const string FlagPonrMartyr = "flag_branch_rebel_4_ponr";
        public const string FlagPonrWarlord = "flag_branch_rebel_5_ponr";
        public const string FlagPonrReformer = "flag_branch_rebel_6_ponr";
        public const string FlagPonrLoneWolf = "flag_branch_rebel_7_ponr";
        public const string FlagPonrRevolution = "flag_branch_rebel_8_ponr";

        /// <summary>PoNR flag id for a given base branch id, in branch declaration order.</summary>
        public static string PonrFlagFor(string branchId) => branchId switch
        {
            BranchTrueRebel => FlagPonrTrueRebel,
            BranchDefector => FlagPonrDefector,
            BranchOpportunist => FlagPonrOpportunist,
            BranchMartyr => FlagPonrMartyr,
            BranchWarlord => FlagPonrWarlord,
            BranchReformer => FlagPonrReformer,
            BranchLoneWolf => FlagPonrLoneWolf,
            BranchRevolution => FlagPonrRevolution,
            _ => throw new ArgumentException($"Unknown Rebel branch id '{branchId}'.", nameof(branchId))
        };

        // ── Endings: ending_rebel_<n><letter>_<slug> ───────────────────
        public const string EndingTrueRebelA = "ending_rebel_1a_liberator";
        public const string EndingTrueRebelB = "ending_rebel_1b_zealot";
        public const string EndingTrueRebelC = "ending_rebel_1c_survivor";

        public const string EndingDefectorA = "ending_rebel_2a_reformer_of_military";
        public const string EndingDefectorB = "ending_rebel_2b_collaborator";
        public const string EndingDefectorC = "ending_rebel_2c_survivor";

        public const string EndingOpportunistA = "ending_rebel_3a_warlord";
        public const string EndingOpportunistB = "ending_rebel_3b_benevolent_broker";
        public const string EndingOpportunistC = "ending_rebel_3c_survivor_king";

        public const string EndingMartyrA = "ending_rebel_4a_martyr_of_the_cause";
        public const string EndingMartyrB = "ending_rebel_4b_fallen_hero";
        public const string EndingMartyrC = "ending_rebel_4c_broken_martyr";

        public const string EndingWarlordA = "ending_rebel_5a_warlord_king";
        public const string EndingWarlordB = "ending_rebel_5b_benevolent_warlord";
        public const string EndingWarlordC = "ending_rebel_5c_survivor_king";

        public const string EndingReformerA = "ending_rebel_6a_visionary";
        public const string EndingReformerB = "ending_rebel_6b_reformer";
        public const string EndingReformerC = "ending_rebel_6c_survivor";

        public const string EndingLoneWolfA = "ending_rebel_7a_lone_survivor";
        public const string EndingLoneWolfB = "ending_rebel_7b_traitor";
        public const string EndingLoneWolfC = "ending_rebel_7c_idealist";

        public const string EndingRevolutionA = "ending_rebel_8a_new_republic";
        public const string EndingRevolutionB = "ending_rebel_8b_survivor";
        public const string EndingRevolutionC = "ending_rebel_8c_warlord";
    }
}
