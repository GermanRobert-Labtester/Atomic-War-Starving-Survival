using System;

namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// Canonical producer/consumer seam for Verdict evidence.
    /// Machine logs produce evidence only when a player reads an entry;
    /// the ledger owns idempotence and Reckoning owns the phase gate.
    /// </summary>
    public sealed class VerdictEvidenceChain
    {
        private readonly MachineLogSystem _machineLog;
        private readonly EvidenceLedger _ledger;
        private readonly ReckoningSystem _reckoning;

        public VerdictEvidenceChain(
            MachineLogSystem machineLog,
            EvidenceLedger ledger,
            ReckoningSystem reckoning)
        {
            _machineLog = machineLog ?? throw new ArgumentNullException(nameof(machineLog));
            _ledger = ledger ?? throw new ArgumentNullException(nameof(ledger));
            _reckoning = reckoning ?? throw new ArgumentNullException(nameof(reckoning));

            _machineLog.OnEntryRead += HandleEntryRead;
        }

        /// <summary>
        /// Replays persisted read entries after restore. The ledger remains the
        /// idempotence authority, so replay is safe and does not double-count.
        /// </summary>
        public int ReconcileReadEntries()
        {
            int enrolled = 0;
            for (int i = 0; i < _machineLog.Entries.Count; i++)
            {
                var entry = _machineLog.Entries[i];
                if (entry == null || !entry.read) continue;
                if (Enroll(entry.evidenceTag, entry.day))
                    enrolled++;
            }
            _reckoning.ReconcileEvidenceCount(_ledger.Count);
            return enrolled;
        }

        private void HandleEntryRead(MachineLogEntry entry)
        {
            if (entry == null) return;
            Enroll(entry.evidenceTag, entry.day);
        }

        private bool Enroll(string evidenceTag, int day)
        {
            if (string.IsNullOrEmpty(evidenceTag)) return false;
            if (!_ledger.Enroll(evidenceTag, day)) return false;

            _reckoning.EnrollEvidence(1);
            return true;
        }
    }
}
