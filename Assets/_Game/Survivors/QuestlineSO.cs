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

        /// <summary>Canonical questline ids (Prompts #215–#224).</summary>
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
        }
    }
}
