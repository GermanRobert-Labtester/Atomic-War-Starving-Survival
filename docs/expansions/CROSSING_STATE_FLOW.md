# Crossing State Flow

## Runtime ownership

```text
CrossingQuestSystem
  ├─ local quest progress, flags, and narrative dispatch keys
  ├─ VouchAccessSystem opening-gate authority
  └─ optional IFlagLedger projection into the campaign consequence ledger
```

Crossing quest state remains in the `ExpansionHubSave` section. The campaign
consequence ledger is not duplicated into that section. When the host binds the
existing ledger, a newly selected Crossing choice records its authored
`set_flag` there with `CrossingQuestSystem` as the origin.

## Choice contract

1. A choice is valid only for a started, active quest.
2. The first selected choice is authoritative.
3. Repeating the same choice is a no-op success for UI retry safety.
4. Selecting a different choice after selection is refused.
5. A choice with an empty `set_flag` changes only the local chosen-choice
   record and emits no flag event.

## Save contract

`CaptureState` already copies the runtime collections. `RestoreState` now
copies every quest progress object and both flag/event-key sets as well.
Mutating a decoded save object after restore cannot mutate the running quest
system or replay a narrative event.

## Integration boundary

The host passes the main campaign `IFlagLedger` into
`ExpansionHostSession.Create`. Crossing does not instantiate a moral-choice
system, a Thirdonary system, or a second flag authority. Those systems can
consume the canonical ledger through their existing host wiring.

## Remaining work

Typed moral deltas and Thirdonary quest triggers require authored fields and
explicit consequence mappings. They are intentionally not inferred from
free-form flag names in this phase.
