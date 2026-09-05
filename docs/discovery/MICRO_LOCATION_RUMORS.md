# ASHFALL — Micro-Location Rumors & Location Discovery
## Task F16 Deliverable: Clue Integration, Discovery Authority, and Expedition Dispatch

---

## 1. Rumor & Location Discovery Overview

Micro-locations serve as crucial environmental clues that expand the player's map. Scavengers inspecting military observations, cargo drops, and abandoned route logs can extract geographic intelligence that reveals previously undiscoverable destinations.

Two primary micro-location clue chains anchor this integration:
1. **Military Observation Post (`micro_observation_post`):**
   - Choice: `read_grid_references` (*"Copy the grid references and dates from the wall."*)
   - Consequences:
     - Unlocks journal entry: `micro_observation_post_grid`
     - Discovers destination: `rural_gas_station`
2. **Supply Drop (`micro_supply_drop`):**
   - Choice: `read_supply_label` (*"Read the shipping label for destination and origin information."*)
   - Consequences:
     - Unlocks journal entry: `micro_supply_drop_label`
     - Discovers destination: `government_bunker`

---

## 2. Canonical Destination Authority

Both target destinations are authored in `expeditions.json` and marked with `requiresDiscovery: true`:
- **`rural_gas_station`**:
  - Distance: 6 ticks, Danger: 3, Base stamina drain: 2.0/hr.
  - Loot categories: `fuel`, `scrap_metal`, `mechanical_parts`, `canned_food`.
  - Initial State: Hidden from dispatch until discovered.
- **`government_bunker`**:
  - Distance: 20 ticks, Danger: 8, Base stamina drain: 2.5/hr.
  - Loot categories: `military_mre`, `military_radio`, `rad_away`, `anti_rad`.
  - Initial State: Hidden from dispatch until discovered.

Neither location is known at campaign start. They can only be unlocked via:
1. Micro-location encounter clues (`discoverLocationId`);
2. Radio signal triangulation (`SignalTriangulationSystem`);
3. Authored narrative story unlocks.

---

## 3. Discovery Authorities & Execution Order

### 3.1 Dual-System Authority Roles
1. **`ExpeditionSystem` (Dispatch Authority):**
   - Tracks `_knownLocations` (`HashSet<string>`).
   - Governs `CanDispatch(locationId, out string? reason)` and `Start(...)`.
   - Fires `OnLocationDiscovered` on first revelation.
   - Idempotent: Subsequent discoveries return `true` without firing duplicate events.
2. **`SignalTriangulationSystem` (Radio/Intelligence Authority):**
   - Tracks `discoveredLocationIds` and triangulation candidates.
   - Exposes `TryDiscoverLocation(locationId)` returning `LocationDiscoveryStatus` (`NewDiscovery`, `AlreadyKnown`, `InvalidId`).
   - Fires `OnLocationRevealed` on new discoveries.

### 3.2 Multi-Effect Ordering
When a choice triggers multiple effects (e.g. `read_grid_references` triggering journal unlock + location discovery):
1. **Item Delta:** Evaluated first (inventory cargo or offering consumption).
2. **Journal Unlock:** Written to `JournalSystem` via `TryDiscoverKnowledge`.
3. **Location Discovery:** Registered to `ExpeditionSystem` via `DiscoverLocation`.
4. **World Flag:** Stamped in `CampaignConsequenceLedger` if defined.

Failure of one step (such as an already-known journal entry) does not block subsequent steps.

---

## 4. Gated Dispatching & Progression Protection

Once a location is discovered:
- It becomes eligible for sortie planning in `ExpeditionSystem.CanDispatch`.
- **Progression Gates:** Discovery does *not* bypass structural prerequisites:
  - If the destination has `requiredFlagId`, `ExpeditionSystem.FlagChecker` must return `true`.
  - If the destination is gated by `DamagedMapSystem`, map completion rules still apply.
  - Required stamina, stance, and survivor availability must be met normally.
