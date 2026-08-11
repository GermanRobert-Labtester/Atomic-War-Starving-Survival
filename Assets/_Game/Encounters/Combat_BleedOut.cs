using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class BleedOutState
    {
        public string mechanicId = "combat_bleed_out";
        public int turnsUntilDeath = 3;
    }

    [Serializable]
    public class BleedOutSave
    {
        public List<DownedEntry> downedEntries = new List<DownedEntry>();
    }

    [Serializable]
    public class DownedEntry
    {
        public string survivorId;
        public int turnsRemaining;
    }

    public class Combat_BleedOut
    {
        public event Action<string> OnSurvivorDowned;
        public event Action<string, int> OnBleedOutTick;
        public event Action<string> OnSurvivorDied;
        public event Action<string, string> OnSurvivorBandaged;

        private BleedOutState _state;
        private Dictionary<string, int> _downedSurvivors = new Dictionary<string, int>();

        public Combat_BleedOut()
        {
            _state = new BleedOutState();
        }

        public Combat_BleedOut(BleedOutState state)
        {
            _state = state ?? new BleedOutState();
        }

        public BleedOutState CaptureState() => _state;

        public void RestoreState(BleedOutState state)
        {
            _state = state ?? new BleedOutState();
        }

        public BleedOutSave CaptureSave()
        {
            var save = new BleedOutSave();
            foreach (var kvp in _downedSurvivors)
            {
                save.downedEntries.Add(new DownedEntry
                {
                    survivorId = kvp.Key,
                    turnsRemaining = kvp.Value
                });
            }
            return save;
        }

        public void RestoreSave(BleedOutSave save)
        {
            _downedSurvivors.Clear();
            if (save == null) return;
            foreach (var entry in save.downedEntries)
            {
                _downedSurvivors[entry.survivorId] = entry.turnsRemaining;
            }
        }

        /// <summary>
        /// Downs a survivor, starting the bleed-out timer.
        /// </summary>
        public void Down(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _downedSurvivors[survivorId] = _state.turnsUntilDeath;
            OnSurvivorDowned?.Invoke(survivorId);
        }

        /// <summary>
        /// Ticks all bleed-out timers by one turn. Survivors reaching 0 turns die permanently.
        /// </summary>
        public void Tick()
        {
            // Collect keys to avoid modifying dictionary during iteration
            List<string> toRemove = new List<string>();

            foreach (var kvp in _downedSurvivors)
            {
                int remaining = kvp.Value - 1;
                _downedSurvivors[kvp.Key] = remaining;

                if (remaining <= 0)
                {
                    toRemove.Add(kvp.Key);
                    OnSurvivorDied?.Invoke(kvp.Key);
                }
                else
                {
                    OnBleedOutTick?.Invoke(kvp.Key, remaining);
                }
            }

            for (int i = 0; i < toRemove.Count; i++)
            {
                _downedSurvivors.Remove(toRemove[i]);
            }
        }

        /// <summary>
        /// Rescuer sacrifices their turn to bandage and save a downed survivor.
        /// </summary>
        public bool Bandage(string rescuerId, string downedId)
        {
            if (string.IsNullOrEmpty(rescuerId) || string.IsNullOrEmpty(downedId))
                return false;
            if (!_downedSurvivors.ContainsKey(downedId))
                return false;

            _downedSurvivors.Remove(downedId);
            OnSurvivorBandaged?.Invoke(downedId, rescuerId);
            return true;
        }

        /// <summary>
        /// Returns remaining turns for a downed survivor, or -1 if not downed.
        /// </summary>
        public int GetTurnsRemaining(string survivorId)
        {
            if (_downedSurvivors.TryGetValue(survivorId, out int remaining))
                return remaining;
            return -1;
        }

        public bool IsDowned(string survivorId)
        {
            return _downedSurvivors.ContainsKey(survivorId);
        }
    }
}
