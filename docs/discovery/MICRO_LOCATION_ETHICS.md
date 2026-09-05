# ASHFALL — Micro-Location Ethical Choices & Consequence Wiring
## Task F15 Deliverable: Moral Agency, Resource Tradeoffs, and Economic Calibration

---

## 1. Ethical Encounters Overview

Micro-locations provide intimate moral dilemmas on the wasteland trail. Unlike macro-faction choices that alter territory boundaries, micro-location dilemmas test the moral threshold of an individual sortie:
- Will the squad desecrate a memory for survival materials?
- Will they expend scarce food to honor a dead stranger?
- Will they take a keepsake from an abandoned child's shelter?

These choices are grounded in concrete consequences: immediate morale impacts, permanent guilt accrual, and tangible resource exchanges.

---

## 2. Core Ethical Encounters & Resolution Contract

### 2.1 Roadside Shrine (`micro_shrine`)
Located on guardrails where travelers left tokens.
- **Choice 1: `leave_shrine`**
  - Text: *"Leave the shrine undisturbed."*
  - Consequences: `moraleDelta: +2`, `guiltDelta: 0`.
  - Narrative Meaning: Respecting the memory of fallen travelers reinforces humanity in the ashfall.
- **Choice 2: `take_shrine_offerings`**
  - Text: *"Take the small offerings left beneath the cloth."*
  - Consequences: `moraleDelta: -2`, `guiltDelta: +3`, `grantItemId: "jewelry"`, `grantItemQuantity: 1`, `depletesOnResolve: true`.
  - Narrative Meaning: Looting offerings provides valuable scrap/jewelry but demoralizes the crew and increases survivor guilt.
- **Choice 3: `add_shrine_offering`**
  - Text: *"Leave a small offering of your own."*
  - Requirements: `requiredItemId: "canned_food"`, `requiredItemQuantity: 1`.
  - Consequences: `moraleDelta: +3`, `guiltDelta: 0`, `grantItemId: "canned_food"`, `grantItemQuantity: -1`.
  - Narrative Meaning: Sacrificing tangible nourishment for spiritual solidarity delivers the highest morale boost (+3).

### 2.2 Improvised Grave (`micro_improvised_grave`)
A shallow earth mound reinforced with broken concrete and river stones.
- **Choice 1: `respect_grave`**
  - Text: *"Pay respects and move on."*
  - Consequences: `moraleDelta: +2`, `guiltDelta: 0`.
- **Choice 2: `inspect_grave_marker`**
  - Text: *"Read the name and date scratched into the plank."*
  - Consequences: `moraleDelta: 0`, `guiltDelta: 0`, `journalUnlockId: "micro_improvised_grave_marker"`.
- **Choice 3: `disturb_grave`**
  - Text: *"Check beneath the stones for any buried belongings."*
  - Consequences: `moraleDelta: -3`, `guiltDelta: +4`, `grantItemId: "wedding_ring"`, `grantItemQuantity: 1`, `depletesOnResolve: true`.
  - Narrative Meaning: Grave robbery is the most severe moral transgression in the micro-location catalog (-3 morale, +4 guilt).

### 2.3 Abandoned Tent (`micro_abandoned_tent`)
A canvas shelter laced shut from within, containing tin cups and a child's crayon drawing.
- **Choice 1: `leave_tent`**
  - Text: *"Leave the tent undisturbed."*
  - Consequences: `moraleDelta: +2`, `guiltDelta: 0`.
- **Choice 2: `take_drawing`**
  - Text: *"Take the child's drawing from the plastic sleeve."*
  - Consequences: `moraleDelta: -1`, `guiltDelta: +1`, `grantItemId: "childs_drawing"`, `grantItemQuantity: 1`, `depletesOnResolve: true`.
- **Choice 3: `search_tent`**
  - Text: *"Cut open the tent and search for supplies."*
  - Consequences: `moraleDelta: -1`, `guiltDelta: +2`, `grantItemId: "cloth"`, `grantItemQuantity: 2`, `depletesOnResolve: true`.

---

## 3. Consequence Pipeline & Atomic Transactions

1. **Atomic Resolution:**
   - Morale and guilt deltas are calculated and committed in `NarrativeEncounterSystem.TryResolve` within the same transaction that appends to `_state.history` and increments `_state.totalResolved`.
   - The returned `NarrativeEncounterResolutionResult` captures all outcome metadata (`MoraleDelta`, `GuiltDelta`, `GrantItemId`, `GrantItemQuantity`, `JournalUnlockId`, `DiscoverLocationId`, `DepletesEncounter`).
2. **Resource Tradeoff Preflight:**
   - Negative grant quantities (`grantItemQuantity: -1`) represent resource consumption.
   - When `requiredItemId` is specified (e.g. `canned_food` on `add_shrine_offering`), the host verifies sufficient shelter inventory before committing. If insufficient, the resolution is rejected cleanly. No partial morale or guilt deltas are committed on a failed resource check.
3. **Exactly-Once Resolution Guard:**
   - Both the Core bridge (`ExpeditionEncounterBridge.ResolveChoice`) and the host (`ExpeditionHostSession`) enforce resolution idempotency.
   - Once resolved at lead (`resolved_at_lead = true`), re-invoking the resolution for the same surfaced instance returns false immediately.
   - Restoring a save game or refreshing UI views does not re-apply consequences.

---

## 4. Economic Calibration: The Wedding Ring Tradeoff

The choice to disturb the improvised grave grants `wedding_ring` (`tradeValue: 25`, `weight: 0.05` kg):
- **Economic Value:** 25 trade value is approximately equivalent to 2–3 units of canned rations or a basic medical kit, making it an attractive temptation for a starving or desperate settlement.
- **Psychological Cost:** The immediate penalty of `-3` morale and `+4` guilt is the steepest in the micro-location system. In ASHFALL's psychological model, high guilt triggers survivor distress states and trauma events during night segments.
- **Verdict:** The payoff is balanced: immediate material relief at substantial long-term psychological expense.
