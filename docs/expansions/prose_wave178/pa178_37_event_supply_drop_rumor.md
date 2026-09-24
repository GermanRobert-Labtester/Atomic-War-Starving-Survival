# Subject Plan 178.37 — Supply Drop Rumor: focused event prose review

Lane: A · Subject family: authored event prose · Status: PROPOSAL (one-record editorial subject; not an implementation or integration plan)

## Subject and editorial brief

Review only the exact selector `supply_drop_rumor` in `Assets/StreamingAssets/Data/events.json`. Its current `bodyText` contains 128 words / 714 characters. This is not a defect finding, a quota, or a target length. Preserve it unchanged when a revision would merely make it longer.

Editorial question: **What single grounded detail would make the existing scene easier to picture while leaving its unresolved facts intact?** Treat this as a particular authored record, not a slot that needs to be filled to a word count. The result should sound native to its source, carry only information justified by the current record and canon, and avoid repeating imagery or cadence used in neighboring entries.

## Live-source evidence

- **VERIFIED:** exact selector `supply_drop_rumor` exists once in current `events.json` (`events`, 240 records), with fields `bodyText, id, minDay, title, weight`.
- **VERIFIED:** `bodyText` is the sole prose field proposed for review; current value is 714 characters / 128 whitespace-delimited words.
- **VERIFIED:** current non-prose snapshot is `{"id": "supply_drop_rumor", "minDay": 10, "title": "Supply Drop Rumor", "weight": 1.0}`.
- **VERIFIED:** this ID is absent from the current expansion-plan indexes searched for this batch.

**Current source prose:**

> A crackling radio message repeats itself on the old military frequency: supply drop, three miles north, grid marker on the burnt-out water tower. The voice is flat, recorded, possibly automated. It has been looping for two days. Three miles north is the flats, open ground with no cover except the dry riverbed, and the water tower has been a landmark for raiders since the first winter. The drop could be real: the broadcasts started the same week the garrison pulled back. It could also be a bell rung over a snare. You have one team fit enough to make the walk, and the walk itself costs a day of water. Somebody wants people to come north. The question is whether they are handing out food or collecting packs.

## Record-specific treatment

Preserve the scene’s asserted facts, viewpoint, chronology, and level of certainty. Add a detail only when it makes an existing person, physical trace, decision, or immediate consequence clearer. Distinguish witnessed evidence from inference; keep rumor, suspicion, and unresolved causality unresolved. Do not add a new decision or imply any unrecorded resource, status, relationship, faction, hazard, or world-flag change.

Use the selector’s surrounding facts rather than importing a second story. Check how its current wording positions the speaker or observer, what evidence the record actually provides, and what the player is meant to infer. Keep the diction concrete and economical. Avoid decorative catastrophe, generic “the world has changed” endings, unexplained new names, omniscient knowledge, and exposition that belongs in a separate lore document.

If the present copy already does this work, the editorial disposition is **retain**. A no-op is a valid completion for a future prose pass. Do not compensate for a weak premise by inventing backstory, mechanics, or an outcome.

## Immutable data contract

Only top-level `bodyText` may change. Re-read the complete live object immediately before any edit. Preserve this exact snapshot of every other field:

```json
{
  "id": "supply_drop_rumor",
  "minDay": 10,
  "title": "Supply Drop Rumor",
  "weight": 1.0
}
```

Do not add/remove records or keys; change selector, array order, day gate, weights, thresholds, flags, nested choices or conditions, audio metadata, delivery state, save behavior, UI, code, or any other data. No new route is authorized by this proposal. Prose may describe only the state and knowledge already supported by the current record; it must not claim that an unselected branch has happened.

## Route and promotion gate

`EventsHostSession` loads the ID-addressed `events.json` catalog and exposes `TryGetEvent(eventId, out EventData)`. This confirms an existing load/lookup seam, not this selector’s player-facing dispatch. Trace the exact ID through current callers to an observable presentation before any promotion; if none exists, keep this as a DOCS-ONLY editorial subject.

**Conditional scope:** if the exact display path and current file ownership are confirmed, the bounded implementation would be a single top-level `bodyText` edit in this one catalog record. A body-only change has no save-schema or deterministic-state impact. If reaching the content requires changing a consumer, adding a field, altering timing, or authoring a new mechanic, stop and request a separate, current-evidence plan through the active foreman.

## Canon and duplicate review

1. Read the relevant current ASHFALL lore and neighboring records before editing. The compiled expansion authority is a subject source, not a substitute for current data or runtime evidence.
2. Search for matching phrases, scene images, speakers, and causal claims in adjacent records and prior prose plans. An unused exact ID does not prove the idea is original.
3. Keep this catalog’s identity and channel conventions intact. For radio, distinguish sourced fact, instruction, warning, and rumor; for events, preserve what is observed versus inferred.
4. Do not borrow real-world wars, countries, people, or copied text. Keep the setting fictional and the tone restrained.
5. At promotion, refresh `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `KNOWN_DEBT.md` under the current coordination process. This proposal makes no path claim and does not authorize a 50-record implementation wave.

## Future verification

Strict-parse the complete source file. Compare before/after structures and prove that only this selector’s `bodyText` changed. Assert selector uniqueness, unchanged order, all frozen values, and the catalog’s current integrity rules. Review prose for factual scope, tone, repetition, continuity, clarity, and accidental branch promises. For a radio record, additionally check that `dayTrigger`, source, frequency, emergency marker, signal strength, and `audio_cue` remain unchanged; for an event, inspect every condition and choice payload in full. Use only focused verification appropriate to that one-record data edit.

## Acceptance and disposition

Accept a proposed revision only if it improves comprehension, specificity, voice, or diegetic function while respecting this contract. The current text may be retained. This document records a candidate editorial subject, not a runtime-reachability certification, integration approval, owner claim, or permission to edit the source catalog.
