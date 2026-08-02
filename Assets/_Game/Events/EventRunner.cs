using System.Collections.Generic;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Selects and applies GameEvents over time (weighted random and scheduled),
    /// enforcing cooldowns and gating conditions. Raises bus events so the UI can
    /// present them.
    /// </summary>
    public class EventRunner
    {
        /// <summary>Advance event scheduling/selection over elapsed game hours.</summary>
        public void Tick(float gameHours) => throw new System.NotImplementedException();

        /// <summary>Run a specific event immediately.</summary>
        public void Run(GameEvent gameEvent) => throw new System.NotImplementedException();

        /// <summary>Replace the active pool of selectable events.</summary>
        public void SetPool(IReadOnlyList<GameEvent> pool) => throw new System.NotImplementedException();
    }
}
