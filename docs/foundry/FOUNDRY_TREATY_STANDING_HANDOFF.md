# Foundry Treaty Standing Handoff

**Owner:** `SilentFoundryConsequenceState.guildStanding`

**Mirror:** `FactionStanceEngine` through `SilentFoundryHostSession`

`standing_delta` is the only standing effect supported by the live policy
schema. Core clamps accumulated Foundry standing to `[-100, 100]`, records the
delta in `FoundryConsequenceRecord`, and the host mirrors that delta into the
existing stance engine. No second faction-standing store is introduced.

## Plan 103 deltas

| Outcome | Standing deltas |
|---|---|
| `met` | +2 brine/labour/Incident Book; +3 road/Saltworks/Coal; +4 Membrane/Crisis |
| `missed` | -6 brine/road; -5 Coal Window |
| `violated` | -8 labour; -10 Saltworks; -12 Membrane; -14 Crisis |

The ranges preserve the existing scale: a missed delivery is smaller than an
active violation, and even the strongest new row remains far from the -50
hostile-raid threshold by itself. Repeated cycles can matter, but the clamp
and existing stance thresholds remain authoritative.

The new rows are data-ready. Only the original quota/labour assessor currently
invokes the application path; no new standing trigger was added in Plan 103.
