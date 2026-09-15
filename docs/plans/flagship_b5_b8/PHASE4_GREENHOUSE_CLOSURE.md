# Phase 4 — Greenhouse Loop Closure (B5–B8 / Plan 64) — increment 1 landed

> Reconciliation-driven scope: ~80% of the brief's Plan 64 was already live
> (treatment path, tray-moisture model, apiculture, deterministic blight
> rolls, powered grow-light coupling via the Plan 162 agriculture wrapper,
> harvest→nutrition closure). This increment lands the reconciliation's
> remaining B5 items.

## Landed in this increment

### 1. Nutrient band — the canonical nutrient consumer (§7.5, §7.7)
- Additive `GreenhousePlotState.nutrientLevel` (0..1; legacy saves restore 0 —
  no free prevention).
- `GreenhouseSystem.ApplyNutrients(plotIndex, out consumedItemId)`: raises the
  band by `NutrientApplicationLevel` (0.5), capped at 1; blocked on
  fallow/failed plots (nutrients feed a crop, not the soil).
- `GreenhouseHostSession.ApplyNutrients`: inventory check → authoritative
  commit → consume `item_hydroponic_nutrients` exactly once — the same
  atomic discipline as watering; blocked reasons name the missing piece.
- Real player route: GreenhousePanel "DOSE NUTRIENTS" button →
  `Main.World` dispatch case `dose_nutrients`.
- Decays `NutrientDecayPerDay` (0.1) — dosing is a recurring cost, not a
  permanent buff. Item supply chain: `recipe_aeroponics_nutrient_batch`
  (craft) / doser-blueprint enrichment chain — research gates the recipes,
  the item is the input.

### 2. Visible blight-risk decomposition (§7.7 — the actual B5 gap)
- `GreenhouseSystem.GetBlightRiskProfile(plotIndex, hasWater)` → read-only
  `BlightRiskProfile`: `BaseChancePerDay · ResistanceFactor ·
  ContaminationPressure · DroughtStress − NutrientReduction`, clamped [0,1].
- Consumes **no RNG** and mutates nothing (pinned by test) — safe for UI
  bind/refresh; the profile is the exact TickDay outbreak-roll input, not a
  forecast of the roll (no false precision, §17.3).
- Parity: at `nutrientLevel` 0 the legacy chance is bit-identical (pinned).
  Prevention is subtractive, bounded (`NutrientBlightRiskReduction` 0.04 at a
  full band), and can never make risk negative.
- The deterministic blight-roll stream still advances every tick (never
  skipped) — same-seed replay ledger unchanged (pinned).
- Drought blight (`DroughtBlightRatePerDay`) remains an unconditional neglect
  penalty outside the probability decomposition — nutrients do not compensate
  for forgetting water (deliberate; documented).

### 3. Microclimate winter pressure (§7.8/§7.9 — honest research consumer)
- `AgricultureSystem.WinterAdjustedLightPermille(...)` — pure, deterministic
  Core helper: deep-winter windows (`window_deep_freeze`,
  `window_long_winter`) reduce effective light to
  `DeepWinterLightPermille` (600); the researched microclimate capability
  compensates fully **but only with a powered greenhouse room** — research is
  permission, power is the physical input (capability ≠ built
  infrastructure, §15.3).
- Host wiring in `BuildAgricultureEnvironment` (Plan 162 layer) queries
  `HasCapability("knowledge_greenhouse_microclimate")` live (Phase 1
  contract, never cached) and reads the season from the weather authority
  (greenhouse never owns season state). Non-winter seasons pass through
  unchanged (legacy parity).
- `knowledge_hydroponics` disposition: its runtime consumer is the nutrient
  dosing loop's item supply chain (doser-blueprint recipe gate) — the
  capability gate on the host dosing action is a Phase 9 candidate; the
  physical chain research→recipe→item→dose is closed.

### 4. Audit conclusions (closed loops, no change needed)

| Loop | Evidence |
|---|---|
| Harvest → nutrition | every crop id has a `nutrition_profiles.json` entry (kcal closure); `greenhouse_items.json` + `crop_strains.json` cover the rest; no orphan outputs → no new recipes |
| Power → growth | `AgricultureSystem` derives `lightFactor` from `LightingAvailabilityPermille` = `IsRoomPowered("room_greenhouse")`; `TickDay` grows only with light |
| Water → tray | host checks/consumes canonical water items before `System.Water` (Phase 0 finding holds) |
| Apiculture | live (`ApicultureSystem`, pollination harvest bonus) |

## Flagged for the content stream (not edited — catalog balance is not this package's authority)

1. `recipe_aeroponics_nutrient_batch`: 1× `item_hydroponic_nutrients` +
   2× `clean_water` → 2× `item_hydroponic_nutrients` — **net +1 nutrient per
   cycle** (feed-the-output-back arbitrage; violates §13.3/§13.4
   conservation). Suggest ingredients that consume more than they yield.
2. `assemble_pure_sine_solar_inverter` (Phase 2 flag, restated): consumes the
   inverter, produces `battery_pack`×3 — likely a result-id authoring error.

## Deliberately deferred

| Item | Target | Note |
|---|---|---|
| Powered-ventilation blight contributor | Phase 9 | would change live risk when powered; parity-preserving form needs a dedicated balance decision |
| Host dosing action research gate (`knowledge_hydroponics`) | Phase 9 | physical chain already gates via crafted item; capability check is defense-in-depth |
| Labor/busy time for planting/dosing/treatment | Phase 9/audit | duty-roster audit (reconciliation Phase 4 gate) not yet performed |
| 30-day greenhouse balance harness (§7.14) | Phase 8 | scenario set |
| Cold-can viability model (microclimate's fuller scope) | follow-on | `TemperaturePenaltyC` is declared but unused by growth |

## Verification record (2026-09-13)

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Greenhouse/GreenhousePhase4LoopClosureTests.cs` | 13/13 PASS (new) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseSystemTests.cs` | 9/9 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseCommandTests.cs` | 3/3 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseCropExpansionTests.cs` | 6/6 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseEquipmentScalingTests.cs` | 3/3 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs` | 21/21 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/MicroLocationGreenhouseIntegrationTests.cs` | 13/13 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Save/B5B8Phase0SaveFixtureTests.cs` | 6/6 PASS (greenhouse fixture re-captured, additive `nutrientLevel`) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs` | 199/199 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Phase1SharedContractsTests.cs` | 13/13 PASS |
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings, 325 catalogs |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

## Files changed

- `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` (nutrient band, application, risk profile, decay, additive save field)
- `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` (winter light helper)
- `src/Host/GreenhouseHostSession.cs` (ApplyNutrients — atomic inventory→commit)
- `src/Main.World.cs` (dispatch case)
- `src/Main.Plans162_165.cs` (winter light wiring via Core helper + live capability query)
- `src/UI/GreenhousePanel.cs` (DOSE NUTRIENTS button)
- `Ashfall.Core.Tests/Fixtures/B5B8_Phase0/greenhouse_phase0.json` (re-captured)
- `Ashfall.Core.Tests/Greenhouse/GreenhousePhase4LoopClosureTests.cs` (new, 13 tests)
- `docs/plans/flagship_b5_b8/PHASE4_GREENHOUSE_CLOSURE.md` (this file)
