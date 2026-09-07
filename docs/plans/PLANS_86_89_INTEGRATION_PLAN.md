# Plans B86–B89 — Evidence-Grounded Integration Plan

**Scope:** B86 Expedition Combat Breaching · B87 Closed-Loop Aquaponics · B88 HF/DF Direction Finding · B89 Precision Metrology
**Status:** Reconnaissance complete — awaiting approval before production edits
**Date:** 2026-09-06
**Method:** Five parallel read-only audits against current source (combat/expedition, aquaponics deps, radio/DF, metrology/crafting, shared infra)
**Naming caution:** Existing `docs/crafting/PLAN_87_*`, `docs/relationships/PLAN_88_*`, `docs/narrative/PLAN_89_*` are **unrelated** content closeouts. Use **B86–B89** / `PLANS_86_89_*` only.

After approval, first filesystem writes will be:
1. `docs/PLANS_86_89_AUTHORITY_MAP.md` (mandatory §3 exit gate)
2. `docs/plans/PLANS_86_89_INTEGRATION_PLAN.md` (this plan, expanded)
3. Then Phase 0 baseline verification — **no Core/UI code until those docs land**

---

# 1. Objective

Ship one interconnected engineering-and-expedition tranche that moves ASHFALL from improvised survival into disciplined engineering, with:

- Core owning simulation truth
- JSON catalogs owning authored definitions
- Host sessions translating commands/events
- Godot rendering only
- Deterministic replay, mid-op save/load, content utilization, and fast-tier CI

---

# 2. Current Reality (evidence)

## 2.1 Plan B86 — Breaching

| Finding | Evidence |
|---|---|
| Brief file `TacticalCombatSystem.Obstacles.cs` **does not exist** | Combat partials: Actions, Damage, Persistence, Targeting only |
| Tactical “obstacles” are an **unused** `BarrierState` seam | `CombatTypes.cs` BarrierState + `CombatState.Barriers`; save round-trips; `PlayerFire` nulls `BarrierMaterial` after lookup |
| Live route breaching already exists | `MineClearingFlailEngine` + `RouteInfrastructureSystem` mine clearance |
| Rail obstacles already exist | `RailwaySystem.ClearTrackObstacle` |
| Expedition loadout is bike/flashlight/vehicle only | `ExpeditionSystem` / garage profile |
| Winch is tag-only | `winch_kit`, `vmod_heavy_winch` — no tow behavior |
| Combat noise unused | `StanceMods.Noise` authored; `StealthSystem` owns expedition noise |
| VaultDoorBreachingPanel is UI-05 stub | No Core owner — do not promote as B86 UI |

## 2.2 Plan B87 — Aquaponics

| Finding | Evidence |
|---|---|
| **No** aquaponics/fish/biofilter Core system | Search negative across Core/src |
| Closest ecology templates | `AeroponicsSystem`, `HydroponicBiomeSystem`, `GreenhouseSystem` |
| Name-only residue | `GreenhouseExpansionCatalog.Locations.HydroBaronsAquaponics` |
| Water/power/thermal authorities exist | `WaterTreatmentSystem`, `PowerGridSystem`, `ShelterThermalSystem` |
| Kitchen is meal prep, not production | `KitchenNutritionSystem` |
| Hydroponic biome day tick is orphaned | `TickAdvancedShelterSystems` never called from campaign advance |

## 2.3 Plan B88 — HF/DF

| Finding | Evidence |
|---|---|
| Brief `RadioTriangulationEngine` **does not exist** | Real owner: `SignalTriangulationSystem` |
| Continuous DF already implements bearing + uncertainty + multi-obs confidence | `RadioObservation.errorDegrees`, `Triangulate`, uncertainty radius, weather/skill fields |
| Exact-fix HF intercepts already closed (B67) | `ShelterRadioStationSystem.RecordBearing` → authored `revealed_location_id` |
| Continuous DF **not persisted** in host radio save | Core Capture/Restore exists; `RadioHostSession.CaptureSave` omits triangulation |
| Continuous DF map handoff missing | Journals only; `DiscoverRumor` API exists unused on this path |
| Ray origins hardcoded | `(0,0)` / `(10,0)` in intersection math |
| Acoustic DF catalog is a different domain | Early-warning vibration — not HF radio |

## 2.4 Plan B89 — Metrology

