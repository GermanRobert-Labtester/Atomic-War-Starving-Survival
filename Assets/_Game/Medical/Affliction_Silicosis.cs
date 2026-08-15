using System;
using System.Collections.Generic;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Medical
{
    [Serializable]
    public class SilicosisState
    {
        public string survivorId;
        public int daysSinceExposure;
        public float lungFibrosisPercent; // 0..100%
        public bool chronicShortnessOfBreath;
    }

    [Serializable]
    public class SilicosisSystemSave
    {
        public string systemId = "affliction_silicosis";
        public List<SilicosisState> afflicted = new List<SilicosisState>();
    }

    /// <summary>
    /// Expansion V / Spec §3.2: Silicosis (Concrete-Dust Fibrosis).
    /// Chronic respiratory damage from excavating pulverized concrete without a working respirator.
    /// Progressively degrades maximum stamina and increases work fatigue.
    /// </summary>
    public class SilicosisSystem
    {
        private readonly Dictionary<string, SilicosisState> _afflicted = new Dictionary<string, SilicosisState>();

        public IReadOnlyDictionary<string, SilicosisState> Afflicted => _afflicted;

        public event Action<string, string> OnSilicosisContracted;
        public event Action<string> OnFibrosisProgressed;
        public event Action<string> OnSilicosisManaged;

        public void ContractSilicosis(string survivorId, string cause)
        {
            if (_afflicted.ContainsKey(survivorId)) return;

            var state = new SilicosisState
            {
                survivorId = survivorId,
                daysSinceExposure = 0,
                lungFibrosisPercent = 20f,
                chronicShortnessOfBreath = false
            };
            _afflicted[survivorId] = state;
            OnSilicosisContracted?.Invoke(survivorId, cause);
        }

        public void TickDay(string survivorId)
        {
            if (!_afflicted.TryGetValue(survivorId, out var state)) return;

            state.daysSinceExposure++;
            if (state.daysSinceExposure % 3 == 0)
            {
                state.lungFibrosisPercent = Math.Min(100f, state.lungFibrosisPercent + 5f);
                if (state.lungFibrosisPercent >= 50f && !state.chronicShortnessOfBreath)
                {
                    state.chronicShortnessOfBreath = true;
                    OnFibrosisProgressed?.Invoke(survivorId);
                }
            }
        }

        public float GetStaminaPenaltyMultiplier(string survivorId)
        {
            if (_afflicted.TryGetValue(survivorId, out var state))
            {
                return state.chronicShortnessOfBreath ? 0.65f : 0.85f;
            }
            return 1.0f;
        }

        public bool TryManageTherapy(string survivorId, bool hasInhalerOrNebulizer)
        {
            if (!hasInhalerOrNebulizer) return false;

            if (_afflicted.TryGetValue(survivorId, out var state))
            {
                state.lungFibrosisPercent = Math.Max(20f, state.lungFibrosisPercent - 15f);
                OnSilicosisManaged?.Invoke(survivorId);
                return true;
            }
            return false;
        }

        public SilicosisSystemSave CaptureState() => new SilicosisSystemSave
        {
            afflicted = SaveMap.Capture(_afflicted),
        };

        public void RestoreState(SilicosisSystemSave saved) =>
            SaveMap.Restore(_afflicted, saved?.afflicted, e => e.survivorId);
    }
}
