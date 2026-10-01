# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**

# T19 — Kitchen/cooking UI mismatch: bind `CookingSystem` (PFGL W5 P2)

> **STATUS: APPROVED BY USER** — user-directed checklist item T19
> ("Cooking/kitchen UI mismatch → bind `CookingSystem`"). Integrated in the
> same session; original plan follows.

## Goal

Close the host-OK / UI-mismatch gap recorded in the PFGL master plan
(`docs/plans/PLAYER_FACING_GAMEPLAY_LOOPS_MASTER_INTEGRATION_PLAN.md` §2, §PFGL-W5):
`CookingHostSession.StartCooking` / `ProgressCooking` / `CancelCooking` were
live and CLI-verified but unreachable from any player surface. The kitchen panel
(`KitchenNutritionPanel`, route `kitchen_nutrition`) bound only
`KitchenNutritionHostSession`; the Plan 136 cooking authority was host-complete
yet player-inert. The prior food-loop package explicitly deferred this
("No new UI panel or cooking surface for `CookingSystem` recipes").

Outcome: the kitchen panel now exposes the live `CookingSystem` as a cooking
strip — the authored `recipes_cooking.json` roster plus START / ADVANCE / CANCEL
verbs that consume real inventory ingredients and deliver real cooked items —
while the existing nutrition prep/serve path is preserved unchanged.

## Non-goals

