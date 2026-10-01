# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Food Loop Verification + Reasonable Player Week-1 Bot — 2026-09-30

> **STATUS: APPROVED BY USER**
> User tasks 5 and 6 (verbatim scope): "Verify the food loop from the UI.
> Confirm the starter cooking recipes are actually known on a fresh game (the
> log says 0 discovered, which may just be a discovery counter). Then check
> that food → cook → eat works from the panels and not only from tests." and
> "Script a 'reasonable player' bot. Add a deterministic week-1 policy
> (ration, plant, fortify) and run it over several seeds and difficulty
> presets. The smoke test shows survivors at hunger and thirst 100 by day 7
> with no actions, which is expected. The bot shows whether doing the obvious
> things keeps them alive."

## Goal

1. Seal the fresh-game cooking recipe discovery wipe (`[Cooking] 0 recipes
   loaded` on every healthy boot) and prove the starter recipes are known.
2. Prove food → cook → eat through the real composed game and the same host
   seams the panels call (kitchen prep → pantry → serve; inventory consume),
   via a new headless probe `--food-loop-selftest`.
3. Add a deterministic "reasonable player" week-1 bot probe
   `--reasonable-player-selftest` (WorldPlaytest-harness pattern: real Core
   authorities + real data catalogs, seed-swept) with policy legs ration /
   cook / plant / fortify, run over seeds {1986, 2026, 9001, 424242} × all
   four difficulty presets, plus no-action baselines and a greenhouse-remnant
   plant variant, and report week-1 survival outcomes.

## Non-goals

- No new UI panel or cooking surface for `CookingSystem` recipes (finding is
  reported, not designed here).
- No balance changes; the bot reports outcomes, it does not tune data.
- No new save section, no parallel authority, no full test suite run.
- No commit unless the user asks.

## Root cause (verified in source + reproduced headless)

`src/Main.Cooking.cs:47` calls
`session.RestoreState(CookingSaveStore.TryLoad())` unconditionally. In a
healthy production save system the loose `cooking_save.json` must not exist
(sections live inside the campaign envelope; the journey selftest gates
exactly that), so `TryLoad()` returns null on every boot and
`CookingSystem.RestoreState(null)` replaces the state with a fresh
`CookingState()` — wiping the 15 `discoveredRecipeIds` that
`LoadAuthoredRecipes` populated moments earlier. Recipes stay registered and
cookable, but the census logs 0 and the first `SaveAll` persists the empty
discovery list permanently. `SetupKitchenNutrition` in `Main.ShelterBatch3.cs`
shows the correct pattern (`TryLoad() ?? new ...`).

## Files

- `src/Main.Cooking.cs` — null-guard the legacy direct-file restore (fix).
- `src/Main.UiTests.FoodLoop.cs` — NEW: `RunFoodLoopSelfTestAndQuit()`.
- `src/Host/HostCli.ReasonablePlayerBot.cs` — NEW: bot harness + probe.
- `Assets/Ashfall.Core/HostCliRegistry.cs` — enum members + descriptors.
- `src/Host/HostCli.cs` — enum mirror, parse, help lines.
- `src/Main.Application.cs` — dispatch cases.
- `WORKTREE_OWNERSHIP.md`, `.ai/state.md` — governance.

## Verification

1. `godot --headless --path . --max-fps 15 -- --food-loop-selftest` — all
   gates PASS, including `DiscoveredRecipesCount == 15` on a fresh game.
2. `godot --headless --path . --max-fps 15 -- --reasonable-player-selftest`
   — determinism/divergence/policy-execution gates PASS; week-1 survival
   table reported per seed × preset vs baseline.
3. `bin/run-scoped-tests Ashfall.Core.Tests/Cooking/Plan136CookingHostIntegrationTests.cs Ashfall.Core.Tests/Cooking/Plan136WildlifeCookingIntegrationTests.cs` — no cooking regression.
4. `dotnet build Ashfall.csproj --no-restore -v:minimal` — 0 errors.
