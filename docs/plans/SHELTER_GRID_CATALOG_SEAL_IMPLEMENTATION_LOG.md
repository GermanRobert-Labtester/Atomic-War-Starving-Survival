# SHELTER GRID CATALOG SEAL — IMPLEMENTATION LOG

Plan: `docs/plans/SHELTER_GRID_CATALOG_SEAL_INTEGRATION_PLAN.md`
Branch: `feat/asset-pipeline-flagship` (pre-existing uncommitted work from a concurrent
stream present in the tree; none of it touched or staged by this wave).

---

## Phase 0 — Baseline verification

Status: PARTIAL (pre-existing blocker documented)

Results:
* `dotnet build Ashfall.Core.Tests/...` — PASS (0 errors, 11.3 s)
* `dotnet test Ashfall.Core.Tests/...` — PASS (8823/8823, 28 s, with `--blame-hang-timeout 180s`)
* `dotnet build Ashfall.csproj` — **FAIL (pre-existing)**: 1 error,
  `Assets/Ashfall.Core/Shelter/CryoVaultSystem.cs(105): CS0103 'CatalogDiagnostics' does not exist`
  — the file is **untracked** (`??` in git status), authored by the concurrent stream,
  and lacks `using Ashfall.Core.IO;`. Not caused by this wave. Because the host build
  gates Phases 3–6 (and `--data-integrity-selftest` compiles the host), this blocker
  will be repaired minimally at Phase 3 with a one-line using, documented here.
* `godot --headless -- --data-integrity-selftest` — DEFERRED (blocked by host build above)

Divergences: none yet.

Remaining: Phases 1–6.

---

## Phase 1 — Core contract

Status: PASS

Changed:
* CREATE `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs` —
  `ShelterPowerGridCatalogDef`/`ShelterPowerGridRoomDef` DTOs (snake_case JSON contract),
  `ShelterPowerGridCatalogLoader` with `TryLoad` (strict, error names file+field),
  `LoadOrDefault` (fallback + `CatalogDiagnostics.Warn` on malformed; silent fallback on
  file-not-found), `Validate` (schema version, duplicate IDs, negative watts, empty
  display name, unknown priority), `MapPriority`, `FallbackDefault()`
  (the former hardcoded values, now single-sourced in Core).

Tests:
* CREATE `Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs` — 10 tests:
  shipped load, host/strict parity, missing-file fallback + error text, malformed JSON
  fallback + error text, duplicate ID, negative watts, unknown priority, wrong schema
  version, empty display name, deterministic ordering, fallback self-validates.

Result: 10/10 targeted tests pass.

Divergences: none.

---

## Phase 2 — Test reconciliation (G3)

Status: PASS

