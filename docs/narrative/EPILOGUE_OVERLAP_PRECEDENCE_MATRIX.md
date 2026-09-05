# Epilogue Overlap & Precedence Matrix

> **Scope:** Multi-system terminal collision handling
> **Evaluator:** `EpilogueMatrix.Evaluate(input)`

---

## 1. Candidate Collision Resolution Table

| Candidate State A | Candidate State B | Overlap Possible? | Authoritative Winner | Resolution Rationale |
|---|---|---|---|---|
| `ShelterFallen` | `WaterPlantHeld` | Yes | `ending_shelter_falls` | Terminal collapse cannot be masked by infrastructure survival. |
| `ShelterFallen` | `FactionOutcome` | Yes | `ending_shelter_falls` | Total shelter extinction overrides external political alignment. |
| `ShelterFallen` | `MercyPattern` | Yes | `ending_shelter_falls` | Moral history does not soften physical destruction. |
| `MercyPattern` + `WaterPlantHeld` | `MercyRoad` | Yes | `ending_mercy_water_held` | Specific compound ending overrides generic moral road. |
| `MercyPattern` + `WaterPlantHeld` | `WaterPlantHeld` | Yes | `ending_mercy_water_held` | Specific compound ending overrides generic resource control. |
| `IronPattern` + `FuelDepotBurned` | `IronWay` | Yes | `ending_iron_fuel_ash` | Specific compound ending overrides generic coercive route. |
| `IronPattern` + `FuelDepotBurned` | `FuelDepotBurned` | Yes | `ending_iron_fuel_ash` | Specific compound ending overrides generic fuel loss. |
| `VerdictEndingKey` | `MusterEndingKey` | Yes | `VerdictEndingKey` | Judicial census determination overrides generic regional muster. |
| `VerdictEndingKey` | `FactionOutcome` | Yes | `VerdictEndingKey` | The Machine's verdict binds regional factions. |
| `MusterEndingKey` | `FactionOutcome` | Yes | `MusterEndingKey` | Concrete approach chosen at the Muster takes precedence over broad standing. |
| `FactionOutcome` | `WaterPlantHeld` | Yes | `FactionOutcome` | Institutional alignment / annexation determines resource disposition. |
| `FactionOutcome` | `MercyPattern` | Yes | `FactionOutcome` | Institutional alignment / annexation determines social order. |
| `WaterPlantHeld` | `GrainSiloCaptured` | Yes | `WaterPlantHeld` | Water infrastructure is more vital to survival than stored grain. |
| `GrainSiloCaptured` | `FuelDepotBurned` | Yes | `GrainSiloCaptured` | Food reserve preservation overrides fuel denial. |
| `Resource Outcome` | `Moral Pattern` | Yes | `Resource Outcome` | Material survival infrastructure overrides behavioral tendencies. |
| `MercyPattern` | `IronPattern` | Rare | `MercyPattern` | Explicit precedence order in moral pattern ladder. |
