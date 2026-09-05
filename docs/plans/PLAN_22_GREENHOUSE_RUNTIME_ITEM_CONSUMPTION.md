# PLAN 22 — Greenhouse Runtime Consumption of Fertilizer / Pest / Repair Items

> **Mission:** Make The Glass Orchard's runtime actually consume the supply
> ecosystem that Plan 91 authored — soil amendments, pest-control supplies,
> water-management kits, and structural repair materials — through additive
> Core simulation state and thin host consumption, with zero save-breaking
> changes and full determinism.
>
> **Authoritative context:** `docs/expansions/PHASE_STATUS_THE_GLASS_ORCHARD.md`
> (phase audit), `docs/greenhouse/PLAN91_CLOSEOUT.md` (item roster),
> `docs/greenhouse/GREENHOUSE_ITEM_CATALOG_AUTHORITY.md` (registry model).

---

## 0. Problem statement

Plan 91 shipped 16 greenhouse supplies that are valid, reachable, and
integrity-clean — but the greenhouse runtime consumes only seeds, blight
treatment, water items, and yields crops. The maintenance fantasy
("the drip line is failing; can we spare the parts?") has no runtime yet.
Plan 22 closes that gap without inventing a new system beyond the existing
GreenhouseSystem lifecycle.

## 1. Non-negotiable constraints

1. **Invariant 5** — all new simulation logic goes in
   `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` (+ state DTOs);
   `GreenhouseHostSession` stays a thin consumer/wirer.
2. **Invariant 1** — zero engine references in Core.
3. **Invariant 4** — no new unseeded randomness. All new rolls extend the
   existing `SeededRng(_seed * 397 + blightRollCount)` pattern with its
   persisted counter; new counters must be persisted before use.
4. **Invariant 6 / save compatibility** — all new state fields are additive
   with defaults; `CopyInto` normalizes legacy saves (missing field ⇒
   default). Old saves load unchanged; new fields round-trip.
5. **No new item definitions** — Plan 91 IDs are stable inputs
   (`item_greenhouse_*`). If a niche is uncovered, reuse; never re-invent.
6. **Pure Core math + thin host consumption** — Core exposes capability
   methods returning outcomes; the host is the only layer that touches
   inventory.
7. **One phase per task** — phases below land separately, each with its own
   tests and green gates.

## 2. Design model (what consumes what)

```text
                    GreenhouseSystem (Core)
  ┌──────────────────────────────────────────────────────────┐
  │ fertility        ← compost / ash / fish emulsion          │
  │ pestProtection   ← sticky traps / pest mesh (days)        │
  │ blight treatment ← blight treatment / insecticidal soap   │
  │ drip state       ← drip kit / line filter / catchment kit │
  │ glazingCondition ← glass pane / UV sheeting / shade cloth │
  │ growth/water/blight/contamination (existing)               │
  └──────────────────────────────────────────────────────────┘
                    ↓ outcomes only (events + state)
          GreenhouseHostSession (inventory consumption)
```

Item semantic mapping (no new taxonomy):

| Plan 91 item | Type | Runtime role |
|---|---|---|
| `item_greenhouse_compost` | Material | fertility +, mild decontamination |
| `item_greenhouse_ash_fertilizer` | Material | cheap fertility + |
| `item_greenhouse_fish_emulsion` | Material | fertility ++, growth surge |
| `item_greenhouse_insecticidal_soap` | Material | blight treatment (low tier) |
| `item_greenhouse_sticky_traps` | Material | pest protection, days (small) |
| `item_greenhouse_pest_mesh` | Material | pest protection, days (large) |
| `item_greenhouse_drip_kit` | Material | enables auto-irrigation |
| `item_greenhouse_line_filter` | Filter | maintains auto-irrigation (uses) |
| `item_greenhouse_catchment_kit` | Material | cheapens auto-irrigation |
| `item_greenhouse_glass_pane` | Material | glazing repair, large |
| `item_greenhouse_uv_sheeting` | Material | glazing repair, small |
| `item_greenhouse_shade_cloth` | Material | ash-ingress damping, days |
| `item_planter_box` *(pre-91)* | Material | plot count (closes hardcoded-4 gap) |
| `item_grow_lamp` *(pre-91)* | Device | growLightHours bonus |
| `item_grow_medium` *(pre-91)* | Material | plot sterilization on clear/replant |

