using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI
{
    /// <summary>
    /// Utility-AI engine (NOT an LLM at runtime). Each evaluation it scores the
    /// available SurvivorActions for a survivor via the ActionScorer and selects
    /// the highest-scoring one to execute.
    /// </summary>
    public class UtilityAI
    {
        /// <summary>Advance the AI, re-evaluating decisions on its interval.</summary>
        public void Tick(float deltaTimeSeconds) => throw new System.NotImplementedException();

        /// <summary>Pick the best action for a survivor from the candidates.</summary>
        public SurvivorAction SelectAction(Survivor survivor, IReadOnlyList<SurvivorAction> candidates) => throw new System.NotImplementedException();
    }
}
