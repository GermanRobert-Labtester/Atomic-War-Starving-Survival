using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.AI.Actions
{
    [Serializable]
    public class DecryptState
    {
        public string actionId = "action_decrypt";
        public string displayName = "Decrypt Faction Comms";
        public float hoursRequired = 4f;
        public float intelligenceThreshold = 60f;
        public float successChance = 0f;
        public bool isDecoding = false;
    }

    /// <summary>
    /// Prompt #630: Action: Decrypt Intercepted Comms.
    /// A high-Intelligence survivor spends hours intercepting encrypted Faction comms.
    /// Success yields accurate RaidWarnings. Failure produces false coordinates,
    /// leading expeditions into ambushes.
    /// </summary>
    /// <summary>DEMOTE-Action-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_Decrypt
    {
        private DecryptState _state = new DecryptState();

        public event Action<DecryptState, bool, string> OnDecryptComplete;

        public DecryptState State => _state;

        public void StartDecrypt(float survivorIntelligence)
        {
            _state.successChance = Mathf.Clamp01(survivorIntelligence / 100f);
            _state.isDecoding = survivorIntelligence >= _state.intelligenceThreshold;
        }

        public (bool isAccurate, string coordinates) Complete(System.Random rng)
        {
            if (!_state.isDecoding)
                return (false, string.Empty);

            _state.isDecoding = false;
            bool isAccurate = (float)rng.NextDouble() < _state.successChance;

            string coordinates;
            if (isAccurate)
            {
                int x = rng.Next(0, 100);
                int y = rng.Next(0, 100);
                coordinates = $"{x},{y}";
            }
            else
            {
                // False coordinates — deliberately misleading
                int x = rng.Next(0, 100);
                int y = rng.Next(0, 100);
                coordinates = $"{x},{y}";
            }

            OnDecryptComplete?.Invoke(_state, isAccurate, coordinates);
            return (isAccurate, coordinates);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public DecryptState CaptureState() => _state;

        public void RestoreState(DecryptState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