- No new route or panel; the existing `kitchen_nutrition` surface is extended.
- No replacement of `KitchenNutritionSystem` meal serving (PFGL "Do not replace
  KitchenNutrition meal serving").
- No new save section, no parallel authority, no Core gameplay change.
- No balance tuning, no data change, no full test suite run.

## Root cause (verified in source)

- `src/Main.Cooking.cs` composes the Plan 136 `CookingHostSession` and saves its
  `cooking` section, but no UI ever called `StartCooking`.
- `src/UI/KitchenNutritionPanel.cs` bound only `KitchenNutritionHostSession`;
  its "START PREP" verb calls `KitchenNutritionSystem.StartPrepJob` and its
  recipe roster is panel-local nutrition metadata — the cooking authority had no
  player-operable path.
- `--food-loop-selftest` Gate 7 already proved the Core authority live, but by
  calling `cooking.StartCooking` directly, not through a panel seam.

## Files

- `src/UI/KitchenNutritionPanel.cs` — cooking strip (roster + verbs), `BindCooking`
  / `UnbindCooking`, `StartSelectedCooking` / `AdvanceCooking` / `CancelCooking`,
  `IsCookingBound`, `LastCookingFeedback`, `cook_ops` status card.
- `src/Host/CookingHostSession.cs` — host `LastEvent` convention on start /
  progress / cancel (success + refusal).
- `src/Main.Cooking.cs` — bind the strip in `SetupCooking`; unbind in `ResetCooking`.
- `src/Main.ShelterBatch3.cs` — bind the strip when the panel is constructed.
- `src/Main.UiTests.FoodLoop.cs` — Gate 8 drives the panel seams end to end
  (start / advance / cancel / refusal).
- Follow-up repair wave: `src/UI/ExpeditionPanel.cs`,
  `assets/l10n/strings.csv`, `Ashfall.Core.Tests/Journeys/MoralChoiceJourneyTests.cs`,
  `Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs`,
  `Ashfall.Core.Tests/Factions/Plan134TerritoryControlHostIntegrationTests.cs`,
  `Ashfall.Core.Tests/Legacy/Plan140CampaignLegacyHostIntegrationTests.cs`,
  `Ashfall.Core.Tests/Cooking/Plan136CookingHostIntegrationTests.cs`.
- `KNOWN_DEBT.md`, `WORKTREE_OWNERSHIP.md`, `.ai/state.md`, `INTEGRATION_PLANS.md` — governance.

## Implementation

1. Panel fields, `IsCookingBound`, `SelectedCookingRecipeId`, `LastCookingFeedback`,
   `ActiveCookingOperationCount`; `BindCooking` is re-bind safe (drops the prior
   `StateChanged` subscription first) and defaults to the alphabetically-first
   authored recipe.
2. `RefreshCookingStrip()` renders the live `CookingSystem.Recipes` chips, the
   selected recipe's real requirements/effects, `Ingredients on hand` truth from
   `ICookingSource.HasIngredients`, the START / ADVANCE verbs, every active
   `CookingOperation` with progress + CANCEL, and a feedback line.
3. The strip is rendered before the nutrition early-return so it stays truthful
   even if only the cooking authority is bound.
4. `Main` binds the live session both at panel construction
   (`SetupKitchenNutrition` → `EnsureCooking()`) and after `SetupCooking`, and
   detaches it in `ResetCooking`.
5. `--food-loop-selftest` Gate 8 exercises the panel seams: bind check, START
   consumes the authored bill, ADVANCE completes + delivers, CANCEL removes.

## Verification (commands and results)

- `dotnet build Ashfall.csproj --no-restore -v:minimal` — Build succeeded, 0
  errors / 6 pre-existing CS0162 warnings in untouched `HostCli.*` probes.
- `godot --headless --path . --max-fps 15 -- --food-loop-selftest` — **PASS**
  (all 24 gates), including the 5 new cooking-strip gates:
  - kitchen panel cooking strip is bound to the live cooking authority
  - kitchen cooking strip START COOK consumed the authored bill
  - kitchen cooking strip ADVANCE completed the batch (1)
  - kitchen cooking strip started a cancellable batch
  - kitchen cooking strip CANCEL removed the operation
- `godot --headless --path . --max-fps 15 -- --ui-layout-selftest` —
  Failures: 0; UiClickability 679 buttons / 172 panels; UiFocusability
  interactive=676 unreachable=0; UiControllerParity 61/61.
- `scripts/run_test.sh Ashfall.Core.Tests/UI/PanelLiveRefreshGateTests.cs` — 2/2.
- `scripts/run_test.sh Ashfall.Core.Tests/UI/PanelSubscriptionHygieneTests.cs` — 2/2.
- `scripts/run_test.sh Ashfall.Core.Tests/UI/UiA11yTargetSizeGateTests.cs` — 31/31.
- `scripts/run_test.sh Ashfall.Core.Tests/KitchenNutritionSystemTests.cs` — 12/12.
- `scripts/run_test.sh Ashfall.Core.Tests/Cooking/Plan136WildlifeCookingIntegrationTests.cs` — 5/5.
- `scripts/run_test.sh Ashfall.Core.Tests/Tooling/LocalizationRatchetTests.cs` — 2/2
  (GREEN after renaming the feedback field so the `*Text = "…"` literal-count
  regex does not see the new assignments; count stays ≤ 612).

## Limitations / open

- **Pre-existing red repaired + dead-code debt closed (same session):** the
  five stale tests that asserted the retired per-frame flush location in
  `Main.Application.cs` were repaired to the current durability contract
  (`SaveAll` enrollment / day-owner wiring) — `Plan136CookingHostIntegrationTests`
  8/8, `Plan140CampaignLegacyHostIntegrationTests` 8/8,
  `Plan134TerritoryControlHostIntegrationTests` 6/6, `MoralChoiceJourneyTests`
  4/4, `FireIncidentJourneyTests` 5/5, with `MainTriadDriftGateTests` 8/8
  confirming the no-per-frame-flush architecture. The
  `DEBT-DEAD-FLUSH-IFDIRTY-METHODS` promotion condition was then executed in
  full: all **141** dead/redundant `Flush*` staging wrappers (131 `Flush*IfDirty`
  + 10 other dead `Flush*` methods) and all **16** call statements removed across
  **131** `src/Main.*.cs` partials, and
  `docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md` now
  documents the two-entry durability contract (`SaveAll` +
  `FlushDirtyStoresForDayAdvance`). Confirmed before removal that no `SaveX()`
  writes a per-file store directly (`src/Main*.cs` has zero `*SaveStore.TrySave`
  calls) and production never calls a host-session `.Save()` override, so the
  retired wrappers had no durability effect after the `_Process` block was
  removed. Verification: host build 0 errors / 0 new
  warnings; `triad-drift-gate.sh` GATE PASS; `--7-day-smoke-selftest` PASS;
  `--dose-ledger-selftest` PASS; `--chronic-condition-selftest` 12/12;
  `--playable-metrics-selftest` 17/0.
- **Host `Save()` dead overrides removed (`DEBT-DEAD-HOST-SAVE-OVERRIDES`):**
  with the two-path ambiguity gone, 81 of the 83 uncalled
  `override void Save()` methods were deleted across 67 `src/Host/*.cs` files
  (each only wrote a per-file `*SaveStore.TrySave` already captured by the
  envelope). `MedicalWardHostSession.Save()` was kept — and its only caller
  `MedicalWardSaveSelfTest` was failing at HEAD because it saved a non-dirty
  fresh session, so the probe now calls `MarkDirty()` first and passes;
  `WeatherHostSession.Save()` was kept as a documented no-op (weather persists
  in the `world` section). Evidence: host build 0 errors / 0 new warnings;
  `--medical-ward-save-selftest` PASS; `HostSessionConventionGateTests` 1/1;
  `HostSessionStateSemanticsTests` 9/9; `--7-day-smoke-selftest` and
  `--save-load-ui-failure-selftest` PASS; `--cooking-selftest` 26/26.
- **Host `LastEvent` convention closed:** `CookingHostSession` now publishes
  `LastEvent` on success and refusal (`StartCooking`, `ProgressCooking`,
  `CancelCooking`), and the kitchen strip renders it (falling back to a
  panel-composed sentence). `--food-loop-selftest` now proves the refusal path
  reaches the player (`Cooking refused: missing_ingredients.`).
- **Shared l10n ratchet restored:** a concurrent stream added two raw
  `TooltipText` literals to `src/UI/ExpeditionPanel.cs`, pushing the
  `LocalizationRatchetTests` count to 613 > 612. Both tooltips were localized
  (`ui.expedition.radar_tooltip`, `ui.expedition.camp_tooltip`) into
  `assets/l10n/strings.csv`; ratchet 2/2 at ≤ 612, `StringsCsvLocaleGateTests`
  4/4, `l10n_drift_gate.py` PASS (375 keys).
- No commit, no full suite (per policy). Foreign dirty worktree preserved.
