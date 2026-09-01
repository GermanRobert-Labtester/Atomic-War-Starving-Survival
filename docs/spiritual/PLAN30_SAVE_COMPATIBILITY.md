# Save Compatibility & Determinism Contract

---

## 1. Save Compatibility

- **State Container:** `SpiritualCoordinatorSaveState` (`MourningArcs`, `RitualLastPerformedDay`).
- **No Meter Creep:** Zero persisted piety, faith, or devotion counters.
- **Backward Compatibility:** Pre-Plan 30 saves safely load with empty coordinator state. Ongoing campaigns gracefully initialize mourning records on next death.

---

## 2. Determinism Invariants

- All mourning stage calculations depend strictly on `currentDay - DeathDay` integer differences.
- All dictionary enumerations in DTO captures use ordinal key sorting where ordering is serialized.
- Ritual cooldowns use pure integer day timestamps without wall-clock dependencies.
