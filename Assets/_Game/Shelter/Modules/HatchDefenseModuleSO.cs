using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    /// <summary>
    /// Hatch upgrade that contributes to ShelterSecurity (Prompt #33):
    /// reinforced locks, blast doors, traps. Module ids are snake_case.
    /// </summary>
    [CreateAssetMenu(fileName = "HatchDefenseModule", menuName = "ASHFALL/Shelter/Hatch Defense Module")]
    public class HatchDefenseModuleSO : ShelterModule
    {
        public const string ReinforcedLocksId = "reinforced_locks";
        public const string BlastDoorId = "blast_door";
        public const string HatchTrapsId = "hatch_traps";

        [Header("Hatch Defense")]
        [Tooltip("Security points contributed per module level.")]
        public float SecurityContribution = 10f;

        /// <summary>Factory for tests / bootstrap when no asset is loaded.</summary>
        public static HatchDefenseModuleSO Create(
            string moduleId,
            string displayName,
            float securityContribution)
        {
            var so = CreateInstance<HatchDefenseModuleSO>();
            so.ModuleId = moduleId;
            so.DisplayName = displayName;
            so.Description = "Hardens the hatch against raiders.";
            so.SecurityContribution = Mathf.Max(0f, securityContribution);
            so.MaxLevel = 3;
            return so;
        }
    }
}
