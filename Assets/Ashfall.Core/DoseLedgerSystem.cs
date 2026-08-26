using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    // ── Read models ─────────────────────────────────────────────────

    /// <summary>One booked dose reading against a named survivor.</summary>
    [Serializable]
    public class DoseReading
    {
        public int day;
        public string source;              // event id or freeform cause
        public float nominalMsv;           // what the dial showed
        public float bookedMsv;            // what was written after flux/shielding/anti-rad
        public bool fluxAmbiguous;         // the reading was a range, not a point
        public bool antiRadAfter;          // booked dose reduced post-exposure
    }

    [Serializable]
    public class DoseEntry
    {
        public string survivorId;
        public float baselineMsv;          // inherited, never zeroed
        public float cumulativeMsv;
        public string assignedDosimeterTag; // null/empty = not booked going forward
        public List<DoseReading> readingsHistory = new List<DoseReading>();
        public int radiationPhaseCaught;   // the phase index when the ledger caught it
        public float shieldingFactor = 1f;
        public int lastAntiRadDay = -1;
    }

    [Serializable]
    public class DoseLedgerSystemState
    {
        public string systemId = DoseLedgerSystem.SystemId;
        public List<DoseEntry> entries = new List<DoseEntry>();
        public float ceilingMsv = 600f;        // the Black threshold
        public int readingsSinceLastCalibration;
        public bool calibrationOverdue;
    }

    // ── System ──────────────────────────────────────────────────────

    /// <summary>
    /// ASHFALL: THE DOSE — per-survivor cumulative dose as a kept document.
    /// Ports the dose *record*, not the radiation physics (which lives in
    /// RadiationSystem). Readings are only booked against survivors with an
    /// assigned dosimeter tag; unbooked rads are the shelter's silence.
    /// </summary>
    public class DoseLedgerSystem
    {
        public const string SystemId = "dose_ledger_system";
        public const float AmberMsv = 100f;
        public const float RedMsv = 300f;
        public const float BlackMsv = 600f;
        public const int ReadingsPerCalibration = 40;

        // Heat-band thresholds exposed for the Sick List / UI to reuse.
        public const int BandGreen = 0;
        public const int BandAmber = 1;
        public const int BandRed = 2;
        public const int BandBlack = 3;

        private readonly DoseLedgerSystemState _state = new DoseLedgerSystemState();
        private readonly Dictionary<string, DoseEntry> _entries = new Dictionary<string, DoseEntry>();

        public event Action<string, float> OnDoseCorrected;       // survivorId, bookedMsv
        public event Action<string, int> OnBandReached;           // survivorId, band
        public event Action OnLedgerCalibrated;
        public event Action<DoseLedgerSystemState> OnStateChanged;

        public DoseLedgerSystemState State => _state;
        public IReadOnlyList<DoseEntry> Entries => _state.entries;

        // ── Assignment ──────────────────────────────────────────────

        /// <summary>Tag a survivor so their future exposures can be booked.</summary>
        public bool AssignDosimeter(string survivorId, string tag, float baselineMsv = 0f)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(tag)) return false;
            var entry = GetOrCreate(survivorId);
            if (entry.baselineMsv <= 0f) entry.baselineMsv = Math.Max(0f, baselineMsv);
            entry.cumulativeMsv = Math.Max(entry.cumulativeMsv, entry.baselineMsv);
            entry.assignedDosimeterTag = tag;
            RaiseChanged();
            return true;
        }

        public void SetShieldingFactor(string survivorId, float factor)
        {
            var e = GetOrCreate(survivorId);
            e.shieldingFactor = factor > 0f ? factor : 1f;
            RaiseChanged();
        }

        /// <summary>Refund calibration accuracy after the configured reading count.</summary>
        public void Calibrate(string survivorId, int day)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            var e = GetOrCreate(survivorId);
            _state.readingsSinceLastCalibration = 0;
            _state.calibrationOverdue = false;
            OnLedgerCalibrated?.Invoke();
            RaiseChanged();
        }

        // ── Book a reading ──────────────────────────────────────────

        public DoseBandResult BookReading(
            string survivorId,
            int day,
            float nominalMsv,
            string source,
            bool highEnergyEvent,
            bool antiRadBefore,
            bool antiRadAfter,
            ISeededRng rng)
        {
            if (string.IsNullOrEmpty(survivorId) || nominalMsv <= 0f) return DoseBandResult.NoEntry;
            var entry = GetOrCreate(survivorId);
            if (entry == null) return DoseBandResult.NoEntry;
            if (string.IsNullOrEmpty(entry.assignedDosimeterTag)) return DoseBandResult.NoEntry;

            // Flux ambiguity: a high-energy event makes the dial a range, not a point.
            bool fluxAmbiguous = highEnergyEvent && rng != null;
            float ratio = 1f;
            if (fluxAmbiguous && rng != null) ratio = 0.85f + rng.NextFloat() * 0.30f;

            // Pre-exposure anti-rad attenuates the incoming dose.
            float incoming = antiRadBefore ? nominalMsv * 0.5f : nominalMsv;
            // Shielding attenuates what reaches the body.
            incoming *= entry.shieldingFactor;
            // Post-exposure anti-rad reduces what is booked.
            float booked = antiRadAfter ? incoming * 0.6f : incoming;
            if (fluxAmbiguous) booked *= ratio;

            var reading = new DoseReading
            {
                day = day,
                source = source ?? string.Empty,
                nominalMsv = nominalMsv,
                bookedMsv = booked,
                fluxAmbiguous = fluxAmbiguous,
                antiRadAfter = antiRadAfter
            };
            entry.readingsHistory.Add(reading);
            if (antiRadAfter || antiRadBefore) entry.lastAntiRadDay = day;

            float before = entry.cumulativeMsv;
            entry.cumulativeMsv += booked;

            _state.readingsSinceLastCalibration++;
            if (_state.readingsSinceLastCalibration >= ReadingsPerCalibration)
                _state.calibrationOverdue = true;

            OnDoseCorrected?.Invoke(survivorId, booked);

            int bandBefore = BandFor(before);
            int bandAfter = BandFor(entry.cumulativeMsv);
            if (bandAfter > bandBefore)
                OnBandReached?.Invoke(survivorId, bandAfter);

            RaiseChanged();
            return BandForValue(entry.cumulativeMsv);
        }

        /// <summary>The band label for a cumulative total.</summary>
        public static int BandFor(float mSv)
        {
            if (mSv >= BlackMsv) return BandBlack;
            if (mSv >= RedMsv) return BandRed;
            if (mSv >= AmberMsv) return BandAmber;
            return BandGreen;
        }

        private static DoseBandResult BandForValue(float mSv)
        {
            return (DoseBandResult)BandFor(mSv);
        }

        // ── Queries ────────────────────────────────────────────────

        public DoseEntry? GetEntry(string survivorId) =>
            _entries.TryGetValue(survivorId, out var e) ? e : null;

        public float GetCumulative(string survivorId) =>
            _entries.TryGetValue(survivorId, out var e) ? e.cumulativeMsv : 0f;

        // ── Save / Load ─────────────────────────────────────────────

        public DoseLedgerSystemState CaptureState()
        {
            // Fresh copy, ordinal-ordered: never return the live state to the
            // envelope (aliasing), and dictionary iteration order is not a
            // cross-host guarantee, so entries are emitted sorted by survivor id.
            var copy = new DoseLedgerSystemState
            {
                systemId = _state.systemId,
                ceilingMsv = _state.ceilingMsv,
                readingsSinceLastCalibration = _state.readingsSinceLastCalibration,
                calibrationOverdue = _state.calibrationOverdue
            };
            var keys = new List<string>(_entries.Count);
            foreach (var kv in _entries) keys.Add(kv.Key);
            keys.Sort(string.CompareOrdinal);
            for (int i = 0; i < keys.Count; i++)
            {
                var e = _entries[keys[i]];
                copy.entries.Add(new DoseEntry
                {
                    survivorId = e.survivorId,
                    baselineMsv = e.baselineMsv,
                    cumulativeMsv = e.cumulativeMsv,
                    assignedDosimeterTag = e.assignedDosimeterTag,
                    shieldingFactor = e.shieldingFactor,
                    lastAntiRadDay = e.lastAntiRadDay,
                    radiationPhaseCaught = e.radiationPhaseCaught,
                    readingsHistory = new List<DoseReading>(e.readingsHistory)
                });
            }
            return copy;
        }

        public void RestoreState(DoseLedgerSystemState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.ceilingMsv = saved.ceilingMsv;
            _state.readingsSinceLastCalibration = saved.readingsSinceLastCalibration;
            _state.calibrationOverdue = saved.calibrationOverdue;
            _entries.Clear();
            _state.entries.Clear();
            if (saved.entries != null)
            {
                foreach (var e in saved.entries)
                {
                    if (e == null || string.IsNullOrEmpty(e.survivorId)) continue;
                    var copy = new DoseEntry
                    {
                        survivorId = e.survivorId,
                        baselineMsv = e.baselineMsv,
                        cumulativeMsv = e.cumulativeMsv,
                        assignedDosimeterTag = e.assignedDosimeterTag,
                        shieldingFactor = e.shieldingFactor,
                        lastAntiRadDay = e.lastAntiRadDay,
                        radiationPhaseCaught = e.radiationPhaseCaught,
                        readingsHistory = e.readingsHistory != null
                            ? new List<DoseReading>(e.readingsHistory)
                            : new List<DoseReading>()
                    };
                    _entries[e.survivorId] = copy;
                    _state.entries.Add(copy);
                }
            }
            RaiseChanged();
        }

        private DoseEntry GetOrCreate(string survivorId)
        {
            if (_entries.TryGetValue(survivorId, out var existing)) return existing;
            var entry = new DoseEntry { survivorId = survivorId };
            _entries[survivorId] = entry;
            _state.entries.Add(entry);
            return entry;
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }

    /// <summary>Mirror of the band enum for BookReading return values.</summary>
    public enum DoseBandResult
    {
        NoEntry = -1,
        Green = 0,
        Amber = 1,
        Red = 2,
        Black = 3
    }
}