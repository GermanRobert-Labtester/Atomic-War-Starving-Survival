# CI Gate Inventory (Plan VIII · Task 24.1)

Generated from `docs/ci/CI_GATE_MANIFEST.json` on 2026-09-05 — 49 gates, 47 fast. Regenerate with `python3 scripts/ci/run-gates.py --list`. The manifest is the single authority: add or change gates THERE, never in prose only.

Runtimes below are budgeted timeouts (enforced ceiling), not measured durations; measured durations land in every `--report-json` run (Task 24.10 budgets).

| Tier | Gate | Category | Timeout ceiling |
|---|---|---|---|
| fast | `whitespace_hygiene` | Code & Repo Hygiene | 30s |
| fast | `json_schema_policy` | Code & Repo Hygiene | 30s |
| fast | `build_core_tests` | Build & Tests | 60s |
| fast | `test_core_suite` | Build & Tests | 90s |
| fast | `build_godot_host` | Build & Tests | 60s |
| fast | `godot_import` | Host Selftests & Lifecycle | 900s |
| fast | `data_integrity` | Host Selftests & Lifecycle | 60s |
| fast | `bridge_removal` | Host Selftests & Lifecycle | 60s |
| fast | `asset_registry` | Host Selftests & Lifecycle | 60s |
| fast | `player_panels_uitest` | Host Selftests & Lifecycle | 60s |
| fast | `panel_bind_lifecycle` | Host Selftests & Lifecycle | 60s |
| fast | `save_load_failure` | Save Stores & Persistence | 60s |
| fast | `holdfast_save` | Save Stores & Persistence | 60s |
| fast | `inventory_save` | Save Stores & Persistence | 60s |
| fast | `journal_save` | Save Stores & Persistence | 60s |
| fast | `playable_shell` | Campaign Smoke | 60s |
| fast | `day1_onboarding` | Campaign Smoke | 60s |
| fast | `real_campaign_journey` | Campaign Smoke | 90s |
| performance | `runtime_scale_performance` | Performance | 60s |
| fast | `expansions_completeness` | Campaign Smoke | 60s |
| fast | `survivors_selftest` | Host Selftests & Lifecycle | 90s |
| fast | `expedition_selftest` | Host Selftests & Lifecycle | 90s |
| fast | `triad_drift` | Drift & Architecture Gates | 30s |
| fast | `cli_catalog_drift` | Drift & Architecture Gates | 30s |
| fast | `save_store_matrix_drift` | Drift & Architecture Gates | 30s |
| fast | `architecture_map_drift` | Drift & Architecture Gates | 30s |
| fast | `compiler_warning_baseline` | Drift & Architecture Gates | 90s |
| fast | `docs_index_drift` | Drift & Architecture Gates | 30s |
| fast | `forbidden_core_apis` | Source Policy & Lint Gates | 30s |
| fast | `catch_policy_lint` | Source Policy & Lint Gates | 30s |
| fast | `persistent_filename_registry` | Source Policy & Lint Gates | 30s |
| fast | `central_package_management` | Source Policy & Lint Gates | 30s |
| fast | `doc_link_portability` | Source Policy & Lint Gates | 30s |
| fast | `lfs_health_check` | Source Policy & Lint Gates | 30s |
| fast | `legacy_asset_path` | Source Policy & Lint Gates | 30s |
| fast | `legacy_reference` | Source Policy & Lint Gates | 30s |
| fast | `core_systems_catalog_drift` | Architecture & Catalog Gates | 30s |
| fast | `catalog_registry_drift` | Architecture & Catalog Gates | 30s |
| fast | `agent_rulebooks_sync` | Architecture & Catalog Gates | 30s |
| fast | `ui_panel_catalog_drift` | Architecture & Catalog Gates | 30s |
| fast | `expansions_catalog_drift` | Architecture & Catalog Gates | 30s |
| fast | `ui_panel_contracts_test` | Build & Tests | 60s |
| fast | `audio_catalog_drift` | Architecture & Catalog Gates | 30s |
| fast | `agent_skills_catalog_drift` | Architecture & Catalog Gates | 30s |
| fast | `audio_cue_integrity_gate` | Build & Tests | 60s |
| fast | `campaign_envelope_fuzz_test` | Build & Tests | 60s |
| fast | `case_alias_guard` | Repository hygiene | 30s |
| fast | `selftest_manifest_drift` | Drift guard | 60s |
| full | `export_parity` | Release | 300s |

## Tier contract

- **fast** — pre-merge standard: build + unit suite + data integrity + bridge + asset registry + drift guards + case alias guard + save/failure UX smoke. Target < 10 min on a clean machine.
- **full** — shippable standard: everything in fast plus runtime-scale performance and `export_parity` (exported-build packaged-data parity; requires a fresh `scripts/ci/export-build.sh` artifact — on runners without export templates, run the export on a capable machine and verify the artifact, per docs/RELEASE_EXPORT.md).
- **performance** — long runtime-scale runs; never hides release requirements.
