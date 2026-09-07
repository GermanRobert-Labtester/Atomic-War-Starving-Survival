# B5–B8 Baseline Reconciliation — Plans 64–67

> Phase-0 checkpoint artifact. Evidence gathered 2026-09-07 against commit
> `4e53ffc7` **plus a large uncommitted working tree** (Plans 198–205 waves,
> CombatBreachingEngine, Glassworks, NuclearCore publish, PerimeterDefense save
> store, and others are staged/modified but uncommitted).
>
> **No implementation has begun.** This document classifies every task
> assumption in the B5–B8 flagship brief against live repository truth.

## Baseline verification record

| Check | Result |
|---|---|
| Commit implemented against | `4e53ffc7afbc077233b96651159496664f579d91` (2026-09-07) + uncommitted working tree |
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 0 warnings, 0 errors |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **9169 / 9170 PASS**, 1 pre-existing FAIL |
| `dotnet build Ashfall.csproj` | PASS — 0 warnings, 0 errors |
| Pre-existing failure | `DocLinkValidationGateTests.AuthorityDocs_RelativeLinksResolveToExistingFiles` — 12 broken links in `docs/INDEX.md` → missing `Next-steps-plans/Plan_26…Plan_37*.md` files (unrelated to B5–B8; owned by the docs-atlas stream) |

---

## 1. B5 — Plan 64 (Greenhouse) reconciliation

| Task assumption | Live repository truth | Keep | Modify | Delete/obsolete | Owning authority | Save section | Tests that already pin behavior |
|---|---|---|---|---|---|---|---|
| `TreatBlight(int, out string consumedTreatmentId)` needs building | Already live in `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs:270`, with `PreviewTreatBlight`/`ExecuteTreatBlight` command-preview plumbing (lines 222–267) | Whole treatment path | Audit only: confirm iodine-pill alternate path and single-consumption guarantees | Nothing | `GreenhouseSystem` | `greenhouse` (via `GreenhouseState.CaptureState/RestoreState`) | `GreenhouseCommandTests`, `GreenhouseSystemTests` |
| Treatment-item consumption must be built from scratch | Host already checks inventory: `src/Host/GreenhouseHostSession.cs` (`GreenhouseExpansionCatalog.Items.BlightTreatment` or `iodine_pills`) | Host check | Extend only if treatment needs labor/busy time (currently instantaneous — audit decision required in Phase 4) | Nothing | `GreenhouseHostSession` | same | `GreenhouseCommandTests` |
| Greenhouse may hold a private water counter | Plot `water` field (`GreenhousePlotState.water`, max 100) is **intentional tray-moisture state**, not a spendable pool. The source transaction is already authoritative: `GreenhouseHostSession.Water()` checks/consumes `clean_water` / `irradiated_water` from inventory (10 units per item) **before** calling `System.Water(plotIndex, units, tainted)` | Tray-moisture model + authoritative host transaction | Only close any preview/atomicity gap (failure mid-commit) if tests show one | "Replace private water pool" premise is obsolete | `GreenhouseHostSession` (source) + `GreenhouseSystem` (tray state) | `greenhouse` | `GreenhouseSystemTests` (drought/taint constants), `GreenhouseEquipmentScalingTests` |
| Nutrient integration unknown | `item_hydroponic_nutrients` resolves in 3 data files; no Core consumer found yet in this sweep — **Phase 4 must trace the doser-blueprint recipe chain before adding any consumption** | Item ID | Add consumer only if genuinely absent | Do not invent a charge field | `InventoryHostSession` (consumption), greenhouse (demand) | n/a until landed | none yet |
| Apiculture "implement only if repo accepts hive state" | `ApicultureSystem` **already exists** with full hive state (`GreenhouseState.apiculture`), host watering (`Apiary: Insufficient clean water…` at `GreenhouseHostSession.cs:302`) | Entire apiculture loop | Balance/close consumers only | "Deferred hive simulation" premise is obsolete | `ApicultureSystem` (in `GreenhouseState`) | embedded in `greenhouse` | `GreenhouseCropExpansionTests` |
| Blight determinism needs building | Already seeded: `GreenhouseState.blightRollCount`, `BaseBlightChancePerDay = 0.06f`, `OutbreakBlightStep = 0.3f`, `DroughtBlightRatePerDay = 0.25f` | Roll-counter determinism | Add **visible prevention contributors** (the actual B5 gap — risk decomposition is currently implicit in constants) | Nothing | `GreenhouseSystem` | `greenhouse.blightRollCount` | `GreenhouseSystemTests` |
| Research nodes need wiring | `knowledge_greenhouse_microclimate` (1 data file), `knowledge_hydroponics` (2 files) resolve; runtime consumer audit **not yet performed** — Phase 4 gate | Catalog IDs | Trace each ID to a build option / crop family / recipe; no invisible multipliers | Any "free %" interpretation is prohibited | `ResearchSystem` (`IsManualUnlocked`) | `research` section | none yet for greenhouse-specific gating |
| Power/winter coupling needs adding | `room_greenhouse` already exists in `power_grid.json` (160 W, `standard` priority, `fx_grow_lights_off`) and `src/Main.Plans162_165.cs:162_165` reads `IsRoomPowered("room_greenhouse")` for grow lights | Room load | Wire greenhouse Core growth consequences to the powered state if not already (audit in Phase 4) | Do not add a second greenhouse load | `PowerGridSystem` (load), `GreenhouseSystem` (consequence) | `power_grid` rooms | `PowerGridSystemTests` |
| Harvest→kitchen closure may be orphaned | `GreenhouseHarvest` returns `yieldItemId`; consumer sweep pending (Phase 4) | Grant path | Flag orphans in integrity tests; add ≤2 recipes only if truly orphaned | Nothing | `InventoryHostSession` → kitchen catalogs | n/a | none yet |

