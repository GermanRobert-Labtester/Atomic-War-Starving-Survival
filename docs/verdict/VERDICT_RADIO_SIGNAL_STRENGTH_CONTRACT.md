# Verdict Radio Signal Strength Contract

> **Field Definition:** `VerdictRadioEntry.signalStrength`

---

## 1. Signal Strength Levels & Roles

| Strength Token | Semantic Role in Verdict Infrastructure | Baseline Count | Plan 94 Additions | Total Count |
|---|---|---|---|---|
| `S1` | Faint carrier tone, unboosted sensor baseline, or distant relay mast | 3 | 4 | 7 |
| `S2` | Standard telemetry burst, local automated monitoring well, or tape playback | 6 | 7 | 13 |
| `S3` | High-power administrative census broadcast or primary substation grid test | 4 | 5 | 9 |
| `S4` | Master emergency override carrier taking over sector bandwidth | 0 | 1 | 1 |
| `S5` | Reserved for direct facility console interlock | 0 | 0 | 0 |
| **Total** | | **13** | **17** | **30** |

---

## 2. Invariant Rules
- Signal strength represents physical transmission power and distance, **not narrative importance**.
- Critical investigation clues (e.g. `radio_verdict_strata_density_drift`, `radio_verdict_geophone_offset_recal`) use `S1`, forcing players to attend to faint telemetry.
- The single emergency broadcast (`radio_verdict_carrier_override_standby`) uses `S4` to reflect maximum automated transmitter output upon register closure.
