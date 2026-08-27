# ASHFALL — Current Integration Batch Manifest

**Date:** 2026-08-27
**Status:** In Progress / Uncommitted Verification Manifest
**Authority:** [AGENTS.md](../AGENTS.md) · [docs/CURRENT_AUTHORITY.md](CURRENT_AUTHORITY.md)

This manifest indexes all newly authored and modified systems currently in the active working tree prior to commit/lane snap, along with their exact verification commands and contracts.

---

## 1. Batch Systems & Modified Components

| System / Subsystem | Primary Files | Nature of Change |
|---|---|---|
| **Host Self-Test Normalization** | [`Assets/Ashfall.Core/HostTestSummary.cs`](../Assets/Ashfall.Core/HostTestSummary.cs)<br>[`Ashfall.Core.Tests/HostTestSummaryTests.cs`](../Ashfall.Core.Tests/HostTestSummaryTests.cs)<br>[`src/Host/HostCli.Summary.cs`](../src/Host/HostCli.Summary.cs)<br>[`src/Host/HostCli.SelfTests.cs`](../src/Host/HostCli.SelfTests.cs)<br>[`src/Host/HostCli.PanelTests.cs`](../src/Host/HostCli.PanelTests.cs)<br>[`src/Main.UiTests.*.cs`](../src) | Standardized machine-readable summary output (`[HOST_SELFTEST]`, `[HOST_SELFTEST_SUMMARY]`, `[HOST_SELFTEST_JSON]`, `<TOKEN> PASS/FAIL`) across all 35 host self-tests, 29 panel tests, standalone self-tests, and 15 UI test runners. |
| **Core Version Report & Schema Inventory** | [`Assets/Ashfall.Core/VersionReport.cs`](../Assets/Ashfall.Core/VersionReport.cs)<br>[`Ashfall.Core.Tests/VersionReportContractTests.cs`](../Ashfall.Core.Tests/VersionReportContractTests.cs) | Added engine-agnostic `--version` and `--schema-inventory` report generator with save/data schema version tracking and inventory scanners. |
| **UI Node Diagnostics & Hierarchy Introspection** | [`src/UI/UiNodeDiagnostics.cs`](../src/UI/UiNodeDiagnostics.cs) | Dynamic runtime hierarchy inspector and node diagnostic utility for panel verification, child path resolution, and snapshot stability. |
| **Panel Binding Lifecycle & Subscription Disposal** | [`src/Host/PanelBindLifecycleSelfTest.cs`](../src/Host/PanelBindLifecycleSelfTest.cs)<br>[`src/UI/GreenhousePanel.cs`](../src/UI/GreenhousePanel.cs)<br>[`src/UI/JournalPanel.cs`](../src/UI/JournalPanel.cs)<br>[`src/UI/MedicalPanel.cs`](../src/UI/MedicalPanel.cs)<br>[`src/UI/PowerGridPanel.cs`](../src/UI/PowerGridPanel.cs)<br>[`src/UI/SaveLoadPanel.cs`](../src/UI/SaveLoadPanel.cs) | Sealed event unsubscription, re-bind lifecycle safety, and disposal guards preventing memory leaks and duplicate handler invocations. |
| **CLI Catalog Generator & Drift Gate** | [`scripts/ci/generate-cli-catalog.sh`](../scripts/ci/generate-cli-catalog.sh)<br>[`docs/cli/HOST_CLI_COMMAND_CATALOG.md`](cli/HOST_CLI_COMMAND_CATALOG.md) | Automated catalog generator that parses live `--host-help` output and fails CI if documentation diverges from live host CLI verbs. |
| **CI Workflow & Authority Documentation** | [`.github/workflows/ci.yml`](../.github/workflows/ci.yml)<br>[`docs/CI.md`](CI.md)<br>[`docs/CURRENT_AUTHORITY.md`](CURRENT_AUTHORITY.md)<br>[`docs/ci/GATING_VS_DIAGNOSTIC_CHECKS.md`](ci/GATING_VS_DIAGNOSTIC_CHECKS.md)<br>[`docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md`](qa/MANUAL_PLAYTHROUGH_CHECKLIST.md) | Synchronized CI gates, authority map, and testing classification with Godot host reality. |

---

## 2. Verification Commands & Contracts

### Full Gate Suite

```bash
# 1. Pure C# Unit Tests (all unit tests must pass)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# 2. Godot Host Compilation (0 errors)
dotnet build Ashfall.csproj

# 3. Canonical Asset & Export Gate (headless export, PCK packing, isolated smoke)
bash scripts/ci/godot-asset-gate.sh

# 4. CLI Catalog Drift Check
bash scripts/ci/generate-cli-catalog.sh --check

# 5. Triad Drift Gate (Setup/Save/Flush parity across 65 systems)
bash scripts/ci/triad-drift-gate.sh
```

### System-Specific Verification

| System | Verification Command | Expected Output Contract |
|---|---|---|
| **Host Summary Unit Tests** | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter HostTestSummaryTests` | 9/9 tests pass (banner, kv, json, legacy, escaping) |
| **Version Report Contract Tests** | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter VersionReportContractTests` | 6/6 tests pass (schema inventory, formatting, fallback) |
| **Data Integrity Gate** | `godot --headless --path . -- --data-integrity-selftest` | `[HOST_SELFTEST] data_integrity_selftest PASS`, 0 errors across 129 catalogs |
| **Expansions Suite (01–10)** | `godot --headless --path . -- --expansions-selftest` | `[HOST_SELFTEST] expansions_selftest PASS`, 10/10 expansions green |
| **Deep Coast Expansion** | `godot --headless --path . -- --deep-coast-selftest` | `[HOST_SELFTEST] deep_coast_selftest PASS`, 72/72 passed |
| **Disease Expansion** | `godot --headless --path . -- --disease-selftest` | `[HOST_SELFTEST] disease_selftest PASS`, 28/28 passed |
| **Brine Water System** | `godot --headless --path . -- --brine-selftest` | `[HOST_SELFTEST] brine_selftest PASS`, 21/21 passed |
| **Radio Persistence & Triangulation** | `godot --headless --path . -- --radio-selftest` | `[HOST_SELFTEST] radio_selftest PASS` |
| **Version Report CLI** | `godot --headless --path . -- --version` | Emits `ASHFALL version report` with game, data, and save schema summaries |
| **Schema Inventory CLI** | `godot --headless --path . -- --schema-inventory` | `[HOST_SELFTEST] schema_inventory PASS`, tallies all 129 JSON catalogs |
| **UI Layout & Panels** | `godot --headless --path . -- --ui-layout-selftest` | `[HOST_SELFTEST] ui_layout_selftest PASS` |

---

## 3. Pre-Commit Checklist

- [x] Invariant 1: Core (`Assets/Ashfall.Core/`) contains 0 references to `Godot`, `UnityEngine`, or `JsonUtility`.
- [x] Invariant 2: Host needs adapt through pure interfaces in `Ports.cs`.
- [x] Invariant 3: Save wire contracts and SaveChecksum are green.
- [x] Invariant 4: Determinism preserved with `ISeededRng`.
- [x] Invariant 5: No gameplay logic in presentation nodes.
- [x] Invariant 6: Data authority in `Assets/StreamingAssets/Data/` is intact (129 catalogs, 0 errors).
- [x] All unit tests pass cleanly (0 failures).
- [x] Godot host compiles with 0 errors.
- [x] All CI scripts pass without warnings or drift.
