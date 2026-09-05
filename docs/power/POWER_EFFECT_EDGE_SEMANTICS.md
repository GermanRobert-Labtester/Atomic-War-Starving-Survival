# Power Effect Edge Semantics

> **Edge Semantics:** Formal classification of trigger boundaries, idempotence, and recovery behavior.

---

## 1. Effect Classes

1. **Level Gates (State-Dependent Availability):**
   - *Definition:* True while power is absent, immediately false when power returns.
   - *Applies to:* `fx_filtration_off`, `fx_clinic_off`, `fx_water_pressure_drop`, `fx_grow_lights_off`, `fx_foundry_standstill`, `fx_lighting_dim`, `fx_workshop_unpowered`, `fx_kitchen_cold`, `fx_radio_static`, `fx_laboratory_offline`, `fx_armory_lockdown`, `fx_mess_hall_dark`, `fx_dormitory_cold`, `fx_water_contamination`, `fx_surveillance_blind`, `fx_airlock_decon_disabled`, `fx_quarantine_breach`.
   - *Behavior:* Downstream systems check `IsRoomPowered(roomId)` at operation execution time.
   - *Idempotence:* Calling the check repeatedly produces identical results without cumulative state mutation.

2. **Falling-Edge Events (Transition to Unpowered):**
   - *Definition:* Triggered once upon transitioning from powered (`true`) to unpowered (`false`).
   - *Signal:* `PowerGridSystem.OnPowerChanged` emitting `PowerGridEventKind.BreakerToggled` or `PowerGridEventKind.Tripped`.
   - *Behavior:* Host raises an alert or logs a journal entry. No duplicate alerts fire while the room stays dark.

3. **Rising-Edge Events (Transition to Powered):**
   - *Definition:* Triggered once upon transitioning from unpowered (`false`) to powered (`true`).
   - *Signal:* `PowerGridSystem.OnPowerChanged`.
   - *Behavior:* Host clears unpowered warning icons and resumes paused background work.

4. **Delayed Consequences (Time-Accumulated Outage):**
   - *Definition:* An outage initiates a grace timer inside the downstream authority.
   - *Applies to:* `fx_cold_storage_spoilage` (refrigerated stores).
   - *Behavior:* Outage starts a decay timer in the storage system. If power is restored before the timer expires, the timer resets without food loss. If the timer elapses, irreversible spoilage occurs under storage system authority.

---

## 2. Strict Anti-Spam & Conservation Rules

- **No Per-Tick Penalty Spam:** An unpowered room must never apply recurring morale penalties, repeated journal spam, or instant item deletion every simulation tick.
- **Irreversible Actions Stay Committed:** Completed consequences (e.g. food that spoiled after a multi-day blackout) are not resurrected when power returns.
- **Reversible Gates Re-Open Cleanly:** Stations, lamps, and tools resume operation immediately upon power restoration.
