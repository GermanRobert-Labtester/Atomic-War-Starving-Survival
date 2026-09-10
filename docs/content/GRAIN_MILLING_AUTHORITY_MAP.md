# Grain Milling Authority Map & System Boundaries

## 1. Core Principle

**Plan 157 Non-Negotiable Directive**: `GrainMillingCatalog` owns authored historical and technical observations only. It does not own live grain quantity, food safety, spoilage rates, hunger restoration, recipe output, crop production, or market price.

Grain milling records explain *why* food processing and bulk storage are technically difficult and hazardous in the wasteland. They may never silently decide how much food the player owns, how safe it is, or how much flour a recipe produces.

---

## 2. Architectural Boundary Matrix

| Domain / Responsibility | Authoritative System | Plan 157 Grain Milling Interaction | Strict Boundary Constraint |
|---|---|---|---|
| **Grain & Crop Production** | `GreenhouseSystem` / `AgricultureSystem` | Read-only thematic link to post-harvest requirements | Cannot spawn crops, change crop growth cycles, or mutate harvest yields. |
| **Inventory & Stock Ownership** | `Ashfall.Core.Inventory.Inventory` | Read-only item inspection / provenance notes | Cannot grant items, modify stack limits, or alter item weights. |
| **Live Milling & Processing Jobs** | `GrainProcessingSystem` | Provides context for shelter milling operations | Cannot alter `processing_hours`, job recipes, or worker allocation. |
| **Food Recipes & Cooking** | `FoodSystem` / `KitchenNutritionPanel` | Explains flour extraction and bran separation | Cannot modify recipe inputs/outputs (`recipe_ash_grain_flour` output remains exactly 3 flour for 2 grain). |
| **Food Spoilage & Decay** | Shelter Spoilage / Food Condition | Illustrates historical moisture and mold risks | Cannot alter live food shelf life or cause current food decay. |
| **Pest Infestation & Biosecurity** | `GrainProcessingSystem` (silo pest pressure) | Historical weevil audits provide environmental lore | Viewing a weevil audit record does NOT infest current silos or player inventory. |
| **Illness & Toxins** | `DiseaseSystem` / `MedicalSystem` | Historical aflatoxin/mycotoxin references | Cannot trigger disease outbreaks, nausea, or poison status effects. |
| **Commodity Pricing & Barter** | `EconomySystem` / `ShelterBarterSystem` | Displays provenance notes in inspection | Cannot modify item trade values or merchant caravan price multipliers. |
| **World Geography & Facilities** | `ExpeditionSystem` / `WastelandMapSystem` | Maps equipment to existing `loc_*` sites | Cannot create new world map locations solely to host equipment IDs. |
| **Historical & Industrial Science** | **`GrainMillingKnowledgeSystem`** (Plan 157) | **Authoritative owner of discovery state and technical projection** | Read-only codex and journal presentation of authored historical data. |

---

## 3. Numeric Firewall & Invariant 5 Enforcement

In accordance with **Invariant 5** (Zero Host Gameplay Coupling) and Plan 157 §4.3:

1. **`flour_extraction_yield_pct` (55.0% - 86.0%)**:
   - Strictly descriptive of historical test batches and sifting configurations.
   - Never mapped as a multiplier to `recipe_ash_grain_flour` output.
2. **`grain_moisture_content_pct` (11.5% - 19.5%)**:
   - Represents the moisture percentage recorded in a historical silo audit.
   - Never assigned to `GrainSiloState.moisture_pct` in live shelter silos.
3. **`grain_temperature_celsius` (12.0°C - 48.0°C)**:
   - Records past fermentation or ambient conditions at sample time.
   - Never drives shelter thermal simulation or player temperature hazards.
4. **`runner_rotational_rpm` (105.0 - 130.0 RPM)**:
   - Technical parameter of historical waterwheels, steam engines, and grist mills.
   - Never modifies power consumption in `PowerGridSystem` or machine speed in `ShelterWorkshop`.
5. **`tempering_water_addition_pct` (0.0% - 6.5%) & `conditioning_dwell_hours` (0.0 - 24.0h)**:
   - Historical conditioning assays.
   - Never consumes potable water from `WaterTreatmentSystem` or locks jobs in `GrainProcessingSystem`.

---

## 4. Prohibited Incidental Expansions (Workstream G / §4.4)

The following systems are explicitly **forbidden** from being introduced under Plan 157:
- No grain dust explosion mechanics or explosive atmosphere hazard grids.
- No live mycotoxin or aflatoxin poisoning simulation.
- No dynamic weevil reproduction cycles or crawling insect sprites.
- No survivor "Miller" skill tree, apprentice bonuses, or milling perks.
- No interactive grain tempering minigames or stone dressing QTEs.
- No synthetic flour-grading economy or luxury bread tiers.
