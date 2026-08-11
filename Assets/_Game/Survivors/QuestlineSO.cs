using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Personal questline definition (Prompt #214). Assigned to a survivor
    /// archetype as their predetermined path to a latent expert trait.
    /// Survivors never start with the trait — they earn it by finishing this.
    /// </summary>
    [CreateAssetMenu(fileName = "Questline", menuName = "ASHFALL/Survivor/Questline")]
    public class QuestlineSO : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string displayName;
        [TextArea(2, 4)] public string description;

        [Header("Reward")]
        [Tooltip("snake_case latent expert trait perk id unlocked on final stage.")]
        public string latentExpertTraitId;

        [Header("Structure")]
        [Tooltip("Stages required before the latent trait unlocks (final stage = complete).")]
        public int maxStages = 1;

        [Header("Map / narrative hooks")]
        [Tooltip("Optional expedition map node id to spawn when the questline begins.")]
        public string spawnMapNodeId;

        [Tooltip("Optional bunker narrative event id to queue when the questline begins.")]
        public string spawnBunkerEventId;

        /// <summary>Canonical questline ids (Prompts #215–#266).</summary>
        public static class Ids
        {
            public const string ShakingHand = "quest_the_shaking_hand";
            public const string EmptyBottles = "quest_the_empty_bottles";
            public const string RabidPack = "quest_the_rabid_pack";
            public const string BrokenMind = "quest_the_broken_mind";
            public const string MassGrave = "quest_the_mass_grave";
            // Prompts #220–#224
            public const string GhostsOfDay1 = "quest_ghosts_of_day_1";
            public const string ThePrecinct = "quest_the_precinct";
            public const string TheHoldout = "quest_the_holdout";
            public const string TheWhiteElk = "quest_the_white_elk";
            public const string TheWardensKey = "quest_the_wardens_key";
            // Prompts #225–#234
            public const string TheCityMains = "quest_the_city_mains";
            public const string TheSubstationGhost = "quest_the_substation_ghost";
            public const string TheBlueprints = "quest_the_blueprints";
            public const string TheMotorpool = "quest_the_motorpool";
            public const string TheLabRuin = "quest_the_lab_ruin";
            public const string TheSeedVault = "quest_the_seed_vault";
            public const string TheLostRoute = "quest_the_lost_route";
            public const string TheBankHeist = "quest_the_bank_heist";
            public const string TheRadarStation = "quest_the_radar_station";
            public const string GroundZero = "quest_ground_zero";
            // Prompts #235–#248
            public const string TheAbandonedSchool = "quest_the_abandoned_school";
            public const string TheRally = "quest_the_rally";
            public const string CrisisOfFaith = "quest_crisis_of_faith";
            public const string TruthOfDay30 = "quest_truth_of_day_30";
            public const string DeadAir = "quest_dead_air";
            public const string TheFinalHarvest = "quest_the_final_harvest";
            public const string TheMarathon = "quest_the_marathon";
            public const string TheInferno = "quest_the_inferno";
            public const string TheKevlarLoom = "quest_the_kevlar_loom";
            public const string BrokenChronometer = "quest_broken_chronometer";
            public const string MuseumArchive = "quest_museum_archive";
            public const string TheCleansing = "quest_the_cleansing";
            public const string TheLastStash = "quest_the_last_stash";
            public const string TheLocket = "quest_the_locket";
            // Prompts #249–#256
            public const string TheEmptyCrib = "quest_the_empty_crib";
            public const string TheBrokenPromise = "quest_the_broken_promise";
            public const string GrowingUpFast = "quest_growing_up_fast";
            public const string FirstBlood = "quest_first_blood";
            public const string ThePerfectEquation = "quest_the_perfect_equation";
            public const string TheMaskSlips = "quest_the_mask_slips";
            public const string TheBoyWhoCriedWolf = "quest_the_boy_who_cried_wolf";
            public const string TheWeightOfGold = "quest_the_weight_of_gold";
            // Prompts #257–#266
            public const string CourtMartial = "quest_court_martial";
            public const string TheFinalPayload = "quest_the_final_payload";
            public const string HoldingTheLine = "quest_holding_the_line";
            public const string InventoryAudit = "quest_inventory_audit";
            public const string DroppingTheRifle = "quest_dropping_the_rifle";
            public const string TheSponge = "quest_the_sponge";
            public const string HellIsOtherPeople = "quest_hell_is_other_people";
            public const string ShatteredGlass = "quest_shattered_glass";
            public const string TheUltimatePrice = "quest_the_ultimate_price";
            public const string TheBotchedJob = "quest_the_botched_job";
            // Prompts #267–#276 chemistry / adapted
            public const string ColdTurkey = "quest_cold_turkey";
            public const string TheLongNight = "quest_the_long_night";
            public const string TheRealIllness = "quest_the_real_illness";
            public const string TrialByFire = "quest_trial_by_fire";
            public const string AVoiceInTheDark = "quest_a_voice_in_the_dark";
            public const string TheBunkerBreached = "quest_the_bunker_breached";
            public const string EmbracingTheGlow = "quest_embracing_the_glow";
            public const string ThePack = "quest_the_pack";
            public const string TheUltimateTest = "quest_the_ultimate_test";
            public const string TheLastSeed = "quest_the_last_seed";
            // Prompts #277–#283 titles
            public const string RedemptionArc = "quest_redemption_arc";
            public const string TheLastRide = "quest_the_last_ride";
            public const string ARealLeader = "quest_a_real_leader";
            public const string TheHardReboot = "quest_the_hard_reboot";
            public const string TheFinalBroadcast = "quest_the_final_broadcast";
            public const string PuttingDownRoots = "quest_putting_down_roots";
            public const string TheGoldenParachute = "quest_the_golden_parachute";
            // Prompts #284–#288 rebuilders
            public const string TheCanary = "quest_the_canary";
            public const string TheLongHaul = "quest_the_long_haul";
            public const string TheIronGate = "quest_the_iron_gate";
            public const string TheMess = "quest_the_mess";
            public const string TheClearcut = "quest_the_clearcut";
            // Prompts #289–#293 scholars
            public const string TheStrain = "quest_the_strain";
            public const string TheDeadStars = "quest_the_dead_stars";
            public const string TheArchive = "quest_the_archive";
            public const string InTheBlack = "quest_in_the_black";
            public const string TheMasterpiece = "quest_the_masterpiece";
            // Prompts #294–#298 outlaws
            public const string TheStash = "quest_the_stash";
            public const string TheLastContract = "quest_the_last_contract";
            public const string TheBigScore = "quest_the_big_score";
            public const string ThePerfectFake = "quest_the_perfect_fake";
            public const string TheEscape = "quest_the_escape";
            // Prompts #299–#318 ashes / improbable / final flawed
            public const string TheRealWorld = "quest_the_real_world";
            public const string TheGladiator = "quest_the_gladiator";
            public const string TheFirstSave = "quest_the_first_save";
            public const string PlayerOne = "quest_player_one";
            public const string LossOfFaith = "quest_loss_of_faith";
            public const string SeparationAnxiety = "quest_separation_anxiety";
            public const string TheIndependent = "quest_the_independent";
            public const string Vindicated = "quest_vindicated";
            public const string TheOldWoods = "quest_the_old_woods";
            public const string TheAwakening = "quest_the_awakening";
            public const string BreakingChains = "quest_breaking_chains";
            public const string EarningKeep = "quest_earning_keep";
            public const string WorthlessPaper = "quest_worthless_paper";
            public const string TheMastersFate = "quest_the_masters_fate";
            public const string TheBackdraft = "quest_the_backdraft";
            public const string TheProsthetic = "quest_the_prosthetic";
            public const string AGoodDeath = "quest_a_good_death";
            public const string TearsInRain = "quest_tears_in_rain";
            public const string MansBestFriend = "quest_mans_best_friend";
            public const string TheTuringTest = "quest_the_turing_test";
            // Prompts #319–#325 new faction / personal / shelter quests
            public const string GarrisonLastOrder = "quest_garrison_last_order";
            public const string MilitiaGrainWar = "quest_militia_grain_war";
            public const string CultGlowCommunion = "quest_cult_glow_communion";
            public const string ElenaTriage = "quest_elena_triage";
            public const string MechanicHighwayHeart = "quest_mechanic_highway_heart";
            public const string ChildSoldierRifle = "quest_child_soldier_rifle";
            public const string DeepWell = "quest_deep_well";
        }
    }
}
