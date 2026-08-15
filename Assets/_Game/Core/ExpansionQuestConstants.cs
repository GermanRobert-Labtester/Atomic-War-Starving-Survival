// ExpansionQuestConstants.cs — canonical ids for the "Into the Ash" quest system.
// Every flag, event, item reference, and day trigger used by the six questlines
// lives here. Add new quests by extending these classes — never scatter string
// literals. This file is the single source of truth for all quest identifiers.
namespace AtomicWar._Game.Core
{
    /// <summary>Canonical quest, location, and stage identifiers.</summary>
    public static class QuestIds
    {
        public const string CheckpointKiloTruth = "quest_checkpoint_kilo_truth";
        public const string GrainWar            = "quest_grain_war";
        public const string GlowCommunion       = "quest_glow_communion";
        public const string TributeBreak        = "quest_tribute_break";
        public const string StMarenLastShift    = "quest_st_maren_last_shift";
        public const string TowerSevenBroadcast = "quest_tower_seven_broadcast";
    }

    /// <summary>Canonical location node ids for the expansion.</summary>
    public static class LocationIds
    {
        public const string DistrictCoordOffice  = "district_coordination_office";
        public const string CheckpointKilo       = "checkpoint_kilo_memorial";
        public const string MilitiaGrainExchange = "militia_grain_exchange";
        public const string GlowChapel           = "the_glow_chapel";
        public const string TollHouse            = "kael_tribute_ledger_site";
        public const string StMarenAnnex         = "st_maren_hospital_annex";
        public const string RadioTowerSeven      = "radio_tower_seven_bunker";
        public const string MartaFarmhouse       = "marta_farmhouse";
    }

    /// <summary>Canonical world-flag keys. Every flag read or written by the
    /// quest system is declared here so there is never a typo in a literal.</summary>
    public static class QuestFlags
    {
        // ── Checkpoint Kilo Truth ──────────────────────────────────────
        public const string KiloIntercomBroadcast    = "kilo_intercom_broadcast";
        public const string GarrisonRespondedToKilo  = "garrison_responded_to_kilo";
        public const string ComplianceLedgerFound    = "compliance_ledger_found";

        // ── Grain War ──────────────────────────────────────────────────
        public const string MartaReceiptFound        = "marta_receipt_found";
        public const string MartaChildrenExtracted   = "marta_children_extracted";
        public const string MilitiaOfferFired        = "militia_offer_fired";
        public const string MartaArrivalTriggered    = "marta_arrival_triggered";

        // ── Glow Communion ─────────────────────────────────────────────
        public const string CultVisitedBunker        = "cult_visited_bunker";
        public const string CultCeremonyAllowed      = "cult_ceremony_allowed";
        public const string CultCeremonyRefused      = "cult_ceremony_refused";
        public const string CultCupDrunk             = "cult_cup_drunk";
        public const string CultCupTested            = "cult_cup_tested";
        public const string CultConvertExists        = "cult_convert_exists";
        public const string CultSecondVisitFired     = "cult_second_visit_fired";
        public const string CultCupOfferedFired      = "cult_cup_offered_fired";
        public const string CultConvertRequested     = "cult_convert_requested";
        public const string CultPermanentDemandFired = "cult_permanent_demand_fired";
        public const string CultConvertNamePrefix    = "cult_convert_name:";

        // ── Tribute Break ──────────────────────────────────────────────
        public const string WarlordFirstThursday     = "warlord_first_thursday";
        public const string TollHouseChildFreed      = "toll_house_child_freed";
        public const string TributeBroken            = "tribute_broken";
        public const string WarlordEscalationFired   = "warlord_escalation_fired";

        // ── St. Maren's Last Shift ─────────────────────────────────────
        public const string LenaNurseFound           = "lena_nurse_found";
        public const string LenaVisitedAnnex         = "lena_visited_annex";
        public const string AnnexNamesWritten        = "annex_names_written";
        public const string AnnexGardenPlanted       = "annex_garden_planted";

        // ── Tower Seven Broadcast ──────────────────────────────────────
        public const string TowerSevenCodeKnown      = "tower_seven_bunker_code_known";
        public const string TowerSevenBunkerOpen     = "tower_seven_bunker_open";

        // ── Completion marker prefix (host reads: quest_completed:<id>) ─
        public const string QuestCompletedPrefix     = "quest_completed:";
    }

    /// <summary>Canonical bunker event ids scheduled by the quest system.</summary>
    public static class QuestEventIds
    {
        public const string MilitiaSilenceOffer      = "militia_silence_offer";
        public const string MartaReunion             = "marta_reunion";
        public const string CultSecondVisit          = "cult_second_visit";
        public const string CultCupOffered           = "cult_cup_offered";
        public const string CultPermanentConvert     = "cult_permanent_convert_demand";
        public const string WarlordEscalation        = "warlord_escalation";
        public const string GarrisonKiloResponse     = "garrison_kilo_response";
        public const string TowerSevenBreachAttempt  = "tower_seven_breach_attempt";
        public const string LenaAnnexVisit           = "lena_annex_visit";
    }

    /// <summary>Day-based trigger windows. Each constant is the first day
    /// the trigger becomes eligible (it stays eligible thereafter via guard flags).</summary>
    public static class QuestDayTriggers
    {
        public const int FirstThursday         = 20;
        public const int MilitiaOffer          = 25;
        public const int CultReturns           = 30;
        public const int CupOffered            = 35;
        public const int TowerSevenBreach      = 35;
        public const int ConvertRequest        = 40;
        public const int WarlordEscalation     = 40;
        public const int MartaReturns          = 45;
        public const int LenaAnnexVisit        = 50;
        public const int PermanentConvert      = 55;
    }

    /// <summary>Item IDs used as quest triggers. Mirrors IntoTheAshItemsCatalog.Ids
    /// and the base game catalog for items that already exist (battery, etc.).</summary>
    public static class QuestTriggerItems
    {
        public const string PrivateMarenJournal     = "private_maren_journal";
        public const string Battery                 = "battery";
        public const string MartaReceipt            = "marta_receipt";
        public const string RequisitionLedger       = "requisition_ledger";
        public const string MilitiaCharterFeedSack  = "militia_charter_feed_sack";
        public const string ComplianceLedger        = "compliance_ledger";
        public const string BrotherOrinJournal      = "brother_orin_journal";
        public const string TributeLedger           = "tribute_ledger";
        public const string SableJournal            = "sable_journal";
        public const string HeadNurseCombinationCard = "head_nurse_combination_card";
        public const string PatientRecordsAnnex     = "patient_records_annex";
        public const string FrequencyLogSealed      = "frequency_log_sealed";
        public const string CaptainVennKeycard      = "captain_venn_keycard";
    }
}
