# Plan 71 Regression Matrix

> **Verification Matrix:** 20 test cases covering catalog validation, electrical physics, load shedding, failure effect edge semantics, persistence, and incident integration.

---

| # | Test Scenario | Target Subsystem | Expected Outcome | Verification Status |
|---|---|---|---|---|
| 1 | **Catalog Schema Version** | `power_grid.json` | `schema_version == 1` | Verified |
| 2 | **Exact Room Count** | `power_grid.json` | Exactly 18 rooms loaded | Verified |
| 3 | **Baseline 6 Preserved** | `power_grid.json` | IDs 0..5 match original baseline records byte-for-byte | Verified |
| 4 | **12 New Rooms Validated** | `power_grid.json` | IDs 6..17 resolve to Plan 41 / canonical services | Verified |
| 5 | **Zero Duplicate IDs** | `power_grid.json` | 18 unique snake_case IDs with prefix `room_` | Verified |
| 6 | **Valid Wattage Bounds** | `power_grid.json` | Draw values range between 30 W and 300 W (all > 0) | Verified |
| 7 | **Valid Priority Enums** | `power_grid.json` | All priorities are `"critical"`, `"standard"`, or `"low"` | Verified |
| 8 | **Total Load Calculation** | `PowerGridSystem` | Baseline total load = 2,230 W across all 18 rooms | Verified |
| 9 | **Critical Core Under Cap** | `PowerGridSystem` | Critical core = 760 W <= 800 W baseline generation | Verified |
| 10 | **Breaker Toggle Idempotence** | `PowerGridSystem` | Opening breaker removes room draw; re-closing restores draw | Verified |
| 11 | **Disabled Priority Sched** | `PowerGridSystem` | Setting priority to `Disabled` excludes room from total draw | Verified |
| 12 | **Brownout Trigger Threshold** | `PowerGridSystem` | `IsBrownout` true only when demand > gen AND battery <= 0 | Verified |
| 13 | **Battery Draining Dynamics** | `PowerGridSystem` | Deficit drains battery reserve without charging up during brownout | Verified |
| 14 | **Deterministic Day Tick** | `PowerGridSystem` | Same seed produces identical fuel burn and battery end values | Verified |
| 15 | **Overload Breaker Trip** | `PowerGridSystem` | Brownout >= 4 hours trips breakers with 10% seed-controlled roll | Verified |
| 16 | **Snapshot Integrity** | `PowerGridSystem` | `Snapshot()` accurately reports generation, draw, fuel, battery, brownout | Verified |
| 17 | **Event Dispatch Integrity** | `PowerGridSystem` | `OnPowerChanged` fires for breaker toggle and priority change | Verified |
| 18 | **Save Codec Round-Trip** | `PowerGridSaveCodec` | Encode/Decode produces identical state and valid checksum | Verified |
| 19 | **Tampered Save Rejection** | `PowerGridSaveCodec` | Modified payload fails checksum verification with exception | Verified |
| 20 | **Old Save Compatibility** | `PowerGridState` | Loading a 6-room save into an 18-room system retains valid state | Verified |
