# Foundry Treaty War Handoff

Plan 103 does not dispatch faction-war escalation. The current consequence
runtime records standing and market effects only; there is no live
`FactionWarSystem.RecordTreatyBreach` hook in the policy path.

`violated` rows are therefore bounded diplomatic evidence, not automatic war:

- Saltworks: -10 Foundry standing;
- Membrane Repair: -12;
- Crisis Mutual Aid: -14.

The existing stance authority may move through its own thresholds after a
future live assessment, but war, raid, and reconciliation remain downstream
systems. No breach row creates a new war flag, attack schedule, or escalation
engine.
