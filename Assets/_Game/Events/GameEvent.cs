using UnityEngine;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// ScriptableObject definition of a scripted/narrative game event: identity,
    /// selection weight, gating conditions, and outcome hooks. Data-driven from
    /// StreamingAssets/Data/events.json.
    /// </summary>
    [CreateAssetMenu(fileName = "NewGameEvent", menuName = "ASHFALL/Game Event")]
    public class GameEvent : ScriptableObject
    {
        public string id;
        public string title;
        [TextArea(3, 6)] public string bodyText;
        public float weight = 1f;
        public int minDay;

        /// <summary>Apply the event's effects to the world.</summary>
        public virtual void Apply() => throw new System.NotImplementedException();
    }
}
