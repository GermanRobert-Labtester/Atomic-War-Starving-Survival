using System;
using System.Collections.Generic;
using System.Text;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FeralRegressionState
    {
        public string survivorId;
        public bool isRegressed;
        public bool speechLost;
        public bool firearmsDisabled;
        public bool cooperativeDisabled;
        public bool journalScrambled;
    }

    public class FeralRegressionSystem
    {
        private const int MinChronicIllnessStage = 3;

        private readonly Dictionary<string, FeralRegressionState> _states = new Dictionary<string, FeralRegressionState>();

        public IReadOnlyDictionary<string, FeralRegressionState> States => _states;

        public event Action<string> OnFeralRegression;  // survivorId
        public event Action<string> OnSpeechLost;  // survivorId
        public event Action<string> OnFirearmsDisabled;  // survivorId

        private static readonly char[] GarbleChars = { 'x', 'z', 'k', 'r', 'g', 'h', 'n', 'q' };

        private FeralRegressionState GetOrCreate(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state))
            {
                state = new FeralRegressionState
                {
                    survivorId = survivorId,
                    isRegressed = false,
                    speechLost = false,
                    firearmsDisabled = false,
                    cooperativeDisabled = false,
                    journalScrambled = false
                };
                _states[survivorId] = state;
            }
            return state;
        }

        /// <summary>
        /// Afflicts survivor only if chronic illness is at stage 3 or higher.
        /// Returns true if affliction was applied.
        /// </summary>
        public bool Afflict(string survivorId, int chronicIllnessStage)
        {
            if (chronicIllnessStage < MinChronicIllnessStage)
                return false;

            var state = GetOrCreate(survivorId);
            if (state.isRegressed)
                return false;

            state.isRegressed = true;
            state.speechLost = true;
            state.firearmsDisabled = true;
            state.cooperativeDisabled = true;
            state.journalScrambled = true;

            OnFeralRegression?.Invoke(survivorId);
            OnSpeechLost?.Invoke(survivorId);
            OnFirearmsDisabled?.Invoke(survivorId);
            return true;
        }

        /// <summary>
        /// Garbles input text by replacing random characters with noise.
        /// The more regressed the survivor, the more text is scrambled.
        /// </summary>
        public string ScrambleText(string input, System.Random rng)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var sb = new StringBuilder(input);
            int scrambleCount = Math.Max(1, sb.Length / 3);

            for (int i = 0; i < scrambleCount; i++)
            {
                int idx = rng.Next(sb.Length);
                if (!char.IsWhiteSpace(sb[idx]))
                {
                    sb[idx] = GarbleChars[rng.Next(GarbleChars.Length)];
                }
            }
            return sb.ToString();
        }

        public bool CanUseFirearm(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state))
                return true;

            return !state.isRegressed || !state.firearmsDisabled;
        }

        public bool CanCooperate(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state))
                return true;

            return !state.isRegressed || !state.cooperativeDisabled;
        }
    }
}
