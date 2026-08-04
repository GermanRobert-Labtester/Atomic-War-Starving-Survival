using UnityEngine;

namespace AtomicWar._Game.Economy
{
    /// <summary>
    /// Data-driven trading faction (military remnants, scavenger camp, preppers,
    /// Cult of the Glow). Trust is runtime state on DynamicEconomySystem, keyed by
    /// <see cref="id"/>. When <see cref="trustInversion"/> is set, disposition is
    /// driven by party radiation dose instead of stored trust (hostile to healthy,
    /// friendly to the highly irradiated).
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

        [Header("Trust Inversion (Cult of the Glow)")]
        [Tooltip("When true: hostile to healthy parties, friendly to highly-irradiated ones. " +
                 "Stance and barter use party average RadiationDose instead of stored trust.")]
        public bool trustInversion = false;

        [Tooltip("Party avg RadiationDose (0..100) at/below this = 'healthy' → hostile under inversion.")]
        [Range(0f, 100f)]
        public float healthyRadiationCeiling = 20f;

        [Tooltip("Party avg RadiationDose (0..100) at/above this = 'highly irradiated' → friendly under inversion.")]
        [Range(0f, 100f)]
        public float highRadiationFloor = 60f;

        [Tooltip("Barter multiplier applied to IrradiatedWater when trading with this faction. " +
                 "Only meaningful when trustInversion is true (the glow is currency).")]
        [Min(0f)]
        public float irradiatedWaterValueMultiplier = 12f;

        /// <summary>Canonical snake_case faction ids.</summary>
        public static class Ids
        {
            public const string MilitaryRemnants = "military_remnants";
            public const string ScavengerCamp = "scavenger_camp";
            public const string DoomsdayPreppers = "doomsday_preppers";
            /// <summary>Concept 16 — worship the fallout; clean blood is heresy.</summary>
            public const string CultOfTheGlow = "cult_of_the_glow";
        }
    }
}
