# ECOLOGY_BALANCE_AUDIT.md — Plan 28 (as-of this pass)

**Status: guardrail audit from the shipped factors + existing clamps; the 120–180-day
parameter sweep (28BA) is deferred to `ashfall-balance-sim` with the harness below.**

## Bounds in force (all enforced in code, not aspiration)

| Pressure | Mechanism | Hard bound |
|---|---|---|
| Catch rate | `BaseCatchChance × density × skill` | clamp [0.05, 0.95] — no guaranteed catch |
| Density composition | `0.5 + pop×0.1` × seasonal factor | clamp [0.4, 1.5] |
| Hunger pacing | ±30% around authored 0.05/day | clamp [0.6, 1.5] |
| Population growth | +1/day toward 2× seed, 3-day breathing room | ceiling 2× seed |
| Collapse | −1/day above starvation 0.7 | floor at 0 (never negative) |
| Abundance factors | per archetype/window | clamp [0.2, 1.5] |
| Fish run yield | water-bound pair only, seasonal window (Thaw 1.5 → High Cold 0.6) | 2 water sectors; ice empties the run |
| Market demand delta | ±0.02/day max | market demand clamp (existing authority) |
| Notices | sector-change diff | 3 wildlife reports/day |

## Exploit analysis (no-guaranteed-food)

- Best-case window: carp run in Thaw — abundance 1.5 × density cap 1.5 → catch chance still
  ≤ 0.95 and yield scales with the pack's own 2× ceiling; the pack thins under pressure and
  migrates when starved. **Not infinite:** heavy exploitation drains the sector and the run
  moves or collapses (existing starvation rules do the work the old plan wanted a pressure
  tracker for — 28P partially live via starvation→movement).
- Worst-case: Deep Freeze — runners at 0.2, herds 0.6, flocks 0.4; the scarcity chain lifts
  preserved-food demand (economy pressure, not starvation lock: Resident predators and
  trapping floor 0.05 remain).
- Long-horizon sanity is inherited from the live gate: "routes remain plannable after 360
  days (never permanently unwinnable)" (selftest step 18).

## Flagged follow-ups (for the deferred balance pass)

1. Run the 28BA/28BB harness (120–180 day seeded sims: no-exploit vs. heavy-exploit vs.
   missed-window) before tuning any factor above 1.3 / below 0.7.
2. Track per-season trapping yield distribution; flag any season > 2× the annual mean yield.
3. Cascade stress: verify no more than one encounter modifier chains from prey collapse.
