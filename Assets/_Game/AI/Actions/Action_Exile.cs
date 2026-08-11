using System;
using System.Collections.Generic;

namespace AtomicWar._Game.AI.Actions
{
    [Serializable]
    public class ExileActionState
    {
        public string actionId = "action_exile";
        public string survivorId;
        public bool backpackGiven;
        public int foodGiven;
        public string weaponGiven;
        public int exileDay;
        public bool somberClosureTriggered;
        public bool executed;
    }

    /// <summary>
    /// Prompt #844: Honorable Exile — Unlike Banishment, this is consensual.
    /// Give survivor Backpack + 10 Food + Gun. They leave peacefully.
    /// Generates a SomberClosure morale event instead of Despair.
    /// </summary>
    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_Exile
    {
        private ExileActionState _state = new ExileActionState();

        private const int RequiredFood = 10;
        private const float SomberClosureMorale = 5f;
        private const float SomberClosureGrief = -2f;

        public event Action<string> OnExilePrepared;                                   // survivorId
        public event Action<string, int> OnExileExecuted;                              // survivorId, day
        public event Action<string[], float> OnSomberClosure;                          // remainingSurvivors, moraleChange

        public ExileActionState CaptureState() => _state;

        public void RestoreState(ExileActionState state) => _state = state;

        /// <summary>
        /// Prepares the exile by staging the required supplies.
        /// Returns true if preparation is valid (backpack + food + weapon).
        /// </summary>
        public bool PrepareExile(string survivorId, bool backpack, int food, string weapon)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (!backpack || food < RequiredFood || string.IsNullOrEmpty(weapon)) return false;

            _state.survivorId = survivorId;
            _state.backpackGiven = backpack;
            _state.foodGiven = food;
            _state.weaponGiven = weapon;

            OnExilePrepared?.Invoke(survivorId);
            return true;
        }

        /// <summary>
        /// Executes the exile on the given day. Survivor leaves peacefully.
        /// This action cannot be reversed.
        /// </summary>
        public bool ExecuteExile(int currentDay)
        {
            if (_state.executed) return false;
            if (string.IsNullOrEmpty(_state.survivorId)) return false;
            if (!_state.backpackGiven || _state.foodGiven < RequiredFood || string.IsNullOrEmpty(_state.weaponGiven))
                return false;

            _state.exileDay = currentDay;
            _state.executed = true;
            _state.somberClosureTriggered = true;

            OnExileExecuted?.Invoke(_state.survivorId, currentDay);
            return true;
        }

        /// <summary>
        /// Returns the SomberClosure morale event data after a successful exile.
        /// Caller provides the list of remaining survivor IDs.
        /// </summary>
        public (string eventType, float moraleChange, float griefChange, string[] affectedIds) GetMoraleEvent(
            string[] remainingSurvivors)
        {
            if (!_state.somberClosureTriggered)
                return (null, 0f, 0f, Array.Empty<string>());

            OnSomberClosure?.Invoke(remainingSurvivors, SomberClosureMorale);

            return ("somber_closure", SomberClosureMorale, SomberClosureGrief, remainingSurvivors);
        }

        /// <summary>
        /// Returns true if the exile has been executed.
        /// </summary>
        public bool IsExiled() => _state.executed;
    }
}
