# Ration-Conflict Event Matrix — Plan 12B

Six ration-conflict events distinguishing objective shortage, perceived unfairness, proven hoarding, and policy disagreement.

| # | Event ID | Type | Trigger | Participants | Choices | Effects | Cooldown |
|---|----------|------|---------|-------------|---------|---------|----------|
| 1 | `ration_uneven_scoop_third_day` | Perceived unfairness | Ration allocation variance > threshold | Server + recipients | Rotate scooper / Acknowledge / Ignore | `ration_scoop_rotated` or `ration_double_lead_ack` flag, resentment delta | 20 days |
| 2 | `ration_hoarded_tins_first_sign` | Proven hoarding | Inventory discrepancy detected | Hoarder + discoverer | Confront / Report / Share | `ration_hoarded_tins_first_sign` or `ration_tin_shared_response` flag, inventory correction | 40 days |
| 3 | `ration_feast_day_demand` | Policy disagreement | Special occasion + ration surplus demand | Requester + leader | Honor / Defer / Deny | `ration_feast_honored` or `ration_feast_deferred` flag, morale delta | 60 days |
| 4 | `ration_sick_gets_more_dispute` | Policy disagreement | Sick survivor + healthy survivor disagree on extra ration | Sick + healthy + medic | Extra for sick / Equal for all / Audit | `ration_medic_extra_canon` or `ration_medic_extra_audit` flag, needs delta | 30 days |
| 5 | `ration_theft_with_evidence` | Proven hoarding + evidence | Stolen ration evidence + prior observation | Thief + leader | Punish / Investigate / Name second bunk | `ration_theft_second_bunk_named` flag, inventory correction, leadership pressure | 50 days |
| 6 | `ration_resentment_kindling` | Accumulated grievance | RationConflictSystem resentment > threshold | Resentful + target | Mediate / Ignore / Reassign | `ration_resentment_mediated` or `ration_resentment_allowed` flag, grievance relief | 35 days |

## RationConflictSystem Integration

All events read from `RationConflictSystem` state:
- **Resentment:** Built by unequal allocations, decays when fair
- **Fairness:** Computed from allocation variance
- **OnResentmentEvent:** Fires when resentment crosses threshold → eligible for events 5, 6
- **Inventory correction:** Uses legitimate inventory APIs, not narrative-side mutation

## Distinction Matrix

| Type | Objective Shortage | Perceived Unfairness | Proven Hoarding | Policy Disagreement |
|------|-------------------|---------------------|-----------------|---------------------|
| Events | — | 1 | 2, 5 | 3, 4 |
| Resolution | N/A | Procedural change | Inventory correction | Policy decision |
| Authority | — | RationConflictSystem | Inventory + RationConflictSystem | LeadershipSystem |