| Finding | Evidence |
|---|---|
| Workshop already has float `Calibration` + recipe gates | `ShelterWorkshopSystem`, `workshop_recipes.json` `calibration_requirement` |
| Ballistics takes `toolingCalibration` but host hardcodes `0.75f` | `BallisticsWorkbenchSystem.Calibrate` / Plans74–77 host |
| No free global breakdown reduction today | Per-machine / per-vehicle / per-weapon only |
| Seismic/ORC/excavation do not disturb workshop Calibration | Disturbance producers exist without metrology consumers |
| Typed capability patterns exist | `CombatDoctrineCapability`, `ContainmentCapability`, `AdvancedMachineOperatorContext` |
| Precision optics / dosimeter calibration are domain-local | Do not become bunker-wide magic |

## 2.5 Shared infra (cookbook ready)

Campaign day: `CampaignDayCoordinator` + `Main.CampaignOwners`
RNG: `ISeededRng` + `CampaignStreamIds`
Inventory: `BeginTransaction` / `TryExecuteTransaction`
Save: `SaveSectionRegistry` + `SaveStoreHub`
Catalogs: `CatalogIntegrityValidator` + content utilization scanner
CLI: `HostCliRegistry`
UI: PanelRegistry + code-built panels (keep Prototype until real commands)
Templates: `Main.Plans78_81.cs`, `Main.Plans190_193.cs`

---

# 3. Required Delta

| Plan | Missing capability |
|---|---|
| **B86** | Live combat barrier lifecycle + method-traded clearance (quiet cut / mechanical / abstract consumable / vehicle assist / abandon) with noise, wear, exposure, and mid-clear save |
| **B87** | Deterministic closed-loop fish↔biofilter↔plant nutrient ecology with power/heat/water coupling and inventory harvest |
| **B88** | Make existing continuous DF **production-complete**: persist, map rumor handoff, real baselines, weather, distress bridge; optional array catalog |
| **B89** | Typed `PrecisionCalibrationGrade` + standards catalog; registered consumers only; seismic drift; wire ballistics/workshop; no global multipliers |

---

# 4. Architecture decisions (locked recommendations)

## D1 — EXTEND vs CREATE

| Plan | Decision |
|---|---|
| B86 | **CREATE** `CombatBreachingEngine` that mutates **existing** `BarrierState` / combat encounter state; **KEEP** mine flail as route-only |
| B87 | **CREATE** `AquaponicsSystem` (new ecology owner); couple via ports to water/power/thermal/inventory/greenhouse |
| B88 | **EXTEND** `SignalTriangulationSystem` + host radio save/map wiring; **KEEP** `ShelterRadioStationSystem` for exact-fix |
| B89 | **CREATE** thin `PrecisionMetrologySystem` (grades/certificates/standards) that **projects into** `ShelterWorkshopSystem.Calibration` and registered consumers |

## D2 — Explosive content hardening (B86)

Store only fictional gameplay fields: clearance power/radius, noise, setup ticks, operator exposure, durability, consumable count. **No** real recipes, initiation, charge geometry, or placement calculations.

## D3 — Aquaponics model (B87)

Compact deterministic ecology (biomass, feed, temp, DO, N-load, biofilter, uptake, disease, power/heat). Not CFD/microbiology. Follow aeroponics tick + inventory transaction patterns.

## D4 — DF geometry (B88)

One station → bearing + uncertainty. Repeated obs → narrower confidence. Multi-baseline → map fix. Skywave → larger uncertainty. Fingerprints → identity confidence. Prefer `DiscoverRumor` over instant `Discover` for continuous path.

## D5 — Metrology non-magic (B89)

No `"all_machine_breakdowns_-35%"`. Consumers declare grade requirements. Unregistered systems get zero benefit. Bootstrap non-circular (hand tools / starter grade before precision machines).

## D6 — Dependency order

```text
Phase docs + baseline
 → B89 Metrology (enables quality for tools/sensors/instruments)
 → B88 DF completion (map intel for expeditions)  ║ parallelizable with B87
 → B87 Aquaponics                                   ║
 → B86 Combat Breaching (uses tool quality + optional map intel)
 → Cross-tranche 45-day scenario + CI gates
```

B87 and B88 can proceed in parallel after B89 contracts exist; B86 should land last among the four so breach tools can read calibration grades.

---

# 5. Ownership matrix (new artifacts)

