# D1 Flagship Integration Plan [17]
## Plan 196 — Food Type Differentiation & Temperature-Dependent Spoilage

> **Purpose:** Replace ASHFALL's one-size-fits-all food spoilage with one authoritative, deterministic,
> data-driven food-condition pipeline where food category, storage temperature, preservation method, processing
> state, and storage environment jointly determine shelf life and safety—without creating per-item
> micromanagement, duplicate temperature authorities, or an inventory-state explosion.
>
> **Primary source:** Plan 196 — Food Type Differentiation & Temperature-Dependent Spoilage.
>
> **Core repository problem:** `KitchenNutritionSystem.cs` already tracks spoilage timers and preservation
> methods, but its shelf-life behavior is effectively shared across all foods. `cellarTempC` exists but does not
> materially participate in spoilage calculations. No food-type catalog, type-specific preservation matrix,
> storage-temperature resolver, temperature-sensitive decay profile, or authoritative food-safety handoff exists.
>
> **Implementation posture:** deterministic, item-definition driven, stack-aware, temperature-source based,
> preservation-aware, save-compatible, bulk-operation friendly, and integrated through existing kitchen,
> inventory, thermal, weather, disease, needs, crafting, storage, and UI authorities.
>
> **Critical guardrail:** food management must become more strategic without becoming clerical. The player
> should make storage and preservation decisions at container/batch level wherever possible; the simulation may
> track per-instance condition internally where required, but the UI should not force manual handling of dozens
> of identical food units one at a time.

---

## 1. Source Problem Statement

The source establishes the current gap:

- `KitchenNutritionSystem.cs` already has `spoilageTimer`, `maxSpoilageDays`, and preservation methods.
- `GetSpoilageDays()` applies broad fixed values independent of actual food category.
- `cellarTempC` exists but is not a real spoilage input.
- no `FoodType`, `FoodCategory`, `SpoilageRate`, or `TemperatureSpoilage` authority exists.
- no type-specific preservation matrix exists.
- no food-safety consequence model is wired through the disease/medical pipeline.

The flagship target is:

```text
item definition / recipe result
            ↓
       food type profile
            ↓
   current storage environment
            ↓
   resolved storage temperature
            ↓
 preservation state / process
            ↓
 deterministic spoilage integrator
            ↓
 food condition / safety band
       ┌────┼──────────┐
       ↓    ↓          ↓
   kitchen  UI      consumption
                    safety check
                        ↓
                 Disease/Medical
```

`FoodTypeSystem` should own food classification and spoilage-state calculation. It must not own shelter
temperature, weather, disease, hunger, inventory, or recipe crafting.

---

## 2. Flagship Success Criteria

The implementation is complete only when all of the following are true:

1. `FoodTypeSystem.cs` exists with schema-versioned persistence for non-derivable food-condition state.
2. Every spoilage-relevant base food item resolves to a valid food type.
3. Food type determines a base spoilage profile.
4. Storage temperature comes from one canonical resolver, not duplicated per-food temperature logic.
5. Shelter thermal state is used for shelter storage.
6. Weather/outside temperature is used for external storage only.
7. Refrigerator/freezer/root-cellar temperatures come from actual storage capabilities where available.
8. Preservation method effectiveness is food-type specific.
9. Incompatible preservation requests are rejected before resource consumption.
10. Spoilage progression is deterministic and does not need `System.Random`.
11. Save/load preserves exact food condition.
12. Identical food in identical storage/preservation conditions decays identically.
13. Storage-temperature changes affect only future decay; history is not rewritten.
14. Per-item temperature is not redundantly persisted when current storage can resolve it.
15. Temperature history is not logged per food item in normal saves.
16. Food safety is derived from condition plus a safety profile.
17. Food poisoning routes through canonical disease/medical systems.
18. NeedsSystem owns hunger/nutrition.
19. Food quality and food safety remain separate concepts.
20. Persistent preservation is distinguished from cold-storage methods.
21. Stack split/merge semantics cannot improve freshness.
22. Old saves do not default every food item to grain.
23. Migration derives type from item data or a validated mapping.
24. Bulk storage and preservation operations prevent click-heavy management.
25. Headless simulation and UI projection produce the same underlying food state.
26. `--food-type-selftest` validates type mapping, temperature decay, preservation, safety, stacks, migration,
    save round-trip, and deterministic behavior.

---

## 3. Repository Reconnaissance Before Editing

Create `docs/food_spoilage/FOOD_SPOILAGE_INTEGRATION_AUDIT.md`.

Inspect at minimum:

- `Assets/Ashfall.Core/KitchenNutritionSystem.cs`
- `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs`
- item catalogs and edible-item tags
- recipe outputs and cooking pipeline
- preservation recipes/stations
- `Assets/Ashfall.Core/World/WeatherSystem.cs`
- `Assets/Ashfall.Core/Shelter/ShelterThermalSystem.cs`
- refrigerator/freezer/root-cellar/storage systems
- inventory stack semantics
- food consumption pipeline
- DiseaseSystem / MedicalPipeline
- NeedsSystem
- kitchen/inventory UI
- storage container APIs
- save schema/migrations
- event bus and deterministic RNG
- Plan 170 seasonal celebrations if present

