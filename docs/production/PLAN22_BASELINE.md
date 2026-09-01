# Plan 22 — Production & Industrial World Baseline

## 1. Executive Summary

ASHFALL's industrial and agricultural systems represent the backbone of long-term shelter survival and post-cataclysm economic rebirth. Prior to Plan 22, the simulation possessed strong architectural scaffolding (`SilentFoundrySystem`, `GreenhouseSystem`, `ApicultureSystem`, `SaltMineExtractionSystem`, `KitchenNutritionSystem`, `DutyRosterSystem`), but content breadth and cross-system friction were limited:
- **Foundry**: 11 products, 16 items, 6 internal division tags, 8 relationship entries.
- **Greenhouse**: 4 crops (mushroom, tuber, grain, wheat), 14 items, 5-stage lifecycle.
- **Apiculture**: Core hive lifecycle with honey and wax buffers.
- **Salt Extraction**: Rock salt, brine, sulfur with treaty fulfillment.
- **Preservation**: Core preservation enum (`RootCellar`, `Refrigeration`, `Fermentation`, `Smoking`, `Canning`) without deep recipe chains.

Plan 22 deepens these existing systems into a tightly coupled industrial ecology with zero duplicate authorities, zero real-world munitions instructions, strict save compatibility, and deterministic simulation.

---

## 2. Verified Authoritative Systems

| Domain | Authority Class | Source Location | Data Authority |
|---|---|---|---|
| **Foundry Production** | `SilentFoundrySystem` | `Assets/Ashfall.Core/Foundry/` | `foundry_production.json`, `foundry_items.json` |
| **Foundry Heat** | `SilentFoundrySystem.Heat` | `Assets/Ashfall.Core/Foundry/` | `foundry_production.json` (heat parameters) |
| **Treaty Labor & Strikes** | `SilentFoundrySystem.TreatyLabor` | `Assets/Ashfall.Core/Foundry/` | `foundry_accords.json`, `foundry_treaty_consequences.json` |
| **Greenhouse Crops** | `GreenhouseSystem` | `Assets/Ashfall.Core/Greenhouse/` | `GreenhouseExpansionCatalog.cs`, `greenhouse_items.json` |
| **Apiculture** | `ApicultureSystem` | `Assets/Ashfall.Core/Greenhouse/` | `ApicultureBeeCatalog.cs`, `items.json` |
| **Salt Extraction** | `SaltMineExtractionSystem` | `Assets/Ashfall.Core/Foundry/` | `items.json` |
| **Kitchen & Preservation**| `KitchenNutritionSystem` | `Assets/Ashfall.Core/` | `recipes.json`, `items.json` |
| **Labor Scheduling** | `DutyRosterSystem` | `Assets/Ashfall.Core/DutyRoster/` | `duty_roster_duties.json` |
| **Culinary Rationing** | `CulinaryRationCatalog` | `Assets/Ashfall.Core/Narrative/` | `culinary_ration_codex.json` |

---

## 3. Product & Content Gap Analysis

### 3.1 Foundry Product Ladder (11 → 25 Products)
- **Baseline**: 11 products (plowshare, t_beam, ice_anchor, winch_drum, brine_pipe, repair_plate, fastener_bracket, valve_body, heavy_tool, alloy_part, defense_plate).
- **Gaps**:
  - Missing tooling rungs: replacement dies, drill blank sets, crucible spares, press fittings, bearing housings, furnace grate sections.
  - Missing structural rungs: roof-armor plate, excavation shoring bracket, blast-door fitting, reinforcement shoe, structural coupling.
  - Missing abstract ordnance rungs: mortar weather shell body, generic cast shot canister, casing blank set.
- **Target**: 25 distinct products across 4 distinct heat/labor bands.

### 3.2 Crop & Culture Ladder (4 → 12 Crops)
- **Baseline**: 4 crops (spore mushroom, tuber cutting, mutated grain, pre-war wheat).
- **Gaps**:
  - No cold-hardy winter staples (Hardy Tuber, Cold Legume).
  - No short-cycle green for quick famine relief (Leafy Green).
  - No oilseed/protein crop for trade and lamp fuel (Oilseed Flax).
  - No dedicated medicinal herb for dispensary supply (Medicinal Herb).
  - No nutrient slurry/algae for low-light fallback (Nutrient Algae).
  - No fungal bioluminescent crop for light/pharma (Biolum Mushroom).
  - No soot-tolerant ash grain (Ash Grain).
- **Target**: 12 crops utilizing the full 5-stage lifecycle and responding to Plan 19 seasonal state.

### 3.3 Apiculture & Salt Products
- **Apiculture Expansion**: Raw honey pot, beeswax block, raw propolis, mead must / fermentation base.
- **Salt Expansion**: Coarse rock preservation salt, trade salt sack, medical-grade saline salt.

### 3.4 Preservation Recipes
- **Baseline**: Basic cooking recipes without dedicated preservation conversions.
- **Target**: 10 distinct preservation recipes converting perishable yields into stable shelf-life rations with balanced salt/fuel/container costs.

---

## 4. Invariant Compliance

1. **No Engine Coupling**: All core systems in `Assets/Ashfall.Core/` remain 100% C# netstandard2.1 / net8.0 without UnityEngine or Godot dependencies.
2. **Zero Munitions Fabrication Detail**: Ordnance products are abstract game data items (`item_foundry_mortar_casing`, `item_foundry_shot_canister`, `item_foundry_casing_blanks`) with material/labor tiers only. No chemistry, explosive recipes, or machining instructions.
3. **Deterministic Simulation**: All random rolls routed through `ISeededRng` (xorshift64*).
4. **Save Compatibility**: Schema versions pinned, state captures versioned, legacy envelopes migrated safely.
