// CrossingIds.cs — canonical identifiers for ASHFALL: NOBODY'S CHARTER.
// Single source of truth for every quest, location, item, NPC, flag and
// expansion id minted by the pack. Never scatter string literals.
// Source: docs/expansions/expansion_03_nobodys_charter_plan.md.
namespace AtomicWar._Game.Core
{
    public static class CrossingIds
    {
        public const string Expansion = "expansion_nobodys_charter";
        public const string Region = "region_crossing";

        /// <summary>Canonical quest ids (bible §4.1 main questline + §4.2 side).</summary>
        public static class Quests
        {
            public const string TheVouch   = "quest_crossing_the_vouch";
            public const string FirstWeigh = "quest_crossing_first_weigh";
            public const string TheTerms   = "quest_crossing_the_terms";
            public const string ThePetition= "quest_crossing_the_petition";
            public const string TheStanding= "quest_crossing_the_standing";
            public const string TheMarker  = "quest_crossing_the_marker";
            public const string TheForfeit = "quest_crossing_the_forfeit";
            public const string TheVoteNot = "quest_crossing_the_vote_that_isnt";
            public const string ScaleIntegrity = "quest_crossing_scale_integrity";
            public const string CharterCore = "quest_crossing_three_dry_pages";
            public const string WhoHoldsLedger = "quest_crossing_who_holds_the_ledger";
            public const string CompanionMattis = "quest_crossing_companion_mattis";
        }

        /// <summary>Canonical location ids (bible §2.4).</summary>
        public static class Locations
        {
            public const string ViaductGate  = "loc_crossing_viaduct_gate";
            public const string Scalehouse   = "loc_crossing_scalehouse";
            public const string Stallrow     = "loc_crossing_stallrow";
            public const string Watchtower   = "loc_crossing_watchtower";
            public const string Weighbridge  = "loc_crossing_weighbridge";
            public const string Underwrite   = "loc_crossing_underwrite_hall";
            public const string RecordsRoom  = "loc_crossing_records_room";
            public const string Nightfire    = "loc_crossing_nightfire";
        }

        /// <summary>Canonical item ids (bible §7).</summary>
        public static class Items
        {
            public const string VouchToken     = "item_vouch_token_crossing";
            public const string CalibrationWeight = "item_calibration_weight";
            public const string TradedGrain    = "item_crossing_traded_grain";
            public const string TradedSalt     = "item_crossing_traded_salt";
            public const string PledgeSlip     = "item_crossing_pledge_slip";
            public const string CharterPages   = "item_charter_three_pages";
            public const string DebtContractCopy = "item_debt_contract_copy";
            public const string MarkerRubbing  = "item_marker_rubbing";
            public const string DutyLogFragment = "item_duty_log_fragment";
            public const string TradeManifestBlank = "item_trade_manifest_blank";
            public const string WynReceiptPaid = "item_wyn_receipt_paid";
        }

        /// <summary>World / story flag keys (bible §3 branching table).</summary>
        public static class Flags
        {
            public const string VouchedClean   = "flag_crossing_vouched_clean";
            public const string VouchBurned    = "flag_crossing_vouch_burned";
            public const string AccessSoftened = "flag_crossing_access_softened";
            public const string UnderwriteUntested = "flag_crossing_underwrite_untested";
            public const string PetitionUnsigned = "flag_crossing_petition_unsigned";
            public const string StandingHonest = "flag_crossing_standing_honest";
            public const string StandingRigged = "flag_crossing_standing_rigged";
            /// <summary>Set when the Ostrowski rumour starts — never at boot.</summary>
            public const string ExpansionUnlocked = "exp_nobodys_charter_unlocked";
            /// <summary>The vouch quest reward (token + lore) has been granted once.</summary>
            public const string VouchRewarded = "flag_crossing_vouch_rewarded";
        }

        /// <summary>Named Crossing NPCs (ids must exist in characters.json).</summary>
        public static class Npcs
        {
            public const string OsranKell = "npc_osran_kell";
            public const string MattisCray = "npc_mattis_cray";
            public const string DessaVane = "npc_dessa_vane";
            public const string PerrinAshby = "npc_perrin_ashby";
            public const string IvoFenn = "npc_ivo_fenn";
        }

        /// <summary>Knowledge keys granted by quest completion.</summary>
        public static class Knowledge
        {
            public const string TheVouch       = "lore_nc_the_vouch";
            public const string ReadAgain      = "lore_nc_read_again";
            public const string RubricAgain    = "lore_nc_the_rubric_again";
            public const string ThreeLegends   = "lore_nc_three_legends";
            public const string TheForfeit     = "lore_nc_the_forfeit";
            public const string TheStanding    = "lore_nc_the_standing";
        }

        /// <summary>World-state mutation ids (bible §6 endings / WorldStateConsequenceSystem).</summary>
        public static class Mutations
        {
            public const string CharterRevealed = "mutation_crossing_charter_revealed";
            public const string HonestTrader    = "mutation_crossing_honest_trader";
            public const string UnderwriteBurned = "mutation_crossing_underwrite_burned";
            public const string UnderwriteReliable = "mutation_crossing_underwrite_reliable";
            public const string PetitionRevised = "mutation_crossing_petition_revised";
            public const string PetitionLeaked = "mutation_crossing_petition_leaked";
            public const string StandingRigged  = "mutation_crossing_standing_rigged";
            public const string StandingHonest  = "mutation_crossing_standing_honest";
            public const string ForfeitHonoured = "mutation_crossing_forfeit_honoured";
            public const string VoteClean       = "mutation_crossing_vote_clean";
            public const string VoteSabotaged   = "mutation_crossing_vote_sabotaged";
        }

        /// <summary>Ending ids (bible §3 Endings).</summary>
        public static class Endings
        {
            public const string Scale       = "ending_crossing_scale";
            public const string Underwrite  = "ending_crossing_underwrite";
            public const string Compact     = "ending_crossing_compact";
            public const string None        = "ending_crossing_none";
            public const string Walked      = "ending_crossing_walked";
        }

        /// <summary>Canonical Crossing encounters (bible §6.2).</summary>
        public static class Encounters
        {
            public const string CollectorVisit = "enc_nc_collector_visit";
            public const string BackerPressure = "enc_nc_backer_pressure";
            public const string LockupMuscle = "enc_nc_lockup_muscle";
            public const string IronRaidersScout = "enc_nc_iron_raiders_scout";
            public const string DeserterPassage = "enc_nc_deserter_passage";
            public const string ScavengerDispute = "enc_nc_scavenger_dispute";
            public const string GrainExchangeEnvoy = "enc_nc_grain_exchange_envoy";
            public const string SunSeekersPass = "enc_nc_sun_seekers_pass";
            public const string ForfeitWitness = "enc_nc_forfeit_witness";
            public const string StandingAmbush = "enc_nc_standing_ambush";
        }

        /// <summary>Canonical multi-phase Crises (bible §6.3).</summary>
        public static class Crises
        {
            public const string TheForfeit = "crisis_the_forfeit";
            public const string TheVote = "crisis_the_vote";
            public const string TheStandingContested = "crisis_the_standing_contested";
            public const string TheCharterFound = "crisis_the_charter_found";
            public const string WhoHoldsTheLedger = "crisis_who_holds_the_ledger";
        }

        // Blocs (Currents-shaped, live in crossing_factions.json — NOT faction_lore.json).
        public const string FactionScale     = "faction_the_scale";
        public const string FactionUnderwrite= "faction_the_underwrite";
        public const string FactionCompact   = "faction_the_compact";
    }
}