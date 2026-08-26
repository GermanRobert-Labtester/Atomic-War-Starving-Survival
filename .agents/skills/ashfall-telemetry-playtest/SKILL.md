---
name: ashfall-telemetry-playtest
description: Instruments seeded headless playthroughs, harvesting need/morale/radiation/economy/trade/health KPIs to CSV and first-hour funnel diagnostics. Use when tuning difficulty, auditing TutorialPanel, or before balancing PRs.
---

# ASHFALL Telemetry Playtest

## ROLE
Balance opinions without numbers are vibes. You turn seeded headless runs into CSV telemetry and funnel reports that prove whether the first hour teaches what it demands.

Complements `ashfall-balance-sim` (sweeps knobs) with instrumented playthroughs; overlaps `ashfall-tutorial-review` (teach-vs-demand) with data.

## RULES
1. Deterministic only — `ISeededRng` (xorshift64*), never `System.Random`/`Guid.NewGuid()`, same seed ⇒ identical CSV.
2. `dotnet` + `godot --headless` only; no manual play required for gate.
3. Never mutate `StreamingAssets/Data/` in this skill — read KPIs, propose tuning elsewhere.

## WORKFLOW
### PHASE 1 — Scenario
Select 1–3 scenarios: fresh shelter Day 0–7 (first-hour funnel), mid-game Day 30 starvation spiral, late Day 180 radiation load. Fix seed list (e.g., 42, 1337, 9001).

### PHASE 2 — Harness
Reuse headless demo/selftest entry points or xUnit harness that ticks `GameBootstrap` day/hour registries, `NeedsSystem`, `RadiationSystem`, `Economy`, trade stances, cohorts. Emit per-day row: `day, hunger,thirst,fatigue,warmth,morale,radiation,dose,health,ledger_debt,inventory_bytes`.

### PHASE 3 — Run
- `dotnet test --filter Telemetry` or `godot --headless --path . -- --telemetry-csv out/telemetry_seed42.csv --days 30 --seed 42`
- Collect state hashes at checkpoints for `ashfall-seed-replay` parity.

### PHASE 4 — Analyze
- Funnel: Day 0–2 resource exhaustion vs tutorial hints shown; softlock detectors (no food + no trade + no work).
- Curves: days-to-death distribution, debt death-spiral threshold, dose milestone timing.

## OUTPUT
`docs/telemetry/TELEMETRY_<scenario>.md` + `out/telemetry_*.csv` — seed manifest, KPI table, survival curves, softlock list, tuning proposals with seed-reproducible evidence.

## QUALITY GATE
- Every claim reproducible from stated seed + commit SHA; 0 nondeterministic calls in harness (`grep -r System.Random` 0).
