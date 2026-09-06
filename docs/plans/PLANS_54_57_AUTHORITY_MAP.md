# Plans 54–57 Authority Map & Cross-System Dependency Matrix

**Scope:** Plans 54 (Trade Barter Economics), 55 (Generational Apprenticeship & Wills), 56 (Deep-Earth Seismic Dynamics), 57 (Wasteland Weather & Shelter Thermodynamics)
**Status:** Approved Architectural Authority
**Author:** Antigravity
**Date:** 2026-09-06

---

## 1. Primary Domain Authorities

| Domain Concern | Canonical Authority | Storage / Catalog Location | Planned Treatment in Plans 54–57 |
|---|---|---|---|
| **Base Item Values & Definitions** | `ItemCatalog` | `Assets/StreamingAssets/Data/items.json` | Reuse existing canonical items; add required tool/manual definitions (`item_blowtorch`, `item_manual_*`). |
| **Dynamic Valuation & Market Modifiers** | `ShelterBarterSystem` / `DynamicEconomySystem` | `Assets/StreamingAssets/Data/merchant_caravans.json` | Fixed-point basis-point arithmetic (10000 = 1.0x). Seasonal & weather multipliers (Ash Blizzard, Deep Winter, Bandit Pressure). |
| **Merchant Caravans & Airlock Trade** | `ShelterBarterSystem` | `Assets/StreamingAssets/Data/merchant_caravans.json` | Gated by `room_airlock`. Atomic transactions. Counterfeit generation & appraisal. Pre-commit storage checks. |
| **Inventory Transfers & Limits** | `Ashfall.Core.Inventory.Inventory` | Shared Inventory container | Hard pre-commit check on volume/weight/slot limits. Atomic execution. Zero duplication. |
| **Survivor Skills & XP Progression** | `SkillProgressionSystem` / `SurvivorExperienceSystem` | `Assets/StreamingAssets/Data/skills.json` | Single authority for XP awards. Mentorship applies bounded multiplier/bonus during shared work; never bypasses canonical XP. |
| **Apprenticeship & Mentorship** | `ApprenticeshipSystem` | `Assets/StreamingAssets/Data/apprenticeship_catalog.json` | Co-occupancy requirement in eligible room tags. Milestone tracking, manual transcription in reading room, legacy effects on mentor death. |
| **Survivor Wills & Inheritance** | `ApprenticeshipSystem` / `SurvivorFateSystem` | Survivor save state / `ApprenticeshipState` | Wills define item transfers executed atomically upon verified death event. Fallback for dead/missing beneficiaries. Zero item cloning. |
| **Geological Stress & Seismic Shock** | `SeismicDynamicsSystem` | `Assets/StreamingAssets/Data/seismic_fault_catalog.json` | Accumulates baseline tension, excavation depth modifiers, and kinetic impact shocks. Deterministic slip threshold and strata attenuation. |
| **Utility Damage Routing** | `SeismicDynamicsSystem` → Utility Systems | Event/Command routing | Shears pipes (`ShelterThermalSystem.ShearPipe`), electrical junctions, and radiator loops. Does not own utility state. |
| **Seismograph Early Warning** | `SeismicDynamicsSystem` | `room_workshop_precision` | Data-driven pre-slip telemetry warning when station is powered, operational, and staffed. |
| **Emergency Shoring & Reinforcement** | `SeismicDynamicsSystem` | Excavation/Shelter save | Consumes steel/hydraulic jacks for temporary shoring; permanent shock-damping reinforcement per sector. |
| **Fissure Gas Outgassing** | `SeismicDynamicsSystem` → `ExcavationHazardSystem` | Hazard events | Emits methane/toxic gas release to existing hazard system upon severe fracture. |
| **Shelter Room Temperatures** | `ShelterThermalSystem` | `Assets/StreamingAssets/Data/shelter_insulation_catalog.json` | Single canonical thermal authority. Newton cooling model, room volume, insulation upgrades, boiler loop, auxiliary stoves, cogeneration. |
| **Pipe Freezing & Thawing** | `ShelterThermalSystem` | `ShelterThermalState.pipes` | Freeze thresholds based on room/waterway temp. Thaw requires ambient heat or blowtorch + fuel action. |
| **Macro Weather Context** | `WeatherSystem` / `YearOfAshDeepFreezeSystem` | World Weather Catalogs | Read-only dependency. Drives outdoor ambient temperature, infiltration, and economic scarcity. |
| **Medical Cold Injury** | `NeedsSystem` / `MedicalWardSystem` | Medical state | Hypothermia, frostbite risk from cold rooms (<5°C) routed to canonical needs/medical authority. |

---

## 2. Shared Daily Tick Execution Pipeline

To prevent recursive cascades (e.g. seismic → power → thermal → water → trade → seismic), execution follows a strict sequential phase order:

```
[Phase 1] Advance Macro Weather (WeatherSystem / DeepFreeze)
     ↓
[Phase 2] Update Ambient Outdoor Temperature & Infiltration
     ↓
[Phase 3] Advance Seismic Stress & Evaluate Queued Tremor Events (SeismicDynamicsSystem)
     ↓
[Phase 4] Route Seismic Damage to Utilities (Pipes, Power, Radiators, Gas)
     ↓
[Phase 5] Resolve Power & Fuel Availability for Heating Equipment
     ↓
[Phase 6] Advance Thermodynamic Simulation (ShelterThermalSystem)
     ↓
[Phase 7] Resolve Pipe Freeze/Thaw Progression & Radiator Heating
     ↓
[Phase 8] Apply Survivor Cold Exposure (NeedsSystem Warmth & Medical Frostbite)
     ↓
[Phase 9] Advance Work Assignments & Apply Mentorship XP (ApprenticeshipSystem)
     ↓
[Phase 10] Advance Scheduled Caravan Arrivals (ShelterBarterSystem)
     ↓
[Phase 11] Recalculate Dynamic Market Multipliers & Elasticity
     ↓
[Phase 12] Resolve Player Barter Actions (Atomic Pre-Commit & Commit)
     ↓
[Phase 13] Execute Survivor Wills / Legacy Hooks on Death
     ↓
[Phase 14] Advance Dependent Agricultural Systems (Crop Thermal Modifiers)
     ↓
[Phase 15] Emit Presentation Telemetry & Save State
```

---

## 3. High-Risk Engineering Invariants

1. **Zero Duplicate Authorities:**
   - `ShelterThermalSystem` is the sole thermal engine. No `ShelterThermodynamicsSystem`.
   - `SeismicDynamicsSystem` emits damage commands; it does not simulate water or electricity.
   - `ApprenticeshipSystem` awards XP via `SkillProgressionSystem`; it does not maintain a parallel skill ledger.
   - `ShelterBarterSystem` executes transactions through `Inventory`; it does not duplicate item instances or storage.
2. **Deterministic Rounding:**
   - Barter valuation uses integer basis points (10,000 bp = 100%) and deterministic integer floor/rounding.
   - Thermal math uses stable analytic exponential relaxation with bounded clamps.
3. **Save Compatibility:**
   - Every system implements versioned CaptureState/RestoreState.
   - Old saves initialize neutral baselines (no sudden freeze or instant earthquake).
