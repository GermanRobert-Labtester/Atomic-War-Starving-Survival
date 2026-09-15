# PLAN 124 — Diamond Tool Economy (Phase 11)

**Source:** `--plans-122-125-balance-soak` 120-day tool-economy soak (2 industrial ticks/day, skill 65, refined feed + superalloy substrate, plan §11.3)
**Status:** ACCEPTED — specialist material with real economics, not maintenance elimination.

## Observed 120-day run (reactor Mk I, maintenance-on-fault loop)

| Metric | Value |
|---|---|
| Batches started | 10 |
| Batches completed | 9 (1 in flight at horizon) |
| Rejected | 0 |
| Consumed | 9 |
| Grade distribution | master 2, utility 4, industrial 3 |
| Equipment bounds | magnetron/chamber clamped in [0, 10000] throughout |

## Reading

- **Cadence:** with the real catalog reactor (1200 bp base growth rate), one
  batch spans roughly 2 weeks of double-ticks — a slow industrial job that
  competes for the production cadence (plan §14: only active batches tick).
- **Grade spread is real:** 2 master (certified through the metrology seam
  before release), 3 industrial, 4 utility — quality inputs + skill 65 still
  yields utility-grade variance, so defects and conformity matter (plan §6.5).
- **Zero rejections in this window:** the failure economy lives in defect
  rolls (seeded, bounded [100, 3000] bp/tick) and plasma-collapse faults, both
  exercised in unit suites; longer horizons will surface them.
- **Wear economy:** industrial inserts carry 4000 bp wear factor = **2.5×
  service interval** for registered consumers (deep-excavation cutter and
  precision lathe), inside the 2×–3.5× design band. Master grade = 2500 bp
  ≈ 4× — high investment, and certification gates it. **No zero-wear path
  exists** (gate + unit invariant).
- **Equipment wears and is maintained:** magnetron/chamber decay per tick with
  maintenance restoring bounded amounts; neither truth is duplicated with the
  item-condition owner (the reactor is the machine, not an inventory item).

## Gates

`diamond_120_started_batches`, `diamond_120_equipment_bounded`,
`diamond_120_no_zero_wear`, `diamond_120_grades_bounded` — ALL PASS.

## Follow-ups

- Cutter-replacement-avoidance accounting in `ExcavationSystem` downtime terms
  lands when the excavation consumer consumes inserts through the workshop
  (Phase 7 typed seam exists; scenario-level effect tracked in the 75-day
  harness).
