# PHASE STATUS — THE GLASS ORCHARD (Expansion 05 / XI)

Audit date: post-Plan 91 (greenhouse item catalog expansion to 30 live entries).
Method: `ashfall-expansion-phase` read-only audit — current source outranks plans.
Scope of this pass: **Plan 91 supply-ecosystem integration cleanliness** plus the
standard five-phase matrix.

## Phase matrix

| Phase | Status | Evidence |
|---|---|---|
| 1. Core system & state contract | **COMPLETE** | `GreenhouseSystem.cs` — full cultivation engine: `Plant`/`Water`/`Harvest`/`TreatBlight`/`TickDay`; deep-copy `CaptureState`/`RestoreState` (anti-aliasing documented at `GreenhouseSystem.cs:47`); six state events; deterministic blight roll via `SeededRng(_seed * 397 + rollCount)` with persisted `blightRollCount` (A11, `GreenhouseSystem.cs:300-303`). `GreenhouseExpansionCatalog` = ID registry + 12-crop `CropCatalog` with `Get(seedItemId)` lookup. `ApicultureSystem` sub-system ticked daily. |
| 2. Authority data | **COMPLETE** | `greenhouse_items.json`: 30 live entries (Plan 91), `schema_version: 1`, wrapped array. Item definitions are the JSON authority. Note (observation, not violation): crop growth curves (`CropDef`) live code-side in `GreenhouseExpansionCatalog.CropCatalog` — item *definitions* are JSON, crop *tuning* is logic. |
| 3. Canonical IDs & references | **COMPLETE** | `GreenhouseExpansionCatalog.Items` constants match JSON IDs 1:1 (12 seeds, 5 equipment, 12 crops, `tainted_food`). `--data-integrity-selftest` PASS (0 errors, 208 catalogs) covers Tier-1/Tier-2 over these. Plan 91's 16 `item_greenhouse_*` IDs are integrity-clean and deliberately **not yet** referenced in Core constants (consumption is Plan 22 scope). |
| 4. Godot construction / wiring / tick / save | **COMPLETE** (1 host gap noted) | Construction: `Main.World.cs:41` `_greenhouse: GreenhouseHostSession`; `SetupGreenhouse()` called from lifecycle, gameflow, campaign services, and the `greenhouse_foundry` day owner. Tick: `Main.CampaignOwners.cs:193` `_m._greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0.04f)`. Inventory consumption: `GreenhouseHostSession.Plant` consumes seed (`GreenhouseHostSession.cs:99-113`); `Water` consumes `clean_water`/`irradiated_water` (`:115-128`); `TreatBlight` consumes `item_blight_treatment` with `iodine_pills` fallback (`:130-190`); `Harvest` adds yield + apiculture pollination bonus (`:192-206`). Save: checksummed `{State, Checksum}` envelope via `SaveStore<GreenhouseState>` (SaveStoreHub codec façade, atomic write, legacy bare-state fallback) + campaign envelope section `greenhouse` via `CaptureSection("greenhouse", …)` (`Main.World.cs:423-431`, Initiative #42 compliant). UI: `GreenhousePanel` reads live state. **Host gap:** `DefaultPlanterBoxCount = 4` is hardcoded (`GreenhouseHostSession.cs:19`) — inventory `item_planter_box` count does not drive plot count. |
| 5. Behavior / save / determinism / integration tests | **COMPLETE** | `--greenhouse-selftest` **24/24 PASS** (`GreenhouseHeadlessDemo`: gated planting, invalid-seed rejection, growth-to-mature ticks, clean vs tainted harvest, wheat unlock gate, save roundtrip). `GreenhouseCommandTests` 3/3 (preview availability, stale-preview rejection without mutation, fresh execute cures). `ApicultureAndTriangulationIntegrationTests` 5. `GreenhouseItemCatalogTests` 21 (Plan 91). Suite total 7003/7003 PASS. |

## Plan 91 supply-ecosystem integration check (the specific question)

**Verdict: integrates cleanly.** All 30 greenhouse entries register in the
merged global item registry (`GreenhouseItemCatalogTests.GlobalCatalog_
RegistersAllThirtyGreenhouseEntries`); the loader path is exercised by the
content-utilization CI gate (PASS); integrity is clean; every addition has an
acquisition path (4 craft recipes, 3 scavenging bindings, trade).

**Current live-consumption map (host-verified):**

| Consumed by greenhouse runtime today | Where |
|---|---|
| 12 seed IDs | `Plant` → inventory −1 seed |
| `item_blight_treatment` (fallback `iodine_pills`) | `TreatBlight` → inventory −1 |
| `clean_water` / `irradiated_water` | `Water` → inventory −⌈units/10⌉ |
| yields 12 crop IDs + `tainted_food` | `Harvest` → inventory +amount |

**Defined but not yet consumed by the greenhouse runtime** (trade/supply
content, awaiting Plan 22): `item_planter_box` (plots hardcoded 4),
`item_grow_lamp` (light hours hardcoded 6), `item_lead_glass_pane`,
`item_grow_medium`, and the 16 Plan 91 supplies (`item_greenhouse_*`).

This is the intended Plan 91 posture: definitions first, consumption second.
No overclaimed mechanics exist — `GreenhouseFile_NewSuppliesClaimNoConsumable
EffectFields` enforces it in CI.

## Blockers

None. No phase is blocked.

## Risk-ranked next actions (dependency order)

1. ~~**[Plan 22, host-only, low risk]** Close the hardcoded-host gaps~~ — **DONE (Plan 22 Phase E):** planter-box-driven plot capacity (`RefreshPlotCapacity`), grow-lamp light hours (`ComputeGrowLightHours` → `GrowLightHoursFor`), grow-medium bed sterilisation (`Clear(plotIndex, useGrowMedium)`).
2. ~~**[Plan 22, Core, additive state]** Soil fertility amendment loop~~ — **DONE (Phase A).**
3. ~~**[Plan 22, Core+host]** Drip auto-irrigation chain~~ — **DONE (Phase C).**
4. ~~**[Plan 22, Core]** Glazing-condition loop~~ — **DONE (Phase D).** Pest-protection loop (plan action 2's second half) also done (Phase B).
5. **[UI, Stitch handoff]** Plan 22 added live host APIs with no UI affordances — full gap register + paste-ready Stitch prompts in `docs/ui/GREENHOUSE_UI_GAP_SPEC.md`; flagged in AGENTS.md ("STITCH UI HANDOFF").

## Unresolved assumptions

- `CropDef` tuning living code-side is accepted as logic-not-data; if the
  team wants JSON authority for growth curves, that is a separate migration.
- `greenhouse_foundry` day owner couples greenhouse + foundry ticks in one
  owner (`Main.CampaignOwners.cs:193-200`); assumed intentional phase-2
  scheduling, not a defect.
- Apiculture `TickDaily` uses `new SeededRng(DefaultSeed + currentDay)` —
  day-derived seed is deterministic; fine under Invariant 4.
