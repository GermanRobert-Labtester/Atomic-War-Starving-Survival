# Plan 143 save compatibility

Plan 143 reuses the existing `narrative` campaign section. The only new field
is a nullable `arcState` child of `NarrativeEncounterState`; downstream
systems retain their own save stores. Pre-Plan-143 files may omit the field.
New empty arc states serialize it as `null` to preserve the public-field wire
shape across the Core and Unity-compatible serializers.

## State retained

- pending event ID and the day on which it was selected;
- completed Plan 143 event IDs;
- four bounded arc progress records (current stage, branch token, and terminal
  completion);
- one-shot expedition offer locations;
- committed arc choice records needed for duplicate protection and audit.

## Migration rules

- A pre-Plan-143 narrative save without `arcState` restores an empty arc state.
  It does not replay missed events or apply retroactive morale, intel,
  standing, expedition, or journal effects.
- A late save at day 40 with no arc history adopts only a valid stage 1 when a
  future daily selection occurs and the required survivor is present. It does
  not grant earlier stages automatically.
- A catalog reorder does not change an already persisted pending event or
  completed event IDs. New draws sort IDs before using the named RNG stream.
- A missing optional catalog leaves the arc system inert and reports a warning;
  the rest of the campaign loads normally.
- A choice whose effect adapter is unavailable remains non-executable and is
  never marked complete.
- Restore reconstructs ledgers and pending presentation only. It never calls a
  consequence adapter or re-emits a journal/feedback consequence.
