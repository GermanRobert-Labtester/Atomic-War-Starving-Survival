# Psychological System Overlap Audit & Non-Overlap Contract

This audit ensures ASHFALL maintains clear boundaries between its psychological systems without duplicating mental health meters or creating parallel sanity frameworks.

---

## 1. System Inventory & Responsibilities

| System | Primary Role & Tracking | Producer Events | Immediate Symptoms | Timescale | Recovery Path |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **`PsychologicalContaminationSystem`** | Contextual dread burden from visiting horrific disaster locations / deep dive wrecks. | Visiting disaster locations (`location_sunshine_daycare`, `location_automated_abattoir`, deep wrecks). | Action refusal on sensitive tasks (cooking, child comfort), work avoidance. | Transient (2–5 days). | Time away, shelter rest, companion grounding. |
| **`SomaticFlashbackSystem`** | Embodied, sensory memory intrusion triggered by ambient environmental stimuli (sirens, smoke, ash). | Sensory trigger events matching survivor trauma profile. | Temporary paralysis / freeze state during active expedition ticks. | Instantaneous (tick/encounter). | Calming breath, companion intervention, end of encounter. |
| **`CombatTraumaSystem`** | Acute stress and psychological wounds sustained in life-or-death firefights or ambushes. | Critical hits, companion death in battle, close mortar blast. | Reduced accuracy, suppression vulnerability, combat panic. | Medium-term (encounter to several days). | Field triage, companion grounding, safe camp rest. |
| **`GuiltInsomniaSystem`** | Moral burden and sleeplessness arising from ethical triage, rationing refusals, or abandonment. | Selfish moral choices, refusing shelter entry, cutting rations. | Sleep deprivation, delayed stamina regen, fatigue accumulation. | Cumulative over weeks. | Atonement quests, memorial reflection, counseling, honest ledgers. |
| **`NeedsSystem` (Morale/Stress)** | Day-to-day emotional resilience based on hunger, thirst, warmth, and shelter comfort. | Daily hunger, cold, shelter decay, crowding. | Global work efficiency modifier. | Daily baseline. | Good food, warm bed, music, community shelter events. |

---

## 2. Non-Overlap Invariant Contract
1. **No Sanity Meter:** There is no global "Sanity Points" or "Madness Level".
2. **Contextual Gating:** `PsychologicalContaminationSystem` tracks *specific location exposure* (e.g. daycare, abattoir, flooded wreck compartment). It only blocks actions meaningfully related to the trauma (e.g. daycare trauma blocks `action_teach_child`, abattoir trauma blocks `action_cook`).
3. **Downstream Handoff:** Severe psychological contamination (Stage 3/4) does not apply parallel duplicate debuffs; instead, it raises the eligibility flag for a `GuiltInsomniaSystem` sleep disturbance or `CombatTraumaSystem` stress event.
