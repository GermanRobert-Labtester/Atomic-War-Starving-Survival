# Plan 46 — Location-Specific Scavenging Tables: Baseline & Architecture

## 1. Overview & Problem Statement
Prior to Plan 46, `expeditions.json` defined destination rewards using bare `lootCategories` string arrays (e.g. `["scrap_metal", "clean_water", "bandages"]`). This created homogeneous expedition returns where location identity was merely cosmetic text rather than distinct physical resource distributions.

Plan 46 establishes `Assets/StreamingAssets/Data/scavenging_tables.json` as the authoritative catalog containing 20 location-specific weighted loot tables with deterministic seeded resolution, quantity boundaries, hazard metadata, and codex unlock hooks.

---

## 2. Architectural Design & Invariants

```
Expedition Destination Definition (expeditions.json)
        ↓ [scavenging_table_id: "table_loot_hospital"]
ScavengingTableCatalog (Assets/Ashfall.Core/Expeditions/ScavengingTableCatalog.cs)
        ↓ [RollLoot(tableId, ISeededRng)]
Deterministic Cumulative Weight Selection
        ↓
ScavengingRollResult (ItemId, Quantity, RarityTier, HazardTriggered, HazardType, CodexUnlockId)
        ↓
ExpeditionState.loot Accumulation
```

### Invariants Maintained
1. **Engine Agnostic (Invariant 1):** `ScavengingTableCatalog.cs` uses zero Unity or Godot dependencies.
2. **Determinism (Invariant 4):** All loot rolls and quantity variations use `ISeededRng` passed from the simulation tick.
3. **Data Authority (Invariant 6):** `Assets/StreamingAssets/Data/scavenging_tables.json` is the sole authority for loot tables.
4. **Fallback Safety:** Unbound destinations seamlessly fall back to legacy `lootCategories` resolution.
