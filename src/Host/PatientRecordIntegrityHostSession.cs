// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Patient Record Integrity — host adapter over the sealed
// Ashfall.Core.Medical.PatientRecordIntegrityValidator and the ALREADY-LIVE
// medical pipeline state.
//
// The validator reports only. No record is created, mutated, or repaired here:
// a dangling survivor, treatment, or item reference is surfaced, never guessed.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Runs the authored clinical-record integrity rules across the live medical
    /// pipeline and exposes the findings. Read-only by construction: the validator
    /// takes no write path, so a finding is the whole outcome.
    /// </summary>
    public sealed class PatientRecordIntegrityHostSession : HostSessionBase
    {
        private readonly Func<MedicalPipelineSaveState?> _pipelineProvider;
        private readonly Func<string, bool> _isKnownSurvivor;
        private readonly Func<string, bool> _isTreatmentEligible;
        private readonly Func<string, bool> _isKnownItem;

        private List<MedicalPipelineIntegrityFinding> _findings = new List<MedicalPipelineIntegrityFinding>();

        public string LastEvent { get; private set; } = string.Empty;
        public IReadOnlyList<MedicalPipelineIntegrityFinding> Findings => _findings;
        public int FindingCount => _findings.Count;
        public int FatalCount => _findings.Count(f => f != null && f.fatal);
        public bool IsClean => _findings.Count == 0;
        public int ValidationCount { get; private set; }

        public PatientRecordIntegrityHostSession(
            Func<MedicalPipelineSaveState?> pipelineProvider,
            Func<string, bool>? isKnownSurvivor = null,
            Func<string, bool>? isTreatmentEligible = null,
            Func<string, bool>? isKnownItem = null)
        {
            _pipelineProvider = pipelineProvider ?? throw new ArgumentNullException(nameof(pipelineProvider));
            _isKnownSurvivor = isKnownSurvivor ?? (_ => true);
            _isTreatmentEligible = isTreatmentEligible ?? (_ => true);
            _isKnownItem = isKnownItem ?? (_ => true);
        }

        /// <summary>
        /// Validates the live pipeline state. Repeat validation is idempotent: the
        /// same state yields the same ordered findings and mutates nothing.
        /// </summary>
        public IReadOnlyList<MedicalPipelineIntegrityFinding> Validate()
        {
            var state = _pipelineProvider();
            if (state == null)
            {
                _findings = new List<MedicalPipelineIntegrityFinding>();
                LastEvent = "No medical pipeline state bound; nothing to validate.";
                RaiseStateChanged();
                return _findings;
            }

            var context = new PatientRecordIntegrityValidator.Context
            {
                IsKnownSurvivor = _isKnownSurvivor,
                IsTreatmentEligible = _isTreatmentEligible,
                IsKnownItem = _isKnownItem,
            };

            var results = PatientRecordIntegrityValidator.Validate(state, context)
                ?? new List<MedicalPipelineIntegrityFinding>();
            // Deterministic order so findings can be diffed run to run.
            results.Sort((a, b) =>
            {
                int byCode = string.CompareOrdinal(a?.code ?? string.Empty, b?.code ?? string.Empty);
                if (byCode != 0) return byCode;
                return string.CompareOrdinal(a?.detail ?? string.Empty, b?.detail ?? string.Empty);
            });
            _findings = results;
            ValidationCount++;
            LastEvent = _findings.Count == 0
                ? "Clinical records clean."
                : $"Clinical record integrity: {_findings.Count} finding(s), {FatalCount} fatal.";
            RaiseStateChanged();
            return _findings;
        }

        /// <summary>Comma-joined finding codes, for logs and probes.</summary>
        public string FindingCodes()
        {
            if (_findings.Count == 0) return "none";
            var codes = new List<string>(_findings.Count);
            foreach (var f in _findings) codes.Add(f?.code ?? "?");
            codes.Sort(StringComparer.Ordinal);
            return string.Join(",", codes);
        }

        public bool HasCode(string code)
        {
            foreach (var f in _findings)
                if (f != null && string.Equals(f.code, code, StringComparison.Ordinal)) return true;
            return false;
        }

        /// <summary>Truthful projection for a status readout.</summary>
        public string StatusLine() =>
            $"clinical integrity: {(IsClean ? "clean" : $"{FindingCount} finding(s)")}";

        public void Clear()
        {
            _findings = new List<MedicalPipelineIntegrityFinding>();
            LastEvent = "Clinical integrity findings cleared.";
            RaiseStateChanged();
        }
    }
}
