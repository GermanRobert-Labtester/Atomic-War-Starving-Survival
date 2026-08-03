using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// ScriptableObject catalog of the six BeliefProfileSO trait profiles. Hand-authored
    /// (via Tools/ASHFALL/Create Default Belief Profiles), not JSON-generated — the trait
    /// set is closed, unlike open content such as Items/Recipes/Events.
    /// </summary>
    [CreateAssetMenu(fileName = "NewBeliefProfileCatalog", menuName = "ASHFALL/Data/Belief Profile Catalog")]
    public class BeliefProfileCatalogSO : ScriptableObject
    {
        public List<BeliefProfileSO> profiles = new List<BeliefProfileSO>();

        /// <summary>Look up a belief profile by trait.</summary>
        public BeliefProfileSO GetByTrait(RiskBiasTrait trait)
        {
            if (profiles == null) return null;
            for (int i = 0; i < profiles.Count; i++)
            {
                if (profiles[i] != null && profiles[i].Trait == trait)
                {
                    return profiles[i];
                }
            }
            return null;
        }
    }
}
