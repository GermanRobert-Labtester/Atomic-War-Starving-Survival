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

        /// <summary>
        /// Aliases of <see cref="AtomicWar._Game.Survivors.DisabilityId"/>, kept so
        /// existing Medical call sites keep compiling. Do not redeclare the literals
        /// here — that duplication is what let the ids drift apart.
        /// </summary>
        public static class Ids
        {
            public const string Limp = AtomicWar._Game.Survivors.DisabilityId.Limp;
            public const string ScarredLungs = AtomicWar._Game.Survivors.DisabilityId.ScarredLungs;
            public const string Tremors = AtomicWar._Game.Survivors.DisabilityId.Tremors;
            public const string OneEye = AtomicWar._Game.Survivors.DisabilityId.OneEye;
        }
    }
}
