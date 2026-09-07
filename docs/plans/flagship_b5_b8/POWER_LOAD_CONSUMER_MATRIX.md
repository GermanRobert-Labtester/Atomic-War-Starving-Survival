# Power Load Consumer Matrix — Phase 0 evidence

> Discovered consumers of `PowerGridSystem` state. Source sweep of
> `src/Main*.cs`, `src/Audio/`, `src/UI/`, `Assets/Ashfall.Core/` at commit
> `4e53ffc7` + working tree. `Phase 2` column = planned B6 action.

## A. Catalog rooms (loads defined in `power_grid.json`)

| Load ID | Draw W | Default priority | Failure effect | Consumers found (read path) | Shed consequence owner | Tests |
|---|---|---|---|---|---|---|
| `room_air_filtration` | 180 | critical | `fx_filtration_off` | `src/Main.CampaignOwners.cs` (air power factor) | air-filtration/shelter atmosphere owner | `PowerGridSystemTests` |
| `room_clinic` | 120 | critical | `fx_clinic_off` | `src/Main.CampaignOwners.cs`, `src/Main.Medical.cs` | medical authority | same |
| `room_water_pump` | 100 | critical | `fx_water_pressure_drop` | `src/Main.ExpandedShelterSystems.cs`, `src/Main.Plans166_169.cs` | water/pressure authority | `WaterAndQuarantinePowerTests` |
| `room_ward_quarantine` | 90 | critical | (catalog) | `src/Main.Medical.cs` | medical/quarantine authority | same |
| `room_cryo_vault` | 280 | critical | (catalog) | `src/Main.PlansB68_B69.cs` (`IsRoomPowered`) | `CryoVaultSystem` | PlansB68-B69 tests |
| `room_greenhouse` | 160 | standard | `fx_grow_lights_off` | `src/Main.Plans162_165.cs` (grow lights powered check) | greenhouse lighting consequence | Plans162-165 tests |
| `room_foundry` | 220 | low | `fx_foundry_standstill` | (workshop-style gating; audit in Phase 2) | `SilentFoundrySystem` | foundry tests |
| `room_lighting_main` | 80 | low | `fx_lighting_dim` | `ShelterScheduleSystem` (lighting schedules) | morale/comfort | schedule tests |
| `room_workshop` | 300 | low | `fx_workshop_offline` | `src/Main.World.cs` (powered gating) | workshop/crafting | existing |

## B. Brownout (`IsBrownout`) subscribers — must remain compatible

| Subscriber | File | Behavior when brownout | Migration note |
|---|---|---|---|
| Radio / vinyl broadcast | `Assets/Ashfall.Core/Radio/RadioBroadcastModels.cs` | broadcast coupling pauses | convert to typed tick-summary subscription last; regression-pin first |
| Shelter audio | `src/Audio/ShelterAudioController.cs` | brownout cue | keep; subscribe to summary event |
| Ambience evaluation | `Assets/Ashfall.Core/Audio/ReactiveAmbienceEvaluator.cs` | audio mood | keep |
| Crisis presentation | `Assets/Ashfall.Core/UI/CrisisPresentationCoordinator.cs` | crisis visuals | keep |
| Shelter schedules | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` | lighting behavior | keep |
| Plans 62-65 / 74-77 / 162-165 / 198-201 / B68-B69 / B86-B89 / PsyOps / MoraleContagion | `src/Main.*.cs` | assorted gating (`!IsBrownout`) | audit each in Phase 2; convert only with parity tests |

## C. Generation sources (named contributions)

| Source ID | Publisher | Watts origin | Save section | Gating research | Gap |
|---|---|---|---|---|---|
| `NuclearCoreLifecycleSystem.PowerSourceId` | `src/Main.AdvancedShelterSystems.cs:479` (`PublishNuclearCoreGeneration`, idempotent, deliberately not saved as part of grid) | reactor output | `nuclear_core_lifecycle` | nuclear chain | none observed |
| `GeothermalOrcSystem.PowerSourceId = "geothermal_orc"` | `src/Host/Plans74To77HostSessions.cs:85` | `ElectricalOutputKw × 1000` | geothermal ORC store | geothermal chain | none observed |
| Base generator | `PowerGridState.GenerationWatts` + `AddFuel(units)` | fuel draw | `power_grid` | — | maintenance/condition loop missing |
| Solar | — | — | — | — | **REAL GAP**: no grid contribution path found for `SolarConcentratorEngine`; no battery-bank build chain found |
| Battery | `PowerGridState.BatteryReserveWh` / `BatteryCapacityWh` (charge/discharge in `TickDay`) | — | `power_grid` | — | efficiency semantics audit (must stay <100%) |

## D. Known gaps for B6 implementation (Phase 2+)

1. **Deterministic shedding order** — audit `TickDay` room iteration for a pinned order; add a test asserting no lower-priority room is served while an eligible higher-priority room is unserved.
2. **Critical-deficit emergency state** — distinguish "all lower loads shed but critical still unmet" from ordinary brownout; surface in tick summary.
3. **Typed brownout edge events** — `PowerGridEvent`/`PowerGridTickSummary` exist; verify begin/continue/end edges and add tests if absent.
4. **Solar + battery build chains** — research-gated build consuming canonical parts; weather/daylight deterministic output; no overnight generation.
5. **Per-source maintenance** — condition/wear with real item costs (reuse per-system patterns).
6. **Perimeter-defense power coupling** — `perimeter_defenses.json` has `power_draw_watts` (turrets 350/600 W, searchlight 250 W); verify `PerimeterDefenseSystem` feeds these into the grid as loads (audit in Phase 7; if not, register).
7. **Save migration fixture** — capture current `power_grid` section bytes before any field additions.
