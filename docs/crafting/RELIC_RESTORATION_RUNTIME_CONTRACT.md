# Workshop Relic Restoration Runtime Contract (Plan 87)

## 1. Authoritative File Resolution

### Ambiguity Elimination: `relic_inks.json` vs `relic_recipes.json`
- **Audit Finding:** The reference in early project notes to `relic_inks.json` was an unverified draft typo. No file named `relic_inks.json` exists in the repository.
- **Authoritative Catalog:** `Assets/StreamingAssets/Data/relic_recipes.json` is the sole and definitive authority for workshop relic recipes and pre-war artifacts.
- **Loader:** `Ashfall.Core.Crafting.RelicCatalogLoader` (`RelicCatalogLoader.FileName = "relic_recipes.json"`).
- **Runtime Consumer:** `Ashfall.Core.WorkshopReverseEngineeringSystem`.
- **Presentation Layer:** `src/UI/WorkshopPanel.cs` via `CraftingHostSession.Workshop`.

---

## 2. Schema Specification & DTO Structure

`relic_recipes.json` uses a versioned wrapper root object:

```json
{
  "schema_version": 1,
  "recipes": [ ... ]
}
```

Each entry in `recipes` deserializes into `RelicDefinition`:

| Field | Type | Required | Description |
|---|---|---|---|
| `relic_id` | `string` | Yes | Unique canonical identifier (snake_case). Registered in `CatalogIntegrityValidator` definition keys. |
| `display_name` | `string` | Yes | User-facing display title for workshop UI panels. |
| `description` | `string` | Yes | Pre-war backstory and description of current damaged state. |
| `required_components` | `List<string>` | Yes | Array of item IDs consumed from inventory upon starting repair. Validated against `items.json`. |
| `repair_time_hours` | `float` | Yes | Base labor duration in hours, modulated by assigned survivor crafting skill. |
| `morale_bonus` | `int` | Yes | One-time morale delta awarded to the shelter upon completion. |
| `dialogue_event_id` | `string` | Yes | Event identifier resolving to a narrative entry in `events.json`. |
| `restoration_text` | `string` | Yes | Grounded prose describing the moment the artifact returns to function. |
| `world_flag` | `string` | Yes | Persistent world flag (`relic_restored_<relic_id>`), registered in `CatalogIntegrityValidator`. |
| `research_unlock_id` | `string` | Optional | Knowledge node unlocked on research. Empty (`""`) for cultural restoration relics. |
| `dismantle_yield_item` | `string` | Optional | Component yielded if dismantled instead of restored. |
| `dismantle_yield_amount` | `int` | Optional | Quantity of yield item. Defaults to 1. |
| `category` | `string` | Optional | Relic taxonomy (`"relic"` for cultural restoration; `"relic_tech_*"` for technical reverse-engineering). |

---

## 3. Workshop System Mechanics & Lifecycle

1. **Discovery & Cataloging:**
   The workshop session loads the catalog at startup via `RelicCatalogLoader.Load()`. All cataloged relics appear in `WorkshopPanel`.

2. **Component Commitment (`StartRepair`):**
   When the player clicks "RECONSTRUCT & RESTORE", `WorkshopReverseEngineeringSystem.StartRepair(relicId, researcherId)` verifies:
   - Relic exists in catalog.
   - Workshop is not currently busy with another job (`IsBusy == false`).
   - Relic is not already completed (`IsRelicCompleted(relicId) == false`).
   - Inventory contains all items listed in `required_components` (`_inventory.TryConsumeBill(relic.required_components)`).
   - Consumed components are tracked in `WorkshopState.reservedComponentIds` for atomic rollback if canceled.

3. **Time Progression (`TickProgress`):**
   Effective hours required = `relic.repair_time_hours / skillMultiplier`. Progress advances via simulation ticks until `progressHours >= hoursRequired`.

4. **Restoration Completion (`CompleteJob`):**
   - Marks relic as completed: `_state.completedRelicIds.Add(selectedRelicId)`.
   - Returns `ActionResult.Success("workshop.repair_complete", deltas)` with:
     - `deltas["morale_bonus"] = relic.morale_bonus`
     - `deltas["flag_" + relic.world_flag] = 1`
   - Sets `isComplete = true`.

5. **Cancellation & Atomic Refund (`CancelJob`):**
   If canceled before completion, all reserved components in `reservedComponentIds` are refunded back into inventory.

6. **Idempotency & Save Safety:**
   - Completed relics are stored in `WorkshopState.completedRelicIds`.
   - Any attempt to repair an already-completed relic returns `ActionResult.Blocked("already_repaired")`.
   - Restored state persists cleanly across save/load cycles via `CaptureState()` and `RestoreState()`.
