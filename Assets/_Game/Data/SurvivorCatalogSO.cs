using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// ScriptableObject catalog of recruitable survivor archetypes; imported from
    /// StreamingAssets/Data/survivors.json.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSurvivorCatalog", menuName = "ASHFALL/Data/Survivor Catalog")]
    public class SurvivorCatalogSO : ScriptableObject
    {
        public List<SurvivorArchetypeSO> archetypes = new List<SurvivorArchetypeSO>();

        /// <summary>Look up an archetype by its snake_case id.</summary>
        public SurvivorArchetypeSO GetById(string id) => throw new System.NotImplementedException();
    }

    /// <summary>Static definition of a survivor archetype: identity, profession, base stats.</summary>
    [CreateAssetMenu(fileName = "NewSurvivorArchetype", menuName = "ASHFALL/Data/Survivor Archetype")]
    public class SurvivorArchetypeSO : ScriptableObject
    {
        public string id;
        public string displayName;
        public string profession;
        [TextArea(2, 4)] public string bio;
        public float baseHealth = 100f;
    }
}