Authority table:

| Fact | Canonical owner | FoodTypeSystem responsibility |
|---|---|---|
| item identity | Inventory | read stable ID / attach condition component |
| food category | item definition | resolve type |
| shelter temperature | ShelterThermal | read |
| outside temperature | Weather | read |
| storage temperature | storage capability | resolve through adapter |
| hunger/calories | Needs/Nutrition | no ownership |
| disease | Disease/Medical | emit exposure only |
| preservation action | Crafting/Kitchen | validate/result |
| food condition | FoodTypeSystem | authoritative |

Do not add a `currentTemperature` field to every item until storage-temperature authority is known.

---

## 4. Scope Boundary

### In scope
- food type classification;
- spoilage profiles;
- temperature-dependent condition decay;
- preservation compatibility/effectiveness;
- storage-temperature resolution;
- condition/safety bands;
- foodborne-illness exposure handoff;
- stack/batch semantics;
- thermal/weather integration;
- old-save migration;
- bulk operations;
- UI projection;
- CI/selftests.

### Out of scope
- detailed microbiology;
- pathogen-species simulation;
- humidity/water-activity chemistry;
- freezer-burn simulation;
- vitamin-by-vitamin degradation;
- fermentation microbiome;
- real-world food-safety guidance;
- arbitrary cross-contamination simulation;
- cross-campaign food persistence.

The model should be strategically plausible, not a food-science laboratory.

---

## 5. Canonical Data Contract

Create `Assets/StreamingAssets/Data/food_types.json`.

Recommended root:

```json
{
  "schemaVersion": 1,
  "foodTypes": [],
  "temperatureProfiles": [],
  "safetyProfiles": [],
  "preservationMethods": [],
  "storageProfiles": []
}
```

Recommended definition:

```csharp
public sealed record FoodTypeDefinition
{
    public string FoodTypeId { get; init; }
    public string TitleKey { get; init; }
    public string PerishabilityClassId { get; init; }
    public int BaseShelfLifeHoursAtReferenceTemp { get; init; }
    public int ReferenceTemperatureMilliC { get; init; }
    public string TemperatureResponseProfileId { get; init; }
    public string SafetyProfileId { get; init; }
    public IReadOnlyList<string> CompatiblePreservationMethodIds { get; init; }
    public IReadOnlyList<string> PreferredStorageCapabilityIds { get; init; }
}
```

Use hours or fixed-point time units, not whole-day decrements.


---

## 6. Food Type Taxonomy

- Retain the source's starting families: meat, vegetable, dairy, grain, fruit, prepared_meal, and preserved/shelf-stable handling.
- Do not conflate item category, food type, preservation state, cooking state, and storage environment.
- Treat `preserved_food` cautiously: canned meat can remain foodType=meat with preservationState=canned rather than becoming a biologically meaningless universal type.
- Every edible base item must map to one validated food type; base-game fallbacks are CI failures.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 7. Item Definition Mapping

- Add or derive `foodTypeId` from the item definition, not from the runtime item's display name.
- Recipe outputs define their resulting food type explicitly.
- Use a migration map for old item IDs where tags are insufficient.
- Provide `unknown_food` only as a conservative compatibility fallback for legacy/modded items.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 8. Food Condition State

- Prefer `ConditionBasisPoints` 0..10000 plus `LastEvaluatedGameHour` over a single remaining-days timer.
- Persist persistent preservation state, but derive food type and current storage temperature.
- Do not persist rendered labels or estimated shelf life.
- Condition must never increase merely because food is moved into colder storage.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 9. Lazy Spoilage Evaluation

- Do not update each food item every frame.
- Evaluate on storage move, preservation action, consumption, inspection, periodic batch checkpoint, and save/restore boundaries as needed.
- Before changing container, integrate decay in the old environment up to the move time.
- This eliminates the need to persist full temperature history.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 10. Storage Temperature Resolver

- Introduce an `IFoodStorageTemperatureResolver` adapter.
- Resolve shelter room temperature from ShelterThermalSystem, external storage from WeatherSystem, and powered cold storage from the actual appliance/container authority.
- Do not duplicate power-failure or seasonal-temperature logic inside FoodTypeSystem.
- Root-cellar temperature must have one owner; deprecate duplicate mutable `cellarTempC` if ShelterThermal already provides it.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 11. Temperature Response Profiles

- Encode gameplay temperature bands as data with explicit inclusive/exclusive boundaries.
- Initial source bands can seed tuning: frozen/cold/cool/ambient/warm/hot.
- Do not ship '50°C means instant spoilage' as a one-sample rule; model very rapid decay over elapsed exposure instead.
- Ensure the profile covers the entire reachable world-temperature domain.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 12. Deterministic Decay Formula

