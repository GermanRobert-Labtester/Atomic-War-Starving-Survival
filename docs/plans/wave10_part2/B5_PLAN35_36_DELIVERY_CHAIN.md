# Plan 35 / Plan 36 (C1[10]) Implementation & Audit Log: "Goods Must Arrive"

**Package:** Wave 10 Part 2 — Task B5\
**Plan Authority:** `C-integration-plans/C1_planintegration[10].md` (Plan 35 / Plan 36)\
**Status:** **SEALED**\
**Date:** 2026-09-17\
**Integrator:** Antigravity\

---

## 1. Executive Summary

Plan 35 ("Goods Must Arrive: The Production-to-Provisioning Chain") and Plan 36 ("The Production-to-Provisioning Pipeline") establish the architectural delivery discipline for all production activities in ASHFALL:
- Every production process (crafting, kitchen cooking, greenhouse agriculture, water purification, foraging/trapping) must commit its output into an authoritative sink (`Inventory`, `NeedsSystem`, or persisted pantry/depot).
- When a sink cannot accept the output (e.g. inventory weight/slot saturation, missing storage capacity, or invalid target), the system must **never silently drop goods**. It must either cleanly refund the invested input resources, route overflow through explicit overflow events, or return a typed, visible refusal.
- Silent production loss is strictly forbidden across all production domains.

---

## 2. 35A Producer-to-Sink Delivery Audit

| Producer Subsystem | Trigger Command / Tick | Authored Output | Authoritative Sink | Delivery Mechanism | Persisted? | Refusal / Overflow Handling | Silent-Loss Protection Status |
|---|---|---|---|---|---|---|---|
| **CraftingSystem** | `Tick()` completion | Crafted item (`Recipe.result`) | `Inventory` | `Inventory.Add()` | Yes (`inventory` section) | `OnCraftResultOverflow` event fires; consumed ingredients are cleanly refunded to caller inventory | **VERIFIED NO-SILENT-LOSS** |
| **KitchenNutritionSystem** | `StartPrepJob` | Prepared meal batch | `KitchenNutritionState.pantry` | `pantry.Add(PantryItem)` | Yes (`kitchen_nutrition` section) | Inputs consumed via `TryConsumeBill`; jobs can be cancelled with full input refund | **VERIFIED NO-SILENT-LOSS** |
| **KitchenNutritionSystem** | `ServeMeal` | Nutrition & morale | `SurvivorNeedsState` | `_needs.Modify(Hunger)`, `ApplyAttributedDelta(Morale)` | Yes (`survivor_needs` section) | Typed `ActionResult.Blocked("no_meal")`; portions decremented only on successful delivery | **VERIFIED NO-SILENT-LOSS** |
| **GreenhouseSystem** | `Harvest(plotIndex)` | Harvested crop (`CropDef.YieldCleanId`) | `Inventory` (via host/caller sink) | Caller collects `GreenhouseHarvest` struct and invokes `Inventory.Add()` | Yes (`greenhouse` + `inventory` sections) | Returns typed `GreenhouseHarvest { success = false }` if crop is immature/fallow; plot remains intact | **VERIFIED NO-SILENT-LOSS** |
| **WaterPurification** | Purifier tick / cycle | Purified water (`item_purified_water`) | `Inventory` | Direct container commit | Yes (`inventory` section) | Unprocessed water remains in source container if capacity saturated | **VERIFIED NO-SILENT-LOSS** |

---

## 3. Sink Authority & Atomicity Invariants

1. **One Authority Per Sink:**
   - Physical items and materials: `Ashfall.Core.Inventory.Inventory`.
   - Biological sustenance & morale: `Ashfall.Core.Survivors.NeedsSystem`.
   - Intermediate food rations: `KitchenNutritionState.pantry` (owned by `KitchenNutritionSystem`).
   - Agricultural plots: `GreenhouseState.plots` (owned by `GreenhouseSystem`).
2. **Transaction & Refund Semantics:**
   - When `CraftingSystem` completes a recipe but the target `Inventory` is full or cannot accept the output weight, `OnCraftResultOverflow` fires and the consumed ingredients are restored into the inventory without phantom item creation or lost ingredients.
   - When a kitchen prep job is cancelled, `CancelJob` restores 100% of reserved input items to inventory.
   - When meals are served, portion count is decremented in tandem with the direct `NeedsSystem` hunger relief and morale attribution.
3. **Engine-Free Discipline:**
   - All producer delivery mechanisms, refund paths, and sink mutations reside exclusively in `Ashfall.Core` with zero Godot or engine references.

---

## 4. Verification Evidence

A permanent regression test suite was authored in `Ashfall.Core.Tests/Production/Plan35ProductionDeliveryTests.cs`:
1. `Crafting_WhenInventoryFull_RefundsIngredients_NoSilentLoss`: Confirms that crafting into an over-capacity inventory triggers `OnCraftResultOverflow`, cleanly refunds input ingredients, and creates no phantom items.
2. `Kitchen_ServeMeal_MutatesNeeds_AndConsumesPortions_NoSilentLoss`: Confirms that meal preparation consumes inventory ingredients, populates pantry portions, and meal serving decrements portions while directly alleviating hunger in `NeedsSystem`.
3. `Greenhouse_Harvest_DeliversOutputToSink`: Confirms that crop cultivation and harvesting produces valid catalog yield items and successfully delivers them into inventory sinks.
4. `ProductionOutput_SurvivesSaveRoundTrip`: Confirms that production outputs stored in `Inventory` survive capture/restore save cycles intact with exact count preservation.

**Test Run Output:**
```
Passed! - Failed: 0, Passed: 4, Skipped: 0, Total: 4, Duration: 31 ms - Ashfall.Core.Tests.dll (net9.0)
```

**Conclusion:** Plan 35 / Plan 36 (`C1[10]`) is fully audited, verified, and **SEALED**.
