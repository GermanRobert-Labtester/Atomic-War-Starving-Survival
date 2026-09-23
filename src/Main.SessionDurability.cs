// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 39 / C2[16] — Session durability, soak sampling & release-gate read model.
//
// SaveSlotService + SaveLoadHostSession own slot truth, envelopes, quarantine
// and recovery. This partial wires the Core SessionDurabilityManager as the
// audit layer over that pipeline:
//   Save     — mirror the canonical save result (slot summary + checksum).
//   Tick     — one measured soak sample per campaign day advance.
//   Load     — audit a failed canonical load; never take the recovery decision.
//   Surface  — journal corruption/recovery facts and a stability verdict.
// ============================================================================
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private SessionDurabilityHostSession? _sessionDurability;
        private bool _sessionDurabilityDirty;
        private bool _lastSoakStable = true;

        public SessionDurabilityHostSession? SessionDurability => _sessionDurability;

        private SessionDurabilityHostSession EnsureSessionDurability()
        {
            if (_sessionDurability != null) return _sessionDurability;

            var loaded = SessionDurabilitySaveStore.TryLoad();
            var manager = new SessionDurabilityManager(loaded);

            manager.CorruptionDetectedSeam = (slotId, reason) =>
            {
                _journal?.TryAddRawEntry(
                    "session_corruption",
                    $"Save slot {slotId} failed verification: {reason}. The canonical save service will offer its backup for recovery.",
                    null!, _simDay);
                _sessionDurabilityDirty = true;
            };

            manager.BackupRestoredSeam = (slotId, recovered) =>
            {
                _journal?.TryAddRawEntry(
                    "session_recovery",
                    recovered
                        ? $"Save slot {slotId} was recovered from its backup."
                        : $"Save slot {slotId} had no valid backup; recovery was declined and the slot left quarantined.",
                    null!, _simDay);
                _sessionDurabilityDirty = true;
            };

            var session = new SessionDurabilityHostSession(manager);
            session.StateChanged += () => _sessionDurabilityDirty = true;
            _sessionDurability = session;
            return _sessionDurability;
        }

        private void SetupSessionDurability()
        {
            EnsureSessionDurability();
        }

        private void SaveSessionDurability()
        {
            if (_sessionDurability == null) return;
            CaptureSection(
                SessionDurabilitySaveStore.SectionName,
                SessionDurabilitySaveStore.TryCapturePersisted(_sessionDurability.System.CaptureState()));
            _sessionDurabilityDirty = false;
        }

        private void FlushSessionDurabilityIfDirty()
        {
            if (_sessionDurabilityDirty) SaveSessionDurability();
        }

        private void ResetSessionDurability()
        {
            _sessionDurability?.Dispose();
            _sessionDurability = null;
            _sessionDurabilityDirty = false;
            _lastSoakStable = true;
        }

        /// <summary>
        /// Deterministic digest of the committed section payloads, used as the
        /// observed slot checksum in the durability audit.
        /// </summary>
        internal static string ComputeSessionChecksum(System.Collections.Generic.IReadOnlyDictionary<string, string> payloads)
        {
            string joined = string.Join(
                "\n",
                payloads.OrderBy(kv => kv.Key, StringComparer.Ordinal)
                        .Select(kv => kv.Key + ":" + kv.Value));
            return SaveChecksum.Compute(joined);
        }

        private void RecordSessionSaveResult(string slotId, string displayName, string checksum)
        {
            var session = EnsureSessionDurability();
            int living = _survivors?.Roster?.LivingCount ?? (_survivors?.RosterState?.Count ?? 0);
            session.RecordSuccessfulSave(slotId, displayName, Math.Max(1, _simDay), living, checksum);
            _sessionDurabilityDirty = true;
        }

        private void RecordSessionDaySample(int day, float durationMs, long trackedStateBytes)
        {
            if (_sessionDurability == null) return;
            _sessionDurability.RecordDaySample(day, durationMs, trackedStateBytes);
            _sessionDurabilityDirty = true;

            var report = _sessionDurability.EvaluateSoak();
            if (report.IsMonotonicallyStable == _lastSoakStable) return;
            _lastSoakStable = report.IsMonotonicallyStable;
            _journal?.TryAddRawEntry(
                report.IsMonotonicallyStable ? "session_soak_recovered" : "session_soak_unstable",
                $"Session soak gate {(report.IsMonotonicallyStable ? "returned within bounds" : "exceeded its bounds")}: "
                + $"P95 {report.P95AdvanceDurationMs:F0}ms, slope {report.MemorySlopeBytesPerDay:F0}B/day over {report.TotalDaysSampled} sampled day(s).",
                null!, day);
        }

        private void RecordSessionLoadFailure(string slotId, string reason)
        {
            var session = EnsureSessionDurability();
            session.RecordLoadFailure(slotId, reason);
            _sessionDurabilityDirty = true;
        }
    }
}
