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

        /// <summary>Canonical questline ids (Prompts #215–#234).</summary>
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
        }
    }
}
