# Verdict Evidence Chain

## Authority

The Verdict evidence path has one producer and two authorities:

```text
MachineLogSystem.ReadEntry
        ↓
VerdictEvidenceChain
        ↓
EvidenceLedger.Enroll
        ↓
ReckoningSystem.EnrollEvidence
```

`MachineLogSystem` owns whether a record has been read. `EvidenceLedger` owns
the enrolled evidence IDs and idempotence. `ReckoningSystem` owns the numeric
evidence count used by the Culpable gate. The Godot host only constructs the
chain and projects state.

## Rules

1. An unread log does not enroll evidence.
2. A read log with an empty `evidenceTag` is presentation-only.
3. A read log enrolls at most once, even when the read event is replayed.
4. Reckoning increments only when the ledger accepts a new evidence ID.
5. Restoring a save deep-copies log entries and replaying read entries is safe.
6. Restored evidence IDs are preserved even if a partial catalog is loaded.
   Catalog membership is a gate for new enrollment, not a reason to discard
   persisted history.

## Save and replay contract

`MachineLogSystem.CaptureState` and `RestoreState` copy each
`MachineLogEntry`. The save DTO therefore cannot alias the live log or mutate a
restored session through the caller's object graph.

`VerdictEvidenceChain.ReconcileReadEntries()` is called after Verdict restore.
It scans only entries already marked `read`; the ledger's idempotence check
prevents duplicate evidence and duplicate Reckoning increments.
After the scan, the bridge repairs Reckoning's derived evidence count from the
ledger. This also upgrades older saves that contain read logs but no matching
derived count.

## Host projection

`VerdictHostSession` owns the chain instance and records the latest simulation
day for save capture. `AdvanceDay(day, ...)` updates that day before polling
Reckoning, so a Verdict save carries the actual current day rather than the
old placeholder value.

## Remaining work

This contract closes the machine-log producer to evidence-ledger consumer seam.
Typed accusation eligibility, tribunal resolution, and cross-expansion
consequence routing remain separate phases and must not be inferred from the
read-event path.
