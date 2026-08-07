using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ClairvoyantState
    {
        public string survivorId;
        public int predictionsGenerated;
        public float falseAlarmChance = 0.80f;
        public float trueDisasterChance = 0.20f;
    }

    public struct PredictionResult
    {
        public bool isReal;
        public string warningText;
    }

    
    [Serializable]
    public class ClairvoyantSystemSave
    {
        public List<string> keys = new List<string>();
        public List<ClairvoyantState> values = new List<ClairvoyantState>();
    }
public class ClairvoyantSystem
    {
        private readonly Dictionary<string, ClairvoyantState> _states = new Dictionary<string, ClairvoyantState>();

        public IReadOnlyDictionary<string, ClairvoyantState> States => _states;

        public event Action<string, PredictionResult> OnPredictionGenerated;  // survivorId, result
        public event Action<string, string> OnTruePredictionConfirmed;  // survivorId, warningText
        public event Action<string, string> OnFalseAlarm;  // survivorId, warningText

        private static readonly string[] FakeWarnings =
        {
            "Raiders are coming at dawn... I can see them.",
            "The walls will crack tonight. I felt it in my bones.",
            "Something is burrowing beneath the bunker.",
            "They're watching us from the ash clouds.",
            "The water will turn black by morning.",
            "I dreamed of fire — real fire — coming from the east."
        };

        private ClairvoyantState GetOrCreate(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state))
            {
                state = new ClairvoyantState
                {
                    survivorId = survivorId,
                    predictionsGenerated = 0,
                    falseAlarmChance = 0.80f,
                    trueDisasterChance = 0.20f
                };
                _states[survivorId] = state;
            }
            return state;
        }

        public PredictionResult GeneratePrediction(string survivorId, System.Random rng, Func<bool> hasUpcomingDisaster)
        {
            var state = GetOrCreate(survivorId);
            state.predictionsGenerated++;

            float roll = (float)rng.NextDouble();

            if (roll < state.trueDisasterChance && hasUpcomingDisaster != null && hasUpcomingDisaster())
            {
                // True prediction — real disaster exists
                var result = new PredictionResult
                {
                    isReal = true,
                    warningText = "Something terrible is coming. This time I'm sure of it."
                };
                OnPredictionGenerated?.Invoke(survivorId, result);
                OnTruePredictionConfirmed?.Invoke(survivorId, result.warningText);
                return result;
            }

            // False alarm
            var fakeResult = new PredictionResult
            {
                isReal = false,
                warningText = FakeWarnings[rng.Next(FakeWarnings.Length)]
            };
            OnPredictionGenerated?.Invoke(survivorId, fakeResult);
            OnFalseAlarm?.Invoke(survivorId, fakeResult.warningText);
            return fakeResult;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ClairvoyantSystemSave CaptureState()
        {
            var save = new ClairvoyantSystemSave();
            foreach (var kvp in _states)
            {
                save.keys.Add(kvp.Key);
                save.values.Add(kvp.Value);
            }
            return save;
        }

        public void RestoreState(ClairvoyantSystemSave saved)
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
