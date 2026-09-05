# Verdict Hardening Implementation Log

## Phase 1 — Evidence producer/consumer seam

Status: PASS

Changed:

- Added `VerdictEvidenceChain` in Core.
- Wired read machine logs to `EvidenceLedger` and `ReckoningSystem`.
- Made machine-log capture and restore deep-copy entries.
- Preserved persisted evidence IDs during restore.
- Tracked the host simulation day for Verdict save capture.
- Added focused replay and aliasing tests.

Tests:

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Verdict" --no-restore`
- 112 passed.

Result:

- A read evidence-producing log now opens the canonical Reckoning evidence
  gate exactly once.
- Save restore reconciliation is idempotent.

Divergences:

- This phase does not add the typed accusation or tribunal system. Those need
  their own data contract and consequence authority review.

Remaining:

- Add typed accusation eligibility and tribunal resolution.
- Add Verdict replay evidence at the Godot selftest layer.
