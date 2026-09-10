# Plan 143 event inventory

Source: `Assets/StreamingAssets/Data/narrative_arc_events.json`, parsed on
2026-09-09. There are 15 events, 18 choices, and 17 effect records.

## Events

| Event ID | Title | Weight | Min day | Choices |
|---|---|---:|---:|---:|
| `narrative_aris_thorne_stage_1` | The Cracked Floor | 3.0 | 15 | 1 |
| `narrative_aris_thorne_stage_2` | The Overwork | 1.0 | 18 | 2 |
| `narrative_aris_thorne_stage_3` | The Resolution | 1.0 | 25 | 0 |
| `narrative_maya_lin_stage_1` | The Signal | 3.0 | 12 | 1 |
| `narrative_maya_lin_stage_2` | The Death Loop | 1.0 | 18 | 2 |
| `narrative_maya_lin_stage_3` | The Broadcast | 1.0 | 30 | 0 |
| `narrative_victor_vance_stage_1` | The Refugee Mass | 3.0 | 14 | 1 |
| `narrative_victor_vance_stage_2` | The Command Decision | 1.0 | 20 | 2 |
| `narrative_victor_vance_stage_3` | The Treaty | 1.0 | 35 | 0 |
| `narrative_elena_rostov_stage_1` | The ARS Diagnosis | 3.0 | 16 | 1 |
| `narrative_elena_rostov_stage_2` | The Triage | 1.0 | 22 | 2 |
| `narrative_elena_rostov_stage_3` | The Remedy | 1.0 | 35 | 0 |
| `narrative_garrison_defector_intel` | The Defector's Story | 1.5 | 10 | 2 |
| `narrative_cult_prophet_rumor` | The Prophet Sighting | 1.0 | 20 | 2 |
| `narrative_militia_council_invitation` | The Council Invitation | 1.5 | 15 | 2 |

## Choices and effects

| Event | Choice ID | Morale | Effect(s) |
|---|---|---:|---|
| Aris 1 | `investigate_crack` | 0 | `advance_narrative_arc(survivorId=aris_thorne)` |
| Aris 2 | `force_rest_branch_a` | +5 | `narrative_arc_branch(survivorId=aris_thorne, branchId=a)` |
| Aris 2 | `let_continue_branch_b` | -10 | `narrative_arc_branch(survivorId=aris_thorne, branchId=b)` |
| Maya 1 | `trace_signal` | +5 | `advance_narrative_arc(survivorId=maya_lin)` |
| Maya 2 | `channel_grief_branch_a` | +10 | `narrative_arc_branch(survivorId=maya_lin, branchId=a)` |
| Maya 2 | `give_space_branch_b` | -15 | `narrative_arc_branch(survivorId=maya_lin, branchId=b)` |
| Victor 1 | `assess_situation` | 0 | `advance_narrative_arc(survivorId=victor_vance)` |
| Victor 2 | `admit_refugees_branch_a` | +15 | `narrative_arc_branch(survivorId=victor_vance, branchId=a)` |
| Victor 2 | `turn_away_branch_b` | -25 | `narrative_arc_branch(survivorId=victor_vance, branchId=b)` |
| Elena 1 | `review_treatment` | 0 | `advance_narrative_arc(survivorId=elena_rostov)` |
| Elena 2 | `save_patient_branch_a` | +10 | `narrative_arc_branch(survivorId=elena_rostov, branchId=a)` |
| Elena 2 | `let_pass_branch_b` | -15 | `narrative_arc_branch(survivorId=elena_rostov, branchId=b)` |
| Garrison | `grant_asylum_intel` | +5 | `gain_faction_intel(factionId=iron_garrison)` |
| Garrison | `take_intel_turn_away` | -10 | `gain_faction_intel(factionId=iron_garrison)` |
| Cult | `dismiss_rumor` | 0 | none |
| Cult | `investigate_rumor` | -5 | `start_expedition(locationId=loc_missile_silo)` |
| Militia | `attend_council` | +10 | `faction_standing(factionId=ash_militia, delta=20)` |
| Militia | `decline_council` | -5 | `faction_standing(factionId=ash_militia, delta=-5)` |

## Vocabulary and references

- Effect types: `advance_narrative_arc`, `narrative_arc_branch`,
  `gain_faction_intel`, `start_expedition`, `faction_standing`.
- Payload keys: `type`, `survivorId`; `type`, `survivorId`, `branchId`;
  `type`, `factionId`; `type`, `locationId`; and `type`, `factionId`,
  `delta`, respectively.
- Survivor IDs: `aris_thorne`, `maya_lin`, `victor_vance`,
  `elena_rostov`.
- Authored faction IDs: `iron_garrison`, `ash_militia`.
- Authored location ID: `loc_missile_silo`.
- Branch IDs: `a`, `b`.
- Morale distribution: `-25` once, `-15` twice, `-10` twice, `-5` twice,
  `0` four times, `+5` three times, `+10` three times, and `+15` once.
