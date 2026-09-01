# Friction Event Matrix — Plan 12B

Ten bunk-friction events with participant eligibility, state conditions, mediation choices, system deltas, cooldowns, and callback potential.

| # | Event ID | Trigger | Participants | Choices | Effects | Cooldown | Callback |
|---|----------|---------|-------------|---------|---------|----------|----------|
| 1 | `friction_snoring_feud` | Roommate compatibility < threshold | 2 roommates | Mediate / Swap bunks / Ignore | Affinity ±, sleep penalty, `friction_snoring_feud_mediated` or `friction_snoring_feud_swapped` flag | 30 days | `friction_stolen_keepsake` escalation |
| 2 | `friction_stolen_keepsake` | Personal item missing + low trust | Accused + accuser | Confront / Let go / Search | Trust ±, resentment, `friction_stolen_keepsake` or `friction_stolen_keepsake_left` flag | 45 days | `friction_stolen_keepsake_after` callback |
| 3 | `friction_forbidden_radio_listening` | After-hours radio use detected | Listener + complainant | Report / Join / Confiscate | `friction_forbidden_radio_listening` flag, relationship delta | 20 days | — |
| 4 | `friction_work_shirker_accusation` | Duty missed + witness | Shirk-er + witness + leader | Punish / Investigate / Dismiss | `friction_work_shirker_accusation` or `friction_shirker_amended` flag, leadership pressure | 30 days | — |
| 5 | `friction_stealing_sleep_rotation` | Noise complaint during rest cycle | Noisy + sleeper | Rotate bunks / Ear plugs / Confront | `friction_visitor_assigned` or `friction_second_bunk_permitted` flag | 25 days | — |
| 6 | `friction_ash_sermon_quiet_hours` | Nihilist preaching during quiet hours | Preacher + sleepers | Ban / Allow / Redirect | `friction_ash_cant_banned` or `friction_ash_cant_admitted` flag, belief friction | 40 days | — |
| 7 | `friction_inherited_walkout_threat` | Accumulated grievances > threshold | Grievance holder + leader | Council / Concede / Ignore | `friction_walkout_council_held` flag, leadership pressure | 60 days | `escalation_walkout` |
| 8 | `friction_two_sides_of_chalk` | Disputed boundary/marker | 2 survivors | Compromise / Enforce / Erase | `friction_chalk_pen_compromise` or `friction_chalk_dispute_pending` flag | 20 days | — |
| 9 | `friction_ration_observation_overheard` | Survivor notices ration irregularity | Observer + observed | Discreet / Open | `friction_ration_observation_discreet` or `friction_ration_observation_open` flag | 35 days | `ration_theft_with_evidence` |
| 10 | `friction_ration_stolen_dry_rations` | Dry rations missing from stores | Thief (if caught) + leader | Punish / Investigate / Replace | `faction_stores_locked` flag, inventory correction | 50 days | — |

## Belief Set Interactions

| Belief Pair | Friction Level | Effect |
|-------------|---------------|--------|
| ration_collectivist ↔ every_soul_alone | HIGH | Sleep penalty, frequent disputes |
| faith_in_rebuild ↔ ash_nihilist | HIGH | Ideological arguments, morale penalty |
| ration_collectivist ↔ faith_in_rebuild | LOW | Reluctant respect, conditional cooperation |
| every_soul_alone ↔ ash_nihilist | MEDIUM | Debate, mutual suspicion |
| Same belief ↔ Same belief | SYNERGY | Compatibility > 1.0, morale bonus |

## Mediation Consequence Budget

- Low-grade events: ±5 affinity, ±2 morale, small grievance delta
- Medium events: ±10 affinity, ±5 morale, temporary roster implications
- High events: ±15 affinity, ±8 morale, leadership pressure
- No event exceeds a major survival crisis in consequence magnitude
