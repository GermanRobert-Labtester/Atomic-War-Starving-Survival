using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Medical
{
    [Serializable]
    public class AdrenalineCrashState
    {
        public string afflictionId = "affliction_adrenaline_crash";
        public bool staminaDropToZero = true;
        public float restRequiredHours = 4f;
        public float hoursRemaining = 0f;
    }

    [Serializable]
    public class AdrenalineCrashSave
    {
        public List<CrashEntry> crashEntries = new List<CrashEntry>();
    }

    [Serializable]
    public class CrashEntry
    {
        public string survivorId;
        public float hoursRemaining;
    }

    public class Affliction_AdrenalineCrash
    {
        public event Action<string> OnCrashTriggered;
        public event Action<string> OnCrashRecovered;

        private AdrenalineCrashState _state;
        private Dictionary<string, float> _crashedSurvivors = new Dictionary<string, float>();

        public Affliction_AdrenalineCrash()
        {
            _state = new AdrenalineCrashState();
        }

        public Affliction_AdrenalineCrash(AdrenalineCrashState state)
        {
            _state = state ?? new AdrenalineCrashState();
        }

        public AdrenalineCrashState CaptureState() => _state;

        public void RestoreState(AdrenalineCrashState state)
        {
            _state = state ?? new AdrenalineCrashState();
        }

        public AdrenalineCrashSave CaptureSave()
        {
            var save = new AdrenalineCrashSave();
            foreach (var kvp in _crashedSurvivors)
            {
                save.crashEntries.Add(new CrashEntry
                {
                    survivorId = kvp.Key,
                    hoursRemaining = kvp.Value
                });
            }
            return save;
        }

        public void RestoreSave(AdrenalineCrashSave save)
        {
            _crashedSurvivors.Clear();
            if (save == null) return;
            foreach (var entry in save.crashEntries)
            {
                _crashedSurvivors[entry.survivorId] = entry.hoursRemaining;
            }
        }

        /// <summary>
        /// Triggers an adrenaline crash for the survivor.
        /// Stamina drops to 0 and the survivor must rest for the required hours.
        /// </summary>
        public void TriggerCrash(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _crashedSurvivors[survivorId] = _state.restRequiredHours;
            OnCrashTriggered?.Invoke(survivorId);
        }

        /// <summary>
        /// Decrements the crash recovery timer by the given number of hours.
        /// When the timer reaches 0, the survivor recovers.
        /// </summary>
        public void TickHour(string survivorId, float hours)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_crashedSurvivors.TryGetValue(survivorId, out float remaining))
                return;

            remaining -= hours;
            if (remaining <= 0f)
            {
                _crashedSurvivors.Remove(survivorId);
                OnCrashRecovered?.Invoke(survivorId);
            }
            else
            {
                _crashedSurvivors[survivorId] = remaining;
            }
        }

        /// <summary>
        /// Returns whether the survivor can move. False during an adrenaline crash.
        /// </summary>
        public bool CanMove(string survivorId)
        {
            return !_crashedSurvivors.ContainsKey(survivorId);
        }

        /// <summary>
        /// Returns whether the survivor is currently in an adrenaline crash.
        /// </summary>
        public bool IsCrashed(string survivorId)
        {
            return _crashedSurvivors.ContainsKey(survivorId);
        }

        /// <summary>
        /// Returns the hours remaining until recovery, or 0 if not crashed.
        /// </summary>
        public float GetHoursRemaining(string survivorId)
        {
            if (_crashedSurvivors.TryGetValue(survivorId, out float remaining))
                return remaining;
            return 0f;
        }
    }
}
