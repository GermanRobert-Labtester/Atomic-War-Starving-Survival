// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    [Serializable]
    public class SickBand
    {
        public string survivorId;
        public int band;
        public int diagnosedDay;
        public int releaseDay = -1;
        public string palliativePlan;

        /// <summary>
        /// Plan 60 / D5 — which fact produced <see cref="band">band</see>: a dose
        /// reading (<see cref="SickListSystem.SourceDose"/>) or an illness
        /// prognosis (<see cref="SickListSystem.SourceIllness"/>).
        /// </summary>
        public string severitySource;

        /// <summary>Origin id for provenance; it is not a progression input.</summary>
        public string sourceId;
    }

    [Serializable]
    public class SickListSystemState
    {
        public string systemId = SickListSystem.SystemId;
        public List<SickBand> bands = new List<SickBand>();
    }

    /// <summary>
    /// ASHFALL: THE DOSE — the named sick, by dose band, not by death.
    /// A Black-band survivor remains on the roster and receives care.
    /// </summary>
    public class SickListSystem
    {
        public const string SystemId = "sick_list_system";
        public const string SourceDose = "dose";
        public const string SourceIllness = "illness";

        private static readonly StringComparer IdentityComparer = StringComparer.OrdinalIgnoreCase;
        private readonly Dictionary<string, SickBand> _bands =
            new Dictionary<string, SickBand>(IdentityComparer);

        public event Action<string, int> OnDiagnosed;
        public event Action<string> OnReleased;
        public event Action<string, string> OnPalliativeAssigned;
        public event Action<SickListSystemState> OnStateChanged;

        public SickListSystemState State => CaptureState();
        public IReadOnlyList<SickBand> Bands => CaptureState().bands;

        /// <summary>Name a survivor into a dose band. Re-diagnosis moves the band and keeps one row.</summary>
        public bool Diagnose(string survivorId, int band, int day) =>
            Diagnose(survivorId, band, day, SourceDose, null);

        /// <summary>
        /// Name a survivor into the shared band ladder and record which authority
        /// produced the classification.
        /// </summary>
        public bool Diagnose(string survivorId, int band, int day, string severitySource, string sourceId)
        {
            string id = NormalizeId(survivorId);
            if (string.IsNullOrWhiteSpace(id) || !IsValidBand(band) || day < 0) return false;

            if (!_bands.TryGetValue(id, out var entry))
            {
                entry = new SickBand
                {
                    survivorId = id,
                    diagnosedDay = day,
                    releaseDay = -1,
                    palliativePlan = string.Empty,
                    severitySource = SourceDose,
                    sourceId = string.Empty
                };
                _bands[id] = entry;
            }

            entry.band = band;
            entry.releaseDay = -1;
            entry.severitySource = NormalizeSeveritySource(severitySource);
            entry.sourceId = NormalizeId(sourceId);
            OnDiagnosed?.Invoke(id, band);
            RaiseChanged();
            return true;
        }

        /// <summary>Release a survivor from the sick list while retaining the historical row.</summary>
        public bool Release(string survivorId, int day)
        {
            string id = NormalizeId(survivorId);
            if (!_bands.TryGetValue(id, out var entry) || day < entry.diagnosedDay) return false;
            entry.releaseDay = day;
            OnReleased?.Invoke(id);
            RaiseChanged();
            return true;
        }

        public bool AssignPalliative(string survivorId, string plan)
        {
            string id = NormalizeId(survivorId);
            string normalizedPlan = plan?.Trim() ?? string.Empty;
            if (!_bands.TryGetValue(id, out var entry) || string.IsNullOrWhiteSpace(normalizedPlan)) return false;
            entry.palliativePlan = normalizedPlan;
            OnPalliativeAssigned?.Invoke(id, normalizedPlan);
            RaiseChanged();
            return true;
        }

        public SickBand? GetBand(string survivorId)
        {
            string id = NormalizeId(survivorId);
            return _bands.TryGetValue(id, out var band) ? CloneBand(band) : null;
        }

        public SickListSystemState CaptureState()
        {
            var copy = new SickListSystemState { systemId = SystemId };
            var keys = new List<string>(_bands.Keys);
            keys.Sort(StringComparer.Ordinal);
            foreach (string key in keys)
                copy.bands.Add(CloneBand(_bands[key]));
            return copy;
        }

        public void RestoreState(SickListSystemState saved)
        {
            _bands.Clear();
            if (saved?.bands == null)
            {
                RaiseChanged();
                return;
            }

            foreach (var source in saved.bands)
            {
                if (source == null) continue;
                string id = NormalizeId(source.survivorId);
                if (string.IsNullOrWhiteSpace(id) || !IsValidBand(source.band)
                    || source.diagnosedDay < 0
                    || (source.releaseDay >= 0 && source.releaseDay < source.diagnosedDay)
                    || _bands.ContainsKey(id)) continue;

                var copy = new SickBand
                {
                    survivorId = id,
                    band = source.band,
                    diagnosedDay = source.diagnosedDay,
                    releaseDay = source.releaseDay < 0 ? -1 : source.releaseDay,
                    palliativePlan = source.palliativePlan?.Trim() ?? string.Empty,
                    severitySource = NormalizeSeveritySource(source.severitySource),
                    sourceId = NormalizeId(source.sourceId)
                };
                _bands[id] = copy;
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(CaptureState());

        private static SickBand CloneBand(SickBand source) => new SickBand
        {
            survivorId = NormalizeId(source.survivorId),
            band = source.band,
            diagnosedDay = source.diagnosedDay,
            releaseDay = source.releaseDay,
            palliativePlan = source.palliativePlan?.Trim() ?? string.Empty,
            severitySource = NormalizeSeveritySource(source.severitySource),
            sourceId = NormalizeId(source.sourceId)
        };

        private static bool IsValidBand(int band) =>
            band >= DoseLedgerSystem.BandGreen && band <= DoseLedgerSystem.BandBlack;

        private static string NormalizeSeveritySource(string? value)
        {
            string normalized = NormalizeId(value);
            return string.Equals(normalized, SourceIllness, StringComparison.OrdinalIgnoreCase)
                ? SourceIllness
                : SourceDose;
        }

        private static string NormalizeId(string? value) => value?.Trim() ?? string.Empty;
    }
}