**B5 net disposition:** keep ~80% of what the brief asks to "build". Real remaining work: blight **prevention** contributors (transparent decomposition), nutrient consumer trace, research-capability gating, harvest-consumer closure audit, save-migration fixtures.

## 2. B6 — Plan 65 (Power) reconciliation

| Task assumption | Live repository truth | Keep | Modify | Delete/obsolete | Owning authority | Save section | Tests that already pin behavior |
|---|---|---|---|---|---|---|---|
| `IsBrownout = TotalDrawWatts > GenerationWatts && BatteryReserveWh <= 0` needs replacing | Confirmed exactly at `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs:79`. **Do not replace.** Extend with typed summary (a `PowerGridTickSummary` already exists at line 640) | Brownout formula + parity tests | Add edge-transition events only if not already present in `PowerGridEvent` (line 607) | "New load model" premise is obsolete | `PowerGridSystem` | `power_grid` | `PowerGridSystemTests`, `PowerGridDeterminismTests`, `PowerGridSurgeTests` |
| Load identity/priorities missing | `PowerGridRoom` registry with `PowerGridRoomPriority { Disabled, Low, Standard, Critical }`, per-room breakers, trip tracking, `SetPriority` — all live. `power_grid.json` carries 9 rooms with draw/priority/failure-effect IDs | Room registry as the load registry | Extend **only** with: deferable/interruptible flags, deterministic intra-class ordinal (audit whether allocation already orders rooms deterministically), and any missing consumer rooms | Do not create a parallel "named load registry" separate from rooms | `PowerGridSystem` + `power_grid.json` | `power_grid` (rooms, breakers, trips, priorities) | `PowerGridCatalogTests`, `ShelterPowerGridCatalogLoaderTests` |
| Generation portfolio missing | Named source contributions **already live**: `SetGenerationContribution/RemoveGenerationContribution` (lines 86–105); nuclear core republish (`Main.AdvancedShelterSystems.cs:479`, `NuclearCoreLifecycleSystem.PowerSourceId`), geothermal ORC (`GeothermalOrcSystem.PowerSourceId = "geothermal_orc"`), Plans 74–77 session contributions | Contribution mechanism | Audit which research-gated sources (solar array via `SolarConcentratorEngine`, battery bank) still lack a build chain; **solar/battery gap is the real remaining B6 work** | Do not rebuild source model | `PowerGridSystem` (allocations) + per-source systems | per-source sections (`nuclear_core_lifecycle`, geothermal, etc.) | `NuclearCorePowerGridPublishTests` |
| Vinyl/audio/schedule bridges need preserving | Confirmed consumers: `RadioBroadcastModels.cs`, `ShelterAudioController.cs`, `ShelterScheduleSystem.cs`, `ReactiveAmbienceEvaluator.cs`, `CrisisPresentationCoordinator.cs`, plus Main subscribers in Plans 62-65/74-77/162-165/198-201/B68-B69/B86-B89/PsyOps/MoraleContagion | All | Migrate to typed tick-summary subscription **only** when a regression-safe equivalent exists | Nothing | `PowerGridSystem` (publisher) | n/a | `PowerGridSystemTests` (verify vinyl coverage; add if absent) |
| Life-support semantics need building | `Critical` priority class exists; audit whether "critical deficit → explicit emergency state" is distinct from generic brownout (Phase 2 gate) | Priority classes | Add explicit critical-deficit flag to summary if absent | Nothing | `PowerGridSystem` | additive field | new tests required |
| Maintenance loop needs building | Filter/condition patterns exist elsewhere (e.g., sump `pumpCondition`, water `filterIntegrity`). Power sources lack a uniform condition/maintenance loop | Existing per-system patterns | Bounded per-source condition **only** where the source system supports it | No global wear model invented in Core | per-source systems | per-source sections | new tests required |

