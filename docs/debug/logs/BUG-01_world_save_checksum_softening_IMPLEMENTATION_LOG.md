# BUG-01 Implementation Log — WorldSaveStore Checksum Softening

## Phase 1 — Failing regression assertions

**Pre-integration checkpoint:** PASS (repo unchanged since audit; `customPath` hooks
present; mutation probe targets `"rollCount"` — field name verified verbatim in
`SystemTextJsonSerializer` output via `IncludeFields` + no naming policy).
**Changes:** `src/Host/HostCli.PanelTests.cs` — 4 new `Check` assertions inside the
existing shelter-hazard-loop section 10: probe-mutation sanity, mutated-payload
rejection, fresh-resave round-trip.
**Regression test:** `--shelter-hazard-loop-selftest` → `[FAIL] world save with
mutated payload is REJECTED (checksum mismatch)` (Failures: 1). Defect reproduced
against the real store, exactly as designed.
**Related tests:** not run (red phase).
**Diff review:** +32/−1, confined to the save round-trip block.
**Invariant review:** test-only change; no production semantics touched.
**Result:** RED as intended. Proceeded.

## Phase 2 — Store repair

**Pre-integration checkpoint:** PASS (HEAD's committed reject verified via
`git show HEAD:src/Host/WorldSaveStore.cs`; repair restores that semantics; message
corrected so "version migration" is not implied as tolerated).
**Changes:** `src/Host/WorldSaveStore.cs` — mismatch branch now logs
`load failed: checksum mismatch (corrupt or foreign save).` and `return null;`
plus a 4-line comment documenting why tolerance is forbidden and where migration
belongs (versioned codec pattern).
**Regression test:** `--shelter-hazard-loop-selftest` → all 6 world-save checks PASS,
Failures: 0.
**Related tests:** `--world-selftest` PASS 10/10 (incl. "save/load checksum stable").
**Diff review:** net +43/−7 across the two files vs HEAD; the only non-repair deltas
are the pre-existing `customPath` hooks (deliberately retained — they enable the test).
**Invariant review:** (1) mismatch ⇒ reject ✓ (2) missing checksum ⇒ reject ✓
(unchanged) (3) legacy bare-state fallback ✓ (unchanged path) (4) clean round-trip ✓
(asserted twice) (5) no RNG/event/Core changes ✓.
**Result:** GREEN.

## Phase 3 — Verification ladder + adversarial

- `dotnet build Ashfall.csproj` — 0 errors (nullable warnings in the tree are
  pre-existing uncommitted-sweep noise; none attributable to this repair).
- `dotnet test Ashfall.Core.Tests` — 2497/2497 PASS.
- `godot --headless -- --shelter-hazard-loop-selftest` — 0 failures (mutation probe
  rejected; clean round-trip intact after rejection path).
- `godot --headless -- --world-selftest` — PASS 10/10.
- `godot --headless -- --data-integrity-selftest` — PASS, 0 errors.
- `godot --headless --quit-after 2` — boots clean; only pre-existing exit-teardown
  RID-leak warnings.

**Adversarial probes:** repeated save/load cycles (twice across runs), mutated
payload (rejected), post-rejection fresh save (loads), legacy fallback path (code
untouched), world-system checksum stability (selftest-pinned). No probe broke the
repair. The repair removes the laundering chain at the load boundary; no shadow
workaround remains.
