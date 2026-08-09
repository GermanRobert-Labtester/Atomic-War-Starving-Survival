using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PianistState
    {
        public string id = "encounter_pianist";
        public string displayName = "The Pianist";
        public bool hasBeenMet = false;
        public bool hasReceivedFood = false;
        public bool hasBeenToldAboutBunker = false;
        public int timesVisited = 0;
    }

    /// <summary>
    /// Prompt #904: Encounter — The Pianist.
    /// Every evening at the same hour, someone plays a piano in a ruined
    /// conservatory on the east edge of town. The building has no roof.
    /// The piano is out of tune. The pianist is an old man named Matej
    /// who has been blind since the flash. He plays from muscle memory.
    ///
    /// He asks for nothing. He is not a recruitable survivor, not a
    /// trader, not a threat. He is just a man who has decided that as
    /// long as he can still play, the world has not completely ended.
    ///
    /// The survivor can:
    ///   - Sit and listen (small morale boost each visit, diminishing)
    ///   - Share food (larger one-time morale boost, he remembers)
    ///   - Tell him about the bunker (he nods, never comes, but someone
    ///     else might — small chance a refugee shows up later)
    ///   - Take the piano wire (3 wire_cutter items, destroys the piano,
    ///     heavy morale penalty, encounter removed permanently)
    ///
    /// He is still there the next time you pass. He is always there.
    /// Until he isn't.
    /// </summary>
    public class Encounter_Pianist
    {
        private static System.Random _fallbackRng;
        private static System.Random FallbackRng =>
            _fallbackRng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("encounter_pianist");

        private PianistState _state = new PianistState();

        /// <summary>His name. Always the same.</summary>
        public const string PianistName = "Matej";

        /// <summary>Morale boost for listening (diminishes with repeat visits).</summary>
        public const float BaseListenMoraleBoost = 8f;

        /// <summary>How much the boost decays per previous visit.</summary>
        public const float ListenDecayPerVisit = 2f;

        /// <summary>Minimum listen boost (never goes below this).</summary>
        public const float MinListenBoost = 2f;

        /// <summary>Morale boost for sharing food.</summary>
        public const float ShareFoodMoraleBoost = 12f;

        /// <summary>Morale penalty for destroying the piano.</summary>
        public const float DestroyPianoMoralePenalty = 30f;

        /// <summary>Wire cutter yield from piano wire.</summary>
        public const int WireCutterYield = 3;

        /// <summary>Wire cutter item id.</summary>
        public const string WireCutterItemId = "wire_cutters";

        /// <summary>World flag set when the player tells him about the bunker.</summary>
        public const string ToldAboutBunkerFlag = "pianist_told_about_bunker";

        /// <summary>Chance a refugee appears (per day) after telling him.</summary>
        public const float RefugeeChancePerDay = 0.02f;

        // ── Events ──────────────────────────────────────────────────────
        public event Action<PianistState> OnMetMatej;
        public event Action<PianistState, float> OnListened;        // state, moraleBoost
        public event Action<PianistState> OnSharedFood;
        public event Action<PianistState> OnToldAboutBunker;
        public event Action<PianistState> OnPianoDestroyed;

        public PianistState State => _state;

        // ── Public API ──────────────────────────────────────────────────

        /// <summary>First meeting. Fires the introduction event.</summary>
        public void Meet()
        {
            if (!_state.hasBeenMet)
            {
                _state.hasBeenMet = true;
                OnMetMatej?.Invoke(_state);
            }
        }

        /// <summary>
        /// Sit and listen to him play. Returns the morale boost.
        /// Diminishes with repeat visits but never reaches zero.
        /// </summary>
        public float Listen()
        {
            float boost = Mathf.Max(
                MinListenBoost,
                BaseListenMoraleBoost - (_state.timesVisited * ListenDecayPerVisit));
            _state.timesVisited++;
            OnListened?.Invoke(_state, boost);
            return boost;
        }

        /// <summary>
        /// Share some of your food with him. Large one-time morale boost.
        /// He will remember.
        /// </summary>
        public float ShareFood()
        {
            if (_state.hasReceivedFood) return 0f;
            _state.hasReceivedFood = true;
            OnSharedFood?.Invoke(_state);
            return ShareFoodMoraleBoost;
        }

        /// <summary>
        /// Tell Matej about the bunker. He won't come, but word spreads.
        /// Sets a flag that gives a small daily chance of a refugee.
        /// </summary>
        public bool TellAboutBunker()
        {
            if (_state.hasBeenToldAboutBunker) return false;
            _state.hasBeenToldAboutBunker = true;
            OnToldAboutBunker?.Invoke(_state);
            return true;
        }

        /// <summary>
        /// Strip the piano for wire. Destroys the encounter permanently.
        /// Heavy morale penalty. He doesn't stop you. He can't see you.
        /// But he hears the strings snap.
        /// </summary>
        public int DestroyPiano()
        {
            OnPianoDestroyed?.Invoke(_state);
            return WireCutterYield;
        }

        /// <summary>Check if a refugee should appear today.</summary>
        public bool RollRefugeeArrival(System.Random rng = null)
        {
            if (!_state.hasBeenToldAboutBunker) return false;
            rng ??= FallbackRng;
            return rng.NextDouble() < RefugeeChancePerDay;
        }

        // ── Flavor text ─────────────────────────────────────────────────

        /// <summary>What Matej is playing today.</summary>
        public static string GetNowPlaying(System.Random rng = null)
        {
            rng ??= FallbackRng;
            string[] pieces =
            {
                "something that might be Chopin, or might just be the wind",
                "a lullaby his mother sang. The words are gone but the melody survived",
                "a folk song from a country that no longer exists on any map",
                "scales. Just scales, up and down, because the routine matters more than the music",
                "a wedding march. He plays it every day at the same hour. No one has been married here in two years",
                "something he is composing as he goes. It has no name. It probably never will",
                "a children's song. He doesn't know any children anymore, but the song is still there",
            };
            return pieces[rng.Next(pieces.Length)];
        }

        // ── Save / Load ─────────────────────────────────────────────────

        public PianistState CaptureState() => _state;

        public void RestoreState(PianistState save)
        {
            if (save != null) _state = save;
        }
    }
}
