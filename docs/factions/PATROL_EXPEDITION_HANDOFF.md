# Flagship Integration — Patrol Expedition Handoff (PATROL-INT-F1-F4)

## Architecture: Option B Direct Bridge

`ExpeditionEncounterBridge` bridges directly to the shared `TravelEncounterSystem` without duplicating patrol encounter JSON into `narrative_encounters.json`.

### 1. Unified Candidate Pool & Single RNG Draw
- During expedition travel, `ExpeditionEncounterBridge.Surface(state)` queries:
  - `NarrativeEncounterSystem.GetEligibleCandidates(...)` (0 RNG)
  - `TravelEncounterSystem.GetEligiblePatrolCandidates(...)` (0 RNG)
- Combines candidate weights and rolls exactly once via the shared expedition `ISeededRng` stream.
- Respects region tags (`the_toll`, `high_scarp`, `industrial_belt`, `dead_suburbs`, `coastal_shelf`), danger level, stance multipliers, and season windows.

### 2. Resolution & Mechanical Consequences
- When the player selects a patrol encounter choice on `ExpeditionPanel`:
  - Routed through `TravelEncounterSystem.ResolveChoice(...)`.
  - Costs (`cost_items`) paid atomically via `InventoryBill` / `Inventory.BeginTransaction`. Any shortage aborts with zero deductions.
  - Required items (`required_item_id`) act as non-consuming prerequisites; items are verified but never deducted.
  - Faction standing delta applies via canonical systems ID using `FactionStandingIdResolver.ToSystemsId` (e.g. `iron_garrison` -> `faction_central_garrison`).
  - Cooldowns recorded in `TravelEncounterState` (`EncounterAvailableDay`) preventing re-surfacing until expiry day.
  - `ExpeditionEncounterBridge.LastResolution` populated for host event logging.

### 3. UI Presentation & Guardrails
- `ExpeditionPanel` renders tactical risk, stance weighting, requirement badges, item costs, and faction standing deltas.
- Buttons are disabled when shelter inventory cannot satisfy requirements or costs.
- Click-time verification ensures atomic failure feedback without prematurely closing the encounter modal.
