using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    [Serializable]
    public class SickBand
    {
        public string survivorId;
        public int band;              // DoseLedgerSystem.BandGreen..Black
        public int diagnosedDay;
        public int releaseDay = -1;
        public string palliativePlan; // empty = none assigned
    }

    [Serializable]
    public class SickListSystemState
    {
        public string systemId = SickListSystem.SystemId;
        public List<SickBand> bands = new List<SickBand>();
    }

    /// <summary>
    /// ASHFALL: THE DOSE — the named sick, by dose band, not by death.
    /// A Black-band survivor is not removed; they are named, cared for, or
    /// abandoned, and the ledger remembers which.
    /// </summary>
    public class SickListSystem
    {
        public const string SystemId = "sick_list_system";

        private readonly SickListSystemState _state = new SickListSystemState();
        private readonly Dictionary<string, SickBand> _bands = new Dictionary<string, SickBand>();

        public event Action<string, int> OnDiagnosed;   // survivorId, band
        public event Action<string> OnReleased;         // survivorId
        public event Action<string, string> OnPalliativeAssigned; // survivorId, plan
        public event Action<SickListSystemState> OnStateChanged;

        public SickListSystemState State => _state;
        public IReadOnlyList<SickBand> Bands => _state.bands;

        /// <summary>Name a survivor into a dose band. Re-diagnosis moves the band; history is kept.</summary>
        public bool Diagnose(string survivorId, int band, int day)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            var entry = _bands.TryGetValue(survivorId, out var existing)
                ? existing
                : new SickBand { survivorId = survivorId, diagnosedDay = day };
            if (!_bands.ContainsKey(survivorId))
                _state.bands.Add(entry);
            _bands[survivorId] = entry;
            entry.band = band;
            entry.releaseDay = -1;
            OnDiagnosed?.Invoke(survivorId, band);
            RaiseChanged();
            return true;
        }

        /// <summary>Release a survivor from the sick list (e.g. recovering band). Keeps the row.</summary>
        public bool Release(string survivorId, int day)
        {
            if (!_bands.TryGetValue(survivorId, out var entry)) return false;
            entry.releaseDay = day;
            OnReleased?.Invoke(survivorId);
            RaiseChanged();
            return true;
        }

        public bool AssignPalliative(string survivorId, string plan)
        {
            if (!_bands.TryGetValue(survivorId, out var entry)) return false;
            if (string.IsNullOrEmpty(plan)) return false;
            entry.palliativePlan = plan;
            OnPalliativeAssigned?.Invoke(survivorId, plan);
            RaiseChanged();
            return true;
        }

        public SickBand GetBand(string survivorId) =>
            _bands.TryGetValue(survivorId, out var b) ? b : null;

        public SickListSystemState CaptureState()
        {
            // Fresh copy, ordinal-ordered: never return the live state to the
            // envelope (aliasing), and dictionary iteration order is not a
            // cross-host guarantee.
            var copy = new SickListSystemState { systemId = _state.systemId };
            var keys = new List<string>(_bands.Count);
            foreach (var kv in _bands) keys.Add(kv.Key);
            keys.Sort(string.CompareOrdinal);
            for (int i = 0; i < keys.Count; i++)
            {
                var b = _bands[keys[i]];
                copy.bands.Add(new SickBand
                {
                    survivorId = b.survivorId,
                    band = b.band,
                    diagnosedDay = b.diagnosedDay,
                    releaseDay = b.releaseDay,
                    palliativePlan = b.palliativePlan
                });
            }
            return copy;
        }

        public void RestoreState(SickListSystemState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _bands.Clear();
            _state.bands.Clear();
            if (saved.bands != null)
            {
                foreach (var b in saved.bands)
                {
                    if (b == null || string.IsNullOrEmpty(b.survivorId)) continue;
                    var copy = new SickBand
                    {
                        survivorId = b.survivorId,
                        band = b.band,
                        diagnosedDay = b.diagnosedDay,
                        releaseDay = b.releaseDay,
                        palliativePlan = b.palliativePlan
                    };
                    _bands[b.survivorId] = copy;
                    _state.bands.Add(copy);
                }
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}