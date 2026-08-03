using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    /// <summary>
    /// Sleeping bunk / bed module. ComfortLevel and Capacity drive SleepQuality
    /// recovery (Prompt #32). Module id: bed.
    /// </summary>
    [CreateAssetMenu(fileName = "BedModule", menuName = "ASHFALL/Shelter/Bed Module")]
    public class BedModuleSO : ShelterModule
    {
        public const string DefaultModuleId = "bed";

        [Header("Bed")]
        [Tooltip("0..1 comfort contribution to sleep quality (1 = full rest).")]
        [Range(0f, 1f)]
        public float ComfortLevel = 1f;

        [Tooltip("How many survivors can sleep here at once.")]
        public int Capacity = 1;

        /// <summary>Factory for tests / bootstrap when no asset is loaded.</summary>
        public static BedModuleSO CreateDefault(float comfort = 1f, int capacity = 1)
        {
            var so = CreateInstance<BedModuleSO>();
            so.ModuleId = DefaultModuleId;
            so.DisplayName = "Bed";
            so.Description = "A dedicated bunk. Quiet, warm quarters make the difference.";
            so.ComfortLevel = Mathf.Clamp01(comfort);
            so.Capacity = Mathf.Max(1, capacity);
            so.MaxLevel = 3;
            return so;
        }
    }
}
