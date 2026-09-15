# PLAN 122 — SOFC Power Balance Report (Phase 11)

**Source:** `--plans-122-125-balance-soak` (fixed master seed 20260913, per-day `CampaignRngManager` forks, 180-day characterization, plan §11.1)
**Status:** ACCEPTED — characterization complete, design invariants hold.

## Observed 180-day run (stack Mk I, skill 60, clean fuel with dirty weeks 15–17 / 60–62 / 120–122)

| Metric | Value |
|---|---|
| Total fuel drawn | 4,149.8 units |
| Total energy dispatched | 2,396 kWh |
| Fuel intensity | **1.732 units/kWh** |
| Total waste heat recovered (CHP) | 1,598 kW·ticks |
| Fault-recovery maintenance events | 0 |
| Startup (preheat) events | 1 |
| Stack health | start 10000 → min 7440 → end 7440 |
| 30-day health curve | 9486, 9080, 8658, 8252, 7830, 7440 |

## Reading

- **Baseload efficiency is strong but not dominant:** 1.73 units/kWh beats the
  documented legacy generator baseline (0.30 kW-per-fuel-unit ⇒ ~3.33 u/kWh) by
  roughly 2× — inside the design intent (no triple-economy multiplier, no
  generator-killer). The `sofc_180_no_dominance` gate enforces intensity stays
  above 0.02 u/kWh.
- **Degradation is infrastructure-paced:** ~26 bp/day average under mixed
  duty; the stack reaches "Good" (7440 bp) after half a year including three
  dirty-fuel weeks. A full rebuild cycle lands comfortably late — the plant
  feels like a facility, not a consumable (plan §4.7).
- **Waste heat is a real CHP stream:** ~8.9 kW·ticks/day of allocatable heat —
  material for the kitchen/thermal loop without double-counting electrical
  efficiency.
- **Diesel stays relevant:** SOFC cannot surge above its clamped rating, takes
  4+ ticks to start (tested), and pays thermal-cycle penalties on restarts —
  the generator ladder remains meaningful (plan §4.10; dominance gate).
- **Zero maintenance events occurred** in this run: the fault-recovery loop
  never fired with seed 20260913 under 3×3 dirty days at 350 bp risk. Fault
  handling is covered by the 75-day harness and unit suites; the ops loop
  exists and is exercised there.

## Gates

`sofc_180_output_bounded`, `sofc_180_no_dominance`, `sofc_180_waste_heat_recovered`,
`sofc_180_degradation_bounded`, `sofc_180_ops_stable` — ALL PASS.

## Follow-ups

- Watch stack economics once rebuild items enter crafting consumers (Phase 12
  content-utilization pass).
- The 75-day harness (Phase 10) exercises the fault→maintain→restart path.
