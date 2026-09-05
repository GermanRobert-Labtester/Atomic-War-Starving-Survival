# Plan 71 Save Compatibility Contract

> **Persistence Contract:** Integrity, migration, and backward-compatibility rules for `PowerGridSave` and `PowerGridState`.

---

## 1. Legacy Save Preservation (6-Room Saves)

- Existing saves generated under the 6-room catalog store a `PowerGridSave` payload with a 6-room list and state (`ClosedBreakers`, `TrippedRooms`, `Priorities`).
- When restored via `PowerGridState.RestoreInto(state, rooms)`:
  - `NormalizeAndValidate(rooms)` validates existing state against the new 18-room list.
  - Any previously opened breakers or tripped rooms for the original 6 rooms are faithfully preserved.
  - Newly added rooms that did not exist in the save file start in the default healthy state: closed breakers, untripped, default priority.
  - Derived facts (`TotalDrawWatts`, `NetWatts`, `IsBrownout`) recalculate cleanly from the authoritative state.
  - Checksum validation in `PowerGridSaveCodec` continues to enforce full wire integrity.

---

## 2. Derived vs. Persisted State

| Property | Persistence Rule | Rationale |
|---|---|---|
| `GenerationWatts` | Persisted in `PowerGridState` | Tracks baseline dynamo output and generator upgrades/damage |
| `FuelUnits` | Persisted in `PowerGridState` | Tracks active fuel consumption and refueling events |
| `BatteryReserveWh` | Persisted in `PowerGridState` | Tracks exact stored electrical charge |
| `BatteryCapacityWh` | Persisted in `PowerGridState` | Tracks total accumulator battery capacity |
| `ClosedBreakers` | Persisted in `PowerGridState` | Remembers player-opened breaker switches |
| `TrippedRooms` | Persisted in `PowerGridState` | Remembers overloaded circuits requiring reset |
| `Priorities` | Persisted in `PowerGridState` | Remembers user-modified priority overrides |
| `DrawWatts` | **Derived from Catalog** | Authoritative in `power_grid.json`; never duplicated into runtime state |
| `DefaultPriority` | **Derived from Catalog** | Authoritative in `power_grid.json`; used when no user override exists |
| `FailureEffectId` | **Derived from Catalog** | Authoritative in `power_grid.json` |
| `TotalDrawWatts` | **Derived at Runtime** | Dynamically computed from active rooms |
| `IsBrownout` | **Derived at Runtime** | Evaluated deterministically from total draw vs. generation + battery |

---

## 3. Campaign Envelope Integration

- `PowerGridSaveStore` delegates persistence to the generic `SaveStore<PowerGridSave>` façade via `SaveStoreHub.FromCodec`.
- Captures are atomic (temp-file write and atomic rename).
- Serialization produces byte-identical checksummed envelopes compatible with `CampaignEnvelopeBuilder`.
