# Plan 94 — Verdict Radio Broadcasts Baseline & Forensics

> **Catalog Authority:** `Assets/StreamingAssets/Data/verdict_radio.json`
> **Core Loader & Runtime:** `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs`
> **Persistence Authority:** `Assets/Ashfall.Core/Verdict/VerdictSave.cs`
> **Unified Radio Integration:** `Assets/Ashfall.Core/Radio/RadioBroadcastCatalog.cs`

---

## 1. Verified Baseline Reconnaissance

### 1.1 Catalog State Prior to Plan 94
- `verdict_radio.json` authored with `schema_version: 1` and exactly **13 broadcasts**.
- Initial baseline broadcasts:
  1. `radio_verdict_meter_reads_1142` (D210, 99.0 MHz, telemetry, S1)
  2. `radio_verdict_fuse_serviced` (D211, 99.0 MHz, maintenance, S2)
  3. `radio_verdict_wing_sleeps` (D242, 99.0 MHz, telemetry, S2)
  4. `radio_verdict_off_count_assessed` (D240, 99.0 MHz, call, S3)
  5. `radio_verdict_eden_was_here` (D245, 88.5 MHz, witness, S2)
  6. `radio_verdict_count_is_open` (D240, 88.5 MHz, call, S3)
  7. `radio_verdict_clock_disagrees` (D213, 99.0 MHz, telemetry, S1)
  8. `radio_verdict_geophone_taps` (D218, 99.0 MHz, readings, S2)
  9. `radio_verdict_valve_accessed_36` (D250, 99.0 MHz, maintenance, S2)
  10. `radio_verdict_reels_matter` (D255, 99.0 MHz, count, S2)
  11. `radio_verdict_presentation_names_holders` (D260, 99.0 MHz, call, S3)
  12. `radio_verdict_carrier_on_window` (D210, 99.0 MHz, carrier, S1)
  13. `radio_verdict_reckoning_call` (D241, 99.0 MHz, call, S3)

### 1.2 Runtime Gating & Trigger Semantics
Documented in `VerdictRadioSystem.cs`:
- **Carrier Window:** Opens on `CarrierOpenDay = 210`. No broadcast fires before Day 210.
- **Reckoning Phase Gate:** Broadcasts fire only when `ReckoningPhase >= ReckoningPhase.Culpable`.
- **Day Gate:** Broadcast fires when `day >= e.dayTrigger` (available from the trigger day onward).
- **One-Shot Execution:** Fired broadcasts record their ID in `_firedIds`. Subsequent polls do not re-fire.
- **Persistence:** Fired IDs are stored in `VerdictRadioState.firedIds` inside `VerdictSave.radio`.

### 1.3 Expansion Target
- Author **17 new machine-register broadcasts**, expanding the total from 13 to **30**.
- Requested distribution:
  - 5 telemetry
  - 4 maintenance
  - 3 census
  - 2 calibration
  - 2 anomaly
  - 1 emergency
- Spans long campaign investigation timeline: Day 268 to Day 360.
