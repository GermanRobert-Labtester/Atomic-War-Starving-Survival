# Epilogue Matrix Schema & Selection Specification

> **File:** `Assets/StreamingAssets/Data/muster_epilogues.json`
> **Schema Version:** 1
> **Core Architecture:** `Assets/Ashfall.Core/Muster/EpilogueMatrix.cs`

---

## 1. JSON Schema Definition

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "title": "MusterEpilogueCatalog",
  "type": "object",
  "required": ["schema_version", "epilogues"],
  "properties": {
    "schema_version": {
      "type": "integer",
      "minimum": 1
    },
    "epilogues": {
      "type": "array",
      "items": {
        "type": "object",
        "required": ["ending_key", "title", "prose"],
        "properties": {
          "ending_key": {
            "type": "string",
            "pattern": "^[a-z0-9_]+$",
            "description": "Unique snake_case identifier corresponding to an EpilogueMatrix canonical key."
          },
          "title": {
            "type": "string",
            "minLength": 2,
            "maxLength": 60,
            "description": "2-6 word evocative title."
          },
          "prose": {
            "type": "string",
            "minLength": 100,
            "maxLength": 800,
            "description": "Cold, restrained, 2-5 sentence outcome text specifying concrete material consequences."
          }
        }
      }
    }
  }
}
```

---

## 2. Selection Precedence Hierarchy

When evaluating terminal campaign state across overlapping systems, `EpilogueMatrix.Evaluate(input)` resolves exactly one authoritative ending key according to this strict priority ladder:

```text
┌─────────────────────────────────────────────────────────────┐
│ 1. Terminal Failure / Collapse (ending_shelter_falls)       │
├─────────────────────────────────────────────────────────────┤
│ 2. Specific Compound Endings                                │
│    - Mercy + Water Held (ending_mercy_water_held)           │
│    - Iron Way + Fuel Burned (ending_iron_fuel_ash)          │
├─────────────────────────────────────────────────────────────┤
│ 3. Verdict Specific Outcome (ending_verdict_*)              │
├─────────────────────────────────────────────────────────────┤
│ 4. Muster Specific Approach (the_open_muster, etc.)         │
├─────────────────────────────────────────────────────────────┤
│ 5. Faction Terminal Outcome                                 │
│    - Garrison Absorbed (ending_garrison_absorbs_coalition)  │
│    - Rebuilders Joined (ending_rebuilders_joined)           │
│    - Independent Coalition (ending_coalition_independent)  │
│    - Foundry Annexation (ending_foundry_annexation)         │
├─────────────────────────────────────────────────────────────┤
│ 6. Strategic Resource Outcome                               │
│    - Water Plant Held (ending_water_plant_held)             │
│    - Grain Silo Captured (ending_grain_silo_captured)       │
│    - Fuel Depot Burned (ending_fuel_depot_burned)           │
├─────────────────────────────────────────────────────────────┤
│ 7. Moral Pattern Outcome                                    │
│    - Mercy Road (ending_mercy_road)                         │
│    - Iron Way (ending_iron_way)                             │
│    - Listener's Thread (ending_listeners_thread)            │
├─────────────────────────────────────────────────────────────┤
│ 8. Fallback / Uninvestigated (unwritten)                    │
└─────────────────────────────────────────────────────────────┘
```

---

## 3. EpilogueMatrixInput Contract

```csharp
public sealed class EpilogueMatrixInput
{
    public bool ShelterFallen { get; set; }
    public bool WaterPlantHeld { get; set; }
    public bool GrainSiloCaptured { get; set; }
    public bool FuelDepotBurned { get; set; }
    public bool MercyPattern { get; set; }
    public bool IronPattern { get; set; }
    public bool DiplomacyPattern { get; set; }
    public string VerdictEndingKey { get; set; } = string.Empty;
    public string MusterEndingKey { get; set; } = string.Empty;
    public FactionTerminalOutcome FactionOutcome { get; set; } = FactionTerminalOutcome.None;
}
```
