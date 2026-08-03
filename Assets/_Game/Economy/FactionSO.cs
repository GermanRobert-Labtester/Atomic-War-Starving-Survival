using UnityEngine;

namespace AtomicWar._Game.Economy
{
    /// <summary>
    /// Data-driven trading faction (military remnants, scavenger camp, preppers).
    /// Trust is runtime state on DynamicEconomySystem, keyed by <see cref="id"/>.
    /// </summary>
    [CreateAssetMenu(fileName = "Faction", menuName = "ASHFALL/Economy/Faction")]
    public class FactionSO : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string displayName;
        [TextArea(2, 4)] public string description;

        [Header("Disposition")]
        [Tooltip("Starting trust toward the player (-100..100).")]
        [Range(-100f, 100f)]
        public float startingTrust = 0f;

        [Tooltip("How hard this faction hits on a hatch raid (0..1).")]
        [Range(0f, 1f)]
        public float raidAggression = 0.5f;

        [Tooltip("Minimum trust required to open a trade session.")]
        [Range(-100f, 100f)]
        public float minTrustToTrade = -40f;

        [Tooltip("At or below this trust the faction may rob instead of trade.")]
        [Range(-100f, 100f)]
        public float robThreshold = -20f;

        [Tooltip("At or above this trust the faction will share IntelNode-class tips.")]
        [Range(-100f, 100f)]
        public float intelShareThreshold = 40f;

        [Tooltip("At or below this trust the faction may raid the shelter hatch.")]
        [Range(-100f, 100f)]
        public float raidThreshold = -50f;

        /// <summary>Canonical snake_case faction ids.</summary>
        public static class Ids
        {
            public const string MilitaryRemnants = "military_remnants";
            public const string ScavengerCamp = "scavenger_camp";
            public const string DoomsdayPreppers = "doomsday_preppers";
        }
    }
}