- Use baseDecayPerHour × temperatureMultiplier × preservationMultiplier × storageModifier × optional item modifier.
- Use fixed-point arithmetic or project-standard deterministic numerics.
- Ordinary spoilage needs no RNG.
- Property test that one 24-hour step equals twenty-four 1-hour steps under constant conditions.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 13. Preservation Architecture

- Separate storage methods from processing methods.
- Refrigeration and root-cellar storage alter temperature only and cease to help once food leaves the container.
- Smoking, canning, fermentation, and drying are persistent processing states or recipe transformations.
- Do not apply both a duration multiplier and an inverse spoilage multiplier for the same effect.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 14. Preservation Compatibility

- Define food-type × preservation-method compatibility in data.
- Reject incompatible combinations before consuming resources.
- Do not permit arbitrary stacking of preservation states.
- Re-preservation must follow explicit recipes and cannot be used to reset freshness.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 15. Preservation Quality

- Preservation may reduce or retain quality, but never magically restore a nearly spoiled input to fresh.
- Use a `qualityRetentionBasisPoints` or equivalent if the game needs quality separate from condition.
- If the current game has no food-quality concept, keep this as metadata and avoid creating a second quality simulation prematurely.
- Input freshness is updated to the process-start time before preservation begins.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 16. Food Safety Model

- Separate freshness/quality from safety.
- Use configurable bands such as Fresh, Aging, Risky, and Unsafe/Rotten.
- Consumption refreshes condition to the current game time before risk resolution.
- Unsafe consequences are routed to DiseaseSystem/MedicalPipeline rather than applied as custom HP/work penalties.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 17. Foodborne Illness Exposure

- Emit a typed `FoodborneIllnessExposure` containing consumer, consumption event, food type, condition band, and exposure severity.
- Use seeded RNG only if the DiseaseSystem requires probabilistic manifestation; seed from the consumption event.
- One consumption event may create at most one exposure.
- Do not add a second nausea/vomiting timer inside FoodTypeSystem.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 18. Season and Nuclear Winter

- Do not add independent seasonal spoilage multipliers.
- Summer, winter, and nuclear-winter effects should emerge through WeatherSystem and ShelterThermalSystem temperatures.
- Cold exterior storage may legitimately function as a freezer if the world/storage model allows it.
- Warmer shelter temperature may naturally improve comfort while worsening ambient food storage.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 19. Power Failure Integration

- Refrigeration effectiveness follows the appliance/storage temperature, not a boolean preservation flag.
- If power fails, the thermal/appliance system determines warm-up; FoodTypeSystem only observes the resulting temperature.
- Do not instantly rewrite all refrigerator contents to room temperature unless that is how the storage system itself behaves.
- Provide aggregate warnings when significant stock is warming.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 20. Stack and Batch Semantics

- Audit whether inventory stores true per-instance food or quantity stacks.
- Do not average a fresh batch and nearly rotten batch into a falsely safe midpoint.
- Merge only when definition, preservation, contamination class, safety state, and freshness tolerance permit.
- Prefer UI grouping over destructive internal merging when exact per-instance state already exists.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 21. Split/Merge Exploit Prevention

- Splitting a stack cannot improve condition.
- Merging cannot hide risky/unsafe units.
- Quantity must be conserved exactly.
- Create dedicated fuzz tests around mixed-condition batches.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 22. FIFO Consumption

- Allow kitchen/consumption logic to prefer the oldest safe compatible batch first.
- This is an anti-waste quality-of-life feature, not a hidden gameplay bonus.
- Respect reservations, quest locks, and recipe constraints.
- Expose a player policy only if the broader inventory policy system exists.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 23. Bulk Preservation UX

- Support selecting a batch/type and applying one preservation job to many units.
- Show total resource cost, station capacity, processing time, and expected shelf-life improvement.
- Use the existing crafting/preservation queue rather than creating a new queue.
- A 50-unit canning job must not require 50 separate clicks.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 24. Bulk Storage UX

- Support moving all compatible meat/dairy/vegetables to appropriate containers.
- Sort by urgency or estimated time to risk threshold.
- Provide actionable storage suggestions rather than per-unit alert spam.
- Auto-routing may be a follow-on if storage policies already exist.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 25. Prepared Meals

- Prepared meal outputs should have short baseline shelf life unless a recipe explicitly creates a preserved product.
- Output production time becomes the new condition anchor.
- Meal batches can share condition when created together.
- Cooking must not automatically make unsafe ingredients safe unless a recipe explicitly defines a safe transformation.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 26. Raw/Harvested/Looted Food Initialization

- Fresh harvest can initialize at baseline freshness or agriculture-provided quality.
- Looted food must preserve procedural condition/expiration where already generated.
- Traded food should expose condition/preservation to the economy without making FoodTypeSystem own price.
- Do not reset acquired food to 100% condition.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 27. KitchenNutrition Refactor

