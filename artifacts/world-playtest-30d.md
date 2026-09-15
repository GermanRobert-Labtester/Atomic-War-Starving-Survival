# ASHFALL evolving-world 30-day proof

- Source commit: `afc419d941813d2766a27666b25df1702a7afa2a`
- Master seed: `424242`
- Snapshot count: `30`
- Day owner: `world_evolution`

## Target bands

| Measure | Band / rule |
|---|---|
| global wildlife ratio | 0.00 to 2.00; day-30 must remain above 0 (not silent) |
| location contamination / loot depletion | 0.00 to 1.00; no NaN/Infinity or negative values |
| expedition encounter multiplier | 0.50 to 2.00; production component remains bounded |
| market scarcity proxy | 0.25 to 4.00; canonical MarketSystem clamp |

## Trajectory summary

- First migration day: `10`
- First degradation transition: `11`
- Wildlife density min/max: `0 / 26`
- Notable density delta: `57`
- Encounter multiplier min/max: `0.95 / 1.15`
- Canned-food scarcity min/max: `0.636 / 1.545`
- Surfaced events: briefing `129`, journal `2`, radio `61`
- Trapping density reads: `30`; encounter multiplier reads: `11`
- Dead seeds: `0`

## Determinism and persistence

- Same-seed byte equality: `True`
- Midpoint save/load byte equality: `True`
- Different-seed divergence: `True`
- Duplicate narrative events after restore: `False`

## Influence map

| Producer | Value | Consumer | Read cadence | Player-visible consequence |
|---|---|---|---|---|
| wildlife migration | sector pack population / global ratio | wildlife trapping | once per day, phase 2 | prey availability and harvest pressure |
| location evolution | threats, contamination, degradation tier | ExpeditionSystem encounter multiplier | once per active expedition tick | route pressure |
| wildlife ratio | ratio-based demand delta | MarketSystem | once after world tick | canned-food scarcity |
| world evolution events | migration / expedition / degradation events | briefing, journal, radio projection | daily snapshot | surface feedback |

## Checks

- PASS: production EvolvingWorldDayOwner advances the fixed 30-day window
- PASS: 30 daily snapshots emitted — count=30
- PASS: wildlife migration is visible — events=61
- PASS: location degradation tier changes — transitions=1
- PASS: wildlife density moves meaningfully — delta=57
- PASS: encounter multiplier moves meaningfully — movement=0.2
- PASS: scarcity proxy moves — movement=0.304
- PASS: world changes surface through briefing/journal/radio — briefing=129, journal=2, radio=61
- PASS: world trajectory stays inside documented bands
- PASS: ruined location states remain monotonic
- PASS: daily snapshots contain no duplicate location or wildlife records
- PASS: trapping density is consumed once per campaign day — ticks=30, reads=30
- PASS: expedition encounter multiplier is consumed once per evaluation — reads=11, evaluations=11
- PASS: every world-evolution seed participates in the live tick/read pipeline — dead=0
- PASS: same-seed snapshot artifacts are byte-identical — seed=424242
- PASS: different seed produces a divergent trajectory — seed=2026
- PASS: midpoint save/load preserves the full post-restore trajectory — save_day=15
- PASS: restored world keeps seed state and narrative de-duplication — dead=0, duplicate_events=False

## Tuning decision

No tuning applied. The playtest exercises the named AshfallMmFor / ContaminationHazardGain knobs as-is; change either only in a separate trajectory-pinning commit if future evidence shows inert or runaway behavior.
