# Plan 127 Implementation Log

## Phase 1 — Runtime and authority audit

**Status:** PASS

Confirmed:

- `verdict_data.json` is the data authority.
- Corruption text is opaque, seeded, and persisted in machine-log entries.
- The ladder is loaded into journal/codex rows.
- `discovery_location_id` is not a runtime gate.
- EvidenceLedger has no knowledge-key or ladder progression state.
- Existing Verdict lore producers hardcode six keys.

## Phase 2 — Data expansion

**Status:** PASS

Changed:

- `Assets/StreamingAssets/Data/verdict_data.json`

Result:

- `corruption_corpus`: 8 → 25, append-only.
- `world_history_ladder`: 6 → 12, append-only layers 7–12.
- All six new locations are committed catalog IDs.

## Phase 3 — Contracts and inventories

**Status:** PASS

Added:

- `docs/verdict/VERDICT_DATA_RUNTIME_CONTRACT.md`
- `docs/verdict/PLAN_127_CORRUPTION_CORPUS_BASELINE.md`
- `docs/verdict/PLAN_127_WORLD_HISTORY_BASELINE_MATRIX.md`
- `docs/verdict/VERDICT_KNOWLEDGE_KEY_INVENTORY.md`
- `docs/verdict/VERDICT_HISTORY_LOCATION_INVENTORY.md`
- `docs/verdict/PLAN_127_VERDICT_DATA_CORRUPTION_HISTORY_EXPANSION_CLOSEOUT.md`

## Phase 4 — Verification

**Status:** PARTIAL

Passed:

- Plan 127 JSON/reference script: corpus 25, ladder 12, layers 1–12,
  unique keys/corpus, all 12 locations resolved.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
  --filter 'FullyQualifiedName~Verdict' --no-restore`: **172 passed**.
- `godot --headless --path . -- --data-integrity-selftest`: **PASS**, 0
  errors across 298 catalogs.
- `godot --headless --path . -- --content-utilization-selftest`: **PASS**.
- `dotnet build Ashfall.csproj --no-restore`: **PASS**, 0 warnings/errors.
- `godot --headless --path . -- --bridge-selftest`: **PASS**.

The full suite command
`dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore`
did not pass in the current concurrent worktree: **9,947 passed, 2 failed**.
The failures are in unrelated existing faction-branch tests:

- `FactionBranchCoordinatorTests.UIQueries_GetBranchOptionsAndFactionStandingSummaries_ReturnAccurateData`
  expected 24 and received 31.
- `IndependentBranchExpansionTests.SaveRoundTrip_NewBranch_Witness_PreservesState`
  rejected the current `VeryEvil` morality band.

No Plan 127 file appears in either failure.

## Divergence and limitation

The plan's phrase "EvidenceLedger consumes the ladder" does not match the
current repository. EvidenceLedger stores evidence IDs only; the ladder is
journal/codex data. The data expansion is complete, while dynamic reachability
for layers 7–12 remains explicitly deferred rather than fabricated in data.
