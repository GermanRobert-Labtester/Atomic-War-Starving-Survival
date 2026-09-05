# Utility Action Target Matrix

> **Target Arbitration:** Selection, occupancy, and reservation semantics for targeted actions.

---

## 1. Targeted Actions Overview

| Action ID | Target Type | Selection Criteria | Concurrency / Reservation Rule |
|---|---|---|---|
| `action_treat_wounded` | Survivor (Patient) | Highest trauma/affliction severity, nearest proximity | Exclusive reservation: only one medic claims a patient |
| `action_socialize` | Survivor (Partner) | High compatibility, low stress, not sleeping/working | Mutual reservation: temporarily binds both participants |
| `action_resolve_conflict` | Survivor Pair (Disputants) | Highest friction pair | Single mediator claims the conversation |
| `action_teach_skill` | Survivor (Learner) | Receptive survivor with lower skill | Exclusive pairing: one mentor to one student |
| `action_repair_equipment` | Item / Fixture | Lowest durability % below repair threshold | Exclusive reservation on workstation and item |
| `action_cook_food` | Workstation (`room_kitchen`) | Available range burner | Max occupancy bounded by room capacity (2) |
| `action_conduct_research` | Tech Node / Lab Terminal | Active assigned research node | Up to 2 researchers in laboratory concurrently |
| `action_purify_water` | Water Plant | Treatment vat / doser | 1 operator per active treatment cycle |
| `action_stand_watch` | Security Post | Airlock / perimeter sentry spot | 1 guard per watch post |
| `action_rest` | Bunk | Unoccupied bed in bunks | 1 sleeper per bunk |

---

## 2. Deterministic Tie-Breaking & Reservation

- When multiple targets meet the same selection criteria, the target with the lower ordinal ID (e.g. `survivor_01` before `survivor_02`) is selected deterministically.
- Unsuccessful claims immediately cause the action to fail eligibility on the current tick, allowing the survivor to select their next best candidate action rather than freezing.
