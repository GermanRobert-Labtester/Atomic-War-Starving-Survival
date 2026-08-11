using System;
using System.Collections.Generic;

using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Medical
{
    [Serializable]
    public class RadiationBlindnessState
    {
        public string survivorId;
        public bool isBlind;
        public float visionRadius = 1.0f;
    }

    
    [Serializable]
    public class RadiationBlindnessSystemSave
    {
        public string systemId = "affliction_radiation_blindness";

        public List<RadiationBlindnessState> blind = new List<RadiationBlindnessState>();
    }
public class RadiationBlindnessSystem
    {
        private const float Stage4RadiationThreshold = 80f;
        private const float BlindVisionRadius = 0.05f;

        private readonly Dictionary<string, RadiationBlindnessState> _blind = new Dictionary<string, RadiationBlindnessState>();

        public IReadOnlyDictionary<string, RadiationBlindnessState> Blind => _blind;

        public event Action<string> OnBlindnessAfflicted;  // survivorId
        public event Action<string> OnVisionRestored;       // survivorId

        public bool AfflictBlindness(string survivorId, float lifetimeRadiation)
        {
            if (lifetimeRadiation < Stage4RadiationThreshold)
                return false;

            if (_blind.ContainsKey(survivorId))
                return false;

            var state = new RadiationBlindnessState
            {
                survivorId = survivorId,
                isBlind = true,
                visionRadius = BlindVisionRadius
            };
            _blind[survivorId] = state;
            OnBlindnessAfflicted?.Invoke(survivorId);
            return true;
        }

        public RadiationBlindnessState GetVisionState(string survivorId)
        {
            _blind.TryGetValue(survivorId, out var state);
            return state;
        }

        public bool IsScreenBlackoutActive(string survivorId)
        {
            return _blind.TryGetValue(survivorId, out var state) && state.isBlind;
        }

        public void RestoreVision(string survivorId)
        {
            if (!_blind.Remove(survivorId))
                return;

            OnVisionRestored?.Invoke(survivorId);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public RadiationBlindnessSystemSave CaptureState() => new RadiationBlindnessSystemSave
        {
            blind = SaveMap.Capture(_blind),
        };

        public void RestoreState(RadiationBlindnessSystemSave saved) =>
            SaveMap.Restore(_blind, saved?.blind, e => e.survivorId);

}
}
