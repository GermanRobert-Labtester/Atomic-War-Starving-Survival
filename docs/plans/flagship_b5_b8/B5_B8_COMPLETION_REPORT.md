# B5–B8 Flagship Completion Report — Plans 64–67 (Shelter Closed-Loop Survival Infrastructure)

> Completed 2026-09-13 against commit `87b199b2` plus the large pre-existing
> uncommitted working tree (other waves' work preserved untouched).
> Execution followed the phased order in §11 of the flagship brief.

## A. Repository baseline

- Implemented against `87b199b2` (2026-09-13 HEAD) + uncommitted working tree.
- Baseline docs from the Phase 0/1 checkpoint (committed earlier) were treated
  as authoritative and updated only with evidence:
  `docs/plans/flagship_b5_b8/{B5_B8_BASELINE_RECONCILIATION, B5_B8_AUTHORITY_MAP,
  POWER_LOAD_CONSUMER_MATRIX, WATER_FLOW_BASELINE, PLAN66_PLAN189_BOUNDARY,
  RAID_DEFENSE_AUTHORITY_MAP}.md`.
- Biggest premise corrections (all from the Phase 0 reconciliation, honored
  throughout): `PerimeterDefenseSystem` (Plan 203) is the Plan 67 owner;
  `BrineWaterSystem` and `ApicultureSystem` were already live; tray moisture is
  state, not a water counter; the water spendable authority is the treatment
  pools + inventory water items.
- Obsolete task statements rejected: "build TreatBlight" (existed),
  "replace IsBrownout semantics" (extended, never replaced), "create
  DefenseSystem" (prohibited), "define a brine item" (economy already exists).

## B. Files changed (by layer)

| Layer | Files |
|---|---|
| **Core** | `Shelter/PowerGridSystem.cs` (allocation, tick summary, `IsRoomServed`, `RegisterLoadRoom`, battery banks, generator condition, load registration), `Shelter/SolarConcentratorEngine.cs` (grid-tie), `SumpFloodingSystem.cs` (pump load registration + served-state migration), `DecontaminationSystem.cs` (canonical water/bleach), `Greenhouse/GreenhouseSystem.cs` (nutrient band + risk profile), `Farming/AgricultureSystem.cs` (winter light helper), `DeepWellSystem.cs` (new), `Defense/PerimeterDefense{Catalog,System}.cs` (research-gated builds), `Save/SaveSectionRegistry.cs` (+deep_well row/filename; +pre-existing anomaly_hazard filename gap) |
| **Host/composition** | `Host/PowerGridHostSession.cs`, `Host/SolarConcentratorHostSession.cs`, `Host/GreenhouseHostSession.cs`, `Host/WaterTreatmentHostSession.cs` (filter-cost repair), `Host/SumpFloodingHostSession.cs`, `Host/DeepWellSaveStore.cs` (new), `Host/DeepWellHostSession.cs` (new), `Main.DeepWell.cs` (new triad), `Main.Plans110_113.cs` (solar publish), `Main.World.cs` (battery/generator routes + dose dispatch), `Main.Plans162_165.cs` (winter env + BUILD route + served-state turret power), `Main.SaveOrchestrator.cs` (2 call sites), `Main.ExpandedShelterSystems.cs` (tick lines) |
| **UI** | `UI/PowerGridPanel.cs`, `UI/GreenhousePanel.cs`, `UI/SumpFloodingPanel.cs`, `UI/DefenseGridPanel.cs` |
| **Data/catalogs** | `perimeter_defenses.json` (5 research gates, 2 targeting-chip costs — minimal diffs) |
| **Save/migration** | `Fixtures/B5B8_Phase0/{power_grid,greenhouse}_phase0.json` re-captured (additive fields only) |
| **Tests** | `PowerGridPhase2AllocationTests` (14), `PowerGridPhase2GenerationPortfolioTests` (11), `Phase3WaterIntegrationTests` (10), `DeepWellSystemTests` (10), `GreenhousePhase4LoopClosureTests` (13), `PowerGridPhase5MaintenanceTests` (9), `PerimeterDefensePhase7Tests` (7), `FlagshipShelterScenarioTests` (6); + canonical-id fixture updates in decon test files |
| **Docs** | 10 phase docs + this report under `docs/plans/flagship_b5_b8/` |

## C. Authority changes

| Domain | Owner before → after | Why | Migration impact |
|---|---|---|---|
| Deterministic load allocation | none (global brownout) → `PowerGridSystem` | flagship Phase 2 | additive; legacy reads preserved; consumers migrate by phase |
| Battery capacity builds | fixed → `PowerGridSystem` (banks) | canonical item chain | old saves: 0 banks, capacity unchanged |
| Generator condition | none → `PowerGridSystem` | §8.8 closure | legacy restores healthy (100) |
| Solar grid feed | none (output existed, no path) → host publishes under `solar_concentrator` | §8.7 | no feed without a wired inverter |
| Deep well (source) | none → `DeepWellSystem` | §9.8 bounded source | new section; raw water via Plan 189 seam |
| Pump/sentry power reads | global outage → allocation-aware served state | §6.3/§8.11 | critical infrastructure survives brownouts while served |
| Decon water cost | free-but-blocked → canonical bill | silent-failure repair | no legacy stacks can exist (unreachable bootstrap) |
| Filter replacement | free → 1× water_filter | §8.8 closure | none (host-level cost) |
| Emplacement construction | ungated → research-gated | §10.5/§15.3 | ungated fieldworks unchanged |
| Greenhouse nutrients | item existed, no consumer → per-plot band | §7.5 | legacy restores 0 (no free prevention) |

No domain was duplicated; every change extends the frozen authority map.

## D. New/changed IDs

- **Source/load ids**: `solar_concentrator` (grid contribution), `room_deep_well_pump` (registered load), `sump_<nodeId>` loads (Phase 3).
- **Water source id**: `source_deep_well` (Plan 189 intake seam).
- **Save section**: `deep_well` (191st; versioned envelope).
- **Item IDs given real consumers**: `item_solar_inverter`, `item_battery_reconditioned`, `item_sentry_targeting_chip`, `item_hydraulic_actuator` (+2× mechanical_parts), `machine_oil` (generator + well service), `water_filter` (treatment replacement), `clean_water` + `item_liquid_bleach_carboy` (decon repair — `water_clean`/`soap` existed in no catalog).
- **Research ids gated in data**: automated_sentry_doctrine, turret_controller_blueprint, fortified_chokepoints, defensive_tripwire_arrays; runtime-gated: greenhouse_microclimate (host query), deep_well_hydraulics (host query).
- **Catalog field**: `PerimeterDefenseDefinition.required_knowledge` (additive, empty = ungated).
- **Crop save field**: `GreenhousePlotState.nutrientLevel`; grid fields `InstalledBatteryBankCount`, `GeneratorCondition`; solar field `gridTieConnected`.

## E. Save compatibility

- All state additions are additive with legacy-neutral defaults (banks 0, condition 100, nutrients 0, grid-tie false, well unbuilt); each pinned against a pre-migration payload by test.
- Two Phase 0 fixtures re-captured deliberately (`InstalledBatteryBankCount`, `GeneratorCondition`, `nutrientLevel`); the other four fixtures byte-parity untouched.
- New `deep_well` section: save gates bumped to 191 (VersionReport pins were **already stale** at 186 vs a live 190 before this package — noted, then bumped to true counts); pre-existing `anomaly_hazard` filename-map gap from the Plans 174–177 wave fixed (shared gate blocker).
- Scenario G + per-phase restore tests: mid-crisis restore replays nothing.

## F. Determinism

- Blight rolls: persisted counter stream continues (never skipped/replayed) — Phase 0 pins hold.
- Allocation: strict priority prefix, no RNG in order — determinism tests green.
- Brownout edges: derived from state transitions; restore re-seeds (no replay).
- Winter light + well yield + solar feed: pure functions of inputs; RNG never consumed by preview/UI/save.

## G. Balance

30-day tables (Early/Mid/Late) in `PHASE8_SCENARIOS_BALANCE.md`: brownout-hours
52.3 → 15.1 → 1.0, **zero critical-deficit days in every profile**, well yield
1166 L/30d (raw, treatment-gated), harvests 6/12/18, generator wear 92% after
30 burning days — upgrades help visibly; every input remains nonzero.

## H. Verification

| Gate | Result |
|---|---|
| `dotnet test Ashfall.Core.Tests` (full suite) | **10,980 / 10,984** — 4 failures, all pre-existing outside B5–B8 paths (narrative prose pin; 2× HostCli help pins from other waves' uncommitted flags; `DateTime.UtcNow` in CvdDiamondPanel.cs). Fixed in passing: 9 stale decon fixtures, save-registry map gap, 2 stale section-count pin sets, architecture-map node |
| Focused phase suites (2/3/4/5/6/7/8) | all green (details in the 8 phase docs) |
| `godot --headless ... --data-integrity-selftest` | PASS — 0 findings, 325 catalogs |
| `godot --headless ... --content-utilization-selftest` | PASS — 0 hard failures |
| `godot --headless ... --panel-bind-lifecycle-selftest` | PASS |
| `godot --headless ... --ui-accessibility-selftest` | PASS — 5/5 gates |
| `godot --headless ... --scene-binding-selftest` | 25/25 PASS |
| `python3 scripts/ci/generate-architecture-map.py --check` | OK — 191 subsystems, 100% evidence |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

> **D1 addendum (2026-09-17):** `Fixtures/B5B8_Phase0/sump_flooding_phase0.json`
> was re-captured via `B5B8_CAPTURE_FIXTURES=1`. The only drift was the
> additive `SumpNode.lastNetLevelChangeCmPerDay` field (C2[6] 23B rising-water
> clock), which a legacy fixture deserializes as 0 and re-serializes, breaking
> byte parity. The other five Phase 0 fixtures remain byte-identical.

## I. Deferred scope (no silent TODOs)

1. **Battery conversion efficiency <100%** — needs the hourly battery model; the legacy daily aggregate math is parity-frozen (never energy-creating today).
2. **Atmospheric condenser build** — blocked by the broken `craft_desalination_still` recipe (content stream); `knowledge_water_condenser_blueprint` flagged.
3. **IFF automated-defense encounter consumer** — no such encounter authored; classification documented (Phase 7); never wired to human raids.
4. **Kitchen water demand** — no existing mechanic; would be new balance scope.
5. **Deep-well panel UI** — routes live via session/Main; a panel lands in a future UI wave (routes are real).
6. **Per-source condition for nuclear/geothermal/solar feeds** — each owner owns its condition; only the base generator lacked a loop (closed).
7. **30-day flagship Soak as a permanent CI target** — scenarios are permanent tests; the soak table is regenerated per release.
8. **Content-stream flags (not edited here)**: `recipe_aeroponics_nutrient_batch` net-creation loop; `assemble_pure_sine_solar_inverter` result-id oddity; `craft_desalination_still` resultAmount 0; `CvdDiamondPanel` DateTime.UtcNow; HostCli help pins; Crossing prose pin.

Ready for foreman acceptance review.
