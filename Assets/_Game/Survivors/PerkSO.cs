using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Data-driven perk awarded when hidden discipline XP crosses a threshold
    /// (Prompt #179). Tone: calluses and muscle memory, not fantasy levels.
    /// </summary>
    [CreateAssetMenu(fileName = "Perk", menuName = "ASHFALL/Survivor/Perk")]
    public class PerkSO : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string displayName;
        [TextArea(2, 4)] public string description;

        [Header("Discipline")]
        [Tooltip("snake_case discipline: medical, crafting, science, combat, scavenging, survival")]
        public string disciplineId;

        [Tooltip("Hidden XP required to earn this perk.")]
        public float xpThreshold = 100f;

        [Tooltip("Added to effective skill for this discipline while the perk is Active (not Dormant).")]
        [Range(0f, 0.5f)]
        public float skillBonus = 0.15f;

        [Header("Expert track")]
        [Tooltip("If true, only a survivor whose ExpertDisciplineId matches disciplineId may earn this. One expert perk per survivor.")]
        public bool isExpertPerk;
    }
}