| Artifact | Owner | Save |
|---|---|---|
| `breaching_equipment_catalog.json` | B86 loader → `CombatBreachingEngine` | Encounter progress in `combat` (preferred) |
| `aquaponics_system_catalog.json` | B87 → `AquaponicsSystem` | **New** `aquaponics` section |
| `direction_finding_catalog.json` | B88 array/instrument profiles feeding triangulation | Extend **`radio`** (no new DF section) |
| `metrology_standards_catalog.json` | B89 → `PrecisionMetrologySystem` | Prefer extend `shelter_workshop` + metrology DTO; new section only if certificates cannot nest cleanly |
| `ObstacleBreachingCapability` | Combat/expedition projection | N/A |
| `AquaponicNutrientSource` | Aquaponics → greenhouse consumer | N/A |
| `DirectionFindingCapability` | Projection from triangulation state | N/A |
| `PrecisionCalibrationGrade` | Metrology → registered consumers | Persisted with metrology/workshop |

---

# 6. State & API contracts (summary)

## B86 — `CombatBreachingEngine`

State machine: `Available → SettingUp → Clearing → Cleared` (also `Interrupted` / `Failed` / `Abandoned`).
Commands: validate obstacle/tool/operator/items/vehicle/reach → reserve resources → advance ticks → apply wear/exposure/noise → mutate barrier integrity / cover.
Math: bounded `effective_clearance = power × skill × condition × vehicle`; `ticks = ceil(structural / effective)`; deterministic rounding; no method universally dominates.
RNG streams: `breach.operator_incident`, `breach.obstacle_secondary_effect`.

## B87 — `AquaponicsSystem`

Owned state: fish biomass, feed stock, DO, N-load, biofilter health, disease pressure, tank temp proxy, power-starvation flags.
Does **not** own: global water tanks, greenhouse plots, power truth, room temperature truth.
Tick: shelter day (phase 2 after power). Harvest grants canonical food items via inventory transactions.
RNG: `aquaponics.disease`, `aquaponics.fry_survival`.

## B88 — extend `SignalTriangulationSystem`

Add/finish: persist observations/candidates in radio save; station baseline coordinates; skywave/polarization uncertainty modifiers from catalog; `OnLocationRevealed` → `WastelandMap.DiscoverRumor`; optional distress `MarkTriangulated`; fingerprint identity confidence separate from location.
RNG: `df.skywave_jitter`, `df.false_signature`.

## B89 — `PrecisionMetrologySystem`

Grades (example): `Uncalibrated`, `Field`, `Shop`, `Reference`, `Certified`.
APIs: calibrate instrument/machine, certify component lot, query grade for consumer id, apply disturbance (seismic).
Consumers (explicit registration): workshop precision recipes, ballistics `toolingCalibration`, aquaponics sensors/pumps, DF array calibration, optional precision optics.
RNG: `metrology.calibration_drift`, `metrology.measurement_noise`.

---

# 7. Data changes

| Catalog | Contents |
|---|---|
| `breaching_equipment_catalog.json` | Obstacle profiles (wire, hedgehog, sandbag, door, minefield-as-encounter-prop, masonry, gate, fence, wreck, trench lip, aperture, debris) **only after dedupe vs combat materials / perimeter / route mines**; tool profiles (hydraulic cutter, mechanical clear, abstract breach charge, vehicle assist) |
| `aquaponics_system_catalog.json` | Tank classes, fish species, feed, biofilter media, disease bands, power/heat curves, harvest yields |
| `direction_finding_catalog.json` | Array classes, baseline lengths, band limits, skywave seasons, fingerprint defs, calibration intervals |
| `metrology_standards_catalog.json` | Grade defs, certified part tags, consumer registration table, drift rates, bootstrap tools |
| `items.json` / `recipes.json` | Add missing authoritative IDs (e.g. wire cutters, fish feed, biofilter media, gauge blocks) — snake_case, no invented prefixes outside integrity rules |
| `power_grid.json` / rooms | Room draws for aquaponics / DF array / metrology bench if needed |

All catalogs: `schema_version`, duplicate-ID rejection, reference validation, loader tests, utilization coverage.

---

# 8. Save / determinism / ticks

| System | Tick | Save rule |
|---|---|---|
| B86 | Combat/expedition encounter actions | Mid-breach in `combat` state; do not duplicate inventory |
| B87 | Shelter day (phase 2) | Own ecology section; query power/thermal/water |
| B88 | Observation-driven + maintenance day | Nested in `radio`; station exact-fix stays `radio_station` |
| B89 | Shelter maintenance/production cadence | Workshop + metrology certificates; disturbance events idempotent |

Determinism: same state + seed + commands ⇒ same result; no `System.Random`, no wall clock, no `_Process` delta.

---

# 9. Host / UI

