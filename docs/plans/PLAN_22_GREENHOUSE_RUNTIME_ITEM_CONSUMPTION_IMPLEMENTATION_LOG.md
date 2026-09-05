# PLAN 22 IMPLEMENTATION LOG — Greenhouse Runtime Item Consumption

Plan: `docs/plans/PLAN_22_GREENHOUSE_RUNTIME_ITEM_CONSUMPTION.md`
## Phase A — Soil fertility loop

**Status: PASS**

### Changed

- `Assets/Ashfall.Core/Greenhouse/GreenhouseExpansionCatalog.cs` — added the
  12 Plan 91 supply-ID constants (`Compost` … `ShadeCloth`) to
  `GreenhouseExpansionCatalog.Items` (single authority, no string literals in
  runtime code; per plan §7 risk mitigation).
- `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` —
  - `GreenhousePlotState.fertility` (additive field, default 50).
  - 11 tuning constants (`DefaultFertility` … `FertilityCostPerHarvest`).
  - `NewPlot` seeds `fertility = DefaultFertility`.
  - `CopyInto` copies fertility and normalizes legacy saves
    (`fertility <= 0` ⇒ default; 0 is unreachable by clamps, so 0 uniquely
    identifies "field absent").
  - `ApplyAmendment(plotIndex, amendmentItemId, out consumedAmendmentId)` —
    compost +25 fertility / −10 contamination; ash +10; emulsion +15 and a
    +15 growth surge on Sprouting/Growing crops (with stage-transition and
    `OnCropMatured` consistency); rejects non-amendments and invalid plots;
    clamps to [5, 100].
  - `TickPlot` — fertility decay −0.5/day for all planted beds (including
    mature); growth multiplier `1 + (fertility − 50)/200` folded into the
    existing growth line.
  - `Harvest` — `fertility −= 15` (floor 5); fertility survives `ResetPlot`
    and `Clear` (bed quality persists).
- `src/Host/GreenhouseHostSession.cs` — `AmendSoil(plotIndex, amendmentItemId)`
  following the proven `Plant` pattern: inventory check → Core mutation →
  `InventoryHost.Remove(consumed, 1)` → `RaiseStateChanged`.
- `src/UI/GreenhousePanel.cs` — "Fertility x / 100" row in plot detail with
  critical/amber/dim color bands.
- `Assets/Ashfall.Core/Greenhouse/GreenhouseHeadlessDemo.cs` — Phase A
  scenario: defaults, invalid rejections, compost/ash/emulsion curves,
  contamination lift, surge, identical-conditions growth scaling, daily decay,
  harvest drain, legacy normalization (13 new gates).

### Tests

- New `Ashfall.Core.Tests/GreenhouseFertilityTests.cs` (17 tests): NORMAL
  (3 amendment curves, surge stage-advance, fallow banking), BOUNDARY
  (clamps 5/100, growth-factor bounds), INVALID (non-amendment items,
  invalid plot indices), REPEAT (stacking then clamp), decay (planted incl.
  mature; none fallow), harvest drain with floor, persistence through
  reset/clear, SAVE roundtrip (incl. snapshot anti-aliasing), OLD SAVE
  normalization, DETERMINISM, INTEGRATION (amendment IDs resolve globally).

### Verification results (exact)

