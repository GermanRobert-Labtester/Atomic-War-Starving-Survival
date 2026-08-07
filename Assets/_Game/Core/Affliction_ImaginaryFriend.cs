using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ImaginaryFriendState
    {
        public string survivorId;
        public int daysSinceOnset;
        public string fakeSurvivorName;
        public float foodWastedPerDay = 1f;
        public bool isHallucinating;
        public int isolationDaysRequired = 20;
    }

    
    [Serializable]
    public class ImaginaryFriendSystemSave
    {
        public List<string> keys = new List<string>();
        public List<ImaginaryFriendState> values = new List<ImaginaryFriendState>();
    }
public class ImaginaryFriendSystem
    {
        private readonly Dictionary<string, ImaginaryFriendState> _states = new Dictionary<string, ImaginaryFriendState>();

        public IReadOnlyDictionary<string, ImaginaryFriendState> States => _states;

        public event Action<string, string> OnImaginaryFriendSpawned;  // survivorId, fakeName
        public event Action<string, float> OnFoodWasted;  // survivorId, amount
        public event Action<string, string> OnFakeJournalEntry;  // survivorId, entry
        public event Action<string> OnHallucinationCured;  // survivorId

        private static readonly string[] FakeNames =
        {
            "Mira", "Toll", "Isa", "Korr", "Dell",
            "Senna", "Pavek", "Nira", "Hal", "Yeva"
        };

        private static readonly string[] FakeJournalLines =
        {
            "We talked for hours today. They understand.",
            "They said everything will be fine. I believe them.",
            "They brought me something. I can almost see it.",
            "We laughed together. It felt real.",
            "They warned me about the others. They know things."
        };

        private ImaginaryFriendState GetOrCreate(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state))
            {
                state = new ImaginaryFriendState
                {
                    survivorId = survivorId,
                    daysSinceOnset = 0,
                    fakeSurvivorName = string.Empty,
                    foodWastedPerDay = 1f,
                    isHallucinating = false,
                    isolationDaysRequired = 20
                };
                _states[survivorId] = state;
            }
            return state;
        }

        public bool CheckIsolationTrigger(string survivorId, int soloDays)
        {
            var state = GetOrCreate(survivorId);
            if (state.isHallucinating)
                return false;

            if (soloDays >= state.isolationDaysRequired)
            {
                return true;
            }
            return false;
        }

        public void SpawnFakeSurvivor(string survivorId, System.Random rng)
        {
            var state = GetOrCreate(survivorId);
            if (state.isHallucinating)
                return;

            state.isHallucinating = true;
            state.daysSinceOnset = 0;
            state.fakeSurvivorName = FakeNames[rng.Next(FakeNames.Length)];
            OnImaginaryFriendSpawned?.Invoke(survivorId, state.fakeSurvivorName);
        }

        public void TickDay(string survivorId, System.Random rng)
        {
            if (!_states.TryGetValue(survivorId, out var state) || !state.isHallucinating)
                return;

            state.daysSinceOnset++;

            // Waste food for the imaginary companion
            OnFoodWasted?.Invoke(survivorId, state.foodWastedPerDay);

            // Write a fake journal entry
            string entry = $"[{state.fakeSurvivorName}] " + FakeJournalLines[rng.Next(FakeJournalLines.Length)];
            OnFakeJournalEntry?.Invoke(survivorId, entry);
        }

        public void Cure(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state) || !state.isHallucinating)
                return;

            state.isHallucinating = false;
            state.fakeSurvivorName = string.Empty;
            state.daysSinceOnset = 0;
            OnHallucinationCured?.Invoke(survivorId);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ImaginaryFriendSystemSave CaptureState()
        {
            var save = new ImaginaryFriendSystemSave();
            foreach (var kvp in _states)
            {
                save.keys.Add(kvp.Key);
                save.values.Add(kvp.Value);
            }
            return save;
        }

        public void RestoreState(ImaginaryFriendSystemSave saved)
        {
            _states.Clear();
            if (saved == null || saved.keys == null) return;
            for (int i = 0; i < saved.keys.Count; i++)
            {
                var val = (saved.values != null && i < saved.values.Count) ? saved.values[i] : null;
                if (val != null) _states[saved.keys[i]] = val;
            }
        }

}
}
