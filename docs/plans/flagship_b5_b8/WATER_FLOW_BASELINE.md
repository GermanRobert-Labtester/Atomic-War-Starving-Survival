# Water Flow Baseline — Phase 0 evidence

> Every producer, transformation, storage field, and consumer discovered at
> commit `4e53ffc7` + working tree. Required by flagship brief §9.3.

## A. Authorities and storage

| Authority | File | Persisted quantity/quality fields | Save section |
|---|---|---|---|
| `WaterAuthority` | Core (see `WaterAuthorityMassBalanceTests`) | plant + inventory reservoir mass; quality classes incl. irradiated policy | its section |

> **CORRECTION (Phase 1):** the `WaterAuthority` / `DrawWater` / `IOutputSink`
> API referenced by `Ashfall.Core.Tests/Water/WaterAuthorityMassBalanceTests.cs`
> is **not yet in Core** — that test file is quarantined via `Compile Remove`
> in the test csproj, owned by an in-flight stream. The live spendable
> authorities today are the `WaterTreatmentSystem` pools, inventory water
> items, and `ConsumeRation`. Phase 1's `WaterRequestContracts`
> (`Assets/Ashfall.Core/WaterRequest.cs`) is the shared consumer seam over the
> live authority; the mass-balance suite governs conservation once landed.
| `WaterTreatmentSystem` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` | `cleanWater`, `rawWater`, `brackishWater`, `irradiatedWater`, `filterIntegrity` (max `filterMaxIntegrity`), `charcoalSupply`, `distillationFuel`, `activeMode` (`TreatmentMode`), `isProcessing/Progress/Target`, `totalWaterProcessed`, `totalContaminationExposure`, `filterReplacements`, `completedJobs` (`WaterTreatmentJob`: mode, input, cleanOutput, wasteAmount, filterDegradation, fuelConsumed, contaminationRemoved, dayCompleted) | `water_treatment` (`WaterTreatmentSaveStore`) |
| `SumpFloodingSystem` | `Assets/Ashfall.Core/SumpFloodingSystem.cs` | per `SumpNode`: `waterLevelCm`, `maxWaterLevelCm`, `hasSumpPump`, `pumpCondition`, `pumpPowered`, `hasFloatValve`, `hasSandbagMitigation`, `isFlooded`, `equipmentDisabled`, adjacency; global `groundwaterLevel`; `incidentLog` (`FloodIncident`) | `sump_flooding` |
| `BrineWaterSystem` | `Assets/Ashfall.Core/BrineWaterSystem.cs` | brine loop state | its section |
| Inventory water items | `items.json` | `clean_water`, `irradiated_water` (greenhouse watering consumes 1 item : 10 units), `brine`-family items | inventory |

## B. Treatment transformation constants (live)

| Mode | Efficiency | Input cost |
|---|---|---|
| Charcoal filtration | 0.85 | charcoal supply |
| Distillation | 0.70 | `FuelPerDistillationUnit = 0.1` fuel/unit |
| Reverse osmosis | 0.90 | filter wear |
| Decontamination | 0.60 | — |

Each completed job logs exact input/output/waste — the conservation ledger already exists.

## C. Producers → transformations → consumers

| Producer | Handoff | Transformation | Consumer |
|---|---|---|---|
| Sump flood/contamination incidents | `WireWaterTreatmentSumpBridge()` in `src/Main.ExpandedShelterSystems.cs`: `OnIncident` (FloodStart/Contamination) → `_waterTreatment.SetIncomingContamination(0.8f)` | incoming contamination raises treatment load | treatment throughput/quality |
| Groundwater / float valves / sandbags | `SumpFloodingSystem` tick | flood progression, `equipmentDisabled` | shelter equipment state |
| Water pump room | `room_water_pump` (critical, 100 W) — `IsRoomPowered` gates pressure (`Main.ExpandedShelterSystems.cs`, `Main.Plans166_169.cs`) | powered pressure | shelter water availability |
| Greenhouse watering | `GreenhouseHostSession.Water` → inventory `clean_water`/`irradiated_water` → `GreenhouseSystem.Water` (tray moisture + `TaintedWaterContaminationPerUnit = 1.5` soil contamination when tainted) | irrigation | crop growth/blight/contamination |
| Apiculture | `GreenhouseHostSession` hive refill from inventory clean water | hive water | honey chain |
| Kitchen / rations | `WaterAuthority.ConsumeRation` (draws plant+inventory, prioritizes clean; irradiated policy → exposure) | potable consumption | survivor needs + exposure |
| Disease exposure | `DiseaseEngine.TryExpose(DiseaseExposureContext)` — canonical pattern shown by `WireWildlifeDiseaseBridge()` | unsafe-consumption → illness | `DiseaseSystem` |
| Brine loop | `BrineWaterSystem` | reject/by-product economics | trade/processing (audit) |

## D. Research / items relevant to B7 (catalog presence)

`knowledge_water_basics` (implied), `knowledge_water_advanced`, `item_water_filter_advanced` (5 files), `knowledge_deep_well_hydraulics` (3 files), `item_hydraulic_actuator` (4 files), `item_desal_membrane` (5 files), condenser chain — **consumer audit per ID is a Phase 6 gate; research alone must not change treatment efficiency.**

## E. Already-pinned test contracts (do not regress)

- `Water/WaterAuthorityMassBalanceTests` — draw/pour conservation, refuse-on-full, partial delivery exact mass, irradiated policy exposure, 200-day property test, mid-transfer save/reload.
- `WaterTreatmentCommandTests` — command surface.
- `SumpFloodingSystemTests` — flood/pump/condition behavior.
- `WaterTreatmentSumpBridgeTests` — contamination save/restore parity across the bridge.
- `BrineWaterSystemTests` — brine loop.
- `WaterAndQuarantinePowerTests` — power-gated water/quarantine.

## F. B7 implementation notes

1. **No parallel water counter may be introduced** — the reservoir + inventory + treatment pools are the spendable authorities.
2. `0.8f` bridge constant: data-own only with exact legacy parity (existing saves and new floods must behave identically before any balancing).
3. Sump pump is **not** in `power_grid.json` rooms — candidate new room/load or projection coupling; decision in Phase 3.
4. Unsafe-water servings must route via `DiseaseExposureContext` / dose contracts — never direct stat writes.
5. New sources (deep well, condenser/desal) are bounded build-state additions using existing item chains; full network simulation belongs to Plan 189 (see `PLAN66_PLAN189_BOUNDARY.md`).