- KitchenNutritionSystem remains nutrition/cooking authority.
- FoodTypeSystem becomes spoilage/type/safety authority.
- Route or deprecate old `UpdateSpoilage()` and fixed `GetSpoilageDays()` logic.
- Search the repository for duplicate shelf-life constants after cutover.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 28. ProceduralItemInstance Integration

- Reuse existing condition/expiration fields where semantically compatible rather than creating `FoodItem` shadow state.
- If a dedicated food-spoilage component is added, ensure one field remains authoritative.
- Do not let fixed expiration and dynamic condition independently make the same food unsafe.
- Migration tests must cover existing procedural food instances.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 29. Old-Save Type Migration

- Resolve type from new item data first, then explicit migration maps.
- Never default every old food to grain.
- Use `unknown_food` only when mapping is genuinely unavailable.
- Record diagnostics for base-game mapping failures.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 30. Old-Save Condition Migration

- Convert old timer proportionally where possible: remaining / old maximum ≈ new condition.
- Do not reset all old food to fresh.
- Map old persistent preservation states carefully.
- Treat old refrigeration/root-cellar flags as storage semantics, not permanent processing states.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 31. Migration Notification Suppression

- Migration should not trigger a flood of spoilage warnings or preservation events.
- Mark already-passed condition-band notifications as consumed where needed.
- Future band crossings behave normally.
- Create golden migration fixtures for representative old-save foods.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 32. Condition Band Events

- Emit events only on meaningful band transitions, preservation completion, unsafe state, and consumption exposure.
- Do not log every temperature change or every daily decay update.
- Use aggregate stockpile warnings for outages/heatwaves.
- Journal/quest systems observe typed events rather than parsing UI messages.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 33. Quest/Achievement Handoff

- Export facts such as fresh stock count, preservation completions, diversity, sustained optimal storage, and foodborne-illness incidents.
- QuestSystem/Plan 149 own progression and rewards.
- Prevent storage-toggle or preserve/unpreserve farming by using stable transaction IDs and sustained-duration metrics.
- Do not reward intentionally causing poisoning.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 34. Seasonal Celebration Integration

- Plan 170 feast activities consume real food batches and therefore still respect condition/safety.
- Celebration systems must not bypass spoilage checks.
- Seasonal harvest/festival flavor can observe food-type diversity and preserved stocks.
- FoodTypeSystem remains a food-state authority, not a celebration authority.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 35. Trade Integration

- Economy may read food type, condition band, and preservation state.
- FoodTypeSystem does not calculate prices or faction standing.
- Shelf-stable preserved foods can become strategically valuable later without special trade state here.
- Unsafe food deception, if ever supported, belongs to trade/moral systems.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 36. Modding Integration

- Plan 165 can expose schemas for food types, preservation profiles, temperature profiles, and item mappings once stable.
- Mods may not inject arbitrary executable spoilage formulas.
- Missing mod mappings use compatibility diagnostics, not silent remapping to grain.
- Completion/integrity validation includes modded food types.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 37. UI Storage Panel

- Group rows by item definition + preservation state + condition band + storage container.
- Show quantity, food type, condition/safety, storage, current effective temperature, and estimated safe life.
- Provide sorts for spoils-soonest, type, storage, and quantity.
- Exact condition percent may be optional; safety band is mandatory.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 38. UI Preservation Panel

- Show compatible methods only or explain why a method is unavailable.
- Show station, resource cost, processing time, quality retention, and expected shelf-life change.
- Support multi-select/batches.
- Never hide a safety risk behind color-only styling.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 39. UI Warnings

- Warn about actionable conditions such as refrigerator outage or large batches approaching risk.
- Aggregate warnings by batch/container.
- Do not notify for every minor temperature fluctuation.
- Ensure keyboard/controller and large-text accessibility.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 40. Estimated Shelf Life

- Derive estimate from current condition and current effective decay rate.
- Label as approximate because future temperature/storage may change.
- Recalculate after storage/preservation changes.
- Do not persist the estimate as authoritative state.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 41. Data Integrity Validation

- Validate unique food-type IDs, item mappings, temperature-profile coverage, preservation compatibility, safety profiles, and localization.
- Reject overlapping/gapped temperature bands.
- Reject invalid multipliers and nonexistent storage capabilities.
- Fail CI if any base-game perishable food is unmapped.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 42. Selftest

- `--food-type-selftest` loads catalogs, checks mappings, simulates each food type, tests temperature ranges, preservation, stack behavior, migration, and safety handoff.
- Test storage move segmentation explicitly.
- Test save/load in cold storage and during power failure.
- Exit non-zero on any mismatch.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 43. Property/Fuzz Testing

