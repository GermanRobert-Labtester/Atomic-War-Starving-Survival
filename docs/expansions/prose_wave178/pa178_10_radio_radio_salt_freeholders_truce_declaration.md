# Subject Plan 178.10 — Deep Salt Freeholders Council: focused broadcast prose review

Lane: A · Subject family: authored radio message prose · Status: PROPOSAL (one-record editorial subject; not an implementation or integration plan)

## Subject and editorial brief

Review only the exact selector `radio_salt_freeholders_truce_declaration` in `Assets/StreamingAssets/Data/year_of_ash_radio.json`. Its current `message` contains 39 words / 257 characters. This is not a defect finding, a quota, or a target length. Preserve it unchanged when a revision would merely make it longer.

Editorial question: **How can this sender make the information more legible without exceeding what this channel can know or promise?** Treat this as a particular authored record, not a slot that needs to be filled to a word count. The result should sound native to its source, carry only information justified by the current record and canon, and avoid repeating imagery or cadence used in neighboring entries.

## Live-source evidence

- **VERIFIED:** exact selector `radio_salt_freeholders_truce_declaration` exists once in current `year_of_ash_radio.json` (`broadcasts`, 50 records), with fields `dayTrigger, frequency, id, isEmergency, message, signalStrength, source`.
- **VERIFIED:** `message` is the sole prose field proposed for review; current value is 257 characters / 39 whitespace-delimited words.
- **VERIFIED:** current authoring metadata is frozen as `{"dayTrigger": 282, "frequency": "104.200 MHz", "id": "radio_salt_freeholders_truce_declaration", "isEmergency": false, "signalStrength": "S7", "source": "Deep Salt Freeholders Council"}`.
- **VERIFIED:** this ID is absent from the current expansion-plan indexes searched for this batch.

**Current source prose:**

> The Deep Salt Chambers declare absolute territorial neutrality. The hospital cavern is open to wounded soldiers, militia, and refugees alike, provided all firearms are surrendered at the hoist cage. Any unit firing into the shaft will face dynamite sealing.

## Record-specific treatment

Keep the station/source identity, frequency, day timing, emergency status, signal-strength claim, and any audio-cue association intact. Improve intelligibility through a speaker’s operating priorities, concrete transmission evidence, and the limits of what the speaker can know. Preserve protocol texture where the source is procedural; preserve human strain without turning every broadcast into a monologue. A broadcast may be clipped or formal when that fits its sender.

Use the selector’s surrounding facts rather than importing a second story. Check how its current wording positions the speaker or observer, what evidence the record actually provides, and what the player is meant to infer. Keep the diction concrete and economical. Avoid decorative catastrophe, generic “the world has changed” endings, unexplained new names, omniscient knowledge, and exposition that belongs in a separate lore document.

If the present copy already does this work, the editorial disposition is **retain**. A no-op is a valid completion for a future prose pass. Do not compensate for a weak premise by inventing backstory, mechanics, or an outcome.

## Immutable data contract

Only top-level `message` may change. Re-read the complete live object immediately before any edit. Preserve this exact snapshot of every other field:

```json
{
  "dayTrigger": 282,
  "frequency": "104.200 MHz",
  "id": "radio_salt_freeholders_truce_declaration",
  "isEmergency": false,
  "signalStrength": "S7",
  "source": "Deep Salt Freeholders Council"
}
```

Do not add/remove records or keys; change selector, array order, day gate, weights, thresholds, flags, nested choices or conditions, audio metadata, delivery state, save behavior, UI, code, or any other data. No new route is authorized by this proposal. Prose may describe only the state and knowledge already supported by the current record; it must not claim that an unselected branch has happened.

## Route and promotion gate

The current `RadioBroadcastTerminal.LoadBroadcasts` calls `YearOfAshCatalogLoader.LoadRadioBroadcasts`; `RefreshView(currentDay)` renders each record when `currentDay >= dayTrigger`, with source, frequency, emergency marker, and message. This is a proven display seam for the record family. Confirm the owning live UI and current-day refresh call before editing this exact selector; the class-level route alone does not prove every record is instantiated in a normal play session.

**Conditional scope:** if the exact display path and current file ownership are confirmed, the bounded implementation would be a single top-level `message` edit in this one catalog record. A body-only change has no save-schema or deterministic-state impact. If reaching the content requires changing a consumer, adding a field, altering timing, or authoring a new mechanic, stop and request a separate, current-evidence plan through the active foreman.

## Canon and duplicate review

1. Read the relevant current ASHFALL lore and neighboring records before editing. The compiled expansion authority is a subject source, not a substitute for current data or runtime evidence.
2. Search for matching phrases, scene images, speakers, and causal claims in adjacent records and prior prose plans. An unused exact ID does not prove the idea is original.
3. Keep this catalog’s identity and channel conventions intact. For radio, distinguish sourced fact, instruction, warning, and rumor; for events, preserve what is observed versus inferred.
4. Do not borrow real-world wars, countries, people, or copied text. Keep the setting fictional and the tone restrained.
5. At promotion, refresh `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `KNOWN_DEBT.md` under the current coordination process. This proposal makes no path claim and does not authorize a 50-record implementation wave.

## Future verification

Strict-parse the complete source file. Compare before/after structures and prove that only this selector’s `message` changed. Assert selector uniqueness, unchanged order, all frozen values, and the catalog’s current integrity rules. Review prose for factual scope, tone, repetition, continuity, clarity, and accidental branch promises. For a radio record, additionally check that `dayTrigger`, source, frequency, emergency marker, signal strength, and `audio_cue` remain unchanged; for an event, inspect every condition and choice payload in full. Use only focused verification appropriate to that one-record data edit.

## Acceptance and disposition

Accept a proposed revision only if it improves comprehension, specificity, voice, or diegetic function while respecting this contract. The current text may be retained. This document records a candidate editorial subject, not a runtime-reachability certification, integration approval, owner claim, or permission to edit the source catalog.
