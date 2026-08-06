using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AshTongueTraitState
    {
        public string traitId = "trait_ash_tongue";
        public string survivorId;
        public bool isNativeSpeaker;
        public float comprehension; // 0-1
        public Dictionary<string, string> dialectWords = new Dictionary<string, string>();
    }

    /// <summary>
    /// Prompt #846: Language Drift — Children born in bunker / Feral Orphans
    /// develop their own slang. OldWorld survivors suffer an Affinity penalty
    /// when communicating with native Ash Tongue speakers.
    /// </summary>
    public class Trait_AshTongue
    {
        private readonly Dictionary<string, AshTongueTraitState> _states =
            new Dictionary<string, AshTongueTraitState>();

        private const float NativeComprehension = 1.0f;
        private const float OldWorldComprehension = 0.6f;
        private const float AffinityPenaltyPerMiscommunication = -0.05f;

        public event Action<string, bool> OnTraitAssigned;             // survivorId, nativeSpeaker
        public event Action<string, string> OnMiscommunication;        // speakerId, listenerId
        public event Action<string, string, float> OnAffinityPenalty;  // speakerId, listenerId, amount
        public event Action<string, string> OnDialectWordCoined;       // word, meaning

        public IReadOnlyDictionary<string, AshTongueTraitState> States => _states;

        /// <summary>
        /// Assigns the Ash Tongue trait to a survivor.
        /// Native speakers are bunker-born or feral orphans with full dialect comprehension.
        /// </summary>
        public void AssignTrait(string survivorId, bool bornInBunker)
        {
            var state = new AshTongueTraitState
            {
                survivorId = survivorId,
                isNativeSpeaker = bornInBunker,
                comprehension = bornInBunker ? NativeComprehension : 0f,
                dialectWords = new Dictionary<string, string>()
            };
            _states[survivorId] = state;

            OnTraitAssigned?.Invoke(survivorId, bornInBunker);
        }

        /// <summary>
        /// Returns the comprehension level (0-1) when a listener hears an Ash Tongue speaker.
        /// Native-to-native = full. OldWorld listener = reduced.
        /// </summary>
        public float GetComprehensionLevel(string listenerId, bool listenerIsOldWorld)
        {
            if (!_states.ContainsKey(listenerId))
                return listenerIsOldWorld ? OldWorldComprehension : NativeComprehension;

            var listenerState = _states[listenerId];
            if (listenerState.isNativeSpeaker)
                return NativeComprehension;

            return listenerIsOldWorld ? OldWorldComprehension : listenerState.comprehension;
        }

        /// <summary>
        /// Returns the affinity penalty applied when speaker and listener miscommunicate.
        /// Fires the miscommunication and penalty events.
        /// </summary>
        public float GetAffinityPenalty(string speakerId, string listenerId)
        {
            bool speakerIsNative = _states.ContainsKey(speakerId) && _states[speakerId].isNativeSpeaker;
            bool listenerIsNative = _states.ContainsKey(listenerId) && _states[listenerId].isNativeSpeaker;

            // No penalty if both are native speakers
            if (speakerIsNative && listenerIsNative) return 0f;

            // No penalty if neither is a native speaker (both OldWorld)
            if (!speakerIsNative && !listenerIsNative) return 0f;

            // Mixed communication — apply penalty
            OnMiscommunication?.Invoke(speakerId, listenerId);
            OnAffinityPenalty?.Invoke(speakerId, listenerId, AffinityPenaltyPerMiscommunication);

            return AffinityPenaltyPerMiscommunication;
        }

        /// <summary>
        /// Translates an OldWorld phrase into Ash Tongue dialect using the
        /// speaker's dialect map. Returns original phrase if no mapping found.
        /// </summary>
        public string TranslatePhrase(string speakerId, string oldWorldPhrase)
        {
            if (!_states.ContainsKey(speakerId)) return oldWorldPhrase;

            var dialect = _states[speakerId].dialectWords;
            if (dialect.TryGetValue(oldWorldPhrase, out string translated))
                return translated;

            return oldWorldPhrase;
        }

        /// <summary>
        /// Adds a new dialect word mapping for a survivor.
        /// </summary>
        public void CoinDialectWord(string survivorId, string oldWord, string newWord)
        {
            if (!_states.ContainsKey(survivorId)) return;

            _states[survivorId].dialectWords[oldWord] = newWord;
            OnDialectWordCoined?.Invoke(newWord, oldWord);
        }

        /// <summary>
        /// Captures all trait states for save.
        /// </summary>
        public Dictionary<string, AshTongueTraitState> CaptureState() =>
            new Dictionary<string, AshTongueTraitState>(_states);

        /// <summary>
        /// Restores trait states from save.
        /// </summary>
        public void RestoreState(Dictionary<string, AshTongueTraitState> states)
        {
            _states.Clear();
            foreach (var kvp in states)
                _states[kvp.Key] = kvp.Value;
        }
    }
}
