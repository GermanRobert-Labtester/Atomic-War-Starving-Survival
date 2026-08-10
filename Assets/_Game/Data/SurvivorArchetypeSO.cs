using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// Static definition of a survivor archetype: identity, profession, base stats,
    /// and Prompt #214 personal-quest destiny (latent expert trait + questline).
    ///
    /// Lives in its own file because Unity only links a MonoScript to an asset
    /// when the type's file name matches the class name. Declared inside
    /// SurvivorCatalogSO.cs it serialized with m_Script: {fileID: 0}, which made
    /// every generated archetype unresolvable by type.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSurvivorArchetype", menuName = "ASHFALL/Data/Survivor Archetype")]
    public class SurvivorArchetypeSO : ScriptableObject
    {
        public string id;
        public string displayName;
        public string profession;
        [TextArea(2, 4)] public string bio;
        public float baseHealth = 100f;

        [Header("Personal Quest (Prompt #214)")]
        [Tooltip("Predetermined latent expert trait id — NOT granted on Day 0.")]
        public string latentExpertTrait;

        [Tooltip("QuestlineSO id that unlocks the latent trait when completed.")]
        public string activeQuestlineId;

        /// <summary>Build a runtime SurvivorProfile from this archetype.</summary>
        public AtomicWar._Game.Survivors.SurvivorProfile ToProfile()
        {
            return new AtomicWar._Game.Survivors.SurvivorProfile(
                id, latentExpertTrait, activeQuestlineId);
        }
    }
}
