# Foundry Treaty Consequence Save Contract

Plan 103 adds static policy rows only. It does not add a save section or
change the existing `ExpansionHubSave` schema.

The existing durable ledger stores:

- `treatyId`;
- canonical enum outcome (`Met`, `Missed`, or `Violated`);
- `appliedDay` and `cycleMarker`;
- the applied `standingDelta`;
- copied market modifiers and authored reason.

`SilentFoundryConsequenceState.IsApplied(treatyId, cycleMarker)` makes the
application one-shot for an assessment day. Save/load restores the ledger and
does not reapply the standing or market change. Adding a policy cannot
retroactively apply it to a previously resolved treaty because the catalog is
looked up only from the live assessment path.

Old saves remain compatible: the existing six policy IDs and ledger shape are
unchanged, while the nine new IDs become available to future typed triggers.
Checksum and deterministic serializer behavior remain owned by the existing
save envelope.