**B6 net disposition:** the grid is far deeper than the brief assumed (rooms=priority loads, breakers, trips, surge/EMP, subgrid distribution via `PowerDistributionSaveStore`, named source contributions, nuclear/geothermal generation). Remaining work: solar/battery build chains, critical-deficit semantics, deterministic-shedding pin, consumer matrix (see `POWER_LOAD_CONSUMER_MATRIX.md`), save-version fixtures.

## 3. B7 — Plan 66 (Water) reconciliation

| Task assumption | Live repository truth | Keep | Modify | Delete/obsolete | Owning authority | Save section | Tests that already pin behavior |
|---|---|---|---|---|---|---|---|
| Water treatment is the authority | Confirmed: `Assets/Ashfall.Core/WaterTreatmentSystem.cs` — 4 pools (`cleanWater`, `rawWater`, `brackishWater`, `irradiatedWater`), 4 treatment modes with efficiencies (0.85/0.70/0.90/0.60), filter integrity, fuel, job log | Entire system | Add throughput/capacity explicitness only if missing | Nothing | `WaterTreatmentSystem` | `water_treatment` (`WaterTreatmentSaveStore`) | `WaterTreatmentCommandTests` |
| A mass-conservation contract needs creating | **Already pinned**: `Ashfall.Core.Tests/Water/WaterAuthorityMassBalanceTests.cs` — draw/pour conservation, refuse-on-full, 200-day mass-balance property test, mid-transfer save/reload | All of it | Extend to new consumers | Nothing | `WaterAuthority` (the tested authority) | its section | 10+ tests in that file |
| Sump bridge hardcodes `0.8f` | Confirmed: `src/Main.ExpandedShelterSystems.cs` `WireWaterTreatmentSumpBridge()` calls `_waterTreatment.SetIncomingContamination(0.8f)` on `FloodStart`/`Contamination` incidents; sump services bound via `BindServices(inventory, waterTreatment, ventilation)` | Bridge behavior | Data-own the severity **only with exact default-behavior parity** (0.8 for legacy incidents) | Magic constant (conditional) | `SumpFloodingSystem` (producer) → `WaterTreatmentSystem` (receiver) | `sump_flooding` + `water_treatment` | `SumpFloodingSystemTests`, `WaterTreatmentSumpBridgeTests` |
| Sump pumps need power registration | `SumpNode.pumpPowered` and `pumpCondition` already exist; audit whether pump draw is a grid room (it is **not** in `power_grid.json`'s 9 rooms — likely a real gap) | Node state | Add pump power-load coupling via `IsRoomPowered`-style projection or a new room | Nothing | `SumpFloodingSystem` (state) + `PowerGridSystem` (availability) | `sump_flooding` | `SumpFloodingSystemTests` |
| Disease/radiation exposure handoffs | `WireWildlifeDiseaseBridge()` shows the canonical pattern: `_disease.Engine.TryExpose(new DiseaseExposureContext{...})` — reuse for unsafe-water servings | Pattern | Add water→exposure routing only through `DiseaseExposureContext` / dose authority contracts | No direct stat writes | `DiseaseSystem` / dose authority | medical sections | follow existing exposure-test pattern |
| Brine economy | `BrineWaterSystem.cs` + `BrineWaterHeadlessDemo.cs` + `BrineWaterSystemTests.cs` **already exist** in Core — full B7 §9.15 premise is stale | Entire brine system | Audit consumers/trade closure only | "Define a canonical brine item" premise is obsolete | `BrineWaterSystem` | its section | `BrineWaterSystemTests` |
| Plan 189 collision | `Next-steps-plans/Plan_189_Water_Source_Management_Contamination_Network.md` confirmed present | Boundary doc required (`PLAN66_PLAN189_BOUNDARY.md`) | — | — | Plan 189 (future) | — | — |

**B7 net disposition:** largest obsolete-premise density. Water has an authority, a conservation test suite, and a brine system already. Remaining work: consumer admission to the shared contract (greenhouse/kitchen/decon audits), sump pump power coupling, advanced-filter/deep-well/condenser consumer traces, boundary doc, migration fixtures.

## 4. B8 — Plan 67 (Defense) reconciliation

| Task assumption | Live repository truth | Keep | Modify | Delete/obsolete | Owning authority | Save section | Tests that already pin behavior |
|---|---|---|---|---|---|---|---|
| Airlock security is the defense owner | `AirlockSecuritySystem` is **door/visitor/incident management** (blast-door integrity, cycling, visitor quarantine, assigned survivor sentry). It does **not** own fortification state | Airlock scope as-is | Do not bolt fortification onto it | Brief's premise that this is the fortification owner is obsolete | `AirlockSecuritySystem` | `airlock_security` | `AirlockSecuritySystemTests`, `AirlockSecurityCommandTests`, `AirlockSecurityIntegrationTests` |
| Fortification needs a new system | **`Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs` (Plan 203) already exists**: 5 sectors (N/E/S/W/gate), emplacements with HP/ammo/jam/destroyed state, `AssaultSimulationResult`, intrusion log, `PerimeterDefenseSaveStore` (schema_version 2) | Entire system | This is the Plan 67 owner. Extend, never fork | "Create DefenseSystem" is **prohibited** — it would duplicate live architecture | `PerimeterDefenseSystem` | `perimeter_defense` | `Defense/PerimeterDefenseTests.cs`, `Defense/PerimeterDefensePlan203Tests.cs` |
| Turrets/tripwires need building | 8 catalog definitions live in `perimeter_defenses.json`: `def_sandbag_berm`, `def_razorwire_obstacle`, `def_tripwire_flare_line`, `def_sentry_turret_9mm` (350 W, ammo_9x19), `def_sentry_turret_556` (600 W, ammo_556), `def_reinforced_outer_gate`, `def_heavy_barricade`, `def_searchlight_tower` (250 W) | Catalog | Audit power-draw integration against the grid (is `power_draw_watts` a registered load?) | Nothing | `PerimeterDefenseSystem` + catalog | `perimeter_defense` | Plan 203 tests |
| Research gates sentry/fortification builds | **GAP CONFIRMED**: no `required_knowledge`/research gating field on any perimeter definition; `knowledge_automated_sentry_doctrine`, `knowledge_turret_controller_blueprint`, `knowledge_fortified_chokepoints` have **zero code consumers** | Research IDs | Add research→build gating via `ResearchSystem.IsManualUnlocked` capability checks | Nothing | `ResearchSystem` (gate) + `PerimeterDefenseSystem` (build actions) | additive field | new tests required |
| `item_sentry_targeting_chip` needs a consumer | **GAP CONFIRMED**: zero consumers in `src/` and `Assets/Ashfall.Core/` (catalog-only). Real remaining B8 work | Item ID | Attach as a build/upgrade dependency to turret emplacements (data-first: build_costs or a required-components field) | Nothing | `InventoryHostSession` + `PerimeterDefenseSystem` | additive | new tests required |
| `item_iff_beacon` needs classification | **GAP CONFIRMED**: zero code consumers. Brief's §10.9 classification exercise still stands (automated-defense encounter consumer) | Item ID | Classify consumer before implementing; do not let it bypass living-faction raids | Nothing | encounter authority (TBD by audit) | n/a | new negative tests required |
| Raid path integration | `src/Main.Muster.cs` Plan 45/163 comments confirm: perimeter emplacements resolve first; "only raiders that breach" reach survivor combat; warlord enforcers from combat catalog | Integration | Preserve; extend aftermath→wear only if not already applied | Nothing | `PerimeterDefenseSystem` (tactical) + warlord/faction pressure (strategic) | both sections | `PerimeterDefenseTests`, Warlords tests |

**B8 net disposition:** the biggest stale premise of all. A full perimeter-defense loop (sectors, turrets with power+ammo, tripwires, assault simulation, wear, save store) landed in Plan 203 after the brief was written. Remaining work is **narrow**: research gating, targeting-chip dependency, IFF-beacon consumer classification, power-draw grid coupling audit, raid-snapshot typing.

## 5. Cross-cutting classifications

| Task assumption | Live truth | Disposition |
|---|---|---|
| Capability query contract (`HasCapability`) missing | `ResearchSystem.IsManualUnlocked(string)` is the existing query (line 61). No `HasCapability` name anywhere | Standardize on a thin `HasCapability` wrapper over `IsManualUnlocked` **only if** ≥3 call sites materialize; otherwise call `IsManualUnlocked` directly |
| Preview/commit transaction pattern missing | Greenhouse already ships `CommandPreview`/`CommandResult` (`PlayerCommandCode.*`, state-version stale checks) | Reuse this pattern; do not invent a second one |
| Water request contract needs inventing | `WaterAuthority` + mass-balance tests are the spendable authority | B7 exposes request-shaped seam over it; no new counters |

## 6. Save sections in scope (existing, do not fork)

`greenhouse` · `power_grid` · `power_distribution` · `water_treatment` · `sump_flooding` · `airlock_security` · `perimeter_defense` · `nuclear_core_lifecycle` · geothermal/ORC sections · `research` · `brine` (verify exact id in `SaveSectionRegistry`).

## 7. Checkpoint verdict

All four task assumptions are now classified. **Blocked-on-nothing**: Phase 1 (contracts) may proceed after this document set is reviewed. The single largest correction to the flagship brief: **Plan 203 PerimeterDefense supersedes most of Plan 67's build scope**, and **water/brine conservation architecture already exists** — B7 shrinks to consumer admission + pump power coupling + upgrade chains.
