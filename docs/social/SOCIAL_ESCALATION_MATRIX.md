# Social Escalation Matrix — Plan 12B

Four state-driven escalation events requiring accumulated grievance, not single random rolls.

| # | Event ID | Escalation Type | Accumulated State Required | Participants | Choices | Effects | Recovery Path |
|---|----------|----------------|---------------------------|-------------|---------|---------|---------------|
| 1 | `escalation_walkout` | Walkout / Refusal of Duty | Grievance > threshold + `friction_walkout_conditions_set` flag + ≥3 unresolved friction events | Grievance holder + leader + duty roster | Accept demands / Reject / Negotiate council | `escalation_walkout` flag, duty roster disruption, leadership pressure | `escalation_walkout_reconsidered` (if conditions met) |
| 2 | `escalation_sabotage` | Sabotage / Deliberate Neglect | Resentment > high threshold + `friction_stolen_keepsake` or similar + low trust | Saboteur + victim | Investigate / Confront / Ignore | `escalation_sabotage_investigated` flag, inventory damage, relationship collapse | Mediation event, relationship repair quest |
| 3 | `escalation_challenge_to_leadership` | Open Leadership Challenge | Leadership legitimacy < threshold + `escalation_walkout` flag + supporter count ≥ 3 | Challenger + leader + supporters | Step down / Defend / Compromise | `escalation_challenge_acknowledged` or `escalation_leadership_stepdown` flag, LeadershipSystem legitimacy delta | New leadership election, reconciliation |
| 4 | `escalation_walkout_reconsidered` | Walkout Reconsideration | `escalation_walkout` flag + `post_walkout_reconsider` conditions + time elapsed ≥ 10 days | Former walker + leader | Return / Stay away / Negotiate terms | `escalation_walkout_reconsidered` + `escalation_walkout_welcomed` flags, duty roster restoration | Full reintegration |

## Escalation Eligibility Rules

1. **No single-roll escalation:** Every escalation requires accumulated state (grievance thresholds, flags, time elapsed)
2. **Leadership integration:** Challenge events feed `LeadershipSystem` legitimacy/challenge mechanics
3. **Duty roster integration:** Walkout events feed `DutyRosterSystem` work refusal
4. **Relationship integration:** All escalations use `SurvivorRelationsSystem` for interpersonal effects
5. **Recovery required:** Every escalation has at least one recovery path

## State Accumulation Chain

```
Minor friction (events 1-10) → Unresolved grievances accumulate
    ↓
Ration conflict (events 1-6) → Resentment builds in RationConflictSystem
    ↓
Inherited walkout threat (friction event 7) → Conditions set
    ↓
Escalation: Walkout (event 1) → Duty disruption
    ↓
Escalation: Challenge (event 3) → Leadership crisis
    ↓
Recovery: Reconsidered (event 4) → Reintegration
```

## Chronology Guards

- Walkout cannot fire before friction events have accumulated
- Challenge cannot fire before walkout has occurred
- Reconsideration cannot fire before walkout + time elapsed
- Sabotage cannot fire before proven hoarding or keepsake theft
- Dead/departed survivors cannot participate in escalations
