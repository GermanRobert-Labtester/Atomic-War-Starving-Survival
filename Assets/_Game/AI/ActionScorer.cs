using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI
{
    /// <summary>
    /// Produces a normalized 0..1 utility score for a candidate action given a
    /// survivor's current needs and context. Pure function; no side effects.
    /// </summary>
    public class ActionScorer
    {
        /// <summary>Score how desirable an action is for a survivor right now.</summary>
        public float Score(SurvivorAction action, Survivor survivor) => throw new System.NotImplementedException();
    }
}