| Plan | Host | UI |
|---|---|---|
| B86 | CombatHostSession / expedition prep adapters | Domain breaching controls on combat/expedition surface — **not** VaultDoorBreachingPanel relabel |
| B87 | `AquaponicsHostSession` (LastEvent/StateChanged) | New panel via google-stitch proposal → AshfallUiHelpers; Prototype until real commands |
| B88 | Extend `RadioHostSession` + existing `TriangulationPanel` | Prefer completing TriangulationPanel over a parallel DF screen |
| B89 | Workshop / metrology adapter; fix ballistics host hardcoded 0.75 | Extend `WorkshopPanel` + thin metrology readout; do not invent global buff UI |

UI rule: never compute clearance, ecology, bearings, or calibration bonuses in Godot.

---

# 10. Failure modes (must specify in tests)

- Unsupported obstacle/tool tag → typed reject
- Interrupted breach → partial progress only if profile allows
- Aquaponics power loss → DO crash / fish stress, not silent continue
- Single DF obs → bearing only, never exact coords
- Unregistered machine → zero metrology benefit
- Old saves missing new sections → safe defaults
- Empty catalogs → systems idle, no crash
- Seismic during calibration → drift/fail, deterministic

---

# 11. Dependency-ordered phases

## Phase 0 — Docs + baseline (first safe step)

- Write `docs/PLANS_86_89_AUTHORITY_MAP.md`
- Write `docs/plans/PLANS_86_89_INTEGRATION_PLAN.md`
- Record baseline: `dotnet build/test`, `dotnet build Ashfall.csproj`, data-integrity, bridge-selftest
- **Gate:** authority map accepted; baseline green or known-accepted failures listed

## Phase 1 — B89 Core contracts

- `PrecisionCalibrationGrade`, standards catalog + loader, `PrecisionMetrologySystem`
- Wire workshop Calibration projection; fix ballistics host to read live tooling grade
- Seismic disturbance → calibration drift seam
- Save + unit tests + `--precision-metrology-selftest`
- **Gate:** registered consumer benefits; unregistered unchanged; round-trip

## Phase 2 — B88 DF completion (parallelizable with Phase 3)

- Persist triangulation in radio save
- Station baselines; skywave modifiers; DiscoverRumor handoff; distress bridge
- Optional `direction_finding_catalog.json` for array hardware
- Tests + `--direction-finding-selftest`
- **Gate:** same-seed obs replay; uncertainty explicit; no third DF engine

## Phase 3 — B87 Aquaponics

- Catalog + `AquaponicsSystem` + save section + day owner
- Couplings: power room, thermal modifier, water/inventory transactions, nutrient source interface
- 60–120 day soak test + `--aquaponics-selftest`
- **Gate:** ecology deterministic; harvest canonical food; mid-cycle save/load

## Phase 4 — B86 Combat breaching

- Catalog + `CombatBreachingEngine`; activate BarrierState in fire path
- Actions: setup/clear/abandon; noise → stealth/stance; tool wear via EquipmentCondition
- Expedition prep gates for tools; optional winch assist on existing tags
- Dominance balance table tests + `--combat-breaching-selftest`
- **Gate:** method tradeoffs; mid-breach save; no actionable explosive content

## Phase 5 — Cross-tranche integration + CI

- Typed capability wiring B89→B86/B87/B88
- Combined 45-day scenario; save day 40 → reload → days 41–45 hash match
- Content utilization GAMEPLAY_CONSUMED for all four catalogs
- Scene binding / scene lint / `run-gates.py --tier fast`
- **Gate:** zero unresolved P0/P1; evidence ledger recorded

---

# 12. File impact map (planned)

| Area | Action | Risk |
|---|---|---|
| `docs/PLANS_86_89_AUTHORITY_MAP.md` | CREATE | Low |
| `docs/plans/PLANS_86_89_INTEGRATION_PLAN.md` | CREATE | Low |
| `docs/combat/PLAN_86_AUTHORITY_MAP.md` | CREATE (B86 deep dive) | Low |
| `Assets/Ashfall.Core/Combat/CombatBreachingEngine.cs` (+ DTOs) | CREATE | Med |
| `Assets/Ashfall.Core/Combat/TacticalCombatSystem.*.cs` | MODIFY (barrier wire + actions) | Med–High |
| `Assets/Ashfall.Core/Shelter/AquaponicsSystem.cs` (+ catalog loader) | CREATE | Med |
| `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` + Radio save host | MODIFY | Med |
| `Assets/Ashfall.Core/Shelter/PrecisionMetrologySystem.cs` (+ grade types) | CREATE | Med |
| `Assets/Ashfall.Core/Shelter/ShelterWorkshopSystem.cs` | MODIFY (grade projection) | Med |
| `src/Main.Plans74_77.cs` (ballistics cal hardcode) | MODIFY | Low–Med |
| `src/Host/RadioHostSession.cs` | MODIFY (persist DF + map rumor) | Med |
| `src/Main.PlansB86_B89.cs` | CREATE | Med |
| `SaveSectionRegistry` / SaveStores / CampaignOwners / HostCliRegistry | MODIFY | Med |
| Four new JSON catalogs + items/recipes as needed | CREATE/MODIFY | Med |
| Tests under `Ashfall.Core.Tests/` | CREATE | Low |
| UI panels | CREATE/MODIFY after Core green; Stitch for new aquaponics/breach surfaces | Med (UI audit rules) |