- Condition remains 0..10000.
- Elapsed time never increases freshness.
- One large step equals equivalent small steps under constant conditions.
- Same source transaction cannot preserve twice or expose illness twice.
- Stack split/merge conserves quantity and never improves safety.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 44. Performance/Save Budget

- Stress at 10,000 food units and 1,000 batches.
- No per-frame scanning.
- Prefer container/batch evaluation.
- Do not persist temperature history, derived mappings, or daily spoilage logs.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 45. Release Gate

- Release fails if old fixed shelf-life logic remains independently active.
- Release fails if refrigeration/root-cellar are treated as permanent preservation states.
- Release fails if storage moves can reset elapsed spoilage or stack merges hide unsafe food.
- Release fails if UI requires item-by-item handling for common bulk operations.

Implementation consequence: preserve authority boundaries and test the exact transition where this concern crosses into another subsystem. The objective is not merely to make the data exist; the runtime, save model, headless path, and UI projection must all agree on the same source of truth.

---

## 46. Initial Temperature Bands

Use source values as an initial gameplay calibration, not as real-world food-safety guidance:

| Band | Initial multiplier | Interpretation |
|---|---:|---|
| below 0°C | ~0.2x | frozen/near-frozen |
| 0–5°C | ~0.4x | refrigerated |
| 5–15°C | ~0.7x | cool cellar |
| 15–25°C | 1.0x | reference ambient |
| 25–35°C | ~1.5x | warm |
| 35°C+ | ~3.0x | hot |

The actual lower/upper bounds must cover all temperatures reachable from WeatherSystem and ShelterThermalSystem.
Do not use a 50°C instantaneous-spoilage cliff without exposure duration.


---

## 47. Initial Food-Type Baselines

Seed balancing from the source: meat ~2 days, vegetables ~5, dairy ~1, grain 30+,
fruit ~4, prepared meals ~1, and preserved foods much longer at reference temperature. These are game-design
starting points. Create `SHELF_LIFE_CALIBRATION.md` and simulate time to Aging, Risky, and Unsafe under ambient,
cellar, refrigerated, frozen, warm, and hot environments.


---

## 48. Initial Preservation Matrix

Normalize the source's 11+ preservation combinations into one consistent model.
For example, smoking/canning/drying/fermentation should use an effective spoilage-rate multiplier and optional
quality-retention value. Do not multiply both a stated 'effectiveness' and a separate shelf-life multiplier unless
they represent distinct, documented phenomena.


---

## 49. No-RNG Ordinary Spoilage

Ordinary condition decay is deterministic arithmetic. `ISeededRng` is unnecessary
for everyday spoilage. It may be used only where an uncertain consequence exists, such as a probabilistic
foodborne-illness manifestation after consumption. This reduces debugging complexity and removes save-scum
opportunities.


---

## 50. Storage-Move Segmentation

Before an item moves between containers, integrate spoilage from its
`LastEvaluatedGameHour` to the move time using the old container's effective temperature. Only then commit the
storage move and reset the evaluation anchor. This is the flagship anti-exploit rule: ten days of hot exposure
cannot be retroactively treated as refrigerator time merely because the item is moved just before inspection.


---

## 51. Safety-State Monotonicity

Cold storage slows future decay; it does not make risky food fresh again.
Persistent processing may produce a new safe product only through an explicit validated recipe. A simple
container move never raises condition or safety.


---

## 52. Preservation Process Transaction

Flow:
```text
refresh input condition
→ validate food type + method
→ validate station/resources
→ reserve/consume inputs through Crafting
→ process
→ create/apply preserved result
→ emit one completion event
```
Cancellation, save/load, and duplicate callbacks must not consume twice or apply preservation twice.


---

## 53. Foodborne Illness Transaction

Flow:
```text
refresh item condition at consumption time
→ resolve safety profile
→ consume through existing inventory/nutrition path
→ if risky, derive deterministic exposure
→ DiseaseSystem/MedicalPipeline receives exposure
```
FoodTypeSystem never directly writes HP loss, work penalties, or a bespoke nausea timer.


---

## 54. Old-Save Timer Conversion

Where the legacy model stores `spoilageTimer/maxSpoilageDays`, migrate
approximately:
```text
conditionBp = clamp(round(10000 * remaining / max), 0, 10000)
```
This preserves relative freshness better than resetting all food. Type is resolved independently from item data.


---

## 55. Stack Acceptance Scenario

Batch A contains fresh canned vegetables; Batch B contains similar canned
vegetables within merge tolerance; Batch C is already Risky. A/B may group/merge under the chosen inventory
policy, but C must remain separately represented so its unsafe state cannot be hidden. Quantity and total
condition information must be conserved.


---

## 56. Power-Outage Acceptance Scenario

A refrigerator contains meat and dairy. Power fails, the appliance/thermal
system warms over several hours, and FoodTypeSystem integrates using those effective temperatures. No separate
'power-off spoilage multiplier' exists. When power returns, colder temperatures slow future loss; they do not
restore freshness.


---