## 3. New state (all additive, all defaulted)

```csharp
// GreenhousePlotState (per plot)
public float fertility;        // 0–100, default DefaultFertility (50)

// GreenhouseState (per greenhouse)
public int   pestControlDays;   // 0 = none; ticks down once/day
public bool  dripInstalled;     // auto-irrigation enabled
public int   dripFilterUses;    // remaining auto-water events
public bool  catchmentInstalled;// auto-water item cost reduced
public float glazingCondition;  // 0–100, default 100
public int   shadeClothDays;    // ash-ingress damping, ticks down once/day
```

Legacy-save normalization in `CopyInto`: any restored `fertility <= 0` ⇒ 50
(0 is unreachable by design — clamps keep `fertility >= 5` — so 0 uniquely
identifies "field absent in old save"); `glazingCondition <= 0` ⇒ 100;
ints missing ⇒ 0/false. Old saves load and tick unchanged.

## 4. Tuning constants (single source: `GreenhouseSystem` consts)

| Constant | Value | Effect |
|---|---|---:|
| `CompostFertility` | +25 | `ApplyAmendment` |
| `CompostDecontamination` | −10 | soil contamination |
| `AshFertility` | +10 | `ApplyAmendment` |
| `EmulsionFertility` | +15 | `ApplyAmendment` |
| `EmulsionGrowthSurge` | +15 | instant `growth` + |
| `FertilityGrowthDenominator` | 200 | growth ×(0.75–1.25) around 50 |
| `FertilityDecayPerDay` | −0.5 | planted plots only |
| `FertilityCostPerHarvest` | −15 | on harvest |
| `StickyTrapDays` | +3 | pest protection window |
| `PestMeshDays` | +30 | pest protection window |
| `PestProtectionChanceMultiplier` | ×0.6 | on `BaseBlightChancePerDay` |
| `SoapBlightReduction` | −0.5 | plot blight (partial cure) |
| `DripDroughtBlightMultiplier` | ×0.5 | on `DroughtBlightRatePerDay` |
| `DripFilterUses` | 60 | per cartridge |
| `CatchmentSaving` | −1 unit | per auto-water, floor 1 |
| `GlazingDecayPerDay` | −0.4 | + ash-rate coupling below |
| `GlazingAshCoupling` | ×0.5 | decay += ashRate × 0.5 |
| `GlazingMinLightFactor` | 0.6 | light ×(0.6..1.0 by condition) |
| `PaneRepair` | +40 | glazing repair |
| `SheetingRepair` | +25 | glazing repair |
| `ShadeClothDays` | +20 | ash ingress ×0.5 window |

All multipliers are deterministic; the blight roll path is unchanged except
for the protection multiplier and drip drought factor feeding the *existing*
seeded roll.

## 5. Phases (each = one task, one commit)

### Phase A — Soil fertility loop (Core + host)
- `GreenhouseSystem.ApplyAmendment(plotIndex, amendmentId, out consumedId)`:
  validates item role, clamps fertility 5–100, applies decontamination /
  growth surge; returns consumed ID.
- `TickPlot`: fertility decay (planted only); growth multiplier
  `1 + (fertility − 50)/200` folded into the existing `growth +=` line.
- `Harvest`: `fertility −= FertilityCostPerHarvest`.
- Host `AmendSoil(plotIndex, itemId)`: inventory check → consume → call.
- Panel: fertility row on plot detail.
- Tests: amendment math, clamp, decay, harvest cost, legacy-save
  normalization, host consumption, roundtrip.

### Phase B — Pest protection + soap treatment (Core + host)
- `GreenhouseState.pestControlDays`; `TickDay` decrements once (not per plot)
  and multiplies outbreak chance while > 0.
- `ApplyPestProtection(itemId, out consumedId)` (traps/mesh add days).
- `TreatBlightWithSoap(plotIndex, out consumedId)`: partial cure
  (−0.5 blight); distinct from full `item_blight_treatment` cure — host
  fallback order becomes: blight treatment → soap → iodine pills.
- Tests: window decrement (exactly 1/day), chance multiplier effect on roll
  inputs (no roll in test — assert computed `chance` inputs), soap partial
  cure, consumption, roundtrip.

### Phase C — Auto-irrigation chain (Core + host)
- Core: `DripDroughtBlightMultiplier` applied in `TickPlot` when
  `dripInstalled`; expose `AutoIrrigationRequest` outcome
  (plots below 25 water → requested units) so the *host* spends inventory.
