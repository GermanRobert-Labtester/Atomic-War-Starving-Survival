# Starting Cohort Balance Simulation

## Scope

This is a deterministic first-30-day heuristic for Plan 138's six fresh
campaign cohorts. It compares authored starting pressure and canonical
profession coverage. It is not a second gameplay authority and does not alter
campaign data.

## Fixed scenario

| Knob | Value |
|---|---:|
| Seed | `138` |
| Horizon | 30 days |
| Hunger drift | 2.5/day |
| Thirst drift | 3.0/day |
| Warmth drift | 1.5/day |
| Critical hunger/thirst threshold | 80 |
| Critical health stress | 1.0/day |
| Cold health stress | 0.05 per point below warmth 20 |
| Stress sample | `0.98 + SeededRng(138).NextFloat() * 0.04` |

The resulting deterministic stress sample is `1.0165011472`. The harness uses
`Ashfall.Core.SeededRng`, stable authored member order, and ordinal IDs.

## Results

| Profile | Min health, day 30 | Min warmth, day 30 | First hunger critical | First thirst critical | Initial dose | Primary coverage |
|---|---:|---:|---:|---:|---:|---|
| Standard Holdfast | 66 | 29.26 | 18 | 17 | 60 | medical 2, field 1 |
| Triage Ward | 68 | 36.26 | 19 | 15 | 46 | medical 2, repair 1 |
| Repair Crew | 67 | 34.26 | 19 | 14 | 44 | repair 3 |
| Growers & Stewards | 66 | 36.26 | 20 | 15 | 36 | food 3 |
| Convoy Remnant | 62 | 26.26 | 15 | 13 | 86 | field 3 |
| Civilian Improvisers | 66 | 34.26 | 19 | 15 | 41 | social 2 |

## Findings

- Every profile remains above zero health in this conservative, no-intervention
  heuristic through day 30.
- Convoy Remnant is the pressure edge: it reaches thirst and hunger critical
  first and has the lowest health floor.
- Triage Ward has the strongest health floor but reaches thirst critical sooner
  than Standard, so medical coverage does not erase water pressure.
- Repair, Growers, Convoy, and Civilian profiles trade Standard's medical or
  perimeter balance for a clear operational specialty.
- No alternate strictly dominates Standard across health, warmth, critical-day,
  and canonical coverage dimensions.

## Verification

The assertions live in
`Ashfall.Core.Tests/StartingCohortBalanceSimulationTests.cs` and pin:

- identical fingerprints for repeated seed `138` runs;
- finite first-30-day metrics for all six profiles;
- canonical role-coverage counts;
- no alternate dominating Standard across all tracked dimensions.

This simulation does not claim a Plan 134 origin/loadout matrix. Supplies and
origins remain separate from survivor cohort authority.
