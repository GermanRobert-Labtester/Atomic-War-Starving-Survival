# Plan 143 effect contract matrix

Every effect in the current catalog is classified before runtime activation.
The runtime accepts typed effect records only. It does not evaluate arbitrary
JSON strings or invoke methods by name.

| Authored verb | Disposition | Authority | Preflight | Commit / persistence |
|---|---|---|---|---|
| `advance_narrative_arc` | Activate | `NarrativeArcEventSystem` bounded arc graph | Subject matches the known arc, stage 1 is current, and the survivor is alive and resident | Advance the arc to stage 2; the event resolution is persisted once |
| `narrative_arc_branch` | Activate | `NarrativeArcEventSystem` bounded arc graph | Known survivor, current stage 2, branch is `a` or `b`, and no branch was committed | Persist one mutually exclusive branch token and advance to stage 3 |
| `gain_faction_intel` | Activate as knowledge discovery | `JournalSystem.Knowledge` through a canonical faction-intel key | Faction alias resolves to a known faction and the journal authority is bound | Discover `faction_intel_faction_central_garrison` once; no numeric hidden intel score |
| `start_expedition` | Activate as an offer | Existing expedition catalog and normal expedition panel | `loc_missile_silo` resolves in the expedition catalog; no party, supply, weather, route, or vehicle gate is bypassed | Persist a bounded offer token. The player dispatches later through normal expedition validation |
| `faction_standing` | Activate | `FactionWarSystem` in `YearOfAshHostSession` | Canonical faction resolves and delta is finite, integral, and in the standing mutation range | Call `ModifyStanding` once; the faction system owns the persisted standing |

Morale is a choice field rather than an effect verb. For a character arc it is
applied to that addressed resident through `NeedsSystem`. For an independent
event it is applied to the living resident cohort through the same authority.
There is exactly one morale owner.

## Closed failure behavior

An unknown effect type, malformed payload, unsupported adapter, missing
authority, invalid reference, invalid stage, or failed preflight makes the
choice non-executable. It reports a diagnostic and leaves all state unchanged.
The choice is never marked complete merely because its text was displayed.