| Command | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS 0/0 |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **7033/7033 PASS** |
| `dotnet build Ashfall.csproj` | PASS 0 errors / 0 warnings |
| `godot --headless --path . -- --greenhouse-selftest` | **PASS 37/37** (was 24/24) |
| `godot --headless --path . -- --data-integrity-selftest` | PASS, 0 errors |
| `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS |

### Divergences

- **Panel action button deferred.** An explicit AMEND button needs an
  item-selection affordance; `AmendSoil` is callable and the fertility row
  gives players state visibility. Lands with a later phase's action surface.
- The plan's risk note said "the 12 supply IDs" — the four tools are *not*
  greenhouse-runtime consumables, so the constants added are the 12 non-tool
  supplies. Tests pin that tools are rejected by `ApplyAmendment`.

### Remaining

- Phases B/C/D/E — per plan.

## Phase B — Pest protection & soap treatment

**Status: PASS**

### Changed

- `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` —
  - `GreenhouseState.pestControlDays` (additive int, default 0;
    `CopyInto` clamps negatives to 0 for legacy/junk values).
  - Constants: `StickyTrapDays` 3, `PestMeshDays` 30,
    `PestProtectionChanceMultiplier` 0.6, `SoapBlightReduction` 0.5,
    `DroughtBlightFactor` 2.5 (literal promoted to const during extraction).
  - `TickDay` — captures the protection flag, decrements the window **once
    per ticked day** (not per plot), passes it into every plot tick.
  - `ComputeDailyBlightChance(...)` — pure static function extracted from
    `TickPlot`'s inline expression (identical math). TickPlot now calls it;
    tests pin the multiplier inputs without rolling.
  - `ApplyPestProtection(itemId, out consumedId)` — traps +3d, mesh +30d,
    days stack; rejects non-protection items.
  - `TreatBlightWithSoap(plotIndex, out consumedId)` — partial cure −0.5
    blight (floors at 0); rejected on clean/failed plots; distinct from the
    full `TreatBlight` cure.
- `src/Host/GreenhouseHostSession.cs` —
  - `TreatBlight` / `ExecuteTreatBlight` fallback order is now
    **blight treatment → insecticidal soap → iodine pills**; soap uses the
    existing `greenhouse.blight_partial` result key.
  - `PreviewTreatBlight` treats soap as treatment availability.
  - New `ApplyPestProtection(itemId)` (Plant consumption pattern).
- `src/UI/GreenhousePanel.cs` — "Pest Control" status card: `Nd` while a
  window is open, `—` (caution) when expired.
- `Assets/Ashfall.Core/Greenhouse/GreenhouseHeadlessDemo.cs` — Phase B
  scenario: rejection, window open/stack, once-per-day decrement across 3
  plots, pure-chance multiplier + drought behavior, soap partial cure
  (incl. floor + non-full-cure), clean/failed rejections, save round-trip
  (13 new gates).

### Tests

- New `Ashfall.Core.Tests/GreenhousePestProtectionTests.cs` (18 tests):
  NORMAL, window decay (exactly once/day across plots; ticks with zero
  plots; never negative), pure chance function (protection scaling,
  zero-contamination invariant, drought factor, clamp), INVALID, SAVE
  roundtrip + legacy normalization, DETERMINISM (roll stream identical),
  INTEGRATION (pest IDs resolve globally).

### Verification results (exact)

| Command | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS 0/0 |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **7068/7068 PASS** |
| `dotnet build Ashfall.csproj` | PASS 0 errors / 0 warnings |
| `godot --headless --path . -- --greenhouse-selftest` | **PASS 50/50** (was 37/37) |
| `godot --headless --path . -- --data-integrity-selftest` | PASS, 0 errors |
| `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS |
| `godot --headless --path . -- --bridge-selftest` | PASS |

### Divergences

- Chance-input testing done via the extracted pure function
  `ComputeDailyBlightChance` (plan suggested exactly this shape).
- Panel "Pest Control" status card added (not explicitly listed in Phase B)
  — new player-facing state must be discoverable.

### Remaining

- Phases C/D/E — per plan.

## Phase C — Drip auto-irrigation chain

**Status: PASS**

### Changed