Changed:
* `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — removed
  `<Compile Remove="Shelter/PowerGridCatalogTests.cs" />` (un-quarantined).
* REWRITE `Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs` — replaces the
  18-room assertion with the shipped-authority pin: schema_version 1, defaults
  800/4000/100, exactly 7 rooms (6 shipped + `room_workshop` from Phase 4) with
  id/display/draw/priority/failure_effect_id pinned in order, unique-ID check, and a
  canonical-consumer-ID check (`room_water_pump`, `room_workshop`, `room_greenhouse`,
  `room_clinic` must resolve — the G1 bug class as a permanent gate).

Tests: repaired file passes alongside Phase 1 (18/18 targeted).

Divergences: the plan's 18-vs-6 mystery is resolved as documented drift; expanding
the catalog to 18 rooms remains an out-of-scope balance decision.

---

## Phase 3 — Host wiring (G2)

Status: PASS

Changed:
* `src/Host/PowerGridHostSession.cs` — `CreateDefault(ISeededRng, string? dataDir)`
  overload; parses the catalog via the Core loader; deletes `DefaultGrid()` and the
  private `PowerGridJson`/`PowerGridRoomJson` DTOs (drift now structurally impossible);
  old 1-arg overload delegates with null for headless/selftest callers.
* `src/Main.World.cs` — `SetupPowerGrid` passes `_dataDir`.

Note: the pre-existing `CryoVaultSystem.cs` compile blocker (baseline Phase 0) was
fixed by the concurrent stream upstream between phases; no repair was needed here.

Result: `dotnet build Ashfall.csproj` — 0 warnings, 0 errors.

Divergences: none.

---

## Phase 4 — Room-ID reconciliation (G1)

Status: PASS

Changed:
* `src/Main.Plans166_169.cs` — fluid power derivation now queries
  `IsRoomPowered("room_water_pump")` (canonical catalog ID) instead of the
  unknown `room_water_treatment`; comment documents the G1 defect class.
* `Assets/StreamingAssets/Data/power_grid.json` — adds `room_workshop`
  (300 W, low, `fx_workshop_offline`); canonical room already referenced by
  `power_subgrid_nodes.json` (node_workshop_feed) and queried by `Main.World.cs:289`.

Result: host build 0/0; fluid power derivation nominal.

Behavior delta: total nominal draw +380 W (room_water_pump 100 W + room_lighting_main
80 W now exist at runtime; room_workshop 300 W new) — brownout timing shifts in
existing campaigns. Intended correction, not a regression.

Divergences: none.

---

## Phase 5 — Headless selftest

Status: PASS

Changed:
* `src/Host/HostCli.cs` — `PowerGridCatalogSelfTest` enum value, `--power-grid-catalog-selftest`
  parse + help line.
* `src/Main.Application.cs` — dispatch case.
* `src/Host/HostCli.PanelTests.cs` — `RunPowerGridCatalogSelfTest()`: catalog loads via
  host path, ≥7 rooms, canonical IDs present, every room powered at baseline
  (unknown-room-reads-false guard), fluid derivation == 1f with healthy breakers
  (the G1 regression guard).

Result:
```
[HOST_SELFTEST] power_grid_catalog_selftest PASS
[HOST_SELFTEST_SUMMARY] test=power_grid_catalog_selftest status=PASS exit_code=0 details="rooms=7 fluidPower=1"
```

Divergences: none.

---

## Phase 6 — Full verification

Status: PASS

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/...` | PASS, 0 errors |
| `dotnet test Ashfall.Core.Tests/...` | PASS — 8855/8855 (was 8823 baseline; +32 wave tests) |
| `dotnet build Ashfall.csproj` | PASS — 0 warnings, 0 errors |
| `--data-integrity-selftest` | PASS — 284 catalogs, 0 errors, 0 warnings |
| `--bridge-selftest` | PASS, exit 0 |
| `--content-utilization-selftest` | PASS, exit 0 |
| `scripts/ci/scene-lint.py` | PASS — 30 scenes, 0 errors |
| `--power-grid-catalog-selftest` (new) | PASS — rooms=7, fluidPower=1 |

One mid-wave lint catch: `CatchPolicyLintGateTests` flagged the loader's file-read
catch for missing `CatalogDiagnostics.Warn` — fixed (H4 policy), suite re-run green.

---

## Commit decision

NOT COMMITTED. Reason: shared files (`Ashfall.Core.Tests.csproj`, `Main.World.cs`,
`HostCli.cs`, `Main.Application.cs`, `HostCli.PanelTests.cs`) carry interleaved
uncommitted hunks from the concurrent `feat/asset-pipeline-flagship` stream
(e.g. `Main.World.cs` 261 insertions, ~260 not from this wave). Committing whole
files would bundle foreign in-flight work; partial staging risks breaking it.

File manifest for the owner to commit once the tree settles:
* `Assets/Ashfall.Core/Shelter/ShelterPowerGridCatalog.cs` (new)
* `Assets/StreamingAssets/Data/power_grid.json` (+7 lines)
* `src/Host/PowerGridHostSession.cs`
* `src/Main.World.cs` (1 line: `CreateDefault(rng, _dataDir)`)
* `src/Main.Plans166_169.cs` (1 literal + comment)
* `src/Host/HostCli.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Main.Application.cs`
  (selftest verb)
* `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` (1 line: quarantine removed)
* `Ashfall.Core.Tests/Shelter/PowerGridCatalogTests.cs` (rewritten)
* `Ashfall.Core.Tests/Shelter/ShelterPowerGridCatalogLoaderTests.cs` (new)
* `docs/plans/SHELTER_GRID_CATALOG_SEAL_*`, `docs/forensics/SHELTER_CASCADE_SEAMS_FORENSIC_REPORT.md`

Remaining known limitations (all pre-declared out of scope):
* G4 EMP→grid coupling, G5 water-treatment/medical power dependency,
  G6 FailureEffectId consumers — unchanged, pending design decisions.
* Legacy greenhouse fallbacks still hardcode `growLightHours: 6f` (G7).
