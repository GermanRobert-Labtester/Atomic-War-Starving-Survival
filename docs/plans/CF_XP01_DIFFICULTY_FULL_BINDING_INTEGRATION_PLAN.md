# CF-XP01 — Difficulty Full Binding Integration Plan

Status: COMPLETE — Verified 2026-09-19. All 4 phases and all explicit acceptance criteria
satisfied under `claim-xp-wave1-difficulty-2026-09-18`. `--difficulty-selftest` 14/14 PASS;
`dotnet test --filter Difficulty` 19/19 PASS; port contract and CI gates conforming.

## Bounded outcome

Bind the already-landed XP-01 difficulty catalog/director to campaign creation,
the additive campaign manifest field `difficulty_preset_id`, the seven remaining
scalar consumer seams, the starting-cohort panel, and a focused headless
selftest. Standard/legacy values must remain neutral, unknown persisted IDs must
fail closed, and completion-history files owned by Wave 11 remain untouched.

## Non-goals

- No mid-campaign difficulty changes or New Game+ composition.
- No new difficulty save section or parallel campaign authority.
- No edits to `CampaignCompletionHistory`, `CompletionHistoryStore`,
  `Main.Endgame`, or their tests.
- No changes to unrelated pre-existing UI scene-binding failures.

## P0 premise evidence (before production edits)

| seam | current evidence | required additive hook |
|---|---|---|
| new-game selection | `src/Main.GameFlow.cs:151-203` receives cohort + supplies; `src/Main.UiPanels.cs:1582-1587` forwards panel selection | add preset ID to this existing transaction only |
| hunger/thirst | `Assets/Ashfall.Core/Survivors/NeedsSystem.cs:254-260` is the single base-drift site | multiply only hunger and thirst deltas |
| radiation | `Assets/Ashfall.Core/Radiation/RadiationSystem.cs:201-216,268-274` composes the effective rate before exposure | apply radiation scalar before existing clamps/resistance |
| disease | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs:434-479`, probability is clamped at line 470 | multiply onset probability before the existing [0,1] clamp |
| market | `Assets/Ashfall.Core/Economy/MarketSystem.cs:587-779`, `ExplainPrice` owns base/demand/factor/clamp order | multiply before authored floor/ceiling clamps |
| equipment | `Assets/Ashfall.Core/EquipmentConditionSystem.cs:310-328` is the wear delta site | multiply wear only; preserve condition floor and break/jam rules |
| crisis deadlines | `Assets/Ashfall.Core/Campaign/CrisisPredictionModel.cs:127-254` computes runway/horizon deadlines | scale deadline runway/horizon; 1.0 preserves current results |

## Phases and gates

1. Selection/persistence: manifest field, legacy-default migration, strict
   load validation, and sparing bonus through canonical inventory.
2. Consumers: one owner seam at a time with 1.0 parity assertions.
3. Panel/selftest: four authored presets, current read-only words + numbers,
   keyboard/controller-safe controls, and `--difficulty-selftest`.
4. Determinism: same seed × two presets produces distinct bounded outcomes;
   focused Core suites, data integrity, endings, and selftest pass.

## Explicit acceptance

- Four catalog presets are selectable only during new-game creation.
- A chosen ID round-trips in the campaign manifest; absent legacy IDs resolve
  to `difficulty_standard`; unknown IDs reject the load before live restore.
- Sparing adds `canned_food` and `iodine_pills` once through canonical inventory.
- All eight scalars have one existing owner seam and a standard/legacy parity
  assertion.
- Completion history remains Wave 11-owned and unchanged.

## Verification Evidence (2026-09-19)

- `godot --headless --path . -- --difficulty-selftest`: 14/14 PASS (catalog, scalar_bounds, legacy_parity, unknown_fail_closed, starting_bonus_authority, starting_bonus_once, standard_no_bonus, needs_consumer, radiation_consumer, disease_consumer, market_consumer, equipment_consumer, crisis_consumer, save_binding).
- `dotnet test --filter Difficulty`: 19/19 PASS (DifficultyPresetCatalogTests 8/8, DifficultyDirectorTests 11/11).
- `python3 scripts/ci/generate-port-contract.py --check`: 264 seams conforming, 0 deferred ratchet.
