# ASHFALL — Script Catalog & Lifecycle Index

This document catalogs all developer tools, CI gates, asset pipelines, and maintenance utilities under `scripts/`, classifying them by lifecycle tier (`ACTIVE`, `MAINTENANCE`, or `HISTORICAL`) and mapping active scripts to their owning CI workflow.

---

## Lifecycle Tiers

| Tier | Definition | Usage & Modification Policy |
|---|---|---|
| 🟢 **`ACTIVE`** | Core CI gate, release validator, pre-commit hook, or production asset tool. | Executed automatically in CI (`.github/workflows/ci.yml`), `scripts/ci/verify-fast.sh`, or git pre-commit hooks. Subject to strict regression gating. |
| 🟡 **`MAINTENANCE`** | Periodic audit, diagnosis, code refactoring helper, or asset pipeline tool. | Executed on-demand for system sweeps, data hygiene, or asset imports. Preserved and updated when touched. |
| ⚪ **`HISTORICAL`** | One-shot migration or repair utility from a completed architectural milestone. | Preserved for historical provenance and forensic reference only. Do not execute against live production trees. |

---

## 1. Continuous Integration & Quality Gates (`scripts/ci/`)

All scripts executed during continuous integration or local fast-tier verification.

| Script | Status | Owning Workflow / CI Gate | Description |
|---|---|---|---|
| [`verify-fast.sh`](ci/verify-fast.sh) | 🟢 `ACTIVE` | Local Runner & CI master | Master runner executing all 25 fast-tier verification gates in order with strict per-gate time budgets. |
| [`no-whitespace-churn.sh`](ci/no-whitespace-churn.sh) | 🟢 `ACTIVE` | CI Gate 1 (`whitespace_gate`) | Checks working tree and staged diffs for trailing whitespace and carriage-return errors. |
| [`json-schema-policy-gate.sh`](ci/json-schema-policy-gate.sh)<br>[`json-schema-policy-gate.py`](ci/json-schema-policy-gate.py) | 🟢 `ACTIVE` | CI Gate 2 (`json_schema_gate`) | Validates JSON syntax, forbids root arrays, and enforces `schema_version` declarations. |
| [`forbidden-api-gate.sh`](ci/forbidden-api-gate.sh) | 🟢 `ACTIVE` | CI Gate 3 (`forbidden_api_gate`) | Static scanner enforcing Invariant 1 (zero engine coupling in `Assets/Ashfall.Core/`). |
| [`catch-policy-gate.sh`](ci/catch-policy-gate.sh) | 🟢 `ACTIVE` | CI Gate 4 (`catch_policy_gate`) | Audits `src/` for catch policy compliance and logs swallowed exceptions. |
| [`godot-asset-gate.sh`](ci/godot-asset-gate.sh) | 🟢 `ACTIVE` | CI Gate 10 (`asset_registry`) | Verifies texture resolution and `.import` sidecars for runtime assets. |
| [`triad-drift-gate.sh`](ci/triad-drift-gate.sh) | 🟢 `ACTIVE` | CI Gate 15 (`triad_drift`) | Enforces Setup/Save/AllSaveSections parity against [`SaveSectionRegistry`](../Assets/Ashfall.Core/Save/SaveSectionRegistry.cs). |
| [`generate-cli-catalog.sh`](ci/generate-cli-catalog.sh) | 🟢 `ACTIVE` | CI Gate 16 (`cli_catalog_drift`) | Regenerates [`HOST_CLI_COMMAND_CATALOG.md`](../docs/cli/HOST_CLI_COMMAND_CATALOG.md) from live `--host-help` output. |
| [`generate-selftest-manifest.py`](ci/generate-selftest-manifest.py) | 🟢 `ACTIVE` | CI Gate 17 (`selftest_manifest_drift`) | Regenerates [`SELFTEST_MANIFEST.json`](../docs/ci/SELFTEST_MANIFEST.json) from live host test registry. |
| [`generate-save-store-matrix.sh`](ci/generate-save-store-matrix.sh)<br>[`generate-save-store-matrix.py`](ci/generate-save-store-matrix.py) | 🟢 `ACTIVE` | CI Gate 18 (`save_store_matrix`) | Regenerates [`SAVE_STORE_CONTRACT_MATRIX.md`](../docs/saves/SAVE_STORE_CONTRACT_MATRIX.md). |
| [`warning-baseline-gate.sh`](ci/warning-baseline-gate.sh) | 🟢 `ACTIVE` | CI Gate 19 (`compiler_warning_baseline`) | Asserts 0 unexpected compiler warnings across all projects. |
| [`generate-docs-index.py`](ci/generate-docs-index.py) | 🟢 `ACTIVE` | CI Gate 20 (`docs_index_drift`) | Regenerates [`docs/INDEX.md`](../docs/INDEX.md) master documentation index. |
| [`doc-link-gate.sh`](ci/doc-link-gate.sh)<br>[`normalize-doc-links.py`](ci/normalize-doc-links.py) | 🟢 `ACTIVE` | CI Gate 21 (`doc_link_gate`) | Verifies and normalizes documentation links to portable relative paths. |
| [`nuget-dependency-gate.sh`](ci/nuget-dependency-gate.sh)<br>[`nuget-dependency-report.py`](ci/nuget-dependency-report.py) | 🟢 `ACTIVE` | CI Gate 22 (`nuget_cpm_gate`) | Enforces Central Package Management (`Directory.Packages.props`) and reports package updates. |
| [`generate-architecture-map.sh`](ci/generate-architecture-map.sh)<br>[`generate-architecture-map.py`](ci/generate-architecture-map.py) | 🟢 `ACTIVE` | CI Gate 23 (`architecture_map_drift`) | Regenerates [`ARCHITECTURE_TEST_MAP.md`](../docs/architecture/ARCHITECTURE_TEST_MAP.md). |
| [`persistent-filename-gate.sh`](ci/persistent-filename-gate.sh)<br>[`persistent-filename-gate.py`](ci/persistent-filename-gate.py) | 🟢 `ACTIVE` | CI Gate 24 (`persistent_filename_gate`) | Asserts filename uniqueness and declarative registration for all `user://` files. |
| [`godot-export-linux.sh`](ci/godot-export-linux.sh) | 🟢 `ACTIVE` | Release Pipeline (`.github/workflows/export.yml`) | Headless Godot Linux PCK and binary exporter. |
| [`git-hooks/pre-commit`](ci/git-hooks/pre-commit) | 🟢 `ACTIVE` | Local Developer Workflow | Pre-commit hook enforcing `.import` sidecars and whitespace hygiene. |
| [`git-hooks/secret-scan`](ci/git-hooks/secret-scan) | 🟢 `ACTIVE` | Local Developer Workflow | Pre-commit scanner preventing credential leakage. |
| [`lfs-health-check.sh`](ci/lfs-health-check.sh) | 🟡 `MAINTENANCE` | Developer Setup & CI | Validates Git LFS installation, ignorecase configuration, pointer integrity, and missing objects. |
| [`license-header-check.sh`](ci/license-header-check.sh) | 🟡 `MAINTENANCE` | Developer Hygiene & CI | Inspects changed/added C# source files for `// SPDX-License-Identifier: MIT` headers. |
| [`asset-orphan-sweep.sh`](ci/asset-orphan-sweep.sh) | 🟡 `MAINTENANCE` | Asset Maintenance | Sweeps `assets/` for unreferenced textures and orphaned files. |
| [`repo-hygiene-report.sh`](ci/repo-hygiene-report.sh) | 🟡 `MAINTENANCE` | Repository Hygiene | Audits repository junk, stray build outputs, and test artifacts. |
| [`git-object-inventory.sh`](ci/git-object-inventory.sh) | 🟡 `MAINTENANCE` | Repository Hygiene | Audits Git object packfiles and blob sizes. |
| [`quarantine_deprecated_ammo.sh`](ci/quarantine_deprecated_ammo.sh) | ⚪ `HISTORICAL` | Migration One-Shot | Quarantined legacy 16-gauge ammo art to deprecated assets archive. |