## 57. Nuclear-Winter Acceptance Scenario

Outdoor subzero storage may extend shelf life naturally if the storage
system permits items outdoors. This is not a special nuclear-winter bonus. Risks such as raids, contamination,
access, or freezing damage belong to other systems.


---

## 58. Bulk-Management Acceptance Scenario

With hundreds of food units, the player can sort by time-to-risk,
move compatible batches to storage, and queue preservation in bulk. If accomplishing the obvious strategy
requires hundreds of individual clicks, the integration is not finished.


---

## 59. Verification Commands

Run:
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --food-type-selftest
```
Also run any existing bridge/save-migration/selftest gates touched by KitchenNutrition, Inventory, Thermal,
Weather, Disease, and Needs.


---

## 60. Risk Register

Primary risk is not algorithmic difficulty; it is creating a tedious inventory chore layer
or two competing spoilage authorities. Secondary risks are stack-state corruption, double temperature
application, preservation reset exploits, old-save freshness loss, and foodborne-illness duplication. Mitigate
with one condition authority, one temperature resolver, batch UI, lazy evaluation, and property tests.


---

## 61. Implementation Phase A — Audit and Authority

Audit current timer/expiration fields, item stack semantics,
storage containers, thermal sources, preservation recipes, and consumption safety. Produce the authority map,
mapping report, and baseline fixtures before changing runtime behavior.


---

## 62. Implementation Phase B — Type Catalog

Add food types, temperature profiles, safety profiles, preservation
bindings, loader, validator, and item mappings. Exit when every base-game perishable food resolves with no
fallback.


---

## 63. Implementation Phase C — Spoilage Integrator

Implement condition basis points, elapsed-time integration,
temperature resolver, fixed-point decay, lazy refresh, and storage-move segmentation. Exit when two foods under
different temperatures produce deterministic, explainable differences.


---

## 64. Implementation Phase D — Preservation

Separate cold storage from persistent processes; wire
smoking/canning/fermentation/drying through existing crafting; enforce compatibility; preserve input condition;
add bulk processing and idempotent transaction tests.


---

## 65. Implementation Phase E — Food Safety

Add safety bands, consumption-time refresh, exposure contract, Disease
handoff, warnings, and rotten-food behavior. Exit when one risky consumption event produces at most one medical
exposure and no duplicate custom debuff.


---

## 66. Implementation Phase F — Thermal/Weather Integration

Wire ShelterThermal, Weather, cellar, refrigeration,
freezer, power failure, and exterior storage through one resolver. Remove any second seasonal or nuclear-winter
spoilage multiplier.


---

## 67. Implementation Phase G — Stack/Inventory Safety

Define merge/split policy, FIFO selection, storage
grouping, batch evaluation, bulk movement, and fuzz tests. Exit when stack operations cannot alter freshness or
hide unsafe food.


---

## 68. Implementation Phase H — Migration

Map item types, convert legacy timers, translate preservation/storage
semantics, suppress notification floods, and validate old-save fixtures.


---

## 69. Implementation Phase I — UI

Build grouped food-storage projection, preservation panel, urgency sorting,
estimated safe life, temperature display, warnings, and accessibility. Keep formulas in Core.


---

## 70. Implementation Phase J — CI Hardening

Add selftest, integrity checks, temperature boundary tests, time-step
independence, stack fuzz, migration tests, 10k-item benchmark, UI snapshots, and full regression.


---

## 71. Exact File Plan

Expected files, adjusted to repository conventions:

### Core
- `Assets/Ashfall.Core/Kitchen/FoodTypeSystem.cs`
- `Assets/Ashfall.Core/Kitchen/FoodTypeDefinition.cs`
- `Assets/Ashfall.Core/Kitchen/FoodConditionState.cs`
- `Assets/Ashfall.Core/Kitchen/FoodSafetyProfile.cs`
- `Assets/Ashfall.Core/Kitchen/TemperatureResponseProfile.cs`
- `Assets/Ashfall.Core/Kitchen/PreservationMethodDefinition.cs`
- `Assets/Ashfall.Core/Kitchen/FoodStorageTemperatureResolver.cs`
- `Assets/Ashfall.Core/Kitchen/FoodSpoilageEvents.cs`

### Data
- `Assets/StreamingAssets/Data/food_types.json`

### Tests
- `Ashfall.Core.Tests/Kitchen/FoodTypeSystemTests.cs`
- `Ashfall.Core.Tests/Kitchen/FoodTemperatureSpoilageTests.cs`
- `Ashfall.Core.Tests/Kitchen/FoodPreservationTests.cs`
- `Ashfall.Core.Tests/Kitchen/FoodSafetyTests.cs`
- `Ashfall.Core.Tests/Kitchen/FoodSpoilageMigrationTests.cs`
- `Ashfall.Core.Tests/Kitchen/FoodStackSpoilageTests.cs`

### Documentation
- `docs/food_spoilage/FOOD_SPOILAGE_INTEGRATION_AUDIT.md`
- `docs/food_spoilage/FOOD_ITEM_MAPPING.md`
- `docs/food_spoilage/FOOD_TYPE_COVERAGE.md`
- `docs/food_spoilage/SHELF_LIFE_CALIBRATION.md`
- `docs/food_spoilage/PRESERVATION_MATRIX.md`
- `docs/food_spoilage/FOOD_SAFETY_CALIBRATION.md`
- `docs/food_spoilage/FOOD_SPOILAGE_MIGRATION.md`


---

## 72. Bootstrap Order

Recommended:
```text
load item catalog
load food-type/preservation data
validate mappings
construct Inventory
construct KitchenNutrition
construct Thermal/Weather/Storage adapters
construct FoodTypeSystem
restore item/food condition state
wire storage/preservation/consumption events
bind UI projection
```
Restore must not evaluate food using default ambient temperature before storage containers are restored.


---

## 73. Definition of Done — Flagship

### Authority
- [ ] one spoilage authority
- [ ] one storage-temperature resolver
- [ ] old fixed shelf-life logic removed/delegated

### Food types
- [ ] 7+ meaningful/reachable types or documented reserved types
- [ ] every base food mapped
- [ ] no universal grain fallback

### Temperature
- [ ] frozen/refrigerated/cool/ambient/warm/hot profile
- [ ] ShelterThermal
- [ ] Weather/exterior
- [ ] cellar
- [ ] powered cold storage
- [ ] storage-move segmentation

### Preservation
- [ ] smoking
- [ ] canning
- [ ] fermentation
- [ ] drying
- [ ] type-specific compatibility
- [ ] no freshness reset
- [ ] bulk operations

### Safety
- [ ] condition bands
- [ ] deterministic consumption risk
- [ ] DiseaseSystem handoff
- [ ] no duplicate health penalty

### Inventory
- [ ] batch/stack policy
- [ ] split/merge safety
- [ ] FIFO option
- [ ] bulk move
- [ ] no freshness averaging exploit

### Migration/Validation
- [ ] legacy timer conversion
- [ ] save/load
- [ ] stack fuzz
- [ ] temperature boundaries
- [ ] 10k stock stress
- [ ] selftest
- [ ] data integrity
- [ ] headless


---

## 74. Follow-On 196-A — Automatic Food Storage Policies

Route incoming food to preferred containers by urgency,
type, capacity, and player policy. FoodTypeSystem supplies recommendations; Inventory/Storage owns movement.


---

## 75. Follow-On 196-B — Preservation Expertise

Use existing cooking/crafting skills to improve processing time or
quality retention. Do not create a new preservation skill authority and do not make basic safety unpredictable.


---

## 76. Follow-On 196-C — Ingredient Freshness and Meal Quality

Allow recipe outcomes to reflect ingredient
freshness once a real meal-quality system exists. Keep nutrition and safety separate.


---

## 77. Follow-On 196-D — Preserved Food Trade

Let economy value condition, shelf stability, and preservation.
FoodTypeSystem exposes descriptors only; Trade owns price and standing.


---

## 78. Follow-On 196-E — Seasonal Food Events

Connect harvests, preserved stores, and feasts to Plan 170 without
adding another calendar or celebration authority.


---

## 79. Follow-On 196-F — Storage Hygiene/Contamination

Only if later design needs it, add sanitation and
cross-contamination via a dedicated hygiene/disease integration. Do not overload v1.


---

## 80. Final Guardrails

- No second Weather authority.
- No second ShelterThermal authority.
- No per-frame food decay.
- No one-size-fits-all fixed spoilage timer after cutover.
- No extra summer/winter multiplier on top of real temperature.
- No universal grain migration fallback.
- No full temperature history per item.
- No RNG for ordinary spoilage.
- No refrigeration/root-cellar treated as persistent processing.
- No arbitrary preservation stacking.
- No preservation freshness reset.
- No storage-move time reset.
- No fresh+rotten stack averaging exploit.
- No duplicate food-poisoning system.
- No one-sample 50°C instant spoilage rule.
- No UI-owned formulas.
- No per-item click requirement for common bulk operations.
- No dead data types added merely to satisfy a quota.

When complete, Plan 196 should make food storage a real systems problem rather than a single timer. The flagship
proof is one food-type catalog, one storage-temperature resolver, one deterministic spoilage integrator, one
medical safety handoff, stack-safe persistence, faithful migration, and bulk management that scales to a real
survival stockpile.


---

## Annex A — Source-to-Flagship Corrections

The source plan is strong in identifying the gap, but several literal implementation details should be refined
before code lands:

1. **Do not default old food to grain.** Type must come from item definitions or an explicit migration map.
2. **Do not persist `currentTemperature` blindly.** Current storage should resolve it; evaluate condition before
   moves to preserve historical exposure.
3. **Do not treat refrigeration/root cellar as persistent preservation methods.** They are storage environments.
4. **Do not apply both preservation effectiveness and duration multipliers unless they have distinct meanings.**
5. **Do not use RNG for ordinary spoilage.** Deterministic decay is simpler and safer.
6. **Do not guarantee that every preserved food can never cause illness.** Model the game's shelf-stable
   transformation explicitly.
7. **Do not make 50°C one-sample exposure instant spoilage.** Use elapsed-time decay.
8. **Do not create per-item temperature histories.** Storage-move segmentation is sufficient.
9. **Do not average different freshness stacks into a misleading midpoint.**
10. **Do not let seasonal modifiers double count temperature.**

---

## Annex B — Deterministic Storage-Move Example

A meat batch begins at 100% condition.

### Segment 1
- 24 hours in refrigerator at 4°C.
- apply refrigerator/cold multiplier.

### Segment 2
- move to exterior at 30°C.
- **before the move**, commit the first 24-hour condition loss.
- reset evaluation anchor at move time.

### Segment 3
- 12 hours outside at 30°C.
- apply warm multiplier.

### Segment 4
- move to cellar at 10°C.
- commit the 12-hour warm exposure first.

### Segment 5
- 36 hours in cellar.
- apply cool multiplier.

A later inspection must produce:

```text
loss = refrigerator segment
     + warm segment
     + cellar segment
