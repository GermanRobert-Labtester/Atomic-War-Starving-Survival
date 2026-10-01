# CI Gate Inventory (Plan VIII · Task 24.1)

Generated from `docs/ci/CI_GATE_MANIFEST.json` — 73 gates, 69 fast. Regenerate with `python3 scripts/ci/run-gates.py --write-inventory docs/ci/GATE_INVENTORY.md`. The manifest is the single authority: add or change gates THERE, never in prose only.

Runtimes below are budgeted timeouts (enforced ceiling), not measured durations; measured durations land in every `--report-json` run (Task 24.10 budgets).

| Tier | Gate | Category | Timeout ceiling | Critical | Depends on |
|---|---|---|---|---|---|
| fast | `whitespace_hygiene` | Code & Repo Hygiene | 30s | yes | — |
| fast | `json_schema_policy` | Code & Repo Hygiene | 30s | yes | — |
| fast | `pot_template_drift` | Code & Repo Hygiene | 30s | yes | — |
| fast | `build_core_tests` | Build & Tests | 120s | yes | — |
| full | `test_core_suite` | Build & Tests | 120s | yes | — |
| fast | `build_godot_host` | Build & Tests | 180s | yes | — |
| fast | `godot_import` | Host Selftests & Lifecycle | 180s | yes | — |
| fast | `data_integrity` | Host Selftests & Lifecycle | 60s | yes | build_godot_host |
| fast | `bridge_removal` | Host Selftests & Lifecycle | 60s | yes | build_godot_host |
| fast | `asset_registry` | Host Selftests & Lifecycle | 60s | yes | build_godot_host |
| fast | `player_panels_uitest` | Host Selftests & Lifecycle | 60s | yes | build_godot_host |
| fast | `panel_bind_lifecycle` | Host Selftests & Lifecycle | 60s | yes | build_godot_host |
| fast | `save_load_failure` | Save Stores & Persistence | 60s | yes | build_godot_host |
| fast | `holdfast_save` | Save Stores & Persistence | 60s | yes | build_godot_host |
| fast | `inventory_save` | Save Stores & Persistence | 60s | yes | build_godot_host |
| fast | `journal_save` | Save Stores & Persistence | 60s | yes | build_godot_host |
| fast | `playable_shell` | Campaign Smoke | 60s | yes | build_godot_host |
| fast | `day1_onboarding` | Campaign Smoke | 60s | yes | build_godot_host |
| fast | `first_hour_onboarding_journey` | Campaign Smoke | 60s | yes | build_godot_host |
| fast | `real_campaign_journey` | Campaign Smoke | 180s | yes | build_godot_host |
| performance | `runtime_scale_performance` | Performance | 60s | no | build_godot_host |
| fast | `expansions_completeness` | Campaign Smoke | 60s | yes | build_godot_host |
| fast | `survivors_selftest` | Host Selftests & Lifecycle | 90s | yes | build_godot_host |
| fast | `expedition_selftest` | Host Selftests & Lifecycle | 90s | yes | build_godot_host |
| fast | `triad_drift` | Drift & Architecture Gates | 30s | yes | — |
| fast | `cli_catalog_drift` | Drift & Architecture Gates | 30s | yes | — |
| fast | `save_store_matrix_drift` | Drift & Architecture Gates | 30s | yes | — |
| fast | `architecture_map_drift` | Drift & Architecture Gates | 180s | yes | — |
| fast | `plan_integration_audit_drift` | Drift & Architecture Gates | 180s | yes | — |
| fast | `ui_design_map_drift` | Drift & Architecture Gates | 30s | yes | — |
| fast | `compiler_warning_baseline` | Drift & Architecture Gates | 180s | yes | — |
| fast | `docs_index_drift` | Drift & Architecture Gates | 120s | yes | — |
| fast | `forbidden_core_apis` | Source Policy & Lint Gates | 30s | yes | — |
| fast | `catch_policy_lint` | Source Policy & Lint Gates | 30s | yes | — |
| fast | `persistent_filename_registry` | Source Policy & Lint Gates | 30s | yes | — |
| fast | `central_package_management` | Source Policy & Lint Gates | 30s | yes | — |
| fast | `doc_link_portability` | Source Policy & Lint Gates | 180s | yes | — |
| fast | `lfs_health_check` | Source Policy & Lint Gates | 30s | yes | — |
| fast | `legacy_asset_path` | Source Policy & Lint Gates | 30s | yes | — |
| fast | `legacy_reference` | Source Policy & Lint Gates | 30s | yes | — |
| fast | `core_systems_catalog_drift` | Architecture & Catalog Gates | 30s | yes | — |
| fast | `catalog_registry_drift` | Architecture & Catalog Gates | 30s | yes | — |
| fast | `agent_rulebooks_sync` | Architecture & Catalog Gates | 30s | yes | — |
| fast | `ui_panel_catalog_drift` | Architecture & Catalog Gates | 30s | yes | — |
| fast | `expansions_catalog_drift` | Architecture & Catalog Gates | 30s | yes | — |
| fast | `ui_panel_contracts_test` | Build & Tests | 60s | yes | — |
| fast | `audio_catalog_drift` | Architecture & Catalog Gates | 30s | yes | — |
| fast | `agent_skills_catalog_drift` | Architecture & Catalog Gates | 30s | yes | — |
| fast | `audio_cue_integrity_gate` | Build & Tests | 60s | yes | — |
| fast | `campaign_envelope_fuzz_test` | Build & Tests | 60s | yes | — |
| fast | `case_alias_guard` | Repository hygiene | 30s | no | — |
| fast | `selftest_manifest_drift` | Drift guard | 60s | no | — |
| full | `export_parity` | Release | 180s | yes | build_godot_host |
| fast | `release_workflow_parity` | Release | 120s | yes | — |
| fast | `repository_size_budget` | Code & Repo Hygiene | 120s | yes | — |
| fast | `compile_set_reachability` | Code & Repo Hygiene | 120s | yes | — |
| fast | `test_only_production_source` | Code & Repo Hygiene | 120s | yes | — |
| fast | `uid_sidecar_gate` | Code & Repo Hygiene | 30s | yes | — |
| fast | `scene_binding_truth` | Source Policy & Lint Gates | 60s | yes | — |
| fast | `coverage_gate` | Quality & Verification | 60s | yes | — |
| fast | `content_acceptance_pipeline` | Quality & Verification | 120s | yes | — |
| fast | `port_contract_gate` | Quality & Verification | 60s | yes | — |
| fast | `input_map_contract` | Quality & Verification | 30s | yes | — |
| fast | `version_gate` | Release | 30s | yes | — |
| fast | `changelog_drift` | Release | 30s | no | — |
| full | `save_support_window` | Release | 120s | yes | — |
| fast | `ui_layout_selftest` | UI & Accessibility | 180s | no | — |
| fast | `catalog_audit` | Source Policy & Lint Gates | 120s | yes | — |
| fast | `content_certification` | Host Selftests & Lifecycle | 60s | no | build_godot_host |
| fast | `content_utilization` | Host Selftests & Lifecycle | 120s | no | build_godot_host |
| fast | `gotools_test` | Build & Tests | 180s | no | — |
| fast | `gate_inventory_drift` | Drift & Architecture Gates | 30s | no | — |
| fast | `gotools_vet` | Build & Tests | 180s | no | — |

## Tier contract

- **fast** — pre-merge standard: build + unit suite + data integrity + bridge + asset registry + drift guards + case alias guard + save/failure UX smoke. Target < 10 min on a clean machine.
- **full** — shippable standard: everything in fast plus runtime-scale performance and `export_parity` (exported-build packaged-data parity; requires a fresh `scripts/ci/export-build.sh` artifact — on runners without export templates, run the export on a capable machine and verify the artifact, per docs/RELEASE_EXPORT.md).
- **performance** — long runtime-scale runs; never hides release requirements.