- `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` —
  - `GreenhouseState`: `dripInstalled` (bool), `dripFilterUses` (int,
    negative-clamped on restore), `catchmentInstalled` (bool) — all additive.
  - Constants: `AutoIrrigationThreshold` 25, `AutoIrrigationDose` 25,
    `DripFilterUsesPerCartridge` 60, `DripDroughtBlightMultiplier` 0.5,
    `CatchmentCostSaving` 1.
  - `AutoIrrigationRequest` struct (PlotIndex, WaterUnits, CleanWaterCost).
  - `ComputeAutoIrrigationRequests()` — pure read; empty unless the kit is
    installed **and** the filter has uses; only Sprouting/Growing plots below
    the threshold; cost = max(1, ⌈25/10⌉ − catchment saving).
  - `ExecuteAutoIrrigation(plotIndex, waterUnits)` — commit API: re-validates,
    decrements one filter use, waters (untainted).
  - `ApplyDripKit` (single install), `ApplyDripFilter` (requires kit, uses
    stack), `ApplyCatchmentKit` (requires kit, single install).
  - `TickPlot` — drought blight rate ×0.5 while `dripInstalled`.
- `src/Host/GreenhouseHostSession.cs` —
  - `TickDay` wrapper now runs `AutoIrrigate()` **before** the growth tick:
    computes requests, spends `clean_water` per request (skips with a
    "Drip line dry" event when short — never a soft-lock), commits via
    `ExecuteAutoIrrigation`, refunds water on a stale request (defensive;
    unreachable single-threaded).
  - `ApplyDripChainItem(itemId)` — single host entry point for the three
    supplies; enforces kit-first ordering with grounded rejection events.
- `src/UI/GreenhousePanel.cs` — "Drip Line" status card: `—` (not installed,
  caution), filter-uses remaining (normal), `DRY` (spent, warn).
- `Assets/Ashfall.Core/Greenhouse/GreenhouseHeadlessDemo.cs` — Phase C
  scenario (18 new gates): ordering rejections, single install,
  inert-until-filter, request shape, catchment saving, execute + filter
  decrement, spend-to-dry degrade, drought-blight halving (deterministic
  head-to-head), save round-trip.

### Tests

- New `Ashfall.Core.Tests/GreenhouseDripIrrigationTests.cs` (18 tests):
  NORMAL (enable/maintain/cheapen), request filtering, catchment cost
  saving with floor, EXECUTE (water + filter decrement; rejections),
  deterministic drought-blight halving head-to-head, SAVE roundtrip +
  legacy normalization, DETERMINISM (drip state does not disturb the roll
  stream), INTEGRATION (drip IDs resolve globally).

### Verification results (exact)

| Command | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS 0/0 |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 7091/7094 — 3 failures, **all concurrent-agent churn** (their `probe_integrity_tmp.json`, RebelBranch mid-refactor, journal doc `file:///` links); greenhouse/drip suites fully green |
| `dotnet build Ashfall.csproj` | PASS 0 errors / 0 warnings |
| `godot --headless --path . -- --greenhouse-selftest` | **PASS 68/68** (was 50/50) |
| `godot --headless --path . -- --data-integrity-selftest` | PASS, 0 errors |

### Divergences

- Demo lesson (fixed in-phase): `ComputeAutoIrrigationRequests` is empty
  until a filter is loaded — a dedicated "inert until filter" gate pins it.
- Host entry point is one `ApplyDripChainItem(itemId)` rather than three
  methods; Core keeps the three separate `Apply*` APIs.

### Remaining

- Phases D/E — per plan.

## Phase D — Glazing condition & repairs

**Status: PASS**

### Changed

