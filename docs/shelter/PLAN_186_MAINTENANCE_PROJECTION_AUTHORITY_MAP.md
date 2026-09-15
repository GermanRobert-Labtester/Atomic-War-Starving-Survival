# Plan 186 — Shelter maintenance projection authority map

**Status:** ACCEPTED — §3 signed 2026-09-12  
**Package:** `DEBT-170-199-REMAINING-FAMILY-MAPS`  
**Date:** 2026-09-12

**Stale historical prose:** `Plan_186_Shelter_Maintenance_Degradation_System.md` (unified 0–100 component HP).

---

## 1. Premise — keep component owners

| Component | Owner | Save |
|---|---|---|
| HEPA / air-filter % | `StartingLevelSystem` | `starting_level` |
| Water filter integrity | `WaterTreatmentSystem` | `water_treatment` |
| Duct / ESP stages | `VentilationSystem` | rebuilt in autopsy setup (no dedicated section) |
| Tool/weapon wear | `EquipmentConditionSystem` | `equipment_condition` |
| Boiler / rooms / insulation | `ShelterThermalSystem` | `shelter_thermal` |
| Intake ice / pipe freeze | `WeatherHardeningSystem` | `weather_hardening` |
| Sump / flood | `SumpFloodingSystem` | `sump_flooding` |
| Cryo plant | `CryogenicAirSeparationSystem` | `cryogenic_air_separation` |
| Sky-layer armor | `SkyLayerArmorSystem` | `sky_defense_battery` |
| External landmarks | `LandmarkDegradationSystem` | **outside bunker** |
| Power | `PowerGridSystem` | load/outage; **no wear field** |

`ShelterMaintenanceSystem` / wall-section HP / unified overall condition: **ABSENT**.

---

## 2. Ownership (proposed)

Inspection is a **read-only projection** of named getters. No stored HP aggregator.

---

## 3. Recommended defaults

| Item | Default |
|---|---|
| Unified durability ledger / `shelter_components.json` | **OUT** |
| Copying landmark HP onto walls | **OUT** |
| Inventing power-generator wear | **OUT** |
| Third air-filter percent beside starting-level + ventilation | **OUT** |
| Host inspection strip that **reads** existing bands | **IN** (implement later) |

**Next implement:** `DEBT-186-INSPECTION-PROJECTION` — presentation/query only. Consumers: HUD codes, Plan 194 later, duty `intake_sleeper` as labor — query, not mutate.

---

## 4. Evidence paths

`StartingLevelSystem.cs`, `WaterTreatmentSystem.cs`, `VentilationSystem.cs`, `EquipmentConditionSystem.cs`, `item_degradation.json`, `ShelterThermalSystem.cs`, `WeatherHardeningSystem.cs`, `LandmarkDegradationSystem.cs`, `src/Main.ExpandedShelterSystems.cs`

**Signed 2026-09-12.** Implement packages listed in this map stay unclaimed until separately promoted.
