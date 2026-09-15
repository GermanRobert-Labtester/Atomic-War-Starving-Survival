# PLAN 124 — CVD Diamond Tooling Authority Map (Phase 1)

**Status:** ACCEPTED (reconnaissance). Premise-verified against current source.

## Verified owners

| Concern | Owner | Evidence |
|---|---|---|
| Excavation tool gate | `Assets/Ashfall.Core/ExcavationSystem.cs` — `requiredTools` per layer (:27) | tool-grade gate hook point |
| Excavation hazards/downtime | `Excavation/ExcavationHazardSystem.cs`, `ExcavationCatalogLoader.cs` | existing suites |
| Tool wear / equipment condition | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` + `Inventory/IEquipmentConditionSink.cs` | sole condition authority — registered consumers only |
| Precision certification | `Shelter/PrecisionMetrologySystem.cs` | `PrecisionCalibrationGrade`, `MetrologyGradeDef`, `MetrologyCertificateState`, `MetrologyConsumerDef` (:14–98) — typed grade/certification seam already exists |
| Workshop crafting | `Shelter/ShelterWorkshopSystem.cs` (+ `src/UI/WorkshopPanel.cs` Godot) | industrial job cadence |
| Precursor materials | `Narrative/CeramicsKilnCatalog.cs`, `Narrative/CrucibleFoundryCatalog.cs`, `Shelter/CupolaFoundryCatalog.cs`/`CupolaFoundryEngine.cs`, `Foundry/SilentFoundrySystem.*` | substrate/precursor production through existing foundry owners |
| Save | new `cvd_diamond` section via `SaveSectionRegistry` | active batch, progress, reactor/magnetron condition, substrate state, faults |
| RNG | fork keys `diamond.growth_defect`, `diamond.tool_wear` | deterministic defect generation |

## Answers to workstream 124-A questions

1. Who owns cutter/tool wear? `EquipmentConditionSystem` via `IEquipmentConditionSink`;
   excavation tools are item-condition consumers. Diamond inserts register as a wear
   modifier at the sink level for registered consumers only.
2. Excavation bits items or equipment state? Items with condition (inventory-owned);
   CVD output is an item; its wear advantage applies through the condition sink when the
   consuming machine registers the grade.
3. Precision certification? Already exists: `PrecisionMetrologySystem` grades +
   certificates + consumer defs. Plan 124 uses typed `DiamondToolGrade` → metrology
   consumer mapping; no second certification ledger.
4. Optical-window consumers? None live → optical window item is DEFERRED
   (plan §6.10); excluded from `GAMEPLAY_CONSUMED` expectations.
5. Tool grades? Metrology grades exist; tool-grade enum is new but validated against
   metrology consumer definitions.
6. Industrial job duration? Workshop job cadence (existing owner); CVD batch progress is
   CVD-owned state ticking on the industrial cadence.

## Consumer registry (typed, no global buffs)

`HighWearToolConsumer` registrations: deep-excavation cutter, precision lathe insert.
Unregistered consumers receive zero benefit (test case 12). Wear target: 2x–3x service
interval at high investment — data-driven, never "zero wear" (test case: no global
zero-wear invariant).

## Non-goals

No real CVD/microwave procedures or gas ratios; no universal durability bonus; no
outputs without consumers; no duplicated equipment-condition truth. Defects
(poor crystal/non-uniform layer/graphitic inclusion/bond/surface) are seeded and graded;
certification can reject a batch (idempotent, no reward duplication).

## New files (planned)

- `Assets/Ashfall.Core/Shelter/CvdDiamondSynthesisEngine.cs`
- `Assets/StreamingAssets/Data/cvd_diamond_catalog.json`
- `src/Host/CvdDiamondHostSession.cs`, `src/Host/CvdDiamondSaveStore.cs`
- `src/Main.CvdDiamond.cs`, `src/UI/CvdDiamondPanel.cs`
- `Ashfall.Core.Tests/Shelter/CvdDiamondSynthesisEngineTests.cs`
- Save rows: `cvd_diamond`
