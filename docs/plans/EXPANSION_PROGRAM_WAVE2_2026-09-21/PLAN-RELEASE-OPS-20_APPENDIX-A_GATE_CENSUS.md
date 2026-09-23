# PLAN-RELEASE-OPS-20 — Appendix A: CI Gate Census

**Generated:** 2026-09-21 from `docs/ci/CI_GATE_MANIFEST.json`
(57 gates · 53 fast).
**Columns:** id · name · category · classification · critical · timeout (s) ·
command.
**Use:** RO-20A — the reliability census input. Every gate needs: can it fail
(non-zero exit on a real defect), what it reports, runtime class, and flake
status. The census below provides the declared command and timeout; RO-20A
adds the truthfulness probe results per gate.

## Gate census

| Gate ID | Name | Category | Class | Critical | Timeout (s) | Command |
|---|---|---|---|---|---:|---|
| `whitespace_hygiene` | Trailing Whitespace & Hygiene Gate | Code & Repo Hygiene | fast | yes | 30 | `bash scripts/ci/no-whitespace-churn.sh` |
| `json_schema_policy` | StreamingAssets JSON Syntax & Schema Policy | Code & Repo Hygiene | fast | yes | 30 | `bash scripts/ci/json-schema-policy-gate.sh` |
| `build_core_tests` | Build Ashfall.Core.Tests (net9.0) | Build & Tests | fast | yes | 120 | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo` |
| `test_core_suite` | Execute Core Unit & Determinism Test Suite (xUnit) | Build & Tests | full | yes | 120 | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --nologo` |
| `build_godot_host` | Build Godot Host Application (Ashfall.csproj net8.0) | Build & Tests | fast | yes | 120 | `dotnet build Ashfall.csproj --nologo` |
| `godot_import` | Godot Resource Cache Import | Host Selftests & Lifecycle | fast | yes | 180 | `bash scripts/ci/run-godot-bounded.sh --path . --import` |
| `data_integrity` | Data Authority Integrity Gate (129+ Catalogs) | Host Selftests & Lifecycle | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --data-integrity-selftest` |
| `bridge_removal` | Bridge Shim Removal Confirmation Gate | Host Selftests & Lifecycle | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --bridge-selftest` |
| `asset_registry` | Asset Registry Resolution Gate | Host Selftests & Lifecycle | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --asset-registry-selftest` |
| `player_panels_uitest` | Player UI Panels Construction & Binding | Host Selftests & Lifecycle | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --player-panels-uitest` |
| `panel_bind_lifecycle` | Panel Bind/Unbind/Rebind Lifecycle Self-Test | Host Selftests & Lifecycle | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --panel-bind-lifecycle-selftest` |
| `save_load_failure` | Save/Load UI Failure Paths Self-Test | Save Stores & Persistence | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --save-load-ui-failure-selftest` |
| `holdfast_save` | Holdfast S1 Save Store Self-Test | Save Stores & Persistence | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --holdfast-save-selftest` |
| `inventory_save` | Inventory Save Store Self-Test | Save Stores & Persistence | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --inventory-save-selftest` |
| `journal_save` | Journal Save Store Self-Test | Save Stores & Persistence | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --journal-save-selftest` |
| `playable_shell` | Playable Shell Campaign Smoke Self-Test | Campaign Smoke | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --playable-shell-selftest` |
| `day1_onboarding` | Day 1 Onboarding & Shelter Survival Self-Test | Campaign Smoke | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --day1-selftest` |
| `real_campaign_journey` | Real Campaign Journey Self-Test (Plans #5/#7/#8/#9) | Campaign Smoke | fast | yes | 90 | `bash scripts/ci/run-godot-bounded.sh --path . -- --real-campaign-journey-selftest` |
| `runtime_scale_performance` | Runtime Scale Performance Budget Self-Test (Task 130) | Performance | performance | no | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --runtime-scale-selftest` |
| `expansions_completeness` | Expansions 01-10 Completeness Self-Test | Campaign Smoke | fast | yes | 60 | `bash scripts/ci/run-godot-bounded.sh --path . -- --expansions-selftest` |
| `survivors_selftest` | Survivor Needs, Radiation & Restore Integrity Self-Test | Host Selftests & Lifecycle | fast | yes | 90 | `bash scripts/ci/run-godot-bounded.sh --path . -- --survivors-selftest` |
| `expedition_selftest` | Expedition Loop & Vehicle Logistics Self-Test | Host Selftests & Lifecycle | fast | yes | 90 | `bash scripts/ci/run-godot-bounded.sh --path . -- --expedition-selftest` |
| `triad_drift` | Setup/Save/AllSaveSections Triad Drift Gate | Drift & Architecture Gates | fast | yes | 30 | `bash scripts/ci/triad-drift-gate.sh` |
| `cli_catalog_drift` | Host CLI Command Catalog Drift Gate | Drift & Architecture Gates | fast | yes | 30 | `bash scripts/ci/generate-cli-catalog.sh --check` |
| `save_store_matrix_drift` | Save-Store Contract Matrix Drift Gate | Drift & Architecture Gates | fast | yes | 30 | `bash scripts/ci/generate-save-store-matrix.sh --check` |
| `architecture_map_drift` | Architecture Test Map Drift Gate | Drift & Architecture Gates | fast | yes | 30 | `bash scripts/ci/generate-architecture-map.sh --check` |
| `compiler_warning_baseline` | Compiler Warning Baseline Gate (0 Warnings) | Drift & Architecture Gates | fast | yes | 180 | `bash scripts/ci/warning-baseline-gate.sh` |
| `docs_index_drift` | Master Documentation Index Drift Gate | Drift & Architecture Gates | fast | yes | 30 | `python3 scripts/ci/generate-docs-index.py --check` |
| `forbidden_core_apis` | Forbidden Core API Source Gate | Source Policy & Lint Gates | fast | yes | 30 | `bash scripts/ci/forbidden-api-gate.sh` |
| `catch_policy_lint` | Catch Policy & Exception Handling Lint Gate | Source Policy & Lint Gates | fast | yes | 30 | `bash scripts/ci/catch-policy-gate.sh` |
| `persistent_filename_registry` | Persistent Filename Uniqueness & Registry Gate | Source Policy & Lint Gates | fast | yes | 30 | `bash scripts/ci/persistent-filename-gate.sh` |
| `central_package_management` | Central Package Management (CPM) Gate | Source Policy & Lint Gates | fast | yes | 30 | `bash scripts/ci/nuget-dependency-gate.sh` |
| `doc_link_portability` | Portable Documentation Link Gate | Source Policy & Lint Gates | fast | yes | 30 | `bash scripts/ci/doc-link-gate.sh` |
| `lfs_health_check` | Git LFS Object Storage & Binary Health Check | Source Policy & Lint Gates | fast | yes | 30 | `bash scripts/ci/lfs-health-check.sh` |
| `legacy_asset_path` | Legacy Asset Path Gate (Ticket #124) | Source Policy & Lint Gates | fast | yes | 30 | `bash scripts/ci/legacy-asset-path-gate.sh` |
| `legacy_reference` | Legacy Reference Gate (Ticket #124) | Source Policy & Lint Gates | fast | yes | 30 | `bash scripts/ci/legacy-reference-gate.sh` |
| `core_systems_catalog_drift` | Core Systems & Host Seams Catalog Drift Gate | Architecture & Catalog Gates | fast | yes | 30 | `python3 scripts/ci/generate-core-systems-catalog.py --check` |
| `catalog_registry_drift` | Data Authority Catalog Registry Drift Gate | Architecture & Catalog Gates | fast | yes | 30 | `python3 scripts/ci/generate-catalog-registry.py --check` |
| `agent_rulebooks_sync` | Multi-Agent Rulebooks Synchronization Gate | Architecture & Catalog Gates | fast | yes | 30 | `python3 scripts/ci/sync-agent-rulebooks.py --check` |
| `ui_panel_catalog_drift` | UI Panel Architecture Guide Drift Gate | Architecture & Catalog Gates | fast | yes | 30 | `python3 scripts/ci/generate-ui-panel-catalog.py --check` |
| `expansions_catalog_drift` | Expansions 01-11 Master Catalog Drift Gate | Architecture & Catalog Gates | fast | yes | 30 | `python3 scripts/ci/generate-expansions-catalog.py --check` |
| `ui_panel_contracts_test` | UI Scene Unique Node Contract Test Gate (xUnit) | Build & Tests | fast | yes | 60 | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter UiPanelContractTests --nologo` |
| `audio_catalog_drift` | Audio Cue Catalog Drift Gate | Architecture & Catalog Gates | fast | yes | 30 | `python3 scripts/ci/generate-audio-catalog.py --check` |
| `agent_skills_catalog_drift` | Multi-Agent Skills Index Drift Gate | Architecture & Catalog Gates | fast | yes | 30 | `python3 scripts/ci/generate-agent-skills-catalog.py --check` |
| `audio_cue_integrity_gate` | Audio Cue Static Contract & Bus Gate (xUnit) | Build & Tests | fast | yes | 60 | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter AudioCueIntegrityTests --nologo` |
| `campaign_envelope_fuzz_test` | Campaign Envelope Fuzzing & Mutation Gate (xUnit) | Build & Tests | fast | yes | 60 | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter CampaignEnvelopeFuzzTests --nologo` |
| `case_alias_guard` | Case-folded path collision guard (Plan VIII 25.9/25.10) | Repository hygiene | fast | no | 30 | `bash scripts/ci/case-collision-gate.sh` |
| `selftest_manifest_drift` | Selftest manifest in sync with HostCliRegistry (Plan VIII 24.6) | Drift guard | fast | no | 60 | `python3 scripts/ci/generate-selftest-manifest.py --check` |
| `export_parity` | Packaged-data parity of the exported Linux build (Plan VIII 23/24.11) | Release | full | no | 180 | `bash scripts/ci/run-godot-bounded.sh --path . -- --export-parity-selftest` |
| `uid_sidecar_gate` | UID Sidecar Gate (Plan 19 INV-19.6 / 19C-A.2) | Code & Repo Hygiene | fast | yes | 30 | `bash scripts/ci/uid-sidecar-gate.sh` |
| `coverage_gate` | Core Save Round-Trip & Determinism Coverage Gate (Plan 27B) | Quality & Verification | fast | yes | 60 | `bash scripts/ci/coverage-gate.sh` |
| `content_acceptance_pipeline` | Content Acceptance Pipeline Gate (Plan 45 / C1[14]) | Quality & Verification | fast | yes | 120 | `bash scripts/ci/content-acceptance-gate.sh` |
| `port_contract_gate` | Core Port Contracts and Host Wiring Gate (Plan 36A / C2[13]) | Quality & Verification | fast | yes | 60 | `python3 scripts/ci/generate-port-contract.py --check` |
| `input_map_contract` | Input Map Contract and Action Parity Gate (Plan 37 / C2[15]) | Quality & Verification | fast | yes | 30 | `bash scripts/ci/input-map-gate.sh` |
| `version_gate` | Version Drift Gate — three-source agreement + strict semver (Plan 48 / C2[21]) | Release | fast | yes | 30 | `python3 scripts/ci/version-gate.py` |
| `changelog_drift` | Changelog Machine-Region Drift Gate (Plan 48 / C2[21]) | Release | fast | no | 30 | `python3 scripts/release/generate_changelog.py --check` |
| `save_support_window` | Save Support Window & Historical Fixture Corpus Gate (Plan 48 / C2[21] Phase 3) | Release | full | yes | 120 | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter SaveSupportWindowTests --nologo` |

## Categories

Host Selftests & Lifecycle: 8, Source Policy & Lint Gates: 8, Architecture & Catalog Gates: 7, Build & Tests: 6, Drift & Architecture Gates: 6, Save Stores & Persistence: 4, Campaign Smoke: 4, Release: 4, Quality & Verification: 4, Code & Repo Hygiene: 3, Performance: 1, Repository hygiene: 1, Drift guard: 1
