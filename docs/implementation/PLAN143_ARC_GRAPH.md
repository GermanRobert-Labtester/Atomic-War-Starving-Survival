# Plan 143 arc graph

The four character arcs are explicit bounded graphs. The graph is selected by
the verified authored IDs and survivor references, not by executing filename
or dictionary rules.

| Arc | Stage 1 | Stage 2 | Stage 3 | Branches |
|---|---|---|---|---|
| Aris Thorne | `narrative_aris_thorne_stage_1`, day 15 | `narrative_aris_thorne_stage_2`, day 18 | `narrative_aris_thorne_stage_3`, day 25 | `a`, `b` |
| Maya Lin | `narrative_maya_lin_stage_1`, day 12 | `narrative_maya_lin_stage_2`, day 18 | `narrative_maya_lin_stage_3`, day 30 | `a`, `b` |
| Victor Vance | `narrative_victor_vance_stage_1`, day 14 | `narrative_victor_vance_stage_2`, day 20 | `narrative_victor_vance_stage_3`, day 35 | `a`, `b` |
| Elena Rostov | `narrative_elena_rostov_stage_1`, day 16 | `narrative_elena_rostov_stage_2`, day 22 | `narrative_elena_rostov_stage_3`, day 35 | `a`, `b` |

Each stage requires the named survivor to be present, alive, and in the shelter
interior when selected or resolved. Stage 2 is eligible only after stage 1's
choice commits. Stage 3 is eligible only after stage 2 commits a branch. The
terminal stage has zero choices and is acknowledged once; acknowledgement
completes the arc without replaying earlier effects.

The three independent events are one-shot events with no arc prerequisite:

- `narrative_garrison_defector_intel`, day 10
- `narrative_cult_prophet_rumor`, day 20
- `narrative_militia_council_invitation`, day 15

Completed events never re-enter the candidate pool. The selection list is
sorted by ordinal event ID before the named narrative RNG stream rolls. A
pending event is persisted and is presented again after load; selecting a
candidate never mutates a downstream authority.
