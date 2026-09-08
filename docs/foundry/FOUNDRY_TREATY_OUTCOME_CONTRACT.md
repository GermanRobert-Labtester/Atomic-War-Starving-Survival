# Foundry Treaty Outcome Contract

**Runtime source:** `Assets/Ashfall.Core/Foundry/SilentFoundryConsequencePolicy.cs`

The closed policy vocabulary is exactly:

| JSON value | Enum | Meaning |
|---|---|---|
| `met` | `FoundryTreatyOutcome.Met` | obligation fulfilled |
| `missed` | `FoundryTreatyOutcome.Missed` | recoverable shortfall |
| `violated` | `FoundryTreatyOutcome.Violated` | active breach/refusal |

`NotRatified` and `Pending` are neutral enum states and have no policy rows.
The plan's word “breached” is documentation shorthand only; writing
`"breached"` to JSON fails the live catalog validator.

The current assessor distinguishes missed quota/delivery from active labour
violation. The nine new rows preserve that distinction in authored data, but
the new Plan 102 contracts do not yet have typed assessment triggers.

## One-shot lifecycle

`ApplyConsequence` rejects neutral outcomes, looks up `(treaty_id, outcome)`,
and checks `IsApplied(treatyId, assessmentDay)` before changing standing or
copying market modifiers. Reassessment on the same day and save/reload do not
apply the row twice.
