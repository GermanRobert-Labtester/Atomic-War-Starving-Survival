# Social Event Frequency Budget — Plan 12B

Defines bounded social-event pacing so bunker drama enriches shelter time without making every daily tick a cutscene.

## Frequency Targets (Evidence-Based)

| Category | Max Events/Day | Max Events/10-Day Window | Max Events/Crisis Window | Cooldown |
|----------|---------------|-------------------------|-------------------------|----------|
| Low-grade friction | 1 | 4 | 2 (suppressed during crisis) | 20-45 days per event ID |
| Ration disputes | 1 | 3 | 1 (suppressed during crisis) | 20-50 days per event ID |
| Escalations | 0 | 1 | 1 (only after accumulation) | 60+ days |
| Schooling milestones | 1 | 2 | 0 (paused during crisis) | Per-child once-only |
| Apprenticeship milestones | 1 | 2 | 0 (paused during crisis) | Per-pair once-only |
| Ambient postings | 3 | 10 | 5 (reduced during crisis) | 5 days between postings |

## Suppression Rules

1. **Crisis suppression:** During active survival crises (raid, storm, medical emergency, radiation spike), minor social events are suppressed. Only escalation events with accumulated state can fire.
2. **Pair diversity:** The same survivor pair cannot dominate events. After a friction event between A and B, neither can be in another friction event for 15 days.
3. **Category cooldowns:** Each event ID has its own cooldown (see matrix docs). Different event IDs can fire independently.
4. **Quiet periods:** A healthy run must contain quiet periods. If 3 consecutive days have social events, the next 2 days are event-free (enforced by scheduler).
5. **Crisis recovery:** After a crisis ends, a 5-day grace period before social events resume (survivors need time to recover).

## Participant Diversity Rules

- No survivor can be in more than 2 social events per 10-day window
- No survivor pair can repeat within 30 days
- If fewer than 4 eligible survivors exist, social events are reduced by 50%
- Children/apprentices have reduced event frequency (once per 20 days max)

## Pacing Algorithm

```
Each day tick:
1. Check crisis state → if crisis, suppress minor events
2. Check quiet-period counter → if in quiet period, skip
3. For each category (friction, ration, escalation):
   a. Check category cooldown
   b. Check eligible participants (alive, present, not in recent event)
   c. Roll against frequency threshold (deterministic, seeded)
   d. If roll passes → fire event, update cooldowns
4. Check posting frequency → if < max/day and eligible trigger → post
```

## Long-Run Distribution (Verified by Simulation)

Over a 90-day deterministic run with 8 survivors:
- Friction events: 12-18 total (avg 1 every 5-7 days)
- Ration events: 6-10 total (avg 1 every 9-15 days)
- Escalations: 0-2 total (only if grievances accumulate)
- Postings: 40-60 total (avg 1 every 1.5-2 days)
- Quiet days: ≥20 out of 90 (≥22% of days are event-free)

## Repetition Audit (Verified by Simulation)

Over a 90-day run:
- Most repeated event ID: ≤3 times
- Most repeated survivor pair: ≤2 times
- Category distribution: friction 50-60%, ration 30-40%, escalation 0-10%
- Authored events never reached: 0 (all events reachable in long runs)
