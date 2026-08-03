using UnityEngine;

namespace AtomicWar._Game.Medical
{
    public enum DisabilityKind
    {
        Limp,
        ScarredLungs,
        Tremors,
        OneEye
    }

    /// <summary>
    /// Permanent physical impairment earned by spending >72 hours in a critical state.
    /// Dynamic need caps and permanent stat penalties. Non-magical, serializable.
    /// </summary>
    [CreateAssetMenu(fileName = "disability", menuName = "AtomicWar/Medical/Disability")]
    public class DisabilitySO : ScriptableObject
    {
        public string id;
        public string displayName;
        public string description;
        public DisabilityKind kind;

        /// <summary>Dynamic limit on maximum health (100 = uncapped).</summary>
        public float maxHealthCap = 100f;

        /// <summary>Multiplier applied to expedition stamina drain (1.0 = normal).</summary>
        public float staminaDrainMultiplier = 1f;

        /// <summary>Multiplier applied to medical and crafting action speeds (1.0 = normal, 0.5 = 50% speed).</summary>
        public float actionSpeedMultiplier = 1f;

        public static class Ids
        {
            public const string Limp = "limp";
            public const string ScarredLungs = "scarred_lungs";
            public const string Tremors = "tremors";
            public const string OneEye = "one_eye";
        }
    }
}
