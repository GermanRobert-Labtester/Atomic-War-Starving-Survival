# Verdict Radio Frequency Contract

> **Field Definition:** `VerdictRadioEntry.frequency`

---

## 1. Frequency Bands in the Verdict Corpus

The Verdict machine radio infrastructure strictly occupies two discrete frequency bands:

| Frequency | Canonical Role & Band Meaning | Baseline Count | Plan 94 Additions | Total Count |
|---|---|---|---|---|
| `99.0 MHz` | **Authoritative Census Carrier & Machine Registers.** The dedicated military/administrative band carrying automated telemetry, scheduled maintenance, and census summons. | 11 | 16 | 27 |
| `88.5 MHz` | **Civilian / Weather Service Bleed.** The standard civil broadcast band where unsealed transmissions, weather feed corrections, and leaked carrier modulation bleed through. | 2 | 1 | 3 |
| **Total** | | **13** | **17** | **30** |

---

## 2. Invariants
- No random or extraneous frequencies are introduced. 27 of 30 broadcasts operate on the canonical `99.0 MHz` carrier.
- `88.5 MHz` is exclusively used for transmissions that cross the civilian/administrative divide:
  1. `radio_verdict_eden_was_here` (Eden Vale's tube bleed);
  2. `radio_verdict_count_is_open` (Office of Censuses public summons);
  3. `radio_verdict_unscheduled_burst_88` (unscheduled 420ms carrier modulation).