- `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` —
  - `GreenhouseState`: `glazingCondition` (float 0–100, **field-initialized
    to 100** — legacy saves missing the field deserialize to full glazing,
    same convention as `saveId`), `shadeClothDays` (int, default 0).
  - Constants: `GlazingDecayPerDay` 0.4, `GlazingAshCoupling` 0.5,
    `GlazingMinLightFactor` 0.6, `GlazingDegradedThreshold` 30,
    `PaneRepair` 40, `SheetingRepair` 25, `ShadeClothDays` 20.
  - `TickDay` — glazing weathers every ticked day (base + ash-rate coupling;
    shade cloth halves the ash component); shade window decrements once per
    day; `OnGlazingDegraded` fires once on the downward crossing of 30 (and
    again after a repair re-crosses). Greenhouse-wide: applies with zero
    plots.
  - `TickPlot` — growth folds in `GlazingLightFactor()` = lerp(0.6, 1.0,
    condition/100).
  - `RepairGlazing(itemId, out consumedId)` — pane +40 / sheeting +25,
    clamped at 100; rejected at full condition and for non-repair supplies.
  - `ApplyShadeCloth(itemId, out consumedId)` — +20 days, stacks.
  - `GlazingLightFactor()` — public read for UI/tests.
- `src/Host/GreenhouseHostSession.cs` — `RepairGlazingAuto()` (pane
  preferred, UV-sheeting fallback, grounded rejection events) and
  `ApplyShadeClothSupply()`.
- `src/UI/GreenhousePanel.cs` — "Glazing" status card (critical < 30, warn
  < 70) + **REPAIR** action button.
- `src/Main.World.cs` — `case "repair":` routes to `RepairGlazingAuto()`.
- `Assets/Ashfall.Core/Greenhouse/GreenhouseHeadlessDemo.cs` — Phase D
  scenario (16 new gates): intact start, base vs ash decay, shade damping,
  crossing-once event (incl. repair re-cross), repair clamps + rejections,
  dimmed growth head-to-head, light-factor floor, save round-trip.

### Tests

- New `Ashfall.Core.Tests/GreenhouseGlazingTests.cs` (16 tests): NORMAL
  (base decay, ash acceleration, shade damping + window tick, no-plot
  weathering), light-factor lerp bounds + dimmed-growth head-to-head,
  degraded event (fire-once, re-cross after repair), repairs (pane >
  sheeting, clamp, intact rejection, non-supply rejection), SAVE roundtrip
  + legacy normalization via field initializer, DETERMINISM (replayed
  scenario identical; unshaded twin weathers faster), INTEGRATION
  (glazing supply IDs resolve globally).
- Updated `DirtyFlushNoOpRegressionTests.Greenhouse_FallowPlots_TickDay_...`:
  the no-op guarantee is narrowed to **plot-level state** (the dirty-flush
  concern); greenhouse-wide glazing weathering intentionally ticks and is
  now pinned to the exact expected decay.
- Updated `GreenhouseFertilityTests.FertilityNeverDrivesGrowthAbovePlanBounds`:
  holds fertility + glazing at their bounds through each tick so the test
  isolates the fertility factor (expected values now include the controlled
  glazing factor from the in-tick decay).

### Verification results (exact)

