# Plan 127 Verdict Data Expansion Closeout

**Status:** DATA COMPLETE / RUNTIME REACHABILITY DEFERRED
**Authority:** `Assets/StreamingAssets/Data/verdict_data.json`

## Delivered

- Expanded `corruption_corpus` from 8 to 25 entries.
- Preserved all original corpus text and order.
- Added 17 distinct degraded-machine failure modes.
- Expanded `world_history_ladder` from 6 to 12 entries.
- Preserved layers 1–6 and appended layers 7–12 in ascending order.
- Reused committed location IDs only.
- Added six unique `lore_verdict_` journal keys.
- Documented three Plan 113-aligned site references and two Plan 116
  location references.
- Added the runtime/reference contract and inventories in this directory.

## Scope boundary

This remains pure data and documentation. No Core code, Godot host code,
save field, parser, EvidenceLedger API, quest catalog, or new test was added.

The current runtime loads all 12 ladder rows for journal/codex presentation,
but only `Main.UnlockVerdictLore`'s original six keys are fired by Verdict
progress. The new rows therefore pass catalog/reference validation and are
available to a future producer, but are not represented as a reachable
12-step persisted ladder today. Dynamic progression is a follow-up task
outside Plan 127's authorized scope.

## Definition-of-done disposition

| Requirement | Result |
|---|---|
| 25 corruption strings | PASS |
| 12 ascending unique ladder layers | PASS |
| Existing corpus/layers preserved | PASS |
| Knowledge-key references resolve as journal rows | PASS |
| Location references resolve | PASS |
| Plan 113/116 references use committed content | PASS; authored alignment, not runtime link wiring |
| Dynamic unlock through new layers | DEFERRED; requires runtime work |
| New C# code/tests/save schema | NOT ADDED |

## Verification

The exact commands and results are recorded in
`docs/plans/PLAN127_IMPLEMENTATION_LOG.md`.
