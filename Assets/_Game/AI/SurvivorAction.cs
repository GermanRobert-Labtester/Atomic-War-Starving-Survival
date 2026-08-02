using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI
{
    /// <summary>
    /// ScriptableObject describing a candidate action (id, tags, base priority)
    /// that the UtilityAI can score and a survivor can perform. Data-driven so
    /// new behaviours are assets, not code.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSurvivorAction", menuName = "ASHFALL/Survivor Action")]
    public class SurvivorAction : ScriptableObject
    {
        public string id;
        public string displayName;
        [TextArea(2, 4)] public string description;
        public float basePriority;

        /// <summary>Perform the action for a survivor.</summary>
        public virtual void Execute(Survivor survivor) => throw new System.NotImplementedException();
    }
}
