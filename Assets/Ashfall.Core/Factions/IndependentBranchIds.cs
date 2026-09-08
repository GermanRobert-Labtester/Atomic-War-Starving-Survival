using System;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Canonical snake_case ids for the Independent faction branch of "The
    /// Weight of Choices" branching system. Pinned 1:1 against
    /// independent_faction_branch.json by IndependentBranchCatalogTests.
    ///
    /// Unlike Military/Rebel, Independent is not a faction with its own
    /// internal alignment to sway — it is the ABSENCE of Military/Rebel
    /// commitment, so there is no IndependentAlignmentRecord. What makes this
    /// branch set genuinely different rather than a third mirror:
    ///
    /// - Some branches gate on the player's PRPF standing (IndependentBranchIds
    ///   introduces no PRPF-specific ids; PrpfIds.FactionId is read directly)
    ///   rather than only on the player's own MoralChoiceSystem band.
    /// - IND-4 (Exile) gates on hostile PlayerFactionStandingRecord values for
    ///   BOTH faction_military and faction_rebel simultaneously (the "enemy of
    ///   everyone" playstyle), a dual check no Military/Rebel branch needs.
    /// - PRPF's TryJoin already has zero dependency on Military/Rebel
    ///   commitment (a "cold join" is simply calling it with no branch
    ///   committed) — Independent does not need a special cold-join id or
    ///   flag, it needs its OWN branches to acknowledge that PRPF membership
    ///   can precede, coincide with, or replace an Independent branch choice
    ///   entirely.
    ///
    /// Naming mirrors Military/Rebel: branch_ind_&lt;n&gt;_&lt;slug&gt;,
    /// ending_ind_&lt;n&gt;&lt;letter&gt;_&lt;slug&gt;, flag_branch_ind_&lt;n&gt;_ponr.
    /// </summary>
    public static class IndependentBranchIds
    {
        public const string FactionId = "faction_independent";
        public const int BranchCount = 15;

        // ── Base branches ──────────────────────────────────────────────
        public const string BranchSurvivor = "branch_ind_1_survivor";
        public const string BranchMercenary = "branch_ind_2_mercenary";
        public const string BranchPeacekeeperDiplomat = "branch_ind_3_peacekeeper_diplomat";
        public const string BranchExile = "branch_ind_4_exile";
        public const string BranchKingmaker = "branch_ind_5_kingmaker";
        public const string BranchLegend = "branch_ind_6_legend";
        public const string BranchGhost = "branch_ind_7_ghost";
        public const string BranchWastelandMyth = "branch_ind_8_wasteland_myth";
        public const string BranchHermit = "branch_ind_9_hermit";
        public const string BranchMediator = "branch_ind_10_mediator";
        public const string BranchScavengerKing = "branch_ind_11_scavenger_king";
        public const string BranchCaretaker = "branch_ind_12_caretaker";
        public const string BranchWitness = "branch_ind_13_witness";
        public const string BranchEngineer = "branch_ind_14_engineer";
        public const string BranchProphet = "branch_ind_15_prophet";

        public static readonly string[] AllBranches =
        {
            BranchSurvivor, BranchMercenary, BranchPeacekeeperDiplomat, BranchExile,
            BranchKingmaker, BranchLegend, BranchGhost, BranchWastelandMyth,
            BranchHermit, BranchMediator, BranchScavengerKing, BranchCaretaker,
            BranchWitness, BranchEngineer, BranchProphet
        };

        // ── Point-of-no-return flags (one per branch, set once, irreversible) ──
        public const string FlagPonrSurvivor = "flag_branch_ind_1_ponr";
        public const string FlagPonrMercenary = "flag_branch_ind_2_ponr";
        public const string FlagPonrPeacekeeperDiplomat = "flag_branch_ind_3_ponr";
        public const string FlagPonrExile = "flag_branch_ind_4_ponr";
        public const string FlagPonrKingmaker = "flag_branch_ind_5_ponr";
        public const string FlagPonrLegend = "flag_branch_ind_6_ponr";
        public const string FlagPonrGhost = "flag_branch_ind_7_ponr";
        public const string FlagPonrWastelandMyth = "flag_branch_ind_8_ponr";
        public const string FlagPonrHermit = "flag_branch_ind_9_ponr";
        public const string FlagPonrMediator = "flag_branch_ind_10_ponr";
        public const string FlagPonrScavengerKing = "flag_branch_ind_11_ponr";
        public const string FlagPonrCaretaker = "flag_branch_ind_12_ponr";
        public const string FlagPonrWitness = "flag_branch_ind_13_ponr";
        public const string FlagPonrEngineer = "flag_branch_ind_14_ponr";
        public const string FlagPonrProphet = "flag_branch_ind_15_ponr";

        /// <summary>PoNR flag id for a given base branch id, in branch declaration order.</summary>
        public static string PonrFlagFor(string branchId) => branchId switch
        {
            BranchSurvivor => FlagPonrSurvivor,
            BranchMercenary => FlagPonrMercenary,
            BranchPeacekeeperDiplomat => FlagPonrPeacekeeperDiplomat,
            BranchExile => FlagPonrExile,
            BranchKingmaker => FlagPonrKingmaker,
            BranchLegend => FlagPonrLegend,
            BranchGhost => FlagPonrGhost,
            BranchWastelandMyth => FlagPonrWastelandMyth,
            BranchHermit => FlagPonrHermit,
            BranchMediator => FlagPonrMediator,
            BranchScavengerKing => FlagPonrScavengerKing,
            BranchCaretaker => FlagPonrCaretaker,
            BranchWitness => FlagPonrWitness,
            BranchEngineer => FlagPonrEngineer,
            BranchProphet => FlagPonrProphet,
            _ => throw new ArgumentException($"Unknown Independent branch id '{branchId}'.", nameof(branchId))
        };

        // ── Endings: ending_ind_<n><letter>_<slug> ─────────────────────
        public const string EndingSurvivorA = "ending_ind_1a_lone_survivor";
        public const string EndingSurvivorB = "ending_ind_1b_traitor";
        public const string EndingSurvivorC = "ending_ind_1c_legend";

        public const string EndingMercenaryA = "ending_ind_2a_warlord";
        public const string EndingMercenaryB = "ending_ind_2b_survivor";
        public const string EndingMercenaryC = "ending_ind_2c_legend";

        public const string EndingPeacekeeperDiplomatA = "ending_ind_3a_peacekeeper_unifier";
        public const string EndingPeacekeeperDiplomatB = "ending_ind_3b_diplomat";
        public const string EndingPeacekeeperDiplomatC = "ending_ind_3c_survivor";

        public const string EndingExileA = "ending_ind_4a_tyrant";
        public const string EndingExileB = "ending_ind_4b_ghost";
        public const string EndingExileC = "ending_ind_4c_traitor";

        public const string EndingKingmakerA = "ending_ind_5a_puppet_master";
        public const string EndingKingmakerB = "ending_ind_5b_survivor";
        public const string EndingKingmakerC = "ending_ind_5c_unifier";

        public const string EndingLegendA = "ending_ind_6a_savior";
        public const string EndingLegendB = "ending_ind_6b_monster";
        public const string EndingLegendC = "ending_ind_6c_myth";

        public const string EndingGhostA = "ending_ind_7a_unseen";
        public const string EndingGhostB = "ending_ind_7b_forgotten";
        public const string EndingGhostC = "ending_ind_7c_watcher";

        public const string EndingWastelandMythA = "ending_ind_8a_feared_legend";
        public const string EndingWastelandMythB = "ending_ind_8b_revered_legend";
        public const string EndingWastelandMythC = "ending_ind_8c_forgotten_legend";

        public const string EndingHermitA = "ending_ind_9a_quiet_holding";
        public const string EndingHermitB = "ending_ind_9b_unlatched_door";
        public const string EndingHermitC = "ending_ind_9c_no_smoke";

        public const string EndingMediatorA = "ending_ind_10a_open_table";
        public const string EndingMediatorB = "ending_ind_10b_necessary_liar";
        public const string EndingMediatorC = "ending_ind_10c_no_safe_chair";

        public const string EndingScavengerKingA = "ending_ind_11a_quartermaster";
        public const string EndingScavengerKingB = "ending_ind_11b_locked_rooms";
        public const string EndingScavengerKingC = "ending_ind_11c_dead_mans_inventory";

        public const string EndingCaretakerA = "ending_ind_12a_extra_chairs";
        public const string EndingCaretakerB = "ending_ind_12b_hands_full";
        public const string EndingCaretakerC = "ending_ind_12c_broken_promise";

        public const string EndingWitnessA = "ending_ind_13a_record_stands";
        public const string EndingWitnessB = "ending_ind_13b_margin_notes";
        public const string EndingWitnessC = "ending_ind_13c_missing_pages";

        public const string EndingEngineerA = "ending_ind_14a_load_bearing";
        public const string EndingEngineerB = "ending_ind_14b_necessary_failure";
        public const string EndingEngineerC = "ending_ind_14c_name_on_breakdown";

        public const string EndingProphetA = "ending_ind_15a_keeper_of_vigils";
        public const string EndingProphetB = "ending_ind_15b_voice_in_the_hall";
        public const string EndingProphetC = "ending_ind_15c_ash_gospel";
    }
}