```

It must not apply 72 hours at the final cellar temperature.

---

## Annex C — Preservation Transaction Example

Input:
- 20 vegetable units;
- 68% condition;
- canning station;
- compatible jars/resources.

Flow:

```text
refresh condition
→ reserve exact 20-unit batch
→ validate canning compatibility
→ reserve jars/fuel/time
→ process through Crafting/Kitchen
→ output canned state
→ condition cannot exceed the allowed input-retention result
→ one preservation-completed event
```

Failure/reload cannot create 40 units, refund extra jars, or reapply shelf-life multipliers.

---

## Annex D — Food Safety Example

A prepared meal is in the Risky band.

On consumption:

1. update condition to the exact current simulation hour;
2. confirm it remains Risky;
3. consume through normal inventory/nutrition path;
4. create deterministic exposure seed from `consumptionEventId + itemInstanceId + survivorId`;
5. emit at most one `FoodborneIllnessExposure`;
6. DiseaseSystem decides whether an illness manifests.

No direct work-speed penalty or custom nausea timer is created here.

---

## Annex E — Old-Save Migration Example

Legacy vegetable:
- `maxSpoilageDays = 5`
- `spoilageTimer = 2`
- current container = root cellar

Approximation:

```text
condition = 2 / 5 = 40%
```

Migration:

- item definition resolves `foodTypeId = vegetable`;
- condition initializes around 40%;
- root-cellar is represented as current storage, not permanent preservation;
- last-evaluated time becomes migration time;
- future decay uses real cellar temperature.

The item does not become grain and does not reset to fresh.

---

## Annex F — Stack Safety Example

Inventory contains:

- Batch A: 10 canned vegetables, 82% condition.
- Batch B: 5 canned vegetables, 81% condition.
- Batch C: 3 canned vegetables, 23% Risky condition.

A/B may be grouped or merged if the inventory tolerance policy permits.

C must remain separate.

The player must not be able to merge C into A/B and create a falsely safe average. If the UI groups all three,
it must still expose the risky quantity explicitly.

---

## Annex G — Stockpile UX Acceptance

A late-game shelter has:

- 150 vegetables;
- 80 grains;
- 40 meat;
- 20 dairy;
- 30 prepared meals;
- multiple preserved batches.

The player must be able to:

- sort by estimated time to Risky;
- view current storage temperatures;
- move whole compatible batches;
- identify refrigerator/cellar capacity;
- queue bulk canning/smoking/drying;
- see preservation costs;
- receive aggregate outage/spoilage warnings.

This must remain usable without selecting each food instance manually.

---

## Annex H — CI Determinism Digest

For a food batch, normalize:

```text
item/batch ID
definition ID
condition basis points
last evaluated game hour
preservation state
container/storage ID
```

Derived:
- food type;
- current temp;
- safety band;
- estimated shelf life.

Save/load should reproduce the same derived projection from the persisted structural state and restored
environment.

---

## Annex I — Recommended Documentation Deliverables

- `FOOD_SPOILAGE_INTEGRATION_AUDIT.md`
- `FOOD_ITEM_MAPPING.md`
- `FOOD_TYPE_COVERAGE.md`
- `SHELF_LIFE_CALIBRATION.md`
- `PRESERVATION_MATRIX.md`
- `FOOD_SAFETY_CALIBRATION.md`
- `FOOD_SPOILAGE_MIGRATION.md`
- `FOOD_STORAGE_CAPABILITY_MATRIX.md`
- `FOOD_SPOILAGE_PERFORMANCE.md`

These should be treated as implementation evidence, not optional prose.
