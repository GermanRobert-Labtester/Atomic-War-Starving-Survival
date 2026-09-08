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
        public const int BranchCount = 15;

        // ── Base branches ──────────────────────────────────────────────
        public const string BranchTrueRebel = "branch_rebel_1_true_rebel";
        public const string BranchDefector = "branch_rebel_2_defector";
        public const string BranchOpportunist = "branch_rebel_3_opportunist";
        public const string BranchMartyr = "branch_rebel_4_martyr";
        public const string BranchWarlord = "branch_rebel_5_warlord";
        public const string BranchReformer = "branch_rebel_6_reformer";
        public const string BranchLoneWolf = "branch_rebel_7_lone_wolf";
        public const string BranchRevolution = "branch_rebel_8_revolution";
        public const string BranchBombmaker = "branch_rebel_9_bombmaker";
        public const string BranchCourier = "branch_rebel_10_courier";
        public const string BranchPropagandist = "branch_rebel_11_propagandist";
        public const string BranchDissident = "branch_rebel_12_dissident";
        public const string BranchProtector = "branch_rebel_13_protector";
        public const string BranchSaboteur = "branch_rebel_14_saboteur";
        public const string BranchNegotiator = "branch_rebel_15_negotiator";

        public static readonly string[] AllBranches =
        {
            BranchTrueRebel, BranchDefector, BranchOpportunist, BranchMartyr,
            BranchWarlord, BranchReformer, BranchLoneWolf, BranchRevolution,
            BranchBombmaker, BranchCourier, BranchPropagandist, BranchDissident,
            BranchProtector, BranchSaboteur, BranchNegotiator
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
        public const string FlagPonrBombmaker = "flag_branch_rebel_9_ponr";
        public const string FlagPonrCourier = "flag_branch_rebel_10_ponr";
        public const string FlagPonrPropagandist = "flag_branch_rebel_11_ponr";
        public const string FlagPonrDissident = "flag_branch_rebel_12_ponr";
        public const string FlagPonrProtector = "flag_branch_rebel_13_ponr";
        public const string FlagPonrSaboteur = "flag_branch_rebel_14_ponr";
        public const string FlagPonrNegotiator = "flag_branch_rebel_15_ponr";

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
            BranchBombmaker => FlagPonrBombmaker,
            BranchCourier => FlagPonrCourier,
            BranchPropagandist => FlagPonrPropagandist,
            BranchDissident => FlagPonrDissident,
            BranchProtector => FlagPonrProtector,
            BranchSaboteur => FlagPonrSaboteur,
            BranchNegotiator => FlagPonrNegotiator,
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

        public const string EndingBombmakerA = "ending_rebel_9a_repentant_defector";
        public const string EndingBombmakerB = "ending_rebel_9b_scarred_survivor";
        public const string EndingBombmakerC = "ending_rebel_9c_unrepentant_zealot";

        public const string EndingCourierA = "ending_rebel_10a_trusted_network";
        public const string EndingCourierB = "ending_rebel_10b_broken_route";
        public const string EndingCourierC = "ending_rebel_10c_executed_traitor";

        public const string EndingPropagandistA = "ending_rebel_11a_voice_of_the_people";
        public const string EndingPropagandistB = "ending_rebel_11b_broadcast_in_ash";
        public const string EndingPropagandistC = "ending_rebel_11c_silenced_by_their_own";

        public const string EndingDissidentA = "ending_rebel_12a_reconciled_civilian";
        public const string EndingDissidentB = "ending_rebel_12b_unwelcome_witness";
        public const string EndingDissidentC = "ending_rebel_12c_killed_in_defection";

        public const string EndingProtectorA = "ending_rebel_13a_community_shield";
        public const string EndingProtectorB = "ending_rebel_13b_postkeeper";
        public const string EndingProtectorC = "ending_rebel_13c_overwhelmed_guard";

        public const string EndingSaboteurA = "ending_rebel_14a_resistance_hero";
        public const string EndingSaboteurB = "ending_rebel_14b_bitter_liberator";
        public const string EndingSaboteurC = "ending_rebel_14c_starved_freedom";

        public const string EndingNegotiatorA = "ending_rebel_15a_peacemaker";
        public const string EndingNegotiatorB = "ending_rebel_15b_uneasy_accord";
        public const string EndingNegotiatorC = "ending_rebel_15c_marked_collaborator";
    }
}
