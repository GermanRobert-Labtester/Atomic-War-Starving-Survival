using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BurrowersState
    {
        public string id;
        public int breachLevel;
        public bool isBreachPatched;
        public bool returnsNightly;
        public int concreteRequired = 10;
    }

    public class BurrowersSystem
    {
        private readonly BurrowersState _state;

        public BurrowersState State => _state;

        public event Action<string, int> OnBreachTriggered;  // id, breachLevel
        public event Action<string, bool> OnFightOutcome;    // id, survived
        public event Action<string> OnBreachPatched;         // id

        public BurrowersSystem(string id)
        {
            _state = new BurrowersState
            {
                id = id,
                breachLevel = 0,
                isBreachPatched = false,
                returnsNightly = false,
                concreteRequired = 10
            };
        }

        /// <summary>
        /// Burrowers breach from the lowest bunker wall (not the Hatch).
        /// </summary>
        public void TriggerBreach(int lowestBunkerLevel)
        {
            _state.breachLevel = lowestBunkerLevel;
            _state.isBreachPatched = false;
            _state.returnsNightly = true;
            OnBreachTriggered?.Invoke(_state.id, _state.breachLevel);
        }

        /// <summary>
        /// Fight burrowers in combat. Returns true if the group survives.
        /// </summary>
        public bool FightBurrowers(float combatPower, Random rng)
        {
            float threshold = 0.3f + (rng.NextDouble() * 0.4f);
            bool survived = combatPower > threshold;
            OnFightOutcome?.Invoke(_state.id, survived);
            return survived;
        }

        /// <summary>
        /// Patch breach with concrete. Returns true if successful.
        /// </summary>
        public bool PatchBreach(int concreteAvailable)
        {
            if (concreteAvailable < _state.concreteRequired)
                return false;

            _state.isBreachPatched = true;
            _state.returnsNightly = false;
            OnBreachPatched?.Invoke(_state.id);
            return true;
        }
    }
}
