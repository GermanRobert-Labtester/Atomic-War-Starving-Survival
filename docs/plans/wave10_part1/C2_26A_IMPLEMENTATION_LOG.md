# C2[8] / Plan 26A & 26C — One Path Authority & Performance Budgets (Implementation Log)

**Task:** Wave 10 Part 1 — C2[8] "Ship Gate" / Plan 26A & 26C (prerequisite to B1)
**Status:** **SEALED** — all 40 bypass sites migrated, gate allowlist down to 1 (authority itself), boot diagnostics surfaced, performance budgets codified & passing
**Date:** 2026-09-17
**Authorized by:** user ("yes Continue i authorise you!", "Continue!", "continue!")

## Summary of Accomplishments

### Phase A — Extend `CatalogPath` Authority
`src/Host/CatalogPath.cs`:
- `ResolveCatalog(fileName)` — bare file name under the one data root.
- `ResolveSub(dir, fileName)` — file inside a single subdirectory.
- `IsSafeSegment` validation — rejects empty/`..`/`/`/`\`/`:`/rooted input.
- `ResolveRepoRoot()` — robust repository root resolver walking parent directories up to repo markers, replacing brittle relative path hacks across host CLI and selftests.
- `LastResolutionSource` — records how the root resolved (`env` / `executable` / `project` / `cwd` / `pck` / `fallback`) for boot diagnostics.

### Phase C & Remainder — Complete Bypass Migration (Tranches 1 & 2)
All 40 remaining bypass sites migrated to `CatalogPath` authority:
- `src/Main.*.cs`: `Main.Anomaly.cs`, `Main.Bionics.cs`, `Main.Zealotry.cs`, `Main.Companion.cs`, `Main.Plans126_129.cs`, `Main.Plans147.cs`, `Main.Plans178_181.cs`, `Main.Plans182_185.cs`, `Main.Plans186_189.cs`, `Main.Plans190_193.cs`, `Main.Plans194_197.cs`, `Main.Plans198_201.cs`, `Main.Plans202_205.cs`, `Main.Plans46_49.cs`, `Main.Application.cs`, `Main.UiTests.Economy.cs`.
- UI panels: `src/UI/ExpeditionPanel.cs`, `src/UI/FactionMatrixPanel.cs`, `src/UI/FactionsNarrativePanel.cs`, `src/Host/HoldfastTerminalPanel.cs`.
- Audio: `src/Audio/AudioCueCatalog.cs`, `src/Audio/AudioSelfTest.cs`.
- Host CLI / Selftests: `src/Host/ContrabandStashSelfTest.cs`, `src/Host/RadioCatalogSelfTest.cs`, `src/Host/RadioHostSession.cs`, `src/Host/PanelBindLifecycleSelfTest.cs`, `src/Host/SaveStoreChecksumSelfTest.cs`, `src/Host/UiAccessibilitySelfTest.cs`, `src/Host/PortContractSelfTest.cs`, `src/Host/LoaderWiringSelfTest.cs`, `src/Host/HostCli.NpcArcSelfTest.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/HostCli.ExpeditionPlaytest.cs`, `src/Host/HostCli.ExportParity.cs`, `src/Host/HostCli.AdvancedIndustrialRecon.cs`, `src/Host/HostCli.WorldPlaytest.cs`, `src/Host/CoreDemoSession.cs`, `src/Host/DutyRosterHostSession.cs`, `src/Host/PowerGridHostSession.cs`.

### Phase G — Forbidden Data-Path Gate (Shrunk & Sealed)
`Ashfall.Core.Tests/Tooling/CatalogPathForbiddenGateTests.cs`:
- Scans every `src/**/*.cs` for `Assets/StreamingAssets/Data`, `GlobalizePath("res://Assets/`, and `Directory.GetCurrentDirectory()`.
- Allowlist shrank from 40 down to **1** (`src/Host/CatalogPath.cs`, the authority itself).
- `NoNewPrivateDataResolvers` and `AllowlistShrinksAsSitesMigrate` both PASS (2/2).

### Phase I — Boot Resolution Diagnostics
- `HostCli.PrintVersion(string dataDir)` surfaces `data resolution: <dir> [source: <source>]` alongside version information.
- Verified in live headless execution: `data resolution: .../Assets/StreamingAssets/Data [source: project]`.

### Task 26C — Performance Budgets Codified & Gate Active
- Created `docs/perf/BUDGETS.md` defining simulation latency, save latency, heap allocation growth, and lifecycle retained memory budgets.
- Updated `src/Host/PerformanceSelfTest.cs` to resolve artifacts using `CatalogPath.ResolveRepoRoot()` and record budgeted pass/fail status in `artifacts/runtime-scale-results.json`.
- Gate `runtime_scale_performance` executed and passes 6/6 checks.

## Verification Matrix

| Check | Command | Result |
|---|---|---|
| Forbidden Gate | `scripts/run_test.sh Ashfall.Core.Tests/Tooling/CatalogPathForbiddenGateTests.cs` | **PASS (2/2)** |
| Host Build | `dotnet build Ashfall.csproj` | **0 errors, 0 warnings** |
| Core API Policy | `bash scripts/ci/forbidden-api-gate.sh` | **PASS (0 violations)** |
| Runtime Scale Gate | `bash scripts/ci/verify-fast.sh --gate runtime_scale_performance` | **PASS (6/6 checks)** |
| Export Parity Resolver | `godot --headless --path . -- --export-parity-selftest` | **Target resolved cleanly** |
| Campaign Journey | `godot --headless --path . -- --campaign-journey-selftest` | **PASS** |
| Full Fast Gates Suite | `bash scripts/ci/verify-fast.sh` | **ALL 47 GATES PASSED (220.11s)** |

## Consequence for B1

With 26A single data-path authority sealed and 26C performance budgets established, **B1 (C1[6] / Plan 27) entry gate is fully UNBLOCKED**.
