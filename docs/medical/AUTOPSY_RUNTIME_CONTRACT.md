# Autopsy Runtime Contract

> **Runtime Authority:** `Assets/Ashfall.Core/AutopsySystem.cs`, `Assets/Ashfall.Core/AutopsyProcedureCatalogLoader.cs`, `Assets/StreamingAssets/Data/autopsy_procedures.json`.

---

## 1. System Architecture & Lifecycle

The autopsy system in ASHFALL governs post-mortem examinations of deceased survivors to identify causes of death, uncover forensic findings, and unlock scientific and medical knowledge for future casualty prevention.

### The Forensic Loop
$$\text{Death} \longrightarrow \text{Select Procedure} \longrightarrow \text{Commit Tools \& Consumables} \longrightarrow \text{Advance Time} \longrightarrow \text{Roll Risk} \longrightarrow \text{Yield Finding \& Research}$$

---

## 2. Examination Stages

1. **Eligibility & Queuing (`QueueAutopsy`):**
   - Corroborates that the specimen has not already been autopsied (`!_state.completedSpecimenIds.Contains(specimenId)`).
   - Validates that the requested procedure exists in `_catalog`.
   - Validates that all items in `required_tools` and `required_consumables` are currently present in the shelter inventory (`_inventory.CountById(item) >= 1`).
   - If supplies are missing, returns `ActionResult.Blocked("missing_tool")` or `ActionResult.Blocked("missing_consumable")`.
   - Creates an `AutopsyCase` with status `Queued`.

2. **Commencement (`BeginAutopsy`):**
   - Transitions case from `Queued` to `InProgress`.
   - Atomically executes an inventory bill deducting 1 unit of each tool and consumable from inventory via `_inventory.TryExecuteTransaction(bill)`.

3. **Daily Progression (`TickDay`):**
   - For each active `InProgress` case, advances `progressHours += 8f`.
   - **Containment / Airborne Risk:** A deterministic PRNG roll (`_rng.NextDouble() < procedure.airborneRisk`) checks for containment breach.
     - If triggered, sets `case.containmentBreach = true`, logs an alert, and registers a ventilation exhaust hazard with `VentilationSystem`.
   - **Completion:** When `progressHours >= procedure.procedureHours`:
     - Sets status to `Complete`.
     - Appends `specimenId` to `completedSpecimenIds` to prevent duplicate post-mortem examinations on the same remains.
     - Randomly selects a forensic finding from `possibleFindings`:
       $$\text{idx} = \_rng.\text{Next}(0, \text{possibleFindings.Count})$$
     - Iterates through `researchUnlocks` and unlocks each knowledge node in `ResearchSystem` (`_research.UnlockManual(unlock)`).
     - Fires `OnCaseCompleted` and removes the completed case from the active list.

---

## 3. Persistence & Save Compatibility

- `AutopsyState` serializes `systemId`, `cases`, and `completedSpecimenIds`.
- Static procedure definitions are **never** serialized into save files.
- Completed specimens persist by string ID, preventing duplicate examinations across game save/load cycles.