| Command | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS 0/0 |
| `dotnet test … --filter FullyQualifiedName~Greenhouse` (6 greenhouse suites) | **122/122 PASS** |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **7110/7110 PASS** |
| `dotnet build Ashfall.csproj` | PASS 0 errors / 0 warnings |
| `godot --headless --path . -- --greenhouse-selftest` | **PASS 84/84** (was 68/68) |
| `godot --headless --path . -- --data-integrity-selftest` | PASS, 0 errors |
| `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS |
| `godot --headless --path . -- --bridge-selftest` | PASS (stable CI verb) |

### Divergences

- Legacy normalization uses the DTO **field initializer**
  (`glazingCondition = MaxGlazingCondition`) rather than a sentinel in
  `CopyInto` — 0 is a legitimate condition value (ruined glazing). Pinned by
  `LegacySave_DeserializesFullGlazing_ViaFieldInitializer`.
- `DirtyFlushNoOp` test renamed (`..._DoesNotMutatePlotState`) and narrowed
  to plot state — glazing weathering is greenhouse-wide by design; the
  host's dirty-flush concern is unchanged for fallow plots.
- REPAIR button auto-selects pane → sheeting (deterministic); a per-item
  picker is deferred like the Phase A AMEND action. Shade-cloth deployment
  has a host API (`ApplyShadeClothSupply`) but no button yet — same
  deferred-UI rationale.

### Remaining

- Phase E (host-only gaps: planter-box plots, grow-lamp light, grow-medium
  reset) — per plan.


---

## Phase E — Host equipment scaling (host-only)

**Status: PASS**

### Changed

- `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` — pure Core math for
  the host's equipment scaling: `GrowLightHoursFor(lampCount)` (6 h base,
  +2 h/lamp, first two lamps count, clamped) + constants
  (`BaseGrowLightHours`, `GrowLampBonusHours`, `MaxCountedGrowLamps`,
  `BasePlanterBoxPlots`). No TickDay signature change.
- `src/Host/GreenhouseHostSession.cs` —
  - `RefreshPlotCapacity()` — plot count follows planter-box stock
    (`max(4, inventory count)`); run in the constructor, `Create`, and each
    `TickDay` (scavenged beds join on the next day). `EnsurePlots` refuses
    to remove occupied plots, so stock collapses never destroy crops.
  - `ComputeGrowLightHours()` — lamp stock → today's light hours.
  - `Clear(plotIndex, useGrowMedium = false)` overload — grow-medium brick
    sterilises the bed (Clear + zero residual contamination; direct state
    mutation follows the established iodine-fallback precedent). Default
    `false` keeps the existing CLEAR route consumption-free.
- `src/Main.CampaignOwners.cs` — day-owner tick passes
  `ComputeGrowLightHours()` instead of hardcoded 6 h.
- `Assets/Ashfall.Core/Greenhouse/GreenhouseHeadlessDemo.cs` — Phase E gates:
  light-hours scale/cap/clamp (5 checks).

### Tests

- New `Ashfall.Core.Tests/GreenhouseEquipmentScalingTests.cs` (7 tests):
  light-hours theory table (0/1/2/3/9/−2 lamps), linear-bonus-unto-cap,
  capacity growth, occupied-plots-never-removed on stock collapse,
  grow-medium sterilisation contract (residual scrubbed → clean harvest).
  Host-only wiring (refresh cadence, CampaignOwners call) is build-verified —
  the Godot host assembly is not xUnit-referenceable.

### Verification results (exact)

| Command | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS 0/0 |
| `dotnet test … --filter FullyQualifiedName~Greenhouse` (7 greenhouse suites) | **133/133 PASS** |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **7121/7121 PASS** |
| `dotnet build Ashfall.csproj` | PASS 0 errors / 0 warnings |
| `godot --headless --path . -- --greenhouse-selftest` | **PASS 89/89** (was 84/84) |
| `godot --headless --path . -- --data-integrity-selftest` | PASS, 0 errors |
| `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS |
| `godot --headless --path . -- --bridge-selftest` | PASS |

### Divergences

- Light-hours math lives in Core (`GrowLightHoursFor`) rather than the host
  plan-literal — the host still owns the call; the math being pure Core
  makes it xUnit-testable. No Core TickDay signature changed.
- STERILIZE button deferred to the Stitch UI pass (same rationale as
  AMEND/SHADE); the overload + contract are live and tested.

### Remaining

- None — Plan 22 phases A–E complete. Deferred UI affordances are catalogued
  in `docs/ui/GREENHOUSE_UI_GAP_SPEC.md` (Stitch handoff; flagged in
  AGENTS.md).

---

# PLAN 22 COMPLETE — phases A–E all PASS

The greenhouse runtime now consumes the full Plan 91 supply ecosystem:
amendments (fertility), pest supplies (protection window + soap), the drip
chain (enable/maintain/cheapen), structural repairs (glazing + shade), and
equipment scaling (boxes/lamps/medium) — 89 headless gates, 7121 suite tests,
zero item-JSON or gameplay-authority changes beyond the planned Core loops.