**Must NOT touch:** Unity `_Game/`, inventing parallel water/power/thermal/inventory authorities, promoting VaultDoorBreachingPanel as complete, creating `RadioTriangulationEngine` duplicate, global breakdown flags.

---

# 13. Out of scope

- Full CFD/microbiology aquaponics
- Real-world explosive engineering content
- Replacing B67 exact-fix shelter radio intercepts
- Acoustic early-warning DF as HF radio
- Completing all UI-05/UI-07 stubs unrelated to this tranche
- Fixing orphaned hydroponic biome campaign tick (note only; optional follow-up)
- ADR unfinished water packaging APIs (consume current inventory/water seams)

---

# 14. Rollback

- One plan per commit series (B89 → B88/B87 → B86 → integration)
- New save sections default-empty for old campaigns
- Feature isolation via catalog presence + day-owner registration
- Revert commit restores prior behavior; no silent migration that corrupts workshop/radio/combat

---

# 15. Definition of Done (tranche)

- [ ] Authority map + integration plan committed
- [ ] All four catalogs GAMEPLAY_CONSUMED
- [ ] Deterministic replay for each domain + combined 45-day scenario
- [ ] Mid-operation save/load for breach, ecology, DF obs, calibration
- [ ] Cross-system typed capabilities verified (registered vs unregistered)
- [ ] Balance: B86 method dominance table; B87 60–120d viability; B88 confidence characterization; B89 consumer matrix
- [ ] CI: build, test, data-integrity, content-utilization, scene-binding, scene-lint, fast gates, four new selftests
- [ ] Diff-only second-tool review
- [ ] Zero unresolved P0/P1

---

# 16. Implementation handoff

## MUST PRESERVE
- `RouteInfrastructureSystem` / mine flail as route mine authority
- `ShelterRadioStationSystem` exact-fix intercept grid
- `SignalTriangulationSystem` as the continuous DF geometry owner
- `PowerGridSystem`, `ShelterThermalSystem`, `WaterTreatmentSystem`, `Inventory` single authorities
- Workshop job gating semantics (extend, don’t bypass)
- Engine-agnostic Core; Godot presentation-only

## MUST ADD
- Authority map + integration plan docs first
- B89 grades + registered consumers + seismic drift
- B88 persistence + DiscoverRumor + baselines + weather
- B87 aquaponics ecology system + save + day tick
- B86 breaching engine activating BarrierState + method tradeoffs
- Catalogs, tests, selftests, content utilization

## MUST NOT DO
- Create `RadioTriangulationEngine` or third DF system
- Create parallel obstacle DB beside `BarrierState` / route mines
- Global magical reliability multipliers
- Actionable explosive construction content
- UI-owned simulation math
- Promote Prototype panels without state→blocker→cost→consequence

## VERIFY WITH
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
# then feature selftests + content-utilization + scene-binding + scene-lint + run-gates --tier fast
```

## FIRST SAFE IMPLEMENTATION STEP
1. Exit plan mode after approval
2. Write `docs/PLANS_86_89_AUTHORITY_MAP.md` and `docs/plans/PLANS_86_89_INTEGRATION_PLAN.md` from this plan
3. Run baseline verification checklist and record PASS/FAIL
4. Begin Phase 1 (B89 Core contracts) only after docs + baseline are green

---

# 17. Open decisions for approver

These are recommended defaults already baked into the plan; override only if you disagree:

1. **B88 = extend `SignalTriangulationSystem`** (not a new Core DF engine)
2. **B86 = activate `BarrierState` + new breaching engine** (route mines stay with flail)
3. **B87 = new `AquaponicsSystem`** (not a merge into aeroponics/hydro)
4. **B89 first**, then B88∥B87, then B86
5. **New UI panels stay Prototype** until Core+host commands exist; prefer completing `TriangulationPanel` / extending `WorkshopPanel` over parallel screens

**Approve this plan to authorize Phase 0 doc writes and subsequent implementation.**
