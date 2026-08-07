using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class NumbersStationState
    {
        public string eventId = "event_numbers_station";
        public string displayName = "Numbers Station";
        public List<int> numberSequence = new List<int>();
        public bool isDecoded = false;
        public bool cipherBookRequired = true;
        public string unlockedNodeId = string.Empty;
    }

    /// <summary>
    /// Prompt #632: Event: Numbers Station.
    /// A creepy looping number sequence on the radio. The player must match it
    /// against the CipherBook artifact to unlock a legendary pre-war map node.
    /// </summary>
    public class Event_NumbersStation
    {
        private NumbersStationState _state = new NumbersStationState();

        public event Action<NumbersStationState, List<int>> OnSequenceGenerated;
        public event Action<NumbersStationState, bool> OnDecodeAttempt;
        public event Action<NumbersStationState, string> OnMapNodeUnlocked;

        public NumbersStationState State => _state;

        public void GenerateSequence(System.Random rng, int length)
        {
            _state.numberSequence.Clear();
            for (int i = 0; i < length; i++)
            {
                _state.numberSequence.Add(rng.Next(0, 10));
            }
            _state.isDecoded = false;
            OnSequenceGenerated?.Invoke(_state, new List<int>(_state.numberSequence));
        }

        public bool TryMatch(int[] playerInput)
        {
            if (_state.isDecoded) return true;
            if (playerInput == null) return false;

            if (playerInput.Length != _state.numberSequence.Count)
            {
                OnDecodeAttempt?.Invoke(_state, false);
                return false;
            }

            for (int i = 0; i < _state.numberSequence.Count; i++)
            {
                if (playerInput[i] != _state.numberSequence[i])
                {
                    OnDecodeAttempt?.Invoke(_state, false);
                    return false;
                }
            }

            _state.isDecoded = true;
            OnDecodeAttempt?.Invoke(_state, true);
            OnMapNodeUnlocked?.Invoke(_state, _state.unlockedNodeId);
            return true;
        }

        public bool IsDecoded()
        {
            return _state.isDecoded;
        }

        public NumbersStationState CaptureState()
        {
            return _state;
        }

        public void RestoreState(NumbersStationState saved)
        {
            _state = saved ?? new NumbersStationState();
            if (_state.numberSequence == null)
                _state.numberSequence = new System.Collections.Generic.List<int>();
        }
    }
}