---

## 2. Asset Pipeline & Generation (`scripts/` & `scripts/pipeline/`)

| Script | Status | Owning Workflow | Description |
|---|---|---|---|
| [`composio_asset_pipeline.py`](composio_asset_pipeline.py) | 🟢 `ACTIVE` | Asset Generation Workflow | Composio MCP image generation worker and queue processor. |
| [`audit_assets.py`](audit_assets.py) | 🟡 `MAINTENANCE` | Asset Audit Workflow | Validates disk textures against item and location catalog definitions. |
| [`generate_item_icons.py`](generate_item_icons.py) | 🟡 `MAINTENANCE` | Asset Generation Workflow | Batch generation script for item icons from prompt templates. |
| [`pipeline/generate_assets.py`](pipeline/generate_assets.py) | 🟡 `MAINTENANCE` | Asset Pipeline | Image generation coordinator across AI backend providers. |
| [`pipeline/import_approved_assets.py`](pipeline/import_approved_assets.py) | 🟡 `MAINTENANCE` | Asset Pipeline | Moves approved assets into `assets/art/` and generates `.import` sidecars. |

---

## 3. Maintenance & Batch Refactoring (`scripts/maintenance/`)

| Script | Status | Owning Subsystem | Description |
|---|---|---|---|
| [`maintenance/consolidate_catalog_tests.py`](maintenance/consolidate_catalog_tests.py) | 🟡 `MAINTENANCE` | Unit Test Maintenance | Consolidates catalog test classes to inherit from `CatalogTestBase`. |
| [`maintenance/migrate_schema_version.py`](maintenance/migrate_schema_version.py) | 🟡 `MAINTENANCE` | Data Authority | Scans and standardizes `schema_version` in data authority JSON files. |
| [`maintenance/add_p11_methods.py`](maintenance/add_p11_methods.py) | ⚪ `HISTORICAL` | Host Migration | Scaffolds dirty-tracking and Save methods on legacy HostSessions. |
| [`maintenance/add_p11_methods_v2.py`](maintenance/add_p11_methods_v2.py) | ⚪ `HISTORICAL` | Host Migration | Batch-added dirty tracking and save delegation across 28+ HostSessions. |
| [`maintenance/cleanup_p11_hostsessions.py`](maintenance/cleanup_p11_hostsessions.py) | ⚪ `HISTORICAL` | Host Migration | Cleaned up HostSessions to inherit from `HostSessionBase`. |
| [`maintenance/convert_hostsessions.py`](maintenance/convert_hostsessions.py) | ⚪ `HISTORICAL` | Host Migration | Migrated sealed `HostSession` classes in `src/Host/`. |
| [`maintenance/fix_event_leaks.py`](maintenance/fix_event_leaks.py) | ⚪ `HISTORICAL` | Host Migration | Scaffolded `UnsubscribeAll()` methods in HostSessions. |

For detailed instructions on the maintenance tools, see [`scripts/maintenance/README.md`](maintenance/README.md).
