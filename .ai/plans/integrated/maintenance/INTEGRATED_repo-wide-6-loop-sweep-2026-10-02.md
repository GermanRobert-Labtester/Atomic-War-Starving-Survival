# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Repo-wide 6-loop find → repair → harden sweep (fast-tier gates)

> **STATUS: APPROVED BY USER**

User-directed ("do 6 looping phases of find issues, repair them, harden spots
where issues found and repeat repo wide!").

## Method

Ran the canonical fast-tier gate runner (`scripts/ci/run-gates.py`) in batches
across all 69 fast gates, catalogued every failure, repaired it with its owning
generator/owner, then hardened the failure class. Repeated for 6 loops.

## The 6 loops

1. **Generated-artifact drift (3 gates).** `pot_template_drift`,
   `save_store_matrix_drift`, `ui_design_map_drift` were stale. Repaired by running
   each owning generator.
2. **Slow-gate timeouts + stale host.** `docs_index_drift` (172s vs 120s),
   `port_contract_gate` (103s vs 60s), `input_map_contract` (43s vs 30s) timed out;
   `content_certification` failed on a stale host. Repaired by raising budgets and
   rebuilding the host.
3. **Plan-audit drift.** `plan_integration_audit_drift` was stale; regenerated
   `RECENT_PLAN_INTEGRATIONS_AUDIT.md`.
4. **Build/test timeouts.** `build_core_tests` (108s vs 120s), `coverage_gate`
   (96s vs 60s) timed out and blocked `ui_panel_contracts_test`,
   `audio_cue_integrity_gate`, `campaign_envelope_fuzz_test`; `compiler_warning_baseline`
   (three `-t:Rebuild`s: Core.Tests 125s + Core 58s + host ~140s) exceeded the 180s
   ceiling. Repaired budgets; raised the runner upper bound to 420s for the
   3-rebuild gate only.
5. **Godot runtime selftests (17).** `godot_import`, `data_integrity`,
   `player_panels_uitest`, `panel_bind_lifecycle`, `save_load_failure`,
   `holdfast_save`, `inventory_save`, `journal_save`, `playable_shell`,
   `day1_onboarding`, `first_hour_onboarding_journey`, `real_campaign_journey`,
   `expansions_completeness`, `survivors_selftest`, `expedition_selftest`,
   `ui_layout_selftest` — all PASS.
6. **Re-drift + near-cap budget + missing hints.** `ui_design_map` re-staled and
   `docs_index_drift` passed at 178.2s (razor-thin under the 180s cap); two drift
   gates had no remediation hint. Repaired by regenerating, raising
   `docs_index_drift` to 300s, adding remediation hints, and adding
   `scripts/ci/regen-generated-docs.sh` (correct order: docs index **last**).

## Hardening

- `scripts/ci/run-gates.py`: `MAX_GATE_TIMEOUT_SECONDS` 180 → 420, with a comment
  recording the measured rebuild times. Other gates declare ≤180s, so they are
  unaffected.
- `docs/ci/CI_GATE_MANIFEST.json`: raised `docs_index_drift` (300),
  `port_contract_gate` (180), `input_map_contract` (90), `build_core_tests` (180),
  `coverage_gate` (180), `compiler_warning_baseline` (420); added remediation hints
  to `plan_integration_audit_drift` and `ui_design_map_drift`.
- New `scripts/ci/regen-generated-docs.sh` (+ `--check`) regenerates every
  generated artifact in the dependency order that keeps the docs index valid.

## Exact files

- `scripts/ci/run-gates.py`, `scripts/ci/regen-generated-docs.sh` (new)
- `docs/ci/CI_GATE_MANIFEST.json`, `docs/ci/GATE_INVENTORY.md`
- `docs/INDEX.md`, `docs/ui/UI_DESIGN_MAP.md`, `docs/ui/ui_design_map.json`
- regenerated `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md`,
  `docs/plans/RECENT_PLAN_INTEGRATIONS_AUDIT.md`, `assets/l10n/template.pot`
- `.ai/state.md`, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, this plan

## Evidence

- `run-gates.py --check-only`: manifest valid (73 gates, 69 fast)
- All 69 fast gates PASS (individually across the six loops)
- `regen-generated-docs.sh --check`: POT, save-store matrix, UI design map, plan
  audit, gate inventory, docs index all in sync
- `git diff --check` clean on the changed tool/manifest/script
- No gameplay, save, determinism, or authority change; no full test suite; no commit.

## Note

A concurrent agent (`agy`) held the machine near load 30 throughout, which is why
several gates measured at the top of their budgets. The raised budgets and the
420s upper bound keep the gates robust under that contention without changing any
gate's semantics.