# Verdict Expedition Handoff Contract

> **Scope:** Interfacing Verdict investigation sites with the Expedition system (`ExpeditionHostSession` / `ExpeditionSystem`) without duplicate destination authority.

---

## 1. Candidate Selection for Dual Reachability

Per Plan 82 guidelines, two Verdict sites are designated for direct integration into expedition travel corridors:
1. **Coastal Route Candidate:** `loc_clifftop_observation_bunker` (North Cliff Observation Bunker)
   - Matches expedition coastal exploration routes.
   - Travel: 7.5 hours (~15 expedition ticks).
   - High observation value and tactical orientation.
2. **Border Route Candidate:** `loc_border_checkpoint_ruins` (Gate Seven Border Checkpoint)
   - Matches mountain pass demarcation lines.
   - Travel: 8.0 hours (~16 expedition ticks).
   - Significant military and communications salvage value.

---

## 2. Shared Identity & Authority Rules

1. **Single Location ID Rule:** When expedition dispatch targets `loc_clifftop_observation_bunker` or `loc_border_checkpoint_ruins`, the exact canonical string ID must be used across both systems.
2. **Arrival Arbitration:**
   - Expedition arrival triggers standard destination loot and encounter evaluation.
   - The Verdict investigation layer detects arrival at the shared ID and sets the corresponding discovery flag (`flag_verdict_*`), presenting the environmental investigation prompt.
   - No duplicate travel time or double stamina deduction is applied.
3. **Save/Load Integrity:** The expedition sortie state tracks the travel party's physical presence at `destinationId`, while Verdict state tracks `visited_locations`. Both serialize into their respective sections of the campaign envelope without conflict.
