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

        /// <summary>Canonical questline ids (Prompts #215–#256).</summary>
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
        }
    }
}