- Host: in `TickDay` wrapper — if `dripInstalled && dripFilterUses > 0`:
  for each planted plot under 25 water, consume
  `max(1, ⌈units/10⌉ − (catchmentInstalled ? 1 : 0))` clean water and
  `Water(...)`, decrement filter uses; when `dripFilterUses == 0`, drip is
  inert until a cartridge is applied.
- `ApplyDripKit/ApplyFilter/ApplyCatchment(itemId, out consumedId)`.
- Tests: enable→maintain→degrade chain, catchment saving, floor-1 cost,
  no water in inventory ⇒ no auto-water (no soft-lock), roundtrip.

### Phase D — Glazing condition + repairs (Core + host)
- `glazingCondition` decay in `TickDay`: `−0.4 − ashRate × 0.5` (shade cloth
  days halve the ash component). Light factor multiplier
  `lerp(0.6, 1.0, condition/100)` folds into `lightFactor`.
- `RepairGlazing(itemId, out consumedId)` (pane +40 / sheeting +25, clamp
  100); `ApplyShadeCloth(itemId, out consumedId)` (+20 days).
- New event `OnGlazingDegraded` (fires crossing 30) — narrative hook only.
- Panel: glazing status row + repair action.
- Tests: decay vs ash rate, shade damping, repair clamps, light-factor
  effect on growth timing, roundtrip.

### Phase E — Close pre-91 host gaps (host-only)
- Plot count from inventory: `EnsurePlots(max(4, inventory
  .CountById("item_planter_box")))` on setup + when inventory changes;
  `EnsurePlots` already refuses to remove occupied plots (safe).
- `growLightHours = 6 + 2 × min(2, lampCount)` in the day-owner tick call
  (host owns the call; Core signature unchanged).
- `Clear`/replant with `item_grow_medium`: consume 1 ⇒
  `soilContamination = 0` (sterile bed) — host-side `Clear(plotIndex,
  useGrowMedium: false)` overload.
- Tests: host-level plot scaling, light computation, medium consumption.

## 6. Test & gate matrix (per phase)

- xUnit: behavior math, clamp/normalization, host consumption, save
  roundtrip of every new field, determinism (same seed ⇒ same plot states).
- `--greenhouse-selftest`: extend `GreenhouseHeadlessDemo` with one scenario
  per phase (amendment growth delta, protection window, drip chain,
  glazing decay/repair) — gates must fail before, pass after.
- `--data-integrity-selftest`: must stay 0 errors (no new IDs).
- Full `dotnet test` + `dotnet build Ashfall.csproj` green per phase.
- Save compat: extend the existing envelope tests with a legacy-shape save
  (no new fields) asserting load + first-tick normalization.

## 7. Risks & mitigations

| Risk | Mitigation |
|---|---|
| Save incompatibility from new fields | additive + `CopyInto` normalization + roundtrip tests per phase |
| Determinism drift | all new effects are deterministic multipliers; any future roll reuses the persisted-counter reseed pattern |
| Host/ Core drift on item IDs | host reads `GreenhouseExpansionCatalog.Items` — extend the constants class with the 12 supply IDs in Phase A (single authority, no string literals) |
| Inventory soft-locks (drip without water) | no-water ⇒ no auto-water, explicit LastEvent; drip state persists |
| Scope creep into new systems | no weather/pest-fauna simulation — protection windows are counters, not agents |
| UI overload on plot detail | one status row + one action per loop |

## 8. Explicit non-goals

- No pest-fauna simulation, no weather coupling beyond the existing
  `ashContaminationRate` parameter, no irrigation network graph, no per-plot
  pipes, no real-time decay — the greenhouse tick remains day-granular.
- No changes to item definitions (Plan 91 roster is frozen authority).
- No new crafting recipes in Plan 22 (Plan 55 owns the chain).

## 9. Next prompt to run

> "Execute Plan 22 Phase A (soil fertility loop) per
> `docs/plans/PLAN_22_GREENHOUSE_RUNTIME_ITEM_CONSUMPTION.md` using
> ashfall-implement: Core `ApplyAmendment` + fertility in TickPlot/Harvest,
> host `AmendSoil`, GreenhouseExpansionCatalog supply constants, tests and
> greenhouse-selftest gates. One phase only."
