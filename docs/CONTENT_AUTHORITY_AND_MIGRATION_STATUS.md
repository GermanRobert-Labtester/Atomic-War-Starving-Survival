# ASHFALL — Content Authority & Migration Status (Plans 47–50)

One index over the four content-authority domains. This page links; the
linked ledgers own their content — do not duplicate here.

| Domain | Authority | Proof contract | Live ledger / artifact |
|---|---|---|---|
| JSON property naming | Typed Core loader / DTO + `CatalogKeyNormalizer` dual-read | dual-read tests + `--data-integrity-selftest` + checksum stability | [docs/data/SNAKE_CASE_MIGRATION.md](data/SNAKE_CASE_MIGRATION.md) · `artifacts/snake-case-survey.json` |
| Godot assets | `res://assets/...` (LFS-tracked) | live consumer paths + `./scripts/ci/godot-asset-gate.sh` + case-collision gate | [docs/ASSET_MIGRATION_LEDGER.md](ASSET_MIGRATION_LEDGER.md) · `artifacts/asset-migration-batch-01-report.json` |
| Gameplay content | Systems/routes (deep chains) | `--content-utilization-selftest` incl. Plan 49 deep-chain gate | `artifacts/content-utilization.{json,md}` · `artifacts/content-utilization-deep-chain.json` |
| Narrative graphs | Normalized graph + flag ledger | `--narrative-continuity-selftest` (Plan 50) | `artifacts/narrative-continuity.{json,md}` |

## Status snapshot (Plan 47–50 wave)

- **Task 13 (snake_case wave 1):** 6 catalogs migrated, dual-read aliases
  retained, IDs untouched, integrity 0 errors. Tracker:
  `docs/data/SNAKE_CASE_MIGRATION.md`.
- **Task 14 (asset batch 1):** Unity-era art trees already fully migrated by
  earlier waves (legacy trees are `.gdignore` stubs); batch 01 closed the
  remaining root strays. Ledger: `docs/ASSET_MIGRATION_LEDGER.md`.
- **Task 15 (deep-chain gate):** three flagship chains hard-gated
  (research→recipe→craft, expedition→loot→inventory→use,
  faction→treaty→consequence→briefing) plus five warn-tier chains; offline
  and deterministic (`ContentDeepChainGate`).
- **Task 16 (narrative continuity):** corpus normalized (21 files, 1303
  nodes, 759 edges, 832 authored flag setters); structural lints hard-gated,
  flag audits warn-tier with a documented allowlist
  (`NarrativeContinuityAllowlist`). One Tier-1 dangling schedule repaired
  (`events.json`: day-51 follow-up pointed at the flag name instead of the
  `escalation_sabotage` event).

## Standing rules

1. Migrations are spelling/asset-path only — never rename stable IDs.
2. Legacy compatibility is removed only in a dedicated cleanup wave.
3. Baselines and allowlists are contracts — every entry needs a reason.
4. All scanners run offline (selftest/CI only), never on the startup path.
5. Artifact ordering is ordinal/deterministic; same repo → same artifact.
